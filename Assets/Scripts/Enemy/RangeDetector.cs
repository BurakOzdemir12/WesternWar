using UnityEngine;
using UnityEngine;

public class RangeDetector : MonoBehaviour
{
    [SerializeField] private float attackRange = 1.5f;
    [SerializeField] private float searchRange = 1.5f;
    [SerializeField] private LayerMask targetLayerMask;

    // Tek seferlik buffer (GC yok)
    private readonly Collider[] _buffer = new Collider[8];
    private readonly Collider[] _bufferRange = new Collider[8];
    private readonly Collider[] _search = new Collider[8];


    // Layer filtresiyle çevre kontrolü istersen:
    public bool IsAnyTargetInAttackRange(out GameObject first)
    {
        first = null;
        int hitCount = Physics.OverlapSphereNonAlloc(
            transform.position, attackRange, _buffer, targetLayerMask);

        if (hitCount > 0)
        {
            first = _buffer[0].gameObject;
            return true;
        }

        return false;
    }

    public bool IsAnyTargetInRange(out GameObject obj)
    {
        obj = null;
        int hitCount = Physics.OverlapSphereNonAlloc(
            transform.position, searchRange, _search, targetLayerMask);
        if (hitCount > 0)
        {
            obj = _search[0].gameObject;
            return true;
        }

        return false;
    }

    public int GetAllTargetsInAttackRangeNonAlloc(out Collider[] results)
    {
        int hitCount = Physics.OverlapSphereNonAlloc(
            transform.position, attackRange, _bufferRange, targetLayerMask);
        results = _bufferRange;
        return hitCount;
    }

    private void OnDrawGizmosSelected()
    {
        //Search Range
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, searchRange);

        //Attack Range
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}