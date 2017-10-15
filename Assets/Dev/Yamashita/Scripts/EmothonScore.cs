using AY_Util;
using System.Collections.Generic;

public class EmotionScore
{
    private float[] mValue = new float[( int )VALUE.SIZE];

    public enum VALUE
    {
        anger, contempt, disgust, fear, happiness, neutral, sadness, surprise, SIZE
    }

    public static KeyValuePair<VALUE, string>[] Subjects ( )
    {
        KeyValuePair<VALUE, string>[] arr = new KeyValuePair<VALUE, string>[( int )VALUE.SIZE];

        for (int i = 0; i < ( int )VALUE.SIZE; i++)
        {
            arr[i] = new KeyValuePair<VALUE, string>(
                EnumUtil<VALUE>.GetElement( i ),
                EnumUtil<VALUE>.GetString( i ) );
        }
        return arr;
    }

    public float Get ( VALUE key )
    {
        return mValue[( int )key];
    }

    public EmotionScore Set ( VALUE key, double value )
    {
        mValue[( int )key] = ( float )value;
        return this;
    }
}
