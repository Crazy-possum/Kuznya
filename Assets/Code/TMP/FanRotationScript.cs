using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FanRotationScript : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private Button _fanButton;
    [SerializeField] private TMP_Text _scoreText;
    [SerializeField] private int _maxSpeed;
    private int _score;
    private float _fanSpeed;


    private void Start()
    {
        _fanButton.onClick.AddListener(() => FanButtonClick());
    }

    private void OnDestroy()
    {
        _fanButton.onClick.RemoveListener(() => FanButtonClick());
    }

    private void FixedUpdate()
    {
        if (_fanSpeed >= 0)
        {
            _fanSpeed -= 0.01f;
            if (_fanSpeed < 0)
            {
                _fanSpeed = 0;
            }
        }
        UpdateAnimation();
    }

    private void FanButtonClick()
    {
        if (_fanSpeed <= _maxSpeed)
        {
            _fanSpeed += 0.5f;
        }
        _score++;
        UpdateScoreText();
    }

    private void UpdateScoreText()
    {
        _scoreText.text = $"{_score}";
    }

    private void UpdateAnimation()
    {
        _animator.SetFloat("Speed", _fanSpeed);
    }
}
