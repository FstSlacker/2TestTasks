using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EventFiltering : MonoBehaviour
{
    private Stack<GameObject> windowsList;
    private bool IsCursorInsideRegion(GameObject obj)
    {
        Vector3 _mousePositionLocal = obj.transform.InverseTransformPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        Rect rect = obj.GetComponent<RectTransform>().rect;

        if(_mousePositionLocal.x >= rect.xMin && _mousePositionLocal.x <= rect.xMax && _mousePositionLocal.y >= rect.yMin && _mousePositionLocal.y <= rect.yMax)
            return true;
        else
            return false;
    }
    private void InitWindow(Transform parentTransform)
    {
        for(int i = 0; i < parentTransform.childCount; i++)
        {
            var g = parentTransform.GetChild(i);
            windowsList.Push(g.gameObject);
            InitWindow(g);
        }
    }
    private void InitWindows()
    {
        windowsList = new Stack<GameObject>();
        InitWindow(transform);
    }
    private GameObject GetClickedObject()
    {
        foreach (var g in windowsList)
            if (IsCursorInsideRegion(g)) return g;

        return null;
    }
    private void Start()
    {
        InitWindows();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            var clickedWindow = GetClickedObject();
            if(clickedWindow != null) clickedWindow.GetComponent<Outline>().effectColor = Color.red;
        }
    }
}
