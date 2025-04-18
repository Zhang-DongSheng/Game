using UnityEngine;
using System.Collections;

public class camfollow : MonoBehaviour {

public GameObject model;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

	transform.LookAt(model.transform.position);
	
	}
}
