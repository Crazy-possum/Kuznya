using Economy;
using MAEngine.Extention;
using TMPro;
using UnityEngine;

public class ScoreTextView : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;
    private WorkZoneUIView _workZoneUIView;
    private Timer _lifeTimer;

    public TMP_Text Text { get => _text; }
    public Timer LifeTimer { get => _lifeTimer; }

    public void InitializeView(WorkZoneUIView workZoneUIView, float lifetime)
    {
        _workZoneUIView = workZoneUIView;
        _lifeTimer = new Timer(lifetime);
    }

    public void RemoveText()
    {
        _workZoneUIView.ScoreTextList.Remove(this);
        Destroy(gameObject);
    }
}