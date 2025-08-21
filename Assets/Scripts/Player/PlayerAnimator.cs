using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;

    private List<Animator> _animators = new List<Animator>();
    private List<Animator> _buffer = new List<Animator>();

    [SerializeField] private Transform runnerParent;
    private Vector2 _currentVelocityX;
    private int _animIDRunning;

    private int _isRunningHash;
    private int _isStrafeHash;
    private int _velocityXHash;
    private int _isDead;
    private int Dead;

    private bool _lastRun;
    private float _lastStrafe;

    private void Awake()
    {
        Dead = Animator.StringToHash("Dead");
        _isDead = Animator.StringToHash("isDead");
        _isRunningHash = Animator.StringToHash("isRunning");
        _isStrafeHash = Animator.StringToHash("isStrafe");
        _velocityXHash = Animator.StringToHash("velocityX");
        _animIDRunning = _isRunningHash;
    }

    private void OnEnable()
    {
        CrowdSystem.OnRunnersChanged += RefreshAnimators;
    }

    private void OnDisable()
    {
        CrowdSystem.OnRunnersChanged -= RefreshAnimators;
    }


    void Start()
    {
        RefreshAnimators();
    }

    void Update()
    {
    }

    private void RefreshAnimators()
    {
        GetChildAnimators();
        SetRunState(_lastRun);
        SetStrafeState(_lastStrafe);
    }

    private void GetChildAnimators()
    {
        _animators.Clear();
        if (!runnerParent) return;
        for (int i = 0; i < runnerParent.childCount; i++)
        {
            var child = runnerParent.GetChild(i);
            if (child.TryGetComponent<Animator>(out var animator))
            {
                _animators.Add(animator);
            }
        }
    }

    public void SetRunState(bool isRunning)
    {
        _lastRun = isRunning;
        if (_animators.Count == 0) GetChildAnimators();
        for (int i = 0; i < _animators.Count; i++)
        {
            var a = _animators[i];
            if (a) a.SetBool(_animIDRunning, isRunning);
        }
    }

    public void SetIdleState(bool isRunning)
    {
        _lastRun = isRunning;
        if (_animators.Count == 0) GetChildAnimators();
        for (int i = 0; i < _animators.Count; i++)
        {
            var a = _animators[i];
            if (a) a.SetBool(_animIDRunning, isRunning);
        }
    }

    public void SetStrafeState(float differenceX)
    {
        if (_animators.Count == 0) GetChildAnimators();
        const float dead = 0.01f;
        _lastStrafe = Mathf.Clamp(differenceX, -1f, 1f);
        bool strafing = Mathf.Abs(_lastStrafe) > dead;
        for (int i = 0; i < _animators.Count; i++)
        {
            var a = _animators[i];
            if (!a) continue;
            a.SetBool(_isStrafeHash, strafing);
            a.SetFloat(_velocityXHash, strafing ? _lastStrafe : 0f);
        }
    }

    public void SetMultipleDeathState(Transform runner)
    {
        if (runner.TryGetComponent<Animator>(out var selfAnimator))
        {
            selfAnimator.SetTrigger(Dead);
            selfAnimator.SetBool(_isDead, true);
        }
      
    }

    public void SetSingleDeathState(Animator animator)
    {
        animator.SetTrigger(Dead);
        animator.SetBool(_isDead, true);
    }
}