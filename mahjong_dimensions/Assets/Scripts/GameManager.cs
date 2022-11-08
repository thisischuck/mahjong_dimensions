using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
public class GameManager : MonoBehaviour
{
    public float Timer = 300;//5 minutes
    [HideInInspector]
    public float timerInternal = 0;
    public int Score = 0;
    [HideInInspector]
    public int scoreToAdd = 0;
    public GameObject dicePrefab;
    public GameObject cubeHolder;
    public int cubeSize = 4;
    public float scale = 0.1f;
    public Queue<GameObject> clickedObjects;
    public GameObject hudCanvas;
    public GameObject mainCanvas;
    public int multiplier = 1;

    public GameObject[,,] gameCube;

    List<int> typeList;
    int chosen = -1;
    bool started = false;
    bool paused = false;


    public float ratio = 1f;
    Vector3 initialMouse;
    Vector3 mousePosition;

    public UnityAction PauseAction;
    public UnityAction MatchedAction;

    bool matched = false;
    float timeSinceLastMatch = 4;

    // Start is called before the first frame update
    void Start()
    {
        clickedObjects = new Queue<GameObject>();
        gameCube = new GameObject[cubeSize, cubeSize, cubeSize];
        timerInternal = Timer;
        // StartSetup();
    }

    public void PauseGame()
    {
        PauseAction.Invoke();
        paused = !paused;
    }

    // Update is called once per frame
    void Update()
    {
        if (!started)
            return;

        if (!paused)
        {
            mousePosition = Input.mousePosition;
            if (Input.GetMouseButtonDown(0))
            {
                initialMouse = mousePosition;
            }
            else if (Input.GetMouseButton(0))
            {
                float difference = (mousePosition - initialMouse).magnitude;

                if (mousePosition.x > initialMouse.x)
                    cubeHolder.transform.Rotate(Vector3.up, -difference * ratio * Time.deltaTime, Space.World);
                else
                    cubeHolder.transform.Rotate(Vector3.up, difference * ratio * Time.deltaTime, Space.World);

            }
            timerInternal -= Time.deltaTime;

            timeSinceLastMatch += Time.deltaTime;

            if (cubeHolder.transform.childCount == 0 || timerInternal < 0)
            {
                GameEnd(cubeHolder.transform.childCount == 0);
            }
        }
    }

    void GenerateList()
    {
        typeList = new List<int>();
        for (int i = 0; i <= 5; i++)
        {
            typeList.Add(i + 1);
        }
    }

    int ChooseType(bool isFinal)
    {
        int i = -1;
        int x = -1;
        if (isFinal)
        {
            if (chosen == -1)
            {
                i = Random.Range(0, typeList.Count);
                chosen = i;
                x = typeList[i];
            }
            else
            {
                x = typeList[chosen];
                chosen = -1;
            }
        }
        else
        {
            i = Random.Range(0, typeList.Count);
            x = typeList[i];
            typeList.RemoveAt(i);
        }

        if (typeList.Count == 0)
            GenerateList();

        return x;
    }

    public void StartSetup()
    {
        timerInternal = Timer;
        started = true;
        int totalSize = cubeSize * cubeSize * cubeSize;
        GenerateList();
        for (int i = 0; i < cubeSize; i++)
            for (int j = 0; j < cubeSize; j++)
                for (int k = 0; k < cubeSize; k++)
                {
                    var obj = Instantiate(
                        dicePrefab,
                        transform.position + new Vector3(scale * (i - cubeSize / 2) + scale / 2, scale * j, scale * (k - cubeSize / 2) + scale / 2),
                        Quaternion.identity,
                        this.cubeHolder.transform
                    );
                    obj.transform.localScale = new Vector3(scale, scale, scale);
                    var objManager = obj.GetComponent<DiceManager>();
                    objManager.ClickedAction += DiceClicked;
                    PauseAction += objManager.Pause;
                    objManager.cubePosition = new Vector3(i, j, k);
                    objManager.SetType(ChooseType(totalSize <= 4));
                    gameCube[i, j, k] = obj;
                    totalSize--;
                }
    }

    int CheckNeighbors(bool horizontal, Vector3 pos)
    {
        int n = 0;
        if (horizontal)
        {
            if (!(pos.x - 1 < 0 || pos.x + 1 >= cubeSize))
            {
                n += gameCube[(int)(pos.x + 1), (int)pos.y, (int)pos.z] ? 1 : 0;
                n += gameCube[(int)(pos.x - 1), (int)pos.y, (int)pos.z] ? 1 : 0;
            }
        }
        else
        {
            if (!(pos.z - 1 < 0 || pos.z + 1 >= cubeSize))
            {
                n += gameCube[(int)(pos.x), (int)pos.y, (int)pos.z + 1] ? 1 : 0;
                n += gameCube[(int)(pos.x), (int)pos.y, (int)pos.z - 1] ? 1 : 0;
            }
        }
        return n;
    }

    void GameEnd(bool playerWon)
    {
        started = false;
        clickedObjects.Clear();
        mainCanvas.SetActive(true);
        mainCanvas.transform.Find(playerWon ? "YouWin" : "GameOver").gameObject.SetActive(true);
        mainCanvas.transform.Find("Score").gameObject.SetActive(true);
        mainCanvas.transform.Find("Score").GetComponent<TextMeshProUGUI>().text = Score.ToString();
        hudCanvas.SetActive(false);
        if (!playerWon)
        {
            foreach (var obj in cubeHolder.transform.GetComponentsInChildren<DiceManager>())
                Destroy(obj.gameObject);
        }
    }

    void DiceClicked(GameObject obj)
    {
        //- Check the neighbor
        DiceManager objManager = obj.GetComponent<DiceManager>();
        Vector3 pos = objManager.cubePosition;

        if (CheckNeighbors(true, pos) > 1 || CheckNeighbors(false, pos) > 1)
        {
            objManager.Invalid();
            return;
        }

        if (clickedObjects.Count == 0)
        {
            clickedObjects.Enqueue(obj);
            return;
        }

        GameObject temp = clickedObjects.Peek();
        DiceManager tempManager = temp.GetComponent<DiceManager>();
        if (clickedObjects.Contains(obj))
        {
            if (temp == obj) //is it the first one?
            {
                tempManager.Removed();
                clickedObjects.Dequeue();
            }
            else
            {
                //since it's not the first one
                //clear the queue and remove the object. 
                //add the other one after
                objManager.Removed();
                clickedObjects.Clear();
                clickedObjects.Enqueue(temp);
            }

        }
        else
        {
            int tempType = tempManager.Type;
            int objType = objManager.Type;

            if (tempType == objType)
            {
                if (timeSinceLastMatch < 3)
                {
                    multiplier++;
                }
                else
                    multiplier = 1;

                scoreToAdd = 100 * multiplier;
                Score += scoreToAdd;
                clickedObjects.Clear();
                tempManager.Matched();
                objManager.Matched();
                timeSinceLastMatch = 0;
                MatchedAction.Invoke();
            }
            else
            {
                //tempManager.Invalid();
                clickedObjects.Clear();
                tempManager.Removed();
                clickedObjects.Enqueue(obj.gameObject);
            }
        }
    }
}
