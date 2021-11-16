using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterPanels : MonoBehaviour
{
    [SerializeField] private CharacterPanel panelPrefab;

    private static CharacterPanels _instance;
    private static CharacterPanels Instance
    {
        get
        {
            if (_instance == null)
                _instance = GameObject.FindObjectOfType<CharacterPanels>();

            return _instance;
        }
    }

    public static void CreateCharacterPanel(CharacterController character)
    {
        Instance.Create(character);
    }
    
    private void Create(CharacterController character)
    {
        var newPanel = Instantiate(panelPrefab, transform);
        newPanel.Setup(character);
    }
}
