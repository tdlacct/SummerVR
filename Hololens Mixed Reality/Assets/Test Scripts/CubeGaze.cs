using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeGaze : MonoBehaviour {
    public Color32 GazeColor;
    public bool IsGaze;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnGaze()
    {
        if (IsGaze == false)
        {
            GetComponent<Renderer>().material.color = GazeColor;
            IsGaze = true;
        }
    }

    public void OffGaze()
    {
        if (IsGaze == true)
        {
            GetComponent<Renderer>().material.color = Color.white;
            IsGaze = false;
        }
    }
}
