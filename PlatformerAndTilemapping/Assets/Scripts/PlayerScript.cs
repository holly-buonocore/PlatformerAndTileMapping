using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{
    private Rigidbody2D rd2d;
    public float speed;
    public float jumpForce;
    public Text score;
    private int scoreValue;
    public GameObject win;
    private bool isOnGround;
    public Transform groundcheck;
    public float checkRadius;
    public LayerMask allGround;
    public Text lives;
    private int livesValue;
    public GameObject lose;
    public AudioClip musicClipOne;
    public AudioClip musicClipTwo;
    public AudioSource musicSource;
    Animator anim;
    private bool facingRight = true;

    // Start is called before the first frame update
    void Start()
    {
        rd2d = GetComponent<Rigidbody2D>();
        rd2d.simulated = true;

        scoreValue = 0;
        score.text = "Score = " + scoreValue.ToString();
        win.SetActive(false);

        livesValue = 3;
        lives.text = "Lives = " + livesValue.ToString();
        lose.SetActive(false);

        musicSource.clip = musicClipOne;
        musicSource.Play();
        musicSource.loop = true;

        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float hozMovement = Input.GetAxis("Horizontal");
        float verMovement = Input.GetAxis("Vertical");

        rd2d.AddForce(new Vector2(hozMovement*speed, verMovement*speed));

        isOnGround = Physics2D.OverlapCircle(groundcheck.position, checkRadius, allGround);

        if (facingRight == false && hozMovement > 0)
        {
            Flip();
        }
        else if (facingRight == true && hozMovement < 0)
        {
            Flip();
        }

        if(verMovement > 0 && isOnGround == false)
        {
            anim.SetInteger("State", 2);
        }
        if(verMovement == 0 && isOnGround==true && hozMovement == 0)
        {
            anim.SetInteger("State", 0);
        }
        if(hozMovement != 0 && isOnGround == true)
        {
            anim.SetInteger("State", 1);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.tag == "Coin")
        {
            scoreValue += 1;
            score.text = "Score = " + scoreValue.ToString();
            Destroy(collision.collider.gameObject);

            if(scoreValue == 4)
            {
                livesValue = 3;
                lives.text = "Lives = " + livesValue.ToString();
                transform.position = new Vector2(44.87f, -0.07f);
            }
            if(scoreValue == 8)
            {
                win.SetActive(true);
                rd2d.simulated = false;

                musicSource.clip = musicClipTwo;
                musicSource.Play();
                musicSource.loop = false;
            }
        }

        if(collision.collider.tag == "Enemy")
        {
            livesValue -= 1;
            lives.text = "Lives = " + livesValue.ToString();
            Destroy(collision.collider.gameObject);

            if(livesValue == 0)
            {
                lose.SetActive(true);
                Destroy(this);
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.tag == "Ground" && isOnGround)
        {
            if (Input.GetKey(KeyCode.W))
            {
                rd2d.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
            }
        }
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector2 Scaler = transform.localScale;
        Scaler.x = Scaler.x * -1;
        transform.localScale = Scaler;
    }
}
