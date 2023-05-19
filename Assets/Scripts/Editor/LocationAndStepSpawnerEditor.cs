using System;
using Location;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(LocationAndStepSpawner))]
    public class LocationAndStepSpawnerEditor: UnityEditor.Editor
    {
        private LocationAndStepSpawner _locationAndStepSpawner;

        private void OnEnable()
        {
            _locationAndStepSpawner = target as LocationAndStepSpawner;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if(GUILayout.Button("Convert to Json")) _locationAndStepSpawner.ConvertToJson();
        }
    }
}