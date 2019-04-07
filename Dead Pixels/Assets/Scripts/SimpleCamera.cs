using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleCamera : MonoBehaviour
{

    public Player _player;

    public void LateUpdate ()
    {
        // Set the position of the camera's transform to be the same as the player's, but offset by the calculated offset distance.
        var position = transform.position;
        var cameraPosition = new Vector3(_player.transform.position.x, position.y, position.z);
        
        if (cameraPosition.x < 0)
        {
            // Do not move further to the left than level start
            cameraPosition.x = 0;
        }

        transform.position = cameraPosition;
    }
}