using System;
using System.Collections.Generic;

[Serializable]
public class StoreDataStore
{
    public StoreDataStore()
    {
        chickenAvailable = true;
        cowAvailable = true;
        sheepAvailable = true;
        lampAvailable = true;
        fishingRodAvailable = true;
        bucketAvailable = true;
        scissorAvailable = true;
    }

    public StoreDataStore(StoreDataStore store)
    {
        chickenAvailable = store.chickenAvailable;
        cowAvailable = store.cowAvailable;
        sheepAvailable = store.sheepAvailable;
        lampAvailable = store.lampAvailable;
        fishingRodAvailable = store.fishingRodAvailable;
        bucketAvailable = store.bucketAvailable;
        scissorAvailable = store.scissorAvailable;
    }
    
    public bool chickenAvailable;
    public bool cowAvailable;
    public bool sheepAvailable;
    public bool lampAvailable;
    public bool fishingRodAvailable;
    public bool bucketAvailable;
    public bool scissorAvailable;
}
