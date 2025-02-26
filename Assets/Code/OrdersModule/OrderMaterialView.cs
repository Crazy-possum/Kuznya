using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OrderMaterialView : MonoBehaviour
{
    [SerializeField] private Image _materialImage;
    [SerializeField] private TMP_Text _materialName;
    [SerializeField] private TMP_Text _materialCount;

    public Image MaterialImage { get => _materialImage; set => _materialImage = value; }
    public TMP_Text MaterialName { get => _materialName; set => _materialName = value; }
    public TMP_Text MaterialCount { get => _materialCount; set => _materialCount = value; }
}