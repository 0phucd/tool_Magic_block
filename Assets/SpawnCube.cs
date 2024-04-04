using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;
using TMPro;
using UnityEngine.EventSystems;
using Dummiesman;
using Siccity.GLTFUtility;
public class SpawnCube : MonoBehaviour

{

    public Button colorButtonPrefab;

// Reference to the parent transform where buttons will be instantiated
    public Transform buttonsParent;
    public GameObject colorPickerBug;
    public Transform fileNamesParent;
  
    public Button fileNameButtonPrefab; 
    public InputField hex;
    public InputField MagicBlockName;
    private Vector3 initialMousePosition;
    private bool isDragging = false;

    private Button selectedButton;
    public bool chanceColor= false;
    public bool createBlock= true;
    public bool deleteBlock= false;
    public InputField nameInputField;
    public InputField imageInputField;
    public InputField collectionInputField;
    public JArray JArrayModel;
    public JArray JArraySize;
    public List<Vector3> listPosition;
    public Dictionary<Vector3, Color> dicPositionColor;
    private string name;
    private string image;
    private string colltection;
    private int maxY, maxZ, minY, minZ;
    public List<Vector3> listSortingPosition;
    public  int[] numberZ = new []{0,1,2,3,4,5,6,7,8,9,10 } ;
    public Color[] colors = new Color[] { Color.black, Color.blue, new Color(1.0f, 0.6617616f, 0.0588235f) ,Color.cyan, Color.gray, Color.green, new Color(0.854902f, 0, 1f), Color.magenta, Color.red, Color.white, Color.yellow };
    private Color selectedColor = Color.white; // Default color

    public GameObject camPos;
    public Image cusCoLor;
    private int lastBlock=1;

    private Vector3 initialCameraPosition; 
    public List<GameObject> instantiatedCubes = new List<GameObject>();
    public TMP_Text textXYZ;
    public GameObject block;
    public bool hasDeleteOriginBlock = false;
    public Transform content;
    public Button yButtonPrefab;
    private float previousMaxY = -1f;
    public Button duplicate;
    private int selectedYValue;
    public float previousTotalYLayers = 0f;
    private Vector3 lastMousePosition;
    public GameObject cube;
    public GameObject cameraPos;
    public GameObject tick;
    public Image imageColor;
    public Color selectedLayerColor = new Color(0.16f, 0.98f, 0.58f);
    public bool _timeToFix=true;
    private bool _convertNow;
    public bool colorFix;
    public string glbFilePath; // Đường dẫn đến file GLB
    public GameObject cubePrefab; // Reference to a cube prefab
    public float voxelSize = 1.0f; // Size of each voxel
    public float xNumber;
    private Color lastValidColor = Color.white; 
    public GameObject buttonPrefab; // Prefab của button
    public Transform buttonParent; // Parent để chứa các button được tạo ra
  
    public InputField GlbInputField;
    public List<GameObject> buttonGLB;
    public class Block
    {
        public int index;
        public int indexLayer;
        public Vector3 position;
        public float red;
        public float green;
        public float blue;
        public float alpha;
    }


    public class Layer
    {
        public int index;
        public JArray JArrayBlock;
    }

    public List<Block> listBlocks;
    private void Start()
    {
     
        List<string> fileNames = GetJsonFileNames();

        // Display the file names on the UI
        DisplayFileNames(fileNames);
        
        listBlocks = new List<Block>();

       
        imageColor.material.color= Color.white;
        JArrayModel = new JArray();
        listPosition = new List<Vector3>();
        dicPositionColor = new Dictionary<Vector3, Color>();
            GlbInputField.onValueChanged.AddListener(UpdateNumber);
        
        // Create voxel representation
      
        // File path
        CreateButtons();

        // Load and parse JSON from the persistent data path with the specified file name
        if (MagicBlockName != null)
        {
            MagicBlockName.onEndEdit.AddListener(OnMagicBlockNameEndEdit);
        }
        if (nameInputField != null)
        {
            // Attach the OnEndEdit event listener to the InputField
            nameInputField.onEndEdit.AddListener(OnNameEndEdit);
        }
        if (imageInputField != null)
        {
            // Attach the OnEndEdit event listener to the InputField
            imageInputField.onEndEdit.AddListener(OnImageEndEdit);
        }
        if (collectionInputField != null)
        {
            // Attach the OnEndEdit event listener to the InputField
            collectionInputField.onEndEdit.AddListener(OnCollectionEndEdit);
        }
        initialCameraPosition = camPos.transform.position;
        textXYZ.text = $"x: {GetMaxX()}   y: {GetMaxY()}   Z: {GetMaxZ()}";
        duplicate.onClick.AddListener(OnDuplicateButtonClick);
        
   
               
            
       
    }
    void UpdateNumber(string newValue)
    {
        // Cố gắng chuyển đổi giá trị nhập vào thành số float
        if (float.TryParse(newValue, out float result))
        {
            // Nếu chuyển đổi thành công, gán giá trị vào số
            xNumber = result;
        }
    }
    public  void CreateButtons()
    {
        foreach (var button in buttonGLB)
        {
            Destroy(button);
        }
        string[] glbFiles = Directory.GetFiles(Application.persistentDataPath, "*.glb"); // Lấy danh sách các tệp GLB trong thư mục
        foreach (string filePath in glbFiles)
        {
            string fileName = Path.GetFileNameWithoutExtension(filePath); // Lấy tên của tệp mà không có phần mở rộng
            GameObject button = Instantiate(buttonPrefab, buttonParent); // Tạo button từ prefab
            button.GetComponentInChildren<TMP_Text>().text = fileName; // Đặt tên của button là tên của tệp GLB
            button.GetComponent<Button>().onClick.AddListener(() => OnFileNameButtonClick(fileName));
            button.GetComponent<Button>().onClick.AddListener(() => glbFilePath = filePath);
            
            buttonGLB.Add(button);
        }
    }
 
