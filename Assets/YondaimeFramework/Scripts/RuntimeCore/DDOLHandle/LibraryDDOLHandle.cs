using System.Collections.Generic;
using UnityEngine;


namespace YondaimeFramework 
{
    public class LibraryDDOLHandle : MonoBehaviour
    {
        private static Dictionary<SceneId,ILibrary> libraries = new Dictionary<SceneId,ILibrary>();

        public static void PurgeLibraries()
        {
            foreach (KeyValuePair<SceneId, ILibrary> item in libraries)
            {
                item.Value.PurgeLibrary();
            }
        }

        private void Awake()
        {
            GenerateSingleton();
        }

        private void GenerateSingleton()
        {
            ILibrary targetLibary = GetComponent<ILibrary>();
            SceneId sceneId = targetLibary.SceneId;
            
            if (!libraries.ContainsKey(sceneId))
            {
                DontDestroyOnLoad((MonoBehaviour)targetLibary);
                libraries.Add(sceneId,targetLibary);
            }
            else
            {
                ILibrary ddolLibrary = libraries[sceneId];
                ddolLibrary.SyncLibrary(Cleanup(targetLibary.GetBehaviourSyncData()));
                DestroyImmediate(gameObject);
            }
        }

        private CustomBehaviour[] Cleanup(CustomBehaviour[] ipArray) 
        { 
            List<CustomBehaviour> behaviours = new List<CustomBehaviour>(ipArray);
            for (int i = 0; i < behaviours.Count;) 
            {
                if (behaviours[i] == null)
                {
                    behaviours.RemoveAt(i);
                    continue;
                }

                i++;
            }
            return behaviours.ToArray();    
        }

    }
}