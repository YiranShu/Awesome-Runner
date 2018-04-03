using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectorScripts : MonoBehaviour {

	void OnTriggerEnter(Collider target) {
        if(target.tag == Tags.PLATFORM_TAG || target.tag == Tags.HEALTH_TAG || target.tag == Tags.MONSTER_TAG) {
            target.gameObject.SetActive(false);
        }
    }
}
