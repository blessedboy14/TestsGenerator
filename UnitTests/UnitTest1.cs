using NUnit.Framework.Legacy;
using TestsGenerator;

namespace UnitTests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TestGeneration_WithNamespaceDeclaration()
        {
            string interfaceDeclaration = "namespace MyNamespace\r\n{\r\n    public interface Test\r\n    {\r\n        void firstMethod();\r\n\r\n        int secondMethod();\r\n    }\r\n}\r\n";
            TestsGenerator.TestWrapper[] tests = TestsGenerator.TestsGenerator.GenerateTests(
                    interfaceDeclaration
                );
            ClassicAssert.AreEqual(0, tests.Length);
        }

        [Test]
        public async Task TestGenerationClassEquals_UsingExistingTestAsync()
        {
             TestsGenerator.TestWrapper[] tests = TestsGenerator.TestsGenerator.GenerateTests(
                    await FileReader.ReadTextAsync("D:\\LABS\\SPP\\Tracer\\Tracer\\Tracing\\CustomTracer.cs")
                );
            string expected = "using NUnit.Framework;\r\nusing Tracing;\r\n\r\nnamespace Tracing.Test\r\n{\r\n    [TestFixture]\r\n    public class CustomTracerTests\r\n    {\r\n        [Test]\r\n        public  void  GetTraceResultTest()\r\n        {\r\n            Assert.Fail(\"autogenerated\");\r\n        }\r\n\r\n        [Test]\r\n        public  void  StartTraceTest()\r\n        {\r\n            Assert.Fail(\"autogenerated\");\r\n        }\r\n\r\n        [Test]\r\n        public  void  StopTraceTest()\r\n        {\r\n            Assert.Fail(\"autogenerated\");\r\n        }\r\n    }\r\n}";
            ClassicAssert.AreEqual(tests[0].sourceCode, expected);
        }

        [Test]
        public async Task TestNumberOfGeneratedFiles_PlusFileName()
        {
            string[] inputFiles = [
                await FileReader.ReadTextAsync("D:\\LABS\\SPP\\Tracer\\Tracer\\Tracing\\CustomTracer.cs"),
                await FileReader.ReadTextAsync("D:\\LABS\\SPP\\TestsGenerator\\TestsGenerator\\TestsGenerator.cs"),
            ];
            List<TestWrapper> allTests = new List<TestWrapper>();
            foreach ( string file in inputFiles )
            {
                TestsGenerator.TestWrapper[] tests = TestsGenerator.TestsGenerator.GenerateTests(
                    file
                );
                allTests.AddRange( tests );
            }
            ClassicAssert.AreEqual(3, allTests.Count);
            ClassicAssert.AreEqual(allTests.Where(test => (test.fileName == "Tracing_CustomTracer_Test.cs"
            || test.fileName == "TestsGenerator_TestsGenerator_Test.cs")).ToList().Count, 2);
        }

        [Test]
        public async Task TestInnerNamespace_PlusMultipleClasses()
        {
            string input = "namespace UnitTests\r\n{\r\n    namespace InnerSpace\r\n    {\r\n        public class BaseClass()\r\n        {\r\n            public void doJob(string input)\r\n            {\r\n\r\n            }\r\n        }\r\n    }\r\n    class OuterClass\r\n    {\r\n        public void doJob(int input)\r\n        {\r\n\r\n        }\r\n    }\r\n}";
            TestsGenerator.TestWrapper[] tests = TestsGenerator.TestsGenerator.GenerateTests(
                    input
                );
            ClassicAssert.AreEqual(tests.Length, 2);
            ClassicAssert.IsTrue(tests.Where(test => test.fileName == "UnitTests.InnerSpace_BaseClass_Test.cs").Count() > 0);
            ClassicAssert.IsTrue(tests.Where(test => test.fileName == "UnitTests_OuterClass_Test.cs").Count() > 0);
        }
    }
}