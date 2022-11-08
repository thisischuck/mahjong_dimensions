using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HudManager : MonoBehaviour
{
    public GameManager gameManager;
    Animator animator;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI TimerText;
    public TextMeshProUGUI scoreAddText;
    // Start is called before the first frame update
    void Start()
    {
        gameManager.MatchedAction += PlayAnimation;
        animator = this.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        scoreText.text = gameManager.Score.ToString();
        TimerText.text = $"{(int)gameManager.timerInternal / 60}:{(int)gameManager.timerInternal % 60}";
    }

    void PlayAnimation()
    {
        scoreAddText.text = $"x{gameManager.multiplier} - {gameManager.scoreToAdd}";
        animator.Play("AddScore");
    }
}
