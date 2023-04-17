using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TypeWriter : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("打字间隔")] public float typeTimer = 0.2f;
    [Header("打字的内容")] public string words;
    private Text txtFile;
    private bool isStartTyping = true;//是否开始打字
    private float timer;
    private int currentIndex = 0;

    private void Start()
    {
        txtFile = GetComponent<Text>();
        //返回两个或多个值中最大的值
        typeTimer = Mathf.Max(0.02f, typeTimer);

    }

    private void Update()
    {
        OnStartTyping();
    }

    private void OnStartTyping()
    {

        if (isStartTyping)
        {
            timer += Time.deltaTime;
            if (timer >= typeTimer)
            {
                timer = 0;
                currentIndex++;
                txtFile.text = words.Substring(0, currentIndex);
                if (currentIndex >= words.Length)
                {
                    OnFinshTyping();
                }
            }
        }
    }

    private void OnFinshTyping()
    {
        isStartTyping = false;
        timer = 0;
        currentIndex = 0;
        txtFile.text = words;
    }
    
}
