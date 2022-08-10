using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class King : MonoBehaviour
{
    public float movementSpeed;
    public bool modifyFurnace = false;
    private Rigidbody2D rigidBody;
    private Animator animator;
    private float Align = 0;
    private Vector2 targetPosition;
    private MapGenerator mapGenerator;
    private Goblin[] goblins;
    private Player player;
    private Furnace furnace;
    private UIManager uiManager;
    private bool moving = false;
    public void SetTargetPosition(Vector2 position)
    {
        targetPosition = position;
    }
    

    // Start is called before the first frame update
    void Start()
    {

        animator = GetComponent<Animator>();
        rigidBody = GetComponent<Rigidbody2D>();
        player = FindObjectOfType<Player>();
        targetPosition = GetOwnPosition();
        goblins = FindObjectsOfType<Goblin>();
        furnace = FindObjectOfType<Furnace>();
        uiManager = FindObjectOfType<UIManager>();
        mapGenerator = FindObjectOfType<MapGenerator>();
        DisableScripts();
    }

    void DisableScripts()
    {
        foreach (Goblin goblin in goblins)
            if(goblin)
                goblin.enabled = false;
        player.enabled = false;
        if (furnace.enabled)
        {
            furnace.OutsideUpdate();
            if (modifyFurnace)
                furnace.shouldSkip2 = true;
        }
        furnace.enabled = false;
        uiManager.gameObject.SetActive(false);
        mapGenerator.enabled = false;
    }

    void EnableScripts()
    {
        foreach (Goblin goblin in goblins)
            if(goblin)
                goblin.enabled = true;
        player.enabled = true;
        furnace.enabled = true;
        uiManager.gameObject.SetActive(true);
        mapGenerator.enabled = true;
    }

    Vector2 GetOwnPosition()
    {
        return transform.position + new Vector3(0, -0.5f);
    }
    void ChangeAlignment()
    {

        Vector2 difference = targetPosition - GetOwnPosition();
        if (!moving)
        {
            difference = (Vector2)player.transform.position - GetOwnPosition();
            animator.SetBool("Moving", false);
            animator.SetFloat("Direction", 2f);
            return;
        }

        if (difference.sqrMagnitude < 0.05)
        {
            animator.SetBool("Moving", false);
            return;
        }
        else animator.SetBool("Moving", true);

        float Angle = UseTools.RealVector2Angle(difference);
        while (Angle < -180f)
            Angle += 360f;
        while (Angle > 180f)
            Angle -= 360f;




        if (Angle < -135f || Angle >= 135f)
            Align = 2;
        else if (Angle >= -135f && Angle < -45f)
            Align = 3;
        else if (Angle >= -45f && Angle < 45f)
            Align = 0;
        else if (Angle >= 45f && Angle < 145f)
            Align = 1;
        animator.SetFloat("Direction", Align);

    }

    private void Move()
    {
        if (!moving)
            return;
        float moveSpeed = this.movementSpeed;

        Vector2 difference = targetPosition - GetOwnPosition();
        if (difference.sqrMagnitude < 0.05f)
            rigidBody.velocity = Vector3.zero;
        else rigidBody.velocity = difference.normalized * movementSpeed;
    }

    public void StartMoving()
    {
        moving = true;
        targetPosition = transform.position;
        targetPosition += Random.insideUnitCircle.normalized * 100f;
        Destroy(gameObject, 5f);
        Invoke("EnableScripts", 3f);
    }

    // Update is called once per frame
    void Update()
    {
        if (!moving)
            DisableScripts();
        ChangeAlignment();
        Move();
    }
}
