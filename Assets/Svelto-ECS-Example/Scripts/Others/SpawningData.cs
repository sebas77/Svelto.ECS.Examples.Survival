using System;
using System.IO;
using Svelto.ECS.Example.Survive.Components.Enemies;
using Svelto.ECS.Example.Survive.Others;
using UnityEngine;

public class SpawningData : MonoBehaviour
{
    public void SerializeData()
    {
        var data = GetComponents<EnemySpawnDataSource>();
        EnemySpawnData[] spawningdata = new EnemySpawnData[data.Length];

        for (int i = 0; i < data.Length; i++)
            spawningdata[i] = data[i].spawnData;

        var json = JsonHelper.arrayToJson(spawningdata);

        Utility.Console.Log(json);

        File.WriteAllText("EnemySpawningData.json", json);
    }
}


static class JsonHelper
{
    //Usage:
    //YouObject[] objects = JsonHelper.getJsonArray<YouObject> (jsonString);
    public static T[] getJsonArray<T>(string json)
    {
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>> (json);
        return wrapper.array;
    }
    //Usage:
    //string jsonString = JsonHelper.arrayToJson<YouObject>(objects);
    public static string arrayToJson<T>(T[] array)
    {
        Wrapper<T> wrapper = new Wrapper<T> ();
        wrapper.array = array;
        return JsonUtility.ToJson (wrapper);
    }
 
    [Serializable]
    private class Wrapper<T>
    {
        public T[] array;
    }
}