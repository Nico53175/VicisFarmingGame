using UnityEngine;
public class PatchManager : MonoBehaviour
{
    public GridManager gridManagerPrefab;
    public Camera mainCamera;
    private Vector2Int? firstCorner;
    private bool isPatchModeActive = false;

    public void ActivatePatchMode()
    {
        isPatchModeActive = true;
        firstCorner = null;
    }

    private void Update()
    {
        if (isPatchModeActive)
        {
            if (!firstCorner.HasValue && Input.GetMouseButtonDown(0))
            {
                Vector2 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
                int gridX = Mathf.RoundToInt(mousePosition.x - 0.5f);
                int gridY = Mathf.RoundToInt(mousePosition.y - 0.5f);
                firstCorner = new Vector2Int(gridX, gridY);
                return;
            }

            if (firstCorner.HasValue && Input.GetMouseButtonDown(0))
            {
                Vector2 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
                int gridX = Mathf.RoundToInt(mousePosition.x - 0.5f);
                int gridY = Mathf.RoundToInt(mousePosition.y - 0.5f);

                Vector2Int secondCorner = new Vector2Int(gridX, gridY);
                CreatePatch(firstCorner.Value, secondCorner);

                firstCorner = null;
                isPatchModeActive = false;
            }
        }
    }

    private void CreatePatch(Vector2Int corner1, Vector2Int corner2)
    {
        int patchWidth = Mathf.Abs(corner2.x - corner1.x) + 1;
        int patchHeight = Mathf.Abs(corner2.y - corner1.y) + 1;
        Vector2Int patchPosition = new Vector2Int(Mathf.Min(corner1.x, corner2.x), Mathf.Min(corner1.y, corner2.y));

        // Shift the grid by 0.5 to match with tilemap
        GameObject gridObject = Instantiate(gridManagerPrefab.gameObject, new Vector3(patchPosition.x - 0.5f, patchPosition.y - 0.5f, 0), Quaternion.identity);
        GridManager currentGrid = gridObject.GetComponent<GridManager>();
        currentGrid.Initialize(patchWidth, patchHeight);
    }

}