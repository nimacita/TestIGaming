using UnityEngine;

public class ParallaxController : MonoBehaviour
{

    [Header("Parallax Settings")]
    [Tooltip(" оэфициента к скорости дл€ разных частей фона")]
    [SerializeField, Range(0f, 1f)] private float[] parallaxStreight;
    [SerializeField] private Transform[] parallaxParts;
    [Tooltip("—корость прокуртки фона")]
    private float mainSpeed;
    private float deathXZone, repeatXZone;
    private bool isMove;

    public static ParallaxController instance;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        mainSpeed = PipeController.instance.PipeSpeed;

        //устанавливаем координаты дл€ обновлен€и фона при движении
        deathXZone = -100f;
        repeatXZone = 52f;

        StartMove();
    }

    
    void Update()
    {
        MoveParts();
    }

    //двигаем части фона
    private void MoveParts()
    {
        if (isMove) 
        {
            for (int i = 0; i < parallaxParts.Length; i++)
            {
                parallaxParts[i].Translate(Vector2.left * (mainSpeed * parallaxStreight[i]) * Time.deltaTime);
                RepeatParts(parallaxParts[i]);
            }
        }
    }

    //если часть зашла за зону, обновл€ем
    private void RepeatParts(Transform currPart)
    {

        if (currPart.localPosition.x <= deathXZone) 
        {
            currPart.localPosition = new Vector2(repeatXZone, currPart.localPosition.y);
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
