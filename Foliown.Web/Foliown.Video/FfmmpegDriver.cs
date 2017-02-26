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

        public void ConcatAndOverlayTextVideo(List<string> filePaths,
            string outputFilename,
            List<TextOverlay> textOverlays)
        {

                  var inputFileName = Guid.NewGuid() + ".txt";

            var destPath = Path.Combine(Environment.CurrentDirectory, OutputFolder);
            if (!Directory.Exists(destPath))
            {
                Directory.CreateDirectory(destPath);
            }

            _sourcePath = Path.Combine(Environment.CurrentDirectory, inputFileName);
            
            var sourceFiles = filePaths.Select(f => $" -i {f} ").ToList();

            filePaths.ForEach(t =>
            {
                if (!File.Exists(t)) throw new FileNotFoundException($"{t} not found when preprocessing video.");
            });
            
            var joinedSources = sourceFiles.Aggregate((x,y) => x+y);

            //FFMPEG filter docs
            //http://ffmpeg.org/ffmpeg-filters.html#drawtext-1

            //check fonts exist
            textOverlays.ForEach(t =>
            {
                if (!File.Exists(t.FontPath)) throw new Exception($"Target font file not found {t.FontPath}");
            });

            string builtArgs = $" {joinedSources} " +
                               "-y -filter_complex \"[0:0] [0:1] [1:0] [1:1]" +
                               $"concat=n={filePaths.Count}:v=1:a=1 [v] [a]; [v]" ;

            foreach (var t in textOverlays)
            {
                if (textOverlays.IndexOf(t) != 0)
                    builtArgs += ",";

                builtArgs += $"drawtext=fontsize={t.FontSize}:fontcolor={t.FontColor}" +
                             $":fontfile={t.FontPath}:text={t.Text}" +
                             ":x=(w-tw)/2:y=(h/PHI)+th:box=1:boxcolor=black@0.4" +
                             $":enable=gt(t\\,{t.Timecode})*lt(t\\,{t.Timecode + t.Duration})";
            }

            builtArgs += $"[output]\" -map \"[output]\" -map \"[a]\" {OutputFolder}\\{outputFilename}";

            
            var psi = new ProcessStartInfo()
            {
                FileName = "ffmpeg\\ffmpeg.exe",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = false,
                Arguments = builtArgs
            };

            //λ ffmpeg\ffmpeg.exe - i SourceFiles\1.mp4 - i SourceFiles\2.mp4 - y 
            //-filter_complex "[0:0] [0:1] [1:0] [1:1] concat=n=2:v=1:a=1 [v] [a]; 
            //[v]drawtext=fontsize=72:fontcolor=White:fontfile=/Windows/Fonts/Arial.ttf:
            //textfile =videoMsg.txt:x=(w-tw)/2:y=(h/PHI)+th:box=1:boxcolor=black@0.4:
            //enable =lt(t\,5),drawtext=fontsize=72:text=SG08BBS:
            //fontcolor =White:fontfile=/Windows/Fonts/Arial.ttf:
            //enable =gt(t\,9)*lt(t\,11):x=(w-tw)/2:y=(h/PHI)+th:box=1:
            //boxcolor =black@0.4[o]" - map "[o]" - map "[a]" outputWithText.mp4



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
