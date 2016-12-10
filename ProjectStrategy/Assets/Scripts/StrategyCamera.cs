using UnityEngine;
using System.Collections;

public class StrategyCamera : MonoBehaviour {

    private Game Game;

    private int scrollDistance = 2;
    private float scrollSpeed = 10;

    // Use this for initialization
    void Start()
    {
        Game = GameObject.Find("Game").GetComponent<Game>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Game.Selector.CurrentUnit != null && (Game.Selector.CurrentUnit.IsMoving() || Game.Selector.CurrentUnit.IsWaitingForMoveAccept()))
            return;

        // Left / Right
        //if (transform.position.x < 15 && transform.position.x > -12 && transform.position.z < 35 && transform.position.z > -4)
        //{
            if (Input.mousePosition.x < scrollDistance && transform.position.x > -11)
                transform.Translate(Vector3.right * -scrollSpeed * Time.deltaTime);
            else if (Input.mousePosition.x >= Screen.width - scrollDistance && transform.position.x < 15)
                transform.Translate(Vector3.right * scrollSpeed * Time.deltaTime);

        // Forward / Backward
            if (Input.mousePosition.y < scrollDistance && transform.position.z < 32)
                transform.Translate((Vector3.forward - transform.forward) * -scrollSpeed * Time.deltaTime);
            else if (Input.mousePosition.y >= Screen.height - scrollDistance && transform.position.z > -4)
                transform.Translate((Vector3.forward - transform.forward) * scrollSpeed * Time.deltaTime);
        //}
        // Zooming
        if (Input.GetAxis("Mouse ScrollWheel") < 0)
            transform.Translate((Vector3.forward + transform.forward) * -scrollSpeed * Time.deltaTime);
        else if (Input.GetAxis("Mouse ScrollWheel") > 0)
            transform.Translate((Vector3.forward + transform.forward) * scrollSpeed * Time.deltaTime);
    }
}
