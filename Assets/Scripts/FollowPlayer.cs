using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour {
    public GameObject player;
    public GameObject meditator;

    [Range(0f,1f)]
    public float lerpPosition = 0.9f;

    private Vector3 velocity = Vector3.zero;
    public float speedDelay = 0.4f;
    public bool followPlayer = true;
    public delegate void SetCameraPlayerFollow(bool setFollow);
    public event SetCameraPlayerFollow OnCameraFollowPlayer;


	// Use this for initialization
	void Start () {
        OnCameraFollowPlayer += DefaultMethod;
	}
	
	// Update is called once per frame
	private void Update () {
        if (!player) {
            player = GameObject.FindGameObjectWithTag("Player");
        }
        if (followPlayer) {
            transform.localPosition = Vector3.SmoothDamp(transform.localPosition, Vector3.Lerp(player.transform.position,meditator.transform.position,lerpPosition), ref velocity, speedDelay);
        }
	}
    public void SetCameraTrue(bool doFollow) {
        followPlayer = doFollow;
    }
    public void DefaultMethod(bool isItTrue) {

    }
}