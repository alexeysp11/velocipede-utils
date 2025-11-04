using Xunit;
using VelocipedeUtils.Dynamical;

namespace VelocipedeUtils.Shared.Tests.Dynamical
{
    public sealed class DynamicCompilingTest
    {
        [Fact]
        public void CompileAndRunCSharpString_CorrectParameters_InstanceCreated()
        {
            // Arrange 
            var code = @"
using System;
using System.Collections.Generic;

namespace Debuggable
{
    public sealed class HelloWorld
    {
        public string DoWork()
        {
            return ""Hello world"";
        }
    }
}
            ";
            string assemblyName = "TestCompiledAssembly";
            string instanceName = "Debuggable.HelloWorld";

            // Act 
            string result = DynamicCompiling.CompileAndRunCSharpString(code, assemblyName, instanceName);

            // Assert 
            Assert.True(result == "Hello world");
        }
    }
}