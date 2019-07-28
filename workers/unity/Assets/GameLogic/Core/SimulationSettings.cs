namespace Assets.Gamelogic.Core
{
    public static class SimulationSettings
    {
        // Entity Prefab Names
//        public static string PlayerPrefabName = "Player";
//        public static string NPCPrefabName = "NPCLumberjack";
//        public static string NPCWizardPrefabName = "NPCWizard";
//        public static string HQPrefabName = "HQ";
        public static string TreePrefabName = "Tree";
        
        // Resource Prefab Paths
        public static string SpellAOEIndicatorPrefabPath = "UI/SpellAOEIndicator";
        public static string EntityInfoCanvasPrefabPath = "UI/EntityInfoCanvas";
        public static string LightningEffectPrefabPath = "Particles/LightningEffect";
        public static string RainEffectPrefabPath = "Particles/RainEffect";
        public static string FireEffectPrefabPath = "Particles/Fire";
        public static string SmallFireEffectPrefabPath = "Particles/SmallFire";
        public static string DeathEffectPrefabPath = "Particles/DeathEffect";
            
        // Tree
        public static int TreeMaxHealth = 3;
        public static int HarvestReturnQuantity = 1;
//        public static int TreeBurningTimeSecs = 10;
        public static int TreeStumpRegrowthTimeSecs = 300;
        public static int BurntTreeRegrowthTimeSecs = 600;
        public static float TreeIgnitionTimeBuffer = 0.4f;
        public static float TreeExtinguishTimeBuffer = 1f;
        public static float TreeCutDownTimeBuffer = 1f;
        
        // World
            public static float SpawningWorldEdgeLength = 100; //1000;
        //public static Coordinates WorldRootPosition = new Coordinates(-SpawningWorldEdgeLength / 2d, 0d, -SpawningWorldEdgeLength / 2d);
        public static float SimulationTickInterval = 1f;
        //public static Vector3 InvalidPosition = Vector3.one * -9999;
        
        // Entity counts
            public static int AttemptedTreeCount = 2000; //20000;
        public static float TreeJitter = 9.0f;
        
        // Component Updates
        public static int TransformUpdatesToSkipBetweenSends = 5;
        public static float AngleQuantisationFactor = 2f;
        
        // Fire
        public static float FireSpreadInterval = 1f;
        public static float FireSpreadRadius = 6f;
//        public static float FireSpreadProbability = 0.5f;
        public static float DefaultFireDamageInterval = 1f;
        public static int FireDamagePerTick = 1;
        public static float OnFireMovementSpeedIncreaseFactor = 3f;
        
        // Player Controls
        public static float PlayerMovementSpeed = 5f;
//        public static float PlayerPositionUpdateMinSqrDistance = 0.001f;
        public static float PlayerPositionUpdateMaxSqrDistance = PlayerMovementSpeed * PlayerMovementSpeed * OnFireMovementSpeedIncreaseFactor * OnFireMovementSpeedIncreaseFactor * 4f;
//        public static float MaxRaycastDistance = 40f;
//        public static float PlayerMovementTargetSlowingThreshold = 0.005f;
            
            // Audio Volume
//            public static float NPCChopVolume = 0.6f;
//            public static float RainVolume = 0.8f;
//            public static float LightningStrikeVolume = 0.4f;
            public static float IgnitionVolume = 1f;
            public static float FireVolume = 1f;
            public static float ExtinguishVolume = 0.4f;
//            public static float SpellChannelVolume = 0.8f;
//            public static float FootstepVolume = 0.8f;
    }
}