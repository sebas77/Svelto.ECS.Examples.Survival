using System.IO;
using Svelto.ECS.Example.Survive;
using UnityEngine;

[ExecuteInEditMode]
public class SpawningData : MonoBehaviour
{
    static bool serializedSpawnDataOnce;
    static bool serializedAttackDataOnce;

    void Awake()
    {
        if (serializedSpawnDataOnce == false)
            SerializeSpawnData();

        if (serializedAttackDataOnce == false)
            SerializeAttackData();            
    }
    
    public void SerializeSpawnData()
    {
        serializedSpawnDataOnce = true;
        
        var data = GetComponents<EnemyData>();
        JSonEnemySpawnData[] spawningdata = new JSonEnemySpawnData[data.Length];

        for (int i = 0; i < data.Length; i++)
            spawningdata[i] = new JSonEnemySpawnData(data[i].spawnData);

        var json = JsonHelper.arrayToJson(spawningdata);

        Utility.Console.Log(json);

        File.WriteAllText(Application.persistentDataPath + "/EnemySpawningData.json", json);
    }
    
    public void SerializeAttackData()
    {
        var data = GetComponents<EnemyData>();
        JSonEnemyAttackData[] attackData = new JSonEnemyAttackData[data.Length];
        
        serializedAttackDataOnce = true;

        for (int i = 0; i < data.Length; i++)
            attackData[i] = new JSonEnemyAttackData(data[i].attackData);

        var json = JsonHelper.arrayToJson(attackData);

        Utility.Console.Log(json);

        File.WriteAllText(Application.persistentDataPath + "/EnemyAttackData.json", json);
    }
}