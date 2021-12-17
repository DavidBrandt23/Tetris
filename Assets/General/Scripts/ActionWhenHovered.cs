using System.Collections.Generic;
using UnityEngine;

public class ActionWhenHovered : MonoBehaviour
{
    public Hoverable hoverableBehavior;
    public List<GameObject> ObjectToShowOnHover;

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

            HoverObjsSetActive(isHovered);
        }
    }

    private void OnDisable()
    {
        if (ObjectToShowOnHover != null)
        {
            HoverObjsSetActive(false);
        }
    }

    private void HoverObjsSetActive(bool active)
    {
        foreach(GameObject ob in ObjectToShowOnHover)
        {
            ob.SetActive(active);
        }
    }
}
