using UnityEngine;

public class Hoverable : MonoBehaviour
{
    //must be on same GameObject as BoxCollider2D
    private bool isHovered;

    public bool GetIsHovered()
    {
        return isHovered;
    }

    void OnMouseOver()
    {
        isHovered = true;
    }

    void OnMouseExit()
    {
        isHovered = false;
    }


    private void OnDisable()
    {
        isHovered = false;
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