    public void LoadAndVoxelizeGLBModel()
    {
        ResetScene();
    
        // Import mô hình từ file GLB
        GameObject model = Importer.LoadFromFile(glbFilePath);
       
        if (model != null)
        {
            model.transform.localScale *= xNumber;
            // Gán mô hình vào một GameObject cha (nếu cần)
            model.transform.SetParent(transform);

            // Tạo biểu diễn voxel cho mô hình GLB và gán màu
            CreateVoxelRepresentation(model);
            SetJson();
            Destroy(model);
        }
        else
        {
            
            Debug.LogError("Failed to load GLB model.");
        }
    }

    void CreateVoxelRepresentation(GameObject obj)
    {
        // Assuming the obj is the parent GameObject of the GLB model

        // Get all mesh renderers in the obj and its children
        MeshRenderer[] meshRenderers = obj.GetComponentsInChildren<MeshRenderer>();

        // HashSet to keep track of used positions
        HashSet<Vector3> usedPositions = new HashSet<Vector3>();

        foreach (MeshRenderer meshRenderer in meshRenderers)
        {
            // Get the mesh filter associated with the mesh renderer
            MeshFilter meshFilter = meshRenderer.GetComponent<MeshFilter>();

            if (meshFilter != null)
            {
                // Create voxel representation for each mesh and get the material color
                CreateVoxelsForMesh(meshFilter.mesh, meshRenderer.transform.localToWorldMatrix, usedPositions, meshRenderer.sharedMaterials);
            }
        }
    }
Color GetMaterialColorAtPoint(Material material, Vector2 textureCoord)
{
    Color finalColor = Color.white;

    if (material.HasProperty("_MainTex"))
    {
        Texture mainTexture = material.GetTexture("_MainTex");
        if (mainTexture != null)
        {
            // Tính toán tọa độ UV dựa trên textureCoord và kích thước texture
            float u = textureCoord.x % 1.0f;
            float v = textureCoord.y % 1.0f;

            // Lấy màu từ texture sử dụng bilinear filtering
            finalColor = ((Texture2D)mainTexture).GetPixelBilinear(u, v);

            // Kiểm tra màu có là NaN không
            if (float.IsNaN(finalColor.r) || float.IsNaN(finalColor.g) || float.IsNaN(finalColor.b) || float.IsNaN(finalColor.a))
            {
                // Log thông báo về màu NaN và các thông tin khác nếu cần
                Debug.LogWarning("NaN color detected at texture coordinates: " + textureCoord.ToString());
            }
        }
    }

    // Thực hiện các thao tác tương tự cho các texture khác nếu cần

    return finalColor;
}
void CreateVoxelsForMesh(Mesh mesh, Matrix4x4 matrix, HashSet<Vector3> usedPositions, Material[] materials)
{
    Vector3[] vertices = mesh.vertices;
    int[] triangles = mesh.triangles;
    Vector2[] uv = mesh.uv; // Get UV coordinates from the mesh

    int materialIndex = 0; // Index to keep track of the material being processed

    for (int i = 0; i < triangles.Length; i += 3)
    {
        if (uv.Length > triangles[i + 2])
        {
            // Get the three vertices of the triangle
            Vector3 vertex1 = matrix.MultiplyPoint(vertices[triangles[i]]);
            Vector3 vertex2 = matrix.MultiplyPoint(vertices[triangles[i + 1]]);
            Vector3 vertex3 = matrix.MultiplyPoint(vertices[triangles[i + 2]]);

            // Calculate the bounding box of the triangle
            Vector3 minBounds = Vector3.Min(vertex1, Vector3.Min(vertex2, vertex3));
            Vector3 maxBounds = Vector3.Max(vertex1, Vector3.Max(vertex2, vertex3));

            // Round the bounds to voxel size
            minBounds = RoundToVoxel(minBounds);
            maxBounds = RoundToVoxel(maxBounds);

            // Iterate through the bounding box and create voxels from bottom to top
            for (float y = minBounds.y; y <= maxBounds.y; y += voxelSize)
            {
                for (float x = minBounds.x; x <= maxBounds.x; x += voxelSize)
                {
                    for (float z = minBounds.z; z <= maxBounds.z; z += voxelSize)
                    {
                        Vector3 voxelPosition = new Vector3(x, y, z);

                        // Check if the voxel position is already used
                        if (!usedPositions.Contains(voxelPosition))
                        {
                            // Instantiate cube prefab at the voxel position
                            GameObject cube = Instantiate(cubePrefab, voxelPosition, Quaternion.identity);

                            // Calculate UV coordinates based on the voxel position and bounding box size
                            Vector2 textureCoord = WorldToUV(voxelPosition, matrix, minBounds, maxBounds, uv[triangles[i]], uv[triangles[i + 1]], uv[triangles[i + 2]]);
                            
                            // Get material color at the UV coordinates
                            Color materialColor = GetMaterialColorAtPoint(materials[materialIndex], textureCoord);

                            // Set color for the cube
                            SetCubeColor(cube, materialColor);

                            // Add position to HashSet
                            usedPositions.Add(voxelPosition);
                            lastBlock++;
                            listPosition.Add(cube.transform.position);
                            dicPositionColor.Add(cube.transform.position, materialColor);
                            instantiatedCubes.Add(cube);
                        }
                    }
                }
            }
        }
        else
        {
        }

        // Move to the next material in the materials array
        materialIndex = (materialIndex + 1) % materials.Length;
    }
}


Vector2 WorldToUV(Vector3 voxelPosition, Matrix4x4 matrix, Vector3 minBounds, Vector3 maxBounds, Vector2 uv1, Vector2 uv2, Vector2 uv3)
{
    // Transform voxel position to local space
    Vector3 localPos = matrix.inverse.MultiplyPoint(voxelPosition);

    // Calculate UV coordinates based on local position and bounding box size
    float denominatorX = maxBounds.x - minBounds.x;
    float denominatorY = maxBounds.y - minBounds.y;
    float denominatorZ = maxBounds.z - minBounds.z;

    float u = (denominatorX != 0) ? Mathf.Clamp01((localPos.x - minBounds.x) / denominatorX) : 0.5f;
    float v = (denominatorY != 0) ? Mathf.Clamp01((localPos.y - minBounds.y) / denominatorY) : 0.5f;
    float w = (denominatorZ != 0) ? Mathf.Clamp01((localPos.z - minBounds.z) / denominatorZ) : 0.5f;

    // Interpolate UV coordinates using barycentric coordinates
    Vector2 interpolatedUV = uv1 * (1 - u - v) + uv2 * u + uv3 * v;

    return interpolatedUV;
}


Vector3 RoundToVoxel(Vector3 input)
{
    return new Vector3(
        Mathf.Floor(input.x / voxelSize) * voxelSize,
        Mathf.Floor(input.y / voxelSize) * voxelSize,
        Mathf.Floor(input.z / voxelSize) * voxelSize
    );
}


