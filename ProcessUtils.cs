using System;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Collections.Generic;
using System.Management;
using System.Linq;

namespace FindProcess
{
    class ProcessUtils
    {
        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();
        [DllImport("user32.dll", SetLastError = true)]
        static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);
        public static Process GetForegroundProcess()
        {
            uint processId = 0;
            IntPtr hWnd = GetForegroundWindow();
            uint threadId = GetWindowThreadProcessId(hWnd, out processId);
            return Process.GetProcessById(Convert.ToInt32(processId));
        }

        public static List<Proc> GetChildProcesses(int parentId)
        {
            // For whole class, check out this page
            // https://docs.microsoft.com/en-us/windows/win32/cimwin32prov/win32-process
            var searcher = new ManagementObjectSearcher("Select Name, ProcessId From Win32_Process Where ParentProcessId = " + parentId);
            var processList = searcher.Get();
            return processList.Cast<ManagementObject>().Select(p => new Proc
            {
                Name = p.GetPropertyValue("Name").ToString(),
                Id = Convert.ToInt32(p.GetPropertyValue("ProcessId"))
            }).ToList();
        }
    }
    class Proc
    {
        public string Name { get; set; }
        public int Id { get; set; }
    }
}
