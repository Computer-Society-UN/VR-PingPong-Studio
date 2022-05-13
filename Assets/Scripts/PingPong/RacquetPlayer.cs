
using UnityEngine;

namespace PingPong
{
    public class RacquetPlayer : MonoBehaviour
    {
        private void OnTriggerEnter(Collider collision)
        {
            var ball = collision.GetComponent<BallSettings>();
            
            if (ball != null)
            {
                GameManager.Instance.KickBall(ball.GetBallColor());
            }
        }
        
    }
}
