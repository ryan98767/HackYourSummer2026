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

    
    
    void Start()
    {
        //spawn initial chunk and setting the first spawn point location
        nextSpawn = firstSpawn.position.x;
        SpawnChunk();
        
    }

    // Update is called once per frame
    void Update()
    {
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
}
