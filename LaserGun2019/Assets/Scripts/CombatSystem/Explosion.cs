using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField] private float explosionRadius;
    [SerializeField] private float explosionForce;
    [SerializeField] private float explosionUpwardsModifier;
    

    private void OnEnable()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);

        foreach (Collider col in colliders)
        {
            Rigidbody rBody = col.GetComponent<Rigidbody>();
            if (rBody == null)
            {
                continue;
            }
            rBody.AddExplosionForce(explosionForce, transform.position, explosionRadius, explosionUpwardsModifier, ForceMode.Impulse);
        }
    }

}
