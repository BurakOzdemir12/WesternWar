using UnityEngine;

public interface IPlayerGetHit
{
    void GetHit(float damage,
        Vector3 hitPoint,
        Vector3 hitDirection,
        GameObject attacker);
}
