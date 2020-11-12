using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameSceneController : MonoBehaviour
{
    [Header("Game")]
    public Player player;

    [Header("UI")]
    public Text instructionText;
    public Text timeText;
    public Text endGameText;

    private bool endedLevel;
    private float gameTimer;
    

    // Start is called before the first frame update
    void Start()
    {
        endGameText.gameObject.SetActive(false);
        player.onCollectOrb = OnCollectOrb;
    }

    // Update is called once per frame
    void Update()
    {
        // update the game timer
        if(endedLevel == false){
            gameTimer += Time.deltaTime;
            timeText.text = "Time: " + Mathf.RoundToInt(gameTimer);
        }
        
    }

    private void OnCollectOrb()
    {
        endedLevel = true;

        // show the end game message
        instructionText.gameObject.SetActive(false);
        timeText.gameObject.SetActive(false);
        endGameText.gameObject.SetActive(true);
        endGameText.text = "Well done! \nYou Time: " + Mathf.FloorToInt(gameTimer);
    }
}
