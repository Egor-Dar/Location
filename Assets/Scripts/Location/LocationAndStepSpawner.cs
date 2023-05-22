using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MapSystem.Placeholders.Step;
using Newtonsoft.Json;
using Spawners;
using UnityEngine;
using LocationInfo = Scriptable_objects.LocationInfo;
using static System.IO.Directory;
using File = System.IO.File;

namespace Location
{
    public class LocationAndStepSpawner : MonoBehaviour
    {
        [SerializeField] private List<LocationInfo> exportLocationInfo;
        [SerializeField] private List<LocationInfo> importLocationInfo;
        [SerializeField] private Transform player;
        [SerializeField] private int currentLocationIndex;
        [SerializeField] private int currentStepIndex;
        [SerializeField] private bool lookAllGizmosInCurrentLocAndStep;
        [SerializeField] private bool lookAtOnlyImportData;
        private Step _currentStep;

        private const string START_FOLDER = "Locations/";
        private const string PERMANENT_FILE_NAME = "Info.json";
        private const string BOUND_POSITION_NAME = "Position.json";
        private const string BOUND_SIZE_NAME = "Size.json";
        private const string ENEMY_POSITION_NAME = "EnemyPosition.json";

        private void Awake()
        {
            var location = importLocationInfo[currentLocationIndex];
            _currentStep = location.Steps[currentStepIndex];
            _currentStep.BindLocation(location);
            _currentStep.Initialize();
            StartCoroutine(LoadAndInstantiateLocation());
        }

        private IEnumerator LoadAndInstantiateLocation()
        {
            var load = StartCoroutine(importLocationInfo[currentLocationIndex].LoadLocation());
            yield return load;
            InstantiateLocation();
        }

        public void Load()
        {
            StartCoroutine(importLocationInfo[currentLocationIndex].LoadLocation());
        }

        public GameObject InstantiateLocation()
        {
            return Instantiate(importLocationInfo[currentLocationIndex].Location);
        }

        private void FixedUpdate()
        {
            _currentStep.Update(player);
        }

        private void OnDrawGizmos()
        {
            switch (lookAtOnlyImportData)
            {
                case false when
                    lookAllGizmosInCurrentLocAndStep &&
                    exportLocationInfo.Count > 0 &&
                    exportLocationInfo?[currentLocationIndex] != null &&
                    exportLocationInfo[currentLocationIndex].Steps[currentStepIndex] is not null:
                    exportLocationInfo[currentLocationIndex].Steps[currentStepIndex].Draw();
                    break;
                case true when
                    lookAllGizmosInCurrentLocAndStep &&
                    importLocationInfo.Count > 0 &&
                    importLocationInfo?[currentLocationIndex] != null &&
                    importLocationInfo[currentLocationIndex].Steps[currentStepIndex] is not null:
                    importLocationInfo[currentLocationIndex].Steps[currentStepIndex].Draw();
                    break;
            }
        }

        #region Serialize

        public void SerializeToJson()
        {
            foreach (var info in exportLocationInfo)
            {
                var locationPath = $"{START_FOLDER}{info.Name}/";
                Generator(locationPath + PERMANENT_FILE_NAME, info);

                for (var i = 0; i < info.Steps.Count; i++)
                {
                    var step = info.Steps[i];
                    var indexStep = i;
                    indexStep++;
                    var stepPath = $"{locationPath}Step {indexStep}";
                    SerializeTrigger(stepPath, step);
                    SerializeSpawn(stepPath, step);
                }
            }

            Debug.Log("Convert to Json completed");
        }

        private void SerializeTrigger(string stepPath, Step step)
        {
            var triggerPath = $"{stepPath}/Triggers";
            SerializeBound($"{triggerPath}/CameraBoss", step.TriggersConfig.CameraBoss);
            SerializeBound($"{triggerPath}/Finish", step.TriggersConfig.Finish);
            for (var i = 0; i < step.TriggersConfig.Abilities.Count; i++)
            {
                var indexAbility = i;
                indexAbility++;
                var ability = step.TriggersConfig.Abilities[i];
                var abilityPath = $"{triggerPath}/Abilities/{indexAbility}";
                SerializeBound(abilityPath, ability);
            }
        }

        private void SerializeSpawn(string stepPath, Step step)
        {
            for (var j = 0; j < step.SpawnerConfigs.Count; j++)
            {
                var spawner = step.SpawnerConfigs[j];
                var indexSpawner = j;
                indexSpawner++;

                var spawnPath = $"{stepPath}/Spawners/{indexSpawner}/";
                SerializeBound($"{spawnPath}/Trigger/", spawner.Trigger);
                for (var k = 0; k < spawner.Enemies.Count; k++)
                {
                    var enemy = spawner.Enemies[k];
                    var indexEnemy = k;
                    indexEnemy++;
                    var enemyPath = $"{spawnPath}/Enemies/{indexEnemy}";
                    Generator($"{enemyPath}/{PERMANENT_FILE_NAME}", enemy);

                    for (var i = 0; i < enemy.Positions.Count; i++)
                    {
                        var indexPosition = i;
                        indexPosition++;
                        var position = enemy.Positions[i];
                        var positionPath = $"{enemyPath}/Positions/{indexPosition}";
                        SerializeVector3(positionPath, position, ENEMY_POSITION_NAME);
                    }
                }
            }
        }

        private void SerializeBound(string path, Trigger trigger)
        {
            var boundPath = $"{path}/Bound";
            SerializeVector3(boundPath, trigger.Bound.PositionJson, BOUND_POSITION_NAME);
            SerializeVector3(boundPath, trigger.Bound.SizeJson, BOUND_SIZE_NAME);
        }

