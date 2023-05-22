using System;
using System.Collections;
using System.Collections.Generic;
using Location;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Serialization;
using GameObject = UnityEngine.GameObject;

namespace Scriptable_objects
{
    [Serializable]
    public class LocationInfo
    {
        [JsonProperty] [SerializeField] private string name;
        [JsonProperty] [SerializeField] private string sprite;
        [JsonProperty] [SerializeField] private string prefab;
        [JsonProperty] [SerializeField] private string bundleLink;
        [SerializeField] private List<Step> steps;
        [SerializeField] private bool locked;
        [SerializeField] private int currentProgress;
        [SerializeField] private AssetBundle assetBundle;
        [SerializeField] private GameObject location;
        [JsonIgnore] public GameObject Location => location;

        [JsonIgnore]
        public Step CurrentStep => currentProgress >= steps.Count
            ? steps[^1]
            : steps[currentProgress];

        [JsonIgnore] public string Name => name;
        [JsonIgnore] public List<Step> Steps => steps;


        public void AddStep(Step variant)
        {
            steps ??= new List<Step>();
            steps.Add(variant);
        }

        public IEnumerator LoadLocation()
        {
            while (!Caching.ready)
            {
                yield return null;
            }

            var www = WWW.LoadFromCacheOrDownload(bundleLink, 0);
            yield return www;
            if (!string.IsNullOrEmpty(www.error))
            {
                Debug.Log(www.error);
                yield break;
            }

            Debug.Log("Bundle load completed");
            assetBundle = www.assetBundle;
            var locationRequest = assetBundle.LoadAssetAsync(prefab);
            yield return locationRequest;
            location = locationRequest.asset as GameObject;
        }

        public void AddProgressValue(int value)
        {
            currentProgress = Mathf.Clamp(currentProgress += value, 0, steps.Count);
            PlayerPrefs.SetInt(name, currentProgress);
        }

        public void Draw()
        {
            if (steps is null) return;
            foreach (var step in steps)
            {
                step.Draw();
            }
        }
    }
}