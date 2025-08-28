using UnityEngine;

public class EnterenceBoardEffect : MonoBehaviour
{
    [SerializeField] private Transform enterenceBoard;
    [SerializeField] private float speed;
    [SerializeField] private float maxXRot;
    private float _currentXRot;

    void Start()
    {
    }

    void Update()
    {
        if (!enterenceBoard) return;
        var xRot = Mathf.Lerp(-maxXRot, maxXRot, Mathf.PingPong(Time.time * speed, 1f));
        enterenceBoard.rotation = Quaternion.Euler(xRot, 0f, -180f);
    }
}