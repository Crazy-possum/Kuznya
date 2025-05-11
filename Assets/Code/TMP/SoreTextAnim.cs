using System;
using System.Collections;
using System.Collections.Generic;
using MAEngine.Extention;
using UnityEngine;
using Random = UnityEngine.Random;

public class SoreTextAnim : MonoBehaviour
{
    [SerializeField] private RectTransform _textRectTransform;
    //[SerializeField] private float _flyUpSpeed = 100f;
    //[SerializeField] private float _fallSpeed = 50f;
    [SerializeField] private float _xOffset = 30f;
    [SerializeField] private float _lifetime = 1f;

    private Timer _animationTimer;
    private Vector2 _startPosition;
    private Vector2 _targetPosition;
    private Vector2 _fallStartPosition;
    private Vector2 _fallTargetPosition;
    private bool _isFalling;
    private float _randomX;

    private void OnEnable()
    {
        InitializeAnimation();
    }

    private void OnDisable()
    {
        _animationTimer = null;
    }

    private void InitializeAnimation()
    {
        _startPosition = _textRectTransform.anchoredPosition;
        _randomX = Random.Range(-_xOffset, _xOffset);
        _targetPosition = _startPosition + new Vector2(_randomX, 100f);
        _animationTimer = new Timer(_lifetime * 0.5f);
        _isFalling = false;
    }

    private void FixedUpdate()
    {
        if (_animationTimer == null) return;

        float progress = (_lifetime * 0.5f - _animationTimer.GetRemainingTime()) / (_lifetime * 0.5f);
        progress = Mathf.Clamp01(progress);

        if (!_isFalling)
        {
            AnimateFlyUp(progress);
            if (_animationTimer.Wait())
            {
                StartFall();
            }
        }
        else
        {
            AnimateFall(progress);
            if (_animationTimer.Wait())
            {
                Destroy(gameObject);
            }
        }
    }

    private void AnimateFlyUp(float progress)
    {
        float easedProgress = 1 - (1 - progress) * (1 - progress);
        Vector2 currentPos = Vector2.Lerp(_startPosition, _targetPosition, progress);
        currentPos.y = Mathf.Lerp(_startPosition.y, _targetPosition.y, easedProgress);
        _textRectTransform.anchoredPosition = currentPos;
    }

    private void StartFall()
    {
        _isFalling = true;
        _fallStartPosition = _textRectTransform.anchoredPosition;
        _fallTargetPosition = _fallStartPosition + Vector2.down * 50f;
        _animationTimer = new Timer(_lifetime * 0.5f);
    }

    private void AnimateFall(float progress)
    {
        float easedProgress = progress * progress;
        Vector2 currentPos = Vector2.Lerp(_fallStartPosition, _fallTargetPosition, progress);
        currentPos.y = Mathf.Lerp(_fallStartPosition.y, _fallTargetPosition.y, easedProgress);
        _textRectTransform.anchoredPosition = currentPos;
    }
}
