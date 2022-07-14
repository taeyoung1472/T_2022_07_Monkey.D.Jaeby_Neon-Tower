using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DashEnemy : LivingEntity
{
    protected enum State
    {
        Tracking,
        AttackBegin,
        Attacking,
        Dash,
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

    protected AudioSource audioPlayer; // 오디오 소스 컴포넌트

    protected Renderer skinRenderer; // 렌더러 컴포넌트

    public float runSpeed = 10f;
    [Range(0.01f, 2f)] public float turnSmoothTime = 0.1f;
    protected float turnSmoothVelocity;

    public float damage = 30f;
    public bool isDash = false;
    public float dashTime = 0.5f;

    [HideInInspector] public LivingEntity targetEntity; // 추적할 대상
    public LayerMask whatIsTarget; // 추적 대상 레이어


    protected RaycastHit[] hits = new RaycastHit[10];
    protected List<LivingEntity> lastAttackedTargets = new List<LivingEntity>();

    bool isAttacked;


    public Material orignMat;
    public Material damageMat;

    private MeshRenderer meshRenderer;
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
        audioPlayer = GetComponent<AudioSource>();
        skinRenderer = GetComponentInChildren<Renderer>();
        meshRenderer = GetComponent<MeshRenderer>();
        targetEntity = GameObject.Find("Player").GetComponent<LivingEntity>();


        agent.stoppingDistance = EnemyData.stoppingDistance;
        //EnemyData.attackDistance = EnemyData.stoppingDistance;

        //agent.stoppingDistance = EnemyData.attackDistance;
        isDash = false;
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

        if (state == State.AttackBegin || state == State.Attacking || state == State.Dash)
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
    IEnumerator Dash()
    {
        agent.isStopped = true;
        agent.stoppingDistance = 1;
        yield return new WaitForSeconds(0.5f);
        
        agent.isStopped = false;
        float startTime = Time.time;

        while (Time.time < startTime + dashTime)
        {
            agent.speed = EnemyData.dashSpeed;


            yield return null;
        }
        isDash = false;
        agent.speed = EnemyData.maxSpeed;
        DashAttack();
    }
    protected virtual void DashAttack() 
    {
        var direction = transform.forward;
        var deltaDistance = agent.velocity.magnitude * Time.deltaTime;

        var size = Physics.SphereCastNonAlloc(attackRoot.position, EnemyData.attackRadius, direction, hits, deltaDistance,
            whatIsTarget);

        for (var i = 0; i < size; i++)
        {
            var attackTargetEntity = hits[i].collider.GetComponent<LivingEntity>();

            if (attackTargetEntity != null && !lastAttackedTargets.Contains(attackTargetEntity))
            {
                var message = new DamageMessage();
                message.amount = EnemyData.dashDamage;
                message.damager = gameObject;
                message.hitPoint = attackRoot.TransformPoint(hits[i].point);
                message.hitNormal = attackRoot.TransformDirection(hits[i].normal);

                attackTargetEntity.ApplyDamage(message);

                lastAttackedTargets.Add(attackTargetEntity);
                break;
            }
        }
        state = State.Tracking;
        agent.stoppingDistance = EnemyData.attackDistance;
    }
    protected virtual void Attack()
    {
        Collider[] cols = Physics.OverlapSphere(transform.position, EnemyData.attackRadius, whatIsTarget);

        if(cols.Length > 0)
        {
            Define.Instance.controller.Damaged();
        }

        isAttacked = true;
        /*var size = Physics.SphereCastNonAlloc(attackRoot.position, EnemyData.attackRadius, direction, hits, deltaDistance,
            whatIsTarget);

        for (var i = 0; i < size; i++)
        {
            var attackTargetEntity = hits[i].collider.GetComponent<LivingEntity>();

            if (attackTargetEntity != null && !lastAttackedTargets.Contains(attackTargetEntity))
            {
                var message = new DamageMessage();
                message.amount = damage;
                message.damager = gameObject;
                message.hitPoint = attackRoot.TransformPoint(hits[i].point);
                message.hitNormal = attackRoot.TransformDirection(hits[i].normal);

                attackTargetEntity.ApplyDamage(message);

                lastAttackedTargets.Add(attackTargetEntity);
                break;
            }
        }*/
    }


    // 주기적으로 추적할 대상의 위치를 찾아 경로를 갱신
    protected virtual IEnumerator UpdatePath()
    {
        // 살아있는 동안 무한 루프
        while (!dead)
        {

            //Debug.Log("남은 거리" + agent.remainingDistance);
            if (state == State.Tracking && isDash == false)
            {
                if(agent.remainingDistance >= 4f && agent.remainingDistance <= 5f)
                {
                    isDash = true;
                    print("대쉬 시작");
                    state = State.Dash;

                    //agent.SetDestination(targetEntity.transform.position);
                    StartCoroutine(Dash());

                }

            }
            else
            {

            }

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
        StartCoroutine(ChangeMaterial());
        //EffectManager.Instance.PlayHitEffect(damageMessage.hitPoint, damageMessage.hitNormal, transform, EffectManager.EffectType.Flesh);
        audioPlayer.PlayOneShot(EnemyData.hitClip);

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
        state = State.Attacking;

        lastAttackedTargets.Clear();

        isAttacked = false;
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
        StopCoroutine("ChangeMaterial");

        state = State.Tracking;

        // 다른 AI들을 방해하지 않도록 자신의 모든 콜라이더들을 비활성화
        GetComponent<Collider>().enabled = false;

        // AI 추적을 중지하고 내비메쉬 컴포넌트를 비활성화
        agent.enabled = false;

        // 사망 애니메이션 재생
        animator.applyRootMotion = true;
        animator.SetTrigger("Die");

        // 사망 효과음 재생
        if (EnemyData.deathClip != null) audioPlayer.PlayOneShot(EnemyData.deathClip);
    }

    IEnumerator ChangeMaterial()
    {
        meshRenderer.material = damageMat;
        yield return new WaitForSeconds(.25f);
        meshRenderer.material = orignMat;
    }
}
