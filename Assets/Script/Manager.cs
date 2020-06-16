using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Agent
{
    public float[] posXData;
    public float[] posYData;

    public Agent()
    {
    }

}

public class Manager : MonoBehaviour
{
    public Camera mainCam;              //main camera
    private float camYPos=50;
    public Transform inputFieldTrns;

    public GameObject[] peopleObj;

    //private string[] dataPath;

    public InputField dataXPath;
    public InputField dataYPath;

    private int agentNum=2;
    public Agent[] agents;
    GameObject[] agentObjs;

    private bool isLoad=false;
    public bool isStart = false;

    private int frameNum=0;
    private int maxFrame = 0;
    public Slider slider;
    public Text frameNumTxt;

    float updateDelayTimer = 0;
    public float delayTime=1.0f;

    private bool first = true;

    // Start is called before the first frame update
    void Awake()
    {
        camYPos = mainCam.transform.position.y;
        //초기화
        agents = new Agent[agentNum];
        for (int i = 0; i < agents.Length; i++)
            agents[i] = new Agent();
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
        
        //로드가 끝났을 때
        if(isLoad)
        {
            frameNumTxt.text = frameNum.ToString();
           
            if (first)
            {
                agentObjs = new GameObject[agents.Length];
                //처음일 때  객체 생성
                for(int i=0;i<agents.Length;i++)
                {
                    agentObjs[i] = Instantiate(peopleObj[i], new Vector3(agents[i].posXData[0], 0, agents[i].posYData[0]), Quaternion.identity);
                }
                first = false;
            }
            else
            {
                if(frameNum<agents[0].posXData.Length)
                {
                    if (updateDelayTimer > delayTime)
                    {
                        updateDelayTimer = 0.0f;
                        for (int i = 0; i < agentObjs.Length; i++)
                        {
                            Vector3 d = agentObjs[i].transform.position - new Vector3(agents[i].posXData[frameNum], 0, agents[i].posYData[frameNum]);
                            d.Normalize();
                            float dir = Mathf.Atan2(d.z, d.x) * (180.0f / 3.1415f);
                            agentObjs[i].transform.localRotation = Quaternion.Euler(new Vector3(0, -dir-90, 0));
                            agentObjs[i].transform.position = new Vector3(agents[i].posXData[frameNum], 0, agents[i].posYData[frameNum]);
                           
                        }
                        slider.value = frameNum;
                        frameNum++;
                    }
                    if (isStart)
                    {
                        updateDelayTimer += Time.deltaTime;
                        
                    }
                }
               
            }
        }

    }


    public void FileLoad()
    {
        if(dataXPath!=null&&dataYPath!=null)
        {
            //X,Y Pos Data
            string[] textXValue = System.IO.File.ReadAllLines(dataXPath.text);
            string[] textYValue = System.IO.File.ReadAllLines(dataYPath.text);
            //System.IO.StreamReader file =new System.IO.StreamReader(dataXPath.text);
            maxFrame = textXValue.Length / agentNum;
            slider.maxValue = maxFrame;
            for (int i = 0; i < agentNum; i++)
            {
                agents[i].posXData = new float[maxFrame];
                agents[i].posYData = new float[maxFrame];
            }

            if (textXValue.Length > 0)
            {
                for (int i = 0; i < textXValue.Length; i++)
                {
                    //Debug.Log(textValue[i]);
                    float.TryParse(textXValue[i], out agents[i% agentNum].posXData[i/ agentNum]);
                    float.TryParse(textYValue[i], out agents[i % agentNum].posYData[i / agentNum]);
                }
            }

            isLoad = true;

        }
        else
        {
            string errorStr="";
            if (dataXPath == null)
                errorStr += "(X Pos Data Path) ";
            if (dataYPath == null)
                errorStr += "(Y Pos Data Path) ";
            Debug.Log(errorStr+"is null");
        }
    }

    public void Restart()
    {
        frameNum = 0;
    }

    public void changeStart(bool Active)
    {
        this.isStart = Active;
    }

    public void setSliderValue(Slider slider)
    {
        frameNum = (int)slider.value;
    }

}
