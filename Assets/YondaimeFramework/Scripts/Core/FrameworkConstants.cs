using System;
using UnityEngine;

namespace YondaimeFramework
{
    [Serializable]
    public struct SystemId
    {
        public string id;
    }

    public static class FrameworkConstants 
    {
        public const bool IsDebug = true;

        public class IdConstants
        {
            public const string TokenEvent = "TokenEvent";
            public const string RestaurantEvent = "TokenEvent";
            public const string SomeEvent = "SomeEvent";
        }

    }

    public static class FrameworkLogger
    {
        public static void Log(object logData)
        {
            Debug.Log($"<FWork> {logData}");
        }
    }
}
