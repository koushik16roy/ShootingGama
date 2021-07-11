using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Text _ScoreText , _BestScoreText;
    [SerializeField]
    private Image _LivesImg;
    [SerializeField]
    private Sprite[] _liveSprites;
    [SerializeField]
    private Text _gameOverText;
    [SerializeField]
    private Text _restartText;
    private GameManager _gameManager;

    public int playScore,BestScore;
    void Start()
    {
        _ScoreText.text = "score : " +0;
         _gameOverText.gameObject.SetActive(false);
         _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();
         if(_gameManager == null)
         {
             Debug.LogError("GameManager is NULL");
         } 
    }
    void Update()
    {
        
    }
    public void UpdateScore(int playScore)
    {
        playScore +=0;
        _ScoreText.text = "Score :" + playScore.ToString();
    }

   public void CheckForBestScore(int BestScore)
   {
        if(playScore > BestScore)
        {
            BestScore = playScore;
            _BestScoreText.text = "Best Score :" + BestScore;
        }
   }
    public void UpdateLives(int currentLives)
    {
        _LivesImg.sprite = _liveSprites[currentLives];
        if(currentLives == 0)
        {
            GameOverSequence();
        }
    }
    void GameOverSequence()
    {
        _gameManager.GameOver();
        _gameOverText.gameObject.SetActive(true);
        _restartText.gameObject.SetActive(true);
        StartCoroutine(GameOverFlickerRoutine());
    }
    IEnumerator GameOverFlickerRoutine()
    {
        while(true)
        {
            _gameOverText.text = "GAME OVER";
            yield return new WaitForSeconds(0.5f);
            _gameOverText.text = "";
            yield return new WaitForSeconds(0.5f);
        }
    }
}
