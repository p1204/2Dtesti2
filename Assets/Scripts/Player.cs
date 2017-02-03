using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine.UI;

public class Player : MonoBehaviour {
    public Vector2 jumpHeight;
    public Animator player;
    public Canvas InventoryCanvas;
    public Text itemText;
    public GameObject Door;
    public Transform startPoint;
    public Transform endPointRight;
    public Transform endPointLeft;
    public Transform laserGunPoint;
    public bool rightTouchingWall;
    public bool leftTouchingWall;
    List<string> Inventory = new List<string>();

    private bool inventoryOpen;
    /*
    todo/notes:
    -Make public arrays of consoles and door
    so that when player interracts with console1-> door1 opens
    console2->door2 opens and so on....

    -remove plutonium stick from the list when player opens a door//done??

    -flip the character depending on movement direction. flipt the arm aiming too at the same time
    -boolean for direction true=right false=left
    -finish shooting laser
    -make test object-> shoot it with laser ->door opens
    -make arm rotation better
    -fix the cursor shit

    -learn math :D

    -level controller script and player controller script-> easier to put together later
        */

    // public Animation Idle;
    // Use this for initialization
    void Start () {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
        InventoryCanvas.gameObject.SetActive(false);
        inventoryOpen = false;
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //Debug.Log("Jump");
            GetComponent<Rigidbody2D>().AddForce(jumpHeight, ForceMode2D.Impulse);
        }
        if (Input.GetKey(KeyCode.D))
        {
            player.SetTrigger("Left");
        }
        if (Input.GetKey(KeyCode.A))
        {
            player.SetTrigger("Right");
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            //open inventory
            ShowInventory();

        }
        if (Input.GetButton("Fire1"))
        {
            ShootLaserGun();
        }
        else if (Input.GetKeyUp(KeyCode.D))
        {
            player.SetTrigger("LeftKeyUp");
        }
        else if (Input.GetKeyUp(KeyCode.A))
        {
            player.SetTrigger("RightKeyUp");
        }
        Raycasting();

        //if collider trigger is "Finnish" -> load last scene
        //ui elementti "you wake up in stange place.Whats that on the middle?"
    }

    void Raycasting()
    {
        //this is for recognising if the player is touching a wall from the left side or right
        //used to disable level rotation once the player is "stuck"

        Debug.DrawLine(startPoint.position,endPointLeft.position,Color.cyan);//leftline

        Debug.DrawLine(startPoint.position, endPointRight.position, Color.red);//rightline

        leftTouchingWall = Physics2D.Linecast(startPoint.position, endPointLeft.position, 1<< LayerMask.NameToLayer("Level"));
        rightTouchingWall = Physics2D.Linecast(startPoint.position, endPointRight.position, 1 << LayerMask.NameToLayer("Level"));

    }
    void ShootLaserGun()
    {
        //get mouse position
        //get gun position
        //draw raycast between them
        //Debug.DrawLine(laserGunPoint.position, Input.mousePosition, Color.red);

    }
    void ShowInventory()
    {
        //open inventory list element
        //make ui element of the list
        if (!inventoryOpen)
        {
            inventoryOpen = true;
            InventoryCanvas.gameObject.SetActive(true);

            foreach (string item in Inventory)
            {
                Debug.Log(item);
                itemText.text = item;
            }
        }
        else if (inventoryOpen)
        {
            inventoryOpen = false;
            InventoryCanvas.gameObject.SetActive(false);
        }
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Finish"))
        {
            Debug.Log("collision");
            SceneManager.LoadScene(2);
        }
        if (other.gameObject.CompareTag("Plutonium"))
        {
            //pick up the plutonium stick
            other.gameObject.SetActive(false);

            //add plutonium to the inventory
            Inventory.Add("Plutonium stick");

        }
        if (other.gameObject.CompareTag("Console"))
        {
            foreach (string item in Inventory)
            {
                itemText.text = item;
                if(item == "Plutonium stick")
                {
                    //open the door
                    Door.gameObject.SetActive(false);
                    Inventory.Remove("Plutonium stick");
                    itemText.text = "";
                }
            }
        }

    }
    }
