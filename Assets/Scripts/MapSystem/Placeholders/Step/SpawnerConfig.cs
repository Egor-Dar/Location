using System;
using System.Collections.Generic;
using Spawners;
using UnityEngine;
using Object = UnityEngine.Object;

namespace MapSystem.Placeholders.Step
{
    [Serializable]
    public class SpawnerConfig
    {
        [SerializeField] private List<EnemySpawnInfo> handlers;
        [SerializeField] private Trigger trigger;
        private bool _initialized;


        public void Initialize()
        {
            if (_initialized) return;
            _initialized = true;
            trigger.Initialize((() => Print($"Spawn {handlers[0].Positions.Count} enemies")));
            Print("Initialize completed");
        }

        public void Update(Vector3 position) => trigger.Update(position);


        public void Draw()
        {
            if(handlers.Count == 0 && handlers[0]==null) return;
            foreach (var spawnInfo in handlers)
            {
                if(spawnInfo.Positions.Count== 0&& spawnInfo.Positions[0] == null) return;
                foreach (var position in spawnInfo.Positions)
                {
                    trigger.BindColor(Color.yellow);
                    Gizmos.color = Color.red;
                    Gizmos.DrawSphere(position.Value, 0.5f);
                    trigger.Draw();
                }
            }
        }

        private void Print(string text) => Debug.Log($"SpawnerConfig: {text}");
    }
}