using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    [SerializeField] private Scrollbar widthBar,heightBar,speedBar;
    [SerializeField] private GameObject square;
    [SerializeField] private float targetScale;
    [SerializeField] private GameObject squareContainer, checkSquareContainer;
    private float fieldWidth, fieldHeight, offset;
    private float cellSize;
    // Start is called before the first frame update
    void Start()
    {
        widthBar.onValueChanged.AddListener((float val) => ScrollbarCallbackWidth(val));
        heightBar.onValueChanged.AddListener((float val) => ScrollbarCallbackHeight(val));
        fieldWidth = widthBar.GetComponent<Scrollbar>().value;
        fieldHeight = heightBar.GetComponent<Scrollbar>().value;
        ChangeFieldSize(fieldWidth,fieldHeight);
    }

    // Update is called once per frame
    void Update()
    {

    }
    
    private void ScrollbarCallbackWidth(float value)
    {
        fieldWidth = widthBar.GetComponent<Scrollbar>().value;
        ChangeFieldSize(fieldWidth,fieldHeight);
    }
    private void ScrollbarCallbackHeight(float value)
    {
        fieldHeight = heightBar.GetComponent<Scrollbar>().value;
        ChangeFieldSize(fieldWidth,fieldHeight);
    }
    private void SpawnCells()
    {
        ClearLevel();
        float widthFieldNumber = fieldWidth*4+2;
        float heightFieldNumber = fieldHeight*4+2;
        for (int i=0; i<widthFieldNumber; i++)
        {
            for (int j=0; j<heightFieldNumber; j++)
            {
                float xPos = (-transform.localScale.x + offset + cellSize)/2 + squareContainer.transform.position.x + (cellSize+offset)*i;
                float yPos = (transform.localScale.y - offset - cellSize)/2 + squareContainer.transform.position.y + (cellSize+offset)*-j;
                Vector3 cellPos = new Vector3(xPos, yPos, 0);
                GameObject cell = Instantiate(square, cellPos, Quaternion.identity);
                cell.transform.parent = squareContainer.transform;
            }
        }
    }
    private void ChangeFieldSize(float fieldWidth, float fieldHeight)
    {
        cellSize = square.transform.localScale.x;
        offset = cellSize/4;
        float scaleModifer = targetScale/(cellSize*6 + offset*6);
        float widthFieldNumber = fieldWidth*4+2;
        float heightFieldNumber = fieldHeight*4+2;
        transform.localScale = new Vector2(((cellSize + offset)*widthFieldNumber), ((cellSize + offset)*heightFieldNumber));
        SpawnCells();
    }
    private void ClearLevel()
    {
        for (int i = squareContainer.transform.childCount - 1; i>=0; i--)
        {
            GameObject child = squareContainer.transform.GetChild(i).gameObject;
            Destroy(child);
        }
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
        float widthFieldNumber = fieldWidth*4+2;
        float heightFieldNumber = fieldHeight*4+2;
        for (int i=0; i<widthFieldNumber; i++)
        {
            for (int j=0; j<heightFieldNumber; j++)
            {
                int numOfChild = i* (int) widthFieldNumber+j;
                Transform child = squareContainer.transform.GetChild(numOfChild);
                child.position = checkSquareContainer.transform.position + new Vector3((cellSize+offset)/2 + (cellSize+offset)*(numOfChild % 2), -(cellSize+offset)/2 - (cellSize+offset)*(numOfChild / 2),0);
                print(numOfChild % 2);
            }
        }
    }
}
