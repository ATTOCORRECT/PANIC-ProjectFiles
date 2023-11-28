using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class NewBehaviourScript : MonoBehaviour
{
    public GameObject selection;

    Vector3 selctionTargetPosition = Vector3.zero;

    public GameObject Player;
    public GameObject Enemy;

    int EnemyPosIndex = 35;
    int PlayerPosIndex = 0;    

    Vector3[] piecePositionLUT = new Vector3[36];

    List<GameObject> Piece = new List<GameObject>(36);

    public Camera Camera;

    Vector3 activeGridSelected;
    int selectionMode = 0; // 0 choose, 1 move, 2 wait
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

        Piece[PlayerPosIndex] = Player;
        Piece[EnemyPosIndex] = Enemy;
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

        selection.transform.position = Vector2.Lerp(selection.transform.position, selctionTargetPosition, 0.2f);
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 cursorPos = Camera.ScreenToWorldPoint(Input.mousePosition);

        Vector3 gridSelected = piecePositionLUT.OrderBy((d) => ((Vector2)d - cursorPos).sqrMagnitude).ToArray()[0];

        selctionTargetPosition = gridSelected;

        if (Input.GetMouseButtonDown(0))
        {
            if (selectionMode == 0) // move
            {
                if(Piece[(int)gridSelected.z] == null)
                {
                    SwapPieces(PlayerPosIndex, (int)gridSelected.z);
                    PlayerPosIndex = (int)gridSelected.z;

                    Debug.Log("MOVED");

                    selectionMode = 2;
                }
                else
                {
                    Debug.Log("CANT MOVE");
                }
            }
        }

        if (selectionMode == 2)
        {
            Invoke("EnemyTurn", 0.5f);
            selectionMode = 3;
        }

    }
    void EnemyTurn()
    {
        
        int randomPosition;
        bool trigger = true;

        while (trigger)
        {
            randomPosition = Random.Range(0, 36);

            if (Piece[randomPosition] != Player || Piece[randomPosition] != Enemy)
            {
                SwapPieces(randomPosition, EnemyPosIndex);
                EnemyPosIndex = randomPosition;

                trigger = false;
            }
        }
        selectionMode = 0;
    }

    void SwapPieces(int indexA, int indexB)
    {
        GameObject tmp = Piece[indexA];
        Piece[indexA] = Piece[indexB];
        Piece[indexB] = tmp;
        //GameObject.Destroy(tmp);
    }
}
