using MiniJSON;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class TextPrintTest : MonoBehaviour
{
    [SerializeField] private string text;
    [SerializeField] private EmotionTextView emotionTextView;

    private Text mText;
    private CognitiveCamera _CognitiveCamera;

    private string LoadSample ( )
    {
        //Object[] objcts = Resources.LoadAll( "" );
        //var textAsset = objcts[0] as TextAsset;
        //Assert.IsNotNull( textAsset );
        //string jsonText = textAsset.text;
        return text;
    }

    private static List<object> JsonRead ( string jsonText )
    {
        List<object> list = Json.Deserialize( jsonText ) as List<object>;
        return list;
    }

    public static PictureEmotionData Parser(string jsonText)
    {

        List<object> list = JsonRead(jsonText);
        var pem = new PictureEmotionData();
        var feceSubject = FaceRectangle.Subjects();
        var emoSubject = EmotionScore.Subjects();

        foreach (object obj in list)
        {
            var dic = obj as Dictionary<string, object>;
            Assert.IsNotNull( dic );
            var set = pem.AddSet();
            var face = set.Key;
            var emo = set.Value;
            var fd = JsonParser<Dictionary<string, object>>.Parse( dic, "faceRectangle" );
            var ed = JsonParser<Dictionary<string, object>>.Parse( dic, "scores" );
            foreach (var fs in feceSubject)
            {
                face.Set( fs.Key, (int)JsonParser<System.Int64>.ParseObj( fd, fs.Value ) );
            }
            foreach (var es in emoSubject)
            {
                emo.Set( es.Key, JsonParser<double>.ParseObj( ed, es.Value ) );
            }
            //Debug.Log( "face.Get(FaceRectangle.VALUE.height)" + face.Get( FaceRectangle.VALUE.height ) );
            //Debug.Log( "emo.Get(EmotionScore.VALUE.anger)" + emo.Get( EmotionScore.VALUE.anger ) );
        }

        Debug.Log(pem.Get().Count);
        return pem;
    }

    private void Start ( )
    {
        //mText = GetComponent<Text>();
        //Assert.IsNotNull( mText );
        //var jsonText = LoadSample();
        //Debug.Log(jsonText);
        _CognitiveCamera = GameObject.Find("_Emolens").GetComponent<CognitiveCamera>();
        Assert.IsNotNull(_CognitiveCamera);

        _CognitiveCamera.onDataReceived.AddListener(OnDataReceived);
    }
    

    private string StringAjast ( string jsonData )
    {
        return "{ \"array\": " + jsonData + "}";
    }

    public class JsonParser<T>
    {
        public static T Parse ( Dictionary<string, object> dic, string key )
        {
            Assert.IsTrue( dic.ContainsKey( key ) );
            object obj = dic[key];
            Assert.IsNotNull( obj );
            //Debug.Log( key );
            //Debug.Log( obj.GetType().Name );
            //Debug.Log( obj.ToString() );
            //Debug.Log( "" + obj );

            return ( T )obj;
        }

        public static T ParseObj ( object dic, string key )
        {
            Dictionary<string, object> d = dic as Dictionary<string, object>;
            Assert.IsNotNull( d );
            return Parse( d, key );
        }
    }

    private void OnDataReceived(string response)
    {
        var data = Parser(response);
        var list = data.Get();
        var numFaces = list.Count;

        if (numFaces > 0)
        {
            float[] mValue = new float[(int)EmotionScore.VALUE.SIZE];
            // zero clear
            for (var i = 0; i < mValue.Length; i++)
            {
                mValue[i] = 0.0f;
            }

            Debug.Log("list.Count: " + list.Count.ToString());
            foreach (var kvp in list)
            {
                Debug.Log("BBB");
                EmotionScore es = kvp.Value;
                for (var i = 0; i < mValue.Length; i++)
                {
                    var key = AY_Util.EnumUtil<EmotionScore.VALUE>.GetElement(i);
                    mValue[i] += es.Get(key);
                    Debug.Log(key.ToString() + ": " + mValue[i].ToString());
                }
            }
            for (var i = 0; i < mValue.Length; i++)
            {
                mValue[i] = mValue[i] / list.Count;
            }

            emotionTextView.updateEmotion(
                mValue[(int)EmotionScore.VALUE.anger],
                mValue[(int)EmotionScore.VALUE.contempt],
                mValue[(int)EmotionScore.VALUE.disgust],
                mValue[(int)EmotionScore.VALUE.fear],
                mValue[(int)EmotionScore.VALUE.happiness],
                mValue[(int)EmotionScore.VALUE.neutral],
                mValue[(int)EmotionScore.VALUE.sadness],
                mValue[(int)EmotionScore.VALUE.surprise]);
        }
    }
}
