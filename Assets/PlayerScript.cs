using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine.UI;
public class PlayerScript : MonoBehaviour {

    public float speed;
    public float airtime=0;
    public Animator animat;
    public bool isrunning=false;
    public bool isfalling=false;
    public bool ishanging=false;
    public bool isgrounded=true;
    public bool isjumping = false;
    public bool isledge1 = false;
    public bool isledge2 = false;
    public bool isledge3 = false;
    private bool grounded;
    public bool facingRight = true;
    public LayerMask Ledge1;
    public LayerMask Ledge2;
    public LayerMask Ledge3;
    public LayerMask groundIs;
    public Transform groundCheck;
    public Transform LedgeCheck1;
    public Transform LedgeCheck2;
    public Transform LedgeCheck3;
    public float ledgeradius = 0.005f;
    public float groundradius = 0.01f;
    public Vector2 jumpHeight;
    public Vector3 climbPosition;
    public Animator player;
    public Canvas InventoryCanvas;
    public Text itemText;
    public GameObject Door;
    public Transform startPoint;
    public Transform endPointRight;
    public Transform endPointLeft;
    public Transform laserGunPoint;
    public bool rightTouchingWall;
    List<string> Inventory = new List<string>();
    private bool inventoryOpen;
    GameObject level;
    Rotate rotator;


