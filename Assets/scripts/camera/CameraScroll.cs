using UnityEngine;

public class CameraScroll : MonoBehaviour {
    private Transform follow;
    private const float scrollSpeed     = 50.0f;
    private const float damping         = 7.0f;
    private const float leftBoundary    = 0.0f;
    private const float rightBoundary   = 100.0f;
    private const float topBoundary     = 80.0f;
    private const float bottomBoundary  = -8.0f;
    private Vector3 upDir       = scrollSpeed * Vector3.forward;
    private Vector3 downDir     = scrollSpeed * Vector3.back;
    private Vector3 rightDir    = scrollSpeed * Vector3.right;
    private Vector3 leftDir     = scrollSpeed * Vector3.left;

    void Awake() {
        follow = new GameObject("Follow").transform;
        follow.position = transform.position;
    }

    void  Update () {
        transform.position = Vector3.Lerp(transform.position, follow.position, damping * Time.deltaTime);

        if (Input.GetKey("right") || Input.GetKey(KeyCode.D)) {
            if (follow.position.x < rightBoundary) {
                follow.Translate(rightDir * Time.deltaTime, Space.World);
            }
        }

        if (Input.GetKey("left") || Input.GetKey(KeyCode.A)) {
            if (follow.position.x > leftBoundary) {
                follow.Translate(leftDir * Time.deltaTime, Space.World);
            }
        }

        if (Input.GetKey("up") || Input.GetKey(KeyCode.W)) {
            if (follow.position.z < topBoundary) {
                follow.Translate(upDir * Time.deltaTime, Space.World);
            }
        }

        if (Input.GetKey("down") || Input.GetKey(KeyCode.S)) {
            if (follow.position.z > bottomBoundary) {
                follow.Translate(downDir * Time.deltaTime, Space.World);
            }
        }
    }
}
