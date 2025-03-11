using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Game : ScreenElement
{
    [SerializeField] private Text missileCounter;

    [SerializeField] private Text[] gameStats = new Text[3];                // score, enemy left, wave counter
    [SerializeField] private GameObject[] HUDs = new GameObject[3];
    [SerializeField] private GameObject[] statBars = new GameObject[3];     // helth, boost
    [SerializeField] private GameObject[] indicators = new GameObject[2];   // shield on, shield off, single, tri, missile

    private float helthFlashingTimer = 0;
    private bool hpActive = true;
    [SerializeField] private GameObject healthIndicator;
    [SerializeField] private GameObject healthVignette;

    //private GameManager gm = null;
    [SerializeField] private Texture[] Icons = new Texture[8];//this is all available weapons
    //[SerializeField] private Texture[] Icons80 = new Texture[7];//this is all available transparanet weapons

    private int CurrentWeapon = 0;
    private bool toFade = false;
    private float fade = 1.5f;
    private float fadeTime = 0.0f;

    //public void AttachGameManager(){ gm = GameManager.instance; }
    //public void RemoveGameManager() { gm = null; }

    private void Update()
    {
        
    }

    //hide base class function for derived func.
    public new void OnScreenOpen()
    {
        OnScreenOpenInternal();
    }
    //protected override void OnScreenOpenInternal()
    //{ 
    //    //fix uxml hud
    //    GetComponent<GameUIPlayerHealth>().StatsChanged();

    //    base.OnScreenOpenInternal();
    //}


    //Button callbacks
    public void Button_PausePressed(bool isPlayerDead)
    {
        UIManager.instance.Pause(isPlayerDead);
    }

    

    public void UpdateGameStats(int score, int NoEnemy, int Wave)
    {
        gameStats[0].text = score.ToString();
        gameStats[1].text = NoEnemy.ToString();
        gameStats[2].text = Wave.ToString();
    }
    public void UpdateShipStats(float boost, bool shield, float health, int missile )
    {
        missileCounter.text = "X " + missile;

        Vector3 temp;
        float maxHp = GameManager.instance.GetShipRef().GetComponent<Stats>().sHealth.Max;
        temp = statBars[0].transform.localScale;
        temp.x = (2.5f / maxHp) * health;
        statBars[0].transform.localScale = temp;

        float maxboost = GameManager.instance.GetShipRef().GetComponent<Stats>().sFuel.Max;
        temp = statBars[1].transform.localScale;
        temp.x = (2.5f / maxboost) * boost;
        statBars[1].transform.localScale = temp;

        Stat statShield = GameManager.instance.GetShipRef().GetComponent<Stats>().sShield;
        temp = statBars[2].transform.localScale;
        temp.x = (2.5f / statShield.Max) * statShield.Value;
        statBars[2].transform.localScale = temp;

        indicators[0].SetActive(shield);
        indicators[1].SetActive(!shield);


        if (health < (maxHp / 2))
        {
            Color tempCol = healthVignette.GetComponent<RawImage>().color;
            tempCol.a = 0.7f - ((health / (maxHp / 2)) * 0.7f);
            healthVignette.GetComponent<RawImage>().color = tempCol;


            if (helthFlashingTimer < Time.time)
            {
                hpActive = !hpActive;
                healthIndicator.SetActive(hpActive);
                helthFlashingTimer = Time.time + 0.2f;

            }
        }
        else
        {
            Color tempCol = healthVignette.GetComponent<RawImage>().color;
            tempCol.a = 0;
            healthVignette.GetComponent<RawImage>().color = tempCol;

            hpActive = true;
            healthIndicator.SetActive(hpActive);
        }



    }






}

