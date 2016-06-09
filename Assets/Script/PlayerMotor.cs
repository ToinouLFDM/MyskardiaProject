using UnityEngine;
using System.Collections;
using System;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMotor : MonoBehaviour
{
    [SerializeField]
    private Camera PlayerView;

    [SerializeField]
    GameObject[] PlayerArms;

    [SerializeField]
    GameObject pov;

    private Rigidbody rb;
    private Vector3 velocity = Vector3.zero;
    private Vector3 rotation = Vector3.zero;
    private float cameraRotationX = 0f;
    private float currentCameraRotationX = 0f;
    private Vector3 thruster = Vector3.zero;

    [SerializeField]
    private float maxCameraRotation = 85f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    public void Move(Vector3 _velocity)
    {
        velocity = _velocity;
    }
    public void Rotate(Vector3 _rotation)
    {
        rotation = _rotation;
    }
    public void RotateCamera(float _cameraRotationX)
    {
        cameraRotationX = _cameraRotationX;
    }
    public void ApplyThruster(Vector3 _thruster)
    {
        thruster = _thruster;
    }

    void FixedUpdate()
    {
        PerformMove();
        PerformRotation();
    }

    private void PerformMove()
    {
        if(velocity != Vector3.zero && thruster == Vector3.zero)
        {
            rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);
        }

        if (velocity != Vector3.zero && thruster != Vector3.zero)
        {
            rb.AddForce(velocity * 0.8f * Time.fixedDeltaTime, ForceMode.Acceleration);
        }

        if(thruster != Vector3.zero)
        {
            rb.AddForce((thruster * 0.8f + 75 * velocity) * Time.fixedDeltaTime, ForceMode.Acceleration);
        }
    }

  
    public void PerformRotation()
    {
        rb.MoveRotation(rb.rotation * Quaternion.Euler(rotation));
        if (PlayerView != null)
        {
            currentCameraRotationX -= cameraRotationX;
            currentCameraRotationX = Mathf.Clamp(currentCameraRotationX, -maxCameraRotation, maxCameraRotation);

            PlayerView.transform.localEulerAngles = new Vector3(currentCameraRotationX, 0f, 0f);

            //foreach (GameObject arm in PlayerArms)
            //{
                //arm.transform.localEulerAngles = new Vector3(arm.transform.localEulerAngles.x, arm.transform.localEulerAngles.y, arm.transform.localEulerAngles.z + currentCameraRotationX);
            //}
        }
    }

    public void SetAim(bool aim)
    {
        if (aim)
        {
            PlayerView.fieldOfView = 10f;
        }
        else
        {
            if (PlayerView.fieldOfView != 60f)
                PlayerView.fieldOfView = 60f;
        }
    }
}
