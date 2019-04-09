using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLight : MonoBehaviour
{
    public Player player;

    public float amplitude = 0.2f;

    public float baseStart = 0.5f;
    
    public float speed = 0.1f;
    
    private Light _light;
    private Color _color;

    public void Start()
    {        
        _light = GetComponent<Light>();
        _color = _light.color;
        
        StartCoroutine(nameof(Flicker));
    }
        
    public void LateUpdate()
    {
        transform.position = player.transform.position;
    }
       
    private IEnumerator Flicker()
    {
        while (true)
        {
            _light.color = _color * Wave();
            yield return new WaitForSeconds(speed);
        }
    }

    private float Wave() { 
        var y = 1f - Random.value * 2;
        return y * amplitude + baseStart;    
    }
}
