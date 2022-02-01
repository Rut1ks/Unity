using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour, IAttackable
{
    public float ViewingDistance = 10f;

    public float AttackableDistance = 2f;

    public GameObject AttackPoint;

    public float AttackRange = 0.7f;

    public LayerMask PlayerLayer;

    public int CountDownSeconds = 1;

    public ParticleSystem DamageParticle;

    public int Health = 30;

    public int ScoreCount;

    private bool EnableAttack = true;
    private Transform Target;
    private NavMeshAgent Agent;
    private Animator Animator;
    private GameManager GameManager;
    private float DistanceToPlayer;

    private void Start()
    {
        Target = GameManager.ManagerInstance.Player.transform;
        Agent = GetComponent<NavMeshAgent>();
        Animator = GetComponent<Animator>();
        GameManager = GameManager.ManagerInstance;
    }

    private void FixedUpdate()
    {
        SetAnimation();

        DistanceToPlayer = Vector3.Distance(Target.position, transform.position);

        if (DistanceToPlayer <= ViewingDistance)
        {
            Agent.SetDestination(Target.position);
            transform.LookAt(Target.position);

            if (DistanceToPlayer <= AttackableDistance && EnableAttack) StartCoroutine(AttackCountDown());
        }
            if (Health <= 0) Death();
        }

    public void DealDamage(int Count)
    {
        Health -= Count;
        DamageParticle.Play();
    }

    private void Attack()
    {
        Collider[] HitedColliders = Physics.OverlapSphere(AttackPoint.transform.position, AttackRange, PlayerLayer);

        EnableAttack = true;

        foreach (Collider HitedCollider in HitedColliders)
        {
            GameManager.DamagePlayer(5);
            Debug.Log(GameManager.Health);
        }
    }

    private IEnumerator AttackCountDown()
    {
        EnableAttack = false;
        int Counter = CountDownSeconds;
        while (Counter > 0)
        {
            yield return new WaitForSeconds(1);
            Counter--;
        }

        Attack();
    }

    private void Death()
    {
        DamageParticle.transform.parent = null;
        DamageParticle.Play();
        GameManager.AddScore(ScoreCount);
        GameObject.Destroy(gameObject);
    }

    private void SetAnimation()
    {
        if (DistanceToPlayer <= AttackableDistance && EnableAttack) Animator.SetInteger("Animation", 2);
        else
        {
            if (DistanceToPlayer <= ViewingDistance) Animator.SetInteger("Animation", 1);
            else Animator.SetInteger("Animation", 0);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(AttackPoint.transform.position, AttackRange);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, ViewingDistance);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, AttackableDistance);
    }
}
