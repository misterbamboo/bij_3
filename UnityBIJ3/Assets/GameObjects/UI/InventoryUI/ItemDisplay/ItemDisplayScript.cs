using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;

public class ItemDisplayScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [Header("Events")]
    [SerializeField] UnityEvent<int> onBuyItem;

    [Header("Icon")]
    [SerializeField] Sprite sourceImage;
    [SerializeField] Sprite bonusImage;
    [SerializeField] Sprite malusImage;

    [Header("Popup Text")]
    [SerializeField] string popoverBonusText;
    [SerializeField] string popoverMalusText;

    [Header("Popup Text")]
    [SerializeField] int price;

    [Header("UI Elements")]
    [SerializeField] Transform popoverTransform;
    [SerializeField] TextMeshProUGUI popoverBonusTextElement;
    [SerializeField] TextMeshProUGUI popoverMalusTextElement;
    [SerializeField] TextMeshProUGUI popoverPriceTextElement;
    [SerializeField] Image iconImageElement;
    [SerializeField] Outline iconOutlineElement;
    [SerializeField] Image iconBonusElement;
    [SerializeField] Image iconMalusElement;

    private void OnDrawGizmos()
    {
        UpdateElements();
    }

    private void Start()
    {
        popoverTransform.gameObject.SetActive(false);
        ChangeOutline(0);
        UpdateElements();
    }

    private void UpdateElements()
    {
        popoverBonusTextElement.text = popoverBonusText;
        popoverMalusTextElement.text = popoverMalusText;
        popoverPriceTextElement.text = $"{price:### ### ### ### ### ###} $";
        iconImageElement.sprite = sourceImage;
        iconBonusElement.sprite = bonusImage;
        iconMalusElement.sprite = malusImage;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        popoverTransform.gameObject.SetActive(true);
        ChangeOutline(1);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        popoverTransform.gameObject.SetActive(false);
        ChangeOutline(0);
    }

    private void ChangeOutline(float alpha)
    {
        var col = iconOutlineElement.effectColor;
        col.a = alpha;
        iconOutlineElement.effectColor = col;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (onBuyItem != null)
        {
            onBuyItem.Invoke(price);
        }
    }
}
