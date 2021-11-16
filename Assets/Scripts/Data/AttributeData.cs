using System;
using UnityEngine;

public enum AttributeType
{
    Health,
    Fatigue
}

[System.Serializable]
public class AttributeData
{
    public AttributeType Type;
    public int maxValue;
    public float YOffset;
    public AttributeBar prefab;
}