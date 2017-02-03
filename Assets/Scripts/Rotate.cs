using UnityEngine;
using System.Collections;

public class Rotate : MonoBehaviour {
    float z;
    public float rotationRate;
    //public GameObject player;
    // Use this for initialization
    //if wall collader and player collider are not touching rotate
    //if wall collader and player collider are touching dont rotate
    void Start ()
    {
        z = 0f;
    }
	
	// Update is called once per frame
	void Update () {
    }

    void FixedUpdate()
    {
        GameObject Player = GameObject.Find("Player");
        PlayerScript playerScript = Player.GetComponent<PlayerScript>();

        if (Input.GetKey(KeyCode.A) && playerScript.ishanging == false)
        {
            //if boolean from player script is true that player is touching left wall you cannot turn more to the left anymore.
            if (!playerScript.rightTouchingWall || playerScript.facingRight==true)
            {
                rotateLeft();
                //Debug.Log("Left");
            }
        }

        if (Input.GetKey(KeyCode.D) && playerScript.ishanging == false)
        {
            if (!playerScript.rightTouchingWall || playerScript.facingRight == false)
            {
                rotateRight();
                //Debug.Log("Right");
            }
        }

        if (Input.GetKeyDown(KeyCode.W)){
            Debug.Log("attempt climb");
            Vector3 target=new Vector3(0, 0,0);
            if (playerScript.facingRight == true)
            {
                
                transform.position = Vector3.MoveTowards(transform.position, target,5);
                transform.Rotate(Vector3.back * 4);   
                        
            }else{
                float step = 13 * Time.deltaTime;
                transform.position = Vector3.MoveTowards(transform.position, target,5);
                transform.Rotate(Vector3.forward * 4);            
            }
        }
    }

    void rotateLeft(){
        GameObject Player = GameObject.Find("Player");
        PlayerScript playerScript = Player.GetComponent<PlayerScript>();
        if (playerScript.isrunning == false){
            transform.Rotate(Vector3.forward * +1);
            playerScript.animat.SetFloat("Speed", 1);
            if (playerScript.facingRight == true)
            {
                Vector3 newScale = Player.transform.localScale;
                newScale.x *= -1;
                Player.transform.localScale = newScale;
                playerScript.facingRight = false;
            }
        }
        else {
            transform.Rotate(Vector3.forward * +2);
            playerScript.animat.SetFloat("Speed", 2);        
        }
    }


    void rotateRight()
    {
        GameObject Player = GameObject.Find("Player");
        PlayerScript playerScript = Player.GetComponent<PlayerScript>();
        if (playerScript.isrunning == false)
        {
            transform.Rotate(Vector3.forward * -1);
            playerScript.animat.SetFloat("Speed", 1);
            if(playerScript.facingRight==false) {
                Vector3 newScale = Player.transform.localScale;
                newScale.x *= -1;
                Player.transform.localScale = newScale;
                playerScript.facingRight = true;
            }
        }
        else
        {
            transform.Rotate(Vector3.forward * -2);
            playerScript.animat.SetFloat("Speed", 2);
        }
    }
}
