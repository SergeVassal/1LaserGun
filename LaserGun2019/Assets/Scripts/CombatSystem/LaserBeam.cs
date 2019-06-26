using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBeam : MonoBehaviour
{
    private LineRenderer lineRenderer;
    private float collisionRayMaxDistance = 100f;
    private RaycastHit hitInfo;
    private Ray ray;
    private float laserLineStep = 0.5f;
    private float laserLineMoveInterval = 0.01f;
    private float laserLineLength = 1f;
    private float hitPointDistanceFromRayOrigin;
    private float laserFleightNoCollisionDistance = 20f;
    private float explosionTime = 3f;
    private Explosion explosion;
    private int pointsForHittingScoreRaiserObject = 100;



    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.enabled = false;
        explosion = transform.GetComponentInChildren<Explosion>();
        explosion.transform.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        lineRenderer.enabled = true;
        CheckCollision();
    }

    private void CheckCollision()
    {
        ray = new Ray(transform.position, transform.forward);

        if (Physics.Raycast(ray, out hitInfo, collisionRayMaxDistance))
        {
            hitPointDistanceFromRayOrigin = hitInfo.distance;
            CheckScoreRaiserObject();
            StartCoroutine(FireLaserHitCoroutine());
        }
        else
        {
            StartCoroutine(FireLaserNoHitCoroutine());
        }
    }

    private void CheckScoreRaiserObject()
    {
        if (hitInfo.collider.gameObject.tag == "Box")
        {
            UpdateScore();
        }
    }

    private void UpdateScore()
    {
        GameManager.Instance.IncreaseScore(pointsForHittingScoreRaiserObject);
    }

    private IEnumerator FireLaserHitCoroutine()
    {
        for (float startingRayPoint = 0f; startingRayPoint <= hitPointDistanceFromRayOrigin - laserLineLength;)
        {
            lineRenderer.SetPosition(0, ray.GetPoint(startingRayPoint));
            lineRenderer.SetPosition(1, ray.GetPoint(startingRayPoint + laserLineLength));
            startingRayPoint += laserLineStep;
            yield return new WaitForSeconds(laserLineMoveInterval);
        }
        FinishLaserFleight();
        yield return new WaitForSeconds(laserLineMoveInterval);
        ActivateExplosion();
        yield return new WaitForSeconds(explosionTime);
        DeactivateLaserBeamAndExplosion();
    }

    private void FinishLaserFleight()
    {
        lineRenderer.SetPosition(0, ray.GetPoint(hitPointDistanceFromRayOrigin - laserLineLength));
        lineRenderer.SetPosition(1, ray.GetPoint(hitPointDistanceFromRayOrigin));
    }

    private void ActivateExplosion()
    {
        lineRenderer.enabled = false;
        explosion.transform.position = hitInfo.point;
        explosion.transform.gameObject.SetActive(true);
    }

    private void DeactivateLaserBeamAndExplosion()
    {
        explosion.transform.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }

    private IEnumerator FireLaserNoHitCoroutine()
    {
        for (float startingRayPoint = 0f; startingRayPoint <= laserFleightNoCollisionDistance;)
        {
            lineRenderer.SetPosition(0, ray.GetPoint(startingRayPoint));
            lineRenderer.SetPosition(1, ray.GetPoint(startingRayPoint + laserLineLength));
            startingRayPoint += laserLineStep;
            yield return new WaitForSeconds(laserLineMoveInterval);
        }
        gameObject.SetActive(false);
    }
}
