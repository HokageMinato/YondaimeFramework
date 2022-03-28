using System;
using UnityEngine;

namespace YondaimeFramework
{
    public class PersistantVariable<T> 
    {
        private readonly string _key = string.Empty;

        public PersistantVariable(string key)
        {
            _key = key;
        }

        public T Value
        {
            get
            {
                Type t = typeof(T);
                if (IsInt(t))
                    return (T)(object)PlayerPrefs.GetInt(_key, default);

                if (IsFloat(t))
                    return (T)(object)PlayerPrefs.GetFloat(_key, default);

                if (IsString(t))
                    return (T)(object)PlayerPrefs.GetString(_key, default);

                return JsonUtility.FromJson<T>(PlayerPrefs.GetString(_key, default));
            }

            set
            {
                Type t = typeof(T);
                if (IsInt(t))
                {
                    PlayerPrefs.SetInt(_key, (int)(object)value);
                    return;
                }

                if (IsFloat(t))
                {
                    PlayerPrefs.SetFloat(_key, (float)(object)value);
                    return;
                }

                if (IsString(t))
                {
                    PlayerPrefs.SetString(_key, (string)(object)value);
                    return;
                }

                PlayerPrefs.SetString(_key, JsonUtility.ToJson(value));
            }
        }


        public bool IsInt(Type t)
        {
            return t == typeof(int);
        }
        public bool IsFloat(Type t)
        {
            return t == typeof(float);
        }
        public bool IsString(Type t)
        {
            return t == typeof(string);
        }

    }
}