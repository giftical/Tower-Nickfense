using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopSlotUI : MonoBehaviour
{
    [SerializeField] Image iconImage;
    [SerializeField] TMP_Text nameText;
    [SerializeField] TMP_Text costText;
    [SerializeField] Button buyButton;

    TowerData data;

    public void Set(TowerData newData)
    {
        data = newData;

        bool has = data != null;
        iconImage.enabled = has;
        nameText.enabled = has;
        costText.enabled = has;
        buyButton.interactable = has;

        if (!has) return;

        iconImage.sprite = data.icon;
        nameText.text = data.displayName;
        costText.text = data.cost.ToString();
    }

    public void OnClickBuy()
    {
        Debug.Log("[ShopSlotUI] Clicked slot, data = " + (data ? data.displayName : "null"));

        if (data == null) return;

        if (BuildManager.Instance == null)
        {
            Debug.LogError("[ShopSlotUI] BuildManager.Instance is null");
            return;
        }

        BuildManager.Instance.StartPurchase(data);
    }
}
