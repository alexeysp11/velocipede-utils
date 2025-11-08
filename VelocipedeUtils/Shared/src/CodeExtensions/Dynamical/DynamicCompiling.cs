using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;
using Microsoft.CodeAnalysis.Text;

namespace VelocipedeUtils.Shared.CodeExtensions.Dynamical;

/// <summary>
/// Class for dynamically compiling a string containing a C# code
/// </summary>
public static class DynamicCompiling 
{
    /// <summary>
    /// Method for compiling a string containing C# code and run it
    /// </summary>
    public static string CompileAndRunCSharpString(string code, string assemblyName, string instanceName)
    {
        if (string.IsNullOrEmpty(code))
            throw new Exception("Code could not be null or empty");
        if (string.IsNullOrEmpty(assemblyName))
            throw new Exception("Assembly name could not be null or empty");

        (byte[] a, byte[] b) = CreateAssembly(code, assemblyName, instanceName);
        var assembly = Assembly.Load(a, b);

        dynamic instance = assembly.CreateInstance(instanceName);
        string result = instance.DoWork();
        return result;
    }

    /// <summary>
    /// Creates assembly
    /// </summary>
    private static (byte[], byte[]) CreateAssembly(string code, string assemblyName, string instanceName)
    {
        var encoding = Encoding.UTF8;
        var symbolsName = Path.ChangeExtension(assemblyName, "pdb");

        var references = new MetadataReference[]
        {
            MetadataReference.CreateFromFile(typeof(object).GetTypeInfo().Assembly.Location),
            MetadataReference.CreateFromFile(Assembly.Load(new AssemblyName("Microsoft.CSharp")).Location),
            MetadataReference.CreateFromFile(Assembly.Load(new AssemblyName("System")).Location),
            MetadataReference.CreateFromFile(Assembly.Load(new AssemblyName("System.Runtime")).Location),
            MetadataReference.CreateFromFile(Assembly.Load(new AssemblyName("System.Collections")).Location),
            MetadataReference.CreateFromFile(Assembly.Load(new AssemblyName("netstandard")).Location),
            MetadataReference.CreateFromFile(Assembly.Load(new AssemblyName("System.Linq.Expressions")).Location),
            MetadataReference.CreateFromFile(Assembly.Load(new AssemblyName("System.Private.CoreLib")).Location),
            MetadataReference.CreateFromFile(typeof(System.Linq.Expressions.ExpressionType).GetTypeInfo().Assembly.Location),
        };

        var syntaxTrees = new List<SyntaxTree>();
        var embeddedTexts = new List<EmbeddedText>();

        var sourceCodePath = instanceName + ".g.cs";
        var buffer = encoding.GetBytes(code);
        var sourceText = SourceText.From(buffer, buffer.Length, encoding, canBeEmbedded: true);

        var syntaxTree = CSharpSyntaxTree.ParseText(
            sourceText,
            new CSharpParseOptions().WithLanguageVersion(LanguageVersion.CSharp8),
            path: sourceCodePath);

        var syntaxRootNode = syntaxTree.GetRoot() as CSharpSyntaxNode;
        var encoded = CSharpSyntaxTree.Create(syntaxRootNode, null, sourceCodePath, encoding);

        syntaxTrees.Add(encoded);
        embeddedTexts.Add(EmbeddedText.FromSource(sourceCodePath, sourceText));

        CSharpCompilation compilation = CSharpCompilation.Create(
            assemblyName,
            syntaxTrees: syntaxTrees,
            references: references,
            options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary)
                .WithOptimizationLevel(OptimizationLevel.Debug)
                .WithPlatform(Platform.AnyCpu)
        );

        using (var assemblyStream = new MemoryStream())
        using (var symbolsStream = new MemoryStream())
        {
            var emitOptions = new EmitOptions(
                debugInformationFormat: DebugInformationFormat.PortablePdb,
                pdbFilePath: symbolsName);
            Console.WriteLine(emitOptions.PdbFilePath);

            EmitResult result = compilation.Emit(
                peStream: assemblyStream,
                pdbStream: symbolsStream,
                embeddedTexts: embeddedTexts,
                options: emitOptions);

            if (!result.Success)
            {
                var errors = new List<string>();

                IEnumerable<Diagnostic> failures = result.Diagnostics.Where(diagnostic =>
                    diagnostic.IsWarningAsError ||
                    diagnostic.Severity == DiagnosticSeverity.Error);

                foreach (Diagnostic diagnostic in failures)
                    errors.Add($"{diagnostic.Id}: {diagnostic.GetMessage()}");

                throw new Exception(string.Join("\n", errors));
            }
            return (assemblyStream.GetBuffer(), symbolsStream.GetBuffer());
        }
    }
}