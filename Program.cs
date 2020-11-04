using System;
using System.Diagnostics;
using System.Threading;

namespace FindProcess
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Process proc = ProcessUtils.GetForegroundProcess();
                string title = !string.IsNullOrWhiteSpace(proc.MainWindowTitle) ? proc.MainWindowTitle : proc.ProcessName;
                string text = $"[{title}]: {proc.Id}\r\n";
                foreach (var child in ProcessUtils.GetChildProcesses(proc.Id))
                    text += $">{child.Name}: {child.Id}\r\n";
                Console.Clear();
                Console.Write(text);
                Thread.Sleep(5000);
            }
        }
    }
}

