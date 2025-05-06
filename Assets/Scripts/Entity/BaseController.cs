using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class BaseController : MonoBehaviour
{
    protected Rigidbody2D _rigidbody;

    // SerializedField를 통해 Inspector에 공개개
    [SerializeField] private SpriteRenderer chracterRenderer;
    [SerializeField] private Transform weaponPivot;

    // 이동 방향
    protected Vector2 movementDirection = Vector2.zero;
    public Vector2 MovementDirection {get {return movementDirection;}}

    // 바라보는 방향
    protected Vector2 lookDirection = Vector2.zero;
    public Vector2 LookDirection {get {return lookDirection;}}

    // 밀려나는 방향
    private Vector2 knockback = Vector2.zero;
    public float KnockbackDuration = 0.0f;

    protected AnimationHandler animationHandler;

    protected StatHandler statHandler;

    [SerializeField] public WeaponHandler WeaponPrefab;
    protected WeaponHandler weaponHandler;

    protected bool isAttacking;
    private float timeSinceLastAttack = float.MaxValue;

    protected virtual void Awake() {
        _rigidbody = GetComponent<Rigidbody2D>();
        animationHandler = GetComponent<AnimationHandler>();
        statHandler = GetComponent<StatHandler>();

        // 생성하거나 찾아오거나
        if(WeaponPrefab != null) {
            // Pivot에 Prefab을 복제해서 생성
            weaponHandler = Instantiate(WeaponPrefab, weaponPivot);
        }
        else {
            weaponHandler = GetComponentInChildren<WeaponHandler>();
        }
    }

    protected virtual void Start() {

    }

    protected virtual void Update() {
        HandleAction();
        Rotate(lookDirection);
        HandleAttackDelay();
    }

    protected virtual void FixedUpdate() {
        Movement(movementDirection);
        if(KnockbackDuration > 0.0f) {
            KnockbackDuration -= Time.fixedDeltaTime;
        }
    }

    protected virtual void HandleAction() {

    }

    private void Movement(Vector2 direction) {
        direction = direction * statHandler.Speed;
        if(KnockbackDuration > 0.0f) {
            direction *= 0.2f;
            direction += knockback;
        }

        // 이동에 대한 처리
        _rigidbody.velocity = direction;
        animationHandler.Move(direction);
    }

    private void Rotate(Vector2 direction) {
        // Atan2는 x, y 좌표를 연결한 직각삼각형의 세타 값을 구하는 연산 (파이 값으로 나옴)
        // 해당 파이 값을 degree로 변경하기 위한 Rad2Deg
        float rotZ = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg; 

        // 절대값이 90보다 크다면 왼쪽을 향하고 있는 것
        bool isLeft = Mathf.Abs(rotZ) > 90f;

        // 캐릭터 이미지 반전
        chracterRenderer.flipX = isLeft;

        // 소지 중인 무기도 반전
        if(weaponPivot != null) {
            weaponPivot.rotation = Quaternion.Euler(0f, 0f, rotZ);
        }

        weaponHandler?.Rotate(isLeft);
    }

    public void ApplyKnockback(Transform other, float power, float duration) {
        KnockbackDuration = duration;
        // Normalized로 단위 벡터로 만들기
        knockback = - (other.position - transform.position).normalized * power;
    }

    private void HandleAttackDelay() {
        if(weaponHandler == null) 
            return;
        
        if(timeSinceLastAttack <= weaponHandler.Delay) {
            timeSinceLastAttack += Time.deltaTime;
        }

        if (isAttacking && timeSinceLastAttack > weaponHandler.Delay) {
            timeSinceLastAttack = 0;
            Attack();
        }
    }

    protected virtual void Attack() {
        if(lookDirection != Vector2.zero) {
            weaponHandler?.Attack();
        }
    }

    public virtual void Death() {
        _rigidbody.velocity = Vector3.zero;

        // 하위에 있는 모든 Sprite 찾기
        foreach(SpriteRenderer renderer in transform.GetComponentsInChildren<SpriteRenderer>()) {
            UnityEngine.Color color = renderer.color;
            color.a = 0.3f;
            renderer.color = color;
        }

        // 모든 Component 끄기
        foreach(Behaviour component in transform.GetComponentsInChildren<Behaviour>()) {
            component.enabled = false;
        }

        Destroy(gameObject, 2f);
    }
}
