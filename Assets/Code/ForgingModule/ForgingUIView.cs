using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ForgingUIView : MonoBehaviour
{
    [SerializeField] private TMP_Text _scoreText;
    [SerializeField] private Slider _progressSlider;
    [SerializeField] private WorkZoneUIView _zone1view;
    [SerializeField] private WorkZoneUIView _zone2view;
    [SerializeField] private WorkZoneUIView _zone3view;

    public TMP_Text ScoreText { get => _scoreText; }
    public Slider ProgressSlider { get => _progressSlider; }
    public WorkZoneUIView Zone1view { get => _zone1view; }
    public WorkZoneUIView Zone2view { get => _zone2view; }
    public WorkZoneUIView Zone3view { get => _zone3view; }
}
