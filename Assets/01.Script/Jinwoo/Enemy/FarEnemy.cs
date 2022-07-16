using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FarEnemy : LivingEntity, IEnemy
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

    [SerializeField] private BulletDataSO _bulletData;
    [SerializeField] private Transform _firePos;
    [SerializeField] private GameObject _muzzle;

    Transform player;
    [SerializeField] private Transform eye;
    [SerializeField] private LayerMask playerLayer;

    float freezeTimer;
    Vector3 knockbackForce;

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


        EnemyData.attackDistance = EnemyData.stoppingDistance;

        //���߿� �������
        Setup();
    }
    // �� AI�� �ʱ� ������ �����ϴ� �¾� �޼���
    protected virtual void Setup()
    {
        // ü�� ����
        this.health = EnemyData.maxHealth + WaveManager.instance.GetFloor();

        // ����޽� ������Ʈ�� �̵� �ӵ� ����  
        this.runSpeed = EnemyData.maxSpeed;

        this.damage = EnemyData.damage;

        state = State.Tracking;

        agent.speed = runSpeed;
    }

    protected virtual void Start()
    {
        // ���� ������Ʈ Ȱ��ȭ�� ���ÿ� AI�� ���� ��ƾ ����
        player = Define.Instance.controller.transform;
        StartCoroutine(UpdatePath());

        EnemySubject.instance.RegisterObserver(this);
    }
    private void Update()
    {
        if (dead) return;

        if (state == State.Tracking &&
            Vector3.Distance(targetEntity.transform.position, transform.position) <= EnemyData.attackDistance)
        {
            RaycastHit hit;
            Debug.DrawRay(eye.position, (player.position - eye.position).normalized * 100, Color.blue, 1);
            if(Physics.Raycast(eye.position, (player.position - eye.position).normalized, out hit, 100, playerLayer))
            {
                if (hit.transform.CompareTag("Player"))
                {
                    print(hit.transform.name);
                    BeginAttack();
                }
            }
            else
            {
                agent.isStopped = false;
                agent.SetDestination(player.position);
            }
            print("BBB");
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

    }

    protected virtual void Attack()
    {
        print("���Ÿ� ����");

        Transform target = targetEntity.transform;

        Vector3 aimDirection = target.position - _firePos.position;
        float desireAngle = Mathf.Atan2(aimDirection.x, aimDirection.z) * Mathf.Rad2Deg;

        Quaternion rot = Quaternion.AngleAxis(desireAngle, Vector3.up);

        SpawnBullet(_firePos.position, rot, true, damage);
        StartCoroutine(SpawnMuzzle());

    }
    private void SpawnBullet(Vector3 pos, Quaternion rot, bool isEnemyBullet, float damage)
    {
        EnemyBullet b = PoolManager.instance.Pop(PoolType.EnemyBullet).GetComponent<EnemyBullet>();

        b.SetPositionAndRotation(pos, rot);
        b.IsEnemy = isEnemyBullet;
        b.BulletData = _bulletData;
        b.damageFactor = damage;
    }
    IEnumerator SpawnMuzzle()
    {
        _muzzle.SetActive(true);
        yield return new WaitForSeconds(0.3f);
        _muzzle.SetActive(false);
    }
    // �ֱ������� ������ ����� ��ġ�� ã�� ��θ� ����
    protected virtual IEnumerator UpdatePath()
    {
        // ����ִ� ���� ���� ����
        while (!dead)
        {
            if(state == State.Tracking)
            {
                agent.SetDestination(targetEntity.transform.position);

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
        agent.isStopped = true;
        animator.SetTrigger("Attack");
    }

    public void EnableAttack()
    {
        state = State.Attacking;

        lastAttackedTargets.Clear();
    }

    public void DisableAttack()
    {
        Attack();
        state = State.Tracking;
        agent.isStopped = false;
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
