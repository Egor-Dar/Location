using UnityEngine;

namespace Location
{
    public class LoadAssetBundles : MonoBehaviour
    {

        AssetBundle loadedAssetBundle;
        public string path;
       // public string pathMaterial;
        public string assetName;
        public Object prefabLoaded;

        public void Start()
        {
            LoadAssetBundle(path);
            InstantiateObjectFromBundle(assetName);
        }

        void LoadAssetBundle(string bundleUrl)
        {
            loadedAssetBundle = AssetBundle.LoadFromFile(bundleUrl);

        }

       public void InstantiateObjectFromBundle(string assetName)
        {
            prefabLoaded = loadedAssetBundle.LoadAsset(assetName);
            Instantiate(prefabLoaded);
        }

       public void Instantiate() => Instantiate(prefabLoaded);

    }
}