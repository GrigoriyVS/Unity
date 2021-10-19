using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public int coinValue = 1;
    public GameObject coin = null;
    public AudioClip audioClip = null;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Debug.Log("Pic a coin");

            Manager.instance.UpdateCoinCount(coinValue);

            
            this.GetComponent<AudioSource>().PlayOneShot(audioClip);

            coin.SetActive(false);

            Destroy(this.gameObject, audioClip.length);
        }
    }
    
}
