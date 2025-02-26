using UnityEngine;
using UnityEngine.UI;

public class LevelIndicatorView : MonoBehaviour
{
    [SerializeField] private Image _levelIndicatorImage;
    [SerializeField] private Sprite _lockedSprite;
    [SerializeField] private Sprite _unlockedSprite;

    public void SetLevelIndicator(bool isUnlocked)
    {
        if (isUnlocked)
        {
            _levelIndicatorImage.sprite = _unlockedSprite;
        }
        else
        {
            _levelIndicatorImage.sprite = _lockedSprite;
        }
    }
    
}