using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class ThirdPersonCamera : MonoBehaviour
{

    public bool lockCursor;
    public float mouseSensitivity = 10;
    public Transform target;
    public float dstFromTarget = 2;
    public Vector2 pitchMinMax = new Vector2(-40, 85);

    public float rotationSmoothTime = .12f;
    Vector3 rotationSmoothVelocity;
    Vector3 currentRotation;

    float yaw;
    float pitch;

    void Start()
    {
        UEventManager.register("OnPointerDown", this.OnPointerDown);
        UEventManager.register("OnDrag", this.OnDrag);
        UEventManager.register("OnPointerUp", this.OnPointerUp);

        return;
        //if (lockCursor)
        //{
        //    Cursor.lockState = CursorLockMode.Locked;
        //    Cursor.visible = false;
        //}
    }

    void LateUpdate()
    {
        return;
        yaw += Input.GetAxis("Mouse X") * mouseSensitivity;
        pitch -= Input.GetAxis("Mouse Y") * mouseSensitivity;
        pitch = Mathf.Clamp(pitch, pitchMinMax.x, pitchMinMax.y);

        currentRotation = Vector3.SmoothDamp(currentRotation, new Vector3(pitch, yaw), ref rotationSmoothVelocity, rotationSmoothTime);
        transform.eulerAngles = currentRotation;

        transform.position = target.position - transform.forward * dstFromTarget;

    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("111111");
    }

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("22");
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Debug.Log("3333");
    }
}
