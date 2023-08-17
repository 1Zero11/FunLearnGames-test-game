using Spine;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 3f;
    public bool isMoving = true;

    private PlayerMove player;
    private SkeletonAnimation skeletonAnimation;
    private GameController gameController;
    

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerMove>();
        gameController = FindObjectOfType<GameController>();
        skeletonAnimation = GetComponentInChildren<SkeletonAnimation>();
    }

    // Update is called once per frame
    void Update()
    {
        if(isMoving && gameController.looseTransform.position.x > transform.position.x)
        {
            gameController.Loose();
            OnPlayerDefeated();
        }

        if (isMoving)
        {
            transform.Translate(Vector3.left * speed * Time.deltaTime);
        }
    }


    public void Hit()
    {
        Destroy(gameObject);

    }

    public void OnPlayerDefeated()
    {
        isMoving = false;
        skeletonAnimation.AnimationState.SetAnimation(0, "win", true);
    }

}
