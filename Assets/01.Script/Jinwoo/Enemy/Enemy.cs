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

    protected Renderer skinRenderer; // 렌더러 컴포넌트

    public float runSpeed = 10f;
    [Range(0.01f, 2f)] public float turnSmoothTime = 0.1f;
    protected float turnSmoothVelocity;

    public float damage = 30f;

    [HideInInspector] public LivingEntity targetEntity; // 추적할 대상
    public LayerMask whatIsTarget; // 추적 대상 레이어


    protected RaycastHit[] hits = new RaycastHit[10];
    protected List<LivingEntity> lastAttackedTargets = new List<LivingEntity>();

    bool isAttacked;


#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        if (attackRoot != null)
        {
            Gizmos.color = new Color(1f, 0f, 0f, 0.5f);
            Gizmos.DrawSphere(attackRoot.position, EnemyData.attackRadius);
        }
        if (dodgeRoot != null)
        {
            Gizmos.color = new Color(1f, 1f, 0f, 0.5f);
            Gizmos.DrawSphere(dodgeRoot.position, EnemyData.dodgeRadius);
        }

    }

#endif

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        skinRenderer = GetComponentInChildren<Renderer>();

        targetEntity = GameObject.Find("Player").GetComponent<LivingEntity>();


        EnemyData.attackDistance = EnemyData.stoppingDistance; 

        agent.stoppingDistance = EnemyData.attackDistance;

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

        this.damage = EnemyData.damage;

        state = State.Tracking;

        agent.speed = runSpeed;
    }

    protected virtual void Start()
    {
        // 게임 오브젝트 활성화와 동시에 AI의 추적 루틴 시작
        StartCoroutine(UpdatePath());
    }
    private void Update()
    {
        if (dead) return;

        if (state == State.Tracking &&
            Vector3.Distance(targetEntity.transform.position, transform.position) <= EnemyData.attackDistance)
        {
            BeginAttack();
        }


        // 추적 대상의 존재 여부에 따라 다른 애니메이션을 재생
        animator.SetFloat("Speed", agent.desiredVelocity.magnitude);
    }

    private void FixedUpdate()
    {
        if (dead) return;

        if (state == State.AttackBegin || state == State.Attacking)
        {
            var lookRotation =
                Quaternion.LookRotation(targetEntity.transform.position - transform.position, Vector3.up);
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

    // 주기적으로 추적할 대상의 위치를 찾아 경로를 갱신
    protected virtual IEnumerator UpdatePath()
    {
        // 살아있는 동안 무한 루프
        while (!dead)
        {

            agent.SetDestination(targetEntity.transform.position);

            


            // 0.2 초 주기로 처리 반복
            yield return new WaitForSeconds(0.2f);
        }
    }

    // 데미지를 입었을때 실행할 처리
    public override bool ApplyDamage(DamageMessage damageMessage)
    {
        if (!base.ApplyDamage(damageMessage)) return false;

        if (targetEntity == null)
        {
            targetEntity = damageMessage.damager.GetComponent<LivingEntity>();
        }

        //EffectManager.Instance.PlayHitEffect(damageMessage.hitPoint, damageMessage.hitNormal, transform, EffectManager.EffectType.Flesh);
        PoolManager.instance.Pop(PoolType.Sound).GetComponent<AudioPoolObject>().Play(EnemyData.hitClip, 1, Random.Range(0.9f, 1.1f));

        return true;
    }

    public void BeginAttack()
    {
        state = State.AttackBegin;
        print("attackbeggin");
        //agent.isStopped = true;
        animator.SetTrigger("Attack");
    }

    public void EnableAttack()
    {
        isAttacked = false;

        print("공격 가능!");

        state = State.Attacking;

        lastAttackedTargets.Clear();
    }

    public void DisableAttack()
    {

        state = State.Tracking;

        //agent.isStopped = false;
    }


    // 사망 처리
    public override void Die()
    {
        // LivingEntity의 Die()를 실행하여 기본 사망 처리 실행
        base.Die();

        state = State.Tracking;

        // 다른 AI들을 방해하지 않도록 자신의 모든 콜라이더들을 비활성화
        //GetComponent<Collider>().enabled = false;

        // AI 추적을 중지하고 내비메쉬 컴포넌트를 비활성화
        agent.enabled = false;

        // 사망 효과음 재생
        PoolManager.instance.Pop(PoolType.Sound).GetComponent<AudioPoolObject>().Play(EnemyData.deathClip, 1, Random.Range(0.9f,1.1f));
    }

}
