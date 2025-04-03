using System.Collections.Generic;
using MAEngine.Extention;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ForgingUIView : MonoBehaviour
{
    [SerializeField] private TMP_Text _scoreText;
    [SerializeField] private Slider _progressSlider;
    [SerializeField] private SerializableDictionary<OrderType, ForgingStepsView> _stepsDict;
    [SerializeField] private Image _itemImage;

    public TMP_Text ScoreText { get => _scoreText; }
    public Slider ProgressSlider { get => _progressSlider; }
    public SerializableDictionary<OrderType, ForgingStepsView> StepsDict => _stepsDict;
    public Image ItemImage { get => _itemImage; }
}
