using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Itemcollector : MonoBehaviour
{
    private int cherries = 0;
    [SerializeField] private Text cherriesText;
    [SerializeField] private AudioSource cherriesAudioSource;
    private void OnTriggerEnter2D(Collider2D collision)
    {
       if (collision.gameObject.CompareTag("Cherry"))
        {
            cherriesAudioSource.Play();
            Destroy(collision.gameObject);
            cherries++;
            cherriesText.text = "Cherries:" + cherries;
        }
    }
}

