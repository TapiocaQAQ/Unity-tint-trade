using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item
{
    public Type type;
    public int count;
    public enum Type
    {
        money,
        food,
        wood,
        iron,
        nutrient
    }

    public Item(Type type, int count)
    {
        this.type = type;
        this.count = count;
    }
}
