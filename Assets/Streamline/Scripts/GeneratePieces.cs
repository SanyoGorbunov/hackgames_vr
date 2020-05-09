using UnityEngine;

public class GeneratePieces : MonoBehaviour
{
    public int pieceCount;
    public Transform point;
    public GameObject prefab;
    public float radius;
    public PlacementType placementType;

    public void Start()
    {
        switch (placementType)
        {
            case PlacementType.Surface:
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

            Vector3 position = point.position + (direction * radius);
            Instantiate(prefab, position, rotation);
        }
    }
}

public enum PlacementType
{
    Surface,
    Flying,
    FlyingAroundPlayer
}
