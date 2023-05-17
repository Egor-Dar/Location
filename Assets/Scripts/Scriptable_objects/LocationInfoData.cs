using System.Collections.Generic;
using Better.SceneManagement.Runtime;
using UnityEngine;

namespace Scriptable_objects
{
    public class LocationInfoData
    {
        public Sprite Sprite { get; }
        public string Name { get; }
        public string Progress { get; }
        public List<Sprite> Enemies { get; }
        public int MoneyInLocation { get; }
        public SceneLoaderAsset Scene { get; }
        public bool Locked { get; }

        public LocationInfoData(Sprite sprite, string name, string progress,
            List<Sprite> enemies, int moneyInLocation, SceneLoaderAsset scene, bool locked)
        {
            Sprite = sprite;
            Name = name;
            Progress = progress;
            Enemies = enemies;
            MoneyInLocation = moneyInLocation;
            Scene = scene;
            Locked = locked;
        }
    }
}