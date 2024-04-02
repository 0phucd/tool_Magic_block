using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using Newtonsoft.Json.Linq;
using UnityEngine;
using Color = UnityEngine.Color;
public class ConvertJson : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        listBlocks = new List<Block>();
        ConVertJson();
    }

    public class Block
    {
        public int index;
        public int indexLayer;
        public Vector3 position;
        public int indexColor;
    }

    public class Layer
    {
        public int index;
        public JArray JArrayBlock;
    }

    public List<Block> listBlocks;
    void ConVertJson()
    {
        string json = File.ReadAllText("Assets/Resources/Magic Blocks - sasuke.json");
        JObject jObject=JObject.Parse(json);
        JArray jArrayLayer = (JArray) jObject["layers"];
        for (int i = 0; i < jArrayLayer.Count; i++)
        {
            Layer layer = new Layer();
            layer.index = i;
            layer.JArrayBlock = (JArray) jArrayLayer[i];
            for (int j = 0; j < layer.JArrayBlock.Count; j++)
            {
                Block block = new Block() ;
                block.index = j;
                block.indexLayer = i;
                JArray jArray  = (JArray) layer.JArrayBlock[j];
                if (jArray.Count == 3)
                {
                    block.position = new Vector3(jArray[0].Value<int>(), i, jArray[1].Value<int>());
                    block.indexColor=jArray[2].Value<int>();
                    Debug.Log(block.position+" "+ block.indexColor);
                    listBlocks.Add(block);
                    GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    cube.transform.position = block.position;
                    Material cubeMaterial = new Material(Shader.Find("Standard"));
                    cubeMaterial.color = GetColorFromIndex(block.indexColor);
                    cube.GetComponent<Renderer>().material = cubeMaterial;
                }
            }
        }
    }Color GetColorFromIndex(int colorIndex)
    {
        // Add your logic to map color index to a specific color
        // For simplicity, I'll use Unity's predefined colors
        Color[] colors = new Color[] { Color.black, Color.blue, Color.clear, Color.cyan ,Color.gray,Color.green,Color.grey,Color.magenta,Color.red,Color.white,Color.yellow};

        // Ensure colorIndex is within the valid range
        colorIndex = Mathf.Clamp(colorIndex, 0, colors.Length - 1);

        return colors[colorIndex];
    }
}