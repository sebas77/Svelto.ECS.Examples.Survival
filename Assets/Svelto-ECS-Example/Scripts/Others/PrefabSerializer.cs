using UnityEngine;

[ExecuteInEditMode]
public class PrefabSerializer : MonoBehaviour
{
	public GameObject[] prefabs;
	
	static bool serializedOnce;

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
		
		var json = JsonHelper.arrayToJson(prefabs);

		Utility.Console.Log(json);

		System.IO.File.WriteAllText(Application.persistentDataPath+ "/prefabs.json", json);
	}
}
