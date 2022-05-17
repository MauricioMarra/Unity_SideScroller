using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUDController : MonoBehaviour
{
    public Slider healtSlider;
    public Gradient healthGradient;
    public Image healthFill;
    public TextMeshProUGUI money;
    private InventoryController inventory;

    private void Start()
    {
        inventory = InventoryController.instance;
        inventory.onItemChangedCallback += UpdateHud;
    }

    public void SetFullHealth(int health)
    {
        healtSlider.maxValue = health;
        healtSlider.value = health;
    }

    public void SetHealth(int health)
    {
        healtSlider.value -= health;
        SetColor();
    }

    private void SetColor()
    {
        healthFill.color = healthGradient.Evaluate(healtSlider.normalizedValue);
    }

    public void UpdateHud()
    {
        money.text = inventory.GetPurse().ToString();
    }
}
