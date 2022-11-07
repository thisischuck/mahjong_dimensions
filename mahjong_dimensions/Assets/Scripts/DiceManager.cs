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
        mat = this.GetComponentInChildren<MeshRenderer>();
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
                mat.material.SetTextureOffset("_BaseMap", new Vector2(0, 0));
                break;
            case 2:
                mat.material.SetTextureOffset("_BaseMap", new Vector2(0.33f, 0));
                break;
            case 3:
                mat.material.SetTextureOffset("_BaseMap", new Vector2(0.66f, 0));
                break;
            case 4:
                mat.material.SetTextureOffset("_BaseMap", new Vector2(0, -0.33f));
                break;
            case 5:
                mat.material.SetTextureOffset("_BaseMap", new Vector2(0.33f, -0.33f));
                break;
            case 6:
                mat.material.SetTextureOffset("_BaseMap", new Vector2(0.66f, -0.33f));
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
