using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

[CustomEditor(typeof(WindowPacking))]
public class InspectorButton : Editor
{

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if (GUILayout.Button("На весь экран"))
            ((WindowPacking)target).MaximizeBoard();

        if (GUILayout.Button("Обновить"))
            ((WindowPacking)target).UpdateWindows();
    }
}
public class WindowPacking : MonoBehaviour
{

    public GameObject board;
    public float boardWidth;
    public float boardHeight;
    

    public float boardPaddingX;
    public float boardPaddingY;
    public int N;
    public float minSpacing;
    public Color windowColor;

    [SerializeField] private Vector2Int gridSize = new Vector2Int(5, 2);
    
    void Start()
    {
        UpdateWindows();
    }

    public void UpdateWindows()
    {
        ClearChilds();
        board.GetComponent<RectTransform>().sizeDelta = new Vector2(boardWidth, boardHeight);
        PackWindows();
    }
    public void MaximizeBoard()
    {
        Vector2 size = GetComponent<Canvas>().GetComponent<RectTransform>().sizeDelta;
        boardWidth = size.x;
        boardHeight = size.y;
        UpdateWindows();
    }

    private Vector2Int FindOptGrid()
    {
        int counter = 0;

        Vector2Int optGrid = new Vector2Int();
        float optSquare = float.MaxValue;
        for(int y = 1; y <= N; y++)
        {
            for(int x = N % y == 0 ? N / y : N / y + 1 ; x * y - N < x && x <= N; x++)
            {
                Vector2 _ws;

                _ws.x = (boardWidth - boardPaddingX * 2 - (x - 1) * minSpacing) / x;
                _ws.y = (boardHeight - boardPaddingY * 2 - (y - 1) * minSpacing) / y;

                float currentSquare = (y - 1) * (boardWidth - boardPaddingX * 2) * minSpacing + (x - 1) * (boardHeight - boardPaddingY * 2) * minSpacing - (x - 1) * (y - 1) * minSpacing * minSpacing + (x * y - N) * _ws.x * _ws.y;

                if (currentSquare < optSquare)
                {
                    optSquare = currentSquare;
                    optGrid = new Vector2Int(x, y);
                }
                counter++;
            }
            
        }

        Debug.Log("Короткий алгоритм: " + counter.ToString());
        return optGrid;


    }

    private Vector2Int FindOptGrid2()
    {
        int counter = 0;

        Vector2Int optGrid = new Vector2Int();
        float optSquare = float.MaxValue;
        for (int y = 1; y <= N; y++)
        {
            for (int x = 1; x <= N; x++)
            {
                counter++;
                if (x * y - N >= x || x * y < N) continue;
                Vector2 _ws;

                _ws.x = (boardWidth - boardPaddingX * 2 - (x - 1) * minSpacing) / x;
                _ws.y = (boardHeight - boardPaddingY * 2 - (y - 1) * minSpacing) / y;

                float currentSquare = (y - 1) * (boardWidth - boardPaddingX * 2) * minSpacing + (x - 1) * (boardHeight - boardPaddingY * 2) * minSpacing - (x - 1) * (y - 1) * minSpacing * minSpacing + (x * y - N) * _ws.x * _ws.y;

                if (currentSquare < optSquare)
                {
                    optSquare = currentSquare;
                    optGrid = new Vector2Int(x, y);
                }

            }
        }
        Debug.Log("Длинный алгоритм: " + counter.ToString());
        return optGrid;
    }
    private void PackWindows()
    {
        Canvas _canvas = gameObject.GetComponent<Canvas>();

        /*
        Vector2Int a1 = FindOptGrid(), a2 = FindOptGrid2();
        if (a1 != a2)
        {
            Debug.LogWarning("Второй алгоритм: (" + a1.x.ToString() + "; " + a1.y.ToString() + "), Первый алгоритм: (" + a2.x.ToString() + "; " + a2.y.ToString() + ")");
            //Debug.LogWarning(a2);
        }*/
            

        gridSize = FindOptGrid();

        Vector2 _windowSize;

        _windowSize.x = (boardWidth - boardPaddingX * 2 - (gridSize.x - 1) * minSpacing) / gridSize.x;
        _windowSize.y = (boardHeight - boardPaddingY * 2 - (gridSize.y - 1) * minSpacing) / gridSize.y;

        Vector2 _offset = -new Vector2(boardWidth / 2, boardHeight / 2);

        int _windowsCount = 0;
        
        for(int y = gridSize.y - 1; y >= 0; y--)
        {
            for(int x = 0; x < gridSize.x; x++)
            {
                GameObject _window = DefaultControls.CreateImage(new DefaultControls.Resources());
                _window.GetComponent<Image>().color = windowColor;
                _window.AddComponent<Outline>();
                
                _window.GetComponent<RectTransform>().position = new Vector2(boardPaddingX + x * (minSpacing + _windowSize.x) + _windowSize.x / 2, boardPaddingY + y * (minSpacing + _windowSize.y) + _windowSize.y / 2) + _offset;
                _window.GetComponent<RectTransform>().sizeDelta = _windowSize;

                _window.transform.SetParent(_canvas.transform, false);

                _windowsCount++;
                if (_windowsCount >= N) return;
            }
        }
    }
    private void ClearChilds()
    {
        for (int i = 1; i < transform.childCount; i++)
        {
            var g = transform.GetChild(i);
            Destroy(g.gameObject);
        }
    }
}
