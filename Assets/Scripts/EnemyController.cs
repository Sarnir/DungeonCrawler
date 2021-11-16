using System.Collections.Generic;
using UnityEngine;

public class EnemyController : CharacterController
{
    [SerializeField] private float aggroRadius;
    
    private HeroController hero;

    protected override void OnUpdate()
    {
        UpdateMovement();
    }
    
    protected override void OnInitialize()
    {
        hero = FindObjectOfType<HeroController>();
    }

    void UpdateMovement()
    {
        if (!(currentAction is AttackAction))
        {
            float distance;
            var targetList = new List<IDamageable>();
            targetList.Add(hero);
            targetList.AddRange(hero.Minions);
            IDamageable target = GetClosestTargetInRange(targetList, out distance);

            if (target != null && distance < aggroRadius)
            {
                ClearAction();
                SetAction(new AttackAction(this, target, 1f));
            }
        }
        else if (!(currentAction is MoveAction))
        {
            //Idle();
        }
    }

    IDamageable GetClosestTargetInRange(List<IDamageable> targets, out float minDistance)
    {
        IDamageable enemy = null;
        minDistance = float.MaxValue;
        
        for (int i = 0; i < targets.Count; i++)
        {
            var distance = (this.Position - targets[i].Position).magnitude;
            if (distance < minDistance)
            {
                minDistance = distance;
                enemy = targets[i];
            }
        }

        return enemy;
    }
}
