using System;

[Serializable]
public class AnimalsDataStore
{
    public AnimalsDataStore(AnimalScript.AnimalType type, float x, float y, float z)
    {
        _type = (int)type;
        _pos_x = x;
        _pos_y = y;
        _pos_z = z;
    }
    
    public int _type;
    public float _pos_x;
    public float _pos_y;
    public float _pos_z;
}
