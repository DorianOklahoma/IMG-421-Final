using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bow : Weapon
{
    public ArrowProjectile arrowPrefab;
    public Transform launchPoint;
    public float maxDrawTime = 1.2f;
    public float minDrawThreshold = 0.1f;
    public float maxDrawMultiplier = 2.5f;

    private bool isDrawing = false;
    private float drawStartTime = 0f;

    public float DrawProgress => isDrawing ? Mathf.Clamp01((Time.time - drawStartTime) / maxDrawTime) : 0f;

    protected override void Start()
    {
        base.Start();
    }

    public override void UsePrimary()
    {
        base.UsePrimary();

        if (anim != null)
        {
            anim.SetTrigger("quickShot");
        }

        ArrowProjectile arrow = Instantiate(arrowPrefab, launchPoint.position, Quaternion.identity);
        
        arrow.Launch(launchPoint.right, primaryDamage);
    }

    public override void UseSecondary()
    {
        if (isDrawing) 
        {
            return;
        }

        isDrawing = true;

        drawStartTime = Time.time;

        if (anim != null)
        {
            anim.SetTrigger("draw");
        }
    }

    public void ReleaseSecondary()
    {
        if (!isDrawing) 
        {
            return;
        }

        float progress = DrawProgress;

        isDrawing = false;

        if (progress < minDrawThreshold)
        {
            if (anim != null) 
            {
                anim.SetTrigger("idle");
            }

            return;
        }

        if (anim != null)
        {
            anim.SetTrigger("release");
        }

        float dmg = secondaryDamage * Mathf.Lerp(1f, maxDrawMultiplier, progress);

        ArrowProjectile arrow = Instantiate(arrowPrefab, launchPoint.position, Quaternion.identity);
        
        arrow.Launch(launchPoint.right, dmg);
    }

    void Update()
    {
        if (anim != null && isDrawing)
        {
            anim.SetFloat("drawProgress", DrawProgress);
        }
    }
}