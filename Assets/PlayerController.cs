using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    Rigidbody _Rigidbody;

    float MovementSpeed = 3.0f;

    float JumpForce = 7.5f;

   public float DestinationToGround = 0.1f;

    bool IsGrounded = true;

    private AnimationManager _AnimationManager;


    public int Strength = 10;

    public GameObject AttackPoint;

    public float AttackRange = 0.7f;

    public LayerMask AttackableLayer;

    public int AttackCountdownSeconds = 1;

    private bool EnableAttack = true;    

    private void SetRotationOnMove()
    {
        Quaternion TargetQuaternion = Quaternion.LookRotation(new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")));
        transform.rotation = Quaternion.Slerp(transform.rotation, TargetQuaternion, MovementSpeed * Time.deltaTime);
    }

    void Start()
    {
        _Rigidbody = GetComponent<Rigidbody>();
        _AnimationManager = GetComponent<AnimationManager>();
    }

    private void FixedUpdate()
    {
        GrounCheck();
        if (Input.GetKey(KeyCode.Space) && IsGrounded) Jump();
        if (Input.GetKey(KeyCode.Mouse0) && EnableAttack) Attack();
        _Rigidbody.MovePosition(CalculateMovement());
        if (!IsMoving()) SetRotation();
        else SetRotationOnMove();
        SetPlayerAnimation();
    }



    Vector3 CalculateMovement()
    {
        float HorizontalDirection = Input.GetAxis("Horizontal");
        float VerticalDirection = Input.GetAxis("Vertical");

        //Debug.Log(HorizontalDirection + " " + VerticalDirection);

        return _Rigidbody.transform.position + new Vector3(HorizontalDirection, 0, VerticalDirection) * Time.fixedDeltaTime * MovementSpeed;
    }

    private void Jump()
    {
        _Rigidbody.AddForce(Vector3.up * JumpForce, ForceMode.Impulse);
    }

    private void GrounCheck()
    {
        IsGrounded = Physics.Raycast(transform.position, Vector3.down, DestinationToGround);
    }

    private void SetRotation()
    {
        Plane PlayerPlane = new Plane(Vector3.up, transform.position);
        Ray Ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        float hitdist = 0;
        if(PlayerPlane.Raycast(Ray,out hitdist))
        {
            Vector3 TargetPoint = Ray.GetPoint(hitdist);
            Quaternion TargetRotation = Quaternion.LookRotation(TargetPoint - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, TargetRotation, MovementSpeed * Time.deltaTime);
        }
    }

    private bool IsMoving()
    {
        return new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")) != Vector3.zero;
    }

    private void SetPlayerAnimation()
    {
        if (IsGrounded)
        {
            if (Input.GetKey(KeyCode.Mouse0)) _AnimationManager.SetAnimationAttack();
            else
            {
                if (IsMoving()) _AnimationManager.SetAnimationRun();
                else _AnimationManager.SetAnimationIdle();
            }
        }
        else _AnimationManager.SetAnimationJump();
    }


    private void Attack() 
    {
        Collider[] HitedColliders = Physics.OverlapSphere(AttackPoint.transform.position, AttackRange, AttackableLayer);
        foreach(Collider HitedCollider in HitedColliders)
        {
            IAttackable Attackable = HitedCollider.gameObject.GetComponent<IAttackable>();
            Attackable.DealDamage(Strength);
            Debug.Log(HitedCollider.name + " нанесено " + Strength + " урона");
        }
        EnableAttack = false;

        StartCoroutine(AttackCountdown());
    }

    private IEnumerator AttackCountdown()
    {
        int Counter = AttackCountdownSeconds;
        while (Counter > 0)
        {
            yield return new WaitForSeconds(1);
            Counter--;
        }

        EnableAttack = true;
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + (Vector3.down * DestinationToGround));
        Gizmos.DrawSphere(AttackPoint.transform.position, AttackRange);
    }
    public void AddStr(int Count)
    {
        Strength += Count;
    }
}
