using System;
using System.Collections.Generic;

[Serializable]
public class AnimalsDataStoreWrapper
{
    public AnimalsDataStoreWrapper()
    {
        animals_ = new List<AnimalsDataStore>();
    }
    
    public List<AnimalsDataStore> animals_;
}
