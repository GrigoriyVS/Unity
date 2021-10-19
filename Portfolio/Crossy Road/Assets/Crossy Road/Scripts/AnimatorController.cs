using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorController : MonoBehaviour
{
    public PlayerController playerController = null;
    private Animator animator = null;

    private void Start()
    {
        animator = this.GetComponent<Animator>();
    }
    private void Update()
    {
        if (playerController.isDead)
        {
            animator.SetBool("dead", true);
        }

        if (playerController.jumpStart)
        {
            animator.SetBool("jumpStart", true);
        }
        else if (playerController.isJumping)
        {
            animator.SetBool("jump", true);
        }
        else
        {
            animator.SetBool("jump", false);
            animator.SetBool("jumpStart", false);
        }

        if (!playerController.isIdle) return;

        if(Input.GetKeyDown(KeyCode.W)){
            gameObject.transform.rotation = Quaternion.Euler(270, 0, 0);
        }
        if (Input.GetKeyDown(KeyCode.S)){
            gameObject.transform.rotation = Quaternion.Euler(270, 180, 0);
        }
        if (Input.GetKeyDown(KeyCode.A)){
            gameObject.transform.rotation = Quaternion.Euler(270, -90, 0);
        }
        if (Input.GetKeyDown(KeyCode.D)){
            gameObject.transform.rotation = Quaternion.Euler(270, 90, 0);
        }

        
    }
}
