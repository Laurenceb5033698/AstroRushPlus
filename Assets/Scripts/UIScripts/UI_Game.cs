using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Game : ScreenElement
{
    [SerializeField] private Text missileCounter;

    [SerializeField] private Text[] gameStats = new Text[3];                // score, enemy left, wave counter
    [SerializeField] private GameObject[] HUDs = new GameObject[3];
    [SerializeField] private GameObject[] statBars = new GameObject[2];     // helth, boost
    [SerializeField] private GameObject[] indicators = new GameObject[2];   // shield on, shield off, single, tri, missile

    private float helthFlashingTimer = 0;
    private bool hpActive = true;
    [SerializeField] private GameObject healthIndicator;
    [SerializeField] private GameObject healthVignette;

    //private GameManager gm = null;
    [SerializeField] private Texture[] Icons = new Texture[8];//this is all available weapons
    //[SerializeField] private Texture[] Icons80 = new Texture[7];//this is all available transparanet weapons

    private List<Texture> activeWeaponIcons = new List<Texture>();//this is weapons in inventory
    [SerializeField] private GameObject[] IconActiveFrame = new GameObject[3];//these are the icons currently on screen
    private int CurrentWeapon = 0;
    private bool toFade = false;
    private float fade = 1.5f;
    private float fadeTime = 0.0f;

    //public void AttachGameManager(){ gm = GameManager.instance; }
    //public void RemoveGameManager() { gm = null; }

    private void Update()
    {
        if (toFade )
        {
            if (fadeTime > 0f)//delay before fade occurs
                fadeTime -= Time.deltaTime;
            else
            {//start fade
                Color faded = IconActiveFrame[0].GetComponent<RawImage>().color;
                if (faded.a > 0.5f) faded.a -= 0.04f; else toFade=false;
                
                IconActiveFrame[0].GetComponent<RawImage>().color = faded;
                
                IconActiveFrame[2].GetComponent<RawImage>().color = faded;
                //toFade = false;
            }
        }
    }

    public void Button_PausePressed(bool isPlayerDead)
    {
        UIManager.instance.Pause(isPlayerDead);
    }

    public void RegisterWeaponIcons(List<Weapon> mWeapons)
    {
        //for each weapon, use its GameObject.name to compare against list of icons (via swtich)
        //when a match is found, add icon to activeWeaponIcon List
        activeWeaponIcons.Clear();
        for (int gun = 0; gun < mWeapons.Count; gun++)
        {
            switch(mWeapons[gun].gameObject.name)
            {
                case "WeaponSingle":
                    activeWeaponIcons.Add(Icons[0]);
                    break;
                case "WeaponTri":
                    activeWeaponIcons.Add(Icons[1]);
                    break;
                case "WeaponHeavy":
                    activeWeaponIcons.Add(Icons[2]);
                    break;
                case "WeaponCharge":
                    activeWeaponIcons.Add(Icons[3]);
                    break;
                case "WeaponGlava":
                    activeWeaponIcons.Add(Icons[4]);
                    break;
                case "WeaponMissile":
                    activeWeaponIcons.Add(Icons[5]);
                    break;
                case "WeaponBFOM":
                    activeWeaponIcons.Add(Icons[6]);
                    break;
                case "WeaponEMP":
                    activeWeaponIcons.Add(Icons[7]);
                    break;
                default:
                    activeWeaponIcons.Add(Icons[0]);
                    break;

            }
        }
        WeaponChanged(0);
    }
    
    public void WeaponChanged(int newWeap)
    {
        toFade = true;
        fadeTime = fade;
        CurrentWeapon = newWeap;
        Color NonFaded = IconActiveFrame[0].GetComponent<RawImage>().color;
        NonFaded.a = 1f;
        IconActiveFrame[0].GetComponent<RawImage>().color = NonFaded;
        IconActiveFrame[2].GetComponent<RawImage>().color = NonFaded;
        if (activeWeaponIcons.Count > 0)
        {
            if (CurrentWeapon == 0)//previous weapon (left)
                IconActiveFrame[0].GetComponent<RawImage>().texture = activeWeaponIcons[activeWeaponIcons.Count - 1];
            else
                IconActiveFrame[0].GetComponent<RawImage>().texture = activeWeaponIcons[CurrentWeapon - 1];
            
            //Curretn weapon (middle)    
            IconActiveFrame[1].GetComponent<RawImage>().texture = activeWeaponIcons[CurrentWeapon];

            if (CurrentWeapon == activeWeaponIcons.Count - 1)//next weapon (right)
                IconActiveFrame[2].GetComponent<RawImage>().texture = activeWeaponIcons[0];
            else
                IconActiveFrame[2].GetComponent<RawImage>().texture = activeWeaponIcons[CurrentWeapon + 1];
            
        }
    }

    public void UpdateGameStats(int score, int NoEnemy, int Wave)
    {
        gameStats[0].text = score.ToString();
        gameStats[1].text = NoEnemy.ToString();
        gameStats[2].text = Wave.ToString();
    }
    public void UpdateShipStats(float boost, bool shield, float health, int missile, int wType)
    {
        missileCounter.text = "X " + missile;

        Vector3 temp;
        float maxval = GameManager.instance.GetShipRef().GetComponent<Stats>().Health.Max;
        temp = statBars[0].transform.localScale;
        temp.x = (2.5f / maxval) * health;
        statBars[0].transform.localScale = temp;

        maxval = GameManager.instance.GetShipRef().GetComponent<Stats>().Fuel.Max;
        temp = statBars[1].transform.localScale;
        temp.x = (2.5f / maxval) * boost;
        statBars[1].transform.localScale = temp;


        indicators[0].SetActive(shield);
        indicators[1].SetActive(!shield);

        if(CurrentWeapon != wType)
          WeaponChanged(wType);
        

        if (health < 50)
        {
            Color tempCol = healthVignette.GetComponent<RawImage>().color;
            tempCol.a = -((health / 50) * 0.7f) + 0.7f;
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

