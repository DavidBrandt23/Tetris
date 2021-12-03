using UnityEngine;

public class Selectable : MonoBehaviour
{
    public GameObject ObjectToShowWhenSelected;

    private bool isSelected;

    public void ToggleSelected()
    {
        SetIsSelected(!isSelected);
    }

    public bool GetIsSelected()
    {
        return isSelected;
    }

    public void SetIsSelected(bool selected)
    {
        if (!isActiveAndEnabled)
        {
            return;
        }

        isSelected = selected;
        if( ObjectToShowWhenSelected != null)
        {
            ObjectToShowWhenSelected.SetActive(isSelected);
        }
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
