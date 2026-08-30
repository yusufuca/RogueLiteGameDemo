using NUnit.Framework;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PerkTreeManager : MonoBehaviour
{
    [SerializeField]private StatManager StatManager;
    [SerializeField] public GameObject descriptionTextGameObject;
    [SerializeField] public TextMeshProUGUI descriptionText;

    public List<Perk_SO> playerPerks = new List<Perk_SO>();
    public event Action <Perk_SO> OnPerkAdded;


    private void Awake()
    {
        StatManager = GameObject.Find("Player").GetComponent<StatManager>();
    }
    void Start()
    {
        
        
    }

    void Update()
    {
        MapHandler();
    }
    private void MapHandler()
    {
       
       

    }
    public bool TryToAddPerk(Perk_SO perk,Image img)
    {
        if (StatManager.currentCoin > perk.cost) 
        {
            for(int i = 0; i < perk.requiredPerks.Count; i++)
            {
                if (!playerPerks.Contains(perk.requiredPerks[i]))
                {
                    Debug.Log("Should unlock this " + perk.requiredPerks[i]);
                    return false;
                }
            }
            playerPerks.Add(perk);
            OnPerkAdded.Invoke(perk);
            StatManager.currentCoin -= perk.cost;
            img.color = Color.green;
            return true;
        }
        else
        {
            
            img.color = Color.red;
            Debug.Log("Not Enough Coin");
            return false;
        }

    }
    private void AddPerk(Perk_SO perk)
    {
        
       
    }
}
