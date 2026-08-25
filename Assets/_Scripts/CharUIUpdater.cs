using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharUIUpdater : MonoBehaviour
{
    public GameObject Parent;
    public Attack parentAttackScript;
    public Slider Slider;
    public TextMeshPro damagePopUpText;
    void Start()
    {
       parentAttackScript = Parent.GetComponent<Attack>();
       Slider = GetComponentInChildren<Slider>();
        Transform foundDamagePopUpText = transform.Find("DamagePopUp");
        if (foundDamagePopUpText != null)
        {
            damagePopUpText = foundDamagePopUpText.GetComponent<TextMeshPro>();
        }
        else Debug.Log("Cant Find Damage PopUp Text" + gameObject.name);


    }

    
    void Update()
    {
        Slider.maxValue = parentAttackScript.maxHP;
        Slider.minValue = 0;
        Slider.value = parentAttackScript.currentHP;
    }
    public IEnumerator DamagePopUp(float value)
    {
        if(damagePopUpText != null)
        {
            damagePopUpText.fontSize = 10;
            damagePopUpText.text = value.ToString();
            yield return new WaitForSeconds(1f);

            damagePopUpText.fontSize = 0;
            



        }
    }
}
