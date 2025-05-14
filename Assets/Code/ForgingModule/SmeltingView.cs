using MAEngine;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SmeltingView : MonoBehaviour
{
    [SerializeField] private Slider _temperatureSlider;
    [SerializeField] private Button _bellowsButton;
    [SerializeField] private RectTransform _spawnZoneTransform;
    [SerializeField] private RectTransform _endZoneTransform;
    [SerializeField] private RectTransform _targetZoneTransform;
    [SerializeField] private TMP_Text _timerText;
    
    public Slider TemperatureSlider => _temperatureSlider;
    public Button BellowsButton => _bellowsButton;
    public RectTransform SpawnZoneTransform => _spawnZoneTransform;
    public RectTransform EndZoneTransform => _endZoneTransform;
    public RectTransform TargetZoneTransform => _targetZoneTransform;
    public TMP_Text TimerText => _timerText;
}
