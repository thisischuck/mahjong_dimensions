using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject dicePrefab;
    public GameObject cubeHolder;
    public int cubeSize = 4;
    public float scale = 0.1f;
    public Queue<GameObject> clickedObjects;

    public GameObject[,,] gameCube;

    List<int> typeList;
    int chosen = -1;

    // Start is called before the first frame update
    void Start()
    {
        clickedObjects = new Queue<GameObject>();
        gameCube = new GameObject[cubeSize, cubeSize, cubeSize];
        StartSetup();
    }

    // Update is called once per frame
    void Update()
    {
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

    void StartSetup()
    {
        int totalSize = cubeSize * cubeSize * cubeSize;
        GenerateList();
        for (int i = 0; i < cubeSize; i++)
            for (int j = 0; j < cubeSize; j++)
                for (int k = 0; k < cubeSize; k++)
                {
                    var obj = Instantiate(
                        dicePrefab,
                        transform.position + new Vector3(scale * (i - cubeSize / 2) + scale / 2, scale * j, scale * (k - cubeSize / 2) + scale / 2),
                        transform.rotation,
                        this.cubeHolder.transform
                    );
                    obj.transform.localScale = new Vector3(scale, scale, scale);
                    var objManager = obj.GetComponent<DiceManager>();
                    objManager.ClickedAction += DiceClicked;
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
                Debug.Log("Matched");
                clickedObjects.Clear();
                Destroy(temp.gameObject);
                Destroy(obj.gameObject);
            }
            else
            {
                clickedObjects.Enqueue(obj);
                if (clickedObjects.Count > 2)
                {
                    clickedObjects.Dequeue();
                    tempManager.Removed();
                }
            }
        }
    }
}
