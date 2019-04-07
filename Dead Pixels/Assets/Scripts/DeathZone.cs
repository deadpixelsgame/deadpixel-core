using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathZone : MonoBehaviour
{

    public Player _player;
    
    private void OnCollisionEnter2D(Collision2D other)
    {
        Debug.Log("Death Zone");
        if (other.gameObject.Equals(_player.gameObject))
        {
            _player.ResetPlayer();
        }
    }
}
