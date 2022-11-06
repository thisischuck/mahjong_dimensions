using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DiceManager : MonoBehaviour
{
    public int Type;
    public Vector3 cubePosition;
    private Renderer mat;
    private bool isHovered = false;
    private bool clicked = false;

    Color mainColor = Color.gray;
    Color clickedColor = Color.red;
    Color hoveredColor = Color.black;

    Color currentColor;

    public UnityAction<GameObject> ClickedAction;

    // Start is called before the first frame update
    void Start()
    {
        mat = this.GetComponent<MeshRenderer>();
        ChooseTypeInternal();
        ColorCorrection();
    }

    void ColorCorrection()
    {
        clickedColor = mainColor + Color.white * 0.5f;
        hoveredColor = mainColor - Color.white * 0.5f;
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

    public void SetType(int type)
    {
        Type = type;
    }

    public void ChooseTypeInternal()
    {
        switch (Type)
        {
            case 1:
                mainColor = Color.blue;
                break;
            case 2:
                mainColor = Color.red;
                break;
            case 3:
                mainColor = Color.green;
                break;
            case 4:
                mainColor = Color.cyan;
                break;
            case 5:
                mainColor = Color.magenta;
                break;
            case 6:
                mainColor = Color.yellow;
                break;
        }
        mat.material.color = mainColor;
    }

    public void Invalid()
    {
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
