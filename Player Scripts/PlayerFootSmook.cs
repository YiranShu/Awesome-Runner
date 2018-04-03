using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFootSmook : MonoBehaviour {

    public GameObject smokeEffect;
    public GameObject smokePosition;

	void OnTriggerEnter(Collider target) {
        if(target.tag == Tags.PLATFORM_TAG) {
            if(smokePosition.activeInHierarchy) {
                Instantiate(smokeEffect, smokePosition.transform.position, Quaternion.identity);
            }
        }
    } 
}
