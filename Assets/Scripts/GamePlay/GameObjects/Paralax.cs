using System.Collections.Generic;
using UnityEngine;

public class Paralax : MonoBehaviour
{
    private const int COUNT = 4;

    private readonly Deque<GameObject> paralaxDeque = new Deque<GameObject>();
    private readonly Transform[] paralaxTransforms = new Transform[COUNT];

    [SerializeField] private Sprite paralax;
    [SerializeField] private GameObject cameraObj;

    [SerializeField] private Vector3 scale;
    [SerializeField] private float divedMultiply;

    private float maxLenght;

    private float height;
    private float minY;
    private float maxY;

    private float lastCameraY;
    private Transform cameraTransform;
    private new Transform transform;

    private void Awake()
    {
        transform = GetComponent<Transform>();
        cameraTransform = cameraObj.GetComponent<Transform>();
    }

    private void Start()
    {
        PrepareParalax();
        CoroutineT.Infinite(NormilizeParalax, 1).Start(this);
    }

    private void FixedUpdate()
    {
        CreateParalaxEffect();
    }

    private void PrepareParalax()
    {
        height = scale.y * paralax.rect.height / 100 - 1;

        var lastPosition = new Vector3(0, 0, 0);

        for (int i = 0; i < COUNT; i++)
        {
            var tmp = new GameObject();

            var renderer = tmp.AddComponent<SpriteRenderer>();
            renderer.sprite = paralax;

            tmp.transform.position = lastPosition;
            tmp.transform.localScale = scale;
            tmp.name = $"Parallax - {i}";

            lastPosition.y += height;

            paralaxDeque.AddLast(tmp);
            paralaxTransforms[i] = tmp.transform;
        }

        maxLenght = COUNT * height / 2;
        maxY = lastPosition.y - height;
    }

    private void CreateParalaxEffect()
    {
        float deltaY = cameraTransform.position.y - lastCameraY;

        if (deltaY != 0)
        {
            float paralaxDeltaY = deltaY / divedMultiply;
            minY += paralaxDeltaY;
            maxY += paralaxDeltaY;

            for (int i = 0; i < COUNT; i++)
            {
                var position = paralaxTransforms[i].position;
                position.y += paralaxDeltaY;
                paralaxTransforms[i].position = position;
            }
        }

        lastCameraY = cameraTransform.position.y;
    }

    private void NormilizeParalax()
    {
        while (transform.position.y - minY > maxLenght)
        {
            maxY += height;
            minY += height;

            var down = paralaxDeque.RemoveFirst();

            var position = down.transform.position;
            position.y = maxY;
            down.transform.position = position;

            paralaxDeque.AddLast(down);
        }

        while (maxY - transform.position.y > maxLenght)
        {
            maxY -= height;
            minY -= height;

            var up = paralaxDeque.RemoveLast();

            var position = up.transform.position;
            position.y = minY;
            up.transform.position = position;

            paralaxDeque.AddFirst(up);
        }
    }
}
