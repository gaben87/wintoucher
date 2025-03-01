using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows;
using WinToucher.Services;

public class TouchInjectionService : ITouchInjectionService
{
    private readonly Dictionary<int, NativeMethods.POINTER_TOUCH_INFO> activeTouches = new();
    private bool isInitialized = false;

    public void Initialize(uint maxTouches)
    {
        if (!NativeMethods.InitializeTouchInjection(maxTouches, NativeMethods.TOUCH_FEEDBACK_DEFAULT))
        {
            int error = Marshal.GetLastWin32Error();
            throw new InvalidOperationException($"Failed to initialize touch injection. Error code: {error}");
        }
        isInitialized = true;
    }

    public void InjectTouchDown(int id, Point position)
    {
        EnsureInitialized();

        if (activeTouches.ContainsKey(id))
            throw new InvalidOperationException($"Touch ID {id} is already active.");

        var touchInfo = CreateTouchInfo(id, position, NativeMethods.POINTER_FLAG_DOWN | NativeMethods.POINTER_FLAG_INCONTACT | NativeMethods.POINTER_FLAG_INRANGE);
        activeTouches[id] = touchInfo;

        Inject(touchInfo);
    }

    public void InjectTouchMove(int id, Point position)
    {
        EnsureInitialized();

        if (!activeTouches.ContainsKey(id))
            throw new InvalidOperationException($"Touch ID {id} is not active.");

        var touchInfo = activeTouches[id];
        touchInfo.pointerInfo.ptPixelLocation = new NativeMethods.POINT { X = (int)position.X, Y = (int)position.Y };
        touchInfo.pointerInfo.pointerFlags = NativeMethods.POINTER_FLAG_UPDATE | NativeMethods.POINTER_FLAG_INCONTACT | NativeMethods.POINTER_FLAG_INRANGE;

        activeTouches[id] = touchInfo;
        Inject(touchInfo);
    }

    public void InjectTouchUp(int id, Point position)
    {
        EnsureInitialized();

        if (!activeTouches.ContainsKey(id))
            throw new InvalidOperationException($"Touch ID {id} is not active.");

        var touchInfo = activeTouches[id];
        touchInfo.pointerInfo.ptPixelLocation = new NativeMethods.POINT {X = (int)position.X, Y = (int)position.Y };
        touchInfo.pointerInfo.pointerFlags = NativeMethods.POINTER_FLAG_UP;

        Inject(touchInfo);
        activeTouches.Remove(id);
    }

    private NativeMethods.POINTER_TOUCH_INFO CreateTouchInfo(int id, Point position, uint flags)
    {
        return new NativeMethods.POINTER_TOUCH_INFO
        {
            pointerInfo = new NativeMethods.POINTER_INFO
            {
                pointerType = NativeMethods.PT_TOUCH,
                pointerId = (uint)id,
                pointerFlags = flags,
                ptPixelLocation = new NativeMethods.POINT {X = (int)position.X, Y = (int)position.Y }
            },
            touchFlags = NativeMethods.TOUCH_FLAGS.TOUCH_FLAG_NONE,
            touchMask = NativeMethods.TOUCH_MASK.TOUCH_MASK_CONTACTAREA | NativeMethods.TOUCH_MASK.TOUCH_MASK_ORIENTATION | NativeMethods.TOUCH_MASK.TOUCH_MASK_PRESSURE,
            rcContact = new NativeMethods.RECT
            {
                left = (int)position.X - 2,
                top = (int)position.Y - 2,
                right = (int)position.X + 2,
                bottom = (int)position.Y + 2
            },
            orientation = 90,
            pressure = 32000
        };
    }

    private void Inject(NativeMethods.POINTER_TOUCH_INFO touchInfo)
    {
        if (!NativeMethods.InjectTouchInput(1, new[] { touchInfo }))
        {
            int error = Marshal.GetLastWin32Error();
            throw new InvalidOperationException($"InjectTouchInput failed. Error code: {error}");
        }
    }

    private void EnsureInitialized()
    {
        if (!isInitialized)
            throw new InvalidOperationException("Touch injection has not been initialized. Call Initialize() first.");
    }
}
