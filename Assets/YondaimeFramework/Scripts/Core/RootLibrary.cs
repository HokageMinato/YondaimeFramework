using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor.SceneManagement;
#endif

namespace YondaimeFramework
{
    public sealed class RootLibrary : BehaviourLibrary
    {
        #region PUBLIC_VARS
        [SerializeField] bool hasNonSystemRoots;
        #endregion


        #region PRIVATE_VARS
        private Dictionary<string, SceneLibrary[]> _systemLibsLookUp = new Dictionary<string, SceneLibrary[]>();
        #endregion

        #region UNITY_CALLBACKS
        private void Awake()
        {
            InitializeFramework();
        }
        #endregion


        #region PRIVATE_METHODS
        
        public void InitializeFramework()
        {
            Dictionary<string, List<SceneLibrary>> tempLibsLookUp = new Dictionary<string, List<SceneLibrary>>();
            InitializeChildLibraries();
            InitializeLookUp();
            ParseTempLookUpToMasterLookup();
            InvokeFillReferences();
            LogSystemLibraries();



            void InitializeChildLibraries() 
            {
                for (int i = 0; i < _childLibs.Length; i++)
                {
                    _childLibs[i].InitializeLibrary();
                }
            }
            void InitializeLookUp()
            {
                if (hasNonSystemRoots)
                    InitializeLookUpFiltered();
                else
                    InitializeLookUpDirectly();
            }
            void ParseTempLookUpToMasterLookup()
            {
                foreach (KeyValuePair<string, List<SceneLibrary>> item in tempLibsLookUp)
                {
                    _systemLibsLookUp.Add(item.Key, item.Value.ToArray());
                }
            }
            void InitializeLookUpDirectly()
            {
                for (int i = 0; i < _childLibs.Length; i++)
                {
                    SceneLibrary lib = (SceneLibrary)_childLibs[i];
                    string id = lib.SystemId;
                    AddToTempLookUp(lib, id);
                }
            }
            void InitializeLookUpFiltered()
            {
                for (int i = 0; i < _childLibs.Length; i++)
                {
                    SceneLibrary lib = _childLibs[i] as SceneLibrary;
                    if (lib)
                    {
                        string id = lib.SystemId;
                        AddToTempLookUp(lib, id);
                    }
                }
            }
            void AddToTempLookUp(SceneLibrary lib, string id)
            {
                if (!tempLibsLookUp.ContainsKey(id))
                {
                    tempLibsLookUp.Add(id, new List<SceneLibrary>());
                }

                tempLibsLookUp[id].Add(lib);
            }
            void LogSystemLibraries()
            {
                if(FrameworkConstants.IsDebug)
                foreach (var item in _systemLibsLookUp)
                {
                    FrameworkLogger.Log($"System Library Added with key {item.Key} count {item.Value.Length}");
                }
            }
           
        }


        #endregion

        #region PUBLIC_METHODS

        public SceneLibrary GetSystemBehaviourById(string systemId)
        {
            return GetSystemBehavioursById(systemId)[0];
        }

        public List<SceneLibrary> GetSystemBehavioursById(string systemId)
        {
            List<SceneLibrary> requestedLibrary = new List<SceneLibrary>();
            requestedLibrary.AddRange(_systemLibsLookUp[systemId]);
            return requestedLibrary;
        }


        [ContextMenu("Refresh Root")]
        public override void ScanBehaviours()
        {
            base.ScanBehaviours();

            if (!hasNonSystemRoots)
                FilterSystemLibraries();
            SetRootLibraryToSystemLibs();
            SetPresentSceneDirty();

            void FilterSystemLibraries()
            {

                //For now scan only system libs and put them in child libs
                // Extend this to have all libraries upon requirement.
                List<BehaviourLibrary> systemLibraries = new List<BehaviourLibrary>(_childLibs);

                for (int i = 0; i < systemLibraries.Count;)
                {
                    if (!(systemLibraries[i] is SceneLibrary))
                        systemLibraries.RemoveAt(i);
                    else
                        i++;
                }

                _childLibs = systemLibraries.ToArray();
            }
            void SetRootLibraryToSystemLibs()
            {
                if (!hasNonSystemRoots)
                    for (int i = 0; i < _childLibs.Length; i++)
                    {
                        ((SceneLibrary)_childLibs[i]).SetRootLibrary(this);
                    }
                else
                    for (int i = 0; i < _childLibs.Length; i++)
                    {
                        SceneLibrary sysLib = _childLibs[i] as SceneLibrary;
                        sysLib?.SetRootLibrary(this);
                    }

            }
            void SetPresentSceneDirty() {
                #if UNITY_EDITOR
                EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
                #endif
            }
        }


        #endregion

        

    }
}
