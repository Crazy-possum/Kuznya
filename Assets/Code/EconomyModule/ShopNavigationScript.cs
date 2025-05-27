using MainGUI;
using UnityEngine;

namespace Economy
{
    public class ShopNavigationScript : MonoBehaviour
    {
        [SerializeField] private GameObject _tab1;
        [SerializeField] private GameObject _tab2;
        [SerializeField] private GameObject _tab3;
        [SerializeField] private GameObject _tab4;
        [SerializeField] private GameObject _tab5;
        [SerializeField] private GameObject _tab6;
    
        [SerializeField] private ButtonView _tab1Button;
        [SerializeField] private ButtonView _tab2Button;
        [SerializeField] private ButtonView _tab3Button;
        [SerializeField] private ButtonView _tab4Button;
        [SerializeField] private ButtonView _tab5Button;
        [SerializeField] private ButtonView _tab6Button;

        private GameObject _activeTab;


        private void Start()
        {
            InitializeTabs();
            _tab1Button.Button.onClick.AddListener(() => SwitchTab(_tab1));
            _tab2Button.Button.onClick.AddListener(() => SwitchTab(_tab2));
            _tab3Button.Button.onClick.AddListener(() => SwitchTab(_tab3));
            _tab4Button.Button.onClick.AddListener(() => SwitchTab(_tab4));
            _tab5Button.Button.onClick.AddListener(() => SwitchTab(_tab5));
            _tab6Button.Button.onClick.AddListener(() => SwitchTab(_tab6));
        }

        private void OnDestroy()
        {
            _tab1Button.Button.onClick.RemoveAllListeners();
            _tab2Button.Button.onClick.RemoveAllListeners();
            _tab3Button.Button.onClick.RemoveAllListeners();
            _tab4Button.Button.onClick.RemoveAllListeners();
            _tab5Button.Button.onClick.RemoveAllListeners();
            _tab6Button.Button.onClick.RemoveAllListeners();
        }

        private void InitializeTabs()
        {
            _tab1.SetActive(false);
            _tab2.SetActive(false);
            _tab3.SetActive(false);
            _tab4.SetActive(false);
            _tab5.SetActive(false);
            _tab6.SetActive(false);
            _tab1.SetActive(true);
            _activeTab = _tab1;
        }

        private void SwitchTab(GameObject tab)
        {
            if (_activeTab != tab)
            {
                _activeTab.SetActive(false);
                tab.SetActive(true);
                _activeTab = tab;
            }
        }
    }

}
