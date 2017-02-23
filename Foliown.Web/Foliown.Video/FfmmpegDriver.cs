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
        private Process _process;
        private string _sourcePath = string.Empty;
        private const string OutputFolder = "output";


        public void ConcatVideo(List<string> filePaths, string outputFilename)
        {

            //ffmpeg -f concat -safe 0 -i ..\videos\input.txt -codec copy ..\videos\output.mp4

            var inputFileName = Guid.NewGuid() + ".txt";

            var destPath = Path.Combine(Environment.CurrentDirectory, OutputFolder);
            if (!Directory.Exists(destPath))
            {
                Directory.CreateDirectory(destPath);
            }

            _sourcePath = Path.Combine(Environment.CurrentDirectory, inputFileName);

            using (var fs = File.CreateText(_sourcePath))
            {
                filePaths.ForEach(t => fs.WriteLine($"file '{t}'"));
            }


            var psi = new ProcessStartInfo()
            {
                FileName = "ffmpeg\\ffmpeg.exe",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = false,
                Arguments = $"-f concat -safe 0 -i {_sourcePath} -codec copy {destPath}\\{outputFilename}"
            };

            _process = new Process
            {
                StartInfo = psi,
                EnableRaisingEvents = true,

            };
            try
            {
                _process.ErrorDataReceived += Proc_ErrorDataReceived;
                _process.OutputDataReceived += Proc_OutputDataReceived;
                _process.Exited += Proc_Exited;

                _process.Start();

                _process.BeginErrorReadLine();
                _process.BeginOutputReadLine();
                _process.WaitForExit();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                _process.Dispose();
            }

        }

        private void Proc_Exited(object sender, EventArgs e)
        {
            _process.Dispose();

            if (File.Exists(_sourcePath))
                File.Delete(_sourcePath);
        }

        private void Proc_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            //throw new NotImplementedException();
        }

        private void Proc_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            //throw new NotImplementedException();
        }
    }
}
