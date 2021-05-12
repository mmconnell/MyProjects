using Manager;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class MovmentScript : MonoBehaviour, I_Timed
{
    public Animator animator;
    float inputX;
    float inputY;
    float Horizontal_f;
    float Vertical_f;
    CharacterController characterController;

    int time;
    float reset;
    float resetTime;
    int comboNum;
    List<String> animationList = new List<String>(new String[] { "Slash", "SlashAgain" });

    Boolean mouseLeft;
    Boolean mouseRight;

    private Vector3 moveDirection = Vector3.zero;
    public float speed = 6.0f;
    public float gravity = 20.0f;

    public bool IsEnabled()
    {

        return true;
    }

    public bool IsTracked()
    {
        return false;
    }

    public void StartTracking()
    {

    }

    public void StopTracking()
    {

    }

    public void UpdateTime()
    {
        //Called every .1 seconds

    }


    // Start is called before the first frame update
    void Start()
    {
        //Get the animator
        animator = this.gameObject.GetComponent<Animator>();

        TimeRegistry.AddListener(this);
        characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {



        //Movment horizontal
        Horizontal_f = Input.GetAxis("Horizontal");
        animator.SetFloat("Horizontal_f", Horizontal_f);


        //Movment Vertical
        Vertical_f = Input.GetAxis("Vertical");
        animator.SetFloat("Vertical_f", Vertical_f);

        moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));
        moveDirection *= speed;

        moveDirection.y -= gravity * Time.deltaTime;
        characterController.Move(moveDirection * Time.deltaTime);

        //Two hit combo
        if (Input.GetButtonDown("Fire1") && comboNum < 2)
        {
            animator.SetTrigger(animationList[comboNum]);
            comboNum++;
            reset = 0f;

        }
        if (comboNum > 0)
        {
            reset += Time.deltaTime;

            if (reset > resetTime)
            {
                animator.SetTrigger("Reset");
                comboNum = 0;
            }
        }
        if (comboNum == 2)
        {
            resetTime = 2f;
            comboNum = 0;
        }
        else
        {
            resetTime = 1f;
        }


        //Attacl right mouse
        mouseRight = Input.GetButtonDown("Fire2");
        animator.SetBool("AttackHeavy", mouseRight);

    }
}
