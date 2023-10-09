using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class NewBehaviourScript : MonoBehaviour
{
    public GameObject BallRed;
    public GameObject BallCya;
        
    Vector3[] piecePositionLUT = new Vector3[36];

    List<GameObject> Piece = new List<GameObject>(36);

    public Camera Camera;

    Vector3 activeGridSelected;
    int selectionMode = 0; // 0 choose, 1 move
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < (36); i++)
        {
            piecePositionLUT[i] = (Vector2)GameObject.Find(i.ToString()).transform.position;
            piecePositionLUT[i].z = i;
        }

        for (int i = 0; i < (36); i++) 
        {
            Piece.Add(null);
        }

        Piece[4] = BallRed;
        Piece[35] = BallCya;
    }
    void FixedUpdate()
    {
        for (int i = 0; i < 36; i++)
        {
            if (Piece[i] != null)
            {
                Piece[i].transform.position = Vector2.Lerp(Piece[i].transform.position, piecePositionLUT[i], 0.2f) + Vector2.up/5;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 cursorPos = Camera.ScreenToWorldPoint(Input.mousePosition);

        Vector3 gridSelected = piecePositionLUT.OrderBy((d) => ((Vector2)d - cursorPos).sqrMagnitude).ToArray()[0];

        

        if (Input.GetMouseButtonDown(0))
        {
            if (selectionMode == 0) // select
            {
                if (Piece[(int)gridSelected.z] != null)
                {
                    activeGridSelected = gridSelected;

                    Debug.Log("SELECTED");

                    selectionMode = 1;
                }
                else
                {
                    Debug.Log("CANT SELECT");
                }
                
                
                
            }
            else if (selectionMode == 1) // move
            {
                if (activeGridSelected != gridSelected)
                {
                    if(Piece[(int)gridSelected.z] == null)
                    {
                        SwapPieces((int)activeGridSelected.z, (int)gridSelected.z);

                        Debug.Log("MOVED");

                        selectionMode = 0;
                    }
                    else
                    {
                        Debug.Log("CANT MOVE");
                    }
                }
                else
                {
                    Debug.Log("CANCELLED");

                    selectionMode = 0;
                }
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            if (selectionMode == 1) // deselect
            {
                activeGridSelected = Vector3.zero;

                Debug.Log("DESELECTED");

                selectionMode = 0;
            }
        }

    }
    void SwapPieces(int indexA, int indexB)
    {
        GameObject tmp = Piece[indexA];
        Piece[indexA] = Piece[indexB];
        Piece[indexB] = tmp;
        //GameObject.Destroy(tmp);
    }
}
