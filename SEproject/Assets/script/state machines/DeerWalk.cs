using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System.Security.Cryptography;

public class DeerWalk : IDeerState
{
    protected DeerStateMachine deer;
    private Rigidbody rb;

    private float speed = 2f;
    private float maxSpeed = 2f;
    private float rotationSpeed = 20f;

    public DeerWalk(DeerStateMachine deer)
    {
        this.deer = deer;
        rb = deer.rb;
        rb.useGravity = true;
    }

    public void handleGravity()
    {
        //uses rigidbody gravity instead
    }

    public void handleForward()
    {
        Vector3 cameraForward = GetCameraForwardDirection();
        cameraForward.y = 0f;
        cameraForward.Normalize();
        rb.AddForce(new Vector3(cameraForward.x, -0.2f, cameraForward.z) * speed, ForceMode.Impulse);

        if (rb.velocity.magnitude > maxSpeed)
        {
            rb.velocity = rb.velocity.normalized * maxSpeed;
        }

        rb.rotation = quatFacade.smoothRotate(cameraForward, rb.rotation, rotationSpeed * Time.deltaTime);

    }
    public void handleBack()
    {
        Vector3 cameraForward = GetCameraForwardDirection();
        cameraForward.y = 0f;
        cameraForward.Normalize();
        rb.AddForce(new Vector3(-cameraForward.x, -0.2f, -cameraForward.z) * speed, ForceMode.Impulse);

        if (rb.velocity.magnitude > maxSpeed)
        {
            rb.velocity = rb.velocity.normalized * maxSpeed;
        }

        rb.rotation = quatFacade.smoothRotate(-cameraForward, rb.rotation, rotationSpeed * Time.deltaTime);
    }
    public void handleLeft()
    {
        Vector3 cameraRight = GetCameraRightDirection();
        cameraRight.y = 0f;
        cameraRight.Normalize();
        rb.AddForce(new Vector3(cameraRight.x, -0.2f, cameraRight.z) * speed, ForceMode.Impulse);

        if (rb.velocity.magnitude > maxSpeed)
        {
            rb.velocity = rb.velocity.normalized * maxSpeed;
        }
        rb.rotation = quatFacade.smoothRotate(cameraRight, rb.rotation, rotationSpeed * Time.deltaTime);
    }
    public void handleRight()
    {
        Vector3 cameraRight = GetCameraRightDirection();
        cameraRight.y = 0f;
        cameraRight.Normalize();
        rb.AddForce(new Vector3(-cameraRight.x, -0.2f, -cameraRight.z) * speed, ForceMode.Impulse);

        if (rb.velocity.magnitude > maxSpeed)
        {
            rb.velocity = rb.velocity.normalized * maxSpeed;
        }
        rb.rotation = quatFacade.smoothRotate(-cameraRight, rb.rotation, rotationSpeed * Time.deltaTime);
    }

    public void handleSpace()
    {
        deer.setState(new DeerJump(deer, speed));
    }
    public void handleShift()
    {
        deer.setState(new DeerSprint(deer));
    }
    public void advanceState()
    {

    }

    private Vector3 GetCameraForwardDirection()
    {
        Debug.Log(deer);
        CinemachineFreeLook freeLookCamera = deer
            .GameManager
            .GetCameraTransform()
            .GetComponent<CinemachineFreeLook>();

        Vector3 lookAtPosition = freeLookCamera.LookAt.position;
        Vector3 cameraPosition = freeLookCamera.transform.position;

        return (lookAtPosition - cameraPosition).normalized;
    }

    private Vector3 GetCameraRightDirection()
    {
        CinemachineFreeLook freeLookCamera = deer.GameManager.GetCameraTransform().GetComponent<CinemachineFreeLook>();

        Vector3 cameraForward = GetCameraForwardDirection();
        Vector3 cameraRight = Vector3.Cross(cameraForward, Vector3.up);

        return cameraRight.normalized;
    }
}
