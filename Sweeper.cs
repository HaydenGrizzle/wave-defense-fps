using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sweeper : MonoBehaviour
{
    public float maxScale = 3f;  // scale target on X
    public float duration = 1f;  // time for a full cycle (0 → 3 → 0)

    void Update()
    {
        // PingPong goes 0 -> duration -> 0 smoothly
        float t = Mathf.PingPong(Time.time * (2f / duration), 1f);
        float xScale = Mathf.Lerp(0f, maxScale, t);

        // Apply new scale (only X changes)
        transform.localScale = new Vector3(xScale, transform.localScale.y, transform.localScale.z);
    }
}
