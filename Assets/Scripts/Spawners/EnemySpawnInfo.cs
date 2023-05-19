using System;
using System.Collections.Generic;
using Better.Attributes.Runtime.Gizmo;
using Newtonsoft.Json;
using UnityEngine;

namespace Spawners
{
    [Serializable]
    public class EnemySpawnInfo
    {
        [JsonProperty] [SerializeField] private EnemyData data;
        [JsonProperty] [SerializeField] private List<Vector3Ref> point;
        [JsonIgnore] public EnemyData Data => data;
        [JsonIgnore] public List<Vector3Ref> Positions => point;
        public void SetPosition(List<Vector3Ref> pos) => point = pos;

        public EnemySpawnInfo(EnemyData enemyData, List<Vector3Ref> point)
        {
            data = enemyData;
            this.point = point;
        }
    }

    [Serializable]
    public class Vector3Ref
    {
       [JsonProperty] private float X => Value.x;
       [JsonProperty] private float Y => Value.y;
       [JsonProperty] private float Z => Value.z;

        [Gizmo] [JsonIgnore] public Vector3 Value;
    }
}