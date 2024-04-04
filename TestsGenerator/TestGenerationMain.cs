using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace TestsGenerator
{
    class TestGenerationMain
    {
        public Task Generate(List<string> inputFiles, string outputDirectory, int maxLoad, int maxGenerate, int maxWrite)
        {
            //init options of parallelism
            var readOption = new ExecutionDataflowBlockOptions();
            readOption.MaxDegreeOfParallelism = maxLoad;
            var generateOption = new ExecutionDataflowBlockOptions();
            readOption.MaxDegreeOfParallelism = maxGenerate;
            var writeOption = new ExecutionDataflowBlockOptions();
            readOption.MaxDegreeOfParallelism = maxWrite;
            // 
            // create blocks of pipeline
            var readFiles = new TransformBlock<string, string>
                (FileReader.ReadTextAsync, readOption);
            var generateTests = new TransformManyBlock<string, TestWrapper> 
                (TestsGenerator.GenerateTests, generateOption);
            var writeFiles = new ActionBlock<TestWrapper>
                (async test => 
                await FileCreaterAndWriter.CreateAndWrite
                (Path.Combine(outputDirectory, test.fileName), test.sourceCode),
                writeOption);
            //
            // link options
            var linkOptions = new DataflowLinkOptions();
            linkOptions.PropagateCompletion = true;
            //
            //linking
            readFiles.LinkTo(generateTests, linkOptions);
            generateTests.LinkTo(writeFiles, linkOptions);
            //
            // main read cycle
            foreach (var file in inputFiles)
            {
                readFiles.Post(file);
            }
            //
            // completing block to don't accept any more data
            readFiles.Complete();
            //
            // returning task of all pipeline(see linking)
            return writeFiles.Completion;
        }
    }
}
