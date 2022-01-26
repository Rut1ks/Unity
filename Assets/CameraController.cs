using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject Player;

    float Smoothing = 10f;

    Vector3 Offset;

    
    void Start()
    {
        Offset = gameObject.transform.position - Player.transform.position;
    }

    
    void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, Player.transform.position + Offset, Smoothing);
    }
}
