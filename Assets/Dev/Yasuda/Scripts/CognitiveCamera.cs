//https://torikasyu.com/?p=921

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Assertions;
using System.Collections;
using System.IO;
using UnityEngine.UI;
using HoloToolkit.Unity.InputModule;
using System.Collections.Generic;

public class CognitiveCamera : MonoBehaviour, IInputClickHandler
{
    public string response = "";
    public class DataReceiveEvent : UnityEvent<string> { };
    public DataReceiveEvent onDataReceived { get; private set; }

    // for inspector(uGUI Objects)
    public RawImage ImageViewer;
    public RawImage ImageCaptured;
    public Text uiText;

    [SerializeField, Range(0.2f, 30.0f)]
    private float framerate = 1.0f;
    [SerializeField]
    string VISIONKEY = "YOURVISIONKEY"; // replace with your Computer Vision API Key
    [SerializeField]
    bool autoRequest = false;

    private bool isWaitForResponse = false;
    private int camWidth_px = 1280;
    private int camHeight_px = 720;
    private WebCamTexture webcamTexture;

    private float requestFrequency;
    private float remainTimeNextRequest;
    private uint accumCount = 0;
    private string filePath = "";

    private string emotionURL = "https://westus.api.cognitive.microsoft.com/emotion/v1.0/recognize";

    void Awake()
    {
        onDataReceived = new DataReceiveEvent();
    }

    void Start()
    {
        isWaitForResponse = false;
        remainTimeNextRequest = -1.0f;
        _UpdateFrequency();
        accumCount = 0;

        // Any place AirTap
        InputManager.Instance.PushFallbackInputHandler(gameObject);

		WebCamDevice[] devices = WebCamTexture.devices;
#if UNITY_EDITOR
        string devicename = devices[0].name;
#else
        //Hololensの実機がここを通ると想定
        string devicename = devices[0].name;
#endif

        if (devices.Length > 0)
        {
            var euler = transform.localRotation.eulerAngles;
#if UNITY_EDITOR
            webcamTexture = new WebCamTexture(devicename, camWidth_px, camHeight_px);
#else
            webcamTexture = new WebCamTexture(camWidth_px, camHeight_px);
#endif
            //iPhone,Androidの場合はカメラの向きを縦にする
            if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.Android)
            {
                transform.localRotation = Quaternion.Euler(euler.x, euler.y, euler.z - 90);
            }

            ImageViewer.texture = webcamTexture;
            webcamTexture.Play();
        }
        else
        {
            Debug.Log("Webカメラが検出できませんでした");
            return;
        }
    }

    void Update()
    {
        _UpdateFrequency(); //FIXME: 値が変わったときだけ更新したいが、害はないでしょう.

        remainTimeNextRequest -= Time.deltaTime;
        if( autoRequest && remainTimeNextRequest <= 0.0f)
        {
            remainTimeNextRequest = requestFrequency;
            SendPhotoIfAvailable();
        }

        uiText.text = MakeStatus();
            
#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(1))
        {
            _OnClicked();
        }
#endif
    }

    public void SavePhoto()
    {
        string filename = string.Format(@"CapturedImage{0}_n.jpg", Time.time);
        ///filePath = System.IO.Path.Combine(Application.persistentDataPath, filename);
        filePath = Application.persistentDataPath + "/" + filename;

        Texture2D snap = new Texture2D(camWidth_px, camHeight_px);
        snap.SetPixels(webcamTexture.GetPixels());
        snap.Apply();

        //var bytes = snap.EncodeToPNG();
        var bytes = snap.EncodeToJPG();
        File.WriteAllBytes(filePath, bytes);

        ImageCaptured.texture = snap;
        //webcamTexture.Stop();

        StartCoroutine(GetVisionDataFromImages());

        accumCount++;
    }

    //Sceneを変更する場合にカメラを止める
    public void EndCam()
    {
        webcamTexture.Stop();
    }

    public void OnInputClicked(InputClickedEventData eventData)
    {
        Debug.Log("AirTapped");
        _OnClicked();
    }

    private void _UpdateFrequency()
    {
        framerate = Mathf.Clamp(framerate, 0.2f, 30.0f);
        requestFrequency = 1.0f / framerate;
    }

    private void _OnClicked()
    {
        if (webcamTexture.isPlaying)
        {
            SendPhotoIfAvailable();
        }
        else
        {
            // タップ時に、webcamTextureをStopするのをやめたので、このパスには入らない.
            Debug.LogError("must not happen");
            webcamTexture.Play();
        }
    }

    private void SendPhotoIfAvailable()
    {
        if (!isWaitForResponse) { SavePhoto(); }
        else { Debug.Log("Wait for responce"); }
    }

    IEnumerator GetVisionDataFromImages()
    {
        if (!isWaitForResponse)
        {
            isWaitForResponse = true; // XXX: ココから

            byte[] bytes = UnityEngine.Windows.File.ReadAllBytes(filePath);

            var headers = new Dictionary<string, string>() {
                { "Ocp-Apim-Subscription-Key", VISIONKEY },
                { "Content-Type", "application/octet-stream" }
            };

            WWW www = new WWW(emotionURL, bytes, headers);

            yield return www;
            string responseData = www.text; // Save the response as JSON string

            //Debug.Log(responseData);
            //GetComponent<ParseComputerVisionResponse>().ParseJSONData(responseData);
            response = responseData;

            onDataReceived.Invoke(response);

            isWaitForResponse = false; // XXX: ココからまで. 多重リクエスト禁止.
        }
    }

    private string MakeStatus()
    {
        string ret = "";
        ret += " auto: " + autoRequest;
        ret += " freq: "  + requestFrequency.ToString("f3");
        ret += " accum: " + accumCount;
        return ret;
    }
}