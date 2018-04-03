using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour {

    public GameObject monsterDiedEffect;
    public Transform bullet;
    public float distanceFromPlayerToStartMove = 20f;
    public float movementSpeed_Min = 1f;
    public float movementSpeed_Max = 2f;

    private bool moveRight;
    private float movementSpeed;
    private bool isPlayerInRegion = false;

    private Transform playerTransform;

    public bool canShoot;

    private string FUNCTION_TO_INVOKE = "StartShooting";

    void Start () {
        if (Random.Range(0.0f, 1.0f) > 0.5f) {
            moveRight = true;
        }
        else {
            moveRight = false;
        }

        movementSpeed = Random.Range(movementSpeed_Min, movementSpeed_Max);

        playerTransform = GameObject.FindGameObjectWithTag(Tags.PLAYER_TAG).transform;
	}
	
	void Update () {
		if(playerTransform) {
            float distanceFromPlayer = (playerTransform.position - transform.position).magnitude;
            if(distanceFromPlayer < distanceFromPlayerToStartMove) {
                if (moveRight) {
                    transform.position = new Vector3(transform.position.x + Time.deltaTime * movementSpeed, transform.position.y, transform.position.z);
                }
                else {
                    transform.position = new Vector3(transform.position.x - Time.deltaTime * movementSpeed, transform.position.y, transform.position.z);
                }

                if (!isPlayerInRegion) {
                    if(canShoot) {
                        InvokeRepeating(FUNCTION_TO_INVOKE, 0.5f, 1.5f);
                    }

                    isPlayerInRegion = true;
                }
            } else {
                CancelInvoke(FUNCTION_TO_INVOKE);
            }
        }

        // I add myself!!!
        if(transform.position.y <= -30f) {
            MonsterDied();
        }
        //!!!
	}

    void StartShooting() {
        if(playerTransform != null) {
            Vector3 bulletPos = transform.position;
            bulletPos.y += 1.5f;
            bulletPos.x -= 1.0f;
            Transform newBullet = (Transform)Instantiate(bullet, bulletPos, Quaternion.identity);
            newBullet.GetComponent<Rigidbody>().AddForce(transform.forward * 1500f);
            newBullet.parent = transform;
        }
    }

    void MonsterDied() {
        Vector3 effectPos = transform.position;
        effectPos.y += 2f;
        Instantiate(monsterDiedEffect, effectPos, Quaternion.identity);
        gameObject.SetActive(false); // Destroy(gameObject);
    }

    void OnTriggerEnter(Collider target) {
        if(target.tag == Tags.PLAYER_BULLET_TAG || target.tag == Tags.BOUNDS_TAG) {
            if(target.tag == Tags.PLAYER_BULLET_TAG) {
                GamePlayController.instance.IncrementScore(200f);
            }
            MonsterDied();
        }
    }

    void OnCollisionEnter(Collision target) {
        if(target.collider.tag == Tags.PLAYER_TAG) {
            MonsterDied();
        }
    }
}
