using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
#if UNITY_EDITOR
#endif
public class Enemy : LivingEntity
{
    protected enum State
    {
        Tracking,
        AttackBegin,
        Attacking,
        Dash
    }
    [SerializeField]
    private EnemyDataSO _enemyData;
    public EnemyDataSO EnemyData
    {
        get => _enemyData;
        set
        {
            _enemyData = value;
        }
    }

    protected State state;

    protected NavMeshAgent agent; // 경로계산 AI 에이전트
    protected Animator animator; // 애니메이터 컴포넌트

    public Transform attackRoot;
    public Transform dodgeRoot;

    public float runSpeed = 10f;
    [Range(0.01f, 2f)] public float turnSmoothTime = 0.1f;
    protected float turnSmoothVelocity;

    Transform player; // 추적할 대상
    public LayerMask whatIsTarget; // 추적 대상 레이어

    bool isAttacked;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        player = Define.Instance.controller.transform;


        EnemyData.attackDistance = EnemyData.stoppingDistance; 

        agent.stoppingDistance = EnemyData.attackDistance * 0.75f;

        //나중에 지울거임
        Setup();
    }
    // 적 AI의 초기 스펙을 결정하는 셋업 메서드
    protected virtual void Setup()
    {
        // 체력 설정
        this.health = EnemyData.maxHealth;

        // 내비메쉬 에이전트의 이동 속도 설정  
        this.runSpeed = EnemyData.maxSpeed;

        state = State.Tracking;

        agent.speed = runSpeed;
    }

    protected virtual void Start()
    {
        StartCoroutine(UpdatePath());
    }
    private void Update()
    {
        if (dead) return;

        if (state == State.Tracking &&
            Vector3.Distance(player.position, transform.position) <= EnemyData.attackDistance)
        {
            BeginAttack();
        }

        animator.SetFloat("Speed", agent.desiredVelocity.magnitude);
    }

    private void FixedUpdate()
    {
        if (dead) return;

        if (state == State.AttackBegin || state == State.Attacking)
        {
            var lookRotation =
                Quaternion.LookRotation(player.position - transform.position, Vector3.up);
            var targetAngleY = lookRotation.eulerAngles.y;

            transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngleY,
                                        ref turnSmoothVelocity, turnSmoothTime);
        }

        if (state == State.Attacking && !isAttacked)
        {
            Attack();
        }
    }

    protected virtual void Attack()
    {
        Collider[] cols = Physics.OverlapSphere(attackRoot.position, _enemyData.attackRadius, whatIsTarget);

        if(cols.Length > 0)
        {
            Define.Instance.controller.Damaged();
        }

        isAttacked = true;
    }

    protected virtual IEnumerator UpdatePath()
    {
        // 살아있는 동안 무한 루프
        while (!dead)
        {

            agent.SetDestination(player.position);

            yield return new WaitForSeconds(0.2f);
        }
    }

    public override bool ApplyDamage(DamageMessage damageMessage)
    {
        if (!base.ApplyDamage(damageMessage)) return false;

        PoolManager.instance.Pop(PoolType.Sound).GetComponent<AudioPoolObject>().Play(EnemyData.hitClip, 1, Random.Range(0.9f, 1.1f));
        PoolManager.instance.Pop(PoolType.Popup).GetComponent<PopupPoolObject>().PopupTextCritical(transform.position, "1");
        return true;
    }

    public void BeginAttack()
    {
        state = State.AttackBegin;
        animator.SetTrigger("Attack");
    }

    public void EnableAttack()
    {
        isAttacked = false;

        state = State.Attacking;
    }

    public void DisableAttack()
    {
        state = State.Tracking;
    }


    // 사망 처리
    public override void Die()
    {
        PoolManager.instance.Pop(PoolType.EnemyDeadImpact).GetComponentInParent<ParticlePool>().Set(transform.position + Vector3.up * 1f, Quaternion.identity);
        PoolManager.instance.Pop(PoolType.Sound).GetComponent<AudioPoolObject>().Play(EnemyData.deathClip, 1, Random.Range(0.9f, 1.1f));

        int rand = Random.Range(2, 5);
        for (int i = 0; i < rand; i++)
        {
            GameObject obj = PoolManager.instance.Pop(PoolType.ExpBall);
            Vector2 randVec = Random.insideUnitCircle * 1f;
            obj.transform.position = transform.position + new Vector3(randVec.x, 0, randVec.y);
        }

        Destroy(gameObject);
    }

    //IEnumerator 
}
