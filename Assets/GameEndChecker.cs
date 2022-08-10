using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEndChecker : MonoBehaviour
{
    public CanvasGroup blackFade;
    public int neededBreadCount;
    public GameObject endgameKing;
    public Vector3 playerPosition;
    public Vector3 kingPosition;



    private Player player;

    private bool startedFinishing = false;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player>();
    }

    IEnumerator DoEvents()
    {
        blackFade.gameObject.SetActive(true);
        while (true)
        {
            for (; blackFade.alpha <= 1f; )
            {
                if (blackFade.alpha + Time.deltaTime < 1f)
                    blackFade.alpha += Time.deltaTime;
                else
                {
                    blackFade.alpha = 1f;
                    break;
                }
                yield return new WaitForEndOfFrame();
            }
            player.transform.position = playerPosition;
            player.SetTargetPosition(player.GetOwnPosition());
            endgameKing.SetActive(true);
            endgameKing.transform.position = kingPosition;
            var goblins = FindObjectsOfType<Goblin>();
            foreach (Goblin goblin in goblins)
            {
                goblin.SetTargetPosition(goblin.transform.position - (player.transform.position - goblin.transform.position) * 100);
            }
            for (; blackFade.alpha > 0f; blackFade.alpha -= Time.deltaTime)
                yield return new WaitForEndOfFrame();
            blackFade.alpha = 0f;
            break;
        }
        yield return null;
    }
    void StartFinishingGame()
    {

        if (startedFinishing || FindObjectOfType<FurnaceMenuController>())
            return;
        startedFinishing = true;
        Debug.Log("Game started finishing");
        var goblins = FindObjectsOfType<Goblin>();
        foreach (Goblin goblin in goblins)
        {
            goblin.SetTargetPosition(goblin.transform.position - (player.transform.position - goblin.transform.position) * 100);
            Destroy(goblin.gameObject, 5f);
        }
        StartCoroutine(DoEvents());

    }

    // Update is called once per frame
    void Update()
    {
        if (player)
        {
            if (player.bread >= neededBreadCount)
                StartFinishingGame();
        }
        else
            player = FindObjectOfType<Player>();
    }
}
