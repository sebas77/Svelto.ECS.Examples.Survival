using System;
using System.Runtime.InteropServices;

class PInvokeWrapper  
{
    [DllImport("Win32Project1.dll")]
    public static extern void PIXBeginEventEx(UInt64 color, string text);
    [DllImport("Win32Project1.dll")]
    public static extern void PIXEndEventEx();
}  