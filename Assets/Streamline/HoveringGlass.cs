using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoveringGlass : MonoBehaviour
{
    [Tooltip("Frequency at which the item will move up and down")]
    public float verticalBobFrequency = 1f;
    [Tooltip("Distance the item will move up and down")]
    public float bobbingAmount = 1f;
    [Tooltip("Rotation angle per second")]
    public float rotatingSpeed = 360f;

    [Tooltip("Sound played on pickup")]
    public AudioClip pickupSFX;
    [Tooltip("VFX spawned on pickup")]
    public GameObject pickupVFXPrefab;
    public Rigidbody pickupRigidbody { get; private set; }

    Collider m_Collider;
    Vector3 m_StartPosition;
    bool m_HasPlayedFeedback;
    float randomTimeOffset = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        m_StartPosition = gameObject.transform.position;
        randomTimeOffset = Random.Range(0.0f, 20.0f);
    }

    // Update is called once per frame
    void Update()
    {
        float bobbingAnimationPhase = ((Mathf.Sin((randomTimeOffset + Time.time) * verticalBobFrequency) * 0.5f) + 0.5f) * bobbingAmount;
        transform.position = m_StartPosition + Vector3.up * bobbingAnimationPhase;
    }
}
