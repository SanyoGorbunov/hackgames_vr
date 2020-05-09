using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlassPieceController : MonoBehaviour
{
    Transform PlayerCamera = null;
    Transform PlayerTransform = null;
    public float offset = 1.0f;

    private Vector3 startPos;
    private Quaternion startRot;

    bool canInteract = true;
    bool beignInspected = false;
    private Material glassPieceMaterial;

    // Start is called before the first frame update
    void Start()
    {
        PlayerCamera = GameObject.FindWithTag("MainCamera").transform;
        PlayerTransform = GameObject.FindWithTag("Player").transform;
        startPos = transform.position;
        startRot = transform.rotation;
        List<Material> materials = new List<Material>();
        GetComponent<MeshRenderer>().GetMaterials(materials);
        glassPieceMaterial = materials[0];
    }

    IEnumerator MoveToPosition(Vector3 newPosition, Quaternion newRotation, float time, bool makeVisible)
    {
        float elapsedTime = 0;
        Vector3 startingPos = transform.position;
        Quaternion startingRot = transform.rotation;
        while (elapsedTime < time)
        {
            transform.position = Vector3.Lerp(startingPos, newPosition, (elapsedTime / time));
            transform.rotation = Quaternion.Lerp(startingRot, newRotation, (elapsedTime / time));
            float active_value = 0.0f;
            if (makeVisible)
            {
                active_value = elapsedTime / time;
            }
            else
            {
                active_value = 1- elapsedTime / time;
            }
            glassPieceMaterial.SetFloat("_IsActive", active_value);

            elapsedTime += Time.deltaTime;
            yield return null;
        }
        if (makeVisible)
        {
            glassPieceMaterial.SetFloat("_IsActive", 1.0f);
        }
        else
        {
            glassPieceMaterial.SetFloat("_IsActive", 0.0f);
        }
        changeInspected();
    }

    void changeInspected()
    {
        beignInspected = !beignInspected;
        canInteract = true;
    }

    public void InteractWithItem()
    {
        if (canInteract)
        {
            canInteract = false;
            if (beignInspected)
            {
                UninspectPiece();
            }
            else
            {
                InspectPiece();
            }
        }
    }

    public void InspectPiece()
    {
        //PlayerCamera = GameObject.FindWithTag("Player").transform;
        Vector3 pos = PlayerCamera.position + PlayerTransform.forward * offset;
        Quaternion finalRot = Quaternion.identity;
        StartCoroutine(MoveToPosition(pos, finalRot, 0.4f,true));
    }

    public void UninspectPiece()
    {
        StartCoroutine(MoveToPosition(startPos, startRot, 0.4f, false));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
