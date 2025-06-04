using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorkZoneUIView : MonoBehaviour
{
    [SerializeField] private Button _zoneButton;
    [SerializeField] private RectTransform _scoreRoot;
    [SerializeField] private List<ScoreTextView> _scoreTextList;
    [SerializeField] private int _maxScoreTextsCount;

    public Button ZoneButton { get => _zoneButton; }
    public RectTransform ScoreRoot { get => _scoreRoot; }
    public List<ScoreTextView> ScoreTextList { get => _scoreTextList; }

    public void AddTextToList(ScoreTextView text)
    {
        if (_scoreTextList.Count >= _maxScoreTextsCount)
        {
            if (_scoreTextList[0].gameObject != null)
            {
                GameObject textObject = _scoreTextList[0].gameObject;
                _scoreTextList.Remove(_scoreTextList[0]);
                Destroy(textObject);
            }
        }
        _scoreTextList.Add(text);
    }
}
