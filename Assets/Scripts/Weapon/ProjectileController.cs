using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    // 실제로 날아가는 투사체 컨트롤

    [SerializeField] private LayerMask levelConllisionLayer;

    private RangeWeaponHandler rangeWeaponHandler;

    private float currentDuration;
    private Vector2 direction;
    private bool isReady;
    private Transform pivot;

    private Rigidbody2D _rigidbody;
    private SpriteRenderer spriteRenderer;

    public bool fxOnDestroy = true;

    ProjectileManager projectileManager;

    private void Awake() {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _rigidbody = GetComponent<Rigidbody2D>();
        pivot = transform.GetChild(0);
    }

    private void Update() {
        if (!isReady) return;

        currentDuration += Time.deltaTime;

        if (currentDuration > rangeWeaponHandler.Duration) {
            DestroyProjectile(transform.position, false);
        }

        _rigidbody.velocity = direction * rangeWeaponHandler.Speed;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        // layer는 2진 값
        // layer에 shift 연산을 해서 비교해서 충돌 가능한지 아닌지 확인
        if (levelConllisionLayer.value == (levelConllisionLayer.value | (1 << collision.gameObject.layer))) {
            DestroyProjectile(collision.ClosestPoint(transform.position) - direction * 0.2f, fxOnDestroy);
        }
        else if (rangeWeaponHandler.target.value == (rangeWeaponHandler.target.value | (1 << collision.gameObject.layer))) {
            ResourceController resourceController = collision.GetComponent<ResourceController>();

            // 데미지와 넉백 처리
            if (resourceController != null) {
                resourceController.Changehealth(-rangeWeaponHandler.Power);

                if (rangeWeaponHandler.IsOnKnockBack) {
                    BaseController controller = collision.GetComponent<BaseController>();
                    
                    if(controller != null) {
                        controller.ApplyKnockback(transform, rangeWeaponHandler.KnockbackPower, rangeWeaponHandler.KnockbackTime);
                    }
                }
            }
            
            
            DestroyProjectile(collision.ClosestPoint(transform.position), fxOnDestroy);
        }
    }

    public void Init(Vector2 direction, RangeWeaponHandler weaponHandler, ProjectileManager projectileManager) {
        this.projectileManager = projectileManager;

        rangeWeaponHandler = weaponHandler;

        this.direction = direction;
        currentDuration = 0;
        transform.localScale = Vector3.one * weaponHandler.BulletSize;
        spriteRenderer.color = weaponHandler.ProjectileColor;

        // Vector의 right랑은 다른 기능
        // 물체 자체의 '오른쪽'을 해당 방향을 보도록 회전해라
        transform.right = this.direction;

        if (direction.x < 0) {
            pivot.localRotation = Quaternion.Euler(180, 0, 0);
        }
        else {
            pivot.localRotation = Quaternion.Euler(0, 0, 0);
        }

        isReady = true;
    }

    private void DestroyProjectile(Vector3 position, bool createFx) {
        if(createFx) {
            projectileManager.CreateImpactParticleAtPositon(position, rangeWeaponHandler);
        }
        Destroy(this.gameObject);
    }
}
