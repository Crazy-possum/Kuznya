using MAEngine.Extention;
using UnityEngine;
using UnityEngine.UI;

namespace MainGUI
{
    public class ScalableView : MonoBehaviour
    {
        [SerializeField] private CanvasScaler _canvasScaler;
        [SerializeField] private RectTransform _viewportTransform;
        [SerializeField] private RectTransform _mainPanelTransform;
        [SerializeField] private Vector2Seralizable _minScale = new Vector2Seralizable(1920, 1080);
        [SerializeField] private Vector2Seralizable _maxScale = new Vector2Seralizable(5760, 3240);
        [SerializeField] private float _scaleSpeed = 150f;
        [SerializeField] private float _panelMoveSpeed = 200f;
    
        public CanvasScaler CanvasScaler => _canvasScaler;
        public RectTransform ViewportTransform => _viewportTransform;
        public RectTransform MainPanelTransform => _mainPanelTransform;
        public Vector2Seralizable MinScale => _minScale;
        public Vector2Seralizable MaxScale => _maxScale;
        public float ScaleSpeed => _scaleSpeed;
        public float PanelMoveSpeed => _panelMoveSpeed;
    }
}
