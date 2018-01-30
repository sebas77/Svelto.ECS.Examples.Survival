using System.IO;
using Svelto.ECS.Example.Survive.Others;
using UnityEngine;

[ExecuteInEditMode]
public class SpawningData : MonoBehaviour
{
    static private bool serializedOnce;

    void Awake()
    {
        if (serializedOnce == false)
        {
            SerializeData();
        }
    }
    public void SerializeData()
    {
        serializedOnce = true;
        var data = GetComponents<EnemySpawnDataSource>();
        JSonEnemySpawnData[] spawningdata = new JSonEnemySpawnData[data.Length];

        for (int i = 0; i < data.Length; i++)
            spawningdata[i] = new JSonEnemySpawnData(data[i].spawnData);

        var json = JsonHelper.arrayToJson(spawningdata);

        Utility.Console.Log(json);

        File.WriteAllText(Application.persistentDataPath+ "/EnemySpawningData.json", json);
    }
}