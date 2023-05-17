using System;
using System.Collections.Generic;
using UnityEngine;

namespace MapSystem.Placeholders.Step
{
    [Serializable]
    public class Spawners
    {
        [SerializeField] private List<SpawnerConfig> spawners;

        private bool _initialized = false;
        private Transform _player;

        public void BindPlayerTransform(Transform transform) => _player = transform;

        public void Initialize()
        {
            if (_initialized) return;
            _initialized = true;
            foreach (var spawner in spawners)
            {
                spawner
                    .Initialize();
            }
        }

        public void Update()
        {
            if (!_initialized) return;
            foreach (var spawner in spawners)
            {
                spawner.Update(_player.position);
            }
        }

        public void Draw()
        {
            if (spawners is null || spawners.Count == 0) return;
            foreach (var spawner in spawners)
            {
                spawner.Draw();
            }
        }
    }
}