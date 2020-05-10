
using UnityEngine;
using UnityEngine.EventSystems;

public class GlassPiecesController : MonoBehaviour
{
    public MirrorController mirror;
    public int winningPieces;

    private GlassPieceController _currentGlassPiece;

    // Start is called before the first frame update
    void Start()
    {
        var winningPositions = GetWinningPositions();

        for (int i = 0; i < transform.childCount; i++)
        {
            Transform glassPiece = transform.GetChild(i);

            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerClick;
            entry.callback.AddListener((eventData) => {
                glassPiece.gameObject.GetComponent<GlassPieceController>().InteractWithItem();
            });
            glassPiece.GetComponent<EventTrigger>().triggers.Add(entry);

            var controller = glassPiece.GetComponent<GlassPieceController>();
            controller.isWinning = winningPositions[i];
            controller.RenderWinning();
            controller.onInspection += () => { _currentGlassPiece = controller; };
            controller.onUninspection += () => { _currentGlassPiece = null; };
        }

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

    // Update is called once per frame
    void Update()
    {
        
    }
}
