using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class ZombieAI : MonoBehaviour
{
    [Header("AI Settings")]
    public float detectionRange = 15f; // Phạm vi phát hiện player
    public float attackRange = 2f; // Phạm vi tấn công
    public float walkSpeed = 2f;
    public float chaseSpeed = 3.5f;

    [Header("Attack Settings")]
    public int attackDamage = 10;
    public float attackRate = 1f; // Tấn công mỗi giây

    [Header("State")]
    public ZombieState currentState = ZombieState.Idle;

    private Transform player;
    private NavMeshAgent agent;
    private Animator animator;
    private float nextAttackTime = 0f;
    private float distanceToPlayer;

    public enum ZombieState
    {
        Idle,
        Patrol,
        Chase,
        Attack
    }

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        // Tìm player
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            player = playerObj.transform;

        agent.speed = walkSpeed;
    }

    void Update()
    {
        if (player == null)
            return;

        distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // State machine
        switch (currentState)
        {
            case ZombieState.Idle:
                IdleState();
                break;
            case ZombieState.Patrol:
                PatrolState();
                break;
            case ZombieState.Chase:
                ChaseState();
                break;
            case ZombieState.Attack:
                AttackState();
                break;
        }

        UpdateAnimations();
    }

    void IdleState()
    {
        // Nếu phát hiện player trong phạm vi, đuổi theo
        if (distanceToPlayer <= detectionRange)
        {
            currentState = ZombieState.Chase;
            agent.speed = chaseSpeed;
        }
    }

    void PatrolState()
    {
        // Có thể thêm logic tuần tra random sau
        // Hiện tại chuyển sang Idle
        currentState = ZombieState.Idle;
    }

    void ChaseState()
    {
        // Đuổi theo player
        agent.SetDestination(player.position);

        // Nếu đủ gần, chuyển sang tấn công
        if (distanceToPlayer <= attackRange)
        {
            currentState = ZombieState.Attack;
            agent.isStopped = true;
        }
        // Nếu player chạy xa, quay lại Idle
        else if (distanceToPlayer > detectionRange + 5f)
        {
            currentState = ZombieState.Idle;
            agent.isStopped = true;
            agent.speed = walkSpeed;
        }
    }

    void AttackState()
    {
        // Quay mặt về phía player
        Vector3 direction = (player.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);

        // Tấn công
        if (Time.time >= nextAttackTime)
        {
            Attack();
            nextAttackTime = Time.time + attackRate;
        }

        // Nếu player chạy xa, đuổi theo
        if (distanceToPlayer > attackRange + 0.5f)
        {
            currentState = ZombieState.Chase;
            agent.isStopped = false;
        }
    }

    void Attack()
    {
        // Play attack animation
        if (animator != null)
        {
            try
            {
                animator.SetTrigger("Attack");
            }
            catch
            {
                // Animator không có trigger Attack
            }
        }

        // Gây damage cho player
        PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
        if (playerHealth != null && distanceToPlayer <= attackRange)
        {
            playerHealth.TakeDamage(attackDamage);
        }
    }

    void UpdateAnimations()
    {
        if (animator == null)
            return;

        // Update animation parameters
        float speed = agent.velocity.magnitude;

        // Check nếu animator có parameter trước khi set
        // Tránh lỗi nếu zombie model không có animation
        try
        {
            animator.SetFloat("Speed", speed);
            animator.SetBool("IsAttacking", currentState == ZombieState.Attack);
        }
        catch
        {
            // Animator không có parameter, bỏ qua
        }
    }

    // Vẽ gizmos để debug
    void OnDrawGizmosSelected()
    {
        // Detection range
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        // Attack range
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
