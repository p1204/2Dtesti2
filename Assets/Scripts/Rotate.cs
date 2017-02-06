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
    }

    public void rotateLeft(){
      
        GameObject Player = GameObject.Find("Player");
        PlayerScript playerScript = Player.GetComponent<PlayerScript>();
       if (playerScript.isrunning == false){
            transform.Rotate(Vector3.forward * +1);
            playerScript.animat.SetFloat("Speed", 1);
        }
        else {
            transform.Rotate(Vector3.forward * +2);
            playerScript.animat.SetFloat("Speed", 2);        
        }
    }


    public void rotateRight()
    {
        GameObject Player = GameObject.Find("Player");
        PlayerScript playerScript = Player.GetComponent<PlayerScript>();

        if (playerScript.isrunning == false)
        {
            transform.Rotate(Vector3.forward * -1);
            playerScript.animat.SetFloat("Speed", 1);
        }else{
            transform.Rotate(Vector3.forward * -1.4F);
            playerScript.animat.SetFloat("Speed", 2);
        }
    }
}
