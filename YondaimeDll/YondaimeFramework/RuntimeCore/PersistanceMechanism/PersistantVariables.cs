using System;
using UnityEngine;

namespace YondaimeFramework
{
    public class PersistantVariable<T> 
    {
        private ISerializationService serializationService = new UnityJsonSerializationService();
        private readonly string _key = string.Empty;

        private Type intType = typeof(int);
        private Type floatType = typeof(float);
        private Type stringType = typeof(string);
        private T defaultValue;

        public PersistantVariable(string key)
        {
            _key = key;
        }
        
        public PersistantVariable(string key, T defaultValue = default,ISerializationService service = null)
        {
            _key = key;

            if(service!=null)
                serializationService = service;

            this.defaultValue = defaultValue;
        }
        



        public T Value
        {
            get
            {
                Type t = typeof(T);
                if (IsInt(t))
                    return (T)(object)PlayerPrefs.GetInt(_key,(int)(object)defaultValue);

                if (IsFloat(t))
                    return (T)(object)PlayerPrefs.GetFloat(_key, (float)(object)defaultValue);

                if (IsString(t))
                    return (T)(object)PlayerPrefs.GetString(_key, (string)(object)defaultValue);

                if (!HasKey(_key) && defaultValue != null)
                    return defaultValue;

                return serializationService.Deserialize<T>(PlayerPrefs.GetString(_key));
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

                PlayerPrefs.SetString(_key, serializationService.Serialize(value));
            }
        }


        public bool IsInt(Type t)
        {
            return t == intType;
        }
        public bool IsFloat(Type t)
        {
            return t == floatType;
        }
        public bool IsString(Type t)
        {
            return t == stringType;
        }

        public static bool HasKey(string key) 
        { 
            return PlayerPrefs.HasKey(key);
        }



    }

    public interface ISerializationService 
    { 
        public string Serialize<T>(T obj);
        public T Deserialize<T>(string json);
    }
}