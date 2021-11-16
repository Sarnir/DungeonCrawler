using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "charData", menuName = "Data", order = 1)]
public class CharacterData : ScriptableObject
{
    [Header("Stats")]
    public int Accuracy;
    public int MaxHealth;
}