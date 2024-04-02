using System.Collections.Generic;
using UnityEngine;

public class CubePoolingManager : MonoBehaviour
{
    public GameObject cubePrefab;
    public int poolSize = 100;

    private List<GameObject> cubePool;

    private void Start()
    {
        cubePool = new List<GameObject>();

        for (int i = 0; i < poolSize; i++)
        {
            GameObject cube = InstantiateCube(Vector3.zero, Color.white);
            cube.SetActive(false);
            cubePool.Add(cube);
        }
    }

    public GameObject GetPooledCube(Vector3 position, Color color)
    {
        foreach (GameObject cube in cubePool)
        {
            if (!cube.activeInHierarchy)
            {
                cube.transform.position = position;
                cube.GetComponent<Renderer>().material.color = color;
                cube.SetActive(true);
                return cube;
            }
        }

        // Nếu tất cả các cube đều đang sử dụng, tạo một cái mới
        GameObject newCube = InstantiateCube(position, color);
        cubePool.Add(newCube);
        return newCube;
    }

    private GameObject InstantiateCube(Vector3 position, Color color)
    {
        GameObject cube = Instantiate(cubePrefab, position, Quaternion.identity);
        cube.GetComponent<Renderer>().material.color = color;
        return cube;
    }
}