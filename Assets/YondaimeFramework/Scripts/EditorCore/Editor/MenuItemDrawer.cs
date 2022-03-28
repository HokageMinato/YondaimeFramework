using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YondaimeFramework;
using UnityEditor;
using System.Linq;
using System;

namespace YondaimeFramework.EditorHandles
{

    public class MenuBarItems : Editor {

      

        [MenuItem(("YondaimeFramework/Create/RootLibrary"))]
        public static void CreateRootLibrary() 
        {
            const string LOAD_PATH = "Assets/YondaimeFramework/Templates/RootLibrary.prefab";
            const string LIB_ASSET_NAME = "RootLibrary";

            OverrideExistingLib<RootLibrary>(LIB_ASSET_NAME);
            Generate<RootLibrary>(LOAD_PATH, LIB_ASSET_NAME,false);
        }

        [MenuItem(("YondaimeFramework/Create/StandardLibrary"))]
        public static void CreateStandardLibrary() 
        {
            const string LOAD_PATH = "Assets/YondaimeFramework/Templates/StandardBehaviourLibrary.prefab";
            const string LIB_ASSET_NAME = "StandardBehaviourLibrary";

            OverrideExistingLib<ILibrary>(LIB_ASSET_NAME);
            Generate<StandardBehaviourLibrary>(LOAD_PATH, LIB_ASSET_NAME,true);
        }
        
        [MenuItem(("YondaimeFramework/Create/PooledLibrary"))]
        public static void CreatePooledLibrary()
        {
            const string LOAD_PATH = "Assets/YondaimeFramework/Templates/PooledBehaviourLibrary.prefab";
            const string LIB_ASSET_NAME = "PooledBehaviourLibrary";

            OverrideExistingLib<ILibrary>(LIB_ASSET_NAME);
            Generate<PooledBehaviourLibrary>(LOAD_PATH, LIB_ASSET_NAME,true);
        }

      


        [MenuItem(("YondaimeFramework/Create/CentralIdContainer"))]
        public static void CreateIdContainer()
        {
            const string ASSET_PATH = ASSET_PATHS.CentalIdContainerAssetPath;
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

        private static void Generate<T>(string loadPath, string prefabName,bool hasHandle) where T : MonoBehaviour
        {
            //T prefab = AssetDatabase.LoadAssetAtPath<T>(loadPath);
            GameObject go = new GameObject(prefabName);
           
            if(hasHandle)
                go.AddComponent<LibraryHandle>();
            
            go.AddComponent<T>();
            go.transform.SetSiblingIndex(2);
        }


        private static MonoBehaviour GetExistingLibrary<T>()
        {
            MonoBehaviour[] monos = FindObjectsOfType<MonoBehaviour>();
            for (int i = 0; i < monos.Length; i++)
            {
                if (monos[i].GetComponent<T>() != null)
                    return monos[i];
            }

            return null;
        }

        private static void OverrideExistingLib<T>(string libName)
        {
            MonoBehaviour lib = GetExistingLibrary<T>();
            if (lib != null)
            {
                bool overwrite = EditorUtility.DisplayDialog("Override Alert", $"A library already exists, Creating {libName} will override it. You Sure ?", "Create", "Cancel");
                if (overwrite)
                {
                    GameObject go = lib.gameObject;
                    DestroyImmediate(lib.GetComponent<LibraryHandle>());
                    DestroyImmediate(lib);
                    DestroyImmediate(go);
                }
            }
        }

    }
}
