using System;
using System.Collections.Generic;
using UnityEngine;
using LocationInfo = Scriptable_objects.LocationInfo;

namespace MapSystem.Placeholders.Step
{
    [Serializable]
    public sealed class TriggersConfig
    {
        [SerializeField] private List<Trigger> abilities;
        [SerializeField] private Trigger cameraBoss;
        [SerializeField] private Trigger finish;
        private LocationInfo _currentLocation;

        private bool _initialized;


        public void BindLocation(LocationInfo location) => _currentLocation = location;

        public void Update(Vector3 position)
        {
            foreach (var ability in abilities)
            {
                ability.Update(position);
                cameraBoss.Update(position);
                finish.Update(position);
            }
        }

        public void Draw()
        {
            cameraBoss.BindColor(Color.magenta);
            cameraBoss.Draw();

            finish.BindColor(Color.green);
            finish.Draw();

            if (abilities.Count == 0) return;
            foreach (var ability in abilities)
            {
                ability.BindColor(Color.cyan);
                ability.Draw();
            }
        }

        public void Initialize()
        {
            if (_initialized) return;
            _initialized = true;
            cameraBoss.Initialize(OnBoss, OnExit);
            finish.Initialize(OnFinish);
            if (abilities.Count > 0)
            {
                foreach (var ability in abilities)
                {
                    ability.Initialize(OnAbilities, OnExit);
                }
            }

            Print("Initialize completed");
        }

        private void OnAbilities()
        {
            Print("OnAbilities");
        }

        private void OnBoss()
        {
            Print("OnBoss");
        }

        private void OnFinish()
        {
            Print("OnFinish");
        }

        private void OnExit()
        {
            Print("OnExit");
        }

        private void Print(string text) => Debug.Log($"TriggerConfig: {text}");
    }
}