using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Hole : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other) {
        SceneManager.LoadScene("MiniGameScene");
    }
}
