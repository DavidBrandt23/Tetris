using UnityEngine;

public class ActionWhenHovered : MonoBehaviour
{
    public Hoverable hoverableBehavior;
    public GameObject ObjectToShowOnHover;

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if(isActiveAndEnabled &&  hoverableBehavior && ObjectToShowOnHover != null)
        {
            bool isHovered = hoverableBehavior.GetIsHovered();
            ObjectToShowOnHover.SetActive(isHovered);
        }
    }

    private void OnDisable()
    {
        if (ObjectToShowOnHover != null)
        {
            ObjectToShowOnHover.SetActive(false);
        }
    }
}
