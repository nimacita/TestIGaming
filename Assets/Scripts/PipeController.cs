using UnityEngine;

public class PipeController : MonoBehaviour
{

    [Header("Main Settings")]
    [SerializeField, Tooltip("�������� ����")]
    private float pipeSpeed;
    [SerializeField, Tooltip("���������� ����� ����, �� ������������ �� �������������")]
    private Vector2 pipeBetweenDist;
    [SerializeField, Tooltip("������ ����, �� ������������ �� �������������")]
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

    //��������� ����������� ����
    private void StartPipePositing()
    {
        RandPipePos(0, true);
        for (int i = 1; i < pipes.Length; i++) 
        {
            RandPipePos(i);
        }
    }

    //������� �����
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

    //��������� �� �������� �� ����� �������� ����
    private void CheckPipe(int pipeInd)
    {
        if (pipes[pipeInd].transform.position.x < -6f)
        {
            RandPipePos(pipeInd);
        }
    }

    //�������� �������������� �����
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
        //��������� �� ������� �����
        randX += pipes[prevInd].transform.position.x;

        if (!isStartPipe)
        {
            //���� �� ��������� �����
            pipes[pipeInd].transform.position = new Vector2(randX, randY);
        }
        else
        {
            //���� ��������� �����
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