    void SetCubeColor(GameObject cube, Color color)
    {
        // Gán màu cho cubePrefab
        Renderer cubeRenderer = cube.GetComponent<Renderer>();
        if (cubeRenderer != null)
        {
            // Check if the color is invalid (NaN)
            if (float.IsNaN(color.r) || float.IsNaN(color.g) || float.IsNaN(color.b) || float.IsNaN(color.a))
            {
                // If the color is invalid, set the cube color to the last valid color
                cubeRenderer.material.color = lastValidColor;
            }
            else
            {
                // If the color is valid, set the cube color to the obtained color
                cubeRenderer.material.color = color;

                // Update the last valid color
                lastValidColor = color;
            }

           
        }
    }

    void Update()
    {
 
        imageColor.color = imageColor.material.color;
        cusCoLor.color = imageColor.color;
       
        if (_timeToFix == false && colorFix == false )
        {
          

           
            if (block == null)
            {
                hasDeleteOriginBlock = true;
            }

            if (block != null)
            {
                hasDeleteOriginBlock = false;
            }

            if (Input.GetMouseButtonDown(0))
            {
                isDragging = false;
                initialMousePosition = Input.mousePosition;

            }

            if (Input.GetMouseButton(0))
            {
                // Calculate the delta movement from the initial mouse position
                Vector3 deltaMousePosition = Input.mousePosition - initialMousePosition;

                // Check if the user is dragging
                if (Mathf.Abs(deltaMousePosition.x) > 10f || Mathf.Abs(deltaMousePosition.y) > 10f)
                {
                    // User is dragging, set the flag
                    isDragging = true;
                }
            }

            if (Input.GetMouseButtonUp(0) && !isDragging)
            {
                // Kiểm tra nút chuột trái
                if (createBlock)
                {
                    

                    if (Input.GetMouseButton(0))
                    {

                        // Calculate the delta movement from the initial mouse position
                        Vector3 deltaMousePosition = Input.mousePosition - initialMousePosition;

                        // Check if the user is dragging
                        if (Mathf.Abs(deltaMousePosition.x) > 10f || Mathf.Abs(deltaMousePosition.y) > 10f)
                        {
                            // User is dragging, set the flag
                            isDragging = true;
                        }
                    }

                    if (Input.GetMouseButtonUp(0))
                    {


                        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                        if (Physics.Raycast(ray, out RaycastHit hit, 500f))
                        {
                            Vector3 sideVector = new Vector3(
                                hit.transform.localScale.x * hit.normal.x,
                                hit.transform.localScale.y * hit.normal.y,
                                hit.transform.localScale.z * hit.normal.z);

                            GameObject newCube = Instantiate(hit.transform.gameObject,
                                hit.transform.position + sideVector, hit.transform.rotation);
                            camPos.transform.position = newCube.transform.position;
                            Renderer cubeRenderer = newCube.GetComponent<Renderer>();
                            if (cubeRenderer != null)
                            {
                                cubeRenderer.material.color = selectedColor;
                            }

                            lastBlock++;
                            listPosition.Add(newCube.transform.position);
                            dicPositionColor.Add(newCube.transform.position, selectedColor);
                            instantiatedCubes.Add(newCube);
                        }


                        if (lastBlock > 7)
                        {
                            SetJson();
                        }
                    }

                    lastMousePosition = Input.mousePosition;
                }




                // Kiểm tra nút chuột phải
                if (deleteBlock && lastBlock >= 2)
                {
                   

                    if (Input.GetMouseButtonUp(0))
                    {
                        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                        if (Physics.Raycast(ray, out RaycastHit hit, 500f))
                        {
                            // Destroy the clicked cube
                            Destroy(hit.transform.gameObject);

                            // Remove data from the list and dictionary
                            RemoveData(hit.transform.position);
                            lastBlock--;
                        }



                        SetJson();

                    }
                }

                if (chanceColor)
                {
                    if (Input.GetMouseButtonUp(0))
                    {

                        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                        if (Physics.Raycast(ray, out RaycastHit hit, 500f))
                        {
                            // Get the position before removing the cube
                            Vector3 removedPosition = hit.transform.position;

                            // Destroy the clicked cube
                            Destroy(hit.transform.gameObject);

                            // Remove data from the list and dictionary
                            RemoveData(removedPosition);

                            // Instantiate a new cube at the removed position
                            GameObject newCube = Instantiate(hit.transform.gameObject, removedPosition,
                                hit.transform.rotation);
                            Renderer cubeRenderer = newCube.GetComponent<Renderer>();
                            if (cubeRenderer != null)
                            {
                                cubeRenderer.material.color = selectedColor; // or set it to any other color
                            }

                            // Add the new cube's data to the list and dictionary
                            listPosition.Add(newCube.transform.position);
                            dicPositionColor.Add(newCube.transform.position, selectedColor);
                            instantiatedCubes.Add(newCube);

                        }

                        SetJson();

                    }
                }
            }
            

            if (CountDistinctYLayers() != previousTotalYLayers)
            {
                // Gọi một phương thức riêng để tạo các nút Y
                CreateYButtons();

                // Cập nhật giá trị trước đó của tổng số lớp Y
                previousTotalYLayers = CountDistinctYLayers();
            }
            
        }
    
        if (colorPickerBug.gameObject.activeSelf)
        {
            colorFix = true;
        }
        else
        {
            colorFix = false;
        }
    }
   
