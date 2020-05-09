using UnityEngine;

public class GeneratePieces : MonoBehaviour
{
    public int pieceCount;
    public GameObject point;
    public GameObject prefab;
    public float radius;
    public PlacementType placementType;

    public void Start()
    {
        switch (placementType)
        {
            case PlacementType.Surface:
                InstantiateOnTable();
                break;
            case PlacementType.Flying:
                break;
            case PlacementType.FlyingAroundPlayer:
                InstantiateCircle();
                break;
        }
    }

    void InstantiateCircle()
    {
        float angle = 360f / (float)pieceCount;
        for (int i = 0; i < pieceCount; i++)
        {
            Quaternion rotation = Quaternion.AngleAxis(i * angle, Vector3.up);
            Vector3 direction = rotation * Vector3.forward;

            Vector3 position = point.transform.position + (direction * radius);
            Instantiate(prefab, position, rotation);
        }
    }

    void InstantiateOnTable()
    {
        // get size of piece
        var prefabRenderer = prefab.GetComponent<Renderer>();
        // get size of table
        var tableRenderer = point.GetComponent<Renderer>();

        float offset = 0.05f;
        float tableX = tableRenderer.bounds.size.x,
            tableZ = tableRenderer.bounds.size.z;
        float pieceX = prefab.transform.localScale.x,
            pieceZ = prefab.transform.localScale.y;

        int pieceToRender = pieceCount;
        int rows = 4;
        int maxInRow = pieceCount / rows;

        float cellX = (tableX - 2 * offset) / maxInRow,
            cellZ = (tableZ - 2* offset) / rows;

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < (maxInRow < pieceToRender ? maxInRow : pieceToRender); j++)
            {
                var gameObject = Instantiate(prefab, Vector3.zero, Quaternion.identity);

                float xElem = j * cellX + cellX / 2,
                    zElem = i * cellZ + cellZ / 2;

                Vector3 position = new Vector3(
                    tableRenderer.bounds.center.x - tableRenderer.bounds.extents.x + xElem + pieceX / 2f,
                    tableRenderer.bounds.center.y + tableRenderer.bounds.extents.y + prefabRenderer.bounds.extents.y - pieceZ / 2f + offset,
                    tableRenderer.bounds.center.z - tableRenderer.bounds.extents.z + zElem);

                gameObject.transform.position = position;
                gameObject.transform.Rotate(90, 90, Random.Range(0, 180));
            }

            pieceToRender -= maxInRow;
        }
    }
}

public enum PlacementType
{
    Surface,
    Flying,
    FlyingAroundPlayer
}
