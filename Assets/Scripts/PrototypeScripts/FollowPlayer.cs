using System.Collections;
using System.Collections.Generic;
using Spirinse.System.Player;
using UnityEngine;

public class FollowPlayer : MonoBehaviour {
    public GameObject player;
    public GameObject meditator;
    
    public float speedDelay = 0.4f;
    public bool followPlayer = true;
    public float maxVelocityPerFrame = 3;
    
    private GameObject _gameObject;
    private Vector3 velocity = Vector3.zero;
    
	private void Start ()
    {
        FindPlayer();
        transform.position = player.transform.position;
    }
    
	private void Update () {
        if (!player) {
            FindPlayer();
        }
        if (followPlayer)
        {
            SetPosition();
        }
	}

    private void SetPosition()
    {
        Vector3 nextTarget = player.transform.position - transform.localPosition;
        var distance = Mathf.Min(Vector3.Distance(Vector3.zero, nextTarget), maxVelocityPerFrame);
            
        transform.localPosition = Vector3.SmoothDamp(transform.localPosition, transform.localPosition + nextTarget.normalized * distance, ref velocity, speedDelay);
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