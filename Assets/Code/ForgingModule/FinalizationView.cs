using System.Collections;
using System.Collections.Generic;
using MainGUI;
using UnityEngine;
using UnityEngine.UI;

public class FinalizationView : MonoBehaviour
{
    [SerializeField] private List<ButtonView> _memoryButtons;
    [SerializeField] private Image _itemImage;
    [SerializeField] private Image _handleImage;
    
    public List<ButtonView> MemoryButtons => _memoryButtons;
    public Image ItemImage => _itemImage;
    public Image HandleImage => _handleImage;
}