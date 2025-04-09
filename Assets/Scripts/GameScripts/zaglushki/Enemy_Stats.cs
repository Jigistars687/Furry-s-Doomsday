using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Stats : MonoBehaviour
{
    [SerializeField] private float _maxHealth = 60;
     [SerializeField] private float _damagePerTick = 1;
    public float DamagePerTick => _damagePerTick;
    private float _health;

    public event Action HealthChanger;
    public Enemy_Stats()
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
        set { _health = value; }
    }

    public void TakeDamage(float damage)
    {
        if (Health > 0)
        {
            Health -= damage;
            HealthChanger?.Invoke();
        }
    }
}
