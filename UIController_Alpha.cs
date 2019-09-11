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
    public void startNewGame() {
        GameState.resetGame();
        dynamicObjects.SetActive(true);
        GameState.gamePaused = false;
        hidePanels();
    }
    public void startNextRound() {
        //dynamicObjects.SetActive(true);
        GameState.gamePaused = false;
        GameState.resetRound();
        hidePanels();
    }
    public void exitSummary() {
        hidePanels();
        showPanel("MainPanel");
    }
    public void hidePanels() {
        foreach (Transform child in transform) {
            child.gameObject.SetActive(false);
        }
    }
    private void OnEnable() {
        EventManager.StartListening("END_GAME", endGame);
        EventManager.StartListening("NEXT_ROUND", nextRound);
    }
    private void OnDisable() {
        EventManager.StopListening("END_GAME", endGame);
        EventManager.StopListening("NEXT_ROUND", nextRound);
    }
    private void showPanel(string name) {
        gameObject.transform.Find(name).gameObject.SetActive(true);
    }
    private void nextRound() {
        //dynamicObjects.SetActive(false);
        GameState.gamePaused = true;
        hidePanels();
        showPanel("NextRoundPanel");
        setNextRoundText();
        StartCoroutine(waitForNextRound());
        audioSource.PlayOneShot(sounds[0]);
    }
    private void setGameWinnerText() {
        gameWinnerText.GetComponent<Text>().text = 
            "You " + (GameState.playerScore < GameState.opponentScore ? "lose" : "win") + "! (<b>" + GameState.playerScore + "-" + GameState.opponentScore + "</b>)";
    }
    private void setNextRoundText() {
        roundScoreText.GetComponent<Text>().text = 
            "Current score: <b>" + GameState.playerScore + "-" + GameState.opponentScore + "</b>";
        roundWinnerText.GetComponent<Text>().text = 
            "You" + (GameState.lastRoundWinner == 0 ? " win!" : " lose!");
    }
    private IEnumerator waitForNextRound() {
        yield return new WaitForSeconds(3.0f);
        startNextRound();
    }
    private void endGame() {
        audioSource.PlayOneShot(sounds[1]);
        hidePanels();
        showPanel("YouWinPanel");
        setGameWinnerText();
        //GameState.resetGame();
        //dynamicObjects.SetActive(false);
        GameState.gamePaused = true;
    }
    private void Update() {
    }
}
