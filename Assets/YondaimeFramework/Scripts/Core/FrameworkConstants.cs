using System;
using UnityEngine;

namespace YondaimeFramework
{

    //-----------------{Component Id}---------------------------------
    [Serializable]
    public struct ComponentId
    {
        public string id;
    }

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
