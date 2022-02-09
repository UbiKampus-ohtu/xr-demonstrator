
[System.Serializable]
public class Colliders
{
    public Origin origin;
    public Dimensions dimensions;
}

[System.Serializable]
public class Origin
{
    public float x;
    public float y;
    public float z;
}

[System.Serializable]
public class Dimensions
{
    public float width;
    public float depth;
    public float height;
}