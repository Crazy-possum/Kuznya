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
    [SerializeField] private RectTransform _scoreRoot;
    [SerializeField] private Image _itemImage;
    [SerializeField] private Image _barrelImage;
    [SerializeField] private Sprite _barrel1Sprite;
    [SerializeField] private Sprite _barrel2Sprite;

    public Slider HardeningSlider => _hardeningSlider;
    public RectTransform SliderCheckZone1 => _sliderCheckZone1;
    public RectTransform SliderCheckZone2 => _sliderCheckZone2;
    public RectTransform SliderCheckZone3 => _sliderCheckZone3;
    public TMP_Text ScoreText => _scoreText;
    public RectTransform ScoreRoot => _scoreRoot;
    public Image ItemImage => _itemImage;
    public Image BarrelImage => _barrelImage;
    public Sprite Barrel1Sprite => _barrel1Sprite;
    public Sprite Barrel2Sprite => _barrel2Sprite;

    public void UpdateScore(int currentScore)
    {
        _scoreText.text = currentScore.ToString();
    }
}