using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapObject : MonoBehaviour {

    [HideInInspector]
    public int ElementID = 0;

    public Transform StartTransform;

    private void Start()
    {
        StartTransform = gameObject.transform;
    }

    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Destroy(gameObject);
        }
    }
}
