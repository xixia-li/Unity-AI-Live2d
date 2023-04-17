using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Live2D.Cubism.Framework.Json;
using System;
using System.IO;


public class Instantiation : MonoBehaviour
{
    public static GameObject CameraObj;
    public static GameObject MySelf;
    // Start is called before the first frame update
    void Start()
    {
        //初始化方法
        Initialize();
        /*string Path = Application.dataPath + "/AssetCollections/AKA/AssetsMaterial/AKA_N001/Akahuhu3001.model3.json";
        CubismModel3Json cubismModel3Json = CubismModel3Json.LoadAtPath(Path, BuiltInLoadAssetAtPath);
        cubismModel3Json.ToModel();*/
        //MySelf = GetComponent<Game_Interface>();

    }
    //初始化
    public static void Initialize()
    {
        //定义myCamera摄像机
        CameraObj = new GameObject("myCamera");
        //查找myCamera摄像机
        Camera camera = CameraObj.AddComponent<Camera>();
        //设置myCamera摄像机位置
        camera.transform.localPosition =new Vector3(0,0,-10);
        //设置myCamera摄像机大小
        camera.transform.localScale = new Vector3(1, 1, 1);
        //设置myCamera摄像机距离
        camera.fieldOfView = 7.0f;
        //设置myCamera摄像机背景颜色
        camera.backgroundColor = new Color(0, 0, 0);
        //设置MyCamera摄像机脚本挂载
        CameraObj.AddComponent<Play_Audio>();


        //设置MyCamera挂载audio音频播放器
        CameraObj.AddComponent<AudioSource>();
        //挂载TransparentWindow透明化脚本
        CameraObj.AddComponent<TransparentWindow>();
        //挂载Window_Transparency拖拽脚本 -- 有bug 在测试界面也能拖拽，在不打包时勿用
        CameraObj.AddComponent<Window_Transparency>();
        //挂载MyCameraAudioListener接收器
        CameraObj.AddComponent<AudioListener>();

        //获取本地Canvas画布加载实例
        GameObject UserUI = Resources.Load<GameObject>("myCanvas");
        //获取EventSystem加载实例
        GameObject eventSystem = Resources.Load<GameObject>("EventSystem");
        //获取AKA加载实例
        GameObject AKA = Resources.Load<GameObject>("Akahuhu3001");
        //实例化以下
        Instantiate(AKA);
        Instantiate(UserUI);
        Instantiate(eventSystem);

    }



    //加载--------暂时未定义--------------
    public static object BuiltInLoadAssetAtPath(Type assetType, string AbsolutePath)
    {
        if (assetType == typeof(byte[]))
        {
            return File.ReadAllBytes(AbsolutePath);
        }
        else if (assetType == typeof(string))
        {
            return File.ReadAllText(AbsolutePath);
        }
        else if (assetType == typeof(Texture2D))
        {
            Texture2D texture2D = new Texture2D(1, 1);
            texture2D.LoadImage(File.ReadAllBytes(AbsolutePath));
            return texture2D;
        }
        throw new NotSupportedException();
    }
}
