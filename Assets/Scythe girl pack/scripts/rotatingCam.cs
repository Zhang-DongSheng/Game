using UnityEngine;
using System.Collections;

public class rotatingCam : MonoBehaviour {
 float velocity = 60;

 Quaternion grades;
 Quaternion grades2;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		

		grades =  Quaternion.Euler(0,Input.GetAxis("Mouse ScrollWheel")* velocity,0);
		
		grades2 =transform.rotation * grades;

		transform.rotation = grades2;
	
	}
}
