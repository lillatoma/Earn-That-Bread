using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goblin : MonoBehaviour
{
    public GameObject onDeathEffect;
    public GameObject goblinPunchSound;
    public float movementSpeed;
    public int health;
    public int maxHealth;
    public int damage;

    public float playerRange;
    public float furnaceRange;
    public float attackRange;
    public Vector2 randomMoveTimeWindow;
    public float randomMoveMaxDistance;

    private Rigidbody2D rigidBody;
    private Animator animator;
    private float Align = 0;
    private int isAttacking = 0; //1 - Player, 2 - Furnace
    private Vector2 targetPosition;
    private Player player;
    private Furnace furnace;
    private bool focused = false;
    private float timeTillNextRandomMove;
    public void SetTargetPosition(Vector2 position)
    {
        targetPosition = position;
    }
    


    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        rigidBody = GetComponent<Rigidbody2D>();
        targetPosition = transform.position;
        player = FindObjectOfType<Player>();
        furnace = FindObjectOfType<Furnace>();

        health = maxHealth;
        timeTillNextRandomMove = Random.Range(randomMoveTimeWindow.x, randomMoveTimeWindow.y);
    }

    Vector2 GetOwnPosition()
    {
        return transform.position + new Vector3(0, -0.5f);
    }
    void ChangeAlignment()
    {

        Vector2 difference = targetPosition - GetOwnPosition();

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
        if (isAttacking != 0)
        {
            rigidBody.velocity = Vector3.zero;
            return;
        }
        float moveSpeed = this.movementSpeed;

        Vector2 difference = targetPosition - GetOwnPosition();
        if (difference.sqrMagnitude < 0.05f)
            rigidBody.velocity = Vector3.zero;
        else rigidBody.velocity = difference.normalized * movementSpeed;
    }

    void CheckRanges()
    {
        if ((player.transform.position - transform.position).sqrMagnitude < playerRange * playerRange)
        {
            targetPosition = player.transform.position;
            focused = true;
            if ((player.transform.position - transform.position).sqrMagnitude < attackRange * attackRange)
            {
                StartAttack(1);
            }
        }
        else if ((furnace.transform.position - transform.position).sqrMagnitude < furnaceRange * furnaceRange)
        {
            targetPosition = furnace.transform.position;
            focused = true;
            if ((furnace.transform.position - transform.position).sqrMagnitude < attackRange * attackRange)
            {
                StartAttack(2);
            }
        }
        else focused = false;
    }

    void RandomMove()
    {
        if (!focused)
        {
            timeTillNextRandomMove -= Time.deltaTime;
            if (timeTillNextRandomMove <= 0f)
            {
                Vector3 randomPoint = Random.insideUnitCircle.normalized * Random.Range(0.5f*randomMoveMaxDistance,randomMoveMaxDistance);
                randomPoint += transform.position;
                targetPosition = randomPoint;
                timeTillNextRandomMove = Random.Range(randomMoveTimeWindow.x, randomMoveTimeWindow.y);
            }
        }
    }

    void StartAttack(int i)
    {
        isAttacking = i;
        animator.SetBool("Attacking", true);
    }

    public void FinishAttack()
    {
        if(isAttacking == 1)
        {
            if ((player.transform.position - transform.position).sqrMagnitude < attackRange * attackRange)
            {
                GameObject go = Instantiate(goblinPunchSound);
                go.transform.position = transform.position;
                player.Damage(damage);
            }
        }
        else if (isAttacking == 2)
        {
            if ((furnace.transform.position - transform.position).sqrMagnitude < attackRange * attackRange)
            {
                GameObject go = Instantiate(goblinPunchSound);
                go.transform.position = transform.position;
                furnace.Damage(damage);
            }
        }
        isAttacking = 0;
        animator.SetBool("Attacking", false);
    }

    public void Damage(int damage)
    {
        health -= damage;
    }


    void CheckDying()
    {
        if(health <= 0)
        {

            GameObject go = Instantiate(onDeathEffect);
            go.transform.position = transform.position;

            while (true)
            {
                bool got = false;
                if (Random.value < 0.35)
                {
                    player.stone++;
                    got = true;
                }
                if (Random.value < 0.35)
                {
                    player.wheat++;
                    got = true;
                }
                if (Random.value < 0.35)
                {
                    player.iron++;
                    got = true;
                }
                if (Random.value < 0.35)
                {
                    player.coal++;
                    got = true;
                }
                if (!got)
                    break;
            }
            Destroy(gameObject);
        }
    }
   
    // Update is called once per frame
    void Update()
    {
        ChangeAlignment();
        CheckRanges();
        Move();
        CheckDying();
        RandomMove();
    }
}
