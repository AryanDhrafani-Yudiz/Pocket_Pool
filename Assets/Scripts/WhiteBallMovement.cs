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
        if (!isCoroutineRunning)
        {
            if (Input.GetMouseButtonDown(0))    // When First Clicked At A Point On Screen
            {
                if (Vector2.Distance(transform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition)) < 0.5f)
                {
                    startingPos = Input.mousePosition;
                    //Debug.Log("Starting Pose: " + startingPos);
                    //Debug.Log("distance : " + Vector2.Distance(transform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition)));
                }
            }
            else if (Input.GetMouseButtonUp(0))    // When First Clicked At A Point On Screen
            {
                if (Vector2.Distance(transform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition)) < 2f)
                {
                    endingPos = Input.mousePosition;
                    //Debug.Log("ending Pose: " + endingPos);
                    Vector3 dragVectorDirection = (endingPos - startingPos).normalized;
                    GetDragDirection(dragVectorDirection);
                }
            }
        }
        #region
        //if (Input.touchCount > 0)
        //{
        //    Touch touch = Input.GetTouch(0);
        //    if (!EventSystem.current.IsPointerOverGameObject(0))
        //    {
        //        if (touch.phase == TouchPhase.Began)
        //        {
        //            startingPos = touch.position;
        //        }
        //        else if (touch.phase == TouchPhase.Ended)
        //        {
        //            endingPos = touch.position;
        //            Vector3 dragVectorDirection = (endingPos - startingPos).normalized;
        //            GetDragDirection(dragVectorDirection);
        //        }
        //    }
        //}
        #endregion
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
        //Debug.Log(draggedDir);
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
        //Debug.Log("Raycast fired in direction : " + directionToFire);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, directionToFire, 30f, layerMask);
        if (hit.collider.CompareTag("Cushion"))
        {
            //Debug.Log("Raycast Hitted Point: " + hit.point);
            StartCoroutine(MoveTheBall(hit.point, speedOfBall, draggedDirection));
        }
        else if (hit.collider.CompareTag("Ball"))
        {
            isHittingBall = true;
            ballMovementScript = hit.collider?.GetComponent<BallMovement>();
            StartCoroutine(MoveTheBall(hit.point, speedOfBall, draggedDirection));
        }
        else if (hit.collider.CompareTag("Pocket"))
        {
            isHittingPocket = true;
            StartCoroutine(MoveTheBall(hit.point, speedOfBall, draggedDirection));
        }
    }
    IEnumerator MoveTheBall(Vector2 targetPosition, float speed, DraggedDirection draggedDirection)
    {
        float time = 0;
        Vector2 startPosition = transform.position;
        isCoroutineRunning = true;
        //Debug.Log("CoRoutine Started");
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
        float duration = Vector3.Distance(targetPosition, startPosition) / speed;
        while (time < duration)
        {
            transform.position = Vector2.Lerp(startPosition, targetPosition, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        transform.position = targetPosition;
        if (isHittingBall) { ballMovementScript?.PottableBallMovement(draggedDirection); isHittingBall = false; }
        else if (isHittingPocket) { gameObject.SetActive(false); }
        isCoroutineRunning = false;
    }
}