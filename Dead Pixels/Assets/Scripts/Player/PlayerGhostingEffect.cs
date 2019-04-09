using System.Collections;
using UnityEngine;

public class PlayerGhostingEffect : MonoBehaviour
{    
    public Player player;

    private static readonly Color StartColor = new Color(1.0f, 1.0f, 1.0f,1.0f);
    private static readonly Color EndColor = new Color(1.0f, 1.0f, 1.0f,0.0f);
    
    private Vector2 _movement;
    private Vector2 _position;
    private Renderer[] _renderer;
    private bool _ghostDisplayed;

    public void Start()
    {
        _renderer = GetComponentsInChildren<Renderer>();

        foreach (var renderer in _renderer)
        {
            renderer.material.color = Color.clear;
        }
    }
    
    public void RenderGhosts()
    {
        if (_ghostDisplayed) return;
        
        _ghostDisplayed = true;        
        _movement = player._body.velocity;
        _position = player._body.position;
        
        var ghostCount = transform.childCount;
        
        for (var i = 0; i < ghostCount; i++)
        {
            var child = transform.GetChild(i);
            var childRenderer = _renderer[i];
            
            child.transform.position = _position - new Vector2(
                                                           _movement.normalized.x * (i + 0.4f) / 2, 
                                                           _movement.normalized.y * (i + 0.4f) / 2);

            StartCoroutine(FadeOut(childRenderer, ghostCount, i, .2f));
        }   
    }
    
    private IEnumerator FadeOut(Renderer renderer, int total, int index, float fadeOutDuration)
    {
        var time = 0.0f;
        var rate = 1.0f / fadeOutDuration;
        
        while (time < 1.0f) {
            time += Time.deltaTime * rate;
            renderer.material.color = Color.Lerp(StartColor * 1 / total * (total - index + 1) , EndColor, time);
            
            yield return null;
        }

        _ghostDisplayed = false;
    }
}
