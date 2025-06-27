using UnityEngine;
using UnityEditor;
public class PlacingRocks : EditorWindow
{
    public GameObject rockPrefab;
    public Terrain targetTerrain;
    public int rockCount = 200;
    public float minHeight = 0f;
    public float maxHeight = 200f;
    
    [MenuItem("Tools/Place Rock")]
    public static void ShowWindow()
    {
        GetWindow<PlacingRocks>("Place Rock");
    }

    void OnGUI()
    {
        rockPrefab = (GameObject)EditorGUILayout.ObjectField("Rock Prefab", rockPrefab, typeof(GameObject), false);
        targetTerrain = (Terrain)EditorGUILayout.ObjectField("Terrain", targetTerrain, typeof(Terrain), true);
        rockCount = EditorGUILayout.IntField("rock Count", rockCount);
        minHeight = EditorGUILayout.FloatField("Min Height", minHeight);
        maxHeight = EditorGUILayout.FloatField("Max Height", maxHeight);

        if (GUILayout.Button("Place Rocks"))
            PlaceRocks();
    }

    void PlaceRocks()
    {
        if (rockPrefab == null || targetTerrain == null)
        {
            Debug.LogWarning("Please assign a rock prefab and a terrain.");
            return;
        }

        TerrainData terrainData = targetTerrain.terrainData;
        Vector3 terrainPos = targetTerrain.transform.position;

        int placeCount = 0;
        int placeAttempts = 0;

        while (placeCount < rockCount && placeAttempts < rockCount * 10)
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

            GameObject rock = (GameObject)PrefabUtility.InstantiatePrefab(rockPrefab);
            rock.transform.position = pos;
            rock.transform.rotation = Quaternion.Euler(0, Random.Range(0f, 360f), 0);
            Undo.RegisterCreatedObjectUndo(rock, "Place rock");
            placeCount++;
            
        }
    }
}
