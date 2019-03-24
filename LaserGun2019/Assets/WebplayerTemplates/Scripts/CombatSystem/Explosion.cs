using UnityEngine;
using System.Collections;

public class Explosion : MonoBehaviour
{

    private Collider[] colliders;
    private LaserBeam laserBeamScript;

    void Awake()
    {
        laserBeamScript = transform.parent.gameObject.GetComponent<LaserBeam>();
    }

    void OnEnable()
    {
        transform.position = laserBeamScript.GetHitPoint();
        colliders = Physics.OverlapSphere(transform.position, 10f);
        foreach (Collider c in colliders)
        {
            if (c.gameObject.GetComponent<Rigidbody>() == null)
            {
                continue;
            }
            Rigidbody rBody = c.gameObject.GetComponent<Rigidbody>();
            rBody.AddExplosionForce(10f, transform.position, 10f, 1f, ForceMode.Impulse);
        }
    }
}