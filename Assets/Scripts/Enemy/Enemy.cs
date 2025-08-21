using System;
using System.Collections.Generic;
using Unity.Behavior;
using UnityEngine;
using UnityEngine.Serialization;

public class Enemy : MonoBehaviour
{
    [SerializeField] private EnemySo enemySo;
    [SerializeField] private RangeDetector rangeDetector;
    [SerializeField] private BehaviorGraphAgent behaviorGraphAgent;
    [SerializeField] private string speedVarName = "speed";
    [SerializeField] private string attackCooldownVarName = "attackCooldown";
    private bool _isDead = false;

    [Header("projectile")] [SerializeField]
    private Transform handReleasePosition;

    private IGetHit _getHitImplementation;

    private void Awake()
    {
        if (!behaviorGraphAgent) behaviorGraphAgent = GetComponent<BehaviorGraphAgent>();
    }

    private void OnEnable()
    {
        EnemyHealth.OnEnemyDeath += HandleEnemyDeath;
        behaviorGraphAgent.BlackboardReference.SetVariableValue(speedVarName, enemySo.speed);
        behaviorGraphAgent.BlackboardReference.SetVariableValue(attackCooldownVarName, enemySo.attackCooldown);
    }

    private void HandleEnemyDeath(EnemyHealth obj)
    {
        _isDead = true;
    }

    private void Start()
    {
    }

    public void MeleeAttack()
    {
        if (enemySo.attackProfiles.category == AttackCategory.Melee && !_isDead)
        {
            behaviorGraphAgent.BlackboardReference.GetVariableValue("IsInAttackRange", out bool isInAttackRange);
            if (!isInAttackRange)
            {
                Debug.Log("Target is not in attack range!");
                return;
            }

            behaviorGraphAgent.BlackboardReference.GetVariableValue("Target", out GameObject target);
            if (target != null)
            {
                var info = new DamageInfo(
                    enemySo.attackProfiles.damage,
                    gameObject,
                    transform.position,
                    -transform.forward
                );

                target.TryGetComponent(out IGetHit damageable);
                damageable.GetHit(info);
                // Debug.Log("Melee attack on player! from " + gameObject.name + " to " + target.name);
            }
            else
            {
                Debug.LogWarning("Target not found in Blackboard!");
            }
        }
    }

    public void RangeAttack()
    {
        if (enemySo.attackProfiles.category == AttackCategory.Ranged && !_isDead)
        {
            // Handle ranged attack logic here
            LaunchProjectile();
            // Debug.Log("Ranged attack on player!");
        }
        else
        {
            Debug.LogWarning("Unknown attack category!");
        }
    }

    private void LaunchProjectile()
    {
        var go = Instantiate(enemySo.attackProfiles.projectilePrefab, handReleasePosition.position,
            Quaternion.identity);
        Vector3 euler = go.transform.eulerAngles;
        euler.y = 0;
        go.transform.eulerAngles = euler;
        //---- SOUND EFFECT ----//
        var clip = enemySo.attackProfiles.hitSfx;
        var volume = enemySo.attackProfiles.volume;
        AudioSource.PlayClipAtPoint(
            clip,
            transform.position,
            volume
        );

        // go.transform.SetParent(projectileParent);
        behaviorGraphAgent.BlackboardReference.GetVariableValue("Target", out GameObject target);

        Vector3 targetPos = target.transform.position + Vector3.up * 2.2f;
        var damage = enemySo.attackProfiles.damage;
        if (go.TryGetComponent(out MageProjectile proj))
            proj.Launch(targetPos, damage, handReleasePosition);
    }

    public void AreaOfEffectAttack()
    {
        if (enemySo.attackProfiles.category == AttackCategory.AreaOfEffect && !_isDead)
        {
            behaviorGraphAgent.BlackboardReference.GetVariableValue("IsInAttackRange", out bool isInAttackRange);
            if (!isInAttackRange)
            {
                Debug.Log("Target is not in attack range!");
                return;
            }

            rangeDetector.GetAllTargetsInAttackRangeNonAlloc(out var targets);
            if (targets != null)
            {
                var info = new DamageInfo(
                    enemySo.attackProfiles.damage,
                    gameObject,
                    transform.position,
                    -transform.forward
                );

                foreach (var t in targets)
                {
                    if (t == null) continue;
                    var target = t.gameObject;
                    if (target.TryGetComponent(out IGetHit hit))
                        hit.GetHit(info);
                }
                // Debug.Log("Melee attack on player! from " + gameObject.name + " to " + target.name);
            }
            else
            {
                Debug.LogWarning("Target not found in Blackboard!");
            }
        }
    }

    private void OnDisable()
    {
        EnemyHealth.OnEnemyDeath -= HandleEnemyDeath;
    }
}