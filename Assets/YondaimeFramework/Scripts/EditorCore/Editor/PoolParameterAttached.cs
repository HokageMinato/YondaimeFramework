using YondaimeFramework;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System;

namespace YondaimeFramework.EditorHandles
{
    [CustomEditor(typeof(SceneLibrary))]
    public class PoolParameterAttached : Editor
    {

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            SceneLibrary lib = (SceneLibrary)target;


            if (IsLibrarySetToPooled(lib) && !IsPooledLibPresent(lib))
            {
                AddPoolParameterComponent(lib);
            }
            else if (!IsLibrarySetToPooled(lib) && IsPooledLibPresent(lib))
            {
                RemovePoolParameterComponent(lib);
            }



            #region DECLARATIONS
            static bool IsLibrarySetToPooled(SceneLibrary lib)
            {
                return lib.LibType == LibraryType.Pooled;
                                
            }

            static bool IsPooledLibPresent(SceneLibrary lib) 
            { 
                return lib.gameObject.GetComponent<PoolParameters>() != null;
            }

            static void AddPoolParameterComponent(SceneLibrary lib) 
            { 
                lib.gameObject.AddComponent<PoolParameters>();
            }
            
            static void RemovePoolParameterComponent(SceneLibrary lib)
            {
                GameObject.DestroyImmediate(lib.gameObject.GetComponent<PoolParameters>());
                lib.poolParameters = null;
            }
            #endregion
        }


    }
}
