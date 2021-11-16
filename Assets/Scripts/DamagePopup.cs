using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using DG.Tweening;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class DamagePopup : MonoBehaviour
{
    [SerializeField] private TextMeshPro textMesh;
    private static float animDuration = 2f; 

    public static DamagePopup Spawn(int damage, Vector2 pos)
    {
        var popup = Instantiate((GameObject) Resources.Load("Prefabs/DamagePopup"), pos, Quaternion.identity).GetComponent<DamagePopup>();
        popup.textMesh.text = damage.ToString();

        popup.textMesh.DOFade(0f, animDuration).SetEase(Ease.InExpo);
        popup.transform.DOLocalJump(new Vector3(pos.x + (Random.value - 0.5f)*0.5f, pos.y + Random.value * 0.25f, 0f), 0.5f, 1, animDuration);
        popup.transform.DOScale(Vector3.zero, animDuration).OnComplete(() => Destroy(popup.gameObject));

        return popup;
    }

    public static DamagePopup SpawnMiss(Vector2 pos)
    {
        var popup = Spawn(0, pos);
        popup.textMesh.text = "MISS!";

        return popup;
    }
}
