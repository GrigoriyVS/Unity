using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveDistance = 1;
    public float moveTime = 0.4f;
    public float colliderDistCheck = 1;

    public bool isIdle = true;
    public bool isDead = false;
    public bool isMoving = false;
    public bool isJumping = false;
    public bool jumpStart = false;

    public ParticleSystem particle = null;
    public GameObject chick = null;
    private Renderer renderer = null;

    private bool isVisible = false;

    void Start()
    {
        renderer = chick.GetComponent<Renderer>();

    }
    void Update()
    {
        if (!Manager.instance.CanPlay()) return;

        if (isDead) return;
        
        CanIdle();
        CanMove();

        IsVisible();
    }

    void CanIdle()
    {
        if (isIdle)
        {
            if (isAnyArrowPressDown())
            {
                CheckIfCanMove();
            }
        }
    }
    bool isAnyArrowPressDown()
    {
        return (Input.GetKeyDown(KeyCode.W) ||
                Input.GetKeyDown(KeyCode.S) ||
                Input.GetKeyDown(KeyCode.A) ||
                Input.GetKeyDown(KeyCode.D));
    }
    void CheckIfCanMove()
    {
        RaycastHit hit;
        Physics.Raycast(this.transform.position,
                        -chick.transform.up,
                        out hit,
                        colliderDistCheck);
        Debug.DrawRay(this.transform.position,
            -chick.transform.up*colliderDistCheck,
            Color.red,
            duration:2);

        if (hit.collider?.tag == "collider")
        {
            Debug.Log("Hit something forword.");
        }
        else
        {
            SetMove();
        }
    }
    void SetMove()
    {
        Debug.Log("Hit nothing. keep moving.");

        isIdle = false;
        isMoving = true;
        jumpStart = true; 
    }
    void CanMove()
    {
        if (isMoving)
        { 
            if (Input.GetKeyUp(KeyCode.W)){
                Moving(new Vector3(0, 0, moveDistance));
                SetMoveForwardsState();
            }
            else if (Input.GetKeyUp(KeyCode.S)){
                Moving(new Vector3(0,0, -moveDistance));
            }
            else if (Input.GetKeyUp(KeyCode.A)){
                Moving(new Vector3(-moveDistance, 0, 0));
            }
            else if (Input.GetKeyUp(KeyCode.D)){
                Moving(new Vector3(moveDistance, 0, 0));
            }
        }
    }
    void Moving(Vector3 delta)
    {
        isIdle = false;
        isMoving = false;
        isJumping = true;
        jumpStart = false;

        LeanTween
            .move(this.gameObject, transform.position+delta, moveTime)
            .setOnComplete(MoveComplite);
    }
    void MoveComplite()
    {
        isJumping = false;
        isIdle = true;
    }

    void SetMoveForwardsState()
    {
        Manager.instance.UpdateDistanceCount();
    }
    void IsVisible()
    {
        if(renderer.isVisible)
        {
            isVisible = true;
        }
        if (!renderer.isVisible && isVisible)
        {
            Debug.Log("Player off screen");
            GotHit();
        }
    }
    public void GotHit()
    {
        isDead = true;
        ParticleSystem.EmissionModule em = particle.emission;
        em.enabled = true;

        Manager.instance.GameOver();
    }
}
