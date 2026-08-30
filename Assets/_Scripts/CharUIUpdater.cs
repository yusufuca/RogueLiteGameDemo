using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharUIUpdater : MonoBehaviour
{
    public GameObject Parent;
    public StatManager parentStatManagerScript;
    public Slider Slider;

    private void Awake()
    {
        parentStatManagerScript = Parent.GetComponent<StatManager>();
        Slider = GetComponentInChildren<Slider>();
    }
    private void OnEnable()
    {
        if (parentStatManagerScript != null)
        {
            parentStatManagerScript.OnHealthChanged += UpdateHPSlider;
        }
    }
    private void OnDisable()
    {

        if (parentStatManagerScript != null)
        {
            parentStatManagerScript.OnHealthChanged -= UpdateHPSlider;
        }
    }

    void Start()
    {
      
       
       
        
    }

    
    void Update()
    {

    }
    private void UpdateHPSlider(float current, float max)
    {
        Slider.maxValue = max;
        Slider.minValue = 0;
        Slider.value = current;

    }
  
}
