using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListOfAnimationOfWeaponsAndTools : MonoBehaviour
{
    public Animator animator;
    Vector2 motionVector;
    public Vector2 lastMotionVector;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        motionVector.x = horizontal;
        motionVector.y = vertical;

        animator.SetFloat("horizontal", horizontal);
        animator.SetFloat("vertical", vertical);

        if (horizontal != 0 || vertical != 0)
        {
            lastMotionVector = new Vector2(
                horizontal,
                vertical
                ).normalized;
            animator.SetFloat("lastHorizontal", horizontal);
            animator.SetFloat("lastVertical", vertical);
        }
    }
}
