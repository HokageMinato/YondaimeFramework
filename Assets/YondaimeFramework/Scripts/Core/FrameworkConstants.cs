using System;
using UnityEngine;

namespace YondaimeFramework
{
    [Serializable]
    public struct SceneId
    {
        public string id;
    }

    public static class FrameworkConstants 
    {
        public const bool IsDebug = true;

    }

    public class SceneIDs
    {
        public const string Scene1 = "Scene1";
        public const string Scene2 = "Scene2";
    }

    public static class FrameworkLogger {
        public static void Log(object logData) {
            Debug.Log($"<FWork> {logData}");
        }
    }
}
