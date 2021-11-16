using System;
using UnityEngine;

public interface IDamageable : ITarget
{
    public Action OnDeath { get; set; }
    public void TakeDamage(int damage);
}