        private void SerializeVector3(string path, Vector3Ref vector3, string namingVector)
        {
            var vectorPath = $"{path}/Vector/{namingVector}";
            Generator(vectorPath, vector3);
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

        #endregion


        #region Deserialize

        public void DeserializeAllJson()
        {
            var directroriesLocations = GetDirectories(START_FOLDER).ToList();
            foreach (var locations in directroriesLocations)
            {
                Debug.Log(locations);
                var info = $"{locations}/{PERMANENT_FILE_NAME}";
                var infoInJson = File.ReadAllText(info);
                var variant = JsonConvert.DeserializeObject<LocationInfo>(infoInJson);
                var steps = GetDirectories(locations).ToList();
                foreach (var step in steps)
                {
                    var stepVariant = new Step();
                    DeserializeSpawners(step, ref stepVariant);
                    DeserializeTriggers(step, ref stepVariant);
                    variant.AddStep(stepVariant);
                }

                importLocationInfo.Add(variant);
            }
        }

        public void DeserializeCurrentLocationAndStep()
        {
            var locations = GetDirectories(START_FOLDER).ToList();
            var infoLocation = File.ReadAllText($"{locations[currentLocationIndex]}/{PERMANENT_FILE_NAME}");
            var variant = JsonConvert.DeserializeObject<LocationInfo>(infoLocation);
            var steps = GetDirectories(locations[currentLocationIndex]).ToList();
            var stepVariant = new Step();
            DeserializeSpawners(steps[currentStepIndex], ref stepVariant);
            DeserializeSpawners(steps[currentStepIndex], ref stepVariant);
            variant.AddStep(stepVariant);
            importLocationInfo = new List<LocationInfo>();
            importLocationInfo.Add(variant);
        }

        private void DeserializeTriggers(string step, ref Step stepVariant)
        {
            var triggersPath = $"{step}/Triggers";
            var triggerVariant = new TriggersConfig();

            var cameraBoss = new Trigger();
            DeserializeBound($"{triggersPath}/CameraBoss", ref cameraBoss);
            triggerVariant.BindCameraBoss(cameraBoss);


            var finish = new Trigger();
            DeserializeBound($"{triggersPath}/Finish", ref finish);
            triggerVariant.BindFinish(finish);

            var abilitiesPath = $"{triggersPath}/Abilities";
            var abilitiesInfo = GetDirectories(abilitiesPath);
            foreach (var ability in abilitiesInfo)
            {
                var abilityVariant = new Trigger();
                DeserializeBound(ability, ref abilityVariant);
                triggerVariant.AddAbility(abilityVariant);
            }

            stepVariant.BindTriggers(triggerVariant);
        }

        private void DeserializeSpawners(string step, ref Step stepVariant)
        {
            var spawnerPath = $"{step}/Spawners";
            var spawners = GetDirectories(spawnerPath);
            foreach (var spawner in spawners)
            {
                var triggerPath = $"{spawner}/Trigger";
                var triggerInstance = new Trigger();
                DeserializeBound(triggerPath, ref triggerInstance);
                var spawnerVariant = new SpawnerConfig();
                spawnerVariant.BindTrigger(triggerInstance);
                var enemiesPath = $"{spawner}/Enemies";
                var enemies = new List<string>();
                try
                {
                    enemies = GetDirectories(enemiesPath).ToList();
                }
                catch (DirectoryNotFoundException exception)
                {
                    stepVariant.AddSpawner(spawnerVariant);
                    return;
                }

                foreach (var enemy in enemies)
                {
                    var infoEnemy = File.ReadAllText($"{enemy}/{PERMANENT_FILE_NAME}");
                    var enemySpawnInfoInstance = JsonConvert.DeserializeObject<EnemySpawnInfo>(infoEnemy);
                    DeserializeEnemyPosition(enemy, ref enemySpawnInfoInstance);
                    spawnerVariant.AddEnemy(enemySpawnInfoInstance);
                }

                stepVariant.AddSpawner(spawnerVariant);
            }
        }

        private void DeserializeBound(string path, ref Trigger trigger)
        {
            var boundPath = $"{path}/Bound";
            var boundVariant = new Trigger.BoundRef();
            var vectorSizePath = $"{boundPath}/Vector/{BOUND_SIZE_NAME}";
            var vectorPositionPath = $"{boundPath}/Vector/{BOUND_POSITION_NAME}";
            var sizeString = File.ReadAllText(vectorSizePath);
            var positionString = File.ReadAllText(vectorPositionPath);
            var positionInstance = JsonConvert.DeserializeObject<Vector3Ref>(positionString);
            var sizeInstance = JsonConvert.DeserializeObject<Vector3Ref>(sizeString);
            boundVariant.PositionJson = positionInstance;
            boundVariant.SizeJson = sizeInstance;
            trigger.BindBound(boundVariant);
        }

        private void DeserializeEnemyPosition(string path, ref EnemySpawnInfo enemySpawnInfo)
        {
            var enemyPositionPath = $"{path}/Positions";
            var enemies = GetDirectories(enemyPositionPath).ToList();
            foreach (var enemy in enemies)
            {
                var enemyPositionString = File.ReadAllText($"{enemy}/Vector/{ENEMY_POSITION_NAME}");
                var positionInstance = JsonConvert.DeserializeObject<Vector3Ref>(enemyPositionString);
                enemySpawnInfo.SetPosition(positionInstance);
            }
        }

        #endregion

        public void DeleteSerializeFolders()
        {
            try
            {
                Delete(START_FOLDER, true);
                Debug.Log("Delete completed");
            }
            catch (DirectoryNotFoundException e)
            {
                Debug.Log("Directory not found");
            }
        }
    }
}