    List<string> GetJsonFileNames()
    {
        string[] filePaths = Directory.GetFiles(Application.persistentDataPath, "*.json");
        List<string> fileNames = new List<string>();

        foreach (string filePath in filePaths)
        {
            string fileName = Path.GetFileNameWithoutExtension(filePath);

            // Check if the file name is not empty or null before adding to the list
            if (!string.IsNullOrEmpty(fileName))
            {
                fileNames.Add(fileName);
            }
        }

        return fileNames;
    }
    void DisplayFileNames(List<string> fileNames)
    {
        foreach (Transform child in fileNamesParent)
        {
            Destroy(child.gameObject);
        }

        // Instantiate Button objects for each file name
        foreach (string fileName in fileNames)
        {
            Button fileNameButton = Instantiate(fileNameButtonPrefab, fileNamesParent);
            fileNameButton.GetComponentInChildren<TMP_Text>().text = fileName;

            // Attach an onClick listener to the button
            fileNameButton.onClick.AddListener(() => OnFileNameButtonClick(fileName));
        }
    }
    public void StartConvert()
    {
        _convertNow = true;
        ResetScene();
        if (!string.IsNullOrEmpty(MagicBlockName.text))
        {
            OnFileNameButtonClick(MagicBlockName.text);
            
        }
    }

