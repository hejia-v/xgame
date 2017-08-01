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
        UEventManager.register(UEventManager.EventType.OnPointerDown, new UEventManager.BoolDelegate(this.OnPointerDown), this, true, UEventPriority.third_person_camera);
        UEventManager.register(UEventManager.EventType.OnDrag, new UEventManager.VoidDelegate(this.OnDrag), this, true, UEventPriority.third_person_camera);
        UEventManager.register(UEventManager.EventType.OnPointerUp, new UEventManager.VoidDelegate(this.OnPointerUp), this, true, UEventPriority.third_person_camera);
    }

    void LateUpdate()
    {
    }

    private void UpdateCamera()
    {
        yaw += Input.GetAxis("Mouse X") * mouseSensitivity;
        pitch -= Input.GetAxis("Mouse Y") * mouseSensitivity;
        pitch = Mathf.Clamp(pitch, pitchMinMax.x, pitchMinMax.y);

        currentRotation = Vector3.SmoothDamp(currentRotation, new Vector3(pitch, yaw), ref rotationSmoothVelocity, rotationSmoothTime);
        transform.eulerAngles = currentRotation;

        transform.position = target.position - transform.forward * dstFromTarget;
    }

    public bool OnPointerDown(PointerEventData eventData)
    {
        return true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        //float s = Input.GetAxis("Mouse X");
        //Debug.Log("22    " + s);

        UpdateCamera();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
    }
}
