using System.Collections;
using UnityEngine;

public class StickScript : MonoBehaviour
{
    [SerializeField] private SpriteRenderer stickSpriteRenderer;
    [SerializeField] private SpriteRenderer arrowSpriteRenderer;

    [SerializeField] private float distanceOffsetForStick;
    private Vector3 targetTransformPosition;

    private void Awake()
    {
        stickSpriteRenderer.enabled = false;
        arrowSpriteRenderer.enabled = false;
    }
    public void DisplayStick(Vector3 position, DraggedDirection draggedDirection)
    {
        switch (draggedDirection)
        {
            case DraggedDirection.Up:
                targetTransformPosition = position;
                targetTransformPosition.y -= distanceOffsetForStick;
                transform.rotation = Quaternion.Euler(0, 0, 270);
                break;
            case DraggedDirection.Down:
                targetTransformPosition = position;
                targetTransformPosition.y += distanceOffsetForStick;
                transform.rotation = Quaternion.Euler(0, 0, 90);
                break;
            case DraggedDirection.Left:
                targetTransformPosition = position;
                targetTransformPosition.x += distanceOffsetForStick;
                transform.rotation = Quaternion.Euler(0, 0, 0);
                break;
            case DraggedDirection.Right:
                targetTransformPosition = position;
                targetTransformPosition.x -= distanceOffsetForStick;
                transform.rotation = Quaternion.Euler(0, 0, 180);
                break;
        }
        transform.position = targetTransformPosition;
        stickSpriteRenderer.enabled = true;
        arrowSpriteRenderer.enabled = true;

        StartCoroutine(HideStick());
    }
    IEnumerator HideStick()
    {
        yield return new WaitForSecondsRealtime(0.2f);
        stickSpriteRenderer.enabled = false;
        arrowSpriteRenderer.enabled = false;
    }
}