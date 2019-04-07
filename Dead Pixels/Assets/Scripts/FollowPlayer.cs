using UnityEngine;

public class FollowPlayer : MonoBehaviour
{    
    public Player _player;

    private Vector2 _movement;
    
    private void OnEnable()
    {
        _movement = _player._body.velocity;
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
    }
}
