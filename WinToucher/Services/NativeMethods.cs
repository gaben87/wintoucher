using System;
using System.Runtime.InteropServices;

public static class NativeMethods
{
    [StructLayout(LayoutKind.Sequential)]
    public struct POINT
    {
        public int X;
        public int Y;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct POINTER_TOUCH_INFO
    {
        public POINTER_INFO pointerInfo;
        public TOUCH_FLAGS touchFlags;
        public TOUCH_MASK touchMask;
        public RECT rcContact;
        public RECT rcContactRaw;
        public uint orientation;
        public uint pressure;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct POINTER_INFO
    {
        public uint pointerType;
        public uint pointerId;
        public uint frameId;
        public uint pointerFlags;
        public IntPtr sourceDevice;
        public IntPtr hwndTarget;
        public POINT ptPixelLocation;
        public POINT ptHimetricLocation;
        public POINT ptPixelLocationRaw;
        public POINT ptHimetricLocationRaw;
        public uint dwTime;
        public uint historyCount;
        public int InputData;
        public uint dwKeyStates;
        public ulong PerformanceCount;
        public int ButtonChangeType;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct RECT
    {
        public int left;
        public int top;
        public int right;
        public int bottom;
    }

    [Flags]
    public enum TOUCH_FLAGS
    {
        TOUCH_FLAG_NONE = 0x00000000
    }

    [Flags]
    public enum TOUCH_MASK
    {
        TOUCH_MASK_NONE = 0x00000000,
        TOUCH_MASK_CONTACTAREA = 0x00000001,
        TOUCH_MASK_ORIENTATION = 0x00000002,
        TOUCH_MASK_PRESSURE = 0x00000004
    }

    [DllImport("user32.dll")]
    public static extern bool InitializeTouchInjection(uint maxTouches, uint feedbackMode);

    [DllImport("user32.dll", SetLastError = true)]
    public static extern bool InjectTouchInput(int count, [In] POINTER_TOUCH_INFO[] contacts);

    [DllImport("kernel32.dll")]
    public static extern uint GetLastError();

    [DllImport("user32.dll")]
    public static extern bool GetCursorPos(out POINT lpPoint);

    public const uint TOUCH_FEEDBACK_DEFAULT = 0x1;
    public const uint PT_TOUCH = 0x2;
    public const uint POINTER_FLAG_DOWN = 0x0001;
    public const uint POINTER_FLAG_UP = 0x0004;
    public const uint POINTER_FLAG_UPDATE = 0x0002;
    public const uint POINTER_FLAG_INRANGE = 0x0008;
    public const uint POINTER_FLAG_INCONTACT = 0x0010;
}