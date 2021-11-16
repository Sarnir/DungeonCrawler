using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterPanel : MonoBehaviour
{
    [SerializeField] private Image avatar;
    [SerializeField] private AttributeBar hpBar;
    [SerializeField] private TextMeshProUGUI nameText;

    [SerializeField] private Sprite heroAvatar;
    [SerializeField] private Sprite minionAvatar;
    
    public void Setup(CharacterController character)
    {
        hpBar.Initialize(character.Health);
        avatar.sprite = character is HeroController ? heroAvatar : minionAvatar;
        nameText.text = GetRandomName();

        character.OnDeath += () => { Destroy(gameObject); };
    }

    // put it somewhere more proper
    private string GetRandomName()
    {
        string[] names = new[]
            {"Boris", "Dagomir", "Eneas", "Volodia", "Soham", "Crassius", "Rhean", "Vitkacy", "Artemka"};

        return names[Random.Range(0, names.Length)];
    }
}
