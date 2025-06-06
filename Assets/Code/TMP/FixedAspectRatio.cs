using UnityEngine;

public class FixedAspectRatio : MonoBehaviour
{
    [SerializeField] private float _targetAspect = 16f / 9f; // 16:9
    private Camera _camera;

    private void Start()
    {
        _camera = GetComponent<Camera>();
        UpdateViewport();
    }

    private void Update()
    {
        // Обновляем при изменении разрешения экрана
        if (Screen.width != _lastWidth || Screen.height != _lastHeight)
        {
            UpdateViewport();
        }
    }

    private int _lastWidth;
    private int _lastHeight;

    private void UpdateViewport()
    {
        // Текущее соотношение экрана
        float screenAspect = (float)Screen.width / Screen.height;
        
        // Расчёт масштаба viewport
        float scale = screenAspect > _targetAspect 
            ? _targetAspect / screenAspect   // Боковые полосы
            : screenAspect / _targetAspect;  // Верхние/нижние полосы

        Rect viewportRect = new Rect
        {
            width = scale,
            height = 1,
            x = (1 - scale) / 2f,
            y = 0
        };

        if (screenAspect < _targetAspect)
        {
            viewportRect.width = 1;
            viewportRect.height = scale;
            viewportRect.x = 0;
            viewportRect.y = (1 - scale) / 2f;
        }

        _camera.rect = viewportRect;
        _lastWidth = Screen.width;
        _lastHeight = Screen.height;
    }
}
