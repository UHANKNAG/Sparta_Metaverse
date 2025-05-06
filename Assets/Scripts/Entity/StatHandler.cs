using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatHandler : MonoBehaviour
{
    // Range(Min, Max)로 범위 지정, SerializeField로 인스펙터에 동기화
    [Range(1, 100)][SerializeField] private int health = 100;
    public int Health {
        get => health;
        // Clamp(값, 최소, 최대): 값이 최소보다 작다면 최소로, 최대보다 크다면 최대로 return
        set => health = Mathf.Clamp(value, 0, 100);
    }

    [Range(1f, 20f)][SerializeField] private float speed = 3;
    public float Speed {
        get => speed;
        set => speed = Mathf.Clamp(value, 0, 20);
    }
}
