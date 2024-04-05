using TestsGenerator;

namespace ConsoleApp
{
    class MainClass
    {
        static void Main(string[] args)
        {
            var basePath = "";
            var outputBase = "../../../../generated";
            Console.Write("Enter path to files( use , to divide): ");
            var input = Console.ReadLine().Trim().Split(',');
            Console.WriteLine();
            Console.Write("Write path to output directory: ");
            var dir = Console.ReadLine().Trim();
            Console.WriteLine();
            Console.Write("Enter task limit for reading: ");
            var readMax = int.Parse(Console.ReadLine());
            Console.WriteLine();
            Console.Write("Enter task limit for generating: ");
            var generateMax = int.Parse(Console.ReadLine());
            Console.WriteLine();
            Console.Write("Enter task limit for writing: ");
            var writeMax = int.Parse(Console.ReadLine());
            var generator = new TestGenerationMain();
            Console.WriteLine("Generation started");
            try
            {
                generator.Generate(input.ToList(), outputBase, readMax, generateMax, writeMax).Wait();
            } catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            Console.WriteLine("Generation ended");
        }
    }
}