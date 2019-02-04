using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleLaserBeam : AbstractBullet
{  
    private LineRenderer line;
    private Ray ray;
    private RaycastHit hitInfo;
    private float hitDistance;
    private Vector3 hitPoint;

    private void Awake()
    {
        line = GetComponent<LineRenderer>();        
        line.enabled = false;
    }

    public override void FireBullet()
    {
        ContactCheck();
        line.enabled = true;
        Debug.Log("Fire");
    }
    private void ContactCheck()
    {
        ray = new Ray(transform.position, transform.forward);

        if(Physics.Raycast(ray,out hitInfo, 100f))
        {
            hitDistance = hitInfo.distance;
            hitPoint = hitInfo.point;
            HitFunc();
        }
        else
        {
            NoHitFunc();
        }
    }

    private void HitFunc()
    {
        if (hitInfo.collider.gameObject.tag == "Box")
        {
            GameManager.Instance.IncreaseScore(50);
        }
        StartCoroutine(HitCoroutine());
    }

    private void NoHitFunc()
    {
        StartCoroutine(NoHitCoroutine());
    }

    IEnumerator HitCoroutine()
    {
        for (float start = 0f, end = 0.5f; end < hitDistance + 5f; start += 0.5f, end += 0.5f)
        {
            if (start >= hitDistance)
            {
                line.enabled = false;
                if(transform.GetChild(0).gameObject!=null&&
                    transform.GetChild(0).gameObject.GetComponent<AbstractExplo>() != null)
                {
                    transform.GetChild(0).gameObject.SetActive(true);
                    transform.GetChild(0).gameObject.GetComponent<AbstractExplo>().Explo(hitPoint);
                }
                
                Invoke("Deactivation", 2f);
            }
            line.SetPosition(0, ray.GetPoint(start));
            line.SetPosition(1, ray.GetPoint(end));
            yield return new WaitForSeconds(0.01f);
        }
    }

    IEnumerator NoHitCoroutine()
    {
        for (float start = 0f, end = 0.5f; end < 101f; start += 0.5f, end += 0.5f)
        {
            if (end >= 100f)
            {
                line.enabled = false;
                gameObject.SetActive(false);
            }
            line.SetPosition(0, ray.GetPoint(start));
            line.SetPosition(1, ray.GetPoint(end));
            yield return new WaitForSeconds(0.01f);
        }
    }

    void Deactivation()
    {
        gameObject.SetActive(false);
        transform.GetChild(0).gameObject.SetActive(false);
    }



}
