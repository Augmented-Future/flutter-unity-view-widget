using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class controlRecognition : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public UnityEvent onDown;
    public UnityEvent onUp;
    public UnityEvent onStart;
    bool started = false;
    public void OnPointerDown(PointerEventData eventData)
    {
        if (!started)
        {
            started = true;
            onStart.Invoke();
        }
        onDown.Invoke();
        Debug.Log("ondowninvoke");
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        onUp.Invoke();
        Debug.Log("onupinvoke");
    }
}
