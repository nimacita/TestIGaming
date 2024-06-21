using UnityEngine;

public class PipeController : MonoBehaviour
{

    [Header("Main Settings")]
    [SerializeField, Tooltip("Скорость труб")]
    private float pipeSpeed;
    [SerializeField, Tooltip("Расстояние между труб, от минимального до максимального")]
    private Vector2 pipeBetweenDist;
    [SerializeField, Tooltip("Высота труб, от минимального до максимального")]
    private Vector2 pipeHight;
    private bool isMove;


    [Header("Pipes Settings")]
    [SerializeField]
    private GameObject[] pipes;
    private float deathXZone;


    public static PipeController instance;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        deathXZone = -6f;
        StartPipePositing();
    }

    public float PipeSpeed
    {
        get { return pipeSpeed; }
    }

    // Update is called once per frame
    void Update()
    {
        PipeMove();
    }

    //начальное расстановка труб
    private void StartPipePositing()
    {
        RandPipePos(0, true);
        for (int i = 1; i < pipes.Length; i++) 
        {
            RandPipePos(i);
        }
    }

    //двигаем трубы
    private void PipeMove()
    {
        if (isMove)
        {
            for (int i = 0; i < pipes.Length; i++) 
            {
                pipes[i].transform.Translate(Vector2.left * pipeSpeed * Time.deltaTime);
                CheckPipe(i);
            }
        }
    }

    //проверяем не достигла ли труба конечной зоны
    private void CheckPipe(int pipeInd)
    {
        if (pipes[pipeInd].transform.position.x < -6f)
        {
            RandPipePos(pipeInd);
        }
    }

    //рандомим местоположение трубы
    private void RandPipePos(int pipeInd, bool isStartPipe = false)
    {
        float randX = Random.Range(pipeBetweenDist.x,pipeBetweenDist.y);
        float randY = Random.Range(pipeHight.x, pipeHight.y);
        int prevInd;
        if (pipeInd == 0)
        {
            prevInd = pipes.Length - 1;
        }
        else
        {
            prevInd = pipeInd - 1;
        }
        //отступаем от прошлой трубы
        randX += pipes[prevInd].transform.position.x;

        if (!isStartPipe)
        {
            //если не начальная труба
            pipes[pipeInd].transform.position = new Vector2(randX, randY);
        }
        else
        {
            //если начальная труба
            pipes[pipeInd].transform.position = new Vector2(pipes[pipeInd].transform.position.x, randY);
        }
    }

    public void StartMove()
    {
        isMove = true;
    }
    public void StopMove()
    {
        isMove = false;
    }
}
