using System;
using UnityEngine;

namespace YondaimeFramework
{

    //-----------------{Scene Id}---------------------------------
    [Serializable]
    public struct SceneId
    {
        public string id;
    }

  
    //-----------------{FrameworkConstants}---------------------------------
    public static class FrameworkConstants 
    {
        public const bool IsDebug = true;
    }



    //-----------------{FrameworkLogger}---------------------------------
    public static class FrameworkLogger {
        public static void Log(object logData) {
            Debug.Log($"<FWork> {logData}");
        }
    }
}
