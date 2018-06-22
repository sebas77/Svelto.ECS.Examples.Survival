using UnityEngine;

[ExecuteInEditMode]
public class PrefabSerializer : MonoBehaviour
{
	public GameObject[] prefabs;
	
	static bool serializedOnce;

	void Awake()
	{
		Init();	
	}
	
	public void SerializeData()
	{
		serializedOnce = true;
		
		var json = JsonHelper.arrayToJson(prefabs);

		Utility.Console.Log(json);

		System.IO.File.WriteAllText("prefabs.json", json);
	}

	public void Init()
	{
		if (serializedOnce == false)
		{
			SerializeData();
		}
	}
}
