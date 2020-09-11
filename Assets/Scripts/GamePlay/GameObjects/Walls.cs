using System.Collections.Generic;
using UnityEngine;

public class Walls : MonoBehaviour
{
    private const int COUNT = 3;
    private readonly Deque<(GameObject LeftWall, GameObject RightWall)> wallDeque = new Deque<(GameObject LeftWall, GameObject RightWall)>();

    private static readonly Quaternion normal = Quaternion.Euler(0, 0, 0);
    private static readonly Quaternion reverse = Quaternion.Euler(0, 180, 0);

    [SerializeField] private GameObject wall;

    private float maxLenght;

    private float height;
    private float minY;
    private float maxY;

    private new Transform transform;

    private void Awake()
    {
        transform = GetComponent<Transform>();
    }

    private void Start()
    {
        SpawnWalls();
        CoroutineT.Infinite(NormalizeWalls, 1).Start(this);
    }

    private void SpawnWalls()
    {
        var intsWalls = new GameObject
        {
            name = "Walls"
        };

        float heightSprite = wall.GetComponent<SpriteRenderer>().sprite.rect.height / 100;
        height = wall.transform.localScale.y * heightSprite * 1.001f;

        maxLenght = COUNT * height / 2;

        float lastY = 0;
        for (int i = 0; i < COUNT; i++)
        {
            var position = new Vector3(0, lastY, 0);

            var objRight = Instantiate
            (
                wall,
                position,
                reverse,
                intsWalls.transform
            );
            objRight.name = $"Right wall ({i})";

            var objLeft = Instantiate
            (
                wall,
                position,
                normal,
                intsWalls.transform
            );
            objLeft.name = $"Left wall ({i})";

            lastY += height;

            wallDeque.AddLast((objLeft, objRight));
        }

        maxY = lastY - height;
    }

    private void NormalizeWalls()
    {
        while (transform.position.y - minY > maxLenght)
        {
            maxY += height;
            minY += height;

            var down = wallDeque.RemoveFirst();

            var leftPos = down.LeftWall.transform.position;
            var rightPos = down.RightWall.transform.position;

            leftPos.y = maxY;
            rightPos.y = maxY;

            down.LeftWall.transform.position = leftPos;
            down.RightWall.transform.position = rightPos;

            wallDeque.AddLast(down);
        }

        while (maxY - transform.position.y > maxLenght)
        {
            maxY -= height;
            minY -= height;

            var up = wallDeque.RemoveLast();

            var leftPos = up.LeftWall.transform.position;
            var rightPos = up.RightWall.transform.position;

            leftPos.y = minY;
            rightPos.y = minY;

            up.LeftWall.transform.position = leftPos;
            up.RightWall.transform.position = rightPos;

            wallDeque.AddFirst(up);
        }
    }
}