using System.Collections;
using UnityEngine;

[System.Serializable]
public class Block
{
    public int x;
    public int z;
    public int colorId;
}

[System.Serializable]
public class Layer
{
    public int y;
    public Block[] blocks;

}
[System.Serializable]
public class ModelData
{
    public Layer[] layers;
}

public class ModelLoader : MonoBehaviour
{
    public TextAsset jsonFile;

    void Start()
    {
        if (jsonFile != null)
        {
            ModelData modelData = JsonUtility.FromJson<ModelData>(jsonFile.text);
            if (modelData != null)
            {
                
                CreateModel(modelData);
               
                
            }
            else
            {
                Debug.LogError("ModelData is null.");
            }
        }
        else
        {
            Debug.LogError("JsonFile is null.");
        }
    }
    void CreateModel(ModelData modelData)
    {
        if (modelData.layers != null)
        {
            
            foreach (Layer layer in modelData.layers)
            {
               
                Debug.Log("Layer Y: " + layer.y);
                if (layer.blocks != null)
                {
                    Debug.Log("ModelData is x.");
                    foreach (Block block in layer.blocks)
                    {
                        Debug.Log("ModelData is y.");
                        Debug.Log("Block X: " + block.x);
                        // Create a 3D object (e.g., a cube) based on block information
                        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                        cube.transform.position = new Vector3(block.x, layer.y, block.z);

                        // Set color based on colorId
                        // (You may want to implement a color mapping function)
                        cube.GetComponent<Renderer>().material.color = GetColorFromId(block.colorId);
                       
                        Debug.Log("Block X: " + block.x);
                      
                    }
                }
                else
                {
                    Debug.LogError("Layer.blocks is null for layer with y = " + layer.y);
                }
            }
            // Tiếp tục xử lý modelData.blocks
        }
        else
        {
            Debug.LogError("ModelData.layers is null.");
        }
    }

    Color GetColorFromId(int colorId)
    {
        // Implement your color mapping logic here
        // This is just a simple example, you may want to use a switch or dictionary
        switch (colorId)
        {
            case 1:
                return Color.black;
            case 2:
                return Color.blue;
            case 3:
                return Color.clear;
            case 4:
                return Color.cyan;
            case 5:
                return Color.gray;
            case 6:
                return Color.green;
            case 7:
                return Color.grey;
            case 8:
                return Color.magenta;
            case 9:
                return Color.red;
            case 10:
                return Color.white;
            
            
            default:
                return Color.yellow;
        }
    }
} 