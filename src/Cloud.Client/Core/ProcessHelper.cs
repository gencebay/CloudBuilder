using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Threading;
using System.Text;

namespace Cloud.Client.Core
{
    public class ProcessHelper
    {
        public static void Run(string workingDirectory, string args)
        {
            using (Process process = new Process())
            {
                process.StartInfo.WorkingDirectory = workingDirectory;
                process.StartInfo.FileName = "cmd";
                process.StartInfo.Arguments = args;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = true;

                process.OutputDataReceived += (sender, e) =>
                {
                    Console.WriteLine(e.Data);
                };

                process.ErrorDataReceived += (sernder, e) =>
                {
                    Console.Error.WriteLine(e.Data);
                };

                process.Start();
                process.BeginOutputReadLine();
                process.BeginErrorReadLine();
            }
        }
    }
}