    void OnFileNameButtonClick(string fileName)
    {
        nameInputField.text = MagicBlockName.text;
        if (MagicBlockName != null)
        {
            MagicBlockName.text = fileName;

            string filePath = Path.Combine(Application.persistentDataPath, fileName + ".json");

            if (File.Exists(filePath))
            {
                if (_convertNow)
                { 
                    StartCoroutine(DelayedResetScene(filePath, fileName));
                    _convertNow = false;
                }
            }
            else
            {
                Debug.Log($"File {fileName} không tồn tại.");
            }
        }
    }

    IEnumerator DelayedResetScene(string filePath, string fileName)
    {
        yield return null; // Wait for a frame to ensure UI updates
        ResetScene();
        ConVertJson(filePath, fileName);
    }

    public void SortingPosition()
    {
        
        listSortingPosition.Clear();
        JArrayModel.Clear();
        for (int y = (int)GetMinY(); y <= (int)GetMaxY(); y++)
        {
            JArray layerJArray = new JArray();
            
            for (int z = (int)GetMinZ(); z <= GetMaxZ(); z++)
            {
                for (int x = (int)GetMinX(); x <= GetMaxX(); x++)
                {
                    if (listPosition.Contains(new Vector3(x, y, z)))
                    {
                        JArray pointJarray = new JArray();
                        pointJarray.Add((int)(x - GetMinX()));
                        pointJarray.Add((int)(z - GetMinZ()));

                        if (dicPositionColor.ContainsKey(new Vector3(x, y, z)))
                        {
                            Color cubeColor = dicPositionColor[new Vector3(x, y, z)];
                            float[] rgbaArray = new float[] { cubeColor.r, cubeColor.g, cubeColor.b, cubeColor.a };
                            pointJarray.Add(rgbaArray);
                        }

                        listSortingPosition.Add(new Vector3(x, y, z));
                        layerJArray.Add(pointJarray);
                    }
                }

            }
           

            JArrayModel.Add(layerJArray);
        }
    }
    public float GetMinY()
    {
        float y = listPosition[0].y;
        foreach (var point in listPosition)
        {
            if (point.y < y)
            {
                y = point.y;
            }
        }
       
        return y;
    }

    public float GetMinX()
    {
        float x = listPosition[0].x;
        foreach (var point in listPosition)
{
            if (point.x < x)
            {
                x = point.x;
            }
        }
        return x;
    }

    public float GetMaxX()
    {
        float x = listPosition[0].x;
        foreach (var point in listPosition)
        {
            if (point.x > x)
            {
                x = point.x;
            }
        }
        return x;
    }

