using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class WindowsToolMgr : MonoBehaviour
{

    //系统方法类实体
    public WindowsTools winTool = new WindowsTools();

    //当前Unity程序进程
    private static IntPtr currentWindow;
    
    void Start()
    {
        currentWindow = WindowsTools.GetForegroundWindow();
    }

    //最小化窗口
    public void MinWindows()
    {
        winTool.SetMinWindows();
    }

    //设置无边框窗口
    public void SetNoFrame()
    {
        //窗口大小 以此为例
        float WindowWidth = 1280;
        float WindowHeight = 720;
        //计算框体显示位置
        float posX = (Screen.currentResolution.width - WindowWidth) / 2;
        float posY = (Screen.currentResolution.height - WindowHeight) / 2;
        Rect _rect = new Rect(posX, posY, WindowWidth, WindowHeight);

        winTool.SetNoFrameWindow(_rect);
    }
    //全屏
    public void FullScreen()
    {
        if (Screen.fullScreen)
        {
            Screen.SetResolution(1024, 768, false);
        }
        else
        {
            Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, true);
        }
        //等待当前帧完成 再去除边框
        //StartCoroutine(IE_NoFrame());
        
        
    }

    private IEnumerator IE_NoFrame()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForFixedUpdate();
        if (!Screen.fullScreen)
        {
            SetNoFrame();
        }
    }

    //窗口拖动
    public void Drag()
    {
        winTool.DragWindow(currentWindow);
    }
}
