using UnityEngine;

[System.Serializable]
public class Layers
{
    public int[][][] tiles;
}

[System.Serializable]
public class MapData
{
    public string name;
    public Layers[] layers;
}

public class JsonReader : MonoBehaviour
{
    public TextAsset jsonFile; // Drag and drop your JSON file here in the Unity Editor

    void Start()
    {
        if (jsonFile != null)
        {
            LoadMapFromJSON(jsonFile.text);
        }
        else
        {
            Debug.LogError("No JSON file assigned!");
        }
    }

    void LoadMapFromJSON(string json)
    {
        MapData mapData = JsonUtility.FromJson<MapData>(json);

        Debug.Log("Map Name: " + mapData.name);

        foreach (Layers layer in mapData.layers)
        {
            Debug.Log("Layer Tiles:");

            foreach (int[][] row in layer.tiles)
            {
                foreach (int[] tile in row)
                {
                    Debug.Log("[" + tile[0] + "," + tile[1] + "," + tile[2] + "]");
                }
            }
        }
    }
}