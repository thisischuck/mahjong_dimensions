using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DiceManager : MonoBehaviour
{
    public Vector3 cubePosition;
    private Renderer mat;
    private bool isHovered = false;
    private bool clicked = false;

    Color mainColor = Color.white;
    Color clickedColor = Color.red;
    Color hoveredColor = Color.black;

    Color currentColor;

    public UnityAction<GameObject> ClickedAction;

    // Start is called before the first frame update
    void Start()
    {
        mat = this.GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isHovered)
        {
            if (Input.GetMouseButtonDown(0))
            {
                clicked = !clicked;
                if (clicked)
                    currentColor = clickedColor;
                else
                    currentColor = mainColor;
                ClickedAction.Invoke(this.gameObject);
            }
        }
    }

    public void Invalid()
    {
        mat.material.color = Color.blue;
        clicked = false;
        isHovered = false;
    }


    public void Removed()
    {
        mat.material.color = mainColor;
        clicked = false;
        isHovered = false;
    }

    void OnMouseEnter()
    {
        Debug.Log("Hit");
        mat.material.color = hoveredColor;
    }
    void OnMouseOver()
    {
        isHovered = true;
    }
    void OnMouseExit()
    {
        isHovered = false;
        if (clicked)
        {
            mat.material.color = clickedColor;
        }
        else
            mat.material.color = mainColor;
    }
}
