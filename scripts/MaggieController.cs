using System;
using UnityEngine;
using UnityEngine.UI;

/*
    Notes:

    clamping now tracks what it's clamped to in the "clampedto" variable.
    
    MoveWith() is a new experimental function to try to let Maggie move with clampedto, or with whatever she's standing on

    Clamping, and attraction in general, need to be done by calling the attraction / clamping functions in the classes of the metal objects. This will allow different things to respond differently to attraction. 
    For example, attraction to a platform will pull Maggie to the platform, while attraction to a pick up will pull the pick up to Maggie.

    Pick up item for polarity reversal.

*/
namespace UnityStandardAssets._2D
{
    public class MaggieController : damageable
    {
        // 
        public Text data;
        public float magRepDiv = 70f;
        private float testDist = 0f;
        //

        [SerializeField]
        private float m_MaxSpeed = 10f;                    // The fastest the player can travel in the x axis.
        [SerializeField]
        private float m_JumpForce = 400f;                  // Amount of force added when the player jumps.
        [Range(0, 1)]
        [SerializeField]
        private float m_CrouchSpeed = .36f;  // Amount of maxSpeed applied to crouching movement. 1 = 100%
        [SerializeField]
        private bool m_AirControl = true;                 // Whether or not a player can steer while jumping;
        [SerializeField]
        private LayerMask m_WhatIsGround;                  // A mask determining what is ground to the character
        
        public Transform m_GroundCheck;    // A position marking where to check if the player is grounded.
        const float k_GroundedRadius = .2f; // Radius of the overlap circle to determine if grounded
        private bool m_Grounded;            // Whether or not the player is grounded.
        private Transform m_CeilingCheck;   // A position marking where to check for ceilings
        const float k_CeilingRadius = .2f; // Radius of the overlap circle to determine if the player can stand up
        private Animator m_Anim;            // Reference to the player's animator component.
        private Rigidbody2D m_Rigidbody2D;
        private bool m_FacingRight = true;  // For determining which way the player is currently facing.

        public bool startFaceLeft = false;       // she faces right at start by default. This is true when she should start facing left.

        public float magHeadRange = 3f;          // variables for the head magnet
        public float magHeadForce = 5f;
        public float minHeadDist = 1f;           // distance at which head magnet is at maximum power
        public float minPulseDist = 5f;          // distance at which the pulse is at maximum power
        public float pulseForce = 100f;
        int magMask;                             // layermask that magnet affects
	int bombBotMask;			 // layermask for bomb bot
        public bool magPause = false;            // true to escape head magnet pull
        float magTimer = 0f;                     // timer for magPause
        public float magPauseLength = 0.25f;     // length of magPause
        public bool pulse;
        private bool pulseReady = true;


        public GameObject gun;                  // the magnet gun
        public Vector3 forward;

        public bool clamped = false;             // whether or not Maggie is clamped to a platform

        public int polarity = 1;                 // 1 for attract, -1 for repel. It should never be equal to any value other than 1 or -1.
        public bool reverseEnabled = false;      // if true, Maggie can reverse polarity with a button

        public bool readyForBoots = true;       // used in the reversal input section of Update()
        public bool bootsActive = false;        // true is the magboots are active
        public float magBootRange = 11.62f;
        public float magBootForce = 40f;
        public float magBootRepForce = 30f;
        public float minBootDist = 3f;
        public float magBootHover = 3.2f;
        public float minGrav = -0.15f;

        public GameObject clampedto;

        override public void damage() {
            // put damage code here *********
        }

        private void Awake()
        {
            // Setting up references.
            m_GroundCheck = transform.Find("GroundCheck");
            m_CeilingCheck = transform.Find("CeilingCheck");
            m_Anim = GetComponent<Animator>();
            m_Rigidbody2D = GetComponent<Rigidbody2D>();
            magMask = LayerMask.GetMask("metal");                       // sets layer the magnets can interact with
			bombBotMask = LayerMask.GetMask("bomb bot");
            if (startFaceLeft == true) { Flip(); }                       // starts Maggie facing left instead of right.
        }

        private void Update()
        {
            forward = gun.transform.right; 
            if(transform.localScale.x < 0) { forward = forward * -1; }
            if (reverseEnabled == true && Input.GetButtonDown("Reverse")) { ReversePolarity(); }        // lets Maggie reverse polarity with input if it's enabled.

            magPause = false;                                                            //* magePause is normally false
            if (magTimer > 0f)                                                              // but if the timer is more than zero,
            {
                magPause = true;                                                            // magPause is true and the timer counts down
                magTimer = magTimer - Time.deltaTime;                                    //**
            }
            //* This section of Update() handles the input for the magnet boots.
            if (polarity > 0f && Input.GetAxis("Vertical") >= 0f && !clamped) { bootsActive = false; }             // if polarity is positive, boots aren't active while the button isn't held down
            if (Input.GetAxis("Vertical") >= 0f) { readyForBoots = true; }                          // it's only ready for input if the vertical axis has been 0 or more
            if (readyForBoots && Input.GetAxis("Vertical") < 0f)                                                    // if the vertical axis is pressed down,
            {                                                                                           //  
                readyForBoots = false;                                                                  // no longer ready for input
                if (bootsActive) { bootsActive = false; }                                               // if boots were active they now aren't
                else { bootsActive = true; }                                                            // if boots weren't active they now are
            }                                                                                   //**
        }

