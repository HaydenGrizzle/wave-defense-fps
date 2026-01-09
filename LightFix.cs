using UnityEngine;

public class LightFix : MonoBehaviour
{
    void Start()
    {
        DynamicGI.UpdateEnvironment();
    }
}