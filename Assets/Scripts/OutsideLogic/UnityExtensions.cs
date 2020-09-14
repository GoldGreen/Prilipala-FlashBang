using UnityEngine;

public static class UnityExtensions
{
    public static Vector3 Change(this Vector3 vector, float? x = null, float? y = null, float? z = null)
    {
        return new Vector3(x ?? vector.x, y ?? vector.y, z ?? vector.z);
    }

    public static Vector2 Change(this Vector2 vector, float? x = null, float? y = null)
    {
        return new Vector2(x ?? vector.x, y ?? vector.y);
    }

    public static Vector3 Delta(this Vector3 vector, float? x = null, float? y = null, float? z = null)
    {
        return new Vector3(vector.x + (x ?? 0), vector.y + (y ?? 0), vector.z + (z ?? 0));
    }

    public static Vector2 Delta(this Vector2 vector, float? x = null, float? y = null)
    {
        return new Vector2(vector.x + (x ?? 0), vector.y + (y ?? 0));
    }

    public static Vector3 Multiply(this Vector3 vector, float? x = null, float? y = null, float? z = null)
    {
        return new Vector3(vector.x * (x ?? 1), vector.y * (y ?? 1), vector.z * (z ?? 1));
    }

    public static Vector2 Multiply(this Vector2 vector, float? x = null, float? y = null)
    {
        return new Vector2(vector.x * (x ?? 1), vector.y * (y ?? 1));
    }


    public static Vector2 ToVectorFromRad(this float angleInRad)
    {
        return new Vector2(Mathf.Cos(angleInRad), Mathf.Sin(angleInRad));
    }

    public static Rigidbody2D AddForce(this Rigidbody2D rigidbody, float x, float y)
    {
        rigidbody.AddForce(new Vector2(x, y));
        return rigidbody;
    }

    public static float Atan2(this Vector2 vector)
    {
        return Mathf.Atan2(vector.y, vector.x);
    }

    public static float Atan2(this Vector3 vector)
    {
        return Mathf.Atan2(vector.y, vector.x);
    }

    public static Quaternion ToQuartetion(this Vector2 vector)
    {
        float zAngle = Mathf.Rad2Deg * Mathf.Atan2(vector.y, vector.x);
        float yAngle = 0;

        if (zAngle < -90 || zAngle > 90)
        {
            yAngle = 180;
            zAngle = 180 - zAngle;
        }

        return Quaternion.Euler(0, yAngle, zAngle);
    }

    public static Quaternion ToQuartetion(this Vector3 vector)
    {
        float zAngle = Mathf.Rad2Deg * Mathf.Atan2(vector.y, vector.x);
        float yAngle = 0;

        if (zAngle < -90 || zAngle > 90)
        {
            yAngle = 180;
            zAngle = 180 - zAngle;
        }

        return Quaternion.Euler(0, yAngle, zAngle);
    }
}
