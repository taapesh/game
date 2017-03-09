using UnityEngine;
using System.Collections;

public class CameraZoom : MonoBehaviour {
    private float distance;
    private const float sensitivity = 20.0f;
    private const float damping     = 10.0f;
    private const float minFOV      = 35.0f;
    private const float maxFOV      = 65.0f;

    void Start () {
        distance = GetComponent<Camera>().fieldOfView;
    }

    void Update () {
        distance -= Input.GetAxis("Mouse ScrollWheel") * sensitivity;
        distance = Mathf.Clamp(distance, minFOV, maxFOV);
        GetComponent<Camera>().fieldOfView = Mathf.Lerp(GetComponent<Camera>().fieldOfView, distance, damping * Time.deltaTime);
    }
}
