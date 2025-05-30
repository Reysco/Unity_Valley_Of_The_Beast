using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Stat
{
    public int maxVal;
    public int currVal;

    public Stat(int curr, int max)
    {
        maxVal = max;
        currVal = curr;
    }

    internal void Subtract(int amount)
    {
        currVal -= amount;
    }

    internal void Add(int amount)
    {
        currVal += amount;
        if (currVal > maxVal) { currVal = maxVal; }
    }

    internal void SetToMax()
    {
        currVal = maxVal;
    }
}

public class Character : MonoBehaviour
{
    public Stat hp;
    [SerializeField] StatusBar hpBar;
    public Stat stamina;
    [SerializeField] StatusBar staminaBar;

    public bool isDead;
    public bool isExhausted;


    private void Start()
    {
        UpdateHPBar();
        UpdateStaminaBar();
    }

    public void TakeDamage(int amount)
    {
        hp.Subtract(amount);
        if(hp.currVal <= 0)
        {
            isDead = true;
        }
        UpdateHPBar();
    }

    public void UpdateHPBar()
    {
        hpBar.Set(hp.currVal, hp.maxVal);
    }

    public void UpdateStaminaBar()
    {
        staminaBar.Set(stamina.currVal, stamina.maxVal);
    }

    public void Heal(int amount)
    {
        hp.Add(amount);
        UpdateHPBar();
    }

    public void FullHeal()
    {
        hp.SetToMax();
        UpdateHPBar();
    }

    public void GetTired(int amount)
    {
        stamina.Subtract(amount);
        if(stamina.currVal < 0)
        {
            isExhausted = true;
        }
        UpdateStaminaBar();
    }

    public void Rest(int amount)
    {
        stamina.Add(amount);
        UpdateStaminaBar();
    }

    public void FullRest(int amount)
    {
        stamina.SetToMax();
        UpdateStaminaBar();
    }
}
