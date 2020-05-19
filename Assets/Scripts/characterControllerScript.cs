using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class characterControllerScript : IEffectable
{
    public float mouseSensitivity = 100f;
    public float Defaultspeed = 6f;
    float speed;
    public float DefaultgravityConstant = -9.81f;
    float gravityConstant;
    public float groundDistance = 0.4f;
    public float DefaultjumpHeight = 1f;
    float jumpHeight = 2f;
    public float pickupLength = 1.4f;
    Vector3 DefaultScale;

    private Transform playerTrans;
    private Transform cameraTrans;
    private Transform groundCheck;

    public Transform objectHoldingPoint;

    public LayerMask groundmask;
    private CharacterController cc;
    Rigidbody rigid;
    public Collider col;

    Vector3 velocity;
    bool isGrounded;
    bool killed = false;
    bool controlLocked = false;

    float xRot = 0f;

    bool holdingObject = false;
    public GameObject heldObject;

    // Start is called before the first frame update
    void Start()
    {
        MaxHP = 100;
        MaxMana = 100;
        MaxStamina = 100;
        CurrentHP = 100;
        gravityConstant = DefaultgravityConstant;
        speed = Defaultspeed;
        DefaultScale = transform.localScale;
        jumpHeight = DefaultjumpHeight;


        CurrentMana = MaxMana;
        CurrentStamina = MaxStamina;
        playerTrans = transform;
        cameraTrans = GetComponentInChildren<Camera>().transform;
        cc = GetComponent<CharacterController>();
        rigid = GetComponent<Rigidbody>();
        rigid.useGravity = false;
        col.enabled = false;
        cc.enabled = true;

        foreach (Transform t in GetComponentsInChildren<Transform>())
        {
            if(t.name == "_groundcheck")
            {
                groundCheck = t;
            }
        }

        Cursor.lockState = CursorLockMode.Locked;
        GameEvents.current.switchControlLock += onSwitchControls;
    }

    // Update is called once per frame
    void Update()
    {
        if (killed)
        {
            // respawning
            if (Input.GetMouseButtonDown(0))
            {
                GameEvents.current.PlayerDeath();
            }
        } 
        else
        {
            if (CurrentHP <= 0)
            {
                kill();
            }
            if (!controlLocked)
            {
                Move();
                checkpickup();
                if (holdingObject)
                {
                    heldObject.transform.position = objectHoldingPoint.transform.position;
                    heldObject.transform.rotation = objectHoldingPoint.transform.rotation;
                }
                // TODO change this to interact or unity input system
                if (Input.GetKeyDown("e") && holdingObject)
                {
                    potionController p = heldObject.GetComponent<potionController>();
                    p.Drink(this);
                }
            }
            else
            {
                cc.Move(new Vector3(0,0,0));
            }
        }
    }

    private void Move()
    {
        // Check for falling
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundmask);

        if(isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        // Mouse Look
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRot -= mouseY;
        xRot = Mathf.Clamp(xRot, -90f, 90f);

        cameraTrans.localRotation = Quaternion.Euler(xRot, 0f, 0f);
        playerTrans.Rotate(Vector3.up * mouseX);

        // Character Moving
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;
        cc.Move(move * speed * Time.deltaTime);

        // Jump
        if(Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravityConstant);
        }


        // Falling
        velocity.y += gravityConstant * Time.deltaTime;

        cc.Move(velocity * Time.deltaTime);
    }

    void checkpickup()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 origin = new Vector3(0.5f, 0.5f, 0f);
            Ray ray = Camera.main.ViewportPointToRay(origin);

            RaycastHit hit;

            if (holdingObject)
            {
                if(Physics.Raycast(ray, out hit, pickupLength))
                {
                    if (hit.collider.gameObject.tag == "cauldron")
                    {
                        heldObject.GetComponent<potionController>().setProperty("contents", hit.collider.gameObject.GetComponent<CauldronController>().getContents().ID.ToString());
                    }
                    else
                    {
                        DropObject();
                    }
                }
                else
                {
                    DropObject();
                }
            }
            else
            {
                if(Physics.Raycast(ray, out hit, pickupLength))
                {
               //     Debug.Log("HIT! " + hit.collider.gameObject.name + " Distance = " + hit.distance);
                    if(hit.collider.gameObject.tag == "holdable")
                    {
                        GameObject holdingNow = hit.collider.gameObject;
                        holdingNow.GetComponent<Collider>().enabled = false;
                        holdingNow.GetComponent<Rigidbody>().isKinematic = true;
                        heldObject = holdingNow;
                        holdingObject = true;
                    }
                }
            }

        }
    }

    public void onSwitchControls()
    {
        controlLocked = !controlLocked;
    }

    public void kill()
    {
        if (!killed)
        {
            killed = true;
            DropObject();
            rigid.useGravity = true;
            col.enabled = true;
            cc.enabled = false;
            rigid.AddForceAtPosition(new Vector3(-1, -1, -1), new Vector3(1, 1, 1));
            rigid.MovePosition(Vector3.forward);
        }
    }

    void DropObject()
    {
        if (holdingObject)
        {
            // putdown
            holdingObject = false;
            heldObject.GetComponent<Collider>().enabled = true;
            heldObject.GetComponent<Rigidbody>().isKinematic = false;
            heldObject = null;
        }
    }

    public override Transform GetTransform()
    {
        return transform;
    }

    public override Renderer GetRenderer()
    {
        return GetComponent<Renderer>();
    }

    public override float GetSpeed()
    {
        return speed;
    }

    public override float GetGravity()
    {
        return gravityConstant;
    }


    public override void SetTransform(Transform t)
    {
        this.transform.position = t.position;
        this.transform.localScale = t.localScale;
        this.transform.rotation = t.rotation;
    }

    public override void SetRenderer(Renderer r)
    {
        Renderer rOld = GetComponent<Renderer>();
        rOld = r;
    }

    public override void SetSpeed(float newspeed)
    {
        speed = newspeed;
    }

    public override void SetGravity(float newgravity)
    {
        gravityConstant = newgravity;
    }

    public override Vector3 GetDefaultScale()
    {
        return DefaultScale;
    }

    public override float GetDefaultSpeed()
    {
        return Defaultspeed;
    }

    public override float GetDefaultGravity()
    {
        return DefaultgravityConstant;
    }

    public override float GetDefaultJumpHeight()
    {
        return DefaultjumpHeight;
    }

    public override void SetJumpHeight(float height)
    {
        jumpHeight = height;
    }
}
