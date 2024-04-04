using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

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
            List<TestWrapper> tests = new List<TestWrapper>();
            SyntaxTree tree = CSharpSyntaxTree.ParseText(inputSource);
            CompilationUnitSyntax root = tree.GetCompilationUnitRoot();
            var classes = root.DescendantNodes(n => !(n is ClassDeclarationSyntax)).OfType <ClassDeclarationSyntax>();
            foreach ( ClassDeclarationSyntax classDeclaration in classes)
            {
                generateSingleTestFile(classDeclaration, root, tests);
            }
            return [.. tests];
        }

        private static string generateSingleTestFile(SyntaxNode classNode, CompilationUnitSyntax root, 
            List<TestWrapper> tests)
        {
            return "";
        }
    }
}
