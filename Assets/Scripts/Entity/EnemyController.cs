using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EnemyController : BaseController
{
    private EnemyManager enemyManager;
    private Transform target;

    [SerializeField] private float followRange = 15f;

    public void Init(EnemyManager enemyManager, Transform target) {
        this.enemyManager = enemyManager;
        this.target = target;
    }

    protected float DistanceToTarget() {
        return Vector3.Distance(transform.position, target.position);
    }

    protected Vector2 DirectionToTarget() {
        return (target.position - transform.position).normalized;
    }

    protected override void HandleAction()
    {
        base.HandleAction();

        if (weaponHandler == null || target == null) {
            if (!movementDirection.Equals(Vector2.zero)) movementDirection = Vector2.zero;
            return;
        }

        float distance = DistanceToTarget();
        Vector2 direction = DirectionToTarget();

        isAttacking = false;

        // 따라갈 거리면 따라가고 아니면 안 따라가게
        if (distance <= followRange) {
            lookDirection = direction;

            if (distance < weaponHandler.AttackRange) {
                // 공격 거리에 들어왔다면 공격
                int layerMaskTarget = weaponHandler.target;
                // RaycastHit2D 안 보이는 물리적 레이저를 이용해 충돌처리
                RaycastHit2D hit = Physics2D.Raycast(transform.position, 
                direction, weaponHandler.AttackRange * 1.5f,
                (1 << LayerMask.NameToLayer("Level")) | layerMaskTarget);

            // 레이저가 충돌을 했고, 내가 처리해야 하는 Layer가 맞다면(공격을 할 수 있는 layer가 맞다면)
            if (hit.collider != null && layerMaskTarget == (layerMaskTarget | (1 << hit.collider.gameObject.layer))) {
                isAttacking = true;
            }
                // 이동하지 않고 공격만
                movementDirection = Vector2.zero;
                return;
            }
            
            // 아니라면 이동만
            movementDirection = direction;
        }
    }

// 추후 수정 예정
    public override void Death()
    {
        base.Death();
        enemyManager.RemoveEnemyOnDeath(this);
    }

}
