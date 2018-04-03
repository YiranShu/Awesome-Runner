using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectorScripts : MonoBehaviour {

    private Transform playerTarget = null;

    [SerializeField]
    private float offsetX = -30f;

    void Awake() {
        if (GameObject.FindGameObjectWithTag(Tags.PLAYER_TAG)) {
            playerTarget = GameObject.FindGameObjectWithTag(Tags.PLAYER_TAG).transform;
        }
    }

    void FixedUpdate() {

        if (playerTarget) {
            float x = playerTarget.position.x + offsetX;
            transform.position = new Vector3(x, playerTarget.position.y, playerTarget.position.z);
        }
    }

	void OnTriggerEnter(Collider target) {
        if(target.tag == Tags.PLATFORM_TAG || target.tag == Tags.HEALTH_TAG || target.tag == Tags.MONSTER_TAG) {
            target.gameObject.SetActive(false);
        }
    }
}
