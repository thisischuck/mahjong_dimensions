using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject dicePrefab;
    public int size = 6;
    public Queue<GameObject> clickedObjects;

    public GameObject[,,] gameCube;

    // Start is called before the first frame update
    void Start()
    {
        clickedObjects = new Queue<GameObject>();
        gameCube = new GameObject[size, size, size];
        StartSetup();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void StartSetup()
    {
        for (int i = 0; i < size; i++)
            for (int j = 0; j < size; j++)
                for (int k = 0; k < size; k++)
                {
                    var obj = Instantiate(
                        dicePrefab,
                        transform.position + new Vector3(0.1f * i, 0.1f * j, 0.1f * k),
                        transform.rotation,
                        this.gameObject.transform
                    );
                    var objManager = obj.GetComponent<DiceManager>();
                    objManager.ClickedAction += DiceClicked;
                    objManager.cubePosition = new Vector3(i, j, k);
                    gameCube[i, j, k] = obj;
                }
    }

    int CheckNeighbors(bool horizontal, Vector3 pos)
    {
        int n = 0;
        if (horizontal)
        {
            if (!(pos.x - 1 < 0 || pos.x + 1 >= size))
            {
                n += gameCube[(int)(pos.x + 1), (int)pos.y, (int)pos.z] ? 1 : 0;
                n += gameCube[(int)(pos.x - 1), (int)pos.y, (int)pos.z] ? 1 : 0;
            }
        }
        else
        {
            if (!(pos.z - 1 < 0 || pos.z + 1 >= size))
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
        Vector3 pos = obj.GetComponent<DiceManager>().cubePosition;

        if (CheckNeighbors(true, pos) > 1 || CheckNeighbors(false, pos) > 1)
        {
            obj.GetComponent<DiceManager>().Invalid();
            return;
        }

        if (clickedObjects.Count == 0)
        {
            clickedObjects.Enqueue(obj);
            return;
        }

        var o = clickedObjects.Peek();
        if (clickedObjects.Contains(obj))
        {
            if (o == obj) //is it the first one?
            {
                o.GetComponent<DiceManager>().Removed();
                clickedObjects.Dequeue();
            }
            else
            {
                obj.GetComponent<DiceManager>().Removed();
                clickedObjects.Clear();
                clickedObjects.Enqueue(o);
            }

        }
        else
        {
            clickedObjects.Enqueue(obj);
            if (clickedObjects.Count > 2)
            {
                clickedObjects.Dequeue();
                o.GetComponent<DiceManager>().Removed();
            }
        }
    }
}
