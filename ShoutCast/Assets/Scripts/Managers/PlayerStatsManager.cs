using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;




public class PlayerStatsManager : MonoBehaviour
{
    private static PlayerStatsManager instance;
    
    //[Header("Player Stats Settings")]
    [SerializeField] private Image healthBar, manaBar, staminaBar;
    [SerializeField] private TextMeshProUGUI healthNumbers, manaNumbers, staminaNumbers;
    [SerializeField] private Image staminaObject;
    
   
    
    //[Header("Player Magic Settings")]
    // Magic Ranks
    public SpellRanks fireRank { get; private set; } = SpellRanks.Novice; 
    public SpellRanks waterRank { get; private set; } = SpellRanks.Novice;
    public SpellRanks earthRank { get; private set; } = SpellRanks.Novice;
    public SpellRanks windRank { get; private set; } = SpellRanks.Novice;

    public int fireLevel = 1;
    public int waterLevel = 1;
    public int earthLevel = 1;
    public int windLevel = 1;
    
    private Dictionary<SpellRanks, int> RankRequirements = new Dictionary<SpellRanks, int>()
    {
        // Key is level requirement for rank promotion, value is spell rank
        { SpellRanks.Novice, 1 },
        { SpellRanks.Intermediate, 15 },
        { SpellRanks.Advanced, 30 },
        { SpellRanks.Saint, 60 },
        { SpellRanks.King, 120 },
        { SpellRanks.Imperial, 240 },
        { SpellRanks.God, 500 }
    };
    
    // Player stamina
    private float staminaDrainRate = 10f;
    private float staminaRegenRate = 10f;

    // Player stats
    private int healthLevel = 1;

    public void CheckRankStatus(Elements type, SpellRanks currentRank)
    {
        Debug.Log("Evaluating Rank");
        Debug.Log("Current Rank: " + fireRank);

        int levelValue = GetMagicTypeLevel(type);
        int levelRequirement = 0;
        SpellRanks newRank = SpellRanks.Novice;
        
        foreach (var key in RankRequirements.Keys) // loop through keys
        {
            if (currentRank == key)
            {
                newRank = key + 1;
                Debug.Log("Next Rank:" + newRank);
                levelRequirement = RankRequirements[newRank];
                Debug.Log("int level:" + levelRequirement);
            }
        }

        if (newRank == SpellRanks.Novice || levelRequirement == 0)
        {
            // Exit if no rank was found or levelRequirement was not set
            return;
        }
        
        
        if (levelValue == levelRequirement)
        {
            // promote rank if level req is met
            Debug.Log(type + " proficiency has increased to " + newRank);

            // Get what element's level 
            switch (type)
            {
                case Elements.Fire:
                    fireRank = newRank;
                    break;
                    
                case Elements.Water:
                    waterRank = newRank;
                    break;
                    
                case Elements.Earth:
                    earthRank = newRank;
                    break;
                    
                case Elements.Wind:
                    windRank = newRank;
                    break;
            }
        }
        else
        {
            Debug.Log("No promotion in " + type);
        }
    }

    public int GetMagicTypeLevel(Elements type)
    {
        switch (type)
        {
            case Elements.Fire:
                return fireLevel;
            
            case Elements.Water:
                return waterLevel;
            
            case Elements.Earth:
                return earthLevel;
            
            case Elements.Wind:
                return windLevel;
            
            default:
                Debug.Log("Error no elemental magic type found");
                return 0;
        }
    }

    public int GetSpellXP(SpellRanks rank)
    {
        Debug.Log("spell rank: " + rank);
        
        switch (rank)
        {
            case SpellRanks.Novice:
                Debug.Log("Getting XP: " + (int)SpellRankXP.Novice);
                return (int)SpellRankXP.Novice;
            
            case SpellRanks.Intermediate:
                Debug.Log("Getting XP: " + (int)SpellRankXP.Intermediate);
                return (int)SpellRankXP.Intermediate;
            
            case SpellRanks.Advanced:
                Debug.Log("Getting XP: " + (int)SpellRankXP.Advanced);
                return (int)SpellRankXP.Advanced;
            
            case SpellRanks.Saint:
                Debug.Log("Getting XP: " + (int)SpellRankXP.Saint);
                return (int)SpellRankXP.Saint;
            
            case SpellRanks.King:
                Debug.Log("Getting XP: " + (int)SpellRankXP.King);
                return (int)SpellRankXP.King;
            
            case SpellRanks.Imperial:
                Debug.Log("Getting XP: " + (int)SpellRankXP.Imperial);
                return (int)SpellRankXP.Imperial;
            
            case SpellRanks.God:
                Debug.Log("Getting XP: " + (int)SpellRankXP.God);
                return (int)SpellRankXP.God;
            
            default:
                Debug.Log("Getting XP: 0");
                return 0;
        }
    }
    
