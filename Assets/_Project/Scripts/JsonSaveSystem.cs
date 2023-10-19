using System;
using UnityEngine;

public class JsonSaveSystem : ISaveSystem
{
    public bool HasKey(string key)
    {
        return PlayerPrefs.HasKey(key);
    }

    public bool Save<T>(string key, T data)
    {
        try
        {
            string json = JsonUtility.ToJson(data);
            if (string.IsNullOrEmpty(json))
            {
                return false;
            }

            PlayerPrefs.SetString(key, json);
            return true;
        }
        catch (Exception e)
        {
            Debug.LogWarning(e.StackTrace);
            return false;
        }
    }

    public bool TryGet<T>(string key, out T data)
    {
        try
        {
            T value = JsonUtility.FromJson<T>(PlayerPrefs.GetString(key));
            if (value == null)
            {
                data = default;
                return false;
            }
            data = value;
            return true;
        }
        catch (Exception e)
        {
            Debug.LogWarning(e.StackTrace);
            data = default(T);
            return false;
        }
    }
}
