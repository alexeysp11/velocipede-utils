# DynamicCompiling

`DynamicCompiling` is class for **dynamically compiling** a string containing a C# code.

Namespace: [Cims.WorkflowLib.Dynamical](Cims.WorkflowLib.Dynamical.md)

## CompileAndRunCSharpString()

Example of using `DynamicCompiling.CompileAndRunCSharpString()`: 

```C#
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Xunit;
using Cims.WorkflowLib.Dynamical; 

namespace Cims.Tests.WorkflowLib.Dynamical
{
    public class DynamicCompilingTest
    {
        private string FolderName = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), typeof(DynamicCompiling).ToString().Split('.').Last()); 

        // 
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
```
