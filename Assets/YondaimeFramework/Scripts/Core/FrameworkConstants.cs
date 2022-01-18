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

        public class IdConstants
        {
            public const string UIElements = "UIElements";
        }

    }

    public static class FrameworkLogger {
        public static void Log(object logData) {
            Debug.Log($"<FWork> {logData}");
        }
    }
}
