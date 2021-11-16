using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class CharacterCorpse : MonoBehaviour, IInteractable
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

    public static CharacterCorpse Spawn(Sprite sprite)
    {
        var newObject = new GameObject("corpse");
        var sr = newObject.AddComponent<SpriteRenderer>();
        sr.spriteSortPoint = SpriteSortPoint.Pivot;
        sr.sprite = sprite;
        newObject.AddComponent<BoxCollider2D>().isTrigger = true;
        return newObject.AddComponent<CharacterCorpse>();
    }
}
