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

    public AudioClip audioIdel1 = null;
    public AudioClip audioIdel2 = null;
    public AudioClip audioHop = null;
    public AudioClip audioHit = null;
    public AudioClip audioSplash = null;

    public ParticleSystem splash = null;
    public bool parentedToObject = false;

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
            if (Input.GetKeyDown(KeyCode.W))
            {
                CheckIfIdle(270, 0, 0);
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                CheckIfIdle(270, 180, 0);
            }
            if (Input.GetKeyDown(KeyCode.A))
            {
                CheckIfIdle(270, -90, 0);
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                CheckIfIdle(270, 90, 0);
            }


        }
    }
    void CheckIfIdle(float x, float y, float z)
    {
        chick.transform.rotation = Quaternion.Euler(x, y, z);
        CheckIfCanMove();

        if (Random.value < .4f) PlayAudioClip(audioIdel1);
    }

    void CheckIfCanMove()
    {
        RaycastHit hit;
        Physics.Raycast(this.transform.position,
                        -chick.transform.up,
                        out hit,
                        colliderDistCheck);
        Debug.DrawRay(this.transform.position,
            -chick.transform.up * colliderDistCheck,
            Color.red,
            duration: 2);

        if (hit.collider?.tag == "collider")
        {
            Debug.Log("Hit something forword.");

            isIdle = true;
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
            if (Input.GetKeyUp(KeyCode.W))
            {
                Moving(new Vector3(0, 0, moveDistance));
                SetMoveForwardsState();
            }
            else if (Input.GetKeyUp(KeyCode.S))
            {
                Moving(new Vector3(0, 0, -moveDistance));
            }
            else if (Input.GetKeyUp(KeyCode.A))
            {
                Moving(new Vector3(-moveDistance, 0, 0));
            }
            else if (Input.GetKeyUp(KeyCode.D))
            {
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

        PlayAudioClip(audioHop);

        LeanTween
            .move(this.gameObject, transform.position + delta, moveTime)
            .setOnComplete(MoveComplite);
    }
    void MoveComplite()
    {
        isJumping = false;
        isIdle = true;

        if (Random.value < .4f) PlayAudioClip(audioIdel2);
    }

    void SetMoveForwardsState()
    {
        Manager.instance.UpdateDistanceCount();
    }
    void IsVisible()
    {
        if (renderer.isVisible)
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

        PlayAudioClip(audioHit);

        Manager.instance.GameOver();
    }

    public void GotSoaked()
    {
        isDead = true;
        ParticleSystem.EmissionModule em = splash.emission;
        em.enabled = true;

        PlayAudioClip(audioSplash);

        chick.SetActive(false);

        Manager.instance.GameOver();
    }

    void PlayAudioClip(AudioClip audioClip)
    {
        this.GetComponent<AudioSource>().PlayOneShot(audioClip);
    }
}
