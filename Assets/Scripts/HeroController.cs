using System;
using System.Collections.Generic;
using UnityEngine;

public class HeroController : CharacterController
{
    [SerializeField] private MinionController minionPrefab;
    [SerializeField] private Projectile projectilePrefab;
    [SerializeField] private int minionsCap = 3;

    [SerializeField] private Vector2[] formationPositions;

    private List<MinionController> minions = new List<MinionController>();

    public List<MinionController> Minions => minions;

    protected override void OnInitialize()
    {
        base.OnInitialize();
        
        CharacterPanels.CreateCharacterPanel(this);
    }
    
    protected override void OnUpdate()
    {
        UpdateInput();
    }

    void UpdateInput()
    {
        if (Input.GetMouseButtonUp(0))
        {
            //Shoot(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        }

        if (Input.GetMouseButtonUp(1))
        {
            // check if something is there and react
            
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D[] hits = Physics2D.RaycastAll(ray.origin, ray.direction);

            foreach (var hit in hits)
            {
                if (hit.transform == null) continue;
                
                var damageable = hit.transform.gameObject.GetComponentInChildren<IDamageable>();
                if (damageable != null && !damageable.Equals(this) && !(damageable is MinionController))
                {
                    if (currentAction is AttackAction attackAction && attackAction.Target == damageable)
                        return;

                    Debug.Log("My object is clicked by mouse, will attack");
                    ClearAction();
                    SetAction(new AttackAction(this, damageable, interactionRadius));
                    return;
                }
            }
            
            foreach (var hit in hits)
            {
                if (hit.transform == null) continue;
                
                var interactable = hit.transform.gameObject.GetComponentInChildren<IInteractable>();
                if (interactable != null)
                {
                    Debug.Log("My object is clicked by mouse, will interact");
                    SetAction(new InteractAction(this, interactable, interactionRadius));
                    return;
                }
            }

            // if not, just move there
            // clear actions queue
            ClearAction();
            var newTarget = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            SetAction(new MoveAction(this, new Vector2Target(newTarget), destinationRadius));
        }
    }

    private void Shoot(Vector2 mousePosition)
    {
        var proj = Instantiate(projectilePrefab, rigidbody2D.position, Quaternion.identity);
        proj.SetTarget(mousePosition);
    }

    public bool SpawnMinion(/*MinionType type, */ Vector2 spawnPos)
    {
        if (minions.Count >= minionsCap) return false;
        
        var newMinion = Instantiate(minionPrefab, spawnPos, Quaternion.identity);
        newMinion.OnDeath += () => { minions.Remove(newMinion); };
        minions.Add(newMinion);

        return true;
    }

    public Vector2 GetFormationPosition(MinionController minion)
    {
        int index = minions.IndexOf(minion);

        if (index >= 0 && index < formationPositions.Length)
            return Position + formationPositions[index];

        return Position;
    }

    private void OnDrawGizmos()
    {
        foreach (var pos in formationPositions)
        {
            Gizmos.DrawSphere((Vector2)transform.position + pos, 0.1f);
        }
    }
}
