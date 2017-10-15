using MiniJSON;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class TextPrintTest : MonoBehaviour
{
    private Text mText;

    private Dictionary<string, object> JsonRead ( )
    {
        UnityEngine.Object[] objcts = Resources.LoadAll( "" );
        var textAsset = objcts[0] as TextAsset;
        Assert.IsNotNull( textAsset );
        //string jsonText = StringAjast( textAsset.text );
        string jsonText = textAsset.text;
        Dictionary<string, object> dic = Json.Deserialize( jsonText ) as Dictionary<string, object>;
        Assert.IsNotNull( dic );
        return dic;
    }

    private void Start ( )
    {
        mText = GetComponent<Text>();
        Assert.IsNotNull( mText );
        Dictionary<string, object> dic = JsonRead();

        //List<object> objList = JsonParser<List<object>>.Parse( dic, "array" );
        //foreach (object obj in objList)
        //{
        //    object data = JsonParser<object>.Parse( obj, "" );
        //    object score = JsonParser<object>.Parse( data, "scores" );
        //    double anger = JsonParser<double>.Parse( data, "anger" );
        //    Log.DebugThrow( "anger is" + anger );
        //}
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
            return ( T )obj;
        }

        public static T Parse ( object dic, string key )
        {
            Dictionary<string, object> d = dic as Dictionary<string, object>;
            Assert.IsNotNull( d );
            return Parse( dic, key );
        }
    }
}