    // Use this for initialization
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
        //InventoryCanvas.gameObject.SetActive(false);
        //inventoryOpen = false;
        animat = GetComponent<Animator>();
        level = GameObject.Find("demoLevelBase");
        rotator = level.GetComponent<Rotate>();

    }

    void FixedUpdate() {
        isgrounded = Physics2D.OverlapCircle(groundCheck.position, groundradius, groundIs);
        isledge1 = Physics2D.OverlapCircle(LedgeCheck1.position, ledgeradius, Ledge1);
        isledge2 = Physics2D.OverlapCircle(LedgeCheck2.position, ledgeradius, Ledge2);
        isledge3 = Physics2D.OverlapCircle(LedgeCheck3.position, ledgeradius, Ledge3);
        animat.SetBool("Ground", isgrounded);
        animat.SetFloat("Speed", Mathf.Abs(GetComponent<Rigidbody2D>().velocity.y));
        if (GetComponent<Rigidbody2D>().velocity.y <-1f && isgrounded==false)
        {
            isfalling = true;
            animat.SetBool("Jumping", false);
            animat.SetBool("Falling",isfalling);
        }
        if (GetComponent<Rigidbody2D>().velocity.y == 0) {
            isfalling = false;
            animat.SetBool("Falling", isfalling);

        }
        if (ishanging == true || isgrounded==true)
        {
            isfalling = false;
            isjumping = false;
        }
    }

    void OnCollisionEnter2D(Collision2D coll) {
        if (isgrounded == true){
            isjumping = false;
            animat.SetBool("Jumping", false);
            isfalling = false;
            animat.SetBool("Falling", false);
        }
    }

    public void turnAround() { 

        bool facing = facingRight;
       
        //if boolean from player script is true that player is touching left wall you cannot turn more to the left anymore.
        if ( facing == true)
            {             
                Vector3 newScale = GameObject.Find("Player").transform.localScale;
                newScale.x *= -1;
                GameObject.Find("Player").transform.localScale = newScale;
                facingRight = false;        
            }    
         
            if(facing==false){
                Vector3 newScale = GameObject.Find("Player").transform.localScale;
                newScale.x *= -1;
                GameObject.Find("Player").transform.localScale = newScale;
                facingRight = true;             
          }
        animat.SetBool("Turning", false);

    }


   
    // Update is called once per frame
    void Update(){
        if (isfalling==true)
            airtime += Time.deltaTime;
        else
            airtime = 0f;

        if (Input.GetKey(KeyCode.A) && ishanging == false )
        {
            //if boolean from player script is true that player is touching left wall you cannot turn more to the left anymore.
            if (facingRight == true)
            {
                Debug.Log("Turning left");
                animat.SetBool("Turning", true);
            }
   
            if(facingRight==false && rightTouchingWall==false) {
                Debug.Log("Moving left");
                rotator.rotateLeft();
            }           
        }

        if (Input.GetKey(KeyCode.D) && ishanging == false && animat.GetBool("Climbing")==false)
        {
            //if boolean from player script is true that player is touching left wall you cannot turn more to the left anymore.
            if (facingRight == false)
            {
                Debug.Log("Turning right");
                animat.SetBool("Turning", true);
            }
           if(facingRight == true && rightTouchingWall == false)
            {
                Debug.Log("Moving right");
                rotator.rotateRight();
            }
        }

        if (Input.GetKey(KeyCode.W) && ishanging == true && animat.GetBool("Climbing") == false) {
            ishanging = false;
            animat.SetBool("Hanging", ishanging);
            animat.SetBool("Climbing", true);
            Debug.Log("Climbing up");
        }

        if (Input.GetKey(KeyCode.S) && ishanging==true) {
            ishanging = false;
            animat.SetBool("Hanging", ishanging);
            GetComponent<Rigidbody2D>().constraints &= ~RigidbodyConstraints2D.FreezePositionY;
            GetComponent<Rigidbody2D>().constraints &= ~RigidbodyConstraints2D.FreezePositionX;
            isfalling = true;
            animat.SetBool("Falling", isfalling);
        }

        if (Input.GetKeyDown(KeyCode.S) && isgrounded== true)
        {
            
            if (isledge3 == false) {
                Debug.Log("Climb down");
                turnAround();
                animat.SetBool("Climb Down", true);
            }
        }

        if (Input.GetKeyDown(KeyCode.Space) && isgrounded==true)
        {
            isjumping = true;
            if (GetComponent<Rigidbody2D>().velocity.y == 0)
                animat.SetBool("Jumping", true);
            else
                Jump();
        }

        if (Input.GetKey(KeyCode.Space)){        
            if (isledge1 == false && isledge2 == true && isgrounded == false)
            {
                GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
                climbPosition = GetComponent<Transform>().position;
                ishanging = true;
                animat.SetBool("Hanging", ishanging);
                animat.SetBool("Jumping", false);
            }
        }


        /*If player presses shift, player character starts to run or slows down to walk.*/
 
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (isrunning == false)
            {
                isrunning = true;              
                animat.SetFloat("Speed", speed);
                animat.SetBool("Running", isrunning);
            }else{
                isrunning = false;     
                animat.SetFloat("Speed", speed);
                animat.SetBool("Running", isrunning);
            }
        }
        Raycasting();
    }

    /*Method for climbing ledgeds. This method is called by animation directly. Animation itself is invoked by button 
     press*/
    void climbUp() {
        animat.SetBool("Climbing", false);
        if (facingRight == true)
            {
                Vector3 climbPosition = GetComponent<Transform>().position;
                GetComponent<Transform>().position = new Vector3(climbPosition.x, climbPosition.y + 0.6f, 0);
                rotator.transform.Rotate(Vector3.forward * -2.2f);
            }
            else
            {
                Vector3 climbPosition = GetComponent<Transform>().position;
                GetComponent<Transform>().position = new Vector3(climbPosition.x, climbPosition.y + 0.6f, 0);
                rotator.transform.Rotate(Vector3.forward * +2.2f);
            }
        GetComponent<Rigidbody2D>().constraints &= ~RigidbodyConstraints2D.FreezePositionX;
        GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
        GetComponent<Rigidbody2D>().constraints &= ~RigidbodyConstraints2D.FreezePositionY;
       
    }

    /*Method for climbing ledgeds down. This method is called by animation directly. Animation itself is invoked by button 
     press*/

    void climbDown() {
        animat.SetBool("Climb Down", false);
        climbPosition = GetComponent<Transform>().position;
            if (facingRight == true)
            {
                rotator.rotateLeft();
                rotator.rotateLeft();
            GetComponent<Transform>().position = new Vector3(climbPosition.x, climbPosition.y - 0.5f, 0);
            }
            else
            {
                rotator.rotateRight();
                rotator.rotateRight();
                GetComponent<Transform>().position = new Vector3(climbPosition.x, climbPosition.y - 0.5f, 0);
            }
        turnAround();      
    }

    /*Method for jumping. Specific animations call this jumping during animation itself*/
    void Jump() {
      
        if (isrunning == false || GetComponent<Rigidbody2D>().velocity.y ==0){          
            jumpHeight = new Vector3(0, 2.2f, 0);
            GetComponent<Rigidbody2D>().AddForce(jumpHeight, ForceMode2D.Impulse);        
        }
           if(isrunning==true){
            jumpHeight = new Vector3(0, 2.5f, 0);
            GetComponent<Rigidbody2D>().AddForce(jumpHeight, ForceMode2D.Impulse);
        }
        Debug.Log("Jump");
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
                if (item == "Plutonium stick")
                {
                    //open the door
                    Door.gameObject.SetActive(false);
                    Inventory.Remove("Plutonium stick");
                    itemText.text = "";
                }
            }
        }

    }

    void Raycasting()
    {
        //this is for recognising if the player is touching a wall from the left side or right
        //used to disable level rotation once the player is "stuck"

        //Debug.DrawLine(startPoint.position, endPointLeft.position, Color.cyan);//leftline
        Debug.DrawLine(startPoint.position, endPointRight.position, Color.red);//rightline
        rightTouchingWall = Physics2D.Linecast(startPoint.position, endPointRight.position, 1 << LayerMask.NameToLayer("Level"));

    }
    void ShootLaserGun()
    {
        //get mouse position
        //get gun position
        //draw raycast between them
        //Debug.DrawLine(laserGunPoint.position, Input.mousePosition, Color.red);

    }
}
