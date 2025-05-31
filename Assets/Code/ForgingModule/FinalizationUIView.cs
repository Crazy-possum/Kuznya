using System.Collections;
using System.Collections.Generic;
using MAEngine.Extention;
using MainGUI;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class FinalizationUIView : MonoBehaviour
{
    [SerializeField] private SerializableDictionary<ButtonNames, ButtonView> _memoryButtons;
    [SerializeField] private Image _itemImage;
    [SerializeField] private Image _handleImage;
    [SerializeField] private TMP_Text _scoreText;
    [SerializeField] private Sprite _defaultButtonSprite;
    [SerializeField] private Sprite _greenButtonSprite;
    [SerializeField] private Sprite _redButtonSprite;
    [SerializeField] private GameObject _blockerPanel;
    [SerializeField] private GameObject _memoryTextPanel;
    [SerializeField] private GameObject _finalizeTextPanel;
    [SerializeField] private RectTransform _scoreRoot;
    
    public SerializableDictionary<ButtonNames, ButtonView> MemoryButtons => _memoryButtons;
    public Image ItemImage => _itemImage;
    public Image HandleImage => _handleImage;
    public Sprite DefaultButtonSprite => _defaultButtonSprite;
    public Sprite GreenButtonSprite => _greenButtonSprite;
    public Sprite RedButtonSprite => _redButtonSprite;
    public GameObject BlockerPanel => _blockerPanel;
    public GameObject MemoryTextPanel => _memoryTextPanel;
    public GameObject FinalizeTextPanel => _finalizeTextPanel;
    public RectTransform ScoreRoot => _scoreRoot;

    public void UpdateScore(int score)
    {
        _scoreText.text = score.ToString();
    }

    public void HighlightGreen(ButtonNames buttonName)
    {
        MemoryButtons.GetValue(buttonName).ButtonImage.sprite = _greenButtonSprite;
    }
    
    public void HighlightRed(ButtonNames buttonName)
    {
        MemoryButtons.GetValue(buttonName).ButtonImage.sprite = _redButtonSprite;
    }
    
    public void ResetButton(ButtonNames buttonName)
    {
        MemoryButtons.GetValue(buttonName).ButtonImage.sprite = _defaultButtonSprite;
    }
}