        private void FixedUpdate()
        {
            m_Grounded = false;

            // The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
            // This can be done using layers instead but Sample Assets will not overwrite your project settings.
            Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i].gameObject != gameObject)
                {
                    m_Grounded = true;
                    pulseReady = true;
                    if (colliders[i].gameObject.GetComponent<Rigidbody2D>() && !clamped)
                    {
                        MoveWith(colliders[i].gameObject);
                    }
                }
            }
            m_Anim.SetBool("Ground", m_Grounded);

            // Set the vertical animation
            m_Anim.SetFloat("vSpeed", m_Rigidbody2D.velocity.y);



            // ************************************ MAG GUN *****************************************
            clamped = false;
            if (!magPause)
            {
                RaycastHit2D hit = Physics2D.Raycast(transform.position, forward, magHeadRange, magMask);
                if(hit)
                {
                    float distance = Mathf.Sqrt(Mathf.Pow(Mathf.Abs(transform.position.x - hit.point.x), 2) + Mathf.Pow(Mathf.Abs(transform.position.y - hit.point.y), 2) );
                    if(distance < 1.0f && polarity > 0)
                    {
                        clamped = true;
                        clampedto = hit.collider.gameObject;
                    }
                    if (distance < minHeadDist) { distance = minHeadDist; }
                    GetComponent<Rigidbody2D>().AddForce(polarity * forward * magHeadForce / distance);
                    if (!clamped)
                    {
                        // if click, add lots of force in the direction. This is the pulse.
                        if (pulseReady)
                        {
                            if(pulse)
                            {
                                if(distance < minPulseDist) { distance = minPulseDist; }
                                GetComponent<Rigidbody2D>().AddForce(polarity * forward * pulseForce / distance);
                                pulseReady = false;
                            }
                        }
                    }
                }
            }

	    // tells a bomb bot to lock on if Maggie points her magnet at it
	    if (!magPause) {
	        RaycastHit2D botHit = Physics2D.Raycast (transform.position, forward, magHeadRange, bombBotMask);   // *** Potential problem: This could allow a bombbot to detect the magnet through a wall.
		    if (botHit) {
	                botHit.collider.SendMessage ("LockOn");
	            }
	    }
            


            // ************************************ MAG HEAD ****************************************
            /*
            clamped = false;
            if (!magPause)              //  if the magnet is paused, none of this stuff happens.
            {
                RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.up, magHeadRange, magMask);
                if (hit && !bootsActive)
                {
                    float distance = Mathf.Abs(transform.position.y - hit.point.y);

                    if (distance < 0.65f && polarity > 0) {
                        clamped = true;
                        clampedto = hit.collider.gameObject;
                    }
                    if (distance < minHeadDist) { distance = minHeadDist; }                                            // avoid divide by zero error
                    GetComponent<Rigidbody2D>().AddForce(polarity * Vector2.up * magHeadForce / distance);
                }
                // MAG BOOTS
               GetComponent<Rigidbody2D>().gravityScale = 3f;                                                          // this is needed because of how magboot repulsion works.
                if (bootsActive)                 // MAG BOOT WHILE ATTRACTING
                {
                    hit = Physics2D.Raycast(transform.position, Vector2.down, magBootRange, magMask);
                    if (hit & polarity == 1)
                    {
                        float distance = Mathf.Abs(transform.position.y - hit.point.y);

                        if (distance < 0.65f && polarity > 0) {
                            clamped = true;
                            clampedto = hit.collider.gameObject;
                        }
                        if (distance < minBootDist) { distance = minBootDist; }                                            // avoid divide by zero error

                        GetComponent<Rigidbody2D>().AddForce(polarity * Vector2.down * magBootForce / ( 70 * distance));

                        //
                    }
                    else if (hit)               // MAG BOOT WHILE REPELING
                    {
                        
                        float distance = Mathf.Abs(transform.position.y - hit.point.y);
                        // GetComponent<Rigidbody2D>().gravityScale = (Mathf.Pow((distance - magBootHover), 3) / (magRepDiv * distance));
                        if (distance < magBootHover) { GetComponent<Rigidbody2D>().gravityScale = ((2f / (1f + Mathf.Pow(2.718f, 3f * (1 - distance)))) - 2.2f); }
                        if(distance >= magBootHover){ GetComponent<Rigidbody2D>().gravityScale = ((6f / (1f + Mathf.Pow(2.718f, 0.2f *(3 - distance)))) - 3f);}

                        if(GetComponent<Rigidbody2D>().gravityScale < minGrav) { GetComponent<Rigidbody2D>().gravityScale = minGrav; }
                        if(distance > 1.172f)
                        {
                            if(distance < 2.686f)
                            {
                                GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x, (GetComponent<Rigidbody2D>().velocity.y * (Mathf.Pow((distance - 2.1f), 3f) + 0.8f)));
                            }
                        }
                        else
                        {
                            GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x, 0f);
                        }

                        if(distance < 2f)
                        {
                            GetComponent<Rigidbody2D>().AddForce(magBootForce * Vector2.up);
                        }

                        //
                        testDist = distance;
                        //

                    }
                }*/
            // *******************************************************************************************
            if (clamped)
            {
                m_Anim.SetFloat("Speed", 0f);                                                       // Maggie isn't walking if she's clamped
                GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;         // freeze Maggie's movement
                gun.GetComponent<MouseLook>().enabled = false;
                //MoveWith(clampedto);
            }
            else
            {
                GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;    // unfreeze Maggie's movement
                gun.GetComponent<MouseLook>().enabled = true;
                clampedto = null;
            }
        }
        //
        /*data.text = "distance: " + testDist + "\ngravity: " + GetComponent<Rigidbody2D>().gravityScale +
            "\nvelocity: " + GetComponent<Rigidbody2D>().velocity.y;*/
        //



        public void MoveWith(GameObject v)
        {
            if (v.GetComponent<Rigidbody2D>()) { m_Rigidbody2D.velocity += v.GetComponent<Rigidbody2D>().velocity; }
            else if (v.GetComponentInParent<Rigidbody2D>()) { m_Rigidbody2D.velocity += v.GetComponentInParent<Rigidbody2D>().velocity; }
        }

        public void Move(float move, bool crouch, bool jump)
        {
            // If crouching, check to see if the character can stand up
            /* if (!crouch && m_Anim.GetBool("Crouch"))                                                      // crouching is not needed in this game
             {
                 // If the character has a ceiling preventing them from standing up, keep them crouching
                 if (Physics2D.OverlapCircle(m_CeilingCheck.position, k_CeilingRadius, m_WhatIsGround))
                 {
                     crouch = true;
                 }
             }

             // Set whether or not the character is crouching in the animator
             m_Anim.SetBool("Crouch", crouch);
             */
            //only control the player if grounded or airControl is turned on
            if (m_Grounded || m_AirControl)
            {
                if (clamped == false)   // and if Maggie isn't clamped to a platform
                {
                    // Reduce the speed if crouching by the crouchSpeed multiplier
                    move = (crouch ? move * m_CrouchSpeed : move);

                    // The Speed animator parameter is set to the absolute value of the horizontal input.
                    m_Anim.SetFloat("Speed", Mathf.Abs(move));

                    // Move the character
                    m_Rigidbody2D.velocity = new Vector2(move * m_MaxSpeed, m_Rigidbody2D.velocity.y);

                    // If the input is moving the player right and the player is facing left...
                    if (move > 0 && !m_FacingRight)
                    {
                        // ... flip the player.
                        Flip();
                    }
                    // Otherwise if the input is moving the player left and the player is facing right...
                    else if (move < 0 && m_FacingRight)
                    {
                        // ... flip the player.
                        Flip();
                    }
                }
                else
                {
                    GetComponent<Rigidbody2D>().velocity = new Vector2(0, GetComponent<Rigidbody2D>().velocity.y);
                }
            }
            // If the player should jump...
            if (m_Grounded && jump && m_Anim.GetBool("Ground"))
            {
                // Add a vertical force to the player.
                m_Grounded = false;
                m_Anim.SetBool("Ground", false);
                if (!clamped) { m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce)); }
            }
            // If the player should jump while clamped...
            if (clamped && (jump || pulse))
            {
                clamped = false;                                                                    // unclamp her
                GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;    // unfreeze Maggie's movement
                magTimer = magPauseLength;                                                          // temporarily turn off the magnet
                if (jump) { m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce)); }                 // jump!
                pulseReady = true;                                                                  // Maggie can use the pulse again
            }
        }


        private void Flip()
        {
            // Switch the way the player is labelled as facing.
            m_FacingRight = !m_FacingRight;

            // Multiply the player's x local scale by -1.
            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
        }



        private void ReversePolarity()
        {
            polarity = polarity * (-1);
        }

        private void OnCollisionEnter2D(Collision2D other)      //on collision with a moving platform, maggie will become
        {                                                       //a child of moving platform so she will collide with it
            if (other.transform.tag == "MovingPlatform")
            {
                transform.parent = other.transform;
            }
        }

        private void OnCollisionExit2D(Collision2D other)       //When maggie leaves of the moving platform, she reverts back
        {                                                       //and is no longer a child of moving platform so she can move at will

            if (other.transform.tag == "MovingPlatform")
            {
                transform.parent = null;
            }
        }
    }
}
