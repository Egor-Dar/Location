using System.Collections.Generic;
using UnityEngine;

public class SetupWorldMap : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }
#if UNITY_EDITOR

    [ContextMenu("SetupAllLocations")]
    public void SetupLocs()
    {
        List<Transform> childs = new List<Transform>();

        transform.GetComponentsInChildren(childs);

        foreach (var child in childs)
        {
                /*if (child.TryGetComponent(out MapSegmentController control))
                {
                    control.GetFog();
                }
*/
                //break;
        }

        UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene());
    }

#endif

}
