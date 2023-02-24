using System;
using System.Collections.Generic;

[Serializable]
public class PlantsDataStoreWrapper
{
    public PlantsDataStoreWrapper(List<PlantsDataStore> plants)
    {
        _plants = plants;
    }

    public PlantsDataStoreWrapper()
    {
        _plants = new List<PlantsDataStore>();
    }

    public List<PlantsDataStore> _plants;
}