using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class WhiteBallMovement : MonoBehaviour
{
    private enum DraggedDirection
    {
        Up,
        Down,
        Right,
        Left
    }
    [SerializeField] private float timeToMoveBall;
    [SerializeField] private float distanceOffsetForCushion;
    private Vector2 startingPos;
    private Vector2 endingPos;

    [SerializeField] private LayerMask layerMask;
    private Vector2 directionToFire;

    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (!EventSystem.current.IsPointerOverGameObject(0))
            {
                if (touch.phase == TouchPhase.Began)
                {
                    startingPos = touch.position;
                }
                else if (touch.phase == TouchPhase.Ended)
                {
                    endingPos = touch.position;
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
        if (hit.collider != null)
        {
            //Debug.Log("Raycast Hitted Point: " + hit.point);
            StartCoroutine(MoveTheBall(hit.point, timeToMoveBall, draggedDirection));
        }
    }
    IEnumerator MoveTheBall(Vector2 targetPosition, float duration, DraggedDirection draggedDirection)
    {
        float time = 0;
        Vector2 startPosition = transform.position;
        //Debug.Log("CoRoutine Started");
        switch (draggedDirection)
        {
            case DraggedDirection.Up:
                targetPosition.y = targetPosition.y - distanceOffsetForCushion;
                break;
            case DraggedDirection.Down:
                targetPosition.y = targetPosition.y + distanceOffsetForCushion;
                break;
            case DraggedDirection.Left:
                targetPosition.x = targetPosition.x + distanceOffsetForCushion;
                break;
            case DraggedDirection.Right:
                targetPosition.x = targetPosition.x - distanceOffsetForCushion;
                break;
        }
        while (time < duration)
        {
            transform.position = Vector2.Lerp(startPosition, targetPosition, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        transform.position = targetPosition;
    }
}