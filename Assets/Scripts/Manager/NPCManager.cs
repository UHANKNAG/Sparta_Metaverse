using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Toolbars;
using UnityEngine;

public class NPCManager : MonoBehaviour
{
    public Transform player;
    private SpriteRenderer spriteRenderer;
    private bool isContact = false;

    void Start() {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (player == null)
            return;
        
        spriteRenderer.flipX = player.position.x > transform.position.x;


        if (isContact) {
            if (Input.GetKeyDown(KeyCode.E)) {
                // InitNPCInfo();
            }
        }
    }

    // private void InitNPCInfo() {
    //     var curNPC = 
    // }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.layer == 6) {
            isContact = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.gameObject.layer == 6) {
            isContact = false;
        }
    }
}

