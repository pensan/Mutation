// REDOX Game Labs 2016

#region INCLUDES
using System;
using UnityEngine;
using System.Collections.Generic;
#endregion

public class RunnerAppearanceLimb : MonoBehaviour 
{
    public FixedJoint2D masterJoint;

    public List<SpriteRenderer> SpriteRenderers
    {
        get;
        private set;
    }

    private Vector3 startLocalPosition;

    void Awake()
    {
        SpriteRenderers = new List<SpriteRenderer>();

        SpriteRenderer ownRenderer = GetComponent<SpriteRenderer>();
        if (ownRenderer != null)
            SpriteRenderers.Add(ownRenderer);

        SpriteRenderer[] childRenderers = GetComponentsInChildren<SpriteRenderer>();
        if (childRenderers != null)
            SpriteRenderers.AddRange(childRenderers);

        startLocalPosition = this.transform.localPosition;
    }

    public void SetOpaque(bool opaque)
    {
        foreach (SpriteRenderer sprite in SpriteRenderers)
        {
            Color color = sprite.color;
            color.a = opaque ? 1f : 0.5f;
            sprite.color = color;
        }
    }

    public void UpdatePosition()
    {
        this.transform.localPosition = startLocalPosition;
    }
}
