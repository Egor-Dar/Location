using System;
using System.Collections.Generic;
using MapSystem.Placeholders.Step;
using UnityEngine;
using LocationInfo = Scriptable_objects.LocationInfo;
using Newtonsoft.Json;

namespace Location
{
    [Serializable]
    public class Step
    {
        [SerializeField] private TriggersConfig triggers;
        [SerializeField] private List<SpawnerConfig> spawners;

        [JsonIgnore] public TriggersConfig TriggersConfig => triggers;
        [JsonIgnore] public List<SpawnerConfig> SpawnerConfigs => spawners;
        public void BindTriggers(TriggersConfig variant) => triggers = variant;

        public void AddSpawner(SpawnerConfig variant)
        {
            spawners ??= new List<SpawnerConfig>();
            if (spawners.Contains(variant)) return;
            spawners.Add(variant);
        }

        public void BindLocation(LocationInfo locationInfo) => triggers.BindLocation(locationInfo);

        public void Initialize()
        {
            triggers.Initialize();
            if (spawners is null || spawners.Count == 0) return;
            foreach (var spawner in spawners)
            {
                spawner.Initialize();
            }
        }

        public void Update(Transform player)
        {
            triggers.Update(player.position);
            foreach (var spawner in spawners)
            {
                spawner.Update(player.position);
            }
        }

        public void Draw()
        {
            triggers.Draw();
            if (spawners is null || spawners.Count == 0) return;
            foreach (var spawner in spawners)
            {
                spawner.Draw();
            }
        }
    }
}