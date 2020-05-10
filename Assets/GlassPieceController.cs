using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GlassPieceController : MonoBehaviour
{
    Transform PlayerCameraTransform = null;
    Transform PlayerTransform = null;
    public float offset = 1.0f;
    public float z_offset = 0.2f;

    private Vector3 startPos;
    private Quaternion startRot;

    bool canInteract = true;
    bool beignInspected = false;
    private Material glassPieceMaterial;

    private Transform parentTransform;
    private Transform MirrorTransform;

    private Vector2 VectorToMirror;  

    public bool isWinning;
    public UnityAction onInspection;
    public UnityAction onUninspection;

    public static GlassPieceController ActiveGlassPiece = null;

    // Start is called before the first frame update
    void Start()
    {
        PlayerCameraTransform = GameObject.FindWithTag("MainCamera").transform;
        PlayerTransform = GameObject.FindWithTag("Player").transform;
        startPos = transform.position;
        startRot = transform.rotation;
        EnsureMaterial();
        parentTransform = transform.parent;
        MirrorTransform = FindObjectOfType<MirrorController>().gameObject.transform;

        Vector2 mirrorPos = new Vector2(MirrorTransform.position.x, MirrorTransform.position.z);
        Vector2 PlayerPos = new Vector2(PlayerTransform.position.x, PlayerTransform.position.z);

        VectorToMirror = mirrorPos - PlayerPos;
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

        if (!beignInspected)
        {
            transform.SetParent(PlayerCameraTransform);
        }
        else
        {
            transform.SetParent(parentTransform);
        }

        changeInspected();
    }

    void changeInspected()
    {
        beignInspected = !beignInspected;
        if (beignInspected)
        {
            ActiveGlassPiece = this;
        }
        else
        {
            ActiveGlassPiece = null;
        }
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
        Vector3 pos = PlayerCameraTransform.position + PlayerCameraTransform.forward * offset;
        Quaternion finalRot = PlayerCameraTransform.rotation;
        StartCoroutine(MoveToPosition(pos, finalRot, 0.4f,true));

        if (onInspection != null)
        {
            onInspection.Invoke();
        }
    }

    public void UninspectPiece()
    {
        StartCoroutine(MoveToPosition(startPos, startRot, 0.4f, false));

        if (onUninspection != null)
        {
            onUninspection.Invoke();
        }
    }

    void EnsureMaterial(Material materialToOverride = null)
    {
        if (glassPieceMaterial == null)
        {
            List<Material> materials = new List<Material>();
            GetComponent<MeshRenderer>().GetMaterials(materials);
            glassPieceMaterial = materials[0];
            glassPieceMaterial.SetFloat("_IsActive", 0.0f);
        }

        if (materialToOverride != null)
        {
            GetComponent<MeshRenderer>().material = materialToOverride;
            glassPieceMaterial = materialToOverride;
            glassPieceMaterial.SetFloat("_IsActive", 0.0f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        glassPieceMaterial.SetVector("_ViewVector", transform.position-transform.forward);
        glassPieceMaterial.SetVector("_RightVector", transform.right);

        if (beignInspected && canInteract)
        {
                        
            Vector2 PlayerForward = new Vector2(PlayerCameraTransform.forward.x, PlayerCameraTransform.forward.z);

            float angle = Vector2.Angle(PlayerForward, VectorToMirror);
            if (angle < 30.0f)
            {
                transform.position = Vector3.Lerp(transform.position,PlayerCameraTransform.position + PlayerCameraTransform.forward * offset - PlayerCameraTransform.up * z_offset, 0.2f); ;
                transform.rotation = PlayerCameraTransform.rotation;
            }
            else {
                transform.position = Vector3.Lerp(transform.position, PlayerCameraTransform.position + PlayerCameraTransform.forward * offset,0.2f);
                transform.rotation = PlayerCameraTransform.rotation;
            }
        }
    }

    public void MoveToDestroy(Transform mirror)
    {
        StartCoroutine(MoveToPositionToDestroy(mirror.position, mirror.rotation, 0.3f));
    }

    IEnumerator MoveToPositionToDestroy(Vector3 newPosition, Quaternion newRotation, float time)
    {
        canInteract = false;
        float elapsedTime = 0;
        Vector3 startingPos = transform.position;
        Quaternion startingRot = transform.rotation;
        while (elapsedTime < time)
        {
            transform.position = Vector3.Lerp(startingPos, newPosition, (elapsedTime / time));
            //transform.rotation = Quaternion.Lerp(startingRot, newRotation, (elapsedTime / time));

            elapsedTime += Time.deltaTime;
            yield return null;
        }
        beignInspected = false;
        ActiveGlassPiece = null;
        Destroy(this.gameObject);
    }

    public void ReplaceMaterial(Material material)
    {
        EnsureMaterial(material);

        if (isWinning)
        {
            glassPieceMaterial.SetColor("_Color", Color.red);
        } else
        {
            glassPieceMaterial.SetColor("_Color", Color.white);
        }
    }
}
