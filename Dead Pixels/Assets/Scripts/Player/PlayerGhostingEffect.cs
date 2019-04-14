using System.Collections;
using UnityEngine;

public class PlayerGhostingEffect : MonoBehaviour
{    
    public Player player;

    private static readonly Color StartColor = new Color(1.0f, 1.0f, 1.0f,1.0f);
    private static readonly Color EndColor = new Color(1.0f, 1.0f, 1.0f,0.0f);
    
    private Vector2 _velocity;
    private Vector2 _position;
    private Renderer[] _renderer;
    
    private bool _ghostDisplayed;

    public float distanceBetweenGhosts = 0.4f;
    public float fadeOutDuration = 0.3f;

    public void Awake()
    {
        _renderer = GetComponentsInChildren<Renderer>();

        foreach (var renderer in _renderer)
        {
            renderer.material.color = Color.clear;
        }
        
        GameEventManager.StartListening("Dash", RenderGhosts);
    }
    
    public void RenderGhosts()
    {   
        _velocity = player.Body.velocity;
        _position = player.Body.position;
        
        var ghostCount = transform.childCount;
        
        for (var i = 0; i < ghostCount; i++)
        {
            var child = transform.GetChild(i);
            var childRenderer = _renderer[i];
            
            child.transform.position = _position - new Vector2(
                                                           _velocity.normalized.x / ghostCount * (i * distanceBetweenGhosts), 
                                                           _velocity.normalized.y / ghostCount * (i * distanceBetweenGhosts));

            StartCoroutine(FadeOut(childRenderer, ghostCount, i, fadeOutDuration));
        }   
    }
    
    private IEnumerator FadeOut(Renderer renderer, int total, int ghostIndex, float fadeOutDuration)
    {
        var time = 0.0f;
        var rate = 1.0f / fadeOutDuration;
        
        while (time < 1.0f) {
            time += Time.deltaTime * rate;
            renderer.material.color = Color.Lerp(StartColor * 1 / total * (total - ghostIndex + 1) , EndColor, time);
            
            yield return null;
        }
    }
}