    public float GetMinZ()
    {
        float z = listPosition[0].z;
        foreach (var point in listPosition)
        {
            if (point.z < z)
            {
                z = point.z;
            }
        }
        return z;
    }
    public float GetMaxZ()
    {
        float z = listPosition[0].z;
        foreach (var point in listPosition)
        {
            if (point.z > z)
            {
                z = point.z;
            }
        }
        return z;
    }
    public float GetMaxY()
    {
        float y = listPosition[0].y;
        foreach (var point in listPosition)
        {
            if (point.y > y)
            {
                y = point.y;
            }
        }
        return y;
    }
    void SetJson()
    {
        SortingPosition();
       
        float maxX = GetMaxX();
        float maxY = GetMaxY();
        float maxZ = GetMaxZ();

        JObject sizeObject = new JObject();
        sizeObject["x"] = (int)(maxX - GetMinX() + 1); // Adding 1 to include the minimum value
        sizeObject["y"] = (int)(maxY - GetMinY() + 1);
        sizeObject["z"] = (int)(maxZ - GetMinZ() + 1);
        JArraySize = new JArray();
        JArraySize.Add(sizeObject);
        JObject jObject = new JObject();
        jObject["Name"] = name;
        jObject["Image"] = image;
        jObject["Collection"] = colltection;
        jObject["size"] = JArraySize;
        jObject["layers"] = JArrayModel;
        string data = JsonConvert.SerializeObject(jObject, Formatting.None);
        
       
        textXYZ.text = $"X: {(int)(maxX - GetMinX() + 1)}   Y: {(int)(maxY - GetMinY() + 1)}   Z: {(int)(maxZ - GetMinZ() + 1)}";
        string fileName = MagicBlockName.text  +".json"; // Use MagicBlockName as part of the file name
    string filePath = Path.Combine(Application.persistentDataPath, fileName);
        File.WriteAllBytes(filePath, Encoding.UTF8.GetBytes(data));
        Debug.Log("File path: " + filePath);
 
     
    // Ghi dữ liệu JSON vào tệp
 
    
        
    }
    public void SetSelectedColor()
    {
        selectedColor = imageColor.color;

        bool isPredefinedColor = false;

        // Check if the selected color matches any predefined color
        foreach (Color predefinedColor in colors)
        {
            if (selectedColor.Equals(predefinedColor))
            {
                isPredefinedColor = true;
                break;
            }
        }

        // Check if the selected color has the same hex code as any predefined color
        if (!isPredefinedColor)
        {
            string selectedHex = ColorUtility.ToHtmlStringRGB(selectedColor);
            foreach (Color predefinedColor in colors)
            {
                string predefinedHex = ColorUtility.ToHtmlStringRGB(predefinedColor);
                if (selectedHex.Equals(predefinedHex))
                {
                    isPredefinedColor = true;
                    break;
                }
            }
        }

        if (!isPredefinedColor)
        {
            // The selected color is not in the predefined colors, add it to the array
            List<Color> updatedColors = new List<Color>(colors);
            updatedColors = updatedColors.Where(c => c != Color.clear).ToList(); // Remove clear color
            updatedColors.Add(selectedColor);

            // Update the colors array
            colors = updatedColors.ToArray();
            CreateColorButton(colors.Length - 1, selectedColor);
        }

        if (cusCoLor != null)
        {
            cusCoLor.color = selectedColor;
        }
    }
    void CreateColorButton(int colorIndex, Color color)
    {
        // Instantiate a new button
        Button colorButton = Instantiate(colorButtonPrefab, buttonsParent);
    
        // Set the button's color
        colorButton.GetComponent<Image>().color = color;

        // Attach an onClick listener to the button, passing the color index
        colorButton.onClick.AddListener(() => SetSelectedColor(colorIndex));
    }
    void RemoveData(Vector3 position)
    {
        // Remove all occurrences of position from list
        listPosition.RemoveAll(pos => pos == position);

        // Remove all occurrences of position from dictionary
        var keysToRemove = dicPositionColor.Keys.Where(pos => pos == position).ToList();
        foreach (var key in keysToRemove)
        {
            dicPositionColor.Remove(key);
        }
    }
    private void OnNameEndEdit(string newName)
    {
        // Update the name variable with the new text from the InputField
        name = newName;
    }
    private void OnImageEndEdit(string newName)
    {
        // Update the name variable with the new text from the InputField
        image = newName;
    }
    private void OnCollectionEndEdit(string newName)
    {
        // Update the name variable with the new text from the InputField
        colltection = newName;
    }
    public void ChanceColor()
    {
        chanceColor = true;
        createBlock = false;
        deleteBlock = false;
    }
    public void CreateBlock()
    {
        chanceColor = false;
        createBlock = true;
        deleteBlock = false;
    }public void DeleteBlock()
    {
        chanceColor = false;
        createBlock = false;
        deleteBlock = true;
    }
    void DuplicateCubesToNextYLevel()
    {
        List<Vector3> duplicatedPositions = new List<Vector3>();

        // Get the maximum Y level in the current list
        float maxYLevel = GetMaxY();

        // Iterate through the list of cube positions
        foreach (Vector3 currentPosition in listPosition)
        {
            // Check if the cube is at the source Y level
            if (Mathf.Approximately(currentPosition.y, selectedYValue))
            {
                // Duplicate the cube to the next Y level
                Vector3 newPosition = new Vector3(currentPosition.x, maxYLevel + 1, currentPosition.z);
                GameObject newCube = InstantiateCube(newPosition, selectedColor);
                
                // Add the new cube's position to the duplicated positions list
                duplicatedPositions.Add(newPosition);
                instantiatedCubes.Add(newCube);
                camPos.transform.position = newCube.transform.position;
                
            }
            
        }

        // Add duplicated positions to the main list
        listPosition.AddRange(duplicatedPositions);

        // Add duplicated positions and colors to the dictionary
        foreach (Vector3 position in duplicatedPositions)
        {
            dicPositionColor.Add(position, selectedColor);
        }
       
        lastBlock += duplicatedPositions.Count;
        SetJson();
        
    }

