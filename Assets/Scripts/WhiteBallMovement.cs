using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public enum DraggedDirection
{
    Up,
    Down,
    Right,
    Left
}
public class WhiteBallMovement : MonoBehaviour
{
    [SerializeField] private float speedOfBall;
    [SerializeField] private float distanceOffset;
    private Vector2 startingPos;
    private Vector2 endingPos;

    [SerializeField] private LayerMask layerMask;
    private Vector2 directionToFire;
    private readonly float rayCastDistance = 15f;

    public static bool userInputEnabled = true;
    private bool isHittingBall;
    private bool isHittingCushion;
    private bool isHittingPocket;
    private int cushionHit;
    private readonly string cushion = "Cushion";
    private readonly string pocket = "Pocket";

    [SerializeField] private StickScript stickScript;
    private LevelManager levelManager;
    private SoundManager soundManager;
    private BallMovement ballMovementScript;

    private void Start()
    {
        levelManager = LevelManager.Instance;
        soundManager = SoundManager.Instance;
    }
    void Update()
    {
        if (userInputEnabled && !EventSystem.current.IsPointerOverGameObject()) GetUserInput();
    }
    private void GetUserInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (Vector2.Distance(transform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition)) < 1f)
            {
                startingPos = Input.mousePosition;
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            if (Vector2.Distance(transform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition)) < 10f && startingPos != Vector2.zero)
            {
                endingPos = Input.mousePosition;
                Vector3 dragVectorDirection = (endingPos - startingPos).normalized;
                if (dragVectorDirection != Vector3.zero) GetDragDirection(dragVectorDirection);
            }
            startingPos = Vector2.zero;
        }
    }
    private void GetDragDirection(Vector3 dragVector)
    {
        float positiveX = Mathf.Abs(dragVector.x);
        float positiveY = Mathf.Abs(dragVector.y);
        DraggedDirection draggedDir;
        if (positiveX > positiveY)
        {
            draggedDir = (dragVector.x > 0) ? DraggedDirection.Right : DraggedDirection.Left;
        }
        else
        {
            draggedDir = (dragVector.y > 0) ? DraggedDirection.Up : DraggedDirection.Down;
        }
        userInputEnabled = false; FireRayCast(draggedDir);
    }
    private void FireRayCast(DraggedDirection draggedDirection)
    {
        switch (draggedDirection)
        {
            case DraggedDirection.Up:
                directionToFire = Vector2.up;
                break;
            case DraggedDirection.Down:
                directionToFire = Vector2.down;
                break;
            case DraggedDirection.Left:
                directionToFire = Vector2.left;
                break;
            case DraggedDirection.Right:
                directionToFire = Vector2.right;
                break;
        }
        RaycastHit2D hit = Physics2D.Raycast(transform.position, directionToFire, rayCastDistance, layerMask);

        if (hit.collider.TryGetComponent<BallMovement>(out var ballMovement))
        {
            isHittingBall = true; ballMovementScript = ballMovement;
        }
        else if (hit.collider.CompareTag(cushion))
        {
            isHittingCushion = true; cushionHit++;
        }
        else if (hit.collider.CompareTag(pocket)) isHittingPocket = true;
        ShootTheBall(hit, draggedDirection);
    }
    private void ShootTheBall(RaycastHit2D hit, DraggedDirection draggedDirection)
    {
        stickScript.DisplayStick(transform.position, draggedDirection);
        soundManager.OnShotPlay();
        StartCoroutine(MoveWhiteBall(hit.point, speedOfBall, draggedDirection));
    }
    IEnumerator MoveWhiteBall(Vector2 targetPosition, float speed, DraggedDirection draggedDirection)
    {
        float time = 0;
        Vector2 startPosition = transform.position;
        switch (draggedDirection)
        {
            case DraggedDirection.Up:
                targetPosition.y -= distanceOffset;
                break;
            case DraggedDirection.Down:
                targetPosition.y += distanceOffset;
                break;
            case DraggedDirection.Left:
                targetPosition.x += distanceOffset;
                break;
            case DraggedDirection.Right:
                targetPosition.x -= distanceOffset;
                break;
        }
        float duration = Vector3.Distance(targetPosition, startPosition) / speed;   // To Keep Uniform Speed Over Any Distance , S = D / T
        while (time < duration)
        {
            transform.position = Vector2.Lerp(startPosition, targetPosition, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        transform.position = targetPosition;
        userInputEnabled = true;
        if (isHittingBall) OnBallHit(draggedDirection);
        else if (isHittingCushion) OnCushionHit();
        else if (isHittingPocket) OnPocketHit();
    }
    private void OnBallHit(DraggedDirection draggedDirection)
    {
        ballMovementScript.FireRayCast(draggedDirection); isHittingBall = false;
    }
    private void OnCushionHit()
    {
        soundManager.OnWallHit(); isHittingCushion = false;
        if (cushionHit == 5) levelManager.CheckIfRetryAvailable();
    }
    private void OnPocketHit()
    {
        soundManager.OnBallInHole(); levelManager.CheckIfRetryAvailable();
        isHittingPocket = false; gameObject.SetActive(false);
    }
}