using UnityEngine;
using UnityEditor;

public class PlaceOre : EditorWindow
{
    public GameObject orePrefab;
    public Terrain targetTerrain;
    public int oreCount = 200;
    public float minHeight = 0f;
    public float maxHeight = 200f;
    
    [MenuItem("Tools/Place ore")]
    public static void ShowWindow()
    {
        GetWindow<PlaceOre>("Place ore");
    }

    void OnGUI()
    {
        orePrefab = (GameObject)EditorGUILayout.ObjectField("ore Prefab", orePrefab, typeof(GameObject), false);
        targetTerrain = (Terrain)EditorGUILayout.ObjectField("Terrain", targetTerrain, typeof(Terrain), true);
        oreCount = EditorGUILayout.IntField("ore Count", oreCount);
        minHeight = EditorGUILayout.FloatField("Min Height", minHeight);
        maxHeight = EditorGUILayout.FloatField("Max Height", maxHeight);

        if (GUILayout.Button("Place ores"))
            Placeores();
    }

    void Placeores()
    {
        if (orePrefab == null || targetTerrain == null)
        {
            Debug.LogWarning("Please assign a ore prefab and a terrain.");
            return;
        }

        TerrainData terrainData = targetTerrain.terrainData;
        Vector3 terrainPos = targetTerrain.transform.position;

        int placeCount = 0;
        int placeAttempts = 0;

        while (placeCount < oreCount && placeAttempts < oreCount * 10)
        {
            placeAttempts++;
            float x = Random.Range(0f, 1f);
            float z = Random.Range(0f, 1f);
            float y = terrainData.GetInterpolatedHeight(x, z);

            if (y < minHeight || y > maxHeight)
                continue;

            Vector3 pos = new Vector3(
                x * terrainData.size.x + terrainPos.x,
                y + terrainPos.y,
                z * terrainData.size.z + terrainPos.z);

            GameObject ore = (GameObject)PrefabUtility.InstantiatePrefab(orePrefab);
            ore.transform.position = pos;
            ore.transform.rotation = Quaternion.Euler(0, Random.Range(0f, 360f), 0);
            Undo.RegisterCreatedObjectUndo(ore, "Place ore");
            placeCount++;
            
        }
    }
}
