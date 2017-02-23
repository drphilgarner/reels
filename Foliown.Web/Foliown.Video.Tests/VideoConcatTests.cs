using Foliown.Video;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Foliown.Video.Tests
{
    public class VideoConcatTests
    {
        [Fact][Trait("testType","integration")]
        public void Can_Concat_Videos()
        {
            var driver = new FfmmpegDriver();

            var sourceFiles = new List<string> {$"{Environment.CurrentDirectory}\\sourceFiles\\1.mp4", $"{Environment.CurrentDirectory}\\sourceFiles\\2.mp4" };

            var outputFile = $"{Guid.NewGuid()}.mp4";

            driver.ConcatVideo(sourceFiles, outputFile);
            var resFile = Path.Combine(Environment.CurrentDirectory, "output", outputFile);

            Assert.True(File.Exists(resFile));

            //clean up and delete
            File.Delete(resFile);
        }
    }
}
