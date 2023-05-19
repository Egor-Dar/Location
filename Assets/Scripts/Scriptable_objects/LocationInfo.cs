using System;
using System.Collections.Generic;
using Better.SceneManagement.Runtime;
using Location;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Serialization;

namespace Scriptable_objects
{
    [Serializable]
    public class LocationInfo
    {
        [JsonProperty] [SerializeField] private string name = "none";
        [JsonProperty] [SerializeField] private string sprite = "none";
        [JsonProperty] [SerializeField] private string sceneURL = "none";
        [SerializeField] private List<Step> steps;
        [SerializeField] private int moneyInLocation;
        [SerializeField] private bool locked;
        [SerializeField] private int currentProgress;


        [JsonIgnore]
        public Step CurrentStep => currentProgress >= steps.Count
            ? steps[^1]
            : steps[currentProgress];

        [JsonIgnore] public string Name => name;
        [JsonIgnore] public List<Step> Steps => steps;

        public void AddProgressValue(int value)
        {
            currentProgress = Mathf.Clamp(currentProgress += value, 0, steps.Count);
            PlayerPrefs.SetInt(name, currentProgress);
        }

        public void Draw()
        {
            if(steps is null) return;
            foreach (var step in steps)
            {
                step.Draw();
            }
        }
    }
}