using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public Transform target;
    float offsetX;

    // Start is called before the first frame update
    void Start()
    {
        if (target == null)
            return;

        offsetX = transform.position.x - target.position.x;
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null)
            return;

        // position의 x 값 자체가 바로 변동 가능한 게 아니라서, 변수에 담고 변수를 변경해서 다시 담는 방법
        UnityEngine.Vector3 pos = transform.position;
        pos.x = target.position.x + offsetX;
        transform.position = pos;
    }
}
