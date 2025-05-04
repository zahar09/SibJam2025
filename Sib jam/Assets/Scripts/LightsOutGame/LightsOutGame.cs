using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[Serializable]
public class SerializableRow
{
    public int[] values;
}


public class LightsOutGame : MonoBehaviour
{
    [SerializeField] private SerializableRow[] initialState = new SerializableRow[0];
    [SerializeField] private Cell cellPrefab;
    [SerializeField] private float spacing = 1f;
    [SerializeField] private Transform gridParent;

    [Header("��� (UI)")]
    [SerializeField] private GameObject backgroundSprite; // ������ ����
    [SerializeField] private Color backgroundColor = Color.white; // ���� ����
    [SerializeField] private float backgroundPadding = 0.5f; // ������ �� ����

    [Header("�������")]
    public UnityEvent onGameWin = new UnityEvent();

    private Cell[,] cells;
    private int rows;
    private int cols;

    private void Start()
    {
        InitializeGame();
    }

    private void InitializeGame()
    {
        rows = initialState.Length;
        if (rows == 0) return;

        cols = initialState[0].values.Length;

        // ��������� ������������ �������
        for (int i = 1; i < rows; i++)
        {
            if (initialState[i].values.Length != cols)
            {
                Debug.LogError($"������ {i} ����� ������ �����! ��������� {cols} ���������.");
                return;
            }
        }

        // ������ ��� ����� ��������
        //CreateUIBackground();

        cells = new Cell[rows, cols];

        // ��������� ����������� �����
        Vector2 center = CalculateGridCenter();
        backgroundSprite.SetActive(true);
        backgroundSprite.transform.position = center;

        // ������ ������
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < cols; col++)
            {
                
                // ��������� ������� � ����� ��������� Z=0
                float x = col * spacing - (cols - 1) * spacing / 2f;
                float y = -row * spacing + (rows - 1) * spacing / 2f;
                Vector3 cellPosition = new Vector3(x, y, 0f) + (Vector3)center;

                Cell cell = Instantiate(cellPrefab, cellPosition, Quaternion.identity, gridParent);
                bool startState = initialState[row].values[col] == 1;
                cell.Initialize(startState);
                cells[row, col] = cell;

                var clickable = cell.GetComponent<Clickable>();
                if (clickable != null)
                {
                    int currentRow = row;
                    int currentCol = col;
                    clickable.OnClick.AddListener(() => OnCellClicked(currentRow, currentCol));
                }
                if (row == 1 && col == 1)
                {
                    backgroundSprite.transform.position = cellPosition;
                }
            }
        }
    }

    //private void CreateUIBackground()
    //{
    //    if (backgroundSprite == null)
    //    {
    //        Debug.LogWarning("������ ���� �� ��������!");
    //        return;
    //    }

    //    // ���������, ���� �� Canvas
    //    Canvas canvas = FindObjectOfType<Canvas>();
    //    if (canvas == null)
    //    {
    //        Debug.LogError("Canvas �� ������! �������� Canvas � �����.");
    //        return;
    //    }

    //    // ������ GameObject ��� ����
    //    GameObject backgroundObj = new GameObject("Background");
    //    backgroundObj.transform.SetParent(canvas.transform);

    //    // ��������� Image
    //    //Image backgroundImage = backgroundObj.AddComponent<Image>();
    //    //backgroundImage.sprite = backgroundSprite;
    //    //backgroundImage.color = backgroundColor;

    //    // ������������ ������� ����
    //    RectTransform rect = backgroundImage.rectTransform;

    //    float width = cols * spacing + backgroundPadding * 2;
    //    float height = rows * spacing + backgroundPadding * 2;

    //    rect.sizeDelta = new Vector2(width, height);
    //    rect.anchorMin = Vector2.one * 0.5f;
    //    rect.anchorMax = Vector2.one * 0.5f;
    //    rect.pivot = Vector2.one * 0.5f;
    //    rect.anchoredPosition = Vector2.zero;
    //}

    private Vector2 CalculateGridCenter()
    {
        Camera mainCamera = Camera.main;
        if (mainCamera == null)
        {
            Debug.LogError("������ �� �������!");
            return Vector2.zero;
        }

        return mainCamera.transform.position;
    }

    private void OnCellClicked(int row, int col)
    {
        ToggleCell(row, col);
        ToggleCell(row - 1, col); // ������
        ToggleCell(row + 1, col); // �����
        ToggleCell(row, col - 1); // �����
        ToggleCell(row, col + 1); // ������

        CheckWinCondition();
    }

    private void ToggleCell(int row, int col)
    {
        if (row < 0 || col < 0 || row >= rows || col >= cols) return;
        cells[row, col].Toggle();
    }

    private void CheckWinCondition()
    {
        foreach (var cell in cells)
        {
            if (!cell.IsOn) return;
        }

        Debug.Log("�� ��������!");
        onGameWin?.Invoke();
    }
}