using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class MinionSpawn : MonoBehaviour, IInteractable
{
    private HeroController hero;

    public Vector2 Position => transform.position;

    void Start()
    {
        hero = FindObjectOfType<HeroController>();
    }

    public void Interact(CharacterController interactor)
    {
        if (hero.SpawnMinion(transform.position))
        {
            Destroy(gameObject);
        }
        else
        {
            
            // show some info that hero can't spawn more minions
        }
    }
}
