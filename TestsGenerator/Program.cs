namespace TestsGenerator
{
    public class Start
    {
        public static void Main(string[] args)
        {
            var tester = new TestGenerationMain();
            tester.Generate(new List<string>{ "D:\\LABS\\SPP\\TestsGenerator\\TestsGenerator\\TestClass.cs"}, "../", 8, 8, 8).Wait();
        }
    }
}
