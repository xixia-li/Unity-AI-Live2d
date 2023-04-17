using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/*xbb
 * 拖动窗口UI
 * */
public class WindowDragObject : MonoBehaviour, IPointerDownHandler, IPointerExitHandler, IPointerUpHandler
{
    private bool isDrag = false;

    void Update()
    {
        if (isDrag == true)
        {
            //WindowsToolMgr.Instance.Drag();
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isDrag = false;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isDrag = false;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isDrag = true;
    }
}