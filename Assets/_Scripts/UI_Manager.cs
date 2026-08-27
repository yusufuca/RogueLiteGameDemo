using NUnit.Framework;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UI_Manager : MonoBehaviour
{
    public Image hpBar;
    public Image staminaBar;
    public GameObject player;
    Attack playerAttackScript;
    Movement movementScript;
    public GameObject Container;
    // skills
    public List<GameObject> skillContainer = new List<GameObject>();
  
    private List<Skills> playerSkills = new List<Skills>();
    void Start()
    {
        player = GameObject.Find("Player");
        playerAttackScript = player.GetComponent<Attack>();
        movementScript = player.GetComponent<Movement>();
    }

    // Update is called once per frame
    void Update()
    {
        hpBar.fillAmount = Mathf.Clamp01(playerAttackScript.currentHP / playerAttackScript.maxHP);
        staminaBar.fillAmount = Mathf.Clamp01(movementScript.currentStamina / movementScript.maxStamina);
        UpdateSkillContainer();
    }
    private void UpdateSkillContainer()
    {
        playerSkills = player.GetComponent<SkillManager>().mySkills;
        for(int i = 0; i < playerSkills.Count; i++)
        {
            if (skillContainer[i].name == playerSkills[i].imagePrefab.name + "(Clone)")
            {
                continue; 
            }
            Vector2 spawnPos = skillContainer[i].transform.position;
            Transform parentTransform = skillContainer[i].transform.parent;
            GameObject newSlot = Instantiate(playerSkills[i].imagePrefab, parentTransform);
            newSlot.transform.position = spawnPos;

            Destroy(skillContainer[i]);
            skillContainer.RemoveAt(i);            
            skillContainer.Insert(i,newSlot);
            
        }
    }
}
