using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIControls : MonoBehaviour
{
    // Start is called before the first frame update
    GameObject SlideDown;
    GameObject Game_Out;
    GameObject SlideUp;
    private void Start()
    {
        SlideDown = GameObject.Find("SlideDown");
        Game_Out = GameObject.Find("Game_Out");
        SlideUp = GameObject.Find("SlideUp");
        Game_Out.SetActive(false);
        SlideUp.SetActive(false);
    }



    public void Slide_Down()
    {
       StartCoroutine(To_Move_Jump());
    }
    public void Slide_UP()
    {
        //调用上拉
        StartCoroutine(Return_Move_Jump());
    }


    private IEnumerator To_Move_Jump()
    {
        SlideDown.SetActive(false);
        SlideUp.SetActive(true);
        Game_Out.SetActive(true);
        yield return StartCoroutine(To_Move());
        
        
    }

    private IEnumerator To_Move()
    {

        //获取下拉列表中的元素
        RectTransform button = transform.Find("Game_Out").GetComponent<RectTransform>();

        //指定位置
        Vector3 ToMove = new Vector3(300, 420, 0);

        //判断是否在指定位置
        while (button.localPosition != ToMove)
        {
            //平移到指定位置
            button.localPosition = Vector3.MoveTowards(button.localPosition, ToMove, 500 * Time.deltaTime);

            yield return new WaitForSeconds(0.01f);

        }

    }



    private IEnumerator Return_Move_Jump()
    {
        SlideDown.SetActive(true);
        SlideUp.SetActive(false);
        yield return StartCoroutine(Return_Move());
        Game_Out.SetActive(false);

    }

    IEnumerator Return_Move()
    {
        //获取下拉列表中的元素
        RectTransform button = transform.Find("Game_Out").GetComponent<RectTransform>();
        //指定位置
        Vector3 ToMove = new Vector3(300, 500, 0);
        //判断是否在指定位置
        while (button.localPosition != ToMove)
        {
            //平移到指定位置
            button.localPosition = Vector3.MoveTowards(button.localPosition, ToMove, 500 * Time.deltaTime);
            //完成后退出
            yield return new WaitForSeconds(0.01f);
        }
        
    }


    //退出游戏函数
    public void Quit()
    {
        Application.Quit();
    }

}
