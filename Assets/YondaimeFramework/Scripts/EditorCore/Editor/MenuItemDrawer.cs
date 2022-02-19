using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YondaimeFramework;
using UnityEditor;
using System.Linq;

namespace YondaimeFramework.EditorHandles
{

    public class MenuBarItems : Editor {

      

        [MenuItem(("YondaimeFramework/Create/RootLibrary"))]
        public static void CreateRootLibrary() 
        {
            const string LOAD_PATH = "Assets/YondaimeFramework/Templates/RootLibrary.prefab";
            const string LIB_ASSET_NAME = "RootLibrary";
            Generate<RootLibrary>(LOAD_PATH, LIB_ASSET_NAME);
        }

        [MenuItem(("YondaimeFramework/Create/SceneLibrary"))]
        public static void CreateSceneLibrary() 
        {
            const string LOAD_PATH = "Assets/YondaimeFramework/Templates/SceneLibrary.prefab";
            const string LIB_ASSET_NAME = "SceneLibrary";

            Generate<SceneLibrary>(LOAD_PATH, LIB_ASSET_NAME);
        }

        [MenuItem(("YondaimeFramework/Create/CentralIdContainer"))]
        public static void CreateIdContainer()
        {
            const string ASSET_PATH = ComponentIdDrawer.ASSET_PATH;
            ValidateFolderHierarchy();
            CreateSOIfNotPresent(ASSET_PATH);



            static void ValidateFolderHierarchy()
            {
                //split asset path
                string assetPath = ASSET_PATH;
                List<string> assets = assetPath.Split("/".ToCharArray()).ToList();
                if (assetPath.Contains(".asset"))
                {
                    assets.RemoveAt(assets.Count - 1);
                }

                //create folder paths based on folder names
                string cache = string.Empty;
                List<string> folderHierarchyPaths = new List<string>();
                for (int i = 0; i < assets.Count; i++)
                {
                    string conditionalForwardSlash = (i == 0) ? string.Empty : "/";
                    cache += $"{conditionalForwardSlash}{assets[i]}";
                    folderHierarchyPaths.Add(cache);
                }

                //create folders
                for (int i = 0; i < folderHierarchyPaths.Count; i++)
                {
                    if (!AssetDatabase.IsValidFolder(folderHierarchyPaths[i]))
                    {
                        AssetDatabase.CreateFolder(folderHierarchyPaths[i - 1], assets[i]);
                    }
                }

                AssetDatabase.Refresh();
                AssetDatabase.SaveAssets();

            }
            static void CreateSOIfNotPresent(string ASSET_PATH)
            {
                EditorCentalIdsDataSO dataSo = AssetDatabase.LoadAssetAtPath<EditorCentalIdsDataSO>(ASSET_PATH);
                if (dataSo == null)
                {
                    dataSo = ScriptableObject.CreateInstance<EditorCentalIdsDataSO>();
                    AssetDatabase.CreateAsset(dataSo, ASSET_PATH);
                    return;
                }
            }

        }

        private static void Generate<T>(string loadPath, string prefabName) where T : CustomBehaviour
        {
            T prefab = AssetDatabase.LoadAssetAtPath<T>(loadPath);
            Instantiate(prefab).gameObject.name = prefabName;
        }


       

    }
}
