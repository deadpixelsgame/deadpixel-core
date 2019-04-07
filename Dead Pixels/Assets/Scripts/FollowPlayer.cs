using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{    
    public Player _player;
    
    private Vector2 _movement;
    
    private void OnEnable()
    {
        _movement = _player._body.velocity;
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

        while (alphaValue >= 0f)
        {
            alphaValue += Time.deltaTime * - 0.1f;

            for (int i = 0; i < rendererObjects.Length; i++)
            {
                Color newColor = rendererObjects[i].material.color;
                newColor.a = Mathf.Min(newColor.a, alphaValue);
                newColor.a = Mathf.Clamp(newColor.a, 0.0f, 1.0f);
                rendererObjects[i].material.color = newColor;
                print(newColor.a);
            }
        }
        
        yield return new WaitForSeconds(0.1f);
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

//        StartCoroutine(nameof(FadeOut));
    }
}
