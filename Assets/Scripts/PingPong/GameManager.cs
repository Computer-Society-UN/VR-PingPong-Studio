
using System;
using System.Collections;
using System.IO;
using TMPro;
using UnityEngine;

public enum GameMode
{
    Tutorial,
    Wait1,
    Round1,
    Wait2,
    Round2
}

namespace PingPong
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;
        
        [SerializeField] private int minutesTutorial;
        [SerializeField] private int secondsTutorial;
        [SerializeField] private int minutesRound1;
        [SerializeField] private int secondsRound1;
        [SerializeField] private int minutesRound2;
        [SerializeField] private int secondsRound2;
        [SerializeField] private int minutesWait1;
        [SerializeField] private int secondsWait1;
        [SerializeField] private int minutesWait2;
        [SerializeField] private int secondsWait2;
        [Space(10)]
        [SerializeField] private int tutorialLimitBallsHit = 50;
        [SerializeField] private int round1LimitBallsHit = 50;
        [SerializeField] private int round2LimitBallsHit = 50;
        [Space(10)]
        [SerializeField] private TextMeshProUGUI timerText;
        [SerializeField] private TextMeshProUGUI gameModeText;
        [Space(10)]
        [SerializeField] private GameObject uiTutorial;
        [SerializeField] private GameObject uiRound1;
        [SerializeField] private GameObject uiRound2;
        [SerializeField] private GameObject uiTimer;
        [SerializeField] private GameObject uiGameOver;
        
        [HideInInspector] public GameMode gameMode;
        public bool gameOver;
        private bool _writeFile;
        
        private static string _fileName = "";
        private int _m,_s;
        
        private int _counterRound1Balls;
        private int _counterRound2Balls;
        
        private int _counterTutorialHits;
        
        private int _counterRound1Hits;
        private int _counterRound1FailuresHit;
        private int _counterRound1FailuresNotHit;
        
        private int _counterRound2Hits;
        private int _counterRound2FailuresHit;
        private int _counterRound2FailuresNotHit;

        public GameManager()
        {
            Instance = this;
        }
        
        private void Start()
        {
            _m = minutesTutorial;
            _s = secondsTutorial;
            
            WriteTimer(_m,_s);
            Invoke(nameof(UpdateTimer), 1f);
            gameModeText.text = "Tutorial";
            _fileName = Application.dataPath + "/Resultados.csv";
            
            StartCoroutine(UITutorial());
        }
        
        private void Update()
        {
            if (!gameOver)
            {
                _counterRound1FailuresNotHit = _counterRound1Balls - (_counterRound1Hits - _counterRound1FailuresHit);
                _counterRound2FailuresNotHit = _counterRound2Balls - (_counterRound2Hits - _counterRound2FailuresHit);
            }

            switch (gameMode)
            {
                case GameMode.Tutorial:
                {
                    if (_counterTutorialHits >= tutorialLimitBallsHit)
                    {
                        StopTimer();
                    }

                    break;
                }
                case GameMode.Round1:
                {
                    if (_counterRound1Hits >= round1LimitBallsHit)
                    {
                        StopTimer();
                    }

                    break;
                }
                case GameMode.Round2:
                {
                    if (_counterRound2Hits >= round2LimitBallsHit)
                    {
                        StopTimer();
                    }

                    break;
                }
                case GameMode.Wait1:
                    break;
                case GameMode.Wait2:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        public void UpdateBalls(string currentGameMode, BallColor color)
        {
            switch (currentGameMode)
            {
                case "Tutorial":
                    break;
                case "Round1" when color == BallColor.Green:
                    _counterRound1Balls++;
                    break;
                case "Round2" when color != BallColor.Green:
                    _counterRound2Balls++;
                    break;
            }
        }
        
        private void StartTimer(int minutes, int seconds)
        {
            _m = minutes;
            _s = seconds;
            
            WriteTimer(_m,_s);
            Invoke(nameof(UpdateTimer), 1f);
        }
        
        private void StopTimer()
        {
            switch (Instance.gameMode)
            {
                case GameMode.Tutorial:
                    CancelInvoke();
                    StartTimer(minutesRound1, secondsRound1);
                    gameMode = GameMode.Wait1;
                    gameModeText.text = "Descanso";
                    uiRound1.SetActive(true);
                    break;
                case GameMode.Wait1:
                    CancelInvoke();
                    StartTimer(minutesWait1, secondsWait1);
                    uiRound1.SetActive(false);
                    gameMode = GameMode.Round1;
                    BallSpawner.Instance.SpawnBalls();
                    gameModeText.text = "Ronda 1";
                    break;
                case GameMode.Round1:
                    CancelInvoke();
                    StartTimer(minutesRound2, secondsRound2);
                    gameMode = GameMode.Wait2;
                    gameModeText.text = "Descanso";
                    uiRound2.SetActive(true);
                    break;
                case GameMode.Wait2:
                    CancelInvoke();
                    StartTimer(minutesWait2, secondsWait2);
                    uiRound2.SetActive(false);
                    gameMode = GameMode.Round2;
                    BallSpawner.Instance.SpawnBalls();
                    gameModeText.text = "Ronda 2";
                    break;
                case GameMode.Round2:
                    CancelInvoke();
                    uiTimer.SetActive(false);
                    uiGameOver.SetActive(true);
                    gameOver = true;
                    gameModeText.text = "";
                    StartCoroutine(SaveFileCsv());
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        private void UpdateTimer()
        {
            _s--;
            
            if (_s < 0)
            {
                if(_m == 0)
                {
                    StopTimer();
                    return;
                }
                else
                {
                    _m--;
                    _s = 59;
                }
            }
            
            WriteTimer(_m,_s);
            Invoke(nameof(UpdateTimer), 1f);
        }
        
        private void WriteTimer(int minutes, int seconds)
        {
            if (_s < 10)
            {
                timerText.text = minutes + ":0" + seconds;
            }
            else
            {
                timerText.text = minutes + ":" + seconds;
            }
        }

        public void KickBall(BallColor ballColor)
        {
            switch (gameMode)
            {
                case GameMode.Tutorial:
                    _counterTutorialHits++;
                    break;
                case GameMode.Round1 when ballColor == BallColor.Green:
                    _counterRound1Hits++;
                    break;
                case GameMode.Round1:
                    _counterRound1FailuresHit++;
                    break;
                case GameMode.Round2 when ballColor == BallColor.Green:
                    _counterRound2FailuresHit++;
                    break;
                case GameMode.Round2:
                    _counterRound2Hits++;
                    break;
                case GameMode.Wait1:
                    break;
                case GameMode.Wait2:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        public void PlayerHit(BallColor ballColor)
        {
            switch (gameMode)
            {
                case GameMode.Tutorial:
                    break;
                case GameMode.Round1 when ballColor != BallColor.Green:
                    _counterRound1FailuresHit++;
                    break;
                case GameMode.Round2 when ballColor == BallColor.Green:
                    _counterRound2FailuresHit++;
                    break;
                case GameMode.Wait1:
                    break;
                case GameMode.Wait2:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        private IEnumerator SaveFileCsv()
        {
            yield return new WaitForSeconds(1f);
            TextWriter textWriter = new StreamWriter(_fileName, false);
            textWriter.WriteLine("                 Round 1 / Round 2");
            textWriter.Close();
            
            textWriter = new StreamWriter(_fileName, true);
            textWriter.WriteLine("Aciertos:          " + _counterRound1Hits + "," + _counterRound2Hits);
            textWriter.WriteLine("Fallos Golpe:      " + _counterRound1FailuresHit + "        " + _counterRound2FailuresHit);
            textWriter.WriteLine("Fallos No Golpe:   " + _counterRound1FailuresNotHit + "        " + _counterRound2FailuresNotHit);
            textWriter.Close();
        }

        private IEnumerator UITutorial()
        {
            yield return new WaitForSeconds(5f);
            uiTutorial.SetActive(false);
        }
        
    }
}
