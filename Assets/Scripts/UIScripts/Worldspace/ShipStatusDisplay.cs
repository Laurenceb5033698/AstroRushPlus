using UnityEngine;
using UnityEngine.UI;

public class ShipStatusDisplay : MonoBehaviour
{
    [SerializeField] Slider healthBar;
    [SerializeField] Slider shieldBar;
    [SerializeField] RectTransform container;
    Stats shipStats;
    
    void Awake()
    {
        shipStats = GetComponentInParent<Stats>();

    }

    private void Start()
    {
        healthBar.maxValue = shipStats.Health.Max;
        healthBar.value = healthBar.maxValue;
        shieldBar.maxValue = shipStats.Shield.Max;
        shieldBar.value = shieldBar.maxValue;
        //health and shield bar size scales with container size. number of healthbar hearts scales with container horizontal size
        container.sizeDelta = new Vector2(healthBar.maxValue*2,5);
    }

    void Update()
    {
        if (shipStats.IsAlive())
        {
            healthBar.value = shipStats.Health.Value;
            shieldBar.value = shipStats.Shield.Value;
        }
        else
        {
            healthBar.value = 0;
            shieldBar.value = 0;
        }
    }
}