    GameObject InstantiateCube(Vector3 position, Color color)
    {
        GameObject newCube = Instantiate(cube, position, Quaternion.identity);
        newCube.transform.position = position;

        Renderer cubeRenderer = newCube.GetComponent<Renderer>();
        if (cubeRenderer != null)
        {
            cubeRenderer.material.color = color;
        }
        return newCube;
        
    }
    public void ResetScene()
    {
        // Destroy all cubes in the scene
        foreach (GameObject cube in instantiatedCubes)
        {
            Destroy(cube);
        }
        instantiatedCubes.Clear();
        // Reset variables
        listPosition.Clear();
        dicPositionColor.Clear();
        lastBlock = 1;

        // Reset the camera position
        camPos.transform.position = initialCameraPosition;

        // Check if lastChanceColorBlock is equal to lastBlock
        if (hasDeleteOriginBlock)
        {
            Vector3 tickPosition =  Vector3.zero;
            // Create a block with red color at (0, 0, 0) and scale (1, 1, 1)
            GameObject cube1 = Instantiate(cube, tickPosition, Quaternion.identity);
          
            block = cube1;
            
        }
        camPos.transform.position = Vector3.zero;
        // Additional reset steps as needed...

        // Reset JSON data
       
    }


    GameObject GetCubeAtPosition(Vector3 position)
    {
        // Find the cube at a specific position
        Collider[] colliders = Physics.OverlapBox(position, Vector3.one * 0.5f); // Assumes cube size is 1x1x1
        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Cube"))
            {
                return collider.gameObject;
            }
        }
        return null;
    }
    void CreateYButtons()
    {
        float maxY = GetMaxY();

        // Remove existing buttons if any
        foreach (Transform child in content)
        {
            Destroy(child.gameObject);
        }

        // Create Y buttons for each layer
        for (int y = (int)GetMinY(); y <= (int)GetMaxY(); y++)
        {
            // Instantiate a new Y button
            Button yButton = Instantiate(yButtonPrefab, content);

            // Set the button's label text to the Y value
            yButton.GetComponentInChildren<TMP_Text>().text = "Y: " + y;

            // Attach an onClick listener to the button
            int yValue = y; // Create a local variable to capture the current y value
            yButton.onClick.AddListener(() => OnYButtonClick(yValue));
            yButton.GetComponent<Image>().color = Color.white; // Default color

            // Additional code to set the color of the selected layer
            if (y == selectedYValue)
            {
                yButton.GetComponent<Image>().color = selectedLayerColor;
                selectedButton = yButton; // Update the selected button reference
            }
           
        }
    }

