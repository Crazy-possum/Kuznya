using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResultsUIView : MonoBehaviour
{
    [SerializeField] private TMP_Text _staticScoreText;
    [SerializeField] private TMP_Text _dynamicScoreText;
    [SerializeField] private TMP_Text _goldText;
    [SerializeField] private Button _endButton;

    public TMP_Text StaticScoreText { get => _staticScoreText; }
    public TMP_Text DynamicScoreText { get => _dynamicScoreText; }
    public TMP_Text GoldText { get => _goldText; }
    public Button EndButton { get => _endButton; }
}
