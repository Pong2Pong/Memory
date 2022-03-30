using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoSizer : MonoBehaviour
{
    [SerializeField] private Camera MainCamera;
    private Vector2 bottomLeft ,topRight;

    // Start is called before the first frame update
    void Start()
    {
        bottomLeft = MainCamera.ScreenToWorldPoint(Vector2.zero);
        topRight = MainCamera.ScreenToWorldPoint(new Vector2 (Screen.width, Screen.height));
        var height = topRight.y - bottomLeft.y;
        var width = topRight.x - bottomLeft.x;
        gameObject.GetComponent<SpriteRenderer>().size = new Vector2(width, height);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