// Function to handle button click event
    void OnYButtonClick(int yValue)
    {
        GameObject existingTick = GameObject.FindWithTag("RedTick");
        if (existingTick != null)
        {
            Destroy(existingTick);
        }

        // Store the selected Y value
        selectedYValue = yValue;

        float maxX = GetMaxX();
        float minY = GetMinY();
        float minZ = GetMinZ();
        Vector3 tickPosition = new Vector3(maxX + 1, selectedYValue, minZ);

        // Instantiate the tick at the specified position
        GameObject redTick = Instantiate(tick, tickPosition, Quaternion.identity);

        // Change the color of the tick to red
        Renderer tickRenderer = redTick.GetComponent<Renderer>();
        if (tickRenderer != null)
        {
            tickRenderer.material.color = Color.red;
        }
        redTick.AddComponent<DestroyOnClick>();

        // Destroy the red tick block after 2 seconds
        Destroy(redTick, 2f);

        // Change the color of the button only if yValue is greater than or equal to 0
        
            // Change the color of the previously selected button to white (if not null)
            if (selectedButton != null)
            {
                selectedButton.GetComponent<Image>().color = Color.white;
            }

            Button clickedButton = EventSystem.current.currentSelectedGameObject.GetComponent<Button>();
            if (clickedButton != null)
            {
                clickedButton.GetComponent<Image>().color = selectedLayerColor;
                selectedButton = clickedButton; // Update the selected button reference
            }
        

        Vector3 averagePosition = Vector3.zero;
        int cubeCount = 0;

        float currentX = camPos.transform.position.x;
        float currentZ = camPos.transform.position.z;
        camPos.transform.position = new Vector3(currentX, selectedYValue, currentZ);

        // Cập nhật vị trí và góc quay của camera
        cameraPos.transform.position = new Vector3(GetMaxX() + 1, camPos.transform.position.y, GetMinZ() + -15f);
        cameraPos.transform.rotation = Quaternion.Euler(0f, 0f, 0f); // Góc quay x về 0 độ
    }

    public void OnDuplicateButtonClick()
    {
        DuplicateCubesToNextYLevel();
        GameObject redTick = GameObject.FindWithTag("RedTick"); // Assuming you have set a tag for the red tick block
        if (redTick != null)
        {
            Destroy(redTick);
        }
    }
    
    int CountDistinctYLayers()
    {
        // Tạo một HashSet để lưu trữ các giá trị duy nhất của lớp Y
        HashSet<int> uniqueYLayers = new HashSet<int>();

        // Lặp qua danh sách vị trí và thêm các giá trị Y vào HashSet
        foreach (Vector3 position in listPosition)
        {
            uniqueYLayers.Add((int)position.y);
        }

        // Trả về số lượng giá trị duy nhất trong HashSet
        return uniqueYLayers.Count;
    }
    // Function to set the active layer based on the clicked button
    public void SetSelectedColor(int colorIndex)
    {
        if (colorIndex >= 0 && colorIndex < colors.Length)
        {
            selectedColor = colors[colorIndex];
        }
        if (cusCoLor != null)
        {
            cusCoLor.color = selectedColor;
            
          
        }
        imageColor.material.color = selectedColor;
        cusCoLor.color = imageColor.color;
        string hexColor = ColorUtility.ToHtmlStringRGB(selectedColor);

        // Set the text of the input field to the hexadecimal color value
        if (hex != null)
        {
            hex.text = hexColor;
        }
    }
    
    void ConVertJson(string filePath, string fileName)
    {
        ResetScene();
      
        string json = File.ReadAllText(filePath);
        JObject jObject = JObject.Parse(json);
        JArray jArrayLayer = (JArray)jObject["layers"];

        for (int i = 0; i < jArrayLayer.Count; i++)
        {
            Layer layer = new Layer();
            layer.index = i;
            layer.JArrayBlock = (JArray)jArrayLayer[i];

            for (int j = 0; j < layer.JArrayBlock.Count; j++)
            {
                Block block = new Block();
                block.index = j;
                block.indexLayer = i;

                JArray jArray = (JArray)layer.JArrayBlock[j];

                if (jArray.Count == 6) // Assuming the array always has 6 values for RGBA
                {
                    block.position = new Vector3(jArray[0].Value<int>(), i, jArray[1].Value<int>());
                    block.red = jArray[2].Value<float>();
                    block.green = jArray[3].Value<float>();
                    block.blue = jArray[4].Value<float>();
                    block.alpha = jArray[5].Value<float>(); 

                 
                    listBlocks.Add(block);
                    GameObject newCube = InstantiateCube(block.position, new Color(block.red, block.green, block.blue, block.alpha));

                    // Increment lastBlock count
                    lastBlock++;

                    // Add the position and color information to respective lists and dictionaries
                    listPosition.Add(newCube.transform.position);
                    dicPositionColor.Add(newCube.transform.position, newCube.GetComponent<Renderer>().material.color);

                    // Add the new cube to the list of instantiated cubes
                    instantiatedCubes.Add(newCube);

                    // Add the position and color information to respective lists and dictionaries
                   
                    
              

                    // Add the new cube to the list of instantiated cubes
                   
                    

                }
            }
        }
    }
    Color GetColorFromIndex(int colorIndex)
    {
        // Add your logic to map color index to a specific color
        // For simplicity, I'll use Unity's predefined colors
        Color[] colors = new Color[] { Color.black, Color.blue, Color.clear, Color.cyan ,Color.gray,Color.green,Color.grey,Color.magenta,Color.red,Color.white,Color.yellow};

        // Ensure colorIndex is within the valid range
        colorIndex = Mathf.Clamp(colorIndex, 0, colors.Length - 1);

        return colors[colorIndex];
    }
    
    private void OnMagicBlockNameEndEdit(string newName)
    {
        // Construct the file path using the specified file name
        string fileName = newName + ".json";
        string filePath = Path.Combine(Application.persistentDataPath, fileName);

        // Check if the file exists
        if (File.Exists(filePath))
        {
            if (_convertNow == true)
            {
                ResetScene();
                // If the file exists, load and parse JSON
                ConVertJson(filePath, fileName);
            }
        }
        else
        {
            ResetScene();
            // If the file does not exist, create a new JSON file
            SetJson();
        }
    }

    public void FixBug()
    {
        _timeToFix =! _timeToFix;
    }
  

    }
