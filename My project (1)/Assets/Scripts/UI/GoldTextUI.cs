using TMPro;
using UnityEngine;

public class GoldText : MonoBehaviour
{
    TextMeshProUGUI text;

    void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        text.text = EconomyManager.Instance.Gold.ToString();
    }
}
