using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameController : MonoBehaviour
{

    private UnityEvent _jumpEvent;

    public Player _player;

    public CameraShake _camera;
    
    // Start is called before the first frame update
    public void Start()
    {
        _jumpEvent = new UnityEvent();
        _jumpEvent.AddListener(_camera.Shake);

        _player.SetJumpEvent(_jumpEvent);


    }
}
