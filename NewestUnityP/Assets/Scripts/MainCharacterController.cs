using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniversalMobileController;

public class MainCharacterController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2;
    [SerializeField] private float turnSpeed = 200;

    [SerializeField] private Animator animator = default;
    [SerializeField] private Rigidbody rigidBody = default;

    [SerializeField] private FloatingJoyStick joyStick = default;
    [SerializeField] private float minValuableInput = 0.2f;

    private Vector2 joyStickInput = Vector2.zero;
    private float currentSpeed;

    private void Start()
    {
        animator.SetBool("Grounded", true);
    }

    private void Update()
    {
        joyStickInput = joyStick.GetHorizontalAndVerticalValue();
    }

    private void FixedUpdate()
    {
        MoveCharacter();
    }

    private void MoveCharacter()
    {
        if (joyStickInput.magnitude > minValuableInput)
        {
            float angle = Vector2.SignedAngle(joyStickInput, Vector2.up);
            Quaternion rotate = Quaternion.Euler(0, angle, 0);
            rigidBody.rotation = Quaternion.RotateTowards(transform.localRotation, rotate, turnSpeed * Time.fixedDeltaTime);
        }

        currentSpeed = moveSpeed * joyStickInput.magnitude;
        animator.SetFloat("MoveSpeed", currentSpeed);
        rigidBody.velocity = transform.forward * currentSpeed;
    }
}
