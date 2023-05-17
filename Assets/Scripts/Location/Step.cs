using MapSystem;
using MapSystem.Placeholders.Step;
using UnityEngine;
using LocationInfo = Scriptable_objects.LocationInfo;

namespace Location
{
    public class Step : MonoBehaviour
    {
        [SerializeField] private Triggers triggers;
        [SerializeField] private MapSystem.Placeholders.Step.Spawners spawner;
        
        public void BindPlayerTransform(Transform transform)
        {
            triggers.BindTransfromPlayer(transform);
            spawner.BindPlayerTransform(transform);
        }

        public void BindLocation(LocationInfo locationInfo) => triggers.BindLocation(locationInfo);

        public void Initialize()
        {
            triggers.Initialize();
            spawner.Initialize();
        }

        private void FixedUpdate()
        {
            triggers.Update();
            spawner.Update();
        }

        private void OnDrawGizmos()
        {
            spawner.Draw();
            triggers.Draw();
        }
    }
}