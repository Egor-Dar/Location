using System.Collections.Generic;
using MapSystem;
using UnityEngine;
using LocationInfo = Scriptable_objects.LocationInfo;

namespace Location
{
    public class StepSpawner : MonoBehaviour
    {
        [SerializeField] private List<Step> steps;
        [SerializeField] private LocationInfo locationInfo;
        [SerializeField] private Transform player;

        private void Awake()
        {
            var stepPrefab = locationInfo.CurrentProgress >= steps.Count
                ? steps[^1]
                : steps[locationInfo.CurrentProgress];
            var step = Instantiate(stepPrefab);
            step.BindPlayerTransform(player);
            step.BindLocation(locationInfo);
            step.Initialize();
        }
    }
}