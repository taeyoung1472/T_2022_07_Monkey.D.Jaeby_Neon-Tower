using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
#if UNITY_EDITOR
#endif
public class Enemy : LivingEntity, IEnemy
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

    float freezeTimer;
    Vector3 knockbackForce;

    public Material orignMat;
    public Material damageMat;

    private MeshRenderer meshRenderer;
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        meshRenderer = GetComponent<MeshRenderer>();

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
        EnemySubject.instance.RegisterObserver(this);
    }

    private void Update()
    {
        if (dead) return;

        if (state == State.Tracking &&
            Vector3.Distance(player.position, transform.position) <= EnemyData.attackDistance)
        {
            BeginAttack();
        }

        FreezeAndKnockbackSystem();

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

    public override bool ApplyDamage(DamageMessage damageMessage)
    {
        if (!base.ApplyDamage(damageMessage)) return false;

        DamagedFeedback(damageMessage);

        return true;
    }

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

        Define.Instance.controller.StealHp();

        //EnemySubject.instance.RemoveObserver(this);

        Destroy(gameObject);
    }
    public void KnockBack(Vector3 dir, float force)
    {
        knockbackForce = dir.normalized * force;
    }
    public void Freeze(float duration)
    {
        freezeTimer = duration;
    }
    void DamagedFeedback(DamageMessage damageMessage)
    {
        StartCoroutine(ChangeMaterial());
        PoolManager.instance.Pop(PoolType.Sound).GetComponent<AudioPoolObject>().Play(EnemyData.hitClip, 1, Random.Range(0.9f, 1.1f));
        PoolManager.instance.Pop(PoolType.Popup).GetComponent<PopupPoolObject>().PopupTextCritical(transform.position, $"{damageMessage.amount:0.0}");
    }
    void FreezeAndKnockbackSystem()
    {
        if (freezeTimer > 0)
        {
            freezeTimer -= Time.deltaTime;
            agent.isStopped = true;
        }
        else
        {
            agent.isStopped = false;
        }

        knockbackForce = Vector3.Lerp(knockbackForce, Vector3.zero, Time.deltaTime * 2.5f);
        if (knockbackForce.magnitude > 0.2f)
        {
            agent.velocity = knockbackForce;
        }
    }


    IEnumerator ChangeMaterial()
    {
        meshRenderer.material = damageMat;
        yield return new WaitForSeconds(.25f);
        meshRenderer.material = orignMat;
    }

    public override void ObserverUpdate()
    {
        Die();
    }
}
