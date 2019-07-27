// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using System;
using Improbable.Gdk.BuildSystem;
using Improbable.Gdk.BuildSystem.Configuration;
using Improbable.Gdk.Tools;
using UnityEditor;
using UnityEngine;

namespace Improbable
{
    internal static class BuildWorkerMenu
    {
        private const string LocalMenu = "Build for local";
        private const string CloudMenu = "Build for cloud";

        private static readonly string[] AllWorkers =
        {
            "MobileClient",
            "UnityClient",
            "UnityGameLogic",
        };

        [MenuItem(EditorConfig.ParentMenu + "/" + LocalMenu + "/MobileClient", false, EditorConfig.MenuOffset + 0)]
        public static void BuildLocalMobileClient()
        {
            MenuBuildLocal(new[] { "MobileClient" });
        }

        [MenuItem(EditorConfig.ParentMenu + "/" + CloudMenu + "/MobileClient", false, EditorConfig.MenuOffset + 0)]
        public static void BuildCloudMobileClient()
        {
            MenuBuildCloud(new[] { "MobileClient" });
        }

        [MenuItem(EditorConfig.ParentMenu + "/" + LocalMenu + "/UnityClient", false, EditorConfig.MenuOffset + 1)]
        public static void BuildLocalUnityClient()
        {
            MenuBuildLocal(new[] { "UnityClient" });
        }

        [MenuItem(EditorConfig.ParentMenu + "/" + CloudMenu + "/UnityClient", false, EditorConfig.MenuOffset + 1)]
        public static void BuildCloudUnityClient()
        {
            MenuBuildCloud(new[] { "UnityClient" });
        }

        [MenuItem(EditorConfig.ParentMenu + "/" + LocalMenu + "/UnityGameLogic", false, EditorConfig.MenuOffset + 2)]
        public static void BuildLocalUnityGameLogic()
        {
            MenuBuildLocal(new[] { "UnityGameLogic" });
        }

        [MenuItem(EditorConfig.ParentMenu + "/" + CloudMenu + "/UnityGameLogic", false, EditorConfig.MenuOffset + 2)]
        public static void BuildCloudUnityGameLogic()
        {
            MenuBuildCloud(new[] { "UnityGameLogic" });
        }


        [MenuItem(EditorConfig.ParentMenu + "/" + LocalMenu + "/All workers", false, EditorConfig.MenuOffset + 3)]
        public static void BuildLocalAll()
        {
            MenuBuildLocal(AllWorkers);
        }

        [MenuItem(EditorConfig.ParentMenu + "/" + CloudMenu + "/All workers", false, EditorConfig.MenuOffset + 3)]
        public static void BuildCloudAll()
        {
            MenuBuildCloud(AllWorkers);
        }

        [MenuItem(EditorConfig.ParentMenu + "/Clean all workers", false, EditorConfig.MenuOffset + 3)]
        public static void Clean()
        {
            MenuCleanAll();
        }

        private static void MenuBuildLocal(string[] filteredWorkerTypes)
        {
            WorkerBuilder.MenuBuild(BuildEnvironment.Local, filteredWorkerTypes);
        }

        private static void MenuBuildCloud(string[] filteredWorkerTypes)
        {
            WorkerBuilder.MenuBuild(BuildEnvironment.Cloud, filteredWorkerTypes);
        }

        private static void MenuCleanAll()
        {
            WorkerBuilder.Clean();
            Debug.Log("Clean completed");
        }
    }
}
