using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class SkullBossAttack : MonoBehaviour
{
    public Animator animator;

    public float slamSpeed = 20f;
    public float riseSpeed = 8f;
    public float sideSpeed = 3f;
    public float timeForSlam = 1f;
    public bool isAlive = true;
    public Transform player;
    private Vector2 startPosition;


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
            Vector2 slamTargetPos = new Vector2(transform.position.x, -2f);
            Vector2 playerTarget = new Vector2(player.position.x, transform.position.y);

            //Addig megy oldalra, amig elnem eri a jatekos x erteket
            while (Vector2.Distance(playerTarget, transform.position) > 0.05f)
            {
                playerTarget = new Vector2(player.position.x, transform.position.y);
                transform.position = Vector2.MoveTowards(transform.position, playerTarget, sideSpeed * Time.deltaTime);
                yield return null;
            }

            animator.Play("Skull_OpenMouth");
            yield return new WaitForSeconds(timeForSlam);

            //Lecsapódik
            startPosition = transform.position;
            while (Vector2.Distance(transform.position, slamTargetPos) > 0.05f)
            {
                slamTargetPos = new Vector2(transform.position.x, -2f);
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
            if (i == 0)
            {
                isAlive = false;
            }
            sideSpeed += 1;
            timeForSlam -= 0.1f;

        }
    }
    private IEnumerator ChooseAttack()
    {
        int randomAttack = Random.Range(0, 2); // 0 vagy 1

        //if (randomAttack == 0)
        //{
        //    yield return SlamAttack();
        //}
        //else
        //{
        yield return TeethAttack();
        //}
    }

    private IEnumerator TeethAttack()
    {
        bool goRight = Random.value > 0.5f;

        Vector2 leftShootPos = new Vector2(14.3f, transform.position.y);
        Vector2 rightShootPos = new Vector2(24.7f, transform.position.y);

        Vector2 target = goRight ? rightShootPos : leftShootPos;

        while (Vector2.Distance(transform.position, target) > 0.05f)
        {
            transform.position = Vector2.MoveTowards(transform.position, target, (sideSpeed+1) * Time.deltaTime);
            yield return null;
        }

        Vector2 direction = player.position - transform.position;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        while (transform.rotation.z != angle + 90f)
        {
            
            Quaternion targetRotation = Quaternion.Euler(0f, 0f, angle + 90f);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 180f * Time.deltaTime);
            yield return null;
        }
        animator.Play("Skull_OpenMouth");

        //transform.rotation = Quaternion.identity;

        isAlive = false;
    }
}