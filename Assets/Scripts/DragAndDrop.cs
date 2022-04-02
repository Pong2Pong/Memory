using UnityEngine;
using TMPro;

public class DragAndDrop : MonoBehaviour
{
    [SerializeField] GameObject objWithGameController;
    private GameObject targetToDrag, targetToDrop = null;
    private bool isMouseDrag;
    Vector3 screenPosition, offset;
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
            RaycastHit hitInfoDrag;
            targetToDrag = ReturnClickedObject(out hitInfoDrag);
            if ((targetToDrag != null) && (targetToDrag.tag == "Draggable"))
            {
                isMouseDrag = true;
                //Convert world position to screen position.
                screenPosition = Camera.main.WorldToScreenPoint(targetToDrag.transform.position);
                offset = targetToDrag.transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
                targetToDrag.layer = 2;
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            RaycastHit hitInfoDrop;
            targetToDrop = ReturnClickedObject(out hitInfoDrop);
            if (targetToDrop != null)
            {
                if ((targetToDrop.tag == "DropBox") && (isMouseDrag))
                {
                    objWithGameController.GetComponent<GameController>().SortElements(targetToDrag);
                    targetToDrag.transform.position = targetToDrop.transform.position;
                }
            }
            if (targetToDrag != null) targetToDrag.layer = 0;
            isMouseDrag = false;
            targetToDrag = null;
            targetToDrop = null;
        }

        if (isMouseDrag)
        {
            //track mouse position.
            Vector3 currentScreenSpace = Input.mousePosition;

            //convert screen position to world position with offset changes.
            Vector3 currentPosition = Camera.main.ScreenToWorldPoint(currentScreenSpace) + offset;

            //It will update target gameobject's current postion.
            targetToDrag.transform.position = currentPosition;
        }
    }
    private void CompareText()
    {
        if (targetToDrag.GetComponentInChildren<TMP_Text>().text == targetToDrop.GetComponentInChildren<TMP_Text>().text)
        {
            targetToDrop.GetComponent<SpriteRenderer>().color = Color.green;
        }
        else
        {
            targetToDrop.GetComponent<SpriteRenderer>().color = Color.red;
        }
    }
}