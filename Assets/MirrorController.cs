using UnityEngine;

public class MirrorController : MonoBehaviour
{
    public GameObject[] missingPieces;
    public Material missingMaterial;
    public Material correctMaterial;

    private int missingIndex;

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

    public void FillMissingPiece()
    {
        if (missingIndex < missingPieces.Length)
        {
            missingPieces[missingIndex].GetComponent<Renderer>().material = correctMaterial;
            missingIndex++;
        }
    }
}
