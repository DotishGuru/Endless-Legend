using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarsUIBehaviour : MonoBehaviour
{
    [SerializeField]
    private Slider HealthBar;
    [SerializeField]
    private Slider ManaBar;
    [SerializeField]
    private Slider ExperienceBar;
    [SerializeField]
    private Slider StaminaBar;
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void SetHealth(float currHealth, float maxHealth)
    {
        if(HealthBar.maxValue != maxHealth)
        {
            HealthBar.maxValue = maxHealth;
        }
        
        if(HealthBar.value != currHealth)
        {
            HealthBar.value = currHealth;
        }        
    }

    public void SetMana(float currMana, float maxMana)
    {
        if(ManaBar.maxValue != maxMana)
        {
            ManaBar.maxValue = maxMana;
        }

        if(ManaBar.value != currMana)
        {
            ManaBar.value = currMana;
        }        
    }

    public void SetExperience(float currExp, float maxExp)
    {
        if(ExperienceBar.maxValue != maxExp)
        {
            ExperienceBar.maxValue = maxExp;
        }

        if(ExperienceBar.value != currExp)
        {
            ExperienceBar.value = currExp;
        }        
    }

    public void SetStamina(float currStamina, float maxStamina)
    {
        if(StaminaBar.maxValue != maxStamina)
        {
            StaminaBar.maxValue = maxStamina;
        }

        if(StaminaBar.value != currStamina)
        {
            StaminaBar.value = currStamina;
        }        
    }
}
