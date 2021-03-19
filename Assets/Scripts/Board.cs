using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Board : MonoBehaviour
{
    // Needed for Setup
    public float easyLimit = 300;
    public float normalLimit = 180;
    public float hardLimit = 90;
    public float timeLimit;
    public enum Difficulty
    {
        Easy,
        Normal,
        Hard
    }
    public Difficulty difficulty;
    public int scoreNeeded = 300;
    public int rows = 10;
    public int cols = 12;
    public Vector2 padding;
    public float TileSize = 40;
    public GameObject TilePrefab;
    public Sprite[] tileTypes;

    // Needed For Tile.cs
    public List<GameObject> Selected;
    public GameObject[,] tiles;

    public int score;

    private float second = 1;
    private Text scoreTxt;
    private Text timeTxt;

    void Start()
    {
        difficulty = Difficulty.Easy;
        scoreTxt = GameObject.Find("ScoreText").GetComponent<Text>();
        timeTxt = GameObject.Find("TimeText").GetComponent<Text>();
    }

    void Update()
    {
        scoreTxt.text =  "You Have " + score.ToString() + "/" + scoreNeeded.ToString() + " Matches";

        if(second <= 0)
        {
            timeLimit--;
            timeTxt.text = timeLimit.ToString();
            second = 1;
        }
        else
        {
            second -= Time.deltaTime;
        }

        if(timeLimit <= 0)
        {
            EndGame();
        }

        if(Selected.Count == 2)
        {
            CheckPair();
            if (score >= scoreNeeded)
            {
                AmpDifficulty();
                EndGame();
            }
        }
    }
    
    void Swap()
    {
        Sprite copySpr = Selected[0].GetComponent<Image>().sprite;
        Selected[0].GetComponent<Tile>().icon = Selected[1].GetComponent<Image>().sprite;
        Selected[1].GetComponent<Tile>().icon = copySpr;
        
        if (difficulty == Difficulty.Easy)
        {
            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < cols; c++)
                {
                    tiles[r, c].GetComponent<Tile>().FindMatch3();
                }
            }
        }
        else
        {
            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < cols; c++)
                {
                    tiles[r, c].GetComponent<Tile>().FindMatch5();
                }
            }
        }
        Selected.Clear();
    }

    void CheckPair()
    {
        if (Selected[0].GetComponent<Tile>().above != null && Selected[1].name == Selected[0].GetComponent<Tile>().above.name)
        {
            Swap();
            return;
        }
        if (Selected[0].GetComponent<Tile>().below != null && Selected[1].name == Selected[0].GetComponent<Tile>().below.name)
        {
            Swap();
            return;
        }
        if (Selected[0].GetComponent<Tile>().right != null && Selected[1].name == Selected[0].GetComponent<Tile>().right.gameObject.name)
        {
            Swap();
            return;
        }
        if (Selected[0].GetComponent<Tile>().left != null && Selected[1].name == Selected[0].GetComponent<Tile>().left.gameObject.name)
        {
            Swap();
            return;
        }

        Selected.Clear();
    }
    
    public void Setup()
    {
        score = 0;
        if(difficulty == Difficulty.Easy)
        {
            timeLimit = easyLimit;
        }
        else if(difficulty == Difficulty.Normal)
        {
            timeLimit = normalLimit;
        }
        else if(difficulty == Difficulty.Hard)
        {
            timeLimit = hardLimit;
        }
        Selected = new List<GameObject>();
        tiles = new GameObject[rows, cols];
        for (int r = 0; r < rows; r++)
        {
            for(int c = 0; c < cols; c++)
            {
                GameObject t = Instantiate(TilePrefab,transform);
                t.GetComponent<RectTransform>().sizeDelta = new Vector2(TileSize, TileSize);
                t.GetComponent<RectTransform>().position = new Vector3(TileSize * c + padding.x, TileSize * r + padding.y);
                int i = Random.Range(0, tileTypes.Length);
                int maxIterations = 0;
                while(MatchesAt(r,c, tileTypes[i]) && maxIterations < 100)
                {
                    i = Random.Range(0, tileTypes.Length);
                    maxIterations++;
                }
                maxIterations = 0;
                t.GetComponent<Tile>().icon = tileTypes[i];
                t.GetComponent<Tile>().row = r;
                t.GetComponent<Tile>().col = c;
                tiles[r, c] = t;

            }
        }
    }

    private bool MatchesAt(int row, int col ,Sprite piece)
    {
        if(col > 1 && row > 1)
        {
            if(tiles[row, col - 1].GetComponent<Tile>().icon == piece && tiles[row, col - 2].GetComponent<Tile>().icon == piece)
            {
                return true;
            }
            if (tiles[row - 1, col].GetComponent<Tile>().icon == piece && tiles[row - 2, col].GetComponent<Tile>().icon == piece)
            {
                return true;
            }
        }
        else if(col <= 1 || row <= 1)
        {
            if(row > 1)
            {
                if(tiles[row - 1 ,col].GetComponent<Tile>().icon == piece && tiles[row - 2, col].GetComponent<Tile>().icon == piece)
                {
                    return true;
                }
            }
            if (col > 1)
            {
                if (tiles[row, col - 1].GetComponent<Tile>().icon == piece && tiles[row, col - 2].GetComponent<Tile>().icon == piece)
                {
                    return true;
                }
            }
        }
        return false;
    }

    private void AmpDifficulty()
    {
        if(difficulty == Difficulty.Easy)
        {
            difficulty = Difficulty.Normal;
        }
        else
        {
            difficulty = Difficulty.Hard;
        }
    }

    private void EndGame()
    {
        foreach(Transform child in transform)
        {
            Destroy(child.gameObject);
        }
        transform.parent.gameObject.SetActive(false);
    }

}
