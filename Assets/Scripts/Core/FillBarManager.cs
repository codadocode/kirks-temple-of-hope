using System;
using UnityEngine;
using UnityEngine.UI;

public class FillBarManager : MonoBehaviour
{
    [Header("Configuration")]
    [SerializeField] private bool canInteract = false;
    [SerializeField] private Slider bar;
    [SerializeField] private float minValue = 0;
    [SerializeField] private float maxValue = 100;
    [SerializeField] private float weight = 0.02f;
    private float actualValue = 0;
    
    public event Action OnBarCompleteEvent;
    public event Action OnInteractionEvent;

    private void Start()
    {
        this.bar.minValue = this.minValue;
        this.bar.maxValue = this.maxValue;
    }

    private void Activate()
    {
        if (this.canInteract) return;
        this.canInteract = true;
        this.actualValue = 0;
        UpdateSlidervalue();
        this.bar.gameObject.SetActive(true);
    }

    private void Deactivate()
    {
        if (!this.canInteract) return;
        this.canInteract = false;
        this.actualValue = 0;
        UpdateSlidervalue();
        this.bar.gameObject.SetActive(false);
    }

    public void ToggleCanInteract(bool interactValue)
    {
        if (interactValue) Activate();
        else Deactivate();
    }

    public void Interact(float valueToAdd)
    {
        if (!canInteract) return;
        
        this.OnInteractionEvent?.Invoke();
        this.actualValue = this.actualValue + valueToAdd >= this.maxValue ? this.maxValue : (this.actualValue + valueToAdd);
        UpdateSlidervalue();
        
        if (this.actualValue < this.maxValue) return;
        OnBarComplete();
    }

    private void UpdateSlidervalue()
    {
        this.bar.value = this.actualValue;
    }

    private void Update()
    {
        if (!this.canInteract) return;
        this.actualValue = this.actualValue - this.weight > 0 ? this.actualValue - this.weight : 0;
        UpdateSlidervalue();
    }

    private void OnBarComplete()
    {
        OnBarCompleteEvent?.Invoke();
        Deactivate();
    }
}
