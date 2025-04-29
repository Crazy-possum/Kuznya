using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HardeningUIView : MonoBehaviour
{
    [SerializeField] private Slider _hardeningSlider;
    [SerializeField] private RectTransform _sliderCheckZone1;
    [SerializeField] private RectTransform _sliderCheckZone2;
    [SerializeField] private RectTransform _sliderCheckZone3;
    [SerializeField] private TMP_Text _scoreText;

    public Slider HardeningSlider => _hardeningSlider;
    public RectTransform SliderCheckZone1 => _sliderCheckZone1;
    public RectTransform SliderCheckZone2 => _sliderCheckZone2;
    public RectTransform SliderCheckZone3 => _sliderCheckZone3;
    public TMP_Text ScoreText => _scoreText;

    public void UpdateScore(int currentScore)
    {
        _scoreText.text = currentScore.ToString();
    }
}