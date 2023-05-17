using System;
using Better.Attributes.Runtime.Gizmo;
using UnityEngine;

namespace MapSystem.Placeholders.Step
{
    [Serializable]
    internal class Trigger
    {
        [SerializeField] private BoundRef bound;
        private bool _continueEnter;
        private bool _continueExit;
        private bool _initialized;
        private Action _onEnter;
        private Action _onExit;
        private Color _boundColor;

        public void Initialize(Action onEntered)
        {
            if (_initialized) return;
            _initialized = true;
            _onEnter += onEntered;
        }

        public void Initialize(Action onEntered, Action onExit)
        {
            if (_initialized) return;
            _initialized = true;
            _onEnter += onEntered;
            _onExit += onExit;
        }

        public void BindColor(Color color) => _boundColor = color;

        public void Update(Vector3 position)
        {
            if (!_continueEnter && bound.Contains(position))
            {
                _continueEnter = true;
                _continueExit = true;
                _onEnter?.Invoke();
            }

            if (_continueExit && !bound.Contains(position))
            {
                _continueExit = false;
                _onExit?.Invoke();
            }
        }

        public void Draw()
        {
            Gizmos.color = _boundColor;
            Gizmos.DrawWireCube(bound.Position, bound.Size);
        }

        [Serializable]
        private class BoundRef
        {
            [Gizmo] [SerializeField] private Bounds bound;

            public Vector3 Size
            {
                get => bound.size;
                set { bound.size = value; }
            }

            public Vector3 Position
            {
                get => bound.center;
                set { bound.center = value; }
            }

            public bool Contains(Vector3 position) => bound.Contains(position);
        }
    }
}