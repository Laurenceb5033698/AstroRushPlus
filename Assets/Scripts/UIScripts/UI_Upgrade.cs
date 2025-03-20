using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


////upgrade stats represent the visual upgrades that players buy in the menu
//    //there are 10 points to buy at the minute
//    public class upgradestats
//    {
//        const int maxValues = 10;
//        public int[] stats = new int[6] { 0, 0, 0, 0, 0, 0 };
//        //public int hp;     //current upgrade level (from 0 -> maxvalues(10))
//        //public int shield;
//        //public int attack;
//        //public int special;
//        //public int speed;
//        //public int fuel;

//        //public upgradestats()
//        //{
//        //    stats = new int[6] { 0, 0, 0, 0, 0, 0 };
//        //}
//    }

public class UI_Upgrade : ScreenElement
{
    
    //values initailised in editor
    [SerializeField] private float[] StatUpgradeValues; //How much each upgrade point adds to the stat modifier
    [SerializeField] private int[] StatUpgradeCosts;    //How much the corrisoponding point costs to buy

    [SerializeField]
    private Text UpgradePoints;

    [SerializeField] protected List<Selectable> pluses;
    [SerializeField] protected List<Selectable> minuses;

    [SerializeField] protected List<Image> statMasks;
    [SerializeField] protected List<TextMeshProUGUI> textVisuals;

    bool selectorx =true;//starts on plus, false ==minus

    //these stats will be saved when the game is exited 
    //upgradestats Prevstate;
    const int maxValues = 10;
    public int[] prevstats = new int[6] { 0, 0, 0, 0, 0, 0 };
    //these are used when buying stats
    //upgradestats tempstats;
    public int[] tempstats = new int[6] { 0, 0, 0, 0, 0, 0 };

    //button layout:
    /*
     *    0      -   +
     *    1      -   +
     *    2      -   +
     *    3      -   +
     *    4      -   +
     *    5      -   +
     *          
     *    6          fin
     * 
     * //process inputs interprets which direction to take on input based on this^
     * first 6 values relate to plus and minuses
     * after that relates to ButtonList selectables
     * 
    */

    new protected void OnEnable()
    {
        base.OnEnable();
        //update upgrade points
        UpgradePoints.text = GameManager.instance.currentScore.ToString();
    }


    public void ProcessInputs()
    {
        
        //up/down
        if (controls.LAnalogueYDown || (controls.DpadYPressed && controls.DpadDown)) AdvanceSelector();
        if (controls.LAnalogueYUp || (controls.DpadYPressed && controls.DpadUp)) RetreatSelector();

        //if can go horizontal ie plus-minus
        //left/right
        if (controls.LAnalogueXLeft || controls.LAnalogueXRight || (controls.DpadXPressed && controls.DpadLeft) || (controls.DpadXPressed && controls.DpadRight) )
            ToggleSelectorX();

    }



    public void loadPlayerStats(Stats oldPlayerStats)
    {   //load stats from playerprefs
        //prevstate <- prefs
    }
    public void savePlayerStats()
    {   //save stats to playerprefs
        //prevstate -> prefs
    }
    public void ResetStats()
    {
        prevstats = new int[6] { 0, 0, 0, 0, 0, 0 };
        tempstats = new int[6] { 0, 0, 0, 0, 0, 0 };
        for (int i=0; i<statMasks.Count; ++i) {
            setVisuals(i);
        }

    }


    public void Button_PlusPressed(Button self)
    {   //clicked/pressed to buy an upgrade point
        int statindex = pluses.IndexOf(self);
        //do upgrade
        if (tempstats[statindex] < maxValues)
            //check price vs cost
            //tempstat[hp]+1 === next value in the hp stat line
            //statUpgradeCosts[hp+1]+1 == cost of next value

            if (StatUpgradeCosts[tempstats[statindex]+1]+1 < GameManager.instance.currentScore)
            {
                //buy stat
                GameManager.instance.currentScore -= StatUpgradeCosts[tempstats[statindex] +1]+1;
                //record stat bought
                tempstats[statindex]++;//level up by 1 point
            }
        setVisuals(statindex);
    }

    public void Button_MinusPressed(Button self)
    {   //clicked/pressed to buy an upgrade point
        int statindex = minuses.IndexOf(self);
        //do refund
        if (tempstats[statindex] > prevstats[statindex])
        {  //cannot reduce below prevstats
            //refund cost of stat undone
            GameManager.instance.currentScore += StatUpgradeCosts[tempstats[statindex]];

            tempstats[statindex]--;//leveldown up by 1 point
        }
        setVisuals(statindex);

    }

    //player pressed confirm upgrades button
    public void Button_ConfirmUpgrades()
    {
        //save temp stats to prev stats
        System.Array.Copy(tempstats, prevstats, tempstats.Length);
        PlayerController pCntrllr = GameManager.instance.GetShipRef().GetComponent<PlayerController>();
        //save new stats to ship
        //pCntrllr.UpdateStats(
        //    StatUpgradeValues[tempstats[0]], StatUpgradeValues[tempstats[1]], StatUpgradeValues[tempstats[2]],
        //    StatUpgradeValues[tempstats[3]], StatUpgradeValues[tempstats[4]], StatUpgradeValues[tempstats[5]]
        //    );
        
        //save new stats to playerprefs??
        GameManager.instance.GetComponent<AIManager>().NewWave();
        //uimanager change to gamescreen
        UIManager.instance.Resume();
    }


    ///////////////////////
    //UI Button interaction overrides
    override public void AdvanceSelector()
    {
        if (selector == (ButtonCount() - 1))
            selector = 0;
        else
            ++selector;
        SelectButton();
    }

    override public void RetreatSelector()
    {
        if (selector == 0)
            selector = (ButtonCount() - 1);
        else
            --selector;
        SelectButton();
    }

    override public void SubmitSelection()
    {
        
        if (selector < pluses.Count)  //if inside plus/minus block
            if (selectorx)
                ((Button)pluses[selector]).onClick.Invoke();
            else
                ((Button)minuses[selector]).onClick.Invoke();
        else
        {   //not in stats block
            ((Button)ButtonList[selector- pluses.Count]).onClick.Invoke();   //subract pluses.count to re-align for buttonlist
        }
    }

    override protected void SelectButton()
    {
        if (ButtonCount() > 0)//earlyout for empty button lists

            if (selector < pluses.Count)  //if inside plus/minus block
                if (selectorx)
                    pluses[selector].Select();
                else
                    minuses[selector].Select();
            else
            {   //not in stats block
                ButtonList[selector - pluses.Count].Select();   //subract pluses.count to re-align for buttonlist
            }
    }

    //**********************************
    //horizontal
    public void ToggleSelectorX()
    {   //toggles between plus/minus buttons
        selectorx = !selectorx;
        SelectButton();
    }

    

    //helpers

    //returns number of up/down buttons.
    //used for managing selector wrapping
    private int ButtonCount()
    {
        int count = 0;
        count += pluses.Count;  //only counts pluses, because minuses are horizontal and there are same number
        count += ButtonList.Count;

        return count;
    }

    private void setVisuals(int index)
    {
        //calc mask val (0->)
        statMasks[index].fillAmount = (float)tempstats[index] * 0.1f;
        float val = 1+StatUpgradeValues[tempstats[index]];
        
        textVisuals[index].SetText(val.ToString("F1"));

        //update upgrade points
        UpgradePoints.text = GameManager.instance.currentScore.ToString();
    }
}
