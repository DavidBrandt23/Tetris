using UnityEngine;
using UnityEngine.Events;

public class Clickable : MonoBehaviour
{
    public UnityEvent OnClicked;
    public UnityEvent OnEnableEvent;
    public UnityEvent OnDisableEvent;
    public BoxCollider2D myBoxCollider2D;

    // Use this for initialization
    void Start()
    {
    }

    void Update()
    {
    }

    private void OnMouseUp()
    {
        if (isActiveAndEnabled)
        {
            OnClicked.Invoke();
        }
    }

    void OnEnable()
    {
        if (myBoxCollider2D == null)
        {
            myBoxCollider2D = GetComponent<BoxCollider2D>();
        }
        OnEnableEvent.Invoke();
        myBoxCollider2D.enabled = true;
    }

    void OnDisable()
    {
        OnDisableEvent.Invoke();
        myBoxCollider2D.enabled = false;
    }
}
