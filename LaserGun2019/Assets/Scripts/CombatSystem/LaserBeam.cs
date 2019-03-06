using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBeam : MonoBehaviour
{
    private Ray ray;
    private LineRenderer line;
    private float hitDistance;
    private RaycastHit hitInfo;
    private Vector3 hitPoint;   



    void Awake()
    {
        line = GetComponent<LineRenderer>();
        line.enabled = false;
    }


    void OnEnable()
    {
        ContactCheck();
    }    


    void ContactCheck()
    {
        ray = new Ray(transform.position, transform.forward);

        if (Physics.Raycast(ray, out hitInfo, 100f))
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


    void HitFunc()
    {
        line.enabled = true;
        if (hitInfo.collider.gameObject.tag == "Box")
        {
            GameManager.Instance.IncreaseScore(500);
        }
        StartCoroutine("HitCoroutine");
    }


    void NoHitFunc()
    {
        line.enabled = true;
        StartCoroutine("NoHitCoroutine");
    }


    IEnumerator HitCoroutine()
    {
        for (float start = 0f, end = 0.5f; end < hitDistance + 5f; start += 0.5f, end += 0.5f)
        {
            if (start >= hitDistance)
            {
                line.enabled = false;
                transform.GetChild(0).gameObject.SetActive(true);
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


    public Vector3 GetHitPoint()
    {
        return hitPoint;
    }


    void Deactivation()
    {
        gameObject.SetActive(false);
        transform.GetChild(0).gameObject.SetActive(false);
    }
}
