using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellsController : MonoBehaviour
{
    public bool smartDrag = true;
    public bool isDraggable = true;
    public bool isDragged = false;
    Vector2 initialPositionMouse;
    Vector2 initialPositionObject;
    

    void Update()
    {
        if(isDragged)
        {
            if(!smartDrag)
            {
                transform.position = (Vector2) Camera.main.ScreenToWorldPoint(Input.mousePosition);
            }
            else
            {
                
        print("change");
                transform.position = initialPositionObject + (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) - initialPositionMouse;
            }
        }
    }
    private void OnMouseOver()
    {print("set");
        if(isDraggable && Input.GetMouseButton(0))
        {
            if(smartDrag)
            {
                
        print("set");
                initialPositionMouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                initialPositionObject = transform.position;
            }
            
        print("true");
            isDragged = true;
        }
    }
    private void OnMouseUp()
    {
        print("Up");
        isDragged = false;
    }
}
