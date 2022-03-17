using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class RandomScaleRot : MonoBehaviour
{
    [SerializeField] private Vector3 minScale;
    [SerializeField] private Vector3 maxScale;

    [SerializeField] private Vector3 minRot;
    [SerializeField] private Vector3 maxRot;

    [SerializeField] private LayerMask noWater;


    [ContextMenu("Random Scale")]
    void RandomScale()
    {
        foreach(Transform child in transform)
        {
            child.localScale = Vector3.Lerp(minScale, maxScale, Random.Range(0f, 1f));
        }
    }

    [ContextMenu("Random Rotation")]
    void RandomRotation()
    {
        foreach(Transform child in transform)
        {
            child.rotation= Quaternion.Euler(Vector3.Lerp(minRot, maxRot, Random.Range(0f, 1f)));
        }
    }

    [ContextMenu("Align with normal")]
    void AlignWithNormal()
    {
        foreach(Transform child in transform)
        {
            if(Physics.Raycast(child.position, Vector3.down, out RaycastHit hit, Mathf.Infinity, noWater))
            {
                child.forward = hit.normal;
                child.position = hit.point + Vector3.up * (child.localScale.y / 2);
                child.localRotation = Quaternion.Euler(child.localEulerAngles.x, child.localEulerAngles.y, Random.Range(0, 360));
                child.localScale *= Random.Range(0.5f, 1.5f);
            }
        }
    }
}
