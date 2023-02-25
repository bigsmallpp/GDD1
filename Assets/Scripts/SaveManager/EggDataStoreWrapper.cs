using System;
using System.Collections.Generic;

[Serializable]
public class EggDataStoreWrapper
{
    public EggDataStoreWrapper()
    {
        eggs_ = new List<EggDataStore>();
    }
    
    public List<EggDataStore> eggs_;
}
