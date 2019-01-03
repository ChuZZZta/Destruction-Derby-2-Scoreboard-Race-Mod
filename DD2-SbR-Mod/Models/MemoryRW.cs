﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Sbr;

namespace Sbr.Models
{
    public class MemoryRW : IMemoryRW
    {
        public MemoryRW(string processname)
        {
            this.processname = processname;
        }
        string processname;

        [DllImport("kernel32.dll")]
        public static extern IntPtr OpenProcess(int dwDesiredAccess, bool bInheritHandle, int dwProcessId);

        [DllImport("kernel32.dll")]
        public static extern bool ReadProcessMemory(int hProcess, int lpBaseAddress, byte[] lpBuffer, int dwSize, ref int lpNumberOfBytesRead);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool WriteProcessMemory(int hProcess, int lpBaseAddress, byte[] lpBuffer, int dwSize, ref int lpNumberOfBytesWritten);

        public int GetByte(int offset)
        {

            Process process = Process.GetProcessesByName(processname)[0];
            IntPtr processHandle = OpenProcess(0x0010, false, process.Id);
            int address = (int)process.MainModule.BaseAddress + offset; // calculating the current position of variable
            int bytesRead = 0;
            byte[] buffer = new byte[1]; 
            ReadProcessMemory((int)processHandle, address, buffer, buffer.Length, ref bytesRead);
            return buffer[0];
        }

        public void SetByte(int offset,byte value)
        {
            Process process = Process.GetProcessesByName(processname)[0];
            IntPtr processHandle = OpenProcess(0x1F0FFF, false, process.Id);
            int address = (int)process.MainModule.BaseAddress + offset; // calculating the current position of variable
            int bytesWritten = 0;
            byte[] buffer = new byte[1];
            buffer[0] = value;
            WriteProcessMemory((int)processHandle, address, buffer, buffer.Length, ref bytesWritten);
        }
    }
}
