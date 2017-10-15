public class FaceRectangle
{
    public float[] value = new float[( int )VALUE.SIZE];

    public enum VALUE
    {
        TOP, LEFT, WIDTH, HEIGHT, SIZE
    }

    public float Get ( VALUE key )
    {
        return value[( int )key];
    }
}
