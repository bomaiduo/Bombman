using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    EnemyBaseState currentState;

    public Animator anim;
    public int animState;

    private GameObject alarmSigh;

    [Header("Base State")]
    public float health;
    public bool isDead;
    public bool hasBomb;
    public bool isBoss;

    [Header("Movement")]
    public float speed;
    public Transform pointA, pointB;
    public Transform targetPoint;

    [Header("Attack Setting")]
    public float attackRate;
    private float nextAttack = 0;
    public float attackRange, skillRange;

    public List<Transform> attackList = new List<Transform>();

    public PatrolState patrolState = new PatrolState();
    public AttackState attackState = new AttackState();

    public virtual void Init()
    {
        anim = GetComponent<Animator>();
        alarmSigh = transform.GetChild(0).gameObject;
    }

    public void Awake()
    {
        Init();
    }

    // Start is called before the first frame update
    void Start()
    {
        TransitionToState(patrolState);
        if(isBoss)
            UIManager.instance.SetBossHealth(health);
    }

    // Update is called once per frame
    public virtual void Update()
    {
        anim.SetBool("dead", isDead);
        if (isDead)
            return;

        currentState.OnUpdate(this);
        anim.SetInteger( "state", animState);

        if (isBoss)
            UIManager.instance.UpdateBossHealth(health);
    }

    public void TransitionToState(EnemyBaseState state)
    {
        currentState = state;
        currentState.EnterState(this);
    }

    public void MoveToTarget()
    {
        transform.position = Vector2.MoveTowards(transform.position, targetPoint.position, speed * Time.deltaTime);
        FilpDirection();
    }

    public void AttackAction()
    {
        if(Vector2.Distance(transform.position ,targetPoint.position)< attackRange)
        {
            if(Time.time > nextAttack)
            {
                anim.SetTrigger("attack");
                nextAttack = Time.time + attackRate;
            }
        }
    }

    public virtual void SkillAction()
    {
        if (Vector2.Distance(transform.position, targetPoint.position) < skillRange)
        {
            if (Time.time > nextAttack)
            {
                anim.SetTrigger("skill");
                nextAttack = Time.time + attackRate;
            }
        }
    }

    public void FilpDirection()
    {
        if (transform.position.x < targetPoint.position.x)
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        else
            transform.rotation = Quaternion.Euler(0f, 180f, 0f);
    }

    public void SwitchPoint()
    {
        if (Mathf.Abs(pointA.position.x - transform.position.x) > Mathf.Abs(pointB.position.x - transform.position.x))
        {
            targetPoint = pointA;
        }
        else
        {
            targetPoint = pointB;
        }
    }

    public void OnTriggerStay2D(Collider2D collision)
    {
        if (!attackList.Contains(collision.transform) && !hasBomb && !isDead && !GameManager.Instance.gameOver)
        {
            attackList.Add(collision.transform); 
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        attackList.Remove(collision.transform);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(!isDead && !GameManager.Instance.gameOver)
            StartCoroutine(OnAlarm());
    }

    IEnumerator OnAlarm()
    {
        alarmSigh.SetActive(true);
        yield return new WaitForSeconds(alarmSigh.GetComponent<Animator>().GetCurrentAnimatorClipInfo(0)[0].clip.length);
        alarmSigh.SetActive(false);
    }
}
