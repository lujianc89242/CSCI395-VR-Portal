using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSceneController : MonoBehaviour
{
    public Player player;
    // Start is called before the first frame update
    void Start()
    {
        player.onCollectOrb = OnCollectOrb;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollectOrb()
    {
        Debug.Log("You win!");
    }
}
