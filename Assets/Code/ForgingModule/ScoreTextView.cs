using Economy;
using MAEngine.Extention;
using TMPro;
using UnityEngine;

public class ScoreTextView : MonoBehaviour
{
    [SerializeField] private RectTransform _textRectTransform;
    [SerializeField] private TMP_Text _text;
    [SerializeField] private Color _textColor;
    [SerializeField] private Color _goodTextColor;
    private WorkZoneUIView _workZoneUIView;
    private Timer _lifeTimer;

    public RectTransform TextRectTransform { get => _textRectTransform; }
    public TMP_Text Text { get => _text; }
    public Timer LifeTimer { get => _lifeTimer; }
    public Color TextColor { get => _textColor; }
    public Color GoodTextColor { get => _goodTextColor; }

    public void InitializeView(WorkZoneUIView workZoneUIView, float lifetime)
    {
        _workZoneUIView = workZoneUIView;
        _lifeTimer = new Timer(lifetime);
    }
    
    public void InitializeView(float lifetime)
    {
        _lifeTimer = new Timer(lifetime);
    }

    public void RemoveText()
    {
        if (_workZoneUIView != null)
        {
            _workZoneUIView.ScoreTextList.Remove(this);
        }
        Destroy(gameObject);
    }
    
    public void SetTextColor(Color color)
    {
        _text.color = color;
    }
}