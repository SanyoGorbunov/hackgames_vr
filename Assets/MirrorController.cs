using UnityEngine;
using UnityEngine.SceneManagement;

public class MirrorController : MonoBehaviour
{
    public GameObject[] missingPieces;
    public Material missingMaterial;
    public Material correctMaterial;
    public string nextSceneName;

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
        missingPieces[missingIndex].GetComponent<Renderer>().material = correctMaterial;
        missingIndex++;

        if (missingIndex == missingPieces.Length)
        {
            SceneManager.LoadScene(nextSceneName);
        }
    }
}
