using Xunit;
using VelocipedeUtils.Dynamical;

namespace Cims.Tests.VelocipedeUtils.Dynamical
{
    public class DynamicCompilingTest
    {
        [Fact]
        public void CompileAndRunCSharpString_CorrectParameters_InstanceCreated()
        {
            // Arrange 
            DynamicCompiling compiler = new DynamicCompiling();
            var code = @"
using System;
using System.Collections.Generic;

namespace Debuggable
{
    public class HelloWorld
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
            string result = compiler.CompileAndRunCSharpString(code, assemblyName, instanceName);

            // Assert 
            Assert.True(result == "Hello world");
        }
    }
}