using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragAndDrop : MonoBehaviour
{
    private GameObject target = null;
    private bool isMouseDrag;
    Vector3 screenPosition, offset;
/*
    GameObject ReturnClickedObject()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray.origin, ray.direction * 10, out hit))
        {
            target = hit.collider.gameObject;
        }
        return target;
    }
    void Update()
    {
 
        if (Input.GetMouseButtonDown(0))
        {
            target = ReturnClickedObject();
            if (target != null)
            {
                isMouseDrag = true;
                Debug.Log("target position :" + target.transform.position);
                //Convert world position to screen position.
                screenPosition = Camera.main.WorldToScreenPoint(target.transform.position);
                offset = target.transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
            }
        }
 
        if (Input.GetMouseButtonUp(0))
        {
            isMouseDrag = false;
        }
 
        if (isMouseDrag)
        {
            //track mouse position.
            Vector3 currentScreenSpace = Input.mousePosition;
 
            //convert screen position to world position with offset changes.
            Vector3 currentPosition = Camera.main.ScreenToWorldPoint(currentScreenSpace) + offset;
 
            //It will update target gameobject's current postion.
            target.transform.position = currentPosition;
        }
 
    }
*/
GameObject ReturnClickedObject(out RaycastHit hit)
    {
        GameObject target = null;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray.origin, ray.direction * 10, out hit))
        {
            target = hit.collider.gameObject;
        }
        return target;
    }
    void Update()
    {
 
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hitInfo;
            target = ReturnClickedObject(out hitInfo);
            if (target != null)
            {
                isMouseDrag = true;
                //Convert world position to screen position.
                screenPosition = Camera.main.WorldToScreenPoint(target.transform.position);
                offset = target.transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
            }
        }
 
        if (Input.GetMouseButtonUp(0))
        {
            isMouseDrag = false;
        }
 
        if (isMouseDrag)
        {
            //track mouse position.
            Vector3 currentScreenSpace = Input.mousePosition;
 
            //convert screen position to world position with offset changes.
            Vector3 currentPosition = Camera.main.ScreenToWorldPoint(currentScreenSpace) + offset;
 
            //It will update target gameobject's current postion.
            target.transform.position = currentPosition;
        }
 
    }
}