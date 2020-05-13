using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MirrorController : MonoBehaviour
{
    private const float DelayBeforeNextScene = 3.0f;

    public GameObject[] missingPieces;
    public Material missingMaterial;
    public Material correctMaterial;
    public string nextSceneName;

    private int missingIndex;
    private int missingNumber;

    // Start is called before the first frame update
    void Start()
    {
        foreach (Transform piece in transform)
        {
            piece.gameObject.GetComponent<Renderer>().material = correctMaterial;
        }

        foreach (var missingPiece in missingPieces)
        {
            missingPiece.GetComponent<Renderer>().material = missingMaterial;
        }

        missingIndex = 0;
    }

    public void RegisterEventTrigger(GlassPiecesController glassPiecesController)
    {
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerClick;
        entry.callback.AddListener((eventData) => {
            if (glassPiecesController.IsCurrentPieceWinning())
            {
                GlassPieceController.ActiveGlassPiece.MoveToDestroy(this.transform);
                FillMissingPiece();
            }
        });

        foreach (Transform piece in transform)
        {
            piece.gameObject.GetComponent<EventTrigger>().triggers.Add(entry);
        }
    }

    public void RegisterEventTrigger(GeneratePieces generatePiecesController)
    {
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerClick;
        entry.callback.AddListener((eventData) => {
            if (generatePiecesController.IsCurrentPieceWinning())
            {
                GlassPieceController.ActiveGlassPiece.MoveToDestroy(this.transform);
                FillMissingPiece();
            }
        });

        foreach (Transform piece in transform)
        {
            piece.gameObject.GetComponent<EventTrigger>().triggers.Add(entry);
        }
    }

    public void SetMissingNumber(int missingNumber)
    {
        foreach (Transform piece in transform)
        {
            piece.gameObject.GetComponent<Renderer>().material = correctMaterial;
        }

        for (int i = 0; i < missingNumber; i++)
        {
            missingPieces[i].GetComponent<Renderer>().material = missingMaterial;
        }

        missingIndex = 0;
        this.missingNumber = missingNumber;
    }

    public void FillMissingPiece()
    {
        missingPieces[missingIndex].GetComponent<Renderer>().material = correctMaterial;
        missingIndex++;

        if (missingIndex == missingNumber)
        {
            Invoke("LoadNextScene", DelayBeforeNextScene);
        }
    }

    public void LoadNextScene()
    {
        SceneManager.LoadScene(nextSceneName);
    }
}
