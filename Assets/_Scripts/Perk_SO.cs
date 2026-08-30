using System.Collections.Generic;
using UnityEngine;
public enum perkTypes
{
    Damage,
    Deffence,
    Mobility
}
[CreateAssetMenu(fileName = "Perk_SO", menuName = "Scriptable Objects/Perk_SO")]
public class Perk_SO : ScriptableObject
{
    public perkTypes perkType;
    public int cost;
    public int value;
    public List<Perk_SO> requiredPerks = new List<Perk_SO>();
    [TextArea(5,10)]
    public string description;


    private const int MaxDescriptionLength = 100;
    private void OnValidate()
    {
        if(description.Length > MaxDescriptionLength)
        {
            description.Substring(0,MaxDescriptionLength);
            Debug.Log(name + " Description cutted");
        }
    }
}
