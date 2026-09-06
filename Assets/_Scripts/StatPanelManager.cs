using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[Serializable]
public struct TextStatUIBinding
{
    public TextMeshProUGUI UIText;
    public StatTypes statType;
}
public class StatPanelManager : MonoBehaviour
{
    [SerializeField] private StatManager statManager;
    [SerializeField] private Movement movement;
    public List<TextStatUIBinding> statBindings = new List<TextStatUIBinding>();
    private Dictionary<StatTypes, TextMeshProUGUI> statTextMap = new Dictionary<StatTypes, TextMeshProUGUI>();
    private void Awake()
    {
        statManager = GameManager.gm.Player.GetComponent<StatManager>();
        movement = GameManager.gm.Player.GetComponent<Movement>();
        foreach (var binding in statBindings)
        {
            if (binding.UIText != null && !statTextMap.ContainsKey(binding.statType))
            {
                statTextMap.Add(binding.statType, binding.UIText);
            }
        }
    }
    private void OnEnable()
    {
        statManager.OnHealthChanged += UpdateHPStat;
        statManager.OnStaminaChanged += UpdateStaminaStat;
        movement.OnMoveSpeedChanged += UpdateMoveSpeedStat;
        statManager.OnExhaustedStateChanged += UpdateExhaustedStat;
        statManager.OnDamageStatChanged += UpdateDamageStat;
        UpdateAllStats();
    }
    private void OnDisable()
    {
        statManager.OnHealthChanged -= UpdateHPStat;
        statManager.OnStaminaChanged -= UpdateStaminaStat;
        movement.OnMoveSpeedChanged -= UpdateMoveSpeedStat;
        statManager.OnExhaustedStateChanged -= UpdateExhaustedStat;
        statManager.OnDamageStatChanged -= UpdateDamageStat;
    }
    public void SetStatText(StatTypes type, string valueText)
    {
        if (statTextMap.TryGetValue(type, out TextMeshProUGUI targetTMP))
        {
            targetTMP.text = valueText;
        }
    }
    public void UpdateAllStats()
    {
        SetStatText(StatTypes.currentHP, "CurrentHP: " + statManager.currentHP + "/" + statManager.maxHP);
        SetStatText(StatTypes.currentDamage, "CurrentDamage: " + statManager.currentDamage);
        SetStatText(StatTypes.attackSpeed, "AttackSpeed: " + statManager.attackSpeed);
        SetStatText(StatTypes.Speed,"MovementSpeed: " + statManager.Speed);
        SetStatText(StatTypes.currentStamina,"Stamina: " + statManager.currentStamina + "/" + statManager.maxStamina);
        SetStatText(StatTypes.isExhausted, "Exhausted " + statManager.isExhausted);
    }
    private void UpdateHPStat(float current, float max)
    {
        SetStatText(StatTypes.currentHP, "CurrentHP: "+ current + " / " + max);
        SetStatText(StatTypes.maxHP,"MaxHP: " + max);   
    }
    private void UpdateStaminaStat(float current, float max)
    {
        SetStatText(StatTypes.currentStamina, "Stamina: " +  (int)current +  " / " + max);
    }
    private void UpdateMoveSpeedStat(float current)
    {
       SetStatText(StatTypes.Speed, "MovementSpeed: " + (int)current + " / " +  statManager.myData.movementStats.moveSpeed);
    }
    private void UpdateExhaustedStat(bool isExhausted)
    {
        SetStatText(StatTypes.isExhausted, "Exhausted " + isExhausted);
    }
    private void UpdateDamageStat(float current)
    {
        SetStatText(StatTypes.currentDamage, "CurrentDamage: " + current);
    }
}
