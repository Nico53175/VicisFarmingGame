using System;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    // Grid SetUp
    public int width;
    public int height;
    public GameObject plantPrefab;
    public GameObject[,] grid;

    public Sprite[] cellSprites; // array of sprites: 0-left, 1-middle, 2-right
    public SpriteRenderer[,] cellRenderers;

    public void Initialize(int newWidth, int newHeight)
    {
        width = newWidth;
        height = newHeight;
        grid = new GameObject[width, height];
        cellRenderers = new SpriteRenderer[width, height];
        AssignSpritesToGrid();

        CreateMainCollider();
        CreateOuterCollider();
    }

    private void AssignSpritesToGrid()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                GameObject cellObject = new GameObject($"Cell_{x}_{y}");
                cellObject.transform.parent = transform;
                cellObject.transform.localPosition = new Vector3(x, y, 0);

                SpriteRenderer cellRenderer = cellObject.AddComponent<SpriteRenderer>();
                cellRenderer.sprite = GetSpriteForCell(x, y);
            }
        }
    }

    private Sprite GetSpriteForCell(int x, int y)
    {
        // In case of 1x1 grid
        if (width == 1 && height == 1)
        {
            return cellSprites[6]; // singleTile sprite
        }
        else if (width >= height)
        {
            // Horizontal layout
            if (x == 0) return cellSprites[0];  // Left
            else if (x == width - 1) return cellSprites[2];  // Right
            else return cellSprites[1];  // MiddleHorizontal
        }
        else
        {
            // Vertical layout
            if (y == 0) return cellSprites[5];  // Up
            else if (y == height - 1) return cellSprites[3];  // Down
            else return cellSprites[4];  // MiddleVertical
        }
    }

    private void CreateMainCollider()
    {
        BoxCollider2D mainCollider = GetComponent<BoxCollider2D>();
        mainCollider.isTrigger = true;
        mainCollider.size = new Vector2(width, height);
        mainCollider.offset = new Vector2(width / 2f - 0.5f, height / 2f - 0.5f);
        mainCollider.tag = "Main Patch Collider";
        gameObject.layer = LayerMask.NameToLayer("Patch");
    }

    private void CreateOuterCollider()
    {
        GameObject outerColliderObject = new GameObject("OuterCollider");
        outerColliderObject.transform.parent = transform;
        outerColliderObject.transform.localPosition = Vector3.zero;
        outerColliderObject.tag = "Outer Patch Collider";
        outerColliderObject.layer = LayerMask.NameToLayer("Ignore Raycast");

        BoxCollider2D outerCollider = outerColliderObject.AddComponent<BoxCollider2D>();
        outerCollider.isTrigger = true;
        outerCollider.size = new Vector2(width + 6, height + 6);
        outerCollider.offset = new Vector2(width / 2f - 0.5f, height / 2f - 0.5f);
    }

    public void Plant(PlantData plantData, int x, int y)
    {
        Debug.Log($"Plant {plantData.plantName} at position ({x}, {y})");
        Vector3 plantPosition = new Vector3(x + transform.position.x, y + transform.position.y, 0);
        GameObject newPlant = Instantiate(plantPrefab, plantPosition, Quaternion.identity, transform);
        PlantController plantController = newPlant.GetComponent<PlantController>();
        plantController.InitializePlant(plantData);
        grid[x, y] = newPlant;
    }

    private void OutlineGrid()
    {
        float offsetX = 0.5f;
        float offsetY = 0.5f;

        for (int i = 0; i <= width; i++)
        {
            CreateLine(transform.position + new Vector3(i - offsetX, -offsetY, 0), transform.position + new Vector3(i - offsetX, height - offsetY, 0));
        }

        for (int j = 0; j <= height; j++)
        {
            CreateLine(transform.position + new Vector3(-offsetX, j - offsetY, 0), transform.position + new Vector3(width - offsetX, j - offsetY, 0));
        }
    }

    private void CreateLine(Vector3 startPoint, Vector3 endPoint)
    {
        GameObject lineObject = new GameObject("GridLine");
        lineObject.transform.parent = transform;
        LineRenderer lineRenderer = lineObject.AddComponent<LineRenderer>();
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = lineRenderer.endColor = Color.black;
        lineRenderer.startWidth = lineRenderer.endWidth = 0.1f;
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, startPoint);
        lineRenderer.SetPosition(1, endPoint);
    }
}


