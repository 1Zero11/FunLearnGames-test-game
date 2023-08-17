using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public static GameController instance;

    public Transform looseTransform;
    public Transform winTransform;
    public PlayerMove player;

    public List<Enemy> enemies;

    public GameObject restartButton;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerMove>();
        enemies = new List<Enemy>(FindObjectsOfType<Enemy>());
    }

    // Update is called once per frame
    void Update()
    {
        if(player.transform.position.x > winTransform.position.x)
        {
            Win();
        }
            
    }

    public void Loose()
    {
        player.Loose();
        foreach (Enemy enemy in enemies)
        {
            if(enemy != null)
                enemy.OnPlayerDefeated();
        }
        restartButton.SetActive(true);
        Debug.Log("Lost!!");
    }

    public void Win()
    {
        player.Win();
        restartButton.SetActive(true);
        Debug.Log("Won!!");
    }


    public void Restart()
    {
        instance = null;
        SceneManager.LoadSceneAsync(0);
    }
}
