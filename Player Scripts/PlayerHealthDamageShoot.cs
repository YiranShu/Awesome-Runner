using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthDamageShoot : MonoBehaviour {

    [SerializeField]
    private Transform playerBullet;

    private float distanceBeforeNewPlatforms = 120f;
    private LevelGenerator levelGenerator;
    private LevelGeneratorPooling levelGeneratorPooling;

    [HideInInspector]
    public bool canShoot;

	void Awake () {
        levelGenerator = GameObject.Find(Tags.LEVEL_GENERATOR_OBJ).GetComponent<LevelGenerator>();
        levelGeneratorPooling = GameObject.Find(Tags.LEVEL_GENERATOR_OBJ).GetComponent<LevelGeneratorPooling>();
    }

    void FixedUpdate () {
        Fire();
	}

    void Fire() {
        if(Input.GetKeyDown(KeyCode.S)) {
            if (canShoot) {
                Vector3 bulletPos = transform.position;
                bulletPos.y += 1.5f;
                bulletPos.x += 1f;
                Transform newBullet = (Transform)Instantiate(playerBullet, bulletPos, Quaternion.identity);
                newBullet.GetComponent<Rigidbody>().AddForce(transform.forward * 1500f);
                newBullet.parent = transform;
            }
        }
    }

    void OnTriggerEnter(Collider target) {
        if(target.tag == Tags.MONSTER_BULLET_TAG || target.tag == Tags.BOUNDS_TAG) {
            // imform the game controller that the player has died
            GamePlayController.instance.TakeDamage();
            Destroy(gameObject);
        }

        if(target.tag == Tags.HEALTH_TAG) {
            // imform the game controller that the player has collected a health
            GamePlayController.instance.IncrementHealth();
            target.gameObject.SetActive(false);
        }

        if(target.tag == Tags.MORE_PLATFORMS_TAG) {
            Vector3 temp = target.transform.position;
            temp.x += distanceBeforeNewPlatforms;
            target.transform.position = temp;

            //levelGenerator.GenerateLevel(false);
            levelGeneratorPooling.PoolingPlatforms();
        }
    }

    void OnCollisionEnter(Collision target) {
        if(target.gameObject.tag == Tags.MONSTER_TAG) {
            GamePlayController.instance.TakeDamage();
            Destroy(gameObject);
        }
    }
}
