using UnityEngine;

public class FollowingCamera : MonoBehaviour
{
    [SerializeField] private Transform target = default;
    [SerializeField] private Camera followingCamera = default;
    [SerializeField] private float cameraSpeed = 10;
    [SerializeField] private RectTransform cameraBound = default;

    private Vector3 offset;
    private bool needToFollow;

    private void Awake()
    {
        offset = transform.position - target.position;
    }

    private void LateUpdate()
    {
        Vector2 screenPoint = followingCamera.WorldToScreenPoint(target.position);
        needToFollow = !RectTransformUtility.RectangleContainsScreenPoint(cameraBound, screenPoint);

        if (needToFollow)
        {
            var moveVector = Vector3.MoveTowards(transform.position, target.position + offset, cameraSpeed * Time.deltaTime);
            transform.position = moveVector;
        }
    }
}
