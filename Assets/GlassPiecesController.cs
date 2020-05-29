
using Assets.Streamline.Scripts;
using UnityEngine;
using UnityEngine.EventSystems;

public class GlassPiecesController : MonoBehaviour
{
    public MirrorController mirror;
    public int winningPieces;

    private GlassPieceController _currentGlassPiece;
    private int[] otherPiecesNums;

    // Start is called before the first frame update
    void Start()
    {
        otherPiecesNums = new int[CubemapSingleton.GetInstance().GetNumberOfOtherMaterials()];
        var winningPositions = GetWinningPositions();

        for (int i = 0; i < transform.childCount; i++)
        {
            Transform glassPiece = transform.GetChild(i);

            /*EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerClick;
            entry.callback.AddListener((eventData) => {
                glassPiece.gameObject.GetComponent<GlassPieceController>().InteractWithItem();
            });
            glassPiece.GetComponent<EventTrigger>().triggers.Add(entry);
            */
            var controller = glassPiece.GetComponent<GlassPieceController>();
            controller.isWinning = winningPositions[i];
            //controller.ReplaceMaterial(Instantiate<Material>(GetMaterial(winningPositions[i])));
            controller.onInspection += () => { _currentGlassPiece = controller; };
            controller.onUninspection += () => { _currentGlassPiece = null; };
        }

        mirror.SetMissingNumber(winningPieces);
        mirror.RegisterEventTrigger(this);
    }

    private bool[] GetWinningPositions()
    {
        bool[] winningPositions = new bool[transform.childCount];

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

    // Update is called once per frame
    void Update()
    {
        
    }
}
