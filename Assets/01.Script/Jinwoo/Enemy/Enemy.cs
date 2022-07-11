using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
#if UNITY_EDITOR
using UnityEditor;
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
            Vector3.Distance(targetEntity.transform.position, transform.position) <= EnemyData.attackDistance)
        {
            BeginAttack();
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

        if (state == State.Attacking)
        {
            Attack();
        }
    }

    protected virtual void Attack()
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
                message.amount = damage;
                message.damager = gameObject;
                message.hitPoint = attackRoot.TransformPoint(hits[i].point);
                message.hitNormal = attackRoot.TransformDirection(hits[i].normal);

                attackTargetEntity.ApplyDamage(message);

                lastAttackedTargets.Add(attackTargetEntity);
                break;
            }
        }
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
                int randomDodge = Random.Range(0, 10);
                if (randomDodge > 5)
                {
                    var patrolPosition = Utility.GetRandomPointOnNavMesh(dodgeRoot.position, EnemyData.dodgeRadius, NavMesh.AllAreas);
                    agent.SetDestination(patrolPosition);
                }

                
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
    }

    public void DisableAttack()
    {
        state = State.Tracking;

        //agent.isStopped = false;
    }


    // ��� ó��
    public override void Die()
    {
        // LivingEntity�� Die()�� �����Ͽ� �⺻ ��� ó�� ����
        base.Die();

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

}
