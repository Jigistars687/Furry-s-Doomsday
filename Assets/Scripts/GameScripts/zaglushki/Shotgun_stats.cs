using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun_stats
{
    [SerializeField] private float _damagePerDrob = 10;
    public float DamagePerPellet => _damagePerDrob;
}
