using System.Collections;
using UnityEngine;

public class BallMovement : MonoBehaviour
{
    [SerializeField] private float distanceOffset;
    [SerializeField] private LayerMask layerMask;
    private Vector2 directionToFire;
    [SerializeField] private float speedOfBall;
    private bool isHittingPocket;
    [SerializeField] private TableManager tableManager;

    private void Awake()
    {
        tableManager = GetComponentInParent<TableManager>();
    }
    public void FireRayCast(DraggedDirection draggedDirection)
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
        if (hit.collider.CompareTag("Pocket"))
        {
            isHittingPocket = true;
        }
        StartCoroutine(MoveTheBall(hit.point, speedOfBall, draggedDirection));
    }
    IEnumerator MoveTheBall(Vector2 targetPosition, float speed, DraggedDirection draggedDirection)
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
        tableManager.DeleteBall(gameObject);
        if (isHittingPocket) { gameObject.SetActive(false); isHittingPocket = false; }
    }
}