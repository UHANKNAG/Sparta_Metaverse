using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BgLooper : MonoBehaviour
{
    public int numBgCount = 5;
    public int obstacleCount = 0;
    public Vector3 obstacleLastPosition = Vector3.zero; // (0, 0, 0)

    // Start is called before the first frame update
    void Start()
    {
        // Obstacle이라는 Object를 모두 찾아 배열에 넣음
        Obstacle[] obstacles = GameObject.FindObjectsOfType<Obstacle>();
        obstacleLastPosition = obstacles[0].transform.position;
        obstacleCount = obstacles.Length;

        for (int i = 0; i < obstacleCount; i++) {
            obstacleLastPosition = obstacles[i].SetRandomPlace(obstacleLastPosition, obstacleCount);
        }
    }

    // OnTriggerEnter: 충돌에 대한 통보 (충돌체에 대한 정보만 줄 수 있음음)
    private void OnTriggerEnter2D(Collider2D collision) {
        Debug.Log("Triggerd: " + collision.name);

        if (collision.CompareTag("Background")) {
            // Collider2D는 모든 Collider의 부모 클래스일 뿐이라 BoxCollider를 가져올 수 없음
            // Collider2D에는 .size 프로퍼티가 없어서 사이즈를 가져올 수 없다.
            // 그래서 .size 프로퍼티가 있는 BoxCollider로 명시적 형변환
            float widthObBgObject = ((BoxCollider2D)collision).size.x;
            Vector3 pos = collision.transform.position;

            pos.x += widthObBgObject * numBgCount;
            collision.transform.position = pos;
            
            return;
        }

        Obstacle obstacle = collision.GetComponent<Obstacle>();
        if (obstacle) {
            obstacleLastPosition = obstacle.SetRandomPlace(obstacleLastPosition ,obstacleCount);
        }
    }
}
