using UnityEngine;
using UnityEngine.UI;

public class UI_Game : ScreenElement
{
    // health, boost, special, missile
    [SerializeField] private Image[] statBars = new Image[4];     

    [SerializeField] private GameObject healthIndicator;
    [SerializeField] private GameObject healthVignette;

    public override void Update()
    {
        HandleSubmit();
        HandleCancel();

        //UpdateShipStats();
        //UpdateGameStats();
    }

    protected override void Cancel()
    {   //open pause menu
        bool PlayerAlive = GameManager.instance.GetShipRef().GetComponent<Stats>().IsAlive();
        Button_PausePressed(PlayerAlive);
    }

    //hide base class function for derived func.
    public new void OnScreenOpen()
    {
        OnScreenOpenInternal();
    }


    //Button callbacks
    public void Button_PausePressed(bool isPlayerDead)
    {
        ServicesManager.Instance.GameStateService.GameState = GameState.PAUSE;
        UIManager.instance.Pause(isPlayerDead);
    }

    public void Button_TestUpgrade()
    {
        //use standard upgrade state for testing upgrades.
        ServicesManager.Instance.GameStateService.GameState = GameState.PICKUPGRADE;

    }

    //public void UpdateGameStats(int score, int NoEnemy, int Wave)
    //{
    //    gameStats[0].text = score.ToString();
    //    gameStats[1].text = NoEnemy.ToString();
    //    gameStats[2].text = Wave.ToString();
    //}

    public void UpdateShipStats(Stats _stats)
    {
        float maxHp = _stats.Get(StatType.sHealth);
        statBars[0].fillAmount = _stats.ShipHealth / maxHp;

        float maxShield = _stats.Get(StatType.sShield);
        statBars[1].fillAmount = _stats.ShipShield / maxShield;

        float maxFuel = _stats.Get(StatType.sFuel);
        statBars[2].fillAmount = _stats.ShipFuel / maxFuel;

        float maxAmmo = _stats.Get(StatType.mAmmo);
        statBars[3].fillAmount = _stats.OrdinanceAmmo / maxAmmo;

        //VIGNETTE
        //if (_stats.ShipHealth < (maxHp / 2))
        //{
        //    Color tempCol = healthVignette.GetComponent<RawImage>().color;
        //    tempCol.a = 0.7f - ((_stats.ShipHealth / (maxHp / 2)) * 0.7f);
        //    healthVignette.GetComponent<RawImage>().color = tempCol;


        //    if (healthFlashingTimer < Time.time)
        //    {
        //        hpActive = !hpActive;
        //        healthIndicator.SetActive(hpActive);
        //        healthFlashingTimer = Time.time + 0.2f;

        //    }
        //}
        //else
        //{
        //    Color tempCol = healthVignette.GetComponent<RawImage>().color;
        //    tempCol.a = 0;
        //    healthVignette.GetComponent<RawImage>().color = tempCol;

        //    hpActive = true;
        //    healthIndicator.SetActive(hpActive);
        //}
    }

}

