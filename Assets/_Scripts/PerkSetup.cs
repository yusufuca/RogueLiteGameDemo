using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PerkSetup : MonoBehaviour, IPointerDownHandler , IPointerExitHandler , IPointerEnterHandler
{
    public Perk_SO myData;
    public bool isPurchased;
    [SerializeField] private Image myImage;
    [SerializeField] private PerkTreeManager perkTreeManager;


    private void Awake()
    {
        perkTreeManager = FindAnyObjectByType<PerkTreeManager>();
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        if (!isPurchased)
        {
            isPurchased = perkTreeManager.TryToAddPerk(myData, myImage);
        }
        else
        {
            Debug.Log("Already Purchased");
        }
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!isPurchased)
        {
            myImage.color = Color.red;
        }
        perkTreeManager.descriptionTextGameObject.SetActive(true);
        perkTreeManager.descriptionTextGameObject.transform.position = new Vector3(transform.position.x + 450,transform.position.y,transform.position.z);
        perkTreeManager.descriptionText.text = myData.description;
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if (!isPurchased)
        {
            myImage.color = Color.white;
        }
        perkTreeManager.descriptionTextGameObject.SetActive(false);
    }
}
