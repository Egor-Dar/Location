using System;
using System.Collections.Generic;
using UnityEngine;

namespace Spawners
{
    [Serializable]
    public class EnemySpawnInfo
    {
        [SerializeField] private EnemyData data;
        [SerializeField] private List<Vector3Ref> point;
        public EnemyData Data => data;
        public List<Vector3Ref> Positions => point;
        public void SetPosition(List<Vector3Ref> pos) => point = pos;

        public EnemySpawnInfo(EnemyData enemyData, List<Vector3Ref> point)
        {
            data = enemyData;
            this.point = point;
        }
        [Serializable]
        public class Vector3Ref
        {
           public Vector3 Value;
        }
    }
}