using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.InteropServices;

public class WindowsTools
{
    //设置当前窗口的显示状态
    [DllImport("user32.dll")]
    public static extern bool ShowWindow(System.IntPtr hwnd, int nCndShow);

    //获取当前激活窗口
    [DllImport("User32.dll",EntryPoint = "GetForegroundWindow")]
    public static extern System.IntPtr GetForegroundWindow();

    //设置窗口边框
    [DllImport("user32.dll")]
    public static extern IntPtr SetWindowLong(IntPtr hwnd, int _nIndex, int dwNewLong);


    //设置窗口位置，大小
    [DllImport("user32.dll")]
    public static extern bool SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

    //窗口拖动
    [DllImport("user32.dll")]
    public static extern bool ReleaseCapture();
    [DllImport("user32.dll")]
    public static extern bool SendMessage(IntPtr hwnd, int wMsg, int wParam, int lParam);

    const uint SWP_ShowWindow = 0x0040;
    const int GWL_Style = -16;
    const int WS_Border = 1;
    const int WS_Popup = 0x800000;
    const int SW_ShowMinimized = 2;

    //窗口最小化
    public void SetMinWindows()
    {
        ShowWindow(GetForegroundWindow(), SW_ShowMinimized);
        //具体窗口参数在这个链接 https://msdn.microsoft.com/en-us/library/windows/desktop/ms633548(v=vs.85).aspx

    }

    //设置无边框，并设置框体大小，位置
    public void SetNoFrameWindow(Rect rect)
    {
        SetWindowLong(GetForegroundWindow(), GWL_Style, WS_Popup);
        bool result = SetWindowPos(GetForegroundWindow(), 0, (int)rect.x, (int)rect.y, (int)rect.width, (int)rect.height, SWP_ShowWindow);

    }

    //拖动窗口
    public void DragWindow(IntPtr window)
    {
        ReleaseCapture();
        SendMessage(window, 0xA1, 0x02, 0);
        SendMessage(window, 0x0202, 0, 0);
    }
}
