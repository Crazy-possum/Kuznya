using UnityEngine;
using UnityEngine.UI;

public class SignalObjectView : MonoBehaviour
{
    [SerializeField] private Image _signalImage;
    [SerializeField] private Sprite _goodSprite;
    [SerializeField] private Sprite _badSprite;
    [SerializeField] private Sprite _activeSptrite;
    
    public Image SignalImage => _signalImage;
    public Sprite SignalGoodSprite => _goodSprite;
    public Sprite SignalBadSprite => _badSprite;
    public Sprite SignalActiveSprite => _activeSptrite;
}