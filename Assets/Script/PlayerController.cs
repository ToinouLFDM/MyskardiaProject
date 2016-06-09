using UnityEngine;
using System.Collections;

[RequireComponent(typeof(PlayerMotor))]
public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float speed = 5f;
    
    [SerializeField]
    private float mouseSensivityNormal = 3f;

    [SerializeField]
    float mouseSensivityAiming = 1f;

    float mouseSensivity;

  

   

    private PlayerMotor motor;
  
 
    private static float initspeed ;
    void Start()
    {
        motor = GetComponent<PlayerMotor>();
   
        
        initspeed = speed;
        mouseSensivity = mouseSensivityNormal;
    }

    void Update()
    {
        speed = initspeed ;
        float movX = Input.GetAxisRaw("Horizontal");
        float movZ = Input.GetAxisRaw("Vertical");

        Vector3 moveHorizontal = transform.right * movX;
        Vector3 moveVertical = transform.forward * movZ;

        Vector3 velocity = (moveHorizontal + moveVertical).normalized * speed;

     

        //Apply
        motor.Move(velocity);

        float movY = Input.GetAxisRaw("Mouse X");
        Vector3 rotation = new Vector3(0f, movY, 0f) * mouseSensivity;

        motor.Rotate(rotation);

        float rotX = Input.GetAxisRaw("Mouse Y");
        float cameraRotationX = rotX * mouseSensivity; //new Vector3(rotX, 0f, 0f) * mouseSensivity;

        motor.RotateCamera(cameraRotationX);

        Vector3 _thruster = Vector3.zero;
        if (Input.GetButton("Jump")  )
        {
            _thruster = Vector3.up ;
           
        }

        if (Input.GetButton("Aim"))
        {
            motor.SetAim(true);
            mouseSensivity = mouseSensivityAiming;
        }
        else
        {
            motor.SetAim(false);
            mouseSensivity = mouseSensivityNormal;
        }

        motor.ApplyThruster(_thruster);
    }
}
