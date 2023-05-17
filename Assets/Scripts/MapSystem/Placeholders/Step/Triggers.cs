using System;
using UnityEngine;
using LocationInfo = Scriptable_objects.LocationInfo;

namespace MapSystem.Placeholders.Step
{
    [Serializable]
    public class Triggers 
    {
        [SerializeField] private TriggersConfig triggersConfig;
        private Transform _player;
        
        public void BindTransfromPlayer(Transform player) => _player = player;
        public void BindLocation(LocationInfo locationInfo) => triggersConfig.BindLocation(locationInfo);
        public void Initialize() => triggersConfig.Initialize();

        public void Update()
        {
            triggersConfig.Update(_player.position);
        }

        public void Draw()
        {
            triggersConfig.Draw();
        }
    }
}