using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrefsSystem : ISaveSystem
{
    public bool HasKey(string key)
    {
        return PlayerPrefs.HasKey(key);
    }

    public bool Save<T>(string key, T data)
    {
        if (data is int value)
        {
            PlayerPrefs.SetInt(key, value);
            return true;
        }
        return false;
    }

    public bool TryGet<T>(string key, out T data)
    {
        if (typeof(T) == typeof(int))
        {
            int value = PlayerPrefs.GetInt(key);
            data = (T)(object)value;
            return true;
        }

        data = default(T);
        return false;
    }
}
