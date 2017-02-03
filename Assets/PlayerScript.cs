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
    public float ledgeradius = 0.5f;
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


    // Use this for initialization
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
        InventoryCanvas.gameObject.SetActive(false);
        inventoryOpen = false;
        animat = GetComponent<Animator>();
        facingRight = true;
    }

    void FixedUpdate() {   
        isgrounded = Physics2D.OverlapCircle(groundCheck.position, groundradius, groundIs);
        animat.SetBool("Ground", isgrounded);
        animat.SetFloat("Speed", Mathf.Abs(GetComponent<Rigidbody2D>().velocity.y));
        if (GetComponent<Rigidbody2D>().velocity.y <-0.4f)
        {
            isfalling = true;
            animat.SetBool("Jumping", false);
            animat.SetBool("Falling",isfalling);
        }else {
            isfalling = false;
            animat.SetBool("Falling", isfalling);
        }
    }

    void OnCollisionEnter2D(Collision2D coll) {
        if (airtime > 2f) {
            GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
            animat.SetBool("Dead", true);
        }
    }
   
    // Update is called once per frame
    void Update(){
        if (isfalling==true)
            airtime += Time.deltaTime;
        else
            airtime = 0f;

        if (Input.GetKey(KeyCode.D) && ishanging == false)
        {        
            
            player.SetTrigger("Left");         
        }

        if (Input.GetKey(KeyCode.F))
        {
           // Code to interract with object like terminals
        }

        if (Input.GetKey(KeyCode.A) && ishanging==false)
        {     
            
            player.SetTrigger("Right");        
        }
       
        if (Input.GetKey(KeyCode.W) && ishanging == true) {
            ishanging = false;
            animat.SetBool("Hanging", ishanging);      
            if (facingRight == true) { 
                GetComponent<Rigidbody2D>().constraints &= ~RigidbodyConstraints2D.FreezePositionX;         
                GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
                GetComponent<Rigidbody2D>().constraints &= ~RigidbodyConstraints2D.FreezePositionY;                   
            }
        else {
                GetComponent<Rigidbody2D>().constraints &= ~RigidbodyConstraints2D.FreezePositionX;        
                GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
                GetComponent<Rigidbody2D>().constraints &= ~RigidbodyConstraints2D.FreezePositionY;                          
            }
        }

        if (Input.GetKey(KeyCode.S) && ishanging==true) {
            ishanging = false;
            animat.SetBool("Hanging", ishanging);
            GetComponent<Rigidbody2D>().constraints &= ~RigidbodyConstraints2D.FreezePositionY;
            GetComponent<Rigidbody2D>().constraints &= ~RigidbodyConstraints2D.FreezePositionX;
            isfalling = true;
            animat.SetBool("Falling", isfalling);
        }
        if (Input.GetKeyDown(KeyCode.S) && isgrounded==true)
        {
            Debug.Log("Climb");
            isledge3 = Physics2D.OverlapCircle(LedgeCheck3.position, ledgeradius, Ledge3);
            if (isledge3 == false) {
                climbPosition = GetComponent<Transform>().position;
                if(facingRight == true)
                    GetComponent<Transform>().position = new Vector3(climbPosition.x, climbPosition.y -1, 0);
                else
                    GetComponent<Transform>().position = new Vector3(climbPosition.x, climbPosition.y - 1, 0);
            }
        }

        if (Input.GetKeyDown(KeyCode.Space) && isgrounded==true)
        {        
            if (isrunning == false)
            {
                //Debug.Log("Jump");
                jumpHeight = new Vector3(0,1,0);
                GetComponent<Rigidbody2D>().AddForce(jumpHeight, ForceMode2D.Impulse);
                animat.SetBool("Jumping", true);
            }
            else {
                //Debug.Log("Runningjump");
                jumpHeight = new Vector3(0, 1.2f, 0);
                GetComponent<Rigidbody2D>().AddForce(jumpHeight, ForceMode2D.Impulse);
                animat.SetBool("Jumping", true);
            }
        }

        if (Input.GetKey(KeyCode.Space))
        {
            isledge1 = Physics2D.OverlapCircle(LedgeCheck1.position, ledgeradius, Ledge1);
            isledge2 = Physics2D.OverlapCircle(LedgeCheck2.position, ledgeradius, Ledge2);
            if (isledge1 == false && isledge2 == true && isgrounded == false)
            {
                GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
                climbPosition = GetComponent<Transform>().position;
                ishanging = true;
                animat.SetBool("Hanging", ishanging);
                animat.SetBool("Jumping", false);
            }
        }
 
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (isrunning == false)
            {
                isrunning = true;
                speed = 2f;
                animat.SetFloat("Speed", speed);
                animat.SetBool("Running", isrunning);
            }else{
                isrunning = false;
                speed = 1f;
                animat.SetFloat("Speed", speed);
                animat.SetBool("Running", isrunning);
            }
        }
        Raycasting();
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
