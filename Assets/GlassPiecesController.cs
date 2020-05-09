
using UnityEngine;
using UnityEngine.EventSystems;

public class GlassPiecesController : MonoBehaviour
{
    public MirrorController mirror;

    // Start is called before the first frame update
    void Start()
    {
        foreach (Transform glassPiece in transform)
        {
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerClick;
            entry.callback.AddListener((eventData) => { glassPiece.gameObject.GetComponent<GlassPieceController>().InteractWithItem(); });
            glassPiece.GetComponent<EventTrigger>().triggers.Add(entry);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
