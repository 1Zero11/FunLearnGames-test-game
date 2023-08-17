using Spine;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float speed = 10f;
    public ParticleSystem muzzle;
    public GameObject explosion;

    private SkeletonAnimation skeletonAnimation;
    private bool isShooting = false;
    private bool lockPlayer = false;
    private AudioSource shootSource;
    

    void Start()
    {
        skeletonAnimation = GetComponentInChildren<SkeletonAnimation>();
        shootSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (!lockPlayer)
        {
            if (!isShooting && Input.GetMouseButtonDown(0))
            {
                StopAndShoot();
            }


            if (!isShooting)
            {
                transform.Translate(Vector3.right * speed * Time.deltaTime);
            }
        }
    }

    void StopAndShoot()
    {
        isShooting = true;

        // Check for enemy collision
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
        if (hit.collider != null && hit.collider.CompareTag("Enemy"))
        {
            // Player hit an enemy
            skeletonAnimation.AnimationState.SetAnimation(0, "shoot", false);
            StartCoroutine(LateShoot(hit));
        }
        else
        {
            // Player didn't hit an enemy, play a different animation
            skeletonAnimation.AnimationState.SetAnimation(0, "shoot_fail", false);
        }
        skeletonAnimation.Update(0);
        skeletonAnimation.AnimationState.Complete += OnShootAnimationComplete;

    }

    public IEnumerator LateShoot(RaycastHit2D hit)
    {
        shootSource.Play();
        yield return new WaitForSeconds(0.5f);
        muzzle.gameObject.SetActive(true);
        hit.collider.GetComponent<Enemy>().Hit();
        Instantiate(explosion, hit.point, Quaternion.identity);
    }

    void OnShootAnimationComplete(TrackEntry trackEntry)
    {
        isShooting = false;
        skeletonAnimation.AnimationState.Complete -= OnShootAnimationComplete;
        skeletonAnimation.AnimationState.SetAnimation(0, "walk", true);
    }

    public void Loose()
    {
        lockPlayer = true;
        skeletonAnimation.AnimationState.SetAnimation(0, "loose", false);
    }

    public void Win()
    {
        lockPlayer = true;
        skeletonAnimation.AnimationState.SetAnimation(0, "idle", true);
    }
}
