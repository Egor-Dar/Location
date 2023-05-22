using System;
using Better.Attributes.Runtime.Gizmo;
using Newtonsoft.Json;
using Spawners;
using UnityEngine;

namespace MapSystem.Placeholders.Step
{
    [Serializable]
    public class Trigger
    {
        [JsonProperty] [SerializeField] private BoundRef bound;
        private bool _continueEnter;
        private bool _continueExit;
        private bool _initialized;
        private Action _onEnter;
        private Action _onExit;
        private Color _boundColor;

        public BoundRef Bound => bound;

        public void BindBound(BoundRef variant) => bound = variant;

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
        public class BoundRef
        {
            [Gizmo] [SerializeField] private Bounds bound;
            [JsonIgnore] private Vector3Ref _setter;

            [JsonIgnore]
            public Vector3Ref SizeJson
            {
                get
                {
                    _setter ??= new Vector3Ref();
                    _setter.Value = bound.size;
                    return _setter;
                }
                set => bound.size = value.Value;
            }

            [JsonIgnore]
            public Vector3Ref PositionJson
            {
                get
                {
                    _setter ??= new Vector3Ref();
                    _setter.Value = bound.center;
                    return _setter;
                }
                set => bound.center = value.Value;
            }

            [JsonIgnore]
            public Vector3 Size
            {
                get => bound.size;
                set
                {
                    _setter ??= new Vector3Ref();
                    _setter.Value = value;
                    SizeJson = _setter;
                }
            }

            [JsonIgnore]
            public Vector3 Position
            {
                get => bound.center;
                set
                {
                    _setter ??= new Vector3Ref();
                    _setter.Value = value;
                    PositionJson = _setter;
                }
            }

            public bool Contains(Vector3 position) => bound.Contains(position);
        }
    }
}