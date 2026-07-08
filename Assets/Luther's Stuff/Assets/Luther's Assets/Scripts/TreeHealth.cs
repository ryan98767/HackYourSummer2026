using UnityEngine;

// Attach to each Tree GameObject.
public class TreeHealth : MonoBehaviour
{
    public Sprite healthySprite;
    public Sprite burningSprite;
    public GameObject firePrefab;  
    public bool startsOnFire = false;

    public Vector2 fireSpawnOffset = new Vector2(0f, 0f);
    public int fireSortingOrderBoost = 1;

    private SpriteRenderer sr;
    private GameObject activeFireInstance;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        if (startsOnFire) SetOnFire();
        else
        {
            sr.sprite = healthySprite;

            FireHazard existingFire = GetComponentInChildren<FireHazard>();
            if (existingFire != null) existingFire.linkedTree = this;
        }
    }

    public void SetOnFire()
    {
        sr.sprite = burningSprite;
        if (activeFireInstance != null) return; 


        FireHazard existingFire = GetComponentInChildren<FireHazard>();
        if (existingFire != null)
        {
            existingFire.linkedTree = this;
            activeFireInstance = existingFire.gameObject;
            existingFire.transform.localPosition = fireSpawnOffset;
            ApplyFireSortOrder(activeFireInstance);
            return;
        }

        if (firePrefab != null)
        {
            Vector3 spawnPos = transform.position + new Vector3(fireSpawnOffset.x, fireSpawnOffset.y, 0);

            activeFireInstance = Instantiate(firePrefab, spawnPos, Quaternion.identity, transform);
            ApplyFireSortOrder(activeFireInstance);

            FireHazard fh = activeFireInstance.GetComponentInChildren<FireHazard>();
            if (fh != null)
            {
                fh.linkedTree = this;
            }
            else
            {
                Debug.LogWarning($"[TreeHealth] '{firePrefab.name}' has no FireHazard component anywhere on it — extinguishing it won't heal this tree.", this);
            }
        }
    }


    void ApplyFireSortOrder(GameObject fireObj)
    {
        SpriteRenderer[] fireRenderers = fireObj.GetComponentsInChildren<SpriteRenderer>();
        foreach (var fr in fireRenderers)
        {
            fr.sortingLayerID = sr.sortingLayerID;
            fr.sortingOrder = sr.sortingOrder + fireSortingOrderBoost;
        }
    }

    public void Extinguish()
    {
        sr.sprite = healthySprite;
        activeFireInstance = null;
    }
}