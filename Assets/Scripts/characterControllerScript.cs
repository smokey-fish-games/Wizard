using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class characterControllerScript : MonoBehaviour
{
    public float mouseSensitivity = 100f;
    public float speed = 12f;
    public float gravityConstant = -9.81f;
    public float groundDistance = 0.4f;
    public float jumpHeight = 2f;
    public float pickupLength = 1.4f;

    private Transform playerTrans;
    private Transform cameraTrans;
    private Transform groundCheck;

    public Transform objectHoldingPoint;

    public LayerMask groundmask;
    private CharacterController cc;

    Vector3 velocity;
    bool isGrounded;

    float xRot = 0f;

    bool holdingObject = false;
    public GameObject heldObject;

    // Start is called before the first frame update
    void Start()
    {
        playerTrans = transform;
        cameraTrans = GetComponentInChildren<Camera>().transform;
        cc = GetComponent<CharacterController>();

        foreach(Transform t in GetComponentsInChildren<Transform>())
        {
            if(t.name == "_groundcheck")
            {
                groundCheck = t;
            }
        }

        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        checkpickup();
        if (holdingObject)
        {
            heldObject.transform.position = objectHoldingPoint.transform.position;
            heldObject.transform.rotation = objectHoldingPoint.transform.rotation;
        }
        if(Input.GetKeyDown("e") && holdingObject)
        {
            heldObject.GetComponent<objectController>().emptyBottle();
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

        //    Debug.DrawRay(ray.origin, ray.direction * pickupLength, Color.red, 5f);
            RaycastHit hit;

            if (holdingObject)
            {
                if(Physics.Raycast(ray, out hit, pickupLength))
                {
                    if (hit.collider.gameObject.tag == "cauldron")
                    {
                        heldObject.GetComponent<objectController>().fillBottle(hit.collider.gameObject.GetComponent<CauldronController>().getContents());
                    }
                    else
                    {
                        holdingObject = false;
                    }
                }
                else
                {
                    holdingObject = false;
                }

                if (!holdingObject)
                {
                    // putdown
                    heldObject.GetComponent<Collider>().enabled = true;
                    heldObject.GetComponent<Rigidbody>().isKinematic = false;
                    heldObject = null;
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
}
