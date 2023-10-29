using System.Collections;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public Transform direction;
    [SerializeField] private float leftBorder;
    [SerializeField] private float rightBorder;
    [SerializeField] private float bottomBorder;
    public Animator animator;

    private bool reloading;
    public bool inNat;
    private bool inCannon;
    [HideInInspector] public Rigidbody2D rb;
    private TrailRenderer tr;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        tr = GetComponent<TrailRenderer>();
        
        StartCoroutine(CheckNat());
        StartCoroutine(CheckVelocity());
    }

    public IEnumerator SwitchTrail()
    {
        tr.enabled = false;
        yield return new WaitForSeconds(0.5f);
        tr.enabled = true;
    }

    private void FixedUpdate()
    {
        rb.velocity = Vector3.ClampMagnitude(rb.velocity, 10);
    }

    private void Update()
    {
        if ((transform.position.x < leftBorder || transform.position.x > rightBorder || transform.position.y < bottomBorder) && !GameManager.instance.allowDrag)
        {
            DisablePhysics();
            GameManager.instance.RestartGame();
        }
    }

    private void OnMouseDown()
    {
        if (GameManager.instance.allowDrag)
            GameManager.instance.StartGame();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        SoundController.instance.PlayBallSound();
        
        if (other.gameObject.tag == "Net")
        {
            GameManager.instance.ShieldAnim.Play();
            SoundController.instance.PlayNetSound();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Finish")
        {
            GameManager.instance.ShieldAnim.Play();
            SoundController.instance.PlayNetSound();
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (!GameManager.instance.allowDrag)
        {
            inNat = other.gameObject.tag == "Finish" ? true : false;
            inCannon = other.gameObject.tag == "Respawn" ? true : false;
        }
    }

    private void OnCollisionExit2D(Collision2D col)
    {
        inNat = false;
        inCannon = false;
    }

    private IEnumerator CheckNat()
    {
        yield return new WaitForSeconds(3);

        if (inCannon)
        {
            StartCoroutine(RespawnBall(GameManager.instance.CannonBallPosition.position));
            inCannon = false;
        }
        if (inNat && GameManager.instance.isGame)
            StartCoroutine(GameManager.instance.EndGame());

        StartCoroutine(CheckNat());
    }

    private IEnumerator CheckVelocity()
    {
        yield return new WaitForSeconds(2);
        if (Mathf.Abs(rb.velocity.x) < 0.1f && Mathf.Abs(rb.velocity.y) < 0.1f && !reloading && !GameManager.instance.allowDrag && !inNat && !inCannon)
            GameManager.instance.RestartGame();

        StartCoroutine(CheckVelocity());
    }

    public IEnumerator RespawnBall(Vector3 position, bool isCannon = false)
    {
        if (isCannon)
            animator.SetBool("isPlaying", true);
        
        transform.position = position;
        
        reloading = true;
        DisablePhysics();

        yield return new WaitForSeconds(1);

        if (!isCannon && GameManager.instance.isGame)
        {
            rb.isKinematic = false;
            SoundController.instance.PlayShotSound();
            GameManager.instance.CannonBombAnimation.Play();
            rb.AddRelativeForce(GameManager.instance.CannonDirectionPosition.position * 2.5f, ForceMode2D.Impulse);
        }
        reloading = false;
    }

    private void DisablePhysics()
    {
        rb.rotation = 0;
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0;
        rb.isKinematic = true;
    }
}
