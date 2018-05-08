using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class VRManager : MonoBehaviour {
    public GameObject OBJ;
	// Use this for initialization
	void Start () {
        XRDevice.SetTrackingSpaceType(TrackingSpaceType.RoomScale);

    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hitInfo;
        if (Physics.Raycast(
                Camera.main.transform.position,
                Camera.main.transform.forward,
                out hitInfo,
                20.0f,
                Physics.DefaultRaycastLayers))
        {
            // If the Raycast has succeeded and hit a hologram
            // hitInfo's point represents the position being gazed at
            // hitInfo's collider GameObject represents the hologram being gazed at
            //Debug.Log(hitInfo.collider);
            hitInfo.collider.gameObject.SendMessage("OnGaze", gameObject, SendMessageOptions.DontRequireReceiver);
            OBJ = hitInfo.collider.gameObject;
        }

        else
        {
            if (OBJ != null)
            {
                OBJ.SendMessage("OffGaze", gameObject, SendMessageOptions.DontRequireReceiver);
                OBJ = null;
            }
        }
    }
}
