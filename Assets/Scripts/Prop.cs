using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;

public class Prop : MonoBehaviour, IDamageable
{
    [SerializeField] private Transform activeState;
    [SerializeField] private Transform brokenState;
    [SerializeField] private SpriteRenderer[] parts;
    public Vector2 Position => transform.position;

    private void Start()
    {
        activeState.gameObject.SetActive(true);
        brokenState.gameObject.SetActive(false);
    }

    public Action OnDeath { get; set; }

    public void TakeDamage(int damage)
    {
        Debug.Log(name + " got hit for " + damage + " damage!");

        activeState.gameObject.SetActive(false);
        brokenState.gameObject.SetActive(true);

        for (int i = 0; i < parts.Length; i++)
            parts[i].transform.DOLocalJump(new Vector3(Random.value - 0.5f, -0.2f, 0f), Random.Range(0.1f, 1f), 1, Random.Range(0.3f, 0.6f));
        
        OnDeath?.Invoke();
    }
}
