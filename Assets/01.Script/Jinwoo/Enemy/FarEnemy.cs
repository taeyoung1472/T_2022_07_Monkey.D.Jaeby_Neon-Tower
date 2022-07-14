using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FarEnemy : LivingEntity
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
        player = Define.Instance.controller.transform;
        StartCoroutine(UpdatePath());
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
        StartCoroutine(ChangeMaterial());
        //EffectManager.Instance.PlayHitEffect(damageMessage.hitPoint, damageMessage.hitNormal, transform, EffectManager.EffectType.Flesh);
        audioPlayer.PlayOneShot(EnemyData.hitClip);

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


    // ��� ó��
    public override void Die()
    {
        // LivingEntity�� Die()�� �����Ͽ� �⺻ ��� ó�� ����
        base.Die();
        StopCoroutine("ChangeMaterial");

        state = State.Tracking;

        // �ٸ� AI���� �������� �ʵ��� �ڽ��� ��� �ݶ��̴����� ��Ȱ��ȭ
        GetComponent<Collider>().enabled = false;

        // AI ������ �����ϰ� ����޽� ������Ʈ�� ��Ȱ��ȭ
        agent.enabled = false;

        // ��� �ִϸ��̼� ���
        animator.applyRootMotion = true;
        animator.SetTrigger("Die");

        // ��� ȿ���� ���
        if (EnemyData.deathClip != null) audioPlayer.PlayOneShot(EnemyData.deathClip);
    }
    IEnumerator ChangeMaterial()
    {
        meshRenderer.material = damageMat;
        yield return new WaitForSeconds(.25f);
        meshRenderer.material = orignMat;
    }
}
