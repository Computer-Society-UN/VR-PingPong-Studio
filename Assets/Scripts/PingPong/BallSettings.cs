
using System;
using System.Collections;
using UnityEngine;

namespace PingPong
{
    public enum BallColor
    {
        White,
        Green,
        Red,
        Blue,
        Yellow
    }
    
    [RequireComponent(typeof(Rigidbody))]
    public class BallSettings : MonoBehaviour
    {
        [SerializeField] private float lauchForce = 4f;
        [SerializeField]private BallColor ballColor;
        
        private Rigidbody _ballRigidbody;
        
        private void Awake()
        {
            _ballRigidbody = gameObject.GetComponent<Rigidbody>();
            
            switch (GameManager.Instance.gameMode)
            {
                case GameMode.Tutorial:
                    GameManager.Instance.UpdateBalls("Tutorial", ballColor);
                    break;
                case GameMode.Round1:
                    GameManager.Instance.UpdateBalls("Round1", ballColor);
                    break;
                case GameMode.Round2:
                    GameManager.Instance.UpdateBalls("Round2", ballColor);
                    break;
                case GameMode.Wait1:
                    GameManager.Instance.UpdateBalls("Tutorial", ballColor);
                    break;
                case GameMode.Wait2:
                    GameManager.Instance.UpdateBalls("Round1", ballColor);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            StartCoroutine(LaunchBall());

            Destroy(gameObject, 3f);
        }
        
        private IEnumerator LaunchBall()
        {
            yield return new WaitForSeconds(0.25f);
            _ballRigidbody.isKinematic = false;
            _ballRigidbody.velocity = transform.forward * lauchForce;
        }
        
        public BallColor GetBallColor()
        {
            return ballColor;
        }
        
        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("RacquetPlayer") || collision.gameObject.CompareTag("Table"))
            {
                gameObject.GetComponent<AudioSource>().Play();
            }
            if (collision.gameObject.CompareTag("Player"))
            {
                GameManager.Instance.PlayerHit(GetBallColor());
            }
        }
        
    }
}
