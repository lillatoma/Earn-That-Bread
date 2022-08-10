using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class FurnaceMenuController : MonoBehaviour
{

    public TMP_Text stoneReqText;
    public TMP_Text ironReqText;
    public TMP_Text wheatReqText;
    public TMP_Text coalReqText;
    public TMP_Text breadInstanceText;
    public Image upgradeImage;
    public Image cookImage;
    public Slider slider;

    public TMP_Text upgradeText;

    private Furnace furnace;
    private Player player;
    // Start is called before the first frame update
    void Start()
    {
        furnace = FindObjectOfType<Furnace>();
        player = FindObjectOfType<Player>();
        CloseFurnaceMenu();
    }

    public void OpenFurnaceMenu()
    {
        gameObject.SetActive(true);

        if (player.health > 0)
            while (furnace.cookedBread > 0 && player.health < 100)
            {
                furnace.cookedBread--;
                player.health += 10;
                if (player.health > 100)
                    player.health = 100;
            }
        player.bread += furnace.cookedBread;
        furnace.cookedBread = 0;

        if (furnace.level <= 4)
        {
            var repairRequirements = furnace.GetRepairCost();
            int stoneReq = furnace.stoneUpgradeReqs[furnace.level];
            int ironReq = furnace.ironUpgradeReqs[furnace.level];
            int wheatReq = furnace.wheatCookReqs[furnace.level] * (int)slider.value;
            int coalReq = furnace.coalCookReqs[furnace.level] * (int)slider.value;

            if (furnace.IsRepairable())
            {
                stoneReq = repairRequirements.x;
                ironReq = repairRequirements.y;
                upgradeText.text = "Repair";
            }
            else upgradeText.text = "Upgrade";

            if (stoneReq > 0)
                stoneReqText.text = stoneReq.ToString();
            else
                stoneReqText.text = "MAX";
            if (ironReq > 0)
                ironReqText.text = ironReq.ToString();
            else
                ironReqText.text = "MAX";
            wheatReqText.text = wheatReq.ToString();
            coalReqText.text = coalReq.ToString();

            if (player.stone < stoneReq)
                stoneReqText.color = new Color(1, 0, 0);
            else
                stoneReqText.color = new Color(0, 0, 0);
            if (player.iron < ironReq)
                ironReqText.color = new Color(1, 0, 0);
            else
                ironReqText.color = new Color(0, 0, 0);
            if (player.wheat < wheatReq)
                wheatReqText.color = new Color(1, 0, 0);
            else
                wheatReqText.color = new Color(0, 0, 0);
            if (player.coal < coalReq)
                coalReqText.color = new Color(1, 0, 0);
            else
                coalReqText.color = new Color(0, 0, 0);

            if (player.wheat < wheatReq || player.coal < coalReq)
                cookImage.color = new Color(1, 1, 1, 0.5f);
            else
                cookImage.color = new Color(1, 1, 1, 1);
            if (stoneReq > 0 && ironReq > 0 && player.iron >= ironReq && player.stone >= stoneReq)
                upgradeImage.color = new Color(1, 1, 1, 1);
            else
                upgradeImage.color = new Color(1, 1, 1, 0.5f);
            if (furnace.breadInstances[furnace.level] > 1)
                breadInstanceText.text = furnace.breadInstances[furnace.level].ToString() + "X";
            else
                breadInstanceText.text = "";

            UpdateSlider();

            
        }
    }

    public void CloseFurnaceMenu()
    {
        gameObject.SetActive(false);
    }

    public void UpgradeFurnace()
    {
        if (furnace.IsRepairable())
        {
            var repairRequirements = furnace.GetRepairCost();
            int stoneReq = repairRequirements.x;
            int ironReq = repairRequirements.y;
            if (stoneReq > 0 && ironReq > 0)
            {
                if (player.stone >= stoneReq && player.iron >= ironReq)
                {
                    furnace.healthPercentage = 1f;
                    player.stone -= stoneReq;
                    player.iron -= ironReq;
                }
            }
        }
        else
        {
            int stoneReq = furnace.stoneUpgradeReqs[furnace.level];
            int ironReq = furnace.ironUpgradeReqs[furnace.level];
            if (stoneReq > 0 && ironReq > 0)
            {
                if (player.stone >= stoneReq && player.iron >= ironReq)
                {
                    furnace.level++;
                    player.stone -= stoneReq;
                    player.iron -= ironReq;
                }
            }
        }
        OpenFurnaceMenu();
    }

    public void DoNothing()
    {
        OpenFurnaceMenu();
    }
    public void UpdateSlider()
    {
        int wheatReq = furnace.wheatCookReqs[furnace.level];
        int coalReq = furnace.coalCookReqs[furnace.level];
        int wheatPossible = player.wheat / wheatReq;
        int coalPossible = player.coal / coalReq;
        if (wheatPossible < coalPossible)
            slider.maxValue = wheatPossible;
        else
            slider.maxValue = coalPossible;

        if (slider.value > slider.maxValue)
            slider.value = slider.maxValue;
        

    }
    public void StartCooking()
    {
        int wheatReq = furnace.wheatCookReqs[furnace.level];
        int coalReq = furnace.coalCookReqs[furnace.level];

        furnace.StartCooking((int)slider.value);
        player.wheat -= (int)slider.value * wheatReq;
        player.coal -= (int)slider.value * coalReq;
        OpenFurnaceMenu();
    }
    
    public void GoBackToFurnace()
    {
        player.SetTargetPosition(furnace.transform.position - new Vector3(0,2,0));
    }

    // Update is called once per frame
    void Update()
    {
        UpdateSlider();
    }
}
