using System;
using System.Runtime.InteropServices;



    public class Apis
    {
        [DllImport("k"+ "e" + "rne" + "l32" + ".d" + "l" + "l")]
        public static extern IntPtr GetProcAddress(IntPtr hModule, string procname);

        [DllImport("ker" + "ne" + "l32.dl" + "l")]
        public static extern IntPtr LoadLibrary(string name);

        [DllImport("ker" + "nel" + "32" + ".d" + "ll")]
        public static extern bool VirtualProtect(IntPtr lpAddress, UIntPtr dwSize, uint flNewProtect, out uint lpfOldPRotect);
    }

