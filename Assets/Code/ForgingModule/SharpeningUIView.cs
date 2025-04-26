using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SharpeningUIView : MonoBehaviour
{
    [SerializeField] private Slider _hardeningSlider;
    [SerializeField] private RectTransform _sliderCheckZone;
    [SerializeField] private RectTransform _targetZoneGreen;
    [SerializeField] private RectTransform _targetZoneYellow;
    [SerializeField] private Button _hardeningButton;
    [SerializeField] private TMP_Text _scoreText;
    
    public Slider HardeningSlider => _hardeningSlider;
    public RectTransform SliderCheckZone => _sliderCheckZone;
    public RectTransform TargetZoneGreen => _targetZoneGreen;
    public RectTransform TargetZoneYellow => _targetZoneYellow;
    public Button HardeningButton => _hardeningButton;
    public TMP_Text ScoreText => _scoreText;
}