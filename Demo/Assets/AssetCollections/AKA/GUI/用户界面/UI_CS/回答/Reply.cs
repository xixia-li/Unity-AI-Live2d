using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class Reply : MonoBehaviour
{
    //输入输入框
    private InputField inputField;
    //获取回答框
    private InputField outField;
    //获取回答框的OBject
    private GameObject outField_OBject;
    //获取网页中的AudioClip音频文件
    private AudioClip audioClip;
    //获取人物Audio播放器
    private AudioSource audioSource;
    //获取回答框的Image
    private Image OutField_Image;
    
    //存放JSON的textWrite  用于打字机
    private char[] Text_Write;
    //Text_Writer打字速度         说话事件/字数=打字速度
    private float TextWrite_Velocity;

    //创建返回json
    private ReturnReply_Json returnReply_Json = new ReturnReply_Json();

    //用于判断TextWrile是否成功
    private bool TextWrite_Bool;
    //用于判断AudioClip是否成功
    private bool AudioClip_Bool;
    //用于判断是否在执行
    private bool execute = false;


    void Start()
    {
        //获取"myCanvas(Clone)/InputField"下的inputField
        inputField = GameObject.Find("myCanvas(Clone)/InputField").GetComponent<InputField>();
        //获取"myCanvas(Clone)/InputField"下的outField
        outField = GameObject.Find("myCanvas(Clone)/OutField").GetComponent<InputField>();
        //获取myCanvas(Clone)/OutField下的InputField组件
        outField_OBject = GameObject.Find("myCanvas(Clone)/OutField");
        //获取myCanvas(Clone)/OutField下的Image组件
        OutField_Image = GameObject.Find("myCanvas(Clone)/OutField").GetComponent<Image>();
        //设置OutField_Image的Color为（0,0,0,0）使其为透明
        OutField_Image.color = new Color(0, 0, 0, 0);
        //将回答框的SetActive设置为False
        outField_OBject.SetActive(false);

        //获取myCamera下的AudioSource
        audioSource = GameObject.Find("myCamera").GetComponent<AudioSource>();

        

    }

    // Update is called once per frame
    void Update()
    {
        
        //输入回车时执行onEndEdlt协程函数
        if (!execute && (Input.GetKey(KeyCode.Return) || Input.GetKey(KeyCode.KeypadEnter)))
        {
            //执行onEndEdlt协程函数
            StartCoroutine(OnEndEdlt());
            //正在执行协程函数
            execute = true;
        }
    }








    //OnEndEdlt协程函数
    IEnumerator OnEndEdlt()
    {
        //延迟1秒 用于释放回车
        Invoke("activelt", 1f);
        //等download协程函数执行完毕
        yield return Download();
        //音频与文字无问题进行播放
        if (AudioClip_Bool && TextWrite_Bool)
        {
            //执行播放音频协程
            StartCoroutine(AudioSource_Play());
            //执行打字机协程
            StartCoroutine(TextWrite_Play());

        }
    }




    //上传用的Json
    [System.Serializable]
    class PostReply_Json
    {
        //生成的文本，支持常见标点符号。英文可能无法正常生成，数字请转换为对应的汉字再进行生成。
        public string text;
        //说话者名称。必须是上面的名称之一。 
        public string speaker;
        //生成时使用的 noise_factor，可用于控制感情等变化程度。默认为0.667。
        public float noise;
        //生成时使用的 noise_factor_w，可用于控制音素发音长度变化程度。默认为0.8。
        public float noisew;
        //浮点数 生成时使用的 length_factor，可用于控制整体语速。默认为1.2。 
        public float length;
        //生成语音的格式，必须为mp3或者wav。默认为mp3。 
        public string format;
    }

    //嵌套用的json格式
    [System.Serializable]
    class Data
    {
        //Json中返回的Text
        public string text;
        //Json中放回的Files
        public string files;

    }

    //返回用的JSON
    [System.Serializable]
    class ReturnReply_Json
    {
        //Json中返回的Status
        public int status;
        //Json中返回的msg
        public string msg;
        //Json中返回的Data合集
        public Data data;
    }

    IEnumerator Download()
    {
        //创建上传用的JSON
        PostReply_Json postReply_Json = new PostReply_Json();
        //对答框中text赋给JSON中的text
        postReply_Json.text = inputField.text;
        // 设置说话者
        postReply_Json.speaker = "派蒙";
        //设置感情变换速度
        postReply_Json.noise = 0.8f;
        //设置发音变化
        postReply_Json.noisew = 1.0f;
        //设置总体语速
        postReply_Json.length = 1.2f;
        //设置音频格式
        postReply_Json.format = "wav";

        //将postReply_Json转换为string
        string JSON = JsonUtility.ToJson(postReply_Json);

        //创建URL并进行POST获取
        UnityWebRequest WWW = new UnityWebRequest("http://logn.club:8999/api/v1/voice", "POST");
        //将JSON 的格式转换为utf-8
        byte[] EncodedPayload = new System.Text.UTF8Encoding().GetBytes(JSON);
        //上传JSON
        WWW.uploadHandler = new UploadHandlerRaw(EncodedPayload);
        //下载返回JSON
        WWW.downloadHandler = new DownloadHandlerBuffer();
        //返回JSON设置为Content-Type  Application/json
        WWW.SetRequestHeader("Content-Type", "application/json");
        //返回JSON设置cache-control   no-cache          
        WWW.SetRequestHeader("cache-control", "no-cache");

        //等待JSON下载完毕
        yield return WWW.SendWebRequest();

        //判断WWW是否获取成功
        if (!WWW.isDone || WWW.isNetworkError)
        {
            //WWW获取失败报错
            Debug.Log("服务器未开启");
        }
        else
        {

            //返回JSON获取WWW下载的内容
            returnReply_Json = JsonUtility.FromJson<ReturnReply_Json>(WWW.downloadHandler.text);

            //将内容全部Debug一遍
            Debug.Log(returnReply_Json.data.text);
            Debug.Log(returnReply_Json.data.files);
        }

        //判断是否获取到URL上音频
        if (returnReply_Json.data.files != null)
        {
            //获取URL上音频格式，并转换为WAV
            var URL_AudioClip = UnityWebRequestMultimedia.GetAudioClip(returnReply_Json.data.files, AudioType.WAV);
            //等待URL_AudioClip下载完
            yield return URL_AudioClip.SendWebRequest();
            //判断下载后内容是否为空
            if (URL_AudioClip.isNetworkError)
            {
                //为空报错
                Debug.Log("URL错误");
            }
            else
            {
                //将URL中的AudioClip存放在audioClip私有变量中
                audioClip = DownloadHandlerAudioClip.GetContent(URL_AudioClip);
                //判断audioClip私有变量是否为空
                if (!audioClip.LoadAudioData())
                {
                    //Audioclip为空报错
                    Debug.Log("AudioClip错误");
                    //AudioClip获取失败
                    AudioClip_Bool = false;
                }
                else
                {
                    audioSource.clip = audioClip;
                    //AudioClip获取成功
                    AudioClip_Bool = true;
                }
            }
        }
        //判断是否在URL上获取到Text文本
        if (returnReply_Json.data.text != null)
        {
            //将URL上获取的文本存放在Text_Write私有变量中
            Text_Write = returnReply_Json.data.text.ToCharArray();

            if (Text_Write == null)
            {
                //Text_Write为空报错
                Debug.Log("text_Write错误");
                //TextWrite获取失败
                TextWrite_Bool = false;
            }
            else
            {
                //设置Text_Writer打字速度         说话事件/字数=打字速度
                TextWrite_Velocity = audioSource.clip.length / Text_Write.Length;
                Debug.Log("打字速度：" + TextWrite_Velocity);
                Debug.Log("说话时间：" + audioSource.clip.length);
                Debug.Log("字数：" + Text_Write.Length);
                //TextWrite获取成功
                TextWrite_Bool = true;
            }
        }
    }

    IEnumerator AudioSource_Play()
    {
        //播放AudioSource音频组件
        audioSource.Play();
        //等待音频播放完毕
        yield return new WaitForSeconds(audioSource.clip.length);

    }

    IEnumerator TextWrite_Play()
    {
        //显示OutField回答框
        outField_OBject.SetActive(true);



        //逐渐增加OutField.color的透明度
        for (float Melt = 0f; Melt < 255f; Melt += 0.001f)
        {
            //增加透明度
            OutField_Image.color = new Color(OutField_Image.color.r, OutField_Image.color.g, OutField_Image.color.b, OutField_Image.color.a + Melt);
            //延迟0.01秒
            yield return new WaitForSeconds(0.01f);
        }

        //输入框初始化
        inputField.text = "";
        //回答框初始化
        outField.text = "";
        //执行打字机
        for (int Contion = 0; Contion < Text_Write.Length; Contion++)
        {
            //逐步打字
            outField.text += Text_Write[Contion];
            //每个字打时间
            yield return new WaitForSeconds(TextWrite_Velocity);
        }
        //执行完成
        execute = false;

        //逐渐增加OutField.color的透明度
        for (float Melt = 0f; Melt < 255f; Melt += 0.001f)
        {
            //减少透明度
            OutField_Image.color = new Color(OutField_Image.color.r, OutField_Image.color.g, OutField_Image.color.b, OutField_Image.color.a - Melt);
            //延迟0.01秒
            yield return new WaitForSeconds(0.01f);
        }

    }


}
