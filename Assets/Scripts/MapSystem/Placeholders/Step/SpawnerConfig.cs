using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Spawners;
using UnityEngine;

namespace MapSystem.Placeholders.Step
{
    [Serializable]
    public class SpawnerConfig
    {
        [SerializeField] private List<EnemySpawnInfo> enemies;
        [SerializeField] private Trigger trigger;
        private bool _initialized;

        [JsonIgnore] public List<EnemySpawnInfo> Enemies => enemies;
        [JsonIgnore] public Trigger Trigger => trigger;
        public void BindTrigger(Trigger value) => trigger = value;

        public void AddEnemy(EnemySpawnInfo variant)
        {
            enemies ??= new List<EnemySpawnInfo>();
            enemies.Add(variant);
        }
        public void Initialize()
        {
            if (_initialized) return;
            _initialized = true;
            trigger.Initialize((() => Print($"Spawn {enemies[0].Positions.Count} enemies")));
            Print("Initialize completed");
        }

        public void Update(Vector3 position) => trigger.Update(position);


        public void Draw()
        {
            if (enemies.Count == 0 && enemies[0] == null) return;
            foreach (var spawnInfo in enemies)
            {
                if (spawnInfo.Positions.Count == 0 && spawnInfo.Positions[0] == null) return;
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