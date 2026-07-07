using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    //movement stuff
    public float moveSpeed = 5f;
    public float jumpForce = 8f;

    //ground check stuff
    public Transform groundCheck;
    public float groundCheckRadius = 0.15f;
    public LayerMask groundLayer; 

    //water stuff
    public int maxWater = 5;
    public int currentWater = 5;
    public Transform sprayPoint;      
    public float sprayRange = 1.2f;
    public LayerMask fireLayer;       
    public bool debugSpray = true;     

    //carrying animal slot above player head
    public Transform carrySlot;  

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private bool isGrounded;
    private bool facingRight = true;
    private List<GameObject> carriedAnimals = new List<GameObject>();

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        currentWater = maxWater;
    }

    void Update()
    {
        HandleMovement();
        HandleJump();
        HandleSpray();
        UpdateGroundCheck();
    }

    void UpdateGroundCheck()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
    }

    void HandleMovement()
    {
        float h = Input.GetAxisRaw("Horizontal"); // a/d or left/right arrows
        rb.linearVelocity = new Vector2(h * moveSpeed, rb.linearVelocity.y);

        if (h > 0 && !facingRight) Flip();
        else if (h < 0 && facingRight) Flip();
    }

    void HandleJump()
    {
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
    }

    void HandleSpray()
    {
        bool ctrlHeld = Input.GetKey(KeyCode.LeftControl);
        bool fire1Held = Input.GetButton("Fire1");
        bool sprayHeld = ctrlHeld || fire1Held;

        if (debugSpray && (ctrlHeld || fire1Held))
        {
            Debug.Log($"[Spray] ctrlHeld={ctrlHeld} fire1Held={fire1Held} currentWater={currentWater}");
        }

        if (sprayHeld && currentWater > 0)
        {
            Collider2D[] hits = Physics2D.OverlapCircleAll(sprayPoint.position, sprayRange, fireLayer);
            if (debugSpray)
            {
                Debug.Log($"[Spray] OverlapCircleAll found {hits.Length} collider(s) on fireLayer at {sprayPoint.position}, radius {sprayRange}");
            }
            if (hits.Length > 0)
            {
                currentWater = Mathf.Max(0, currentWater - 1);
                foreach (var hit in hits)
                {
                    FireHazard fire = hit.GetComponentInParent<FireHazard>();
                    if (fire != null)
                    {
                        if (debugSpray) Debug.Log($"[Spray] Hit FireHazard on '{fire.gameObject.name}', healthPoints before hit = {fire.healthPoints}");
                        fire.ExtinguishHit();
                    }
                    else if (debugSpray)
                    {
                        Debug.LogWarning($"[Spray] Collider '{hit.gameObject.name}' is on the Fire layer but has no FireHazard component on it or any parent.", hit.gameObject);
                    }
                }
            }
        }
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    public void RefillWater(int amount)
    {
        currentWater = Mathf.Min(maxWater, currentWater + amount);
    }

    public void PickUpAnimal(GameObject animal)
    {
        carriedAnimals.Add(animal);
        animal.transform.SetParent(carrySlot);
     
        animal.transform.localPosition = new Vector3(0, 0.25f * carriedAnimals.Count, 0);
        Collider2D col = animal.GetComponent<Collider2D>();
        if (col != null) col.enabled = false;
    }

    public int DropOffAnimals()
    {
        int count = carriedAnimals.Count;
        foreach (var a in carriedAnimals)
        {
            Destroy(a);
        }
        carriedAnimals.Clear();
        return count;
    }

    public int CarriedCount => carriedAnimals.Count;

    void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
        if (sprayPoint != null)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(sprayPoint.position, sprayRange);
        }
    }
}