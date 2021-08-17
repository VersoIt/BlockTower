using UnityEngine;

public static class UnityExtensions
{
    public static void Update(this Rigidbody gameObject)
    {
        gameObject.isKinematic = !gameObject.isKinematic;
        gameObject.isKinematic = !gameObject.isKinematic;
    }
}
