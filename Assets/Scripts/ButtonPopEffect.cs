using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonPopEffect : MonoBehaviour,
    IPointerDownHandler,
    IPointerUpHandler,
    IPointerEnterHandler,
    IPointerExitHandler
{
    Vector3 originalScale;

    void Start()
    {
        originalScale = transform.localScale;
    }

    // 마우스 올렸을 때 (Hover)
    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.localScale = originalScale * 1.1f; // 살짝 커짐
    }

    // 마우스 벗어날 때
    public void OnPointerExit(PointerEventData eventData)
    {
        transform.localScale = originalScale;
    }

    // 클릭 누를 때
    public void OnPointerDown(PointerEventData eventData)
    {
        transform.localScale = originalScale * 0.9f; // 눌림
    }

    // 클릭 뗄 때
    public void OnPointerUp(PointerEventData eventData)
    {
        transform.localScale = originalScale * 1.1f; // 다시 hover 상태로
    }
}