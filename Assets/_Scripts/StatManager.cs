using System;
using System.Runtime.CompilerServices;
using UnityEngine;
public enum StatTypes 
{
    currentHP,
    maxHP,
    currentDamage,
    initialDamage,
    attackSpeed,
    Speed,
    jumpHeight,
    currentStamina,
    maxStamina,
    isExhausted,

}
public class StatManager : MonoBehaviour
{

    public event Action<float, float> OnHealthChanged;
    public event Action<float, float> OnStaminaChanged;
    public event Action<float> OnMoveSpeedChanged;
    public event Action<bool> OnExhaustedStateChanged;
    public event Action<float> OnDamageStatChanged;
    public event Action<float, float> OnDamageTaken;

    [Header("-----REFERENCES-----")]
    public Entity_Types myData;
    public PerkTreeManager perkTreeManager;
    public PlayerMovement movement;

    [Header("-----HP STATS-----")]
    public float currentHP;
    public float maxHP;

    [Header("-----MOVEMENT STATS-----")]
    public float Speed;
    public float jumpHeight;
    public float maxStamina;
    public float currentStamina;
    public bool isExhausted;
    public int dashStaminaCost;

    [Header("-----COMBAT-----")]
    public LayerMask mask;
    public float initialDamage;
    public float currentDamage;
    public float attackSpeed;


    public int currentCoin;
    private void Awake()
    {
        perkTreeManager = FindFirstObjectByType<PerkTreeManager>();
        movement = FindAnyObjectByType<PlayerMovement>();
    }
    private void OnEnable()
    {
       perkTreeManager.OnPerkAdded += AddPerk;
        movement.OnDashing += Dashing;
    }
    private void OnDisable()
    {
        perkTreeManager.OnPerkAdded -= AddPerk;
        movement.OnDashing -= Dashing;
    }
    private void Start()
    {
        // HP STATS

        currentHP = myData.healthStats.maxHealth;
        maxHP = myData.healthStats.maxHealth;

        // MOVEMENT STATS

        Speed = myData.movementStats.moveSpeed;
        jumpHeight = myData.movementStats.jumpHeight;
        maxStamina = myData.movementStats.Stamina;
        currentStamina = myData.movementStats.Stamina;

        // COMBAT STATS

        initialDamage = myData.attackStats.damage;
        currentDamage = myData.attackStats.damage;
        attackSpeed = myData.attackStats.attackSpeed;

        currentCoin = myData.economyStats.startCoin;

        OnHealthChanged?.Invoke(currentHP, maxHP);

    }

    public void ApplyDamage(float damage)
    {
        currentHP -= damage;

        OnHealthChanged?.Invoke(currentHP, maxHP); 
        OnDamageTaken?.Invoke(damage, currentHP);
        if(currentHP < 0)
        {
            Die();
        }
    }
    public void Sprint(bool isSprinting)
    {
        if (isSprinting)
        {
            if (!isExhausted)
            {
                currentStamina -= Time.deltaTime * 5;
                if (currentStamina <= 0)
                {
                    currentStamina = 0;
                    isExhausted = true;
                }
            }
            else
            {
                currentStamina += Time.deltaTime * 5;

                if (currentStamina >= maxStamina * 0.20f)
                {
                    isExhausted = false;
                }
            }
        }
        else
        {
            if (currentStamina < maxStamina)
            {
                currentStamina += Time.deltaTime * 5;

            }
            if (isExhausted && currentStamina >= maxStamina * 0.20f)
            {
                isExhausted = false;
            }
        }
        OnStaminaChanged?.Invoke(currentStamina, maxStamina);
        OnExhaustedStateChanged?.Invoke(isExhausted);
    }
    public void Dashing()
    {
        currentStamina -= dashStaminaCost;
        OnStaminaChanged?.Invoke(currentStamina,maxStamina);
    }
    public void AddPerk(Perk_SO perk)
    {
        switch (perk.perkType)
        {
            case perkTypes.Deffence:
                Debug.Log("Added perk name is: " + perk.name);
                break;
            case perkTypes.Mobility:
                Debug.Log("Added perk name is: " + perk.name);
                break;
            case perkTypes.Damage:
                Debug.Log("Added perk name is: " + perk.name);
                currentDamage += perk.value;
                OnDamageStatChanged?.Invoke(currentDamage);
                break;
        }
    }

    public void Die()
    {

        GameManager.gm.ChangeScore(myData.scoreValue);
        Destroy(gameObject);
  
    }
}
