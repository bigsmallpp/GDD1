using System;

[Serializable]
public class EggDataStore
{
    public EggDataStore(float x, float y)
    {
        _pos_x = x;
        _pos_y = y;
    }
    
    public float _pos_x;
    public float _pos_y;
}
