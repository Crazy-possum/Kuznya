using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeConfirmView : MonoBehaviour
{
    [SerializeField] private TMP_Text _costAndInfoText;
    [SerializeField] private TMP_Text _description;
    [SerializeField] private Button _confirmButton;
    [SerializeField] private Button _cancelButton;
    
    public TMP_Text CostAndInfoText => _costAndInfoText;
    public TMP_Text Description => _description;
    public Button ConfirmButton => _confirmButton;
    public Button CancelButton => _cancelButton;
}
