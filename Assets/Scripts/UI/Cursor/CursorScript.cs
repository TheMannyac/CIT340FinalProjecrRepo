using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorScript : MonoBehaviour
{
    public Texture2D cursorTexture;
    public CursorMode cursorMode = CursorMode.Auto;
    public Vector2 hotspot = Vector2.zero;

    // Start is called before the first frame update
    void Start()
    {
        //Cursor.visible = false;
        //Cursor.SetCursor(cursorTexture,hotspot,cursorMode);
    }

    // Update is called once per frame
    void Update()
    {
        // 0 is left Click; 1 is right click, 2 is middle mouse button
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

            RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);

            if (hit.collider != null)
            {
                //Debug.Log(hit.collider.gameObject.name);

                switch (hit.collider.gameObject.tag)
                {
                    case "Interactable":
                        hit.collider.gameObject.GetComponent<Interactable>().OnClick();
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
