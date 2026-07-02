using UnityEngine;

/// <summary>
/// Spawns platforms upward as the player climbs
/// This creates a simple procedural building layout
/// </summary>
public class BuildingGenerator : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject platformPrefab;
    public GameObject wallPrefab;
    public GameObject exitDoorPrefab;

    [Header("Generation Settings")]
    public int startingPlatforms = 10;
    public float verticalSpacing = 1.2f;
    public float spawnAheadDistance = 10f;

    [Header("Building Width")]
    public float leftWallX = -6f;
    public float rightWallX = 6f;
    public float wallThickness = 0.5f;
    
    [Header("Platform Size")]
    public float minPlatformWidth = 2f;
    public float maxPlatformWidth = 5f;
    public float platformHeight = 0.35f;

    [Header("Wall Settings")]
    public float wallSegmentHeight = 4f;

    [Header("Player Tracking")]
    public Transform player;

    [Header("Exit Settings")]

    [Min(1)]
    public int platformsUntilExit = 200;

    private float highestPlatformY = -3.5f;
    private float highestWallY = -4f;

    private int platformsSpawned = 0;
    private bool exitSpawned = false;

    private void Start()
    {
        for (int i=0; i < startingPlatforms; i++)
        {
            SpawnPlatform();
        }

        GenerateWallsUpTo(player.position.y + spawnAheadDistance);
    }

    
    private void Update()
    {
        if (player == null)
        {
            return;
        }

        //Once the exit has spawned, stop generating new platforms
        if (!exitSpawned)
        {
            //Continue generating platforms ahead of the player
            while (!exitSpawned && highestPlatformY < player.position.y + spawnAheadDistance)
            {
                SpawnPlatform();
            }
        }

        GenerateWallsUpTo(player.position.y + spawnAheadDistance);
    }

    private void SpawnPlatform()
    {
        if (exitSpawned)
        {
            return;
        }

        float randomWidth = Random.Range(minPlatformWidth, maxPlatformWidth);

        float innerLeft = leftWallX + wallThickness + randomWidth / 2f;
        float innerRight = rightWallX - wallThickness - randomWidth / 2f;

        float randomX = Random.Range(innerLeft, innerRight);

        highestPlatformY += verticalSpacing;

        GameObject newPlatform = Instantiate(
            platformPrefab,
            new Vector3(randomX, highestPlatformY, 0f),
            Quaternion.identity
        );

        newPlatform.transform.localScale = new Vector3(
            randomWidth,
            platformHeight,
            1f
        );

        newPlatform.name = "GeneratedPlatform";

        platformsSpawned++;

        if (platformsSpawned >= platformsUntilExit && !exitSpawned)
        {
            SpawnExitDoor(newPlatform);
        }
    }

    private void GenerateWallsUpTo(float targetY)
    {
        while (highestWallY < targetY)
        {
            SpawnWallPair(highestWallY);
            highestWallY += wallSegmentHeight;
        }
    }

    private void SpawnWallPair(float yPosition)
    {
        GameObject leftWall = Instantiate(
                    wallPrefab,
                    new Vector3(leftWallX, yPosition, 0f),
                    Quaternion.identity
                );

        leftWall.transform.localScale = new Vector3(
            wallThickness,
            wallSegmentHeight,
            1f
        );

        leftWall.name = "GeneratedLeftWall";

        GameObject rightWall = Instantiate(
            wallPrefab,
            new Vector3(rightWallX, yPosition, 0f),
            Quaternion.identity
        );

        rightWall.transform.localScale = new Vector3(
            wallThickness,
            wallSegmentHeight,
            1f
        );

        rightWall.name = "GeneratedRightWall";
    }

    private void SpawnExitDoor(GameObject lastPlatform)
    {
        exitSpawned = true;

        SpriteRenderer platformRenderer = lastPlatform.GetComponent<SpriteRenderer>();
        float platformTopY = platformRenderer.bounds.max.y;

        GameObject door = Instantiate(
            exitDoorPrefab,
            new Vector3(lastPlatform.transform.position.x, platformTopY, 0f),
            Quaternion.identity
        );

        SpriteRenderer doorRenderer = door.GetComponent<SpriteRenderer>();

        float doorBottomY = doorRenderer.bounds.min.y;
        float difference = platformTopY - doorBottomY;

        door.transform.position += new Vector3(0f, difference, 0f);

        Debug.Log("Exit door spawned on the last platform.");
    }
}