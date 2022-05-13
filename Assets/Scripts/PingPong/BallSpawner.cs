
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace PingPong
{
    public class BallSpawner : MonoBehaviour
    {
        public static BallSpawner Instance;
        
        [SerializeField] private GameObject ballWhite;
        [SerializeField] private List<GameObject> ballColors;
        [SerializeField] private List<GameObject> spawnPoints;

        public BallSpawner()
        {
            Instance = this;
        }

        private void Start()
        {
            switch (GameManager.Instance.gameMode)
            {
                case GameMode.Tutorial:
                    StartCoroutine(SpawnNewBall("Tutorial"));
                    break;
                case GameMode.Round1:
                    StartCoroutine(SpawnNewBall("Round1"));
                    break;
                case GameMode.Round2:
                    StartCoroutine(SpawnNewBall("Round2"));
                    break;
                case GameMode.Wait1:
                    break;
                case GameMode.Wait2:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            if(GameManager.Instance.gameOver || GameManager.Instance.gameMode is GameMode.Wait1 or GameMode.Wait2)
            {
                return;
            }
            
            Invoke(nameof(SpawnBalls), 5f);
        }
        
        public void SpawnBalls()
        {
            StartCoroutine(SpawnNewBall(GameManager.Instance.gameMode.ToString()));
            Invoke(nameof(SpawnBalls), 1.99f);
        }
        
        private IEnumerator SpawnNewBall(string gameMode)
        {
            switch (gameMode)
            {
                case "Tutorial":
                    yield return new WaitForSeconds(0.01f);
                    var spawnPointT = spawnPoints[Random.Range(0, spawnPoints.Count)].transform;
                    Instantiate(ballWhite,spawnPointT.position, spawnPointT.rotation);
                    break;

                case "Round1":
                    yield return new WaitForSeconds(0.01f);
                    var spawnPointR1 = spawnPoints[Random.Range(0, spawnPoints.Count)].transform;
                    var listRange1 = Random.Range(0, ballColors.Count);
                    Instantiate(ballColors[listRange1],spawnPointR1.position, spawnPointR1.rotation);
                    break;
                
                case "Round2":
                    yield return new WaitForSeconds(0.01f);
                    var spawnPointR2 = spawnPoints[Random.Range(0, spawnPoints.Count)].transform;
                    var listRange2 = Random.Range(0, ballColors.Count);
                    Instantiate(ballColors[listRange2],spawnPointR2.position, spawnPointR2.rotation);
                    break;
            }
        }
        
        private void Update()
        {
            if(GameManager.Instance.gameOver || GameManager.Instance.gameMode is GameMode.Wait1 or GameMode.Wait2)
            {
                CancelInvoke(nameof(SpawnBalls));
            }
        }
        
    }
}
