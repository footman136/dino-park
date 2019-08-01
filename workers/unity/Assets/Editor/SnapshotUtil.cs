using UnityEngine;
using Improbable;
using Assets.Gamelogic.Core;
using Improbable.Gdk.Core;
using Snapshot = Improbable.Gdk.Core.Snapshot;
using Assets.Gamelogic.Utils;
using DinoPark;

namespace Editor
{
    public static class SnapshotUtil
    {
        private static readonly System.Random rand = new System.Random();

        public static void AddPlayerSpawner(Snapshot snapshot)
        {
            var entity = EntityTemplateFactory.CreatePlayerTemplate(snapshot);
            snapshot.AddEntity(entity);
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
        
        public static void AddNPCsAroundHQs(Snapshot snapshot, Coordinates[] locations)
        {
            for (uint teamId = 0; teamId < locations.Length; teamId++)
            {
                SpawnNpcsAroundPosition(snapshot, locations[teamId], teamId);
            }
        }

        public static void SpawnNpcsAroundPosition(Snapshot snapshot, Coordinates position, uint team)
        {
            float totalNpcs = SimulationSettings.HQStartingTRexCount + SimulationSettings.HQStartingBrachioCount;
            float radiusFromHQ = SimulationSettings.NPCSpawnDistanceToHQ;

            for (int i = 0; i < totalNpcs; i++)
            {
                float radians = (i / totalNpcs) * 2 * Mathf.PI;
                Vector3 offset = new Vector3(Mathf.Cos(radians), 0, Mathf.Sin(radians));
                offset *= radiusFromHQ;
                Coordinates coordinates = (position.ToVector3() + offset).ToCoordinates();

                EntityTemplate entity = null;
                if (i < SimulationSettings.HQStartingBrachioCount)
                {
                    //entity = EntityTemplateFactory.CreateNPCLumberjackTemplate(coordinates, team);
                    entity = EntityTemplateFactory.CreateDinoBrachioTemplate(coordinates, team);
                }
                else
                {
                    //entity = EntityTemplateFactory.CreateNPCWizardTemplate(coordinates, team);
                    entity = EntityTemplateFactory.CreateDinoTRexTemplate(coordinates, team);
                }

                if (entity != null)
                {
                    snapshot.AddEntity(entity);
                }
            }
            Debug.Log("Snapshot Dinosaurs generated ! count<"+totalNpcs+">");
        }
    }
}