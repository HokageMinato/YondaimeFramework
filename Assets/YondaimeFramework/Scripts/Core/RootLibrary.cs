using System;
using UnityEngine;
using System.Collections.Generic;

namespace YondaimeFramework
{
    public sealed class RootLibrary : BehaviourLibrary
    {
        #region PUBLIC_VARS
        [SerializeField] bool hasNonSystemRoots;
        #endregion


        #region PRIVATE_VARS
        private Dictionary<string, SystemLibrary[]> _systemLibsLookUp = new Dictionary<string, SystemLibrary[]>();
        #endregion

        #region UNITY_CALLBACKS
        private void Awake()
        {
            InitializeFramework();
        }
        #endregion


        #region PRIVATE_FUNCTIONS

        public void InitializeFramework()
        {
            for (int i = 0; i < _childLibs.Length; i++)
            {
                _childLibs[i].InitializeLibrary();
            }

            Dictionary<string, List<SystemLibrary>> tempLibsLookUp = new Dictionary<string, List<SystemLibrary>>();
            InitializeLookUp();
            ParseTempLookUpToMasterLookup();
            LogSystemLibraries();




            void InitializeLookUp()
            {
                if (hasNonSystemRoots)
                    InitializeLookUpFiltered();
                else
                    InitializeLookUpDirectly();
            }
            void ParseTempLookUpToMasterLookup()
            {

                foreach (KeyValuePair<string, List<SystemLibrary>> item in tempLibsLookUp)
                {
                    _systemLibsLookUp.Add(item.Key, item.Value.ToArray());
                }
            }
            void InitializeLookUpDirectly()
            {
                for (int i = 0; i < _childLibs.Length; i++)
                {
                    SystemLibrary lib = (SystemLibrary)_childLibs[i];
                    string id = lib.SystemId;
                    AddToTempLookUp(lib, id);
                }
            }
            void InitializeLookUpFiltered()
            {
                for (int i = 0; i < _childLibs.Length; i++)
                {
                    SystemLibrary lib = _childLibs[i] as SystemLibrary;
                    if (lib)
                    {
                        string id = lib.SystemId;
                        AddToTempLookUp(lib, id);
                    }
                }
            }
            void AddToTempLookUp(SystemLibrary lib, string id)
            {
                if (!tempLibsLookUp.ContainsKey(id))
                {
                    tempLibsLookUp.Add(id, new List<SystemLibrary>());
                }

                tempLibsLookUp[id].Add(lib);
            }
            void LogSystemLibraries()
            {
                if(FrameworkConstants.IsDebug)
                foreach (var item in _systemLibsLookUp)
                {
                    Debug.Log($"System Library Added with key {item.Key} count {item.Value.Length}");
                }
            }
           
        }






        #endregion

        #region PUBLIC_FUNCTIONS




        public SystemLibrary GetSystemBehaviourById(string systemId)
        {
            return GetSystemBehavioursById(systemId)[0];
        }

        public List<SystemLibrary> GetSystemBehavioursById(string systemId)
        {
            List<SystemLibrary> requestedLibrary = new List<SystemLibrary>();
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


            void FilterSystemLibraries()
            {

                //For now scan only system libs and put them in child libs
                // Extend this to have all libraries upon requirement.
                List<BehaviourLibrary> systemLibraries = new List<BehaviourLibrary>(_childLibs);

                for (int i = 0; i < systemLibraries.Count;)
                {
                    if (!(systemLibraries[i] is SystemLibrary))
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
                        ((SystemLibrary)_childLibs[i]).SetRootLibrary(this);
                    }
                else
                    for (int i = 0; i < _childLibs.Length; i++)
                    {
                        SystemLibrary sysLib = _childLibs[i] as SystemLibrary;
                        sysLib?.SetRootLibrary(this);
                    }

            }
        }


        #endregion


    }
}
