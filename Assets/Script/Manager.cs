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
[System.Serializable]
public class Obstacle
{
    public Vector3 position;
    public float width, height;

    public Obstacle() { }
}


public class Manager : MonoBehaviour
{
    public Camera mainCam;              //main camera
    private float camYPos=50;
    public Transform inputFieldTrns;

    public GameObject[] peopleObj;
    public GameObject buildingObj;
    

    //private string[] dataPath;

    public InputField dataXPath;
    public InputField dataYPath;
    public InputField dataSettingPath;
    public InputField dataObstaclePath;

    private int agentNum=0;
    private int obstacleNum = 0;
    public Agent[] agents;
    GameObject[] agentObjs;

    public Obstacle[] obstacles;
    GameObject[] obstacleObjs;

    private bool isLoad=false;
    public bool isStart = false;

    private int frameNum=0;
    private int maxFrame = 0;
    public Slider slider;
    public Text frameNumTxt;

    float updateDelayTimer = 0;
    public float delayTime=1.0f;

    private bool first = true;

    float moveCamSpeed =30.0f;

    // Start is called before the first frame update
    void Awake()
    {
        camYPos = mainCam.transform.position.y;

       
    }

    // Update is called once per frame
    void Update()
    {
        moveCamPos();
        string inputFieldTxt = inputFieldTrns.GetComponent<InputField>().text;
        float floatInput;
        float.TryParse(inputFieldTxt, out floatInput);
        if (floatInput != camYPos)
        {
            camYPos = floatInput;
            mainCam.transform.position = new Vector3(mainCam.transform.position.x,camYPos, mainCam.transform.position.z);
        }
        
        //Load finished
        if(isLoad)
        {
            frameNumTxt.text = frameNum.ToString();
           
            if (first)
            {
                agentObjs = new GameObject[agentNum];
                obstacleObjs = new GameObject[obstacleNum];
                //Create Object
                for (int i=0;i<agents.Length;i++)
                {
                    agentObjs[i] = Instantiate(peopleObj[i], new Vector3(agents[i].posXData[0], 0, agents[i].posYData[0]), Quaternion.identity);
                }
                //Create Obstalce
                if(obstacleNum>0)
                {
                    for(int i=0;i<obstacleNum;i++)
                    {
                        obstacleObjs[i] = Instantiate(buildingObj, obstacles[i].position, Quaternion.identity);
                        //scale to real width,height
                        float currentSizeX = obstacleObjs[i].GetComponent<Collider>().bounds.size.x;
                        float currentSizeZ = obstacleObjs[i].GetComponent<Collider>().bounds.size.z;
                        obstacleObjs[i].transform.localScale = new Vector3(obstacles[i].width* obstacleObjs[i].transform.localScale.x/ currentSizeX, 1, obstacles[i].height * obstacleObjs[i].transform.localScale.z / currentSizeZ);
                    }
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

    private void moveCamPos()
    {
        if (Input.GetKey(KeyCode.W))
        {
            mainCam.transform.position += new Vector3(0,0,1)*Time.deltaTime*moveCamSpeed;
        }
        if (Input.GetKey(KeyCode.S))
        {
            mainCam.transform.position += new Vector3(0, 0, -1) * Time.deltaTime * moveCamSpeed;
        }
        if (Input.GetKey(KeyCode.A))
        {
            mainCam.transform.position += new Vector3(-1, 0, 0) * Time.deltaTime * moveCamSpeed;
        }
        if (Input.GetKey(KeyCode.D))
        {
            mainCam.transform.position += new Vector3(1, 0, 0) * Time.deltaTime * moveCamSpeed;
        }
        if (Input.GetKey(KeyCode.E))
        {
            mainCam.transform.position += new Vector3(0, 1, 0) * Time.deltaTime * moveCamSpeed;
        }
        if (Input.GetKey(KeyCode.Q))
        {
            mainCam.transform.position += new Vector3(0, -1, 0) * Time.deltaTime * moveCamSpeed;
        }
    }

    public void FileLoad()
    {
        if(dataXPath!=null&&dataYPath!=null&&dataSettingPath!=null)
        {
            string[] settingValue= System.IO.File.ReadAllLines(dataSettingPath.text);
            //if settingValue available
            if (settingValue.Length > 0)
            {
                int.TryParse(settingValue[0], out agentNum);
                int.TryParse(settingValue[1], out obstacleNum);
                
                if (obstacleNum>0)
                {
                    obstacles = new Obstacle[obstacleNum];
                    for (int i = 0; i < obstacles.Length; i++)
                        obstacles[i] = new Obstacle();
                    if (dataObstaclePath!=null)
                    {
                        string[] obatacleValue = System.IO.File.ReadAllLines(dataObstaclePath.text);
                        float[] verticesX = new float[4];
                        float[] verticesY = new float[4];
                        for (int i = 0; i < obstacleNum; i++)
                        {
                            //save vertices of each Obstacle
                            for(int j=0;j<4;j++)
                            {
                                for (int z = 0; z < 2; z++)
                                {
                                    if(z%2==0)
                                        float.TryParse(obatacleValue[i * 8 + j*2+z], out verticesX[j]);
                                    else
                                        float.TryParse(obatacleValue[i * 8 + j*2+z], out verticesY[j]);
                                }
                            }
                            float width = verticesX[2] - verticesX[0];
                            float height= verticesY[2] - verticesY[0];
                            Debug.Log("("+verticesX[0]+","+ verticesY[0]+") "+
                                "(" + verticesX[1] + "," + verticesY[1] + ") "+
                                "(" + verticesX[2] + "," + verticesY[2] + ") "+
                                "(" + verticesX[3] + "," + verticesY[3] + ") ");
                            obstacles[i].position = new Vector3(verticesX[0]+(width / 2),0, verticesY[0] + (height / 2));
                            obstacles[i].width = width;
                            obstacles[i].height = height;
                        }
                    }
                }
            }

            agents = new Agent[agentNum];
            for (int i = 0; i < agents.Length; i++)
            {
                agents[i] = new Agent();
            }
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
                for (int i = 0; i < maxFrame; i++)
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
