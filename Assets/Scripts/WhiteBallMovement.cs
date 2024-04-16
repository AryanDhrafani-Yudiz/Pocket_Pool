using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    [SerializeField] private float distanceOffsetForCushion;
    private Vector2 startingPos;
    private Vector2 endingPos;

    [SerializeField] private LayerMask layerMask;
    private Vector2 directionToFire;
    private bool isCoroutineRunning;
    private bool isHittingBall;
    private bool isHittingPocket;
    private BallMovement ballMovementScript;

    public void RestartGame()
    {
        SceneManager.LoadSceneAsync(0, LoadSceneMode.Single);
    }
    void Update()
    {
        GetUserInput();
    }
    private void GetUserInput()
    {
        if (!isCoroutineRunning)
        {
            if (Input.GetMouseButtonDown(0))    // When First Clicked At A Point On Screen
            {
                if (Vector2.Distance(transform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition)) < 0.5f)
                {
                    startingPos = Input.mousePosition;
                }
            }
            else if (Input.GetMouseButtonUp(0))    // When First Clicked At A Point On Screen
            {
                if (Vector2.Distance(transform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition)) < 5f)
                {
                    endingPos = Input.mousePosition;
                    Vector3 dragVectorDirection = (endingPos - startingPos).normalized;
                    GetDragDirection(dragVectorDirection);
                }
            }
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
        FireRayCast(draggedDir);
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
        RaycastHit2D hit = Physics2D.Raycast(transform.position, directionToFire, 15f, layerMask);
        if (hit.collider.CompareTag("Ball"))
        {
            isHittingBall = true;
            ballMovementScript = hit.collider?.GetComponent<BallMovement>();
        }
        else if (hit.collider.CompareTag("Pocket"))
        {
            isHittingPocket = true;
        }
        StartCoroutine(MoveWhiteBall(hit.point, speedOfBall, draggedDirection));
    }
    IEnumerator MoveWhiteBall(Vector2 targetPosition, float speed, DraggedDirection draggedDirection)
    {
        isCoroutineRunning = true;
        float time = 0;
        Vector2 startPosition = transform.position;
        switch (draggedDirection)
        {
            case DraggedDirection.Up:
                targetPosition.y -= distanceOffsetForCushion;
                break;
            case DraggedDirection.Down:
                targetPosition.y += distanceOffsetForCushion;
                break;
            case DraggedDirection.Left:
                targetPosition.x += distanceOffsetForCushion;
                break;
            case DraggedDirection.Right:
                targetPosition.x -= distanceOffsetForCushion;
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
        if (isHittingBall) { ballMovementScript?.FireRayCast(draggedDirection); isHittingBall = false; }
        else if (isHittingPocket) { gameObject.SetActive(false); }
        isCoroutineRunning = false;
    }
}