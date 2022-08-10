using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Sprite[] healthBarSprites;
    public Image healthImage;
    public Image healthBar;
    public TMP_Text stoneHaveText;
    public TMP_Text ironHaveText;
    public TMP_Text coalHaveText;
    public TMP_Text wheatHaveText;
    public TMP_Text breadHaveText;

    private Player player;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player>();
    }

    void UpdateHealthBars()
    {
        if (player.health > 80)
            healthImage.sprite = healthBarSprites[0];
        else if (player.health > 60)
            healthImage.sprite = healthBarSprites[1];
        else if (player.health > 40)
            healthImage.sprite = healthBarSprites[2];
        else if (player.health > 20)
            healthImage.sprite = healthBarSprites[3];
        else
            healthImage.sprite = healthBarSprites[4];

        healthBar.rectTransform.sizeDelta = new Vector3(0.9f * player.health, 10, 0);
        healthBar.rectTransform.localPosition = new Vector3(-45f + 0.45f * player.health, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        stoneHaveText.text = player.stone.ToString();
        ironHaveText.text = player.iron.ToString();
        coalHaveText.text = player.coal.ToString();
        wheatHaveText.text = player.wheat.ToString();
        breadHaveText.text = player.bread.ToString();
        UpdateHealthBars();
    }
}
