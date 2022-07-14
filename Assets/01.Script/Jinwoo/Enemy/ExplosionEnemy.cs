using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ExplosionEnemy : LivingEntity, IEnemy
{
    protected enum State
    {
        Tracking,
        AttackBegin,
        Attacking,
        
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

    protected NavMeshAgent agent; // ��ΰ�� AI ������Ʈ
    protected Animator animator; // �ִϸ����� ������Ʈ

    public Transform attackRoot;
    public Transform dodgeRoot;

    protected AudioSource audioPlayer; // ����� �ҽ� ������Ʈ

    protected Renderer skinRenderer; // ������ ������Ʈ

    public float runSpeed = 10f;
    [Range(0.01f, 2f)] public float turnSmoothTime = 0.1f;
    protected float turnSmoothVelocity;

    public float damage = 30f;

    [HideInInspector] public LivingEntity targetEntity; // ������ ���
    public LayerMask whatIsTarget; // ���� ��� ���̾�


    protected RaycastHit[] hits = new RaycastHit[10];
    protected List<LivingEntity> lastAttackedTargets = new List<LivingEntity>();


    public Explosion explosionEffect;

    float freezeTimer;
    Vector3 knockbackForce;


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

        targetEntity = GameObject.Find("Player").GetComponent<LivingEntity>();


        EnemyData.attackDistance = EnemyData.stoppingDistance;

        agent.stoppingDistance = EnemyData.attackDistance;

        //���߿� �������
        Setup();
    }
    // �� AI�� �ʱ� ������ �����ϴ� �¾� �޼���
    protected virtual void Setup()
    {
        // ü�� ����
        this.health = EnemyData.maxHealth;

        // ����޽� ������Ʈ�� �̵� �ӵ� ����  
        this.runSpeed = EnemyData.maxSpeed;

        this.damage = EnemyData.damage;

        state = State.Tracking;

        agent.speed = runSpeed;
    }

    protected virtual void Start()
    {
        // ���� ������Ʈ Ȱ��ȭ�� ���ÿ� AI�� ���� ��ƾ ����
        StartCoroutine(UpdatePath());
    }
    private void Update()
    {
        if (dead) return;

        if (state == State.Tracking &&
            Vector3.Distance(targetEntity.transform.position, transform.position) <= EnemyData.explosionDistance)
        {
            BeginAttack();
        }

        FreezeAndKnockbackSystem();

        // ���� ����� ���� ���ο� ���� �ٸ� �ִϸ��̼��� ���
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

        if (state == State.Attacking)
        {
            Attack();
        }
    }

    protected virtual void Attack()
    {
        explosionEffect.gameObject.SetActive(true);
        explosionEffect.transform.SetParent(null);

        Collider[] cols = Physics.OverlapSphere(transform.position, EnemyData.explosionRange, whatIsTarget);

        if(cols.Length > 0)
        {
            Define.Instance.controller.Damaged();
        }

        OnDeath?.Invoke();
        //.SphereCastNonAlloc(attackRoot.position, EnemyData.explosionRange, direction, hits, EnemyData.explosionDistance,
        //whatIsTarget);

        //var attackTargetEntity = hits[0].collider.GetComponent<LivingEntity>();

        //if (explosionEffect._isDamage)
        //{
        //    var message = new DamageMessage();
        //    message.amount = EnemyData.explosionDamage;
        //    message.damager = gameObject;
        //    message.hitPoint = attackRoot.TransformPoint(hits[0].point);
        //    message.hitNormal = attackRoot.TransformDirection(hits[0].normal);

        //    attackTargetEntity.ApplyDamage(message);

        //    lastAttackedTargets.Add(attackTargetEntity);
        //    print("���ݼ���");
        //}

        /*for (var i = 0; i < size; i++)
        {
            var attackTargetEntity = hits[i].collider.GetComponent<LivingEntity>();

            if (attackTargetEntity != null && !lastAttackedTargets.Contains(attackTargetEntity))
            {
                var message = new DamageMessage();
                message.amount = EnemyData.explosionDamage;
                message.damager = gameObject;
                message.hitPoint = attackRoot.TransformPoint(hits[i].point);
                message.hitNormal = attackRoot.TransformDirection(hits[i].normal);

                attackTargetEntity.ApplyDamage(message);

                lastAttackedTargets.Add(attackTargetEntity);
                print("���ݼ���");
                break;
            }
        }*/
        //StartCoroutine(Deading());
    }

    // �ֱ������� ������ ����� ��ġ�� ã�� ��θ� ����
    protected virtual IEnumerator UpdatePath()
    {
        // ����ִ� ���� ���� ����
        while (!dead)
        {

            agent.SetDestination(targetEntity.transform.position);

            if (state == State.Tracking)
            {


            }


            // 0.2 �� �ֱ�� ó�� �ݺ�
            yield return new WaitForSeconds(0.2f);
        }
    }

    // �������� �Ծ����� ������ ó��
    public override bool ApplyDamage(DamageMessage damageMessage)
    {
        if (!base.ApplyDamage(damageMessage)) return false;

        if (targetEntity == null)
        {
            targetEntity = damageMessage.damager.GetComponent<LivingEntity>();
        }

        DamagedFeedback(damageMessage);

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
    }

    public void DisableAttack()
    {
        //state = State.Tracking;
        //agent.isStopped = false;





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
}
