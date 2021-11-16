using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttributeBar : MonoBehaviour
{
    [SerializeField] protected Image barImage;

    private float max;
    
    public void Initialize(Attribute attribute)
    {
        barImage.fillAmount = 1f;
        max = attribute.Max;
        attribute.OnValueChanged += ChangeValue;
        attribute.OnValueDepleted += OnDeath;
    }

    private void ChangeValue(int current)
    {
        float percent = current / max;
        barImage.fillAmount = percent;
        OnChangeValue();
    }

    protected virtual void OnChangeValue()
    {
    }

    private void OnDeath()
    {
        gameObject.SetActive(false);
    }
}
