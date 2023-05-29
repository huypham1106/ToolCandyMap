using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

using FullSerializer;


public class FileUtils : MonoBehaviour
{
    /// <summary>
    /// Loads the specified json file.
    /// </summary>
    /// <param name="serializer">The FullSerializer serializer to use.</param>
    /// <param name="path">The json file path.</param>
    /// <typeparam name="T">The type of the data to load.</typeparam>
    /// <returns>The loaded json data.</returns>
    public static T LoadJsonFile<T>(fsSerializer serializer, string path) where T : class
    {
        var textAsset = Resources.Load<TextAsset>(path);
        Assert.IsNotNull((textAsset));
        var data = fsJsonParser.Parse(textAsset.text);
        object deserialized = null;
        serializer.TryDeserialize(data, typeof(T), ref deserialized).AssertSuccessWithoutWarnings();
        return deserialized as T;
    }

    public static string LoadTextJson(fsSerializer serializer, string path)
    {
        var textAsset = Resources.Load<TextAsset>(path);
        Assert.IsNotNull((textAsset));
        return textAsset.text;
    }

    public static T ParseJsonFile<T>(fsSerializer serializer, string dataJson) where T : class
    {
        var data = fsJsonParser.Parse(dataJson);
        object deserialized = null;
        serializer.TryDeserialize(data, typeof(T), ref deserialized).AssertSuccessWithoutWarnings();
        return deserialized as T;
    }

    /// <summary>
    /// Returns true if the specified path exists and false otherwise.
    /// </summary>
    /// <param name="path">The path.</param>
    /// <returns>True if the specified path exists; false otherwise.</returns>
    public static bool FileExists(string path)
    {
        var textAsset = Resources.Load<TextAsset>(path);
        return textAsset != null;
    }
}
