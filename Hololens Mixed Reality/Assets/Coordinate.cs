using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_WSA && UNITY_2017_2_OR_NEWER
using System.Collections.Generic;
using UnityEngine.XR.WSA.Input;
#endif

public class Coordinate: MonoBehaviour {
#if UNITY_WSA && UNITY_2017_2_OR_NEWER
    private class ControllerState
    {
        public InteractionSourceHandedness Handedness;
        public Vector3 PointerPosition;
        public Quaternion PointerRotation;
        public Vector3 GripPosition;
        public Quaternion GripRotation;
        public bool Grasped;
        public bool MenuPressed;
        public bool SelectPressed;
        public float SelectPressedAmount;
        public bool ThumbstickPressed;
        public Vector2 ThumbstickPosition;
        public bool TouchpadPressed;
        public bool TouchpadTouched;
        public Vector2 TouchpadPosition;
    }

    private Dictionary<uint, ControllerState> controllers;
#endif

    public int x;
    public int y;
    public Material [,] array2D = new Material [5,4];
    public Material[] MList;
    public GameObject Sphere;
    public Camera MR;
    // Use this for initialization

    private void Awake()
    {
#if UNITY_WSA && UNITY_2017_2_OR_NEWER
        controllers = new Dictionary<uint, ControllerState>();

        InteractionManager.InteractionSourceDetected += InteractionManager_InteractionSourceDetected;

        InteractionManager.InteractionSourceLost += InteractionManager_InteractionSourceLost;
        InteractionManager.InteractionSourceUpdated += InteractionManager_InteractionSourceUpdated;
#endif
    }

    void LoadMaterial ()
    {
        int v = 0;
        int i = 0;
        for (i = 0; i < MList.Length;)
        {
            for (int h = 0; h < array2D.GetLength(0) && v < array2D.GetLength(1); h++)
            {
                array2D[h, v] = MList[i];
                Debug.Log(array2D[h, v]);
                i++;
            }
            v++;
            
        }
    }
	
	// Update is called once per frame
	void Start () {
        LoadMaterial();
        UpdateCoordinates();
       
    }

    private void Update()
    {
        if (Input.GetKeyDown("left") && x > 0) {
            --x;
            UpdateCoordinates();
        }
        if (Input.GetKeyDown("right") && x < (array2D.GetLength(0) - 1))
        {
            ++x;
            UpdateCoordinates();
            Debug.Log("UPDATE");
        }
        if (Input.GetKeyDown("up") && y > 0)
            {
                --y;
                UpdateCoordinates();
            }
        if (Input.GetKeyDown("down") && y < (array2D.GetLength(1)-1))
        {
            ++y;
            UpdateCoordinates();
        }
    }

    public void UpdateCoordinates()
    {
        Sphere.GetComponent<MeshRenderer>().material = array2D[x, y];

    }
    #if UNITY_WSA && UNITY_2017_2_OR_NEWER
    private void InteractionManager_InteractionSourceDetected(InteractionSourceDetectedEventArgs obj)
    {
        Debug.LogFormat("{0} {1} Detected", obj.state.source.handedness, obj.state.source.kind);

        if (obj.state.source.kind == InteractionSourceKind.Controller && !controllers.ContainsKey(obj.state.source.id))
        {
            controllers.Add(obj.state.source.id, new ControllerState { Handedness = obj.state.source.handedness });
        }
    }

    private void InteractionManager_InteractionSourceLost(InteractionSourceLostEventArgs obj)
    {
        Debug.LogFormat("{0} {1} Lost", obj.state.source.handedness, obj.state.source.kind);

        controllers.Remove(obj.state.source.id);
    }

    private void InteractionManager_InteractionSourceUpdated(InteractionSourceUpdatedEventArgs obj)
    {
        ControllerState controllerState;
        if (controllers.TryGetValue(obj.state.source.id, out controllerState))
        {
            obj.state.sourcePose.TryGetPosition(out controllerState.PointerPosition, InteractionSourceNode.Pointer);
            obj.state.sourcePose.TryGetRotation(out controllerState.PointerRotation, InteractionSourceNode.Pointer);
            obj.state.sourcePose.TryGetPosition(out controllerState.GripPosition, InteractionSourceNode.Grip);
            obj.state.sourcePose.TryGetRotation(out controllerState.GripRotation, InteractionSourceNode.Grip);

            controllerState.Grasped = obj.state.grasped;
            controllerState.MenuPressed = obj.state.menuPressed;
            controllerState.SelectPressed = obj.state.selectPressed;
            controllerState.SelectPressedAmount = obj.state.selectPressedAmount;
            controllerState.ThumbstickPressed = obj.state.thumbstickPressed;
            controllerState.ThumbstickPosition = obj.state.thumbstickPosition;
            controllerState.TouchpadPressed = obj.state.touchpadPressed;
            controllerState.TouchpadTouched = obj.state.touchpadTouched;
            controllerState.TouchpadPosition = obj.state.touchpadPosition;
        }

        if (controllerState.SelectPressedAmount > 0.02)
        {
            if (MR.transform.rotation.y < 131 && MR.transform.rotation.y > 55 && x < (array2D.GetLength(0) - 1))
            ++x;
            UpdateCoordinates();
        }
        if (controllerState.ThumbstickPosition.x < -0.02 && x > 0)
        {
            --x;
            UpdateCoordinates();
        }
        if (controllerState.ThumbstickPosition.y > 0.02 && y < (array2D.GetLength(1) - 1))
        {
            ++y;
            UpdateCoordinates();
        }
        if (controllerState.ThumbstickPosition.y < -0.02 && y > 0)
        {
            --y;
            UpdateCoordinates();
        }

    }
#endif
}
