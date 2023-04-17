 
using UnityEngine;
using System.Collections;
using System;
using System.Runtime.InteropServices;
using UnityEngine.UI;

public class Window_Transparency : MonoBehaviour
{
    public Rect ScreenPosition;
    [DllImport("user32.dll")]
    static extern IntPtr SetWindowLong(IntPtr hwnd, int _nIndex, int dwNewLong);
    [DllImport("user32.dll")]
    static extern bool SetWindowPos(IntPtr hwnd, int hwndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);
    [DllImport("user32.dll")]
    static extern IntPtr GetForegroundWindow();
    [DllImport("user32.dll")]
    public static extern bool ReleaseCapture();
    [DllImport("user32.dll")]
    public static extern bool SendMessage(IntPtr hwn, int wMsg, int wParam, int lParam);
    [DllImport("user32.dll")]
    public static extern bool ShowWindow(IntPtr hwnd, int nCmdShow);

    const uint SWP_SHOWWINDOW = 0x0040;
    const int GWL_STYLE = -16;
    const int WS_BORDER = 1;
    const int WS_POPUP = 0x800000;
    const int SW_SHOWMINIMIZED = 2;//最小化,激活
    const int SW_SHOWMAXIMIZED = 3;//最大化,激活
    public void btn_onclickMin()
    {
        //最小化
        ShowWindow(GetForegroundWindow(), SW_SHOWMINIMIZED);
    }
    public void btn_onclickMax()
    {
        //最大化
        ShowWindow(GetForegroundWindow(), SW_SHOWMAXIMIZED);
    }

    IntPtr Handle;
    float ScreenX;
    bool BoolX;
    private void Start()
    {
        BoolX = false;
        ScreenX = 0f;
        #if UNITY_STANDALONE_WIN
        Resolution[] resolutions = Screen.resolutions;
        ScreenPosition = new Rect((resolutions[resolutions.Length - 1].width - Screen.width) / 2, (resolutions[resolutions.Length - 1].height - Screen.height) / 2, Screen.width, Screen.height);
        SetWindowLong(GetForegroundWindow(), GWL_STYLE, WS_POPUP);//将WS_BORDER替换为WS_POPUP
        Handle = GetForegroundWindow();//FindWindow((string)null,"popu_windows");
        SetWindowPos(GetForegroundWindow(),0, (int)ScreenPosition.x,(int)ScreenPosition.y,(int)ScreenPosition.width, (int)ScreenPosition.height, SWP_SHOWWINDOW);
        #endif
    }
        private void Update()
        {
#if UNITY_STANDALONE_WIN
        if(Input.GetMouseButtonDown(0))
        {
            ScreenX = 0f;
            BoolX = true;
        }
        if(BoolX&&ScreenX>=0.2f)
        {
            //区分界面上面其它需要滑动的操作
            ReleaseCapture();
            SendMessage(Handle, 0xA1, 0x02, 0);
            SendMessage(Handle, 0x0202, 0, 0);
        }
        if (BoolX)
            ScreenX += Time.deltaTime;
        if(Input.GetMouseButtonUp(0))
        {
            ScreenX = 0f;
            BoolX = false;
        }
#endif
    }
}