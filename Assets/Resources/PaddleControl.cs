
using UnityEngine;

public class PaddleControl : MonoBehaviour {
    public GameObject paddle;
    public GameObject avatar;
    public GameObject player;
    private GameObject _rightHand;
    private GameObject _leftHand;
    public bool holdingPaddleRight;
    public bool holdingPaddleLeft;
    private GameObject _collidingObject;
    // Use this for initialization
    void Start () {
        //right_hand = avatar.transform.GetChild(1).gameObject;
        holdingPaddleRight = false;
        holdingPaddleLeft = false;
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        //set up hand collider
        if (avatar.transform.childCount > 1)
        {
            _leftHand = avatar.transform.GetChild(0).gameObject;
            _rightHand = avatar.transform.GetChild(1).gameObject;
            if (!_rightHand.GetComponent(typeof(Collider)))
            {
                _rightHand.AddComponent<BoxCollider>();
                _rightHand.GetComponent<Collider>().isTrigger = true;
                _rightHand.GetComponent<BoxCollider>().size = new Vector3(0.2f, 0.2f, 0.2f);
            }
            if (!_leftHand.GetComponent(typeof(Collider)))
            {
                _leftHand.AddComponent<BoxCollider>();
                _leftHand.GetComponent<Collider>().isTrigger = true;
                _leftHand.GetComponent<BoxCollider>().size = new Vector3(0.2f, 0.2f, 0.2f);
            }
        }

        //Paddle holding/releasing
        if (OVRInput.Get(OVRInput.RawAxis1D.RHandTrigger) > 0.2f && _collidingObject && _collidingObject.name == "hand_right")
        {
            //PADDLE TRACKING
            avatar.transform.position = player.transform.position;
            //if hand is visible
            if (avatar.transform.childCount > 1)
            {
                paddle.GetComponent<Rigidbody>().isKinematic = true;
                paddle.GetComponent<Rigidbody>().useGravity = false;

                _rightHand = avatar.transform.GetChild(1).gameObject;
                //put paddle to hand
                paddle.transform.position = _rightHand.transform.position + paddle.transform.forward * -0.1f;
                //paddle.GetComponent<Rigidbody>().MovePosition((right_hand.transform.position + paddle.transform.forward * -0.1f)*Time.fixedDeltaTime);
                paddle.transform.rotation = _rightHand.transform.rotation;
                paddle.GetComponent<Rigidbody>().velocity = (OVRInput.GetLocalControllerVelocity(OVRInput.Controller.RTouch));
                //Debug.Log(paddle.GetComponent<Rigidbody>().velocity);
                paddle.transform.Rotate(new Vector3(-45f, 0, 0));
                holdingPaddleRight = true;
                holdingPaddleLeft = false;

            }
        }
        else if (OVRInput.Get(OVRInput.RawAxis1D.LHandTrigger) > 0.2f && _collidingObject && _collidingObject.name == "hand_left")
        {
            //PADDLE TRACKING
            avatar.transform.position = player.transform.position;
            //if hand is visible
            if (avatar.transform.childCount > 1)
            {
                paddle.GetComponent<Rigidbody>().isKinematic = true;
                paddle.GetComponent<Rigidbody>().useGravity = false;

                _leftHand = avatar.transform.GetChild(0).gameObject;
                //put paddle to hand
                paddle.transform.position = _leftHand.transform.position + paddle.transform.forward * -0.1f;
                //paddle.GetComponent<Rigidbody>().MovePosition((right_hand.transform.position + paddle.transform.forward * -0.1f)*Time.fixedDeltaTime);
                paddle.transform.rotation = _leftHand.transform.rotation;
                paddle.GetComponent<Rigidbody>().velocity = (OVRInput.GetLocalControllerVelocity(OVRInput.Controller.LTouch));
                //Debug.Log(paddle.GetComponent<Rigidbody>().velocity);
                paddle.transform.Rotate(new Vector3(-45f, 0, 0));
                holdingPaddleLeft = true;
                holdingPaddleRight = false;

            }
        }
        else
        {
            paddle.GetComponent<Rigidbody>().isKinematic = false;
            paddle.GetComponent<Rigidbody>().useGravity = true;
            holdingPaddleRight = false;
            holdingPaddleLeft = false;
        }

        

    }
    
    void OnTriggerEnter(Collider other)

    {
        //Debug.Log(other.gameObject.name);
        if (other.gameObject.name == "hand_right" && !_collidingObject)
        {

            _collidingObject = _rightHand;

        }
        if (other.gameObject.name == "hand_left" && !_collidingObject)
        {
            _collidingObject = _leftHand;
        }

    }
    private void OnTriggerStay(Collider other)
    {
        if (_collidingObject == null)
        {
            if (other.gameObject.name == "hand_right")
            {
                _collidingObject = _rightHand;
            }
            else if (other.gameObject.name == "hand_left")
            {
                _collidingObject = _leftHand;
            }
        }
    }
    void OnTriggerExit(Collider other)

    {
        //Debug.Log(other.gameObject.name);
        if ((other.gameObject.name == "hand_right" && _collidingObject && _collidingObject.name == "hand_right") || (other.gameObject.name == "hand_left" && _collidingObject && _collidingObject.name == "hand_left"))

        {

            _collidingObject = null;

        }

    }
}
