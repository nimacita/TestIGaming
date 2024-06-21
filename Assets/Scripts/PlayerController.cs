using UnityEngine;

public class PlayerController : MonoBehaviour
{


    [Header("Player Settings")]
    [Tooltip("Сила толчка игрока при нажатии")]
    [SerializeField] private float pushForce;
    [Tooltip("Скорость поворота игрока в направлении")]
    [SerializeField] private float rotateSpeed;
    private bool isLose;
    private bool isPlay;


    [Header("Editor")]
    public Transform ravenSpriteTr;
    private Animator animator;
    private Rigidbody2D rb;
    private float downYBorder, upYBorder;
    private float startGraviryScale;

    private PlayerSoundController soundController;
    public static PlayerController instance;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        isLose = false;
        isPlay = false;

        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        soundController = GetComponent<PlayerSoundController>();

        startGraviryScale = rb.gravityScale;
        rb.gravityScale = 0f;

        downYBorder = -4f;
        upYBorder = 5.5f;

        animator.Play("RavenLoopAnim");
    }

    
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isLose && isPlay)
        {
            //если не симулируем гравитацию, то начинаепм
            if (rb.gravityScale <= 0f)
            {
                rb.gravityScale = startGraviryScale;
                //останавливаем начальную анимацию
                animator.SetBool("StartLoop", false);
            }
            PushPlayer();
        }
        RotatePlayer();
        IsUnderBorder();
    }

    //толкаем игрока вверх
    private void PushPlayer()
    {
        rb.velocity = Vector2.up * pushForce;
        animator.Play("RavenAnim");
    }

    //если за границами
    private void IsUnderBorder()
    {
        if (transform.position.y < downYBorder)
        {
            if (!isLose)
            {
                Lose();
            }
            soundController.PlayFallSound();
        }
        if (transform.position.y > upYBorder)
        {
            if (!isLose)
            {
                Lose();
            }
        }
    }

    //поворачиваем игрока в сторону направления
    private void RotatePlayer()
    {
        ravenSpriteTr.rotation = Quaternion.Euler(0f,0f,rb.velocity.y * rotateSpeed);
    }

    //проиграли
    private void Lose()
    {
        isLose = true;

        GameController.instance.GameOver();

        if (rb.gravityScale <= 0f)
        {
            rb.gravityScale = startGraviryScale;
            //останавливаем начальную анимацию
            animator.SetBool("StartLoop", false);
        }
        GetComponent<BoxCollider2D>().isTrigger = true;
        PushPlayer();
        //играем звук вороны
        soundController.PlayRavenSound();
    }

    //начинаем игру
    public void StartGame()
    {
        isPlay = true;
    }

    //останавливаем
    public void StopPlayer()
    {
        isPlay = false;
        rb.simulated = false;
    }

    //продолжаем
    public void ContinuePlayer()
    {
        isPlay = true;
        rb.simulated = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Pipe")
        {
            Lose();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "RecordTrigger")
        {
            //прибавляем рекорд
            if (!isLose) 
            {
                GameController.instance.Score++;
            }
        }
    }
}
