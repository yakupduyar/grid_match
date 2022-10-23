using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GridController : MonoBehaviour
{
    #region Singleton

    private static GridController _instance;

    public static GridController Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<GridController>();
            }

            return _instance;
        }
    }

    #endregion
    
    [SerializeField] private GridPart _gridPart;
    [SerializeField] private int n=10;
    [SerializeField] private Sprite _cross,_square;

    private List<GridPart> crossParts = new List<GridPart>();
    public Sprite Cross => _cross;
    public Sprite Square => _square;
    public GameObject gridParent;
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void CreateGrid(TMP_InputField input)
    {
        if(gridParent) Destroy(gridParent.gameObject);
        n = int.Parse(input.text);
        StartCoroutine(GridCreation());
    }

    IEnumerator GridCreation()
    {
        gridParent = new GameObject("Grid");
        Camera.main.orthographicSize = n;
        gridParent.transform.position = new Vector2((n-1)*.5f, (n-1)*.5f);
        for (int i = n; i > 0; i--)
        {
            GameObject rowParent = new GameObject("Row " + (n-i));
            for (int j = 0; j < n; j++)
            {
                GridPart newPart = Instantiate(_gridPart, Vector3.right * (j-(n-1)*.5f) + Vector3.up * (i-(n-1)*.5f), Quaternion.identity);
                newPart.name = j.ToString() + (n-i);
                newPart.cord = new Vector2Int(j, (n-i));
                newPart.transform.SetParent(rowParent.transform);
                yield return new WaitForSeconds(.1f/n);
            }
            rowParent.transform.SetParent(gridParent.transform);
        }
        //gridParent.transform.position = Vector3.zero;
        
    }

    public void SelectPart(GridPart p)
    {
        crossParts.Add(p);
        Vector2Int cord = new Vector2Int(p.transform.GetSiblingIndex(), p.transform.parent.GetSiblingIndex());
        print(p.name);
        CheckAll(p);

        if (matchedParts.Count>2)
        {
            foreach (var gp in matchedParts)
            {
                gp.ClearCross();
                crossParts.Remove(gp);
            }
        }
        matchedParts.Clear();
    }

    void CheckAll(GridPart p)
    {
        CheckRight(p);
        CheckLeft(p);
        CheckUp(p);
        CheckDown(p);
    }
    
    public void CheckRight(GridPart p)
    {
        for (int i = p.cord.x+1; i < n; i++)
        {
            Transform pr = gridParent.transform.GetChild(p.cord.y).GetChild(i);
            GridPart prp = pr.GetComponent<GridPart>();
            print(prp.name + ":" + prp.isChecked);
            if (prp.isChecked)
            {
                AddToMatch(prp);
            }
            else
            {
                break;
            }
        }
    }
    public void CheckLeft(GridPart p)
    {
        for (int i = p.cord.x-1; i >= 0; i--)
        {
            Transform pr = gridParent.transform.GetChild(p.cord.y).GetChild(i);
            GridPart prp = pr.GetComponent<GridPart>();
            print(prp.name + ":" + prp.isChecked);
            if (prp.isChecked)
            {
                AddToMatch(prp);
            }
            else
            {
                break;
            }
        }
    }
    public void CheckUp(GridPart p)
    {
        for (int i = p.cord.y+1; i < n; i++)
        {
            Transform pr = gridParent.transform.GetChild(i).GetChild(p.cord.x);
            GridPart prp = pr.GetComponent<GridPart>();
            print(prp.name + ":" + prp.isChecked);
            if (prp.isChecked)
            {
                AddToMatch(prp);
            }
            else
            {
                break;
            }
        }
    }
    public void CheckDown(GridPart p)
    {
        for (int i = p.cord.y; i >= 0; i--)
        {
            Transform pr = gridParent.transform.GetChild(i).GetChild(p.cord.x);
            GridPart prp = pr.GetComponent<GridPart>();
            print(prp.name + ":" + prp.isChecked);
            if (prp.isChecked)
            {
                AddToMatch(prp);
            }
            else
            {
                break;
            }
        }
    }

    private List<GridPart> matchedParts = new List<GridPart>();
    public void AddToMatch(GridPart gp)
    {
        if (!matchedParts.Contains(gp))
        {
            matchedParts.Add(gp);
        }
    }
    
}