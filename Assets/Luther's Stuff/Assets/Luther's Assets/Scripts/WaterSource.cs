using UnityEngine;

public class WaterSource : MonoBehaviour
{
    public float refillRate = 2f;
    private float refillAccumulator = 0f;

    void OnTriggerStay2D(Collider2D other)
    {
        PlayerController player = other.GetComponent<PlayerController>();
        if (player != null && player.currentWater < player.maxWater)
        {
            refillAccumulator += refillRate * Time.deltaTime;
            if (refillAccumulator >= 1f)
            {
                int wholeUnits = Mathf.FloorToInt(refillAccumulator);
                player.RefillWater(wholeUnits);
                refillAccumulator -= wholeUnits;
            }
        }
    }
}
