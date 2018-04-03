using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour {
    public float movementSpeed = 5f;
    public float jumpPower = 500f;
    public float secondJumpPower = 650f;
    public Transform groundCheckPosition;
    public float radius = 0.3f;
    public LayerMask layerGround;

    private Rigidbody myBody;
    private bool isGrounded;
    private bool playerJumped = false;
    private bool canDoubleJump = false;

    private bool gameStarted;
    private PlayerAnimation playAnim;

    public GameObject smokePosition;

    private BGScroller bgScroller;

    private PlayerHealthDamageShoot playerShoot;

    private Button jumpBtn;

    void Awake() {
        myBody = GetComponent<Rigidbody>();
        playAnim = GetComponent<PlayerAnimation>();
        bgScroller = GameObject.Find(Tags.BACKGROUND_OBJ).GetComponent<BGScroller>();
        playerShoot = GetComponent<PlayerHealthDamageShoot>();

        jumpBtn = GameObject.Find(Tags.JUMP_BUTTON_OBJ).GetComponent<Button>();
        jumpBtn.onClick.AddListener(() => Jump());
    }

    void Start() {
        StartCoroutine(StartGame());
    }

    void FixedUpdate() {
        if (gameStarted) {
            PlayerMove();
            PlayerGrounded();
            PlayerJump();
        }
    }

    void PlayerMove() {
        myBody.velocity = new Vector3(movementSpeed, myBody.velocity.y, 0f);
    }

    void PlayerGrounded() {
        isGrounded = (Physics.OverlapSphere(groundCheckPosition.position, radius, layerGround).Length > 0);
    }

    void PlayerJump() {
        if(Input.GetKeyUp(KeyCode.Space) && isGrounded) {
            playAnim.DidJump();
            myBody.AddForce(new Vector3(0f, jumpPower, 0f));
            playerJumped = true;
            canDoubleJump = true;
        } else if(Input.GetKeyDown(KeyCode.Space) && !isGrounded && canDoubleJump) {
            myBody.AddForce(new Vector3(0f, secondJumpPower, 0f));
            canDoubleJump = false;
        }
    }

    public void Jump() {
        if (isGrounded) {
            playAnim.DidJump();
            myBody.AddForce(new Vector3(0f, jumpPower, 0f));
            playerJumped = true;
            canDoubleJump = true;
        }

        if (!isGrounded && canDoubleJump) {
            myBody.AddForce(new Vector3(0f, secondJumpPower, 0f));
            canDoubleJump = false;
        }
    }

    IEnumerator StartGame() {
        yield return new WaitForSeconds(2f);
        gameStarted = true;
        bgScroller.canScroll = true;
        playerShoot.canShoot = true;
        playAnim.PlayerRun();
        GamePlayController.instance.canCountScore = true;
        smokePosition.SetActive(true);
    }

    void OnCollisionEnter(Collision target) {
        if(target.gameObject.tag == Tags.PLATFORM_TAG) {
            if(playerJumped) {
                playerJumped = false;
                playAnim.DidLand();
            }
        }
    }
}
