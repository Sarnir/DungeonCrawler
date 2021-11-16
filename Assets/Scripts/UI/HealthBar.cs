using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : AttributeBar
{
    protected override void OnChangeValue()
    {
        barImage.color = Color.HSVToRGB(0.3921f * barImage.fillAmount, 1f, 1f);
    }
}
