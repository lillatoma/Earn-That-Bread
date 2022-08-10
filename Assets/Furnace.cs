using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Furnace : MonoBehaviour
{

    public FurnaceMenuController furnaceMenu;
    public GameObject furnaceHealth;
    public GameObject furnaceHealthFill;
    [Range(0,4)]
    public int level = 0;
    public bool burning = false;
    public int cookedBread;

    public SpriteRenderer outline;
    public SpriteRenderer burn;
    public Sprite[] bodies;
    public Sprite[] outlines;
    public Sprite[] burns;

    public int[] stoneUpgradeReqs;
    public int[] ironUpgradeReqs;
    public int[] wheatCookReqs;
    public int[] coalCookReqs;
    public int[] breadInstances;
    public float instanceCookTime;

    public int[] healths;
    public float healthPercentage = 1f;

    private int instancesLeft;
    private float instanceTimeLeft;
    private Player player;
    private SpriteRenderer body;
    private int lastLevel;
    public bool shouldSkip2 = false;
    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player>();
        body = GetComponent<SpriteRenderer>();
        outline.sprite = outlines[level];
        burn.sprite = burns[level];
        body.sprite = bodies[level];
        lastLevel = level;
    }

    void SetHealthBar()
    {
        if (healthPercentage >= 1f)
            furnaceHealth.SetActive(false);
        else
        {
            furnaceHealth.SetActive(true);
            furnaceHealthFill.transform.localScale = new Vector3(0.96f * healthPercentage, 0.5f, 1f);
            furnaceHealthFill.transform.localPosition = new Vector3(0f - 0.03125f * (1f-healthPercentage), 0f, -0.05f);
        }
    }

    public bool IsRepairable()
    {
        return healthPercentage < 1f;
    }

    public Vector2Int GetRepairCost()
    {
        int stone = 0;
        int iron = 0;
        for(int i = 0; i < level; i++)
        {
            stone += stoneUpgradeReqs[i];
            iron += ironUpgradeReqs[i];
        }
        stone = 1 + (int)((1f - healthPercentage)*stone);
        iron = 1 + (int)((1f - healthPercentage)*iron);
        return new Vector2Int(stone, iron);
    }

    void OnClick()
    {
        if (Input.GetMouseButtonDown(0))
            if (Mathf.Abs((player.mainCamera.ScreenToWorldPoint(Input.mousePosition) - transform.position).x) < 2
                && Mathf.Abs((player.mainCamera.ScreenToWorldPoint(Input.mousePosition) - transform.position).y) < 1.5)
            {
                    Ray ray = player.mainCamera.ScreenPointToRay(Input.mousePosition);

                    RaycastHit2D hit = Physics2D.GetRayIntersection(ray, Mathf.Infinity);

                    if (!hit.collider && (transform.position - player.transform.position).sqrMagnitude < 9f)
                        furnaceMenu.OpenFurnaceMenu();
            }
    }

    void OnLevelChange()
    {
        if (burning)
            burn.gameObject.SetActive(true);
        else
            burn.gameObject.SetActive(false);

        if(level != lastLevel)
        {
            lastLevel = level;
            outline.sprite = outlines[level];
            burn.sprite = burns[level];
            body.sprite = bodies[level];
        }
    }

    public void StartCooking(int instances)
    {
        instancesLeft += instances;
        if (instanceTimeLeft < 0f)
            instanceTimeLeft = instanceCookTime;
    }

    

    void DoCooking()
    {
        if (instancesLeft > 0)
            burning = true;
        else
            burning = false;
        instanceTimeLeft -= Time.deltaTime;
        if(instanceTimeLeft < 0 && instancesLeft > 0)
        {
            instancesLeft--;
            cookedBread += breadInstances[level];
            instanceTimeLeft = instanceCookTime;
        }
    }

    public void Damage(int damage)
    {
        healthPercentage -= (float)damage / (float)healths[level];
    }

    public void CheckBehind()
    {
        if (Physics2D.OverlapBox(transform.position, new Vector3(4, 3), 0f))
            body.color = new Color(1, 1, 1, 0.5f);
        else
            body.color = new Color(1, 1, 1, 1);
    }

    public void OutsideUpdate()
    {
        if(!body)
            body = GetComponent<SpriteRenderer>();
        body.sprite = bodies[level];
        SetHealthBar();
    }

    // Update is called once per frame
    void Update()
    {
        if (shouldSkip2 && level == 2)
            level = 3;
        CheckBehind();
        OnLevelChange();
        OnClick();
        if (healthPercentage > 0)
            DoCooking();
        else level = 0;
        SetHealthBar();
    }

}
