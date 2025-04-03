using GameCoreModule;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MainGUI
{
    public class ButtonView : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private Image _buttonImage;
        [SerializeField] private TMP_Text _buttonText;
    
        public Button Button => _button;
        public Image ButtonImage => _buttonImage;
        public TMP_Text ButtonText => _buttonText;
    }
}

