using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestsGenerator
{

    class TestWrapper
    {
        public string fileName;
        public string sourceCode;
        public TestWrapper
            (string fileName, string sourceCode)
        {
            this.fileName = fileName;
            this.sourceCode = sourceCode;
        }
    }

    class TestsGenerator
    {
        public static TestWrapper[] GenerateTests(string inputSource)
        {
            return null;
        }
    }
}
