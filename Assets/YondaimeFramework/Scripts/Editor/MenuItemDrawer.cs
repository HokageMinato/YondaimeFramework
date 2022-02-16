using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YondaimeFramework;
using UnityEditor;

namespace YondaimeFramework
{

    public class MenuBarItems : Editor {

        [MenuItem(("YFramework/Create/RootLibrary"))]
        public static void CreateRootLibrary() 
        {
            const string LOAD_PATH = "Assets/YondaimeFramework/Templates/RootLibrary.prefab";
            const string LIB_ASSET_NAME = "RootLibrary";
            Generate<RootLibrary>(LOAD_PATH, LIB_ASSET_NAME);
        }

        [MenuItem(("YFramework/Create/SceneLibrary"))]
        public static void CreateSceneLibrary() 
        {
            const string LOAD_PATH = "Assets/YondaimeFramework/Templates/SceneLibrary.prefab";
            const string LIB_ASSET_NAME = "SceneLibrary";

            Generate<SceneLibrary>(LOAD_PATH, LIB_ASSET_NAME);
        }


        private static void Generate<T>(string loadPath,string prefabName) where T : CustomBehaviour
        {
            T prefab = AssetDatabase.LoadAssetAtPath<T>(loadPath);
            Instantiate(prefab).gameObject.name = prefabName;
        }

    }
}