    // Current values
    private float currentHealth, maxHealth, currentMana, maxMana, currentStamina, maxStamina;

    private PlayerStatsManager()
    {
        instance = this;
    }

    public static PlayerStatsManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new PlayerStatsManager();
            }

            return instance;
        }
    }

    private void Start()
    {
        // Set levels
        healthLevel = 1;

        // Set player values
        maxHealth = 10 + (healthLevel * 10);
        currentHealth = maxHealth;
        maxMana = 20;
        //maxMana = Random.Range(20, 100);
        currentMana = maxMana;

        currentStamina = 100;
        maxStamina = currentStamina;

        // Set health and mana values for text
        healthNumbers.text = currentHealth.ToString() + " / " + maxHealth;
        manaNumbers.text = currentMana.ToString() + " / " + maxMana;
        staminaNumbers.text = currentStamina.ToString() + " / " + maxStamina;
        
        healthBar.fillAmount = currentHealth / maxHealth;
        manaBar.fillAmount = currentMana / maxMana;
        staminaBar.fillAmount = currentStamina / maxStamina;
        
        // Hide stamina bar
        //staminaObject.color = new Color(1, 1, 1, 0);
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        
        healthNumbers.text = currentHealth.ToString() + " / " + maxHealth;
        
        healthBar.fillAmount = currentHealth / maxHealth;
    }
    
    public void UseMana(float manaCost)
    {
        if (currentMana - manaCost < 0)
        {
            return;
        }
        currentMana -= manaCost;
        
        manaNumbers.text = currentMana.ToString() + " / " + maxMana;
        
        manaBar.fillAmount = currentMana / maxMana;
    }

    public float GetCurrentMana()
    {
        return currentMana;
    }

    public void Heal(float healingAmount)
    {
        currentHealth += healingAmount;
        currentHealth = Mathf.Clamp(currentHealth, 0, 100);
        
        healthNumbers.text = currentHealth.ToString() + " / " + maxHealth;

        healthBar.fillAmount = currentHealth / maxHealth;
    }
    
    public void RestoreMana(float amount)
    {
        currentMana += amount;
        //currentMana = Mathf.Clamp(currentMana, 0, 100);
        
        manaNumbers.text = currentMana.ToString() + " / " + maxMana;

        manaBar.fillAmount = currentMana / maxMana;
    }

    public void RegenStamina()
    {
        if (currentStamina >= maxStamina)
        {
            currentStamina = 100;
            return;
        }
        currentStamina += Time.deltaTime * staminaRegenRate;
        
        staminaNumbers.text = currentStamina.ToString() + " / " + maxStamina;
        
        staminaBar.fillAmount = currentStamina / 100;
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Cursor.visible)
            {
                Cursor.visible = false;
            }
            else
            {
                Cursor.visible = true;
            }
        }
        
        if (Input.GetKeyDown(KeyCode.P))
        {
            TakeDamage(1);
        }
        
        if (Input.GetKeyDown(KeyCode.O))
        {
            Heal(1);
        }
        
        
    }

    public void Run()
    {
        if (currentStamina <= 0)
        {
            return;
        }
        currentStamina -= Time.deltaTime * staminaDrainRate;
        
        staminaNumbers.text = currentStamina.ToString() + " / " + maxStamina;
        
        staminaBar.fillAmount = currentStamina / maxStamina;
    }

    public float GetStamina()
    {
        return currentStamina;
    }
    
    public float GetMaxStamina()
    {
        return maxStamina;
    }
}
