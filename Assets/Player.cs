using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameObject axeSwingSound;
    public GameObject deathMenu;
    public GameObject onDeathEffect;
    public Camera mainCamera;
    public int health;
    public int maxHealth = 100;
    public int damage = 20;
    public float attackRange;
    public float movementSpeed;
    public int stone;
    public int iron;
    public int wheat;
    public int coal;
    public int bread;


    private Rigidbody2D rigidBody;
    private Animator animator;
    private float Align = 0;

    private Vector2 targetPosition;
    private StoneTilesMapInteractor stoneTiles;
    private WheatTileMapInteractor wheatTiles;
    private int isAttacking = 0;
    private int isMining = 0; //0 - nothing, 1 - stonelike, 2 - wheat
    private Vector3Int minableBlock;
    private GameObject attackTarget;
    public void SetTargetPosition(Vector2 position)
    {
        targetPosition = position;
    }
    void OnClick()
    {

        if (Input.GetMouseButtonDown(1))
        {
            targetPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            animator.SetBool("Mining", false);
            animator.SetBool("Attacking", false);
            minableBlock = new Vector3Int(0, 0, 10000);
        }
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

            RaycastHit2D hit = Physics2D.GetRayIntersection(ray, Mathf.Infinity);

            if(hit.collider)
            {
                Goblin goblin = hit.transform.GetComponent<Goblin>();
                if (goblin)
                {
                    animator.SetBool("Mining", false);
                    attackTarget = goblin.gameObject;
                    targetPosition = goblin.transform.position;
                }
                else
                    SetupPlayerToMine();
            }  
            else
                SetupPlayerToMine();
        }
    }

    public void PlayAxeSound()
    {
        GameObject go = Instantiate(axeSwingSound);
        go.transform.position = transform.position;
    }

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        rigidBody = GetComponent<Rigidbody2D>();
        targetPosition = GetOwnPosition();
        stoneTiles = FindObjectOfType<StoneTilesMapInteractor>();
        wheatTiles = FindObjectOfType<WheatTileMapInteractor>();

        health = maxHealth;
    }

    public Vector2 GetOwnPosition()
    {
        return transform.position + new Vector3(0, -0.5f);
    }
    void ChangeAlignment()
    {
        if (isMining != 0)
        {
            Vector2 difference = new Vector2(minableBlock.x + 0.5f, minableBlock.y + 0.5f) - GetOwnPosition();

            if (difference.sqrMagnitude < 0.05)
            {
                animator.SetBool("Moved", false);
                return;
            }
            else animator.SetBool("Moved", true);

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
            return;
        }
        if (isAttacking != 0)
        {
            if (!attackTarget)
                return;
            Vector2 difference = new Vector2(attackTarget.transform.position.x, attackTarget.transform.position.y) - new Vector2(0, 0.25f) - GetOwnPosition();

            if (difference.sqrMagnitude < 0.05)
            {
                animator.SetBool("Moved", false);
                return;
            }
            else animator.SetBool("Moved", true);

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
            return;
        }

        else
        {
            Vector2 difference = targetPosition - GetOwnPosition() ;

            if (difference.sqrMagnitude < 0.05)
            {
                animator.SetBool("Moved", false);
                return;
            }
            else animator.SetBool("Moved", true);

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
    }

    private void Move()
    {
        if(isMining != 0)
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

    private void SetupPlayerToMine()
    {
        Vector3Int[] poses = new Vector3Int[4] { new Vector3Int(1, 0, 0), new Vector3Int(0, 1, 0), new Vector3Int(-1, 0, 0), new Vector3Int(0, -1, 0)};
        Vector3Int activeTile = stoneTiles.GetActiveTileCoord();
        if (activeTile.z == 0)
        {
            float distance = 10000000;
            foreach(Vector3Int pos in poses)
                if(!stoneTiles.GetTileAtCoord(activeTile + pos))
                {
                    Vector2 vectDist = transform.position;
                    vectDist -= new Vector2((activeTile + pos).x, (activeTile + pos).y);
                    float cDistance = vectDist.sqrMagnitude;
                    if (cDistance < distance)
                    {
                        targetPosition = new Vector2((activeTile + pos).x + 0.5f, (activeTile + pos).y + 0.5f);
                        minableBlock = activeTile;
                        distance = cDistance;
                    }
                }
            animator.SetBool("Attacking", false);
            return;
        }
        activeTile = wheatTiles.GetActiveTileCoord();
        if (activeTile.z == 0)
        {
            float distance = 10000000;
            foreach (Vector3Int pos in poses)
                if (!stoneTiles.GetTileAtCoord(activeTile + pos))
                {
                    Vector2 vectDist = transform.position;
                    vectDist -= new Vector2((activeTile + pos).x, (activeTile + pos).y);
                    float cDistance = vectDist.sqrMagnitude;
                    if (cDistance < distance)
                    {
                        targetPosition = new Vector2((activeTile + pos).x + 0.5f, (activeTile + pos).y + 0.5f);
                        minableBlock = activeTile;
                        distance = cDistance;
                    }
                }
            animator.SetBool("Attacking", false);
        }
    }

    private void Mine()
    {
        if (minableBlock.z != 0)
        {
            animator.SetBool("Mining", false);
            isMining = 0;
            return;
        }
        Vector2 difference = new Vector2(minableBlock.x+0.5f,minableBlock.y+0.5f) - GetOwnPosition();
        if(difference.sqrMagnitude < 1.5)
        {
            if (isMining == 0 && stoneTiles.GetTileAtCoord(minableBlock))
            {
                isMining = 1;
                animator.SetBool("Mining", true);
            }
            else if (isMining == 0 && wheatTiles.GetTileAtCoord(minableBlock))
            {
                isMining = 2;
                animator.SetBool("Mining", true);
            }
        }
    }

    private void CheckMinable()
    {
        if (minableBlock.z != 0)
            return;
        if (!stoneTiles.GetTileAtCoord(minableBlock) && !wheatTiles.GetTileAtCoord(minableBlock))
        {
            minableBlock.z = 5000;
            animator.SetBool("Mining", false);
            isMining = 0;
        }
    }

    public void FinishMining()
    {
        if (minableBlock.z == 0)
        {
            if ((new Vector2(minableBlock.x, minableBlock.y) - (Vector2)transform.position).sqrMagnitude < 6)
            {
                if (isMining == 1)
                    stoneTiles.Mine(minableBlock);
                else if (isMining == 2)
                    wheatTiles.Mine(minableBlock);
                CheckMinable();
            }
        }
        animator.SetBool("Mining", false);
    }

    public void Damage(int damage)
    {
        health -= damage;
    }

    public void CheckAttack()
    {
        if(attackTarget)
        {
            if ((attackTarget.transform.position - new Vector3(0, 0.25f, 0) - transform.position).sqrMagnitude < attackRange * attackRange)
            {
                isAttacking = 1;
                animator.SetBool("Attacking", true);
            }
            else
                animator.SetBool("Attacking", false);
        }
    }

    public void CancelAttack()
    {
        isAttacking = 0;
        attackTarget = null;
        animator.SetBool("Attacking", false);
    }

    public void DealDamage()
    {
        if (attackTarget && ((Vector2)attackTarget.transform.position - (Vector2)transform.position).sqrMagnitude < attackRange * attackRange)
        {
            Goblin goblin = attackTarget.GetComponent<Goblin>();
            goblin.Damage(damage);
            if (goblin.health < 0)
                CancelAttack();
        }
        else CancelAttack();
    }

    public void OnDeath()
    {
        rigidBody.velocity = Vector3.zero;
        if (mainCamera.orthographicSize > 1.5f)
        {
            mainCamera.orthographicSize -= 5f * Time.deltaTime;
            return;
        }
        Destroy(FindObjectOfType<CookingSpawner>());
        Destroy(FindObjectOfType<MapGenerator>());
        FindObjectOfType<Camera>().transform.parent = null;
        GameObject go = Instantiate(onDeathEffect);
        go.transform.position = transform.position;
        var goblins = FindObjectsOfType<Goblin>();
        foreach (Goblin goblin in goblins)
        {
            Destroy(goblin.GetComponent<Animator>());
            Destroy(goblin);
        }
        var rb2s = FindObjectsOfType<Rigidbody2D>();
        foreach (Rigidbody2D rb in rb2s)
            Destroy(rb);
        deathMenu.SetActive(true);
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (health > 0)
        {
            OnClick();
            ChangeAlignment();
            Move();
            CheckMinable();
            Mine();
            CheckAttack();
        }
        else OnDeath();

    }
}
