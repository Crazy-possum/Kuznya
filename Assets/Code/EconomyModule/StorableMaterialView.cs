using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Economy
{
    public class StorableMaterialView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _countText;
        [SerializeField] private Image _materialImage;
        [SerializeField] private Button _materialButton;
        [SerializeField] private GameObject _highlightObject;
        [SerializeField] private RectTransform _materialRectTransform;
        
        public TMP_Text CountText { get => _countText; }
        public Image MaterialImage { get => _materialImage; }
        public Button MaterialButton { get => _materialButton; }
        public RectTransform MaterialRectTransform { get => _materialRectTransform; }
        
        

        public void UpdateMaterialCount(int materialCount)
        {
            string materialCountText = $"{materialCount} шт.";
            _countText.text = materialCountText;
        }

        public void SetHighlight(bool isHighlighted)
        {
            _highlightObject.gameObject.SetActive(isHighlighted);
        }
    }
}