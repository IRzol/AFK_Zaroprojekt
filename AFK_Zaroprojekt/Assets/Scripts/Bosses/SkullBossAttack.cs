using System.Collections;
using UnityEngine;

public class SkullBossAttack : MonoBehaviour
{
    public Animator animator;

    public float slamSpeed = 20f;
    public float riseSpeed = 8f;
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
            yield return SlamAttack();
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
                transform.position = Vector2.MoveTowards(transform.position, playerTarget, 3f * Time.deltaTime);
                yield return null;
            }

            animator.Play("Skull_OpenMouth");
            yield return new WaitForSeconds(1f);

            //Lecsapódik
            startPosition = transform.position;
            while (Vector2.Distance(transform.position, slamTargetPos) > 0.05f)
            {
                slamTargetPos = new Vector2(transform.position.x, -2f);
                transform.position = Vector2.MoveTowards(transform.position, slamTargetPos, slamSpeed * Time.deltaTime);
                yield return null;
            }
            yield return new WaitForSeconds(1.5f);

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
        }
    }
}