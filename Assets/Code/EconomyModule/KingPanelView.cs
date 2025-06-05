using UnityEngine;
using UnityEngine.UI;

namespace Economy
{
    public class KingPanelView : MonoBehaviour
    {
        [SerializeField] private GameObject _kingPanel;
        [SerializeField] private Button _giveMoneyButton;

        public GameObject KingPanel => _kingPanel;
        public Button GiveMoneyButton => _giveMoneyButton;
    }
}