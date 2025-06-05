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
    [SerializeField] private GameObject _barrel1Image1;
    [SerializeField] private GameObject _barrel1Image2;
    [SerializeField] private GameObject _barrel2Image1;
    [SerializeField] private GameObject _barrel2Image2;
    [SerializeField] private Animator _hardeningAnimator;

    public Slider HardeningSlider => _hardeningSlider;
    public RectTransform SliderCheckZone1 => _sliderCheckZone1;
    public RectTransform SliderCheckZone2 => _sliderCheckZone2;
    public RectTransform SliderCheckZone3 => _sliderCheckZone3;
    public TMP_Text ScoreText => _scoreText;
    public RectTransform ScoreRoot => _scoreRoot;
    public Image ItemImage => _itemImage;
    public GameObject Barrel1Image1 => _barrel1Image1;
    public GameObject Barrel1Image2 => _barrel1Image2;
    public GameObject Barrel2Image1 => _barrel2Image1;
    public GameObject Barrel2Image2 => _barrel2Image2;
    public Animator HardeningAnimator => _hardeningAnimator;

    public void UpdateScore(int currentScore)
    {
        _scoreText.text = currentScore.ToString();
    }
}