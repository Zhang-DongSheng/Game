using UnityEngine;
using System.Collections;

public class camPosition : MonoBehaviour {

public Vector3 startPosition;
public Vector3 camPosition1;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

	transform.LookAt(startPosition);
	transform.position= Vector3.Lerp(transform.position, camPosition1, 0.1f);
	
	}
}
