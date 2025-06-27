using UnityEngine;
using UnityEditor;
using Unity.VisualScripting.IonicZip;

public class PlacingTrees : EditorWindow
{
    public GameObject treePrefab;
    public Terrain targetTerrain;
    public int treeCount = 200;
    public float minHeight = 0f;
    public float maxHeight = 200f;
    
    [MenuItem("Tools/Place Tree")]
    public static void ShowWindow()
    {
        GetWindow<PlacingTrees>("Place Tree");
    }

    void OnGUI()
    {
        treePrefab = (GameObject)EditorGUILayout.ObjectField("Tree Prefab", treePrefab, typeof(GameObject), false);
        targetTerrain = (Terrain)EditorGUILayout.ObjectField("Terrain", targetTerrain, typeof(Terrain), true);
        treeCount = EditorGUILayout.IntField("Tree Count", treeCount);
        minHeight = EditorGUILayout.FloatField("Min Height", minHeight);
        maxHeight = EditorGUILayout.FloatField("Max Height", maxHeight);

        if (GUILayout.Button("Place Trees"))
            PlaceTrees();
    }

    void PlaceTrees()
    {
        if (treePrefab == null || targetTerrain == null)
        {
            Debug.LogWarning("Please assign a tree prefab and a terrain.");
            return;
        }

        TerrainData terrainData = targetTerrain.terrainData;
        Vector3 terrainPos = targetTerrain.transform.position;

        int placeCount = 0;
        int placeAttempts = 0;

        while (placeCount < treeCount && placeAttempts < treeCount * 10)
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

            GameObject tree = (GameObject)PrefabUtility.InstantiatePrefab(treePrefab);
            tree.transform.position = pos;
            tree.transform.rotation = Quaternion.Euler(0, Random.Range(0f, 360f), 0);
            Undo.RegisterCreatedObjectUndo(tree, "Place Tree");
            placeCount++;
            
        }
    }
}
