using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public float highPosY = 1f;
    public float lowPosY = -1f;

    public float holeSizeMin = 1f;
    public float holeSizeMax = 3f;

    public Transform topObject;
    public Transform bottomObject;

    public float widthPadding = 4f;

    GameManager gameManager;

    private void Start() {
        gameManager = GameManager.Instance;
    }

    public UnityEngine.Vector3 SetRandomPlace(UnityEngine.Vector3 lastPosition, int obstaclCount) {
        float holeSize = Random.Range(holeSizeMin, holeSizeMax);
        float halfHoleSize = holeSize / 2;

        topObject.localPosition = new UnityEngine.Vector3(0, halfHoleSize);
        bottomObject.localPosition = new UnityEngine.Vector3(0, -halfHoleSize);

        UnityEngine.Vector3 placePosition = lastPosition + new UnityEngine.Vector3(widthPadding, 0);
        placePosition.y = Random.Range(lowPosY, highPosY);

        transform.position = placePosition;

        return placePosition;

        // position은 월드 좌표로 (0, 0, 0)을 기준으로 함
        // localPosition은 로컬 좌표로 부모 오브젝트를 기준으로 함
    }

    private void OnTriggerExit2D(Collider2D collision) {
        Player player = collision.GetComponent<Player>();
        if (player != null) {
            gameManager.AddScore(1);
        }
    }
}
