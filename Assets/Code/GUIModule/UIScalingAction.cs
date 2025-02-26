using GameCoreModule;
using MAEngine;
using MAEngine.Extention;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace MainGUI
{
    public class UIScalingAction : IAction, IInitialisation, IFixedExecute, IExecute, ICleanUp
    {
        private EconomyEventBus _economyEventBus;
        private CanvasScaler _canvasScaler;
        private RectTransform _viewportTransform;
        private RectTransform _mainPanelTransform;
        private Vector2Seralizable _minScale;
        private Vector2Seralizable _maxScale;
        private float _scaleSpeed;
        private float _panelMoveSpeed;
        
        private float _mouseWheelDelta;
        private Vector2 _currentScale;
        private bool _isDragging = false;
        private Vector2 _lastMousePosition;
        private Vector2 _targetPosition;
        private bool _isControlable;
        
        [Inject]
        public void Construct(EconomyEventBus economyEventBus, ScalableView scalableView)
        {
            _economyEventBus = economyEventBus;
            _canvasScaler = scalableView.CanvasScaler;
            _viewportTransform = scalableView.ViewportTransform;
            _mainPanelTransform = scalableView.MainPanelTransform;
            _minScale = scalableView.MinScale;
            _maxScale = scalableView.MaxScale;
            _scaleSpeed = scalableView.ScaleSpeed;
            _panelMoveSpeed = scalableView.PanelMoveSpeed;
        }
        
        
        public void Initialisation()
        {
            _isControlable = true;
            _currentScale = _canvasScaler.referenceResolution;
            _targetPosition = _mainPanelTransform.anchoredPosition;
            _economyEventBus.OnTryBuyUpgrade += TryBuyUpgrade;
            _economyEventBus.OnBuyUpgradeCanceled += CancelBuyUpgrade;
        }

        public void Execute(float deltaTime)
        {
            if (_canvasScaler.gameObject.activeInHierarchy && _isControlable)
            {
                _mouseWheelDelta += Input.mouseScrollDelta.y;
                if (Input.GetMouseButtonDown(0) & !_isDragging)
                {
                    _isDragging = true;
                    _lastMousePosition = Input.mousePosition;
                }
                else if (Input.GetMouseButtonUp(0))
                {
                    _isDragging = false;
                    _targetPosition = _mainPanelTransform.anchoredPosition;
                }
            
                if (_isDragging)
                {
                    Vector2 delta = (Vector2)Input.mousePosition - _lastMousePosition;
                    SetTargetPosition(delta);
                    _lastMousePosition = Input.mousePosition;
                }
                MovePanel();
            }
        }

        public void FixedExecute(float fixedDeltaTime)
        {
            if (_mouseWheelDelta != 0)
            {
                ChangeUIScale();
                _mouseWheelDelta = 0;
            }
        }
        
        public void Cleanup()
        {
            _economyEventBus.OnTryBuyUpgrade -= TryBuyUpgrade;
            _economyEventBus.OnBuyUpgradeCanceled -= CancelBuyUpgrade;
        }

        private void ChangeUIScale()
        {
            Vector2 mousePositionBeforeScale = Input.mousePosition;
            Vector2 normalizedMousePos = new Vector2(
                mousePositionBeforeScale.x / Screen.width,
                mousePositionBeforeScale.y / Screen.height
            );

            Vector2 oldScale = _currentScale;
            Vector2 newScale = _currentScale - new Vector2(_mouseWheelDelta * _scaleSpeed, _mouseWheelDelta * _scaleSpeed);
            newScale.x = Mathf.Clamp(newScale.x, _minScale.X, _maxScale.X);
            newScale.y = Mathf.Clamp(newScale.y, _minScale.Y, _maxScale.Y);
            _canvasScaler.referenceResolution = newScale;
            _currentScale = newScale;

            Vector2 oldPanelPos = _mainPanelTransform.anchoredPosition;
            Vector2 newPanelPos = new Vector2(
                oldPanelPos.x * (newScale.x / oldScale.x),
                oldPanelPos.y * (newScale.y / oldScale.y)
            );

            Vector2 mouseDelta = new Vector2(
                -(normalizedMousePos.x - 0.5f) * (newScale.x - oldScale.x),
                -(normalizedMousePos.y - 0.5f) * (newScale.y - oldScale.y)
            );

            newPanelPos -= mouseDelta;

            _targetPosition = newPanelPos;
            ClampPanelPosition();
            MovePanel();
        }
        
        private void SetTargetPosition(Vector2 delta)
        {
            _targetPosition += delta;
            ClampPanelPosition();
        }

        private void ClampPanelPosition()
        {
            Vector2 panelSize = _mainPanelTransform.rect.size;
            Vector2 viewportSize = _viewportTransform.rect.size;

            float maxX = -(viewportSize.x - panelSize.x) / 2;
            float maxY = -(viewportSize.y - panelSize.y) / 2;

            _targetPosition.x = Mathf.Clamp(_targetPosition.x, -maxX, maxX);
            _targetPosition.y = Mathf.Clamp(_targetPosition.y, -maxY, maxY);
        }
        
        private void MovePanel()
        {
            _mainPanelTransform.anchoredPosition = Vector2.Lerp(
                _mainPanelTransform.anchoredPosition, 
                _targetPosition, 
                Time.deltaTime * _panelMoveSpeed
            );
        }
        
        private void TryBuyUpgrade(UpgradeName arg1, UpgradeView arg2)
        {
            _isControlable = false;
        }
        
        private void CancelBuyUpgrade()
        {
            _isControlable = true;
        }
    }
}
