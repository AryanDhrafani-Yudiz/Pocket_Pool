using UnityEngine;
using System.Collections;

public class AnimationController : MonoBehaviour
{
    [SerializeField] private RectTransform glow1Transform;
    [SerializeField] private RectTransform glow2Transform;
    [SerializeField] private float shineTargetScale;
    [SerializeField] float timeToLerpShine;
    private bool switchShineCoroutine;
    private float shineScaleModifier = 1f;

    [SerializeField] private RectTransform whiteBallTransform;
    [SerializeField] float timeToLerpWhiteBall;

    [SerializeField] private RectTransform stickTransform;
    [SerializeField] private float speedToMoveStickX;
    [SerializeField] private float speedToMoveStickY;
    [SerializeField] private float unitsToMove;
    private float direction = 1.0f;
    private Vector3 startingPos;

    void Start()
    {
        startingPos = stickTransform.position;
        StartCoroutine(ShineAnimation(shineTargetScale, timeToLerpShine));
        StartCoroutine(BallAnimation());
        StartCoroutine(StickAnimation());
    }
    public void StopAnimations()
    {
        StopAllCoroutines();
    }
    IEnumerator StickAnimation()
    {
        while (true)
        {
            stickTransform.Translate(direction * speedToMoveStickX * Time.deltaTime, direction * speedToMoveStickY * Time.deltaTime, 0);
            if (stickTransform.position.y > startingPos.y + unitsToMove)
            {
                direction = 1;
            }
            else if (stickTransform.position.y < startingPos.y - unitsToMove)
            {
                direction = -1;
            }
            yield return null;
        }
    }
    IEnumerator BallAnimation()
    {
        while (true)
        {
            whiteBallTransform.Rotate(0f, 0f, -5 * timeToLerpWhiteBall * Time.deltaTime);
            yield return null;
        }
    }
    IEnumerator ShineAnimation(float endScaleValue, float duration)
    {
        if (switchShineCoroutine) endScaleValue = 1f;

        float time = 0;
        float startScaleValue = shineScaleModifier;
        while (time < duration)
        {
            shineScaleModifier = Mathf.Lerp(startScaleValue, endScaleValue, time / duration);
            glow1Transform.localScale = Vector3.one * shineScaleModifier; glow2Transform.localScale = Vector3.one * shineScaleModifier;
            time += Time.deltaTime;
            yield return null;
        }
        glow1Transform.localScale = Vector3.one * endScaleValue; glow2Transform.localScale = Vector3.one * endScaleValue;
        shineScaleModifier = endScaleValue;

        switchShineCoroutine = !switchShineCoroutine;
        StartCoroutine(ShineAnimation(shineTargetScale, timeToLerpShine));
    }
}