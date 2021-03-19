using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tile : MonoBehaviour
{
    public Board board;

    public Sprite icon;
    public int row;
    public int col;
    public GameObject left;
    public GameObject right;
    public GameObject above;
    public GameObject below;

    private AudioSource matchSFX;
    
    private void Start()
    {
        matchSFX = GetComponent<AudioSource>();
        GetComponent<Image>().sprite = icon;
        gameObject.name = "(" + row.ToString() + ", " + col.ToString() + ")";
        board = transform.GetComponentInParent<Board>();
        gameObject.GetComponent<Button>().onClick.AddListener(Select);

        // Set Adjacent tiles (left, right, above, below)
        if (row > 0)
        {
            below = board.tiles[row - 1, col];
        }
        if (row < board.rows - 1)
        {
            above = board.tiles[row + 1, col];
        }

        if (col > 0)
        {
            left = board.tiles[row, col - 1];
        }
        if (col < board.cols - 1)
        {
            right = board.tiles[row, col + 1];
        }
    }

    private void Update()
    {
        GetComponent<Image>().sprite = icon;
    }

    public void Select()
    {
        if (board.Selected.Count < 2)
        {
            board.Selected.Add(gameObject);
        }
    }

    public void FindMatch3()
    {
        if(right != null && left != null)
        {
            if(left.GetComponent<Tile>().icon == icon && right.GetComponent<Tile>().icon == icon)
            {
                matchSFX.Play();
                left.GetComponent<Tile>().Drop();
                right.GetComponent<Tile>().Drop();
                Drop();
                board.score += 3;
            }
        }
        if(above != null && below != null)
        {
            if (above.GetComponent<Tile>().icon == icon && below.GetComponent<Tile>().icon == icon)
            {
                matchSFX.Play();
                above.GetComponent<Tile>().Drop();
                below.GetComponent<Tile>().Drop();
                Drop();
                board.score += 3;
            }
        }
    }

    public void Drop()
    {
        if(row < board.rows - 1)
        {
            icon = above.GetComponent<Tile>().icon;
        }
        else
        {
            int n = Random.Range(0, board.tileTypes.Length);
            icon = board.tileTypes[n];
        }
    }

    public void FindMatch5()
    {
        if (col >= 1 && col <= board.cols - 2)
        {
            GameObject left2 = left.GetComponent<Tile>().left;
            GameObject right2 = right.GetComponent<Tile>().right;
            if (right != null && left != null)
            {
                if (right2 != null && left2 != null)
                {
                    if (left.GetComponent<Tile>().icon == icon && right.GetComponent<Tile>().icon == icon)
                    {
                        if (left2.GetComponent<Tile>().icon == icon && right2.GetComponent<Tile>().icon == icon)
                        {
                            matchSFX.Play();
                            left.GetComponent<Tile>().Drop();
                            right.GetComponent<Tile>().Drop();
                            left2.GetComponent<Tile>().Drop();
                            right2.GetComponent<Tile>().Drop();
                            Drop();
                            board.score += 3;
                        }

                    }
                }
            }
        }

        if (row >= 1 && row <= board.rows - 2)
        {
            GameObject above2 = above.GetComponent<Tile>().above;
            GameObject below2 = below.GetComponent<Tile>().below;
            if (above != null && below != null)
            {
                if (above2 != null && below2 != null)
                {
                    if (above.GetComponent<Tile>().icon == icon && below.GetComponent<Tile>().icon == icon)
                    {
                        if (above2.GetComponent<Tile>().icon == icon && below2.GetComponent<Tile>().icon == icon)
                        {
                            matchSFX.Play();
                            above.GetComponent<Tile>().Drop();
                            below.GetComponent<Tile>().Drop();
                            above2.GetComponent<Tile>().Drop();
                            below2.GetComponent<Tile>().Drop();
                            Drop();
                            board.score += 3;
                        }
                    }
                }
            }
        }
    }
}
