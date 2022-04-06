using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupObject : MonoBehaviour
{

    public int ScoreCount;
    public int HealthCount;
    public int StrenghtCount;

    private GameManager _GameManager;
    private PlayerController _PlayerController;

    private void Start()
    {
        _GameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        _PlayerController = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        Destroy(gameObject);
        _PlayerController.AddStr(StrenghtCount);
        _GameManager.AddScore(ScoreCount);
        _GameManager.AddHealth(HealthCount);
    }
}
