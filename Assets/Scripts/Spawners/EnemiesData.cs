using System.Collections.Generic;
using UnityEngine;

namespace Spawners
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Enemy Data", fileName = "EnemiesData", order = -1)]

    public class EnemiesData : ScriptableObject
    {
        [SerializeField] private List<EnemyData> enemiesData;
        public List<EnemyData> GetList => new List<EnemyData>(enemiesData);
    }
}