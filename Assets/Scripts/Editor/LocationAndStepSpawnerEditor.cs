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
            GUILayout.Label("Serialize");
            if(GUILayout.Button("Serialize to Json")) _locationAndStepSpawner.SerializeToJson();
            GUILayout.Space(2f);
            GUILayout.Label("Deserialize");
            if(GUILayout.Button("Deserialize all Json")) _locationAndStepSpawner.DeserializeAllJson();
            if(GUILayout.Button("Deserialize current Json")) _locationAndStepSpawner.DeserializeCurrentLocationAndStep();
            GUILayout.Label("Location");
            if(GUILayout.Button("Load Bundle location")) _locationAndStepSpawner.Load();
            if(GUILayout.Button("Instantiate location")) AssetBundleEditorUtil.FixShadersForEditor(_locationAndStepSpawner.InstantiateLocation());
            GUILayout.Label("Folder");
            if(GUILayout.Button("Delete saves")) _locationAndStepSpawner.DeleteSerializeFolders();
        }
    }
}