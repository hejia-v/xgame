using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Invector;
using UnityEngine.Events;

[RequireComponent(typeof(BoxCollider))]
[vClassHeader("vSimpleTrigger")]
public class vSimpleTrigger : vMonoBehaviour
{
    [System.Serializable]
    public class vTriggerEvent : UnityEvent<Collider> { }
    public List<string> tagsToDetect = new List<string>() { "Player" };
    public LayerMask layerToDetect = 0<<1;
    public vTriggerEvent onTriggerEnter ;
    public vTriggerEvent onTriggerExit;
    [HideInInspector]
    public bool inCollision;
    private bool triggerStay;

    void OnDrawGizmos()
    {

        Color red = new Color(1, 0, 0, 0.5f);
        Color green = new Color(0, 1, 0, 0.5f);       
        Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, (transform.lossyScale));
        Gizmos.color = inCollision && Application.isPlaying ? red : green;
        Gizmos.DrawCube(Vector3.zero, Vector3.one);

    }
    void Start()
    {
        inCollision = false;
        gameObject.GetComponent<Collider>().isTrigger = true;      
    }
   
    void OnTriggerEnter(Collider other)
    {
        if (tagsToDetect.Contains(other.gameObject.tag) && IsInLayerMask(other.gameObject, layerToDetect))
        {
            inCollision = true;
            onTriggerEnter.Invoke(other);         
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (tagsToDetect.Contains(other.gameObject.tag) && IsInLayerMask(other.gameObject, layerToDetect))
        {
            inCollision = false;
            onTriggerExit.Invoke(other);     
        }
    }  

    bool IsInLayerMask(GameObject obj, LayerMask mask)
    {
        return ((mask.value & (1 << obj.layer)) > 0);
    }
}
