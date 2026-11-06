
using UnityEngine;
using UnityEngine.InputSystem;

public class BoyRunner : MonoBehaviour
{
    public float forwardSpeed = 8f;
    public float laneDistance = 2f;
    public float laneChangeSpeed = 12f;
    public float jumpHeight = 2f;
    public float jumpDuration = 0.7f;

    private int currentLane = 1;
    private float startY;
    private float jumpTimer;
    private Transform cameraTransform;

    void Start()
    {
        startY = transform.position.y;

        CreateBoy();

        Camera cam = Camera.main;
        if (cam != null)
        {
            cameraTransform = cam.transform;
            cameraTransform.position = transform.position + new Vector3(0, 5, -8);
            cameraTransform.rotation = Quaternion.Euler(18, 0, 0);
        }
    }

    void CreateBoy()
    {
        // Body and head
        MakePart("Body", PrimitiveType.Capsule,
            new Vector3(0, 1.1f, 0), new Vector3(0.65f, 0.85f, 0.4f),
            new Color(0.1f, 0.3f, 0.85f));

        MakePart("Head", PrimitiveType.Sphere,
            new Vector3(0, 2.15f, 0), new Vector3(0.55f, 0.6f, 0.5f),
            new Color(0.85f, 0.55f, 0.35f));

        MakePart("Hair", PrimitiveType.Sphere,
            new Vector3(0, 2.43f, 0), new Vector3(0.57f, 0.25f, 0.52f),
            new Color(0.12f, 0.07f, 0.04f));

        MakePart("LeftEye", PrimitiveType.Sphere,
            new Vector3(-0.13f, 2.2f, 0.23f), Vector3.one * 0.07f, Color.black);

        MakePart("RightEye", PrimitiveType.Sphere,
            new Vector3(0.13f, 2.2f, 0.23f), Vector3.one * 0.07f, Color.black);

        // Arms and legs
        MakePart("LeftArm", PrimitiveType.Capsule,
            new Vector3(-0.48f, 1.15f, 0), new Vector3(0.22f, 0.55f, 0.22f),
            new Color(0.85f, 0.55f, 0.35f));

        MakePart("RightArm", PrimitiveType.Capsule,
            new Vector3(0.48f, 1.15f, 0), new Vector3(0.22f, 0.55f, 0.22f),
            new Color(0.85f, 0.55f, 0.35f));

        MakePart("LeftLeg", PrimitiveType.Capsule,
            new Vector3(-0.2f, 0.35f, 0), new Vector3(0.23f, 0.55f, 0.23f),
            new Color(0.25f, 0.15f, 0.08f));

        MakePart("RightLeg", PrimitiveType.Capsule,
            new Vector3(0.2f, 0.35f, 0), new Vector3(0.23f, 0.55f, 0.23f),
            new Color(0.25f, 0.15f, 0.08f));
    }

    void MakePart(string partName, PrimitiveType type,
        Vector3 localPosition, Vector3 scale, Color color)
    {
        GameObject part = GameObject.CreatePrimitive(type);
        part.name = partName;
        part.transform.SetParent(transform, false);
        part.transform.localPosition = localPosition;
        part.transform.localScale = scale;

        Collider col = part.GetComponent<Collider>();
        if (col != null) Destroy(col);

        Renderer rend = part.GetComponent<Renderer>();
        rend.material.color = color;
    }

    void Update()
    {
        if (Keyboard.current != null)
        {
            if (Keyboard.current.leftArrowKey.wasPressedThisFrame ||
                Keyboard.current.aKey.wasPressedThisFrame)
                currentLane = Mathf.Max(0, currentLane - 1);

            if (Keyboard.current.rightArrowKey.wasPressedThisFrame ||
                Keyboard.current.dKey.wasPressedThisFrame)
                currentLane = Mathf.Min(2, currentLane + 1);

            if ((Keyboard.current.spaceKey.wasPressedThisFrame ||
                Keyboard.current.upArrowKey.wasPressedThisFrame) && jumpTimer <= 0)
                jumpTimer = jumpDuration;
        }

        Vector3 pos = transform.position;

        // Run forward
        pos.z += forwardSpeed * Time.deltaTime;

        // Move between lanes
        float targetX = (currentLane - 1) * laneDistance;
        pos.x = Mathf.Lerp(pos.x, targetX, laneChangeSpeed * Time.deltaTime);

        // Jump
        if (jumpTimer > 0)
        {
            jumpTimer -= Time.deltaTime;
            pos.y = startY +
                Mathf.Sin((1f - jumpTimer / jumpDuration) * Mathf.PI) * jumpHeight;
        }
        else
        {
            pos.y = startY;
        }

        transform.position = pos;

        // Follow the boy
        if (cameraTransform != null)
        {
            cameraTransform.position = Vector3.Lerp(
                cameraTransform.position,
                transform.position + new Vector3(0, 5, -8),
                5f * Time.deltaTime);

            cameraTransform.LookAt(transform.position + new Vector3(0, 1, 5));
        }

        // Running animation
        float swing = Mathf.Sin(Time.time * 12f) * 30f;

        Transform leftArm = transform.Find("LeftArm");
        Transform rightArm = transform.Find("RightArm");
        Transform leftLeg = transform.Find("LeftLeg");
        Transform rightLeg = transform.Find("RightLeg");

        if (leftArm != null) leftArm.localRotation = Quaternion.Euler(swing, 0, 0);
        if (rightArm != null) rightArm.localRotation = Quaternion.Euler(-swing, 0, 0);
        if (leftLeg != null) leftLeg.localRotation = Quaternion.Euler(-swing, 0, 0);
        if (rightLeg != null) rightLeg.localRotation = Quaternion.Euler(swing, 0, 0);
    }
}
