using System.Collections.Generic;
using UnityEngine;

public class PictureEmotionData
{

    private List<KeyValuePair<FaceRectangle, EmotionScore>> mDataSets = new List<KeyValuePair<FaceRectangle, EmotionScore>>();

    private float Stamp { set; get; }

    public PictureEmotionData ( )
    {
        this.Stamp = Time.time;
    }

    /// <summary>
    /// データセットを追加して、追加したデータセットのインスタンスでもどします。
    /// </summary>
    /// <returns>作成されたデータセットのインスタンスです</returns>
    public KeyValuePair<FaceRectangle, EmotionScore> AddSet ( )
    {
        var data = new KeyValuePair<FaceRectangle, EmotionScore>(new FaceRectangle(), new EmotionScore());
        mDataSets.Add(data);
        return data;
    }

    public List<KeyValuePair<FaceRectangle, EmotionScore>> Get()
    {
        return mDataSets;
    }

}