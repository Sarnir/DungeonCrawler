using System;
using UnityEngine;

public abstract class Attribute : MonoBehaviour
{

    protected int currentValue;
    public int Current => currentValue;
    public int Max => maxValue;

    [SerializeField] private int maxValue;
    [SerializeField] private AttributeBar prefab;
    [SerializeField] private float YOffset;

    public Action<int> OnValueChanged;
    public Action OnValueDepleted;
    public Action OnValueMax;

    public void Setup(int max, int current)
    {
        maxValue = max;
        currentValue = current;

        SetupBar();
    }

    protected void SetupBar()
    {
        if (prefab == null)
            return;
        
        var bar = Instantiate(prefab, transform);
        bar.Initialize(this);
        bar.transform.localPosition = new Vector3(0f, YOffset, 0f);
    }

    public void Change(int change)
    {
        currentValue += change;
        OnValueChanged?.Invoke(currentValue);

        if (currentValue <= 0f)
            OnValueDepleted?.Invoke();
    }
}
