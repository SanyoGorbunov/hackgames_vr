using Assets.Streamline.Scripts;
using UnityEngine;
using UnityEngine.EventSystems;

public class GeneratePieces : MonoBehaviour
{
    public int pieceCount;
    public GameObject point;
    public GameObject prefab;
    public float radius;

    public MirrorController mirror;
    public int winningPieces;

    private GlassPieceController _currentGlassPiece;
    private int[] otherPiecesNums;

    public void Start()
    {
        otherPiecesNums = new int[CubemapSingleton.GetInstance().GetNumberOfOtherMaterials()];

        InstantiateCircle();

        mirror.SetMissingNumber(winningPieces);
        mirror.RegisterEventTrigger(this);
    }

    void InstantiateCircle()
    {
        var winningPositions = GetWinningPositions();

        float angle = 360f / (float)pieceCount;
        for (int i = 0; i < pieceCount; i++)
        {
            Quaternion rotation = Quaternion.AngleAxis(i * angle, Vector3.up);
            Vector3 direction = rotation * Vector3.forward;

            Vector3 position = point.transform.position + (direction * radius);
            var pieceGameObject = Instantiate(prefab, position, rotation);
            Transform glassPiece = pieceGameObject.transform;

            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerClick;
            entry.callback.AddListener((eventData) => {
                glassPiece.gameObject.GetComponent<GlassPieceController>().InteractWithItem();
            });
            glassPiece.GetComponent<EventTrigger>().triggers.Add(entry);

            var controller = glassPiece.GetComponent<GlassPieceController>();
            controller.isWinning = winningPositions[i];
            controller.ReplaceMaterial(Instantiate<Material>(GetMaterial(controller.isWinning)));
            controller.onInspection += () => { _currentGlassPiece = controller; };
            controller.onUninspection += () => { _currentGlassPiece = null; };
        }
    }

    private bool[] GetWinningPositions()
    {
        bool[] winningPositions = new bool[pieceCount];

        for (int i = 0; i < winningPieces; i++)
        {
            int winningPosition = 0;
            do
            {
                winningPosition = Random.Range(0, winningPositions.Length);
            } while (winningPositions[winningPosition]);
            winningPositions[winningPosition] = true;
        }

        return winningPositions;
    }

    public bool IsCurrentPieceWinning()
    {
        if (_currentGlassPiece != null && _currentGlassPiece.isWinning)
        {
            return true;
        }

        return false;
    }

    public Material GetMaterial(bool isWinning)
    {
        if (isWinning)
        {
            return CubemapSingleton.GetInstance().GetByNextScene(mirror.nextSceneName);
        }

        int materialId;
        do
        {
            materialId = Random.Range(0, otherPiecesNums.Length);
        } while (otherPiecesNums[materialId] + 1 == winningPieces);

        otherPiecesNums[materialId]++;
        return CubemapSingleton.GetInstance().GetAnotherMaterialById(materialId);
    }
}
