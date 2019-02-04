using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleExplo : AbstractExplo
{
    private Collider[] colliders;    

    public override void Explo(Vector3 exploPosition)
    {
        transform.position = exploPosition;
        colliders = Physics.OverlapSphere(transform.position, 10f);
        foreach (Collider c in colliders)
        {
            if (c.gameObject.GetComponent<Rigidbody>() == null)
            {
                continue;
            }
            Rigidbody rBody = c.gameObject.GetComponent<Rigidbody>();
            rBody.AddExplosionForce(0.5f, transform.position, 10f, 1f, ForceMode.Impulse);
        }
    }    

}
