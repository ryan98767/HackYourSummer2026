using UnityEngine;

public class FireHazard : MonoBehaviour
{
    [Header("Fire Settings")]
    public int healthPoints = 3;  
    public TreeHealth linkedTree;    
    public GameObject extinguishEffectPrefab; 

    [Header("Spread (optional)")]
    public bool canSpread = false;
    public float spreadInterval = 8f;
    public float spreadRadius = 3f;
    public GameObject firePrefab; 

    private float spreadTimer;

    void Start()
    {
        spreadTimer = spreadInterval;
    }

    void Update()
    {
        if (!canSpread) return;
        spreadTimer -= Time.deltaTime;
        if (spreadTimer <= 0f)
        {
            spreadTimer = spreadInterval;
            TrySpread();
        }
    }

    public void ExtinguishHit()
    {
        healthPoints--;
        if (healthPoints <= 0)
        {
            Extinguish();
        }
    }

    void Extinguish()
    {
        if (linkedTree != null)
        {
            linkedTree.Extinguish();
        }

        if (extinguishEffectPrefab != null)
        {
            Instantiate(extinguishEffectPrefab, transform.position, Quaternion.identity);
        }

        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnFireExtinguished();
        }

        Destroy(gameObject);
    }

    void TrySpread()
    {
        if (firePrefab == null) return;
        Vector2 offset = Random.insideUnitCircle.normalized * spreadRadius;
        Vector3 spawnPos = transform.position + new Vector3(offset.x, 0, 0);
        Instantiate(firePrefab, spawnPos, Quaternion.identity);
    }
}
