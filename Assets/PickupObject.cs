using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupObject : MonoBehaviour
{

    public int ScoreCount;

    private GameManager _GameManager;

    private void Start()
    {
        _GameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        Destroy(gameObject);
        _GameManager.AddScore(ScoreCount);
    }
}
