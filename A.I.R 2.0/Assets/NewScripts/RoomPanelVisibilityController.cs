using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomPanelVisibilityController : MonoBehaviour
{
    List<GameObject> children;

    public void SetUpVisability()
    {
        children = new List<GameObject>();
        AddDescendants(this.gameObject.transform, children);
    }

    public void MakeInvisible()
    {
        GetComponent<Image>().enabled = false;
        foreach (GameObject child in children)
        {
            child.SetActive(false);
        }

    }

    public void MakeVisable()
    {
        GetComponent<Image>().enabled = true;
        foreach (GameObject child in children)
        {
            child.SetActive(true);
        }
    }

    private void AddDescendants(Transform parent, List<GameObject> list)
    {
        foreach (Transform child in parent)
        {
            list.Add(child.gameObject);
            AddDescendants(child, list);
        }
    }
}
