using UnityEngine;

public struct DamageInfo
{
    public float damageAmount;
    public GameObject attacker;     // atan: player/silah
    public Vector3 hitPoint;         // isabet noktası
    public Vector3 normal;        // yüzey normali

    public DamageInfo(float damageAmount, GameObject attacker, Vector3 hitPoint, Vector3 normal)
    {
        this.damageAmount = damageAmount;
        this.attacker = attacker;
        this.hitPoint = hitPoint;
        this.normal = normal;
    }
}

public interface IGetHit
{
    bool IsDead { get; }
    void GetHit(DamageInfo info);
}
