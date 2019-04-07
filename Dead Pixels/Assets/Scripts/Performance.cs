using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Performance : MonoBehaviour
{
    float deltaTime = 0.0f;
    
    private Text _text;

    public void Start()
    {
        _text = GetComponent<Text>();
    }
    
    public void Update()
    {
        var fps = 1.0f / deltaTime;
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
        _text.text = $"{fps:0.}";
    }

}
