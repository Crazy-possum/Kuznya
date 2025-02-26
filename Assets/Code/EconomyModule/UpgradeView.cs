using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeView : MonoBehaviour
{
    [SerializeField] private Button _upgradeButton;
    [SerializeField] private Image _upgradeImage;
    [SerializeField] private TMP_Text _titleText;
    [SerializeField] private TMP_Text _descriptionText;
    [SerializeField] private TMP_Text _currentValueText;
    [SerializeField] private List<LevelIndicatorView> _levelIndicators;
    private UpgradeConfig _upgradeConfig;
    private int _currentLevel;
    private int _currentValue;

    public Button UpgradeButton => _upgradeButton;
    public TMP_Text CurrentValueText => _currentValueText;
    public UpgradeConfig UpgradeConfig => _upgradeConfig;
    public int CurrentValue => _currentValue;

    public void UpdateUI(UpgradeConfig upgradeConfig, int level)
    {
        _upgradeImage.sprite = upgradeConfig.UpgradeSprite;
        _titleText.text = upgradeConfig.Title;
        _descriptionText.text = upgradeConfig.Description;
        _upgradeConfig = upgradeConfig;
        UpdateLevelIndication(level);
    }

    public void UpdateLevelIndication(int level)
    {
        int maxLevel = _upgradeConfig.LevelsCostList.Count;
        if (level < maxLevel)
        {
            _currentValueText.text = $"{_upgradeConfig.LevelsCostList[level]}";
            _currentValue = _upgradeConfig.LevelsCostList[level];
        }
        else
        {
            _currentValueText.text = $"Макс.";
        }
        
        for (int i = 0; i < maxLevel; i++)
        {
            _levelIndicators[i].gameObject.SetActive(true);
        }
        
        foreach (LevelIndicatorView levelIndicator in _levelIndicators)
        {
            levelIndicator.SetLevelIndicator(false);
        }

        for (int i = 0; i < level; i++)
        {
            _levelIndicators[i].SetLevelIndicator(true);
        }
    }

    public void SetLevel(int level)
    {
        _currentLevel = level;
        UpdateLevelIndication(_currentLevel);
    }

    public void AddLevel()
    {
        _currentLevel++;
        int maxLevel = _upgradeConfig.LevelsCostList.Count;
        if (_currentLevel >= maxLevel)
        {
            _upgradeButton.interactable = false;
            _currentLevel = maxLevel;
        }
        UpdateLevelIndication(_currentLevel);
    }
}