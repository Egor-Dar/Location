using System.Collections.Generic;
using Better.SceneManagement.Runtime;
using UnityEngine;

namespace Scriptable_objects
{
    [CreateAssetMenu(fileName = "NewLevel")]
    public class LocationInfo : ScriptableObject
    {
        [SerializeField] private Sprite sprite;
        [SerializeField] private string name;
        [SerializeField] private string progress;
        [SerializeField] private List<Sprite> enemies;
        [SerializeField] private int moneyInLocation;
        [SerializeField] private SceneLoaderAsset scene;
        [SerializeField] private bool locked;
        [SerializeField] private int _currentProgress;
        private const int _maxProgress = 6;

        public int CurrentProgress => _currentProgress;

        public LocationInfoData GetCopy() =>
            new(sprite, name,
                progress = PlayerPrefs.HasKey(name)
                    ? $"{_currentProgress = PlayerPrefs.GetInt(name)}/{_maxProgress}"
                    : $"{_currentProgress = 0}/{_maxProgress}", enemies, moneyInLocation, scene, locked);

        public void AddProgressValue(int value)
        {
            _currentProgress = Mathf.Clamp(_currentProgress += value, 0, _maxProgress);
            PlayerPrefs.SetInt(name, _currentProgress);
            progress = $"{_currentProgress}/{_maxProgress}";
        }
    }
}