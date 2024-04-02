using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetVelifed : MonoBehaviour
{

    public GameObject start;

    public GameObject colorPicker;

    public GameObject resetVerify;
    public GameObject infoMove;

    public bool reset = false;

    public bool info = false;
  

    public bool colorChoice=false;
    public bool startJson=true;

    public GameObject file;

    public GameObject glb;
    // Start is called before the first frame update
    void Start()
    {
     
        file.gameObject.SetActive(true);
        glb.gameObject.SetActive(false);
        resetVerify.gameObject.SetActive(false);
       
        infoMove.gameObject.SetActive(false);
        
    }

    // Update is called once per frame
    void Update()
    {
      
    }

    public void CheckChanceColor()
    {
        
    }
    public void CheckCreateBlock()
    {
    
    }
    public void CheckDeleteBlock()
    {
        
    }
    public void CheckMoveCamera()
    {
        
       
    }
    public void CheckAroundCamera()
    {
        
        
    }

    public void VerifyReset()
    {
        reset =! reset; 
        resetVerify.gameObject.SetActive(reset);
        
    }
    public void VerifyResetNo()
    {
        resetVerify.gameObject.SetActive(false);
        reset = false;

    }

    public void InfoMove()
    {
        info =!info;
        infoMove.gameObject.SetActive(info);
    }
    public void ColorPicker()
    {
       
        colorChoice =! colorChoice;
        colorPicker.gameObject.SetActive(colorChoice);
    }public void ColorPickertrue()
    {
        colorChoice = !colorChoice;
        colorPicker.gameObject.SetActive(colorChoice);
       
    }
    public void Verifyjson()
    {
        
        start.gameObject.SetActive(false);
        startJson = false;
    }
    public void VerifyJson1()
    {
        startJson =! startJson;
        start.gameObject.SetActive(startJson);
        
    }

    public void FileSet()
    {
        file.gameObject.SetActive(true);
        glb.gameObject.SetActive(false);
    }

    public void GlbSet()
    {
        file.gameObject.SetActive(false);
        glb.gameObject.SetActive(true);
    }
}
