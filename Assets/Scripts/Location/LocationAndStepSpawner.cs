using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;
using LocationInfo = Scriptable_objects.LocationInfo;
using static System.IO.Directory;
using File = System.IO.File;

namespace Location
{
    public class LocationAndStepSpawner : MonoBehaviour
    {
        [SerializeField] private List<LocationInfo> locationInfo;
        [SerializeField] private Transform player;
        [SerializeField] private int _currentLocationIndex;
        [SerializeField] private int _currentStepIndex;
        private Step _currentStep;

        private const string START_FOLDER = "Locations/";
        private const string PERMANENT_FILE_NAME = "Info.json";

        private void Awake()
        {
            // _currentStep = locationInfo.CurrentStep;
            //  _currentStep.BindLocation(locationInfo);
            _currentStep.Initialize();
        }

        private void FixedUpdate()
        {
            _currentStep.Update(player);
        }

        private void OnDrawGizmos()
        {
            if (locationInfo[_currentLocationIndex] != null &&
                locationInfo[_currentLocationIndex].Steps[_currentStepIndex] != null)
                locationInfo[_currentLocationIndex].Steps[_currentStepIndex].Draw();
        }

        public void ConvertToJson()
        {
            foreach (var info in locationInfo)
            {
                var locationPath = $"{START_FOLDER}{info.Name}/";
                Generator(locationPath + PERMANENT_FILE_NAME, info);

                for (var i = 0; i < info.Steps.Count; i++)
                {
                    var step = info.Steps[i];
                    var indexStep = i;
                    indexStep++;

                    var stepPath = $"{locationPath}Step {indexStep}";
                    var triggerPath = $"{stepPath}/Triggers";
                    Generator($"{triggerPath}/{PERMANENT_FILE_NAME}", step.TriggersConfig);

                    for (var j = 0; j < step.TriggersConfig.Abilities.Count; j++)
                    {
                        var ability = step.TriggersConfig.Abilities[j];
                        var indexAbility = j;
                        indexAbility++;

                        var abilityPath = $"{triggerPath}/Ability {indexAbility}";
                        Generator($"{abilityPath}/{PERMANENT_FILE_NAME}", ability);
                    }

                    for (var j = 0; j < step.SpawnerConfigs.Count; j++)
                    {
                        var spawner = step.SpawnerConfigs[j];
                        var indexSpawner = j;
                        indexSpawner++;

                        var spawnPath = $"{stepPath}/Spawner {indexSpawner}/";
                        Generator($"{spawnPath}/{PERMANENT_FILE_NAME}", info.Steps[i].SpawnerConfigs[j]);

                        for (var k = 0; k < spawner.Enemies.Count; k++)
                        {
                            var indexEnemy = k;
                            indexEnemy++;
                            var enemyPath = $"{spawnPath}/Enemy {indexEnemy}";
                            Generator($"{enemyPath}/{PERMANENT_FILE_NAME}", spawner.Enemies[k]);
                        }
                    }
                }
            }

            Debug.Log("Convert to Json completed");
        }

        private void Generator<T>(string path, T serializeObject)
        {
            var folders = new List<string>();
            var exitPath = "";
            var foldersCount = path.Count(f => f == '/');
            for (var i = 0; i < foldersCount; i++)
            {
                folders.Add(path.Split('/')[i]);
            }

            foreach (var folder in folders)
            {
                exitPath += folder + "/";
                if (!Exists(exitPath)) CreateDirectory(exitPath);
            }

            var jsonString = JsonConvert.SerializeObject(serializeObject);
            File.WriteAllText(path, jsonString);
        }
    }
}