using System;
using UnityEngine;

public class MageProjectile : MonoBehaviour
{
    [SerializeField] private GameObject explosionParticle;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float speed = 12f;
    [SerializeField] private float lifeTime = 5f;
    [SerializeField] private float yawOffset = 180f;
    private Vector3 _targetPos;
    private bool _launched = false;
    private float _damageAmount;
    private readonly string _playerTag = "Player";
    private Transform _handReleasePosition;
    [SerializeField] private float turnSpeedDeg;


    public void Launch(Vector3 targetPosition, float damage, Transform handReleasePosition)
    {
        _damageAmount = damage;
        _targetPos = targetPosition;
        _launched = true;
        _handReleasePosition = handReleasePosition;
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        if (!_launched) return;

        // transform.LookAt(_targetPos);

        //simple move towards
        // transform.position = Vector3.MoveTowards(
        //     transform.position,
        //     _targetPos,
        //     speed * Time.deltaTime
        // );
        //
        // Vector3 dir = (_targetPos - transform.position).normalized;
        // if (dir != Vector3.zero)
        //     transform.forward = dir;
    }

    private void FixedUpdate()
    {
        if (!_launched) return;
        // transform.LookAt(_targetPos);
        rb.linearVelocity = _handReleasePosition.forward * speed;

        // Vector3 dir = (_targetPos - transform.position).normalized;
        // if (dir != Vector3.zero)
        //     transform.rotation = Quaternion.Slerp(transform.rotation,
        //         Quaternion.LookRotation(-dir), 5f * Time.fixedDeltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(_playerTag))
        {
            var info = new DamageInfo(
                _damageAmount,
                gameObject,
                transform.position,
                -transform.forward
            );
            Instantiate(explosionParticle, info.hitPoint, Quaternion.identity);
            //TODO: Add explosion sound effect here

            Debug.Log(info.hitPoint);
            if (other.TryGetComponent(out IGetHit damageable))
            {
                damageable.GetHit(info);

                Debug.Log("Mage projectile hit player: " + other.name);
            }
            else
            {
                Debug.LogWarning("Mage projectile hit something that is not damageable: " + other.name);
            }

            Destroy(this.gameObject);
        }
    }
}