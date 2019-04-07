using UnityEngine;

public class CameraShake : MonoBehaviour
{
    // Transform of the camera to shake. Grabs the gameObject's transform
    // if null.
    private Transform camTransform;

    public FollowPlayer followPlayer;
	
    // How long the object should shake for.
    public float shakeDuration = 0f;
	
    // Amplitude of the shake. A larger value shakes the camera harder.
    public float shakeAmount = 0.7f;
    public float decreaseFactor = 1.0f;
	
    Vector3 originalPos;
	
    public void Awake()
    {
        if (camTransform == null)
        {
            camTransform = GetComponent(typeof(Transform)) as Transform;
        }
    }

    public void Shake()
    {
        shakeDuration = 0.2f;
    } 
	
    public void OnEnable()
    {
        originalPos = camTransform.localPosition;
    }

    public void Update()
    {
        if (shakeDuration > 0)
        {
            followPlayer.enabled = true;
            camTransform.localPosition = originalPos + Random.insideUnitSphere * shakeAmount;
			
            shakeDuration -= Time.deltaTime * decreaseFactor;
        }
        else
        {
            followPlayer.enabled = false;
            shakeDuration = 0f;
            camTransform.localPosition = originalPos;
        }
    }
}