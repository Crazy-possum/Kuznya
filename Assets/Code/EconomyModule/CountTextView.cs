using System;
using MAEngine.Extention;
using TMPro;
using UnityEngine;

namespace Economy
{
    public class CountTextView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _text;
        [SerializeField] private float _lifetime;
        private MaterialsUIView _materialsUIView;
        private Timer _lifeTimer;

        public TMP_Text Text { get => _text; }
        public Timer LifeTimer { get => _lifeTimer; }

        public void FixedUpdate()
        {
            if (_lifeTimer.Wait())
            {
                RemoveText();
            }
        }

        public void InitializeView(MaterialsUIView materialsUIView, float lifetime)
        {
            _materialsUIView = materialsUIView;
            _lifeTimer = new Timer(lifetime);
        }

        public void InitializeView(MaterialsUIView materialsUIView)
        {
            _materialsUIView = materialsUIView;
            _lifeTimer = new Timer(_lifetime);
        }

        public void RemoveText()
        {
            _materialsUIView.AddingCountTextList.Remove(this);
            Destroy(gameObject);
        }
    }
}