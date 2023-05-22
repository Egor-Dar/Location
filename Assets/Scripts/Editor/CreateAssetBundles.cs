using UnityEditor;

namespace Editor
{
    public class CreateAssetBundles
    {
        [MenuItem("Assets/Build AssetBundles")]
        static void BuildAllAssetBundles()
        {
            BuildPipeline.BuildAssetBundles("Assets/AssetBundle", BuildAssetBundleOptions.None, BuildTarget.Android);
        }
    }
}