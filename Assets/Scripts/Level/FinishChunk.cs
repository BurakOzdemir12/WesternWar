using UnityEngine;

public class FinishChunk : MonoBehaviour
{
    [SerializeField] private Collider finishCollider;

    void Start()
    {
    }


    void Update()
    {
    }

    public void DisableFinishCollider()
    {
        finishCollider.enabled = false;
    }
}