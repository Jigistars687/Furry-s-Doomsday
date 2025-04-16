using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_stats
{
    [SerializeField] private float _maxHealth = 100;
    [SerializeField] private float _health;
    string CURRENT_HEALTH = "CurrentHealth";

    public event Action HealthChanger;

    public Player_stats()
    {
        _health = _maxHealth;

    }

    public float MaxHealth
    {
        get { return _maxHealth; }
        set { _maxHealth = value; }
    }

    public float Health
    {
        get { return _health; }
        set
        {
            _health = value;
            PlayerPrefs.SetFloat(CURRENT_HEALTH, value);
            HealthChanger?.Invoke();
        }
    }
    public void TakeDamage(float damage)
    {
        if (Health > 0)
        {
            Health -= damage;
            HealthChanger?.Invoke();
        }
        if (Health < 0)
        {
            Health = 0;
            HealthChanger?.Invoke();
        }
    }
    public void Heal(float heal)
    {
        if(Health + heal >= MaxHealth)
        {
            Health = MaxHealth;
        }
        if (Health >= MaxHealth)
        {
            Health = MaxHealth;
            HealthChanger?.Invoke();
        }
        else
        {
            Health += heal;
            HealthChanger?.Invoke();
        }
    }
}
