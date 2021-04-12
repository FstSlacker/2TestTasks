using UnityEngine;
using UnityEngine.UI;

public class EventFiltering : MonoBehaviour
{

    private bool IsCursorInsideRegion(GameObject obj)
    {
        Vector3 _mousePositionLocal = obj.transform.InverseTransformPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        Rect rect = obj.GetComponent<RectTransform>().rect;

        if(_mousePositionLocal.x >= rect.xMin && _mousePositionLocal.x <= rect.xMax && _mousePositionLocal.y >= rect.yMin && _mousePositionLocal.y <= rect.yMax)
            return true;
        else
            return false;
    }
    private void Search(Transform parentTransform, ref GameObject target)
    {
        for(int i = 0; i < parentTransform.childCount; i++)
        {
            var g = parentTransform.GetChild(i);
            if (IsCursorInsideRegion(g.gameObject))
            {
                target = g.gameObject;
            }
            Search(g, ref target);
        }
    }
    void Update()
    {
        GameObject img = null;
        Search(transform, ref img);
        if (Input.GetKeyDown(KeyCode.Mouse0) && img != null)
        {
            img.GetComponent<Outline>().effectColor = Color.red;
        }
    }
}
