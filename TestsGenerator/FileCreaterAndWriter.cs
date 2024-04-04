using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestsGenerator
{
    class FileCreaterAndWriter
    {
        public static async Task CreateAndWrite(string pathToFile, string content)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(content);
            using (var stream =
                new FileStream(
                    pathToFile,
                    FileMode.Create, FileAccess.Write, FileShare.None,
                    bufferSize: 4096, useAsync: true
                    ))
            {
                await stream.WriteAsync(buffer, 0, buffer.Length);
            }
        }
    }
}
