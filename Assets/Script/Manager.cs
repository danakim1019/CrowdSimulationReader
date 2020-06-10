using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public enum FileBrowserType
{
    File,
    Directory
}



public class Manager : MonoBehaviour
{

    public Camera mainCam;              //main camera
    private float camYPos;
    public Transform inputFieldTrns;

    public GameObject peopleObj;

    private string[] dataPath;

    // Start is called before the first frame update
    void Start()
    {
        camYPos = mainCam.transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        string inputFieldTxt = inputFieldTrns.GetComponent<InputField>().text;
        float floatInput;
        float.TryParse(inputFieldTxt, out floatInput);
        if (floatInput != camYPos)
        {
            camYPos = floatInput;
            mainCam.transform.position = new Vector3(mainCam.transform.position.x,camYPos, mainCam.transform.position.z);
        }
    }
}
