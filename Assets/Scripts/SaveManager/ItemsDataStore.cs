using System;

[Serializable]
public class ItemsDataStore
{
    public ItemsDataStore(int type, int amount, int price)
    {
        _type = type;
        _amount = amount;
        _price = price;
    }
    
    public int _type;
    public int _amount;
    public int _price;
}
