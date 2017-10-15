public class EmotionScore
{
    public float[] value = new float[( int )VALUE.SIZE];

    public enum VALUE
    {
        ANGER, CONTEMPT, DISGUST, FEAR, HAPPINESS, NATURAL, SADNESS, SURPRISE, SIZE
    }

    public float Get ( VALUE key )
    {
        return value[( int )key];
    }
}
