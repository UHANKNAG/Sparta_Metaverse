using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DungeonDoor : MonoBehaviour
{
    public Animator animator;
    private int IsEnter = Animator.StringToHash("IsEnter");
    public bool isPlayerInPortal = false;


    void Start() {
        animator.GetComponentInChildren<Animator>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W)) {
            if (isPlayerInPortal) {
                SceneManager.LoadScene("DungeonScene");
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        // Player의 Layer: 6
        if(other.gameObject.layer == 6) {
            animator.SetTrigger(IsEnter);
            isPlayerInPortal = true;
        }    
    }

    private void OnTriggerExit2D(Collider2D other) {
        animator.GetComponent<Animator>().enabled = false;

        if(other.gameObject.layer == 6) {
            isPlayerInPortal = false;
        }  
    }
}
