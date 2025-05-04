using System;
using UnityEngine;
using UnityEngine.Events;

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

    [Header("События")]
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
        rows = initialState.GetLength(0);
        if (rows == 0) return;

        cols = initialState[0].values.Length;

        // Проверяем корректность матрицы
        for (int i = 1; i < rows; i++)
        {
            if (initialState[i].values.Length != cols)
            {
                Debug.LogError($"Строка {i} имеет разную длину! Ожидается {cols} элементов.");
                return;
            }
        }

        cells = new Cell[rows, cols];

        // Вычисляем центральную точку
        Vector2 center = CalculateGridCenter();

        // Создаём клетки
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < cols; col++)
            {
                // Вычисляем позицию с явным указанием Z=0
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
            }
        }
    }

    private Vector2 CalculateGridCenter()
    {
        // Центр камеры
        Camera mainCamera = Camera.main;
        if (mainCamera == null)
        {
            Debug.LogError("Камера не найдена!");
            return Vector2.zero;
        }

        return mainCamera.transform.position;
    }

    private void OnCellClicked(int row, int col)
    {
        ToggleCell(row, col);
        ToggleCell(row - 1, col); // Сверху
        ToggleCell(row + 1, col); // Снизу
        ToggleCell(row, col - 1); // Слева
        ToggleCell(row, col + 1); // Справа

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

        Debug.Log("Вы победили!");
        onGameWin?.Invoke();
    }
}