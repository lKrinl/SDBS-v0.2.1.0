using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using System;
using UnityEngine.UI;  

public class UIController : MonoBehaviour
{
    private GameObject roundScoreText;
    private GameObject roundWinnerText;
    private GameObject gameWinnerText;
    private GameObject dynamicObjects;
    private AudioSource audioSource;
    private AudioSource musicSource;
    private List<AudioClip> sounds = new List<AudioClip>();
    private void Start() {
        audioSource = gameObject.GetComponents<AudioSource>()[0];
        musicSource = gameObject.GetComponents<AudioSource>()[1];
        musicSource.loop = true;
        dynamicObjects = GameObject.Find("DynamicObjects");
        dynamicObjects.SetActive(false);
        gameWinnerText = GameObject.Find("GameWinnerText");
        roundScoreText = GameObject.Find("RoundScoreText");
        roundWinnerText = GameObject.Find("RoundWinnerText");
        hidePanels();
        showPanel("MainPanel");
        sounds.Add((AudioClip)Resources.Load("Sound/whistle1"));
        sounds.Add((AudioClip)Resources.Load("Sound/whistle2"));
        musicSource.clip = (AudioClip)Resources.Load("Sound/ambient");
        musicSource.volume = 0.5f;
        musicSource.Play();
    }

    private void Update() {
    }
}

////To do/////
