using UnityEngine;
using System.Collections.Generic;

public class ChunkLoader : MonoBehaviour
{
    [SerializeField] private GameObject[] chunks;
    [SerializeField] private Transform cameraTransform;
    [SerializeField] float loadDistance = 20f;
    [SerializeField] float despawnBehindDistance = 40f;
    [SerializeField] private Transform firstSpawn;

    private float nextSpawn = 0f;
    private List<GameObject> activeChunks = new List<GameObject>();

    [SerializeField] private GameObject[] collectables;
    [SerializeField] private float spawnInterval = 0.5f;
    [SerializeField] private Vector2 spawnYRange = new Vector2(-40f, 40f);
    [SerializeField] private float collectableRadius = 0.5f;
    [SerializeField] private LayerMask terrainLayer;
    private float timer;

    void Start()
    {
        //spawn initial chunk and setting the first spawn point location
        nextSpawn = firstSpawn.position.x;
        SpawnChunk();
        
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        //spawn new chunks if the camera is approaching the next spawn point
        while (nextSpawn > cameraTransform.position.x - loadDistance)
        {
            SpawnChunk();
        }

        //despawn chunks that are behind the camera
        for (int i = activeChunks.Count - 1; i >= 0; i--)
        {
            if (activeChunks[i].transform.position.x > cameraTransform.position.x + despawnBehindDistance)
            {
                Destroy(activeChunks[i]);
                activeChunks.RemoveAt(i);
            }
        }

        if (timer >= spawnInterval)
        {
            SpawnCollectable();
            timer = 0f;
        }
    }

    private void SpawnChunk()
    {
        //spawn a random chunk from the array of chunks and position it at the next spawn point
        GameObject prefab = chunks[Random.Range(0, chunks.Length)];
        GameObject chunk = Instantiate(prefab, new Vector3(nextSpawn, firstSpawn.position.y, firstSpawn.position.z), Quaternion.identity);
        activeChunks.Add(chunk);

        nextSpawn -= GetChunkWidth(chunk); // Update nextSpawn based on the width of the chunk
    }

    // Get the width of the chunk based on its Chunk component
    private float GetChunkWidth(GameObject chunk)
    {
        Chunk c = chunk.GetComponent<Chunk>();
        return c != null ? c.width : 20f; // Default width if no Chunk component is found
    }

    private void SpawnCollectable()
    {
        //give a limited amount of checks to spawn 
        int maxAttempts = 10;
        for (int i = 0; i < maxAttempts; i++)
        {
            //spawn a random y position within the specified range
            float randomY = Random.Range(spawnYRange.x, spawnYRange.y);
            Vector2 loc = new Vector2(cameraTransform.position.x - loadDistance, randomY);

            //if the location is not inside terrain
            Collider2D hit = Physics2D.OverlapCircle(loc, collectableRadius, terrainLayer);
            if (hit == null)
            {
                GameObject prefab = collectables[Random.Range(0, collectables.Length)];
                GameObject collect = Instantiate(prefab, new Vector3(loc.x, loc.y, 0), Quaternion.identity);
                return;
            }
        }
    }
}
