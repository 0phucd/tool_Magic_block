using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Test : MonoBehaviour,IPointerClickHandler,IPointerExitHandler
{
    public static Test instance;
    public bool isSpawn;
    private void Awake()
    {
        isSpawn = true;
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        isSpawn = false;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isSpawn = true;
    }
}
