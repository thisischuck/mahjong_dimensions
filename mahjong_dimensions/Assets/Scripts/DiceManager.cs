using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DiceManager : MonoBehaviour
{
    public int Type;
    public Vector3 cubePosition;
    public Renderer mainRenderer;
    public Renderer subRenderer;
    public ParticleSystemRenderer particleSystem;
    public AudioSource src;
    private bool isHovered = false;
    private bool clicked = false;
    private Animator anim;

    Color mainColor = Color.gray;
    Color clickedColor = Color.red;
    Color hoveredColor = Color.black;

    Color currentColor;

    public UnityAction<GameObject> ClickedAction;

    // Start is called before the first frame update
    void Start()
    {
        anim = this.GetComponent<Animator>();
        src.pitch = Random.Range(0.8f, 1.2f);
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

    public void Pause()
    {
        this.gameObject.layer = this.gameObject.layer == 2 ? 0 : 2;
    }

    public void DestroyMe()
    {
        Debug.Log("Called");
        Destroy(this.gameObject);
    }

    public void Matched()
    {
        anim.Play("Matched");
    }

    public void ChooseTypeInternal()
    {
        switch (Type)
        {
            case 1:
                mainRenderer.material.SetTextureOffset("_BaseMap", new Vector2(0, 0));
                subRenderer.material.SetTextureOffset("_BaseMap", new Vector2(0, 0));
                particleSystem.material.SetTextureOffset("_BaseMap", new Vector2(0, 0));
                break;
            case 2:
                mainRenderer.material.SetTextureOffset("_BaseMap", new Vector2(0.33f, 0));
                subRenderer.material.SetTextureOffset("_BaseMap", new Vector2(0.33f, 0));
                particleSystem.material.SetTextureOffset("_BaseMap", new Vector2(0.33f, 0));
                break;
            case 3:
                mainRenderer.material.SetTextureOffset("_BaseMap", new Vector2(0.66f, 0));
                subRenderer.material.SetTextureOffset("_BaseMap", new Vector2(0.66f, 0));
                particleSystem.material.SetTextureOffset("_BaseMap", new Vector2(0.66f, 0));
                break;
            case 4:
                mainRenderer.material.SetTextureOffset("_BaseMap", new Vector2(0, -0.33f));
                subRenderer.material.SetTextureOffset("_BaseMap", new Vector2(0, -0.33f));
                particleSystem.material.SetTextureOffset("_BaseMap", new Vector2(0, -0.33f));
                break;
            case 5:
                mainRenderer.material.SetTextureOffset("_BaseMap", new Vector2(0.33f, -0.33f));
                subRenderer.material.SetTextureOffset("_BaseMap", new Vector2(0.33f, -0.33f));
                particleSystem.material.SetTextureOffset("_BaseMap", new Vector2(0.33f, -0.33f));
                break;
            case 6:
                mainRenderer.material.SetTextureOffset("_BaseMap", new Vector2(0.66f, -0.33f));
                subRenderer.material.SetTextureOffset("_BaseMap", new Vector2(0.66f, -0.33f));
                particleSystem.material.SetTextureOffset("_BaseMap", new Vector2(0.66f, -0.33f));
                break;
        }
        mainRenderer.material.color = mainColor;
    }
    public void Invalid()
    {
        anim.Play("Invalid");
        clicked = false;
        isHovered = false;
    }
    public void Removed()
    {
        mainRenderer.material.color = mainColor;
        clicked = false;
        isHovered = false;
    }
    void OnMouseEnter()
    {
        mainRenderer.material.color = hoveredColor;
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
            mainRenderer.material.color = clickedColor;
        }
        else
            mainRenderer.material.color = mainColor;
    }
}
