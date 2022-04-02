using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameController : MonoBehaviour
{
    [SerializeField] private Scrollbar widthBar, heightBar, speedBar;
    [SerializeField] private Button startButton, checkButton;
    [SerializeField] private GameObject cell;
    [SerializeField] private float targetScale;
    [SerializeField] private GameObject cellContainer, checkCellContainer;
    private float fieldWidth, fieldHeight, offset, cellSize;
    private int widthFieldNumber, heightFieldNumber;
    List<GameObject> cellsToSolve = new List<GameObject>();
    List<GameObject> cellsToDrag = new List<GameObject>();
    List<string> usedNumbersToSolveList = new List<string>();
    List<int> movedNumbersList = new List<int>();


    // Start is called before the first frame update
    void Start()
    {
        widthBar.onValueChanged.AddListener((float val) => ScrollbarCallbackWidth(val));
        heightBar.onValueChanged.AddListener((float val) => ScrollbarCallbackHeight(val));
        fieldWidth = widthBar.value;
        fieldHeight = heightBar.value;
        widthFieldNumber = (int)(fieldWidth * 4 + 2);
        heightFieldNumber = (int)(fieldHeight * 4 + 2);
        ChangeFieldSize();
    }

    private void ScrollbarCallbackWidth(float value)
    {
        fieldWidth = widthBar.value;
        int oldWidthFieldNumber = widthFieldNumber;
        widthFieldNumber = (int)(fieldWidth * 4 + 2);
        if (oldWidthFieldNumber != widthFieldNumber) ChangeFieldSize();
    }
    private void ScrollbarCallbackHeight(float value)
    {
        fieldHeight = heightBar.value;
        int oldHeightFieldNumber = heightFieldNumber;
        heightFieldNumber = (int)(fieldHeight * 4 + 2);
        if (oldHeightFieldNumber != heightFieldNumber) ChangeFieldSize();

    }
    private void SpawnCells()
    {
        ClearLevel();
        for (int i = 0; i < widthFieldNumber; i++)
        {
            for (int j = 0; j < heightFieldNumber; j++)
            {
                float xPos = (-cellContainer.transform.lossyScale.x + offset + cellSize) / 2 + cellContainer.transform.position.x + (cellSize + offset) * i;
                float yPos = (cellContainer.transform.lossyScale.y - offset - cellSize) / 2 + cellContainer.transform.position.y + (cellSize + offset) * -j;
                GameObject cellToSpawn = Instantiate(cell, new Vector3(xPos, yPos, 0), Quaternion.identity);
                cellsToSolve.Add(cellToSpawn);
            }
        }
    }
    private void ChangeFieldSize()
    {
        cellSize = cell.transform.localScale.x;
        offset = cellSize / 4;
        float scaleModifer = targetScale / (cellSize * 6 + offset * 6);
        cellContainer.transform.localScale = new Vector2(((cellSize + offset) * widthFieldNumber + offset) * scaleModifer, ((cellSize + offset) * heightFieldNumber + offset) * scaleModifer);
        SpawnCells();
    }
    private void ClearLevel()
    {
        foreach (var cell in cellsToSolve) Destroy(cell);
        cellsToSolve.Clear();
        foreach (var cell in cellsToDrag) Destroy(cell);
        cellsToDrag.Clear();
        usedNumbersToSolveList.Clear();
        movedNumbersList.Clear();
    }
    public void StartLevel()
    {
        SpawnCells();
        speedBar.interactable = widthBar.interactable = heightBar.interactable = startButton.interactable = false;
        float speed = speedBar.value * 9 + 1;
        IEnumerator coroutine = WaitForSec(speed);
        StartCoroutine(coroutine);
    }
    private IEnumerator WaitForSec(float waitTime)
    {
        SetNumbers();
        yield return new WaitForSeconds(waitTime);
        HideCells();
        startButton.interactable = checkButton.interactable = true;
    }
    private void HideCells()
    {
        List<GameObject> anotherCellsToDrag = new List<GameObject>();
        foreach (var cellToSolve in cellsToSolve)
        {
            anotherCellsToDrag.Add(Instantiate(cellToSolve, cellToSolve.transform.position, Quaternion.identity));
            cellToSolve.transform.SetParent(cellContainer.transform);
            cellToSolve.GetComponentInChildren<TMP_Text>().alpha = 0.05f;
            cellToSolve.tag = "DropBox";
        }
        for (int i = anotherCellsToDrag.Count - 1; i >= 0; i--)
        {
            int temp = Random.Range(0, anotherCellsToDrag.Count - 1);
            GameObject anotherCell = anotherCellsToDrag[temp];
            anotherCell.transform.position = checkCellContainer.transform.position + new Vector3((cellSize + offset) / 2 + (cellSize + offset) * (i % 3), -(cellSize + offset) / 2 - (cellSize + offset) * (i / 3), 0);
            anotherCell.tag = "Draggable";
            cellsToDrag.Add(anotherCell);
            anotherCellsToDrag.Remove(anotherCell);
        }
        cellsToDrag.Reverse();
    }
    private void SetNumbers()
    {
        foreach (var cell in cellsToSolve)
        {
            int temp = Random.Range(0, 99);
            while (CheckEquals(ref temp)) { }
            usedNumbersToSolveList.Add(temp.ToString());
            cell.GetComponentInChildren<TMP_Text>().text = temp.ToString();
        }
    }
    private bool CheckEquals(ref int temp)
    {
        foreach (var num in usedNumbersToSolveList)
        {
            if (temp.ToString() == num)
            {
                temp = Random.Range(0, 99);
                return true;
            }
        }
        return false;
    }
    public void CheckLevel()
    {
        speedBar.interactable = widthBar.interactable = heightBar.interactable = true;
        foreach (var cellToSolve in cellsToSolve)
        {
            foreach (var cellToDrag in cellsToDrag)
            {
                if (cellToDrag.transform.position == cellToSolve.transform.position)
                {
                    if (cellToDrag.GetComponentInChildren<TMP_Text>().text == cellToSolve.GetComponentInChildren<TMP_Text>().text)
                    {
                        cellToDrag.GetComponent<SpriteRenderer>().color = Color.green;
                    }
                    else
                    {
                        cellToDrag.GetComponent<SpriteRenderer>().color = Color.red;
                    }
                }
            }
        }
    }
    public void SortElements(GameObject elem)
    {
        int movedObjCount = 0;
        bool found = false;
        for (int i = 0; i < cellsToDrag.Count; i++)
        {
            if(movedNumbersList.Exists((x)=>x==i))
            {
                movedObjCount++;
            }
            else if(found)
            {
                cellsToDrag[i].transform.position = checkCellContainer.transform.position + new Vector3((cellSize + offset) / 2 + (cellSize + offset) * ((i-movedObjCount) % 3), -(cellSize + offset) / 2 - (cellSize + offset) * ((i-movedObjCount) / 3), 0);
            }
            if(cellsToDrag[i] == elem)
            {
                found = true;
                movedNumbersList.Add(i);
                movedObjCount++;
            }
        }
    }
}

