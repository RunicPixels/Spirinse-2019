using System.Collections;
using System.Collections.Generic;
using Spirinse.System;
using UnityEngine;

public class FollowPlayer : MonoBehaviour {
    public GameObject player;
    public GameObject meditator;

    [Range(0f,1f)]
    public float lerpPosition = 0.9f;
    public float speedDelay = 0.4f;
    public bool followPlayer = true;
    
    private GameObject _gameObject;
    private Vector3 velocity = Vector3.zero;
    
	private void Start ()
    {
        FindPlayer();
    }
    
	private void Update () {
        if (!player) {
            FindPlayer();
        }
        if (followPlayer) {
            transform.localPosition = Vector3.SmoothDamp(transform.localPosition, Vector3.Lerp(player.transform.position,meditator.transform.position,lerpPosition), ref velocity, speedDelay);
        }
	}
    
    public void FindPlayer()
    {
        player = PlayerManager.Instance.GetPlayer().defender.tracker.gameObject;
        meditator = PlayerManager.Instance.GetPlayer().meditator.gameObject;
    }
    
    public void SetCameraTrue(bool doFollow) {
        followPlayer = doFollow;
    }
}