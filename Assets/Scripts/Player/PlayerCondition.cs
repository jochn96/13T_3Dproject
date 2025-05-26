using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageIbe
{
    void TakePhysiclaDamage(int damage);
}

public class PlayerCondition : MonoBehaviour, IDamageIbe
{
    public UICondition uICondition;

    Condition health { get { return uICondition.health; } }
    Condition hunger { get { return uICondition.hunger; } }
    Condition stamina { get { return uICondition.stamina; } }

    public float noHungerHealthDecay;

    public event Action onTakeDamage;
    private void Update()
    {
        hunger.Subject(hunger.passiveValue * Time.deltaTime);
        stamina.Add(stamina.passiveValue * Time.deltaTime);

        if(hunger.curValue <= 0f)
        {
            health.Subject(noHungerHealthDecay * Time.deltaTime);
        }

        if(health.curValue <= 0f)
        {
            Die();
        }
    }

    public void Heal(float amout)
    {
        health.Add(amout);
    }

    public void Eat(float amout)
    {
        hunger.Add(amout);
    }
    public void Die()
    {
        Debug.Log("die");
    }

    public void TakePhysiclaDamage(int damage)
    {
        health.Subject(damage);
        onTakeDamage?.Invoke();
    }

    public bool UseStamina(float amount)
    {
        if(stamina.curValue -  amount < 0f)
        {
            return false;
        }
        stamina.Subject(amount);
        return true;
    }
}
