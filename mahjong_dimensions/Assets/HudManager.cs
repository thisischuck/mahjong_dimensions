using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HudManager : MonoBehaviour
{
    public GameManager gameManager;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI TimerText;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        scoreText.text = gameManager.Score.ToString();
        TimerText.text = $"{(int)gameManager.Timer / 60}:{(int)gameManager.Timer % 60}";
    }
}
