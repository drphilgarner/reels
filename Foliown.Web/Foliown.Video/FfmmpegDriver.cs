using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Foliown.Video
{
    public class FfmmpegDriver
    {
        private const string OutputFolder = "output";

        public FfmmpegDriver()
        {
                
        }

        public void ConcatVideo(List<string> filePaths, string outputFilename)
        {
            //ffmpeg -f concat -safe 0 -i ..\videos\input.txt -codec copy ..\videos\output.mp4

            var inputFileName = Guid.NewGuid() + ".txt";

            var destPath = Environment.CurrentDirectory + $"\\{OutputFolder}";
            if (!Directory.Exists(destPath))
            {
                Directory.CreateDirectory(destPath);
            }

            var sourcePath = Environment.CurrentDirectory + $"\\work\\{inputFileName}";
            using (var fs = new StreamWriter(sourcePath))
            {
                filePaths.ForEach(t => fs.WriteLine($"file '{t}'"));
            }

            var psi = new ProcessStartInfo()
            {
                FileName = "ffmpeg.exe",
                RedirectStandardInput = true,
                RedirectStandardError = true,
                UseShellExecute = true,
                CreateNoWindow = false,
                Arguments = $"-f concat -safe 0 -i {sourcePath} -codec copy {destPath}\\{outputFilename}"
            };

            var proc = new Process
            {
                StartInfo = psi,
                EnableRaisingEvents = true
            };
            try
            {
                proc.ErrorDataReceived += Proc_ErrorDataReceived;
                proc.OutputDataReceived += Proc_OutputDataReceived;
                proc.Exited += Proc_Exited;

                proc.Start();

                proc.BeginErrorReadLine();
                proc.BeginOutputReadLine();

            }
            catch
            {
                if (proc != null)
                    proc.Dispose();
            }

        }

        private void Proc_Exited(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void Proc_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void Proc_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
