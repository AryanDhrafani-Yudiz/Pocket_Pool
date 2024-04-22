using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonAnimation : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Button Button;

    private Vector3 defaultScale;
    [SerializeField] private Vector3 targetScale;

    private void Start()
    {
        defaultScale = transform.localScale;
    }
    public void IncreaseScale(Button button)
    {
        button.transform.localScale = targetScale;
    }
    public void Defaultscale(Button button)
    {
        button.transform.localScale = defaultScale;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        IncreaseScale(Button);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        Defaultscale(Button);
    }
}