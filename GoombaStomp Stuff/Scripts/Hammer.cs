using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hammer : MonoBehaviour
{
    PlayerController player;
    void Start()
    {
        player = GetComponentInParent<PlayerController>();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        player.OnCollisionEnter2D(collision);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        player.OnCollisionStay2D(collision);
    }
}
