using UnityEngine;

public class Collectable : MonoBehaviour
{
    [SerializeField] private static float followSpeed = 9f;
    [SerializeField] private float followOffset = 1.5f;
    [SerializeField] private float wanderSpeed = 2f;
    [SerializeField] private float separationDistance = 1f;
    [SerializeField] private float separationForce = 3f;

    private bool isFollowing = false;
    private Transform playerTransform;
    private float wanderAngle;
    private Vector3 velocity;
    private PlayerMovement playerMovement;

    void Start()
    {
        wanderAngle = Random.Range(0f, 360f);
    }

    void Update()
    {
        if (!isFollowing) { return; }

        //calculating the flocking effect
        wanderAngle += wanderSpeed * Time.deltaTime * Random.Range(-1f, 1f);

        //how far the target is from the player
        Vector3 offset = new Vector3(Mathf.Cos(wanderAngle * Mathf.Deg2Rad), Mathf.Sin(wanderAngle * Mathf.Deg2Rad), 0f) * followOffset;

        //where they travel to to "wander"
        Vector3 targetPos = playerTransform.position + offset;

        //seperating from other collectables
        Vector3 seperation = GetSeparation();

        //actually moving the things
        Vector3 flocking = (targetPos - transform.position).normalized * followSpeed;
        Vector3 desired = flocking + seperation * separationForce;
        velocity = Vector3.Lerp(velocity, desired, Time.deltaTime * 5f);
        transform.position += velocity * Time.deltaTime;
    }

    private Vector3 GetSeparation()
    {
        //first set seperation force to 0
        Vector3 separationResult = Vector3.zero;

        //get each nearby thing
        Collider2D[] neighbors = Physics2D.OverlapCircleAll(transform.position, separationDistance);

        foreach (Collider2D neighbor in neighbors)
        {
            //keep going if the neighbor is a collectable or terrain
            if (neighbor.gameObject == gameObject) continue;
            if (neighbor.GetComponent<Collectable>() == null) continue;

            //calculate the direction and distance to the neighbor
            Vector3 pushDir = transform.position - neighbor.transform.position;
            float distance = pushDir.magnitude;
            if (distance > 0)
               separationResult += pushDir.normalized / distance * separationForce;
        }

        return separationResult;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isFollowing)
        {
            playerTransform = collision.transform;
            isFollowing = true; 
            ScoreManager.AddScore(1);
            playerMovement = collision.GetComponent<PlayerMovement>();
            //increments player speed by a small amount to try and out run the net
            playerMovement.MoveSpeed += 0.1f;
            followSpeed += 0.2f;

        }
    }
}
