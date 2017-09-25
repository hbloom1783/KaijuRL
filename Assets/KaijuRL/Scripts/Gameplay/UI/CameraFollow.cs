using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    Vector3 lastPosition;

    void CenterCamera()
    {
        Camera.main.transform.position = new Vector3(
            transform.position.x,
            transform.position.y,
            Camera.main.transform.position.z);
        lastPosition = transform.position;
    }

	// Use this for initialization
	void Start ()
    {
        CenterCamera();
	}
	
	// Update is called once per frame
	void Update ()
    {
		if (lastPosition != transform.position)
        {
            CenterCamera();
        }
	}
}
