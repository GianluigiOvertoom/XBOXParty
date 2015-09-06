using UnityEngine;
using System.Collections;

namespace Board
{
    [RequireComponent(typeof(RectTransform))]
    public class UIHover : MonoBehaviour
    {
        [SerializeField]
        private float _amplitude = 1.0f;

        [SerializeField]
        private float _frequency = 1.0f;

        private RectTransform _rectTransform;
        private float _timer = 0.0f;
        private float _defaultY = 0.0f;

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
            _defaultY = _rectTransform.anchoredPosition.y;
        }

        private void Update()
        {
            float sinValue = Mathf.Sin(_timer * _frequency) * _amplitude;
            Vector3 anchoredPos = _rectTransform.anchoredPosition;
            anchoredPos.y = _defaultY + sinValue;

            _rectTransform.anchoredPosition = anchoredPos;

            _timer += Time.deltaTime;
        }
    }
}
