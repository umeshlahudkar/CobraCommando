
using UnityEngine;

public class DestroyOverTime : MonoBehaviour
{
    [SerializeField] private float destroyTime;
    void Start()
    {
        Destroy(gameObject, destroyTime);
    }
}
