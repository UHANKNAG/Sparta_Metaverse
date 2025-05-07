using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Timeline;

public class Player : MonoBehaviour
{
    Animator animator;
    Rigidbody2D _rigidbody;

    public float flapForce = 6f;    // 점프 힘
    public float forwardSpeed = 3f; // 정면 이동 힘
    public bool isDead = false;     // 생사 확인
    float deathCooldown = 0f;      // 일정 시간 이후 죽게끔

    bool isFlap = false;            // 점프 했는지 안 했는지

    public bool godMode = false;    // 게임 테스트용

    GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.Instance;

        // Animator는 Player가 아닌 Player의 자식 Model에 달려있기 때문에 InChildren
        animator = GetComponentInChildren<Animator>();
        _rigidbody = GetComponent<Rigidbody2D>();
         
        if(animator == null) 
            Debug.LogError("Not Founded Animator");

        if (_rigidbody == null) 
            Debug.LogError("Not Founded Rigidbody");
    }

    // Update is called once per frame
    void Update()
    {
        if(isDead) {
            if(deathCooldown <= 0) {
                if(Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)) {
                    // gameManager.RestartGame();
                }
            }
            else {
                deathCooldown -= Time.deltaTime;
            }
        }
        else {
            // 0:L 1:R 2:wheel 3:back 4:front 
            // 0은 스마트폰의 touch도 포함함
            if(Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)) {
                isFlap = true;
            }
        }
    }

    private void FixedUpdate() {
        if (isDead) return;

        Vector3 velocity = _rigidbody.velocity;
        velocity.x = forwardSpeed;

        if (isFlap) {
            velocity.y += flapForce;
            isFlap = false;
        }

        // Vector3 velocity는 구조체 형식으로 불러온 거라 값형식임
        // 그래서 rigidbody velocity에 다시 넣어 주는 작업 필요
        _rigidbody.velocity = velocity;

        // Clamp는 값을 제한한다.
        // 특정한 값을 Min과 Max로 구분한다.
        // Plane이 위로 올라가고 있다면 위 대각선을 향하도록
        // 아래로 내려가고 있다면 아래 대각선을 향하도록 각도 설정
        float angle = Mathf.Clamp((_rigidbody.velocity.y * 10f), -90, 90);
        // Quaternion 사원수 값, Euler는 360도 기준의 각도
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    private void OnCollisionEnter2D(Collision2D collision) 
    {
        if (godMode) return;

        if (isDead) return;

        isDead = true;
        deathCooldown = 1f;

        //animator.SetInteger("IsDie", 1);
        gameManager.MiniGameOver();
    }
}
