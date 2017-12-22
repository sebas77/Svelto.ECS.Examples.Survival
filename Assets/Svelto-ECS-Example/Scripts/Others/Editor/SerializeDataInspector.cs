using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SpawningData))]
public class ObjectBuilderEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        SpawningData myScript = (SpawningData)target;
        if (GUILayout.Button("Export Json file"))
        {
            myScript.SerializeData();
        }
    }
}
