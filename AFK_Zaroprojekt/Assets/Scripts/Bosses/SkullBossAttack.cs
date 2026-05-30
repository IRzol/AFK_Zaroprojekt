using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class SkullBossAttack : MonoBehaviour
{
    public Animator animator;

    public float slamSpeed = 20f;
    public float riseSpeed = 8f;
    public float sideSpeed = 4f;
    public float timeForSlam = 1f;
    public bool isAlive = true;
    public Transform player;
    private Vector2 startPosition;

    public GameObject toothPrefab;

    private void Start()
    {
        startPosition = transform.position;
        StartCoroutine(BossLoop());
    }

    private IEnumerator BossLoop()
    {
        while (isAlive)
        {
            yield return ChooseAttack();
            yield return new WaitForSeconds(1f);
        }
    }

    private IEnumerator SlamAttack()
    {
        for (int i = 2; i >= 0; i--)
        {
            Vector2 slamTargetPos = new Vector2(transform.position.x, -1.5f);
            Vector2 playerTarget = new Vector2(player.position.x, transform.position.y);

            //Addig megy oldalra, amig elnem eri a jatekos x erteket
            while (Vector2.Distance(playerTarget, transform.position) > 0.05f)
            {
                playerTarget = new Vector2(player.position.x, transform.position.y);
                transform.position = Vector2.MoveTowards(transform.position, playerTarget, sideSpeed * Time.deltaTime);
                yield return null;
            }
            startPosition = transform.position;
            animator.Play("Skull_OpenMouth");
            yield return new WaitForSeconds(timeForSlam);

            //Lecsapódik
            
            while (Vector2.Distance(transform.position, slamTargetPos) > 0.05f)
            {
                slamTargetPos = new Vector2(transform.position.x, -1.5f);
                transform.position = Vector2.MoveTowards(transform.position, slamTargetPos, slamSpeed * Time.deltaTime);
                yield return null;
            }
            yield return new WaitForSeconds(1.1f);

            //Felemelkedik az eredeti magasságba
            while (Vector2.Distance(transform.position, startPosition) > 0.05f)
            {
                animator.Play("Skull_CloseMouth");
                transform.position = Vector2.MoveTowards(transform.position,startPosition,riseSpeed * Time.deltaTime);
                yield return null;
            } 
            
            sideSpeed += 0.03f;
            if(timeForSlam >= 0.2) 
            { 
                timeForSlam -= 0.05f;
            }

        }
    }
    private IEnumerator ChooseAttack()
    {
        int randomAttack = Random.Range(0, 2); // 0 vagy 1

        if (randomAttack == 0)
        {
            yield return SlamAttack();
        }
        else
        {
            yield return TeethAttack();
        }
    }

    private IEnumerator TeethAttack()
    {
        bool goRight = Random.value > 0.5f;

        Vector2 leftShootPos = new Vector2(14.3f, transform.position.y);
        Vector2 rightShootPos = new Vector2(24.7f, transform.position.y);

        Vector2 target = goRight ? rightShootPos : leftShootPos;

        while (Vector2.Distance(transform.position, target) > 0.05f)
        {
            transform.position = Vector2.MoveTowards(transform.position, target, sideSpeed * Time.deltaTime);
            yield return null;
        }

        Vector2 direction = player.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.Euler(0f, 0f, angle + 90f);

        float rotateTime = 0f;
        while (rotateTime < 0.5f)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 180f * Time.deltaTime);
            rotateTime += Time.deltaTime;
            yield return null;
        }

        animator.Play("Skull_OpenMouth");
        yield return new WaitForSeconds(0.3f);
        ShootTeeth();
        yield return new WaitForSeconds(1f);
        animator.Play("Skull_CloseMouth");

        transform.rotation = Quaternion.identity;
    }

    void ShootTeeth()
    {
        Vector2 dirToPlayer = (player.position - transform.position).normalized;

        Vector2[] directions = {
        dirToPlayer,
        Rotate(dirToPlayer, 30f),
        Rotate(dirToPlayer, -30f)
    };

        foreach (Vector2 dir in directions)
        {
            GameObject tooth = Instantiate(toothPrefab, transform.position, Quaternion.identity);
            tooth.GetComponent<Tooth>().SetDirection(dir);
        }
    }

    Vector2 Rotate(Vector2 v, float degrees)
    {
        float rad = degrees * Mathf.Deg2Rad;
        return new Vector2(
            v.x * Mathf.Cos(rad) - v.y * Mathf.Sin(rad),
            v.x * Mathf.Sin(rad) + v.y * Mathf.Cos(rad)
        );
    }
}