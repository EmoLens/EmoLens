using MiniJSON;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class TextPrintTest : MonoBehaviour
{
    private Text mText;

    private string LoadSample ( )
    {
        Object[] objcts = Resources.LoadAll( "" );
        var textAsset = objcts[0] as TextAsset;
        Assert.IsNotNull( textAsset );
        string jsonText = textAsset.text;
        return jsonText;

    }

    private List<object> JsonRead ( string jsonText )
    {
        List<object> list = Json.Deserialize( jsonText ) as List<object>;
        return list;
    }

    private PictureEmotionData Parser(List<object> list )
    {
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
            Debug.Log( "face.Get(FaceRectangle.VALUE.height)" + face.Get( FaceRectangle.VALUE.height ) );
            Debug.Log( "emo.Get(EmotionScore.VALUE.anger)" + emo.Get( EmotionScore.VALUE.anger ) );
        }
        return pem;
    }

    private void Start ( )
    {
        mText = GetComponent<Text>();
        Assert.IsNotNull( mText );
        var jsonText = LoadSample();

        List<object> list = JsonRead( jsonText );
        var pem = Parser( list );
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
            Debug.Log( key );
            Debug.Log( obj.GetType().Name );
            Debug.Log( obj.ToString() );
            Debug.Log( "" + obj );

            return ( T )obj;
        }

        public static T ParseObj ( object dic, string key )
        {
            Dictionary<string, object> d = dic as Dictionary<string, object>;
            Assert.IsNotNull( d );
            return Parse( d, key );
        }
    }
}
