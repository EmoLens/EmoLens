using System.Collections.Generic;
using AY_Util;

public class FaceRectangle
{
    private float[] mValue = new float[( int )VALUE.SIZE];

    public enum VALUE
    {
        top, left, width, height, SIZE
    }

    public float Get ( VALUE key )
    {
        return mValue[( int )key];
    }

    public FaceRectangle Set ( VALUE key, double value )
    {
        mValue[( int )key] = ( float )value;
        return this;
    }

    public static KeyValuePair<VALUE,string>[] Subjects ( )
    {
        KeyValuePair<VALUE, string>[] arr = new KeyValuePair<VALUE, string>[(int)VALUE.SIZE];
        for (int i = 0; i < ( int )VALUE.SIZE; i++)
        {
            arr[i] = new KeyValuePair<VALUE, string>(
                EnumUtil<VALUE>.GetElement( i ),
                EnumUtil<VALUE>.GetString( i ) );
        }

        return arr;
    }

}
