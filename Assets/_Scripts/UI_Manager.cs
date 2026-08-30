using NUnit.Framework;
using System;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UI_Manager : MonoBehaviour
{


    public Image hpBar;
    public Image staminaBar;
    public GameObject player;
    [SerializeField] private StatManager statManager;
    public GameObject Container;
    public GameObject perkTree;
    public GameObject statPanel;
    

    // skills
    public List<GameObject> skillContainer = new List<GameObject>();
  
    private List<Skills> playerSkills = new List<Skills>();
    CanvasGroup perkTreeGroup;
    private void Awake()
    {
        player = GameObject.Find("Player");
        if (player != null)  statManager = player.GetComponent<StatManager>();

        
    }

    private void OnEnable()
    {
        if (statManager != null)
        {

            statManager.OnHealthChanged += UpdateHPSlider;
            statManager.OnStaminaChanged += UpdateStaminaSlider;

        }
    }

    private void OnDisable()
    {
        if (statManager != null)
        {
            statManager.OnHealthChanged -= UpdateHPSlider;
            statManager.OnStaminaChanged -= UpdateStaminaSlider;

        }
    }

    void Start()
    {
        perkTreeGroup = perkTree.GetComponent<CanvasGroup>();
       
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (perkTreeGroup.alpha != 0)
            {
                perkTreeGroup.alpha = 0;
                perkTreeGroup.blocksRaycasts = false;
                perkTreeGroup.interactable = false;
            }
            else
            {
                perkTreeGroup.alpha = 1;
                perkTreeGroup.blocksRaycasts = true;
                perkTreeGroup.interactable = true;
            }
        }
        if (Input.GetKeyDown(KeyCode.BackQuote))
        {
            if(statPanel.activeSelf)
            {
                statPanel.SetActive(false);
            }
            else
            {
                statPanel.SetActive(true);
            }
        }

       
       
        UpdateSkillContainer();

    }

    private void UpdateHPSlider(float current, float max)
    {
        hpBar.fillAmount = Mathf.Clamp01(current / max);
    }
    private void UpdateStaminaSlider(float current, float max)
    {
        staminaBar.fillAmount = Mathf.Clamp01(current / max);
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
