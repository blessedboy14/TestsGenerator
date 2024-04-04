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
            var namespaces = root.ChildNodes().Where(nsp => nsp is NamespaceDeclarationSyntax);
            foreach (NamespaceDeclarationSyntax nSpaceDeclaration in namespaces)
            {
                generateSingleTestFile(nSpaceDeclaration, root, tests);
            }
            return [.. tests];
        }

        private static string FullNamespace(SyntaxNode cls, bool forClass = false)
        {
            StringBuilder fullNamespace = new StringBuilder();
            while(!(cls.Parent is CompilationUnitSyntax))
            {
                if (cls.Parent is NamespaceDeclarationSyntax)
                {
                    fullNamespace.Insert(0, '.' +
                        ((cls.Parent as NamespaceDeclarationSyntax).Name as IdentifierNameSyntax).Identifier.Text );
                }
                if (forClass && cls.Parent is ClassDeclarationSyntax)
                {
                    fullNamespace.Insert(0, '.' + 
                        (cls.Parent as ClassDeclarationSyntax).Identifier.Text + '_');
                }
                cls = cls.Parent;
            }
            return fullNamespace.Remove(0, 1).ToString();
        }

        private static NamespaceDeclarationSyntax declareClass(string nSpaceName, ClassDeclarationSyntax fromCls)
        {
            var nSpace = SyntaxFactory.NamespaceDeclaration(
                        SyntaxFactory.ParseName(FullNamespace(fromCls, true) + "Test")
                        );
            var testClass = SyntaxFactory.ClassDeclaration(fromCls.Identifier.Text + "Tests")
                .WithModifiers(
                    SyntaxFactory.TokenList(
                        SyntaxFactory.Token(SyntaxKind.PublicKeyword)));
                   
            return nSpace;
        }

        private static void generateSingleTestFile(SyntaxNode nSpaceNode, CompilationUnitSyntax root, 
            List<TestWrapper> tests)
        {
            var classes = nSpaceNode.ChildNodes().Where(cls => cls is ClassDeclarationSyntax);
            foreach (ClassDeclarationSyntax cls in classes)
            {
                var members = cls.Members;
                var methods = members.Where(method => method is MethodDeclarationSyntax &&
                method.Modifiers.Where(modifier => modifier.IsKind(SyntaxKind.PublicKeyword)).Any());
                if (methods.Count() > 0)
                {
                    var imports = root.Usings;
                    var syntaxFactory = SyntaxFactory.CompilationUnit();
                    syntaxFactory = syntaxFactory.AddUsings(imports.ToArray());
                    syntaxFactory = syntaxFactory.AddUsings(new UsingDirectiveSyntax[]{
                        SyntaxFactory.UsingDirective(SyntaxFactory.ParseName("NUnit.Framework")),
                        SyntaxFactory.UsingDirective(SyntaxFactory.ParseName(FullNamespace(cls)))
                        });
                    var nSpaceName = SyntaxFactory.NamespaceDeclaration(
                        SyntaxFactory.ParseName(FullNamespace(cls, true) + "Test")
                        );



                }
                foreach (var item in members)
                {
                    if (item is ClassDeclarationSyntax)
                    {
                        generateSingleTestFile(cls, root, tests);
                    }
                }
            }    
        }
    }
}
