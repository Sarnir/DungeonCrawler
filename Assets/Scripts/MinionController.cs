using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class MinionController : CharacterController
{
    private HeroController hero;

    private static List<MinionController> allMinions = new List<MinionController>();
    private List<EnemyController> enemiesInRange = new List<EnemyController>();
    
    protected override void OnInitialize()
    {
        base.OnInitialize();
        
        hero = FindObjectOfType<HeroController>();
        allMinions.Add(this);
        
        CharacterPanels.CreateCharacterPanel(this);
    }

    protected override void OnUpdate()
    {
        Separate(allMinions.Select(x => x.Position).ToArray());
        UpdateMovement();
    }

    private void OnDestroy()
    {
        allMinions.Remove(this);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var enemy = other.gameObject.GetComponent<EnemyController>();
        if (enemy != null)
        {
            enemy.OnDeath += () =>
            {
                if(enemiesInRange.Contains(enemy))
                    enemiesInRange.Remove(enemy);
            };
            enemiesInRange.Add(enemy);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        var enemy = other.gameObject.GetComponent<EnemyController>();
        if (enemy != null)
        {
            enemiesInRange.Remove(enemy);
        }
    }

    void UpdateMovement()
    {
        EnemyController enemy = null;
        float minDistance = float.MaxValue;
        
        for (int i = 0; i < enemiesInRange.Count; i++)
        {
            var enemyToHeroDistance = (hero.Position - enemiesInRange[i].Position).magnitude;
            if (enemyToHeroDistance < minDistance)
            {
                minDistance = enemyToHeroDistance;
                enemy = enemiesInRange[i];
            }
        }
        
        if(currentAction is AttackAction)
            return;

        if (!(currentAction is AttackAction) && enemy != null)
        {
            ClearAction();
            SetAction(new AttackAction(this, enemy, 1f));
        }
        else if (currentAction is MoveAction moveAction)
        {
            moveAction.UpdateTarget(new Vector2Target(hero.GetFormationPosition(this)));
        }
        else
        {
            var formationPos = hero.GetFormationPosition(this);
            Vector2 desired = formationPos - Position;
            
            if(desired.magnitude > 0.1f)
                SetAction(new MoveAction(this, new Vector2Target(hero.GetFormationPosition(this)), 0.1f));
        }
    }
}
