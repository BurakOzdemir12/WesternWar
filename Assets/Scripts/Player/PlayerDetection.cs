using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerDetection : MonoBehaviour
{
    [SerializeField] private LayerMask doorLayerMask;
    [SerializeField] private float detectionRadius = 5f;
    private CrowdSystem _crowdSystem;

    private void Awake()
    {
        _crowdSystem = GetComponentInParent<CrowdSystem>();
    }

    void Start()
    {
    }

    void Update()
    {
        // DetectDoors();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_crowdSystem == null) return;

        // Kapı
        if (other.TryGetComponent(out Doors door))
        {
            var amountOfBonus = door.GetBonusAmount(transform.position.x);
            BonusType bonusType = door.GetBonusType(transform.position.x);
            door.DisableDoor();
            _crowdSystem.ApplyBonus(amountOfBonus, bonusType);
        }
        // Finish
        // else if (other.CompareTag("Finish"))
        // {
        //     var finish = other.GetComponentInParent<FinishChunk>();
        //     if (finish != null && finish.TryConsume())
        //     {
        //         // Oyunu bitir / level artır gibi işlemleri burada ya da GameManager’da yap
        //         PlayerPrefs.SetInt("level", PlayerPrefs.GetInt("level") + 1);
        //         GameManager.instance.SetGameState(GameManager.GameState.GameOver);
        //     }
        // }
    }
    
    //Old way to detect doors its expensive for performance CPU
    
    // private void DetectDoors()
    // {
    //     Collider[] hitColliders = Physics.OverlapSphere(transform.position, detectionRadius, doorLayerMask);
    //
    //     var colsLength = hitColliders.Length;
    //     for (int i = 0; i < colsLength; i++)
    //     {
    //         if (hitColliders[i].TryGetComponent(out Doors doors))
    //         {
    //             var amountOfBonus = doors.GetBonusAmount(transform.position.x);
    //             BonusType bonusType = doors.GetBonusType(transform.position.x);
    //             Debug.Log("Player is in a door");
    //
    //             doors.DisableDoor();
    //             _crowdSystem.ApplyBonus(amountOfBonus, bonusType);
    //         }
    //
    //         else if (hitColliders[i].CompareTag("Finish"))
    //         {
    //             var parentFinishLine = hitColliders[i].GetComponentInParent<FinishChunk>();
    //             parentFinishLine.DisableFinishCollider();
    //
    //             //TODO Call Player Level Add and coin gainin
    //             PlayerPrefs.SetInt("level", PlayerPrefs.GetInt("level") + 1);
    //
    //             //TODO Call Gamemanager to end the game
    //             Debug.Log("Player is in a finish");
    //
    //             SceneManager.LoadScene(0);
    //         }
    //     }
    // }

    // private void OnDrawGizmos()
    // {
    //     Gizmos.DrawSphere(transform.position, detectionRadius);
    // }
}