using UnityEngine;
using System.Collections;

public class footSteps : MonoBehaviour {

public AudioClip footStp1;
public AudioClip footStp2;
public AudioSource audio;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void footStep1()

		{
			audio.PlayOneShot(footStp1);
		}

	public void footStep2()

		{
			audio.PlayOneShot(footStp2);
		}
}

