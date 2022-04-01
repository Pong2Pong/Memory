using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameController : MonoBehaviour
{
    [SerializeField] private Scrollbar widthBar, heightBar, speedBar;
    [SerializeField] private GameObject square;
    [SerializeField] private float targetScale;
    [SerializeField] private GameObject squareContainer, checkSquareContainer;
    private float fieldWidth, fieldHeight, offset;
    private float cellSize;
    private int widthFieldNumber, heightFieldNumber;
    List<GameObject> cellsToSolve = new List<GameObject>();
    List<GameObject> cellsToDrag = new List<GameObject>();
    List<string> numbersList = new List<string>();

    // Start is called before the first frame update
    void Start()
    {
        widthBar.onValueChanged.AddListener((float val) => ScrollbarCallbackWidth(val));
        heightBar.onValueChanged.AddListener((float val) => ScrollbarCallbackHeight(val));
        fieldWidth = widthBar.GetComponent<Scrollbar>().value;
        fieldHeight = heightBar.GetComponent<Scrollbar>().value;
        widthFieldNumber = (int)(fieldWidth * 4 + 2);
        heightFieldNumber = (int)(fieldHeight * 4 + 2);

        ChangeFieldSize();
    }

    private void ScrollbarCallbackWidth(float value)
    {
        fieldWidth = widthBar.GetComponent<Scrollbar>().value;
        int oldWidthFieldNumber = widthFieldNumber;
        widthFieldNumber = (int)(fieldWidth * 4 + 2);
        if (oldWidthFieldNumber != widthFieldNumber)
        {
            ChangeFieldSize();
        }
    }
    private void ScrollbarCallbackHeight(float value)
    {
        fieldHeight = heightBar.GetComponent<Scrollbar>().value;
        int oldHeightFieldNumber = heightFieldNumber;
        heightFieldNumber = (int)(fieldHeight * 4 + 2);
        if (oldHeightFieldNumber != heightFieldNumber)
        {
            ChangeFieldSize();
        }

    }
    private void SpawnCells()
    {
        ClearLevel();
        for (int i = 0; i < widthFieldNumber; i++)
        {
            for (int j = 0; j < heightFieldNumber; j++)
            {
                float xPos = (-transform.lossyScale.x + offset + cellSize) / 2 + squareContainer.transform.position.x + (cellSize + offset) * i;
                float yPos = (transform.lossyScale.y - offset - cellSize) / 2 + squareContainer.transform.position.y + (cellSize + offset) * -j;
                Vector3 cellPos = new Vector3(xPos, yPos, 0);
                GameObject cell = Instantiate(square, cellPos, Quaternion.identity);
                cellsToSolve.Add(cell);
                cell.transform.SetParent(squareContainer.transform);

            }
        }
        setNumbers();
    }
    private void ChangeFieldSize()
    {
        cellSize = square.transform.localScale.x;
        offset = cellSize / 4;
        float scaleModifer = targetScale / (cellSize * 6 + offset * 6);
        transform.localScale = new Vector2(((cellSize + offset) * widthFieldNumber - offset) * scaleModifer, ((cellSize + offset) * heightFieldNumber - offset) * scaleModifer);
        SpawnCells();
    }
    private void ClearLevel()
    {
        foreach (var cell in cellsToSolve)
        {
            Destroy(cell);
        }
        cellsToSolve.Clear();
    }
    public void StartLevel()
    {
        float speed = speedBar.GetComponent<Scrollbar>().value * 9 + 1;
        IEnumerator coroutine = WaitForSec(speed);
        StartCoroutine(coroutine);
    }
    private IEnumerator WaitForSec(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        HideCells();
    }
    private void HideCells()
    {
        foreach (var cellToSolve in cellsToSolve)
        {
            cellsToDrag.Add(Instantiate(square, cellToSolve.transform.position, Quaternion.identity));
            cellToSolve.transform.SetParent(squareContainer.transform);
            cellToSolve.GetComponentInChildren<TMP_Text>().alpha = 0;
        }

        print(cellsToSolve.Count);

        print(cellsToDrag.Count);
        int i = 0;
        foreach (var cellToDrag in cellsToDrag)
        {
            cellToDrag.transform.position = checkSquareContainer.transform.position + new Vector3((cellSize + offset) / 2 + (cellSize + offset) * (i % 2), -(cellSize + offset) / 2 - (cellSize + offset) * (i / 2), 0);
            cellToDrag.GetComponentInChildren<TMP_Text>().text = cellsToSolve[i].GetComponentInChildren<TMP_Text>().text;
            cellToDrag.transform.SetParent(checkSquareContainer.transform);
            i++;
        }
    }
    private void setNumbers()
    {
        string[] numbers = new string[(int)(widthFieldNumber * heightFieldNumber)];
        foreach (var cell in cellsToSolve)
        {
            int temp = Random.Range(0, 99);
            while (checkEquals(ref temp)) { }
            numbersList.Add(temp.ToString());
            cell.GetComponentInChildren<TMP_Text>().text = temp.ToString();
        }
    }
    private bool checkEquals(ref int temp)
    {
        foreach (var num in numbersList)
        {
            if (temp.ToString() == num)
            {
                temp = Random.Range(0, 99);
                return true;
            }
        }
        return false;
    }
}

