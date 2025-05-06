using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerController : BaseController
{
    private Camera camera;
    private GameManager gameManager;

    public void Init(GameManager gameManager) {
        this.gameManager = gameManager;
        camera = Camera.main;
    }

    protected override void HandleAction() {
 
    }

    public override void Death()
    {
        base.Death();
        gameManager.GameOver();
    }

    void OnMove(InputValue inputValue) {
        // float horizontal = Input.GetAxisRaw("Horizontal");
        // float vertial = Input.GetAxisRaw("Vertical");
        //movementDirection = new Vector2(horizontal, vertial).normalized;

        movementDirection = inputValue.Get<Vector2>();
        movementDirection = movementDirection.normalized;
    }

    void OnLook(InputValue inputValue) {
        //Vector2 mousePosition = Input.mousePosition;
        Vector2 mousePosition = inputValue.Get<Vector2>();

        // mouse 좌표는 해상도 좌표고
        // 그 해상도 좌표를 우리가 원하는 world 좌표로 바꿔 주는 작업
        Vector2 worldPos = camera.ScreenToWorldPoint(mousePosition);
        lookDirection = (worldPos - (Vector2)transform.position);

        if(lookDirection.magnitude < 0.9f) {
            lookDirection = Vector2.zero;
        }
        else {
            lookDirection = lookDirection.normalized;
        }
    }

    void OnFire(InputValue inputValue) {
        // UI에 마우스 올라가 있을 때는 미사일 발사하지 않도록
        if(EventSystem.current.IsPointerOverGameObject())
            return;

        // isAttacting = Input.GetMouseButton(0);
        isAttacking = inputValue.isPressed;
        
    }
}
