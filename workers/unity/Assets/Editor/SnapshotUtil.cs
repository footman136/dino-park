using Assets.Gamelogic.Core;
using UnityEngine;
using Improbable;
using DinoPark; // UnityClientConnector UnityGameLogicConnector
// EntityTemplateFactory
using Snapshot = Improbable.Gdk.Core.Snapshot;
using Improbable.Gdk.Core; // EntityTemplate
using Improbable.Gdk.PlayerLifecycle; //PlayerCreator

namespace Editor
{
    public static class SnapshotUtil
    {
        private static readonly System.Random rand = new System.Random();

        public static void AddPlayerSpawner(Snapshot snapshot)
        {
            var serverAttribute = UnityGameLogicConnector.WorkerType;

            var template = new EntityTemplate();
            template.AddComponent(new Position.Snapshot(), serverAttribute);
            template.AddComponent(new Metadata.Snapshot { EntityType = "PlayerCreator" }, serverAttribute);
            template.AddComponent(new Persistence.Snapshot(), serverAttribute);
            template.AddComponent(new PlayerCreator.Snapshot(), serverAttribute);

            template.SetReadAccess(UnityClientConnector.WorkerType, UnityGameLogicConnector.WorkerType, MobileClientWorkerConnector.WorkerType);
            template.SetComponentWriteAccess(EntityAcl.ComponentId, serverAttribute);

            snapshot.AddEntity(template);
        }

        public static void AddTrees(Snapshot snapshot, Texture2D sampler, float sampleThreshold, int countAproximate, float edgeLength, float placementJitter)
        {
            var treeCountSqrt = Mathf.CeilToInt(Mathf.Sqrt(countAproximate));
            var spawnGridIntervals = edgeLength / treeCountSqrt;
            int countTree = 0;

            for (var z = 0; z < treeCountSqrt; z++)
            {
                var zProportion = z / (float)treeCountSqrt;

                for (var x = 0; x < treeCountSqrt; x++)
                {
                    var xProportion = x / (float)treeCountSqrt;
                    var xPixel = (int) (xProportion * sampler.width);
                    var zPixel = (int) (zProportion * sampler.height);
                    var sample = sampler.GetPixel(xPixel, zPixel).maxColorComponent;

                    if (sample > sampleThreshold && Random.value < sample)
                    {
                        var xJitter = Random.Range(-placementJitter, placementJitter);
                        var zJitter = Random.Range(-placementJitter, placementJitter);
                        Vector3 positionJitter = new Vector3(xJitter, 0f, zJitter);

                        Vector3f worldRoot = new Vector3f(-edgeLength/2, 0, -edgeLength/2);
                        Vector3 offsetFromWorldRoot = new Vector3(x, 0f, z) * spawnGridIntervals;
                        Vector3f spawnPosition = worldRoot + Vector3f.FromUnityVector(offsetFromWorldRoot + positionJitter);
                        AddTree(snapshot, spawnPosition);
                        countTree++;
                    }
                }
            }
            Debug.Log("Snapshot trees generated ! count<"+countTree+">");
        }

        private static void AddTree(Snapshot snapshot, Vector3f position)
        {
            var spawnRotation = (uint)Mathf.CeilToInt((float)rand.NextDouble() * 360.0f);
            var entity = EntityTemplateFactory.CreateTreeTemplate(position, spawnRotation);
            
            snapshot.AddEntity(entity);
        }

    }
}