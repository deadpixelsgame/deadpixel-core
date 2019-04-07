﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{    
    public Player _player;
    
    private Vector2 _movement;
    
    private void OnEnable()
    {
        _movement = _player._body.velocity;
        Renderer[] renderers = GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in renderers)
        {
            renderer.material.color = Color.red;
        }
    }

    private float MaxAlpha(Renderer[] renderers)
    {
        float maxAlpha = 0.0f;
        foreach (Renderer renderer in renderers)
        {
            maxAlpha = Mathf.Max(maxAlpha, renderer.material.color.a);
        }

        return maxAlpha;
    }

    private IEnumerator FadeOut()
    {
        Renderer[] rendererObjects = GetComponentsInChildren<Renderer>();

        float alphaValue = MaxAlpha(rendererObjects);

        alphaValue += Time.deltaTime * - 0.01f;

        for (int i = 0; i < rendererObjects.Length; i++)
        {
            Color start = rendererObjects[i].material.color;
            Color end = new Color(start.r, start.g, start.b, 0.0f);
            for (float t = 0.0f; t < 0.2f; t += Time.deltaTime)
            {
                rendererObjects[i].material.color = Color.Lerp(start, end, t / 0.2f);
                yield return null;
            }
        }
    }

    public void LateUpdate ()
    {
        var ghostCount = transform.childCount;
        
        for (var i = 0; i < ghostCount; i++)
        {            
            transform.GetChild(i).transform.position = _player.transform.position - new Vector3(
                                     _movement.normalized.x * (i + 1) / 2, 
                                     _movement.normalized.y * (i + 1) / 2);
        }

        StartCoroutine(nameof(FadeOut));
    }
}
