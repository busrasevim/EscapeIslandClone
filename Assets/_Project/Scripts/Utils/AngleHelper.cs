using UnityEngine;

public static class AngleHelper
{
    public static Vector3 AngleLerp(Vector3 firstAngle, Vector3 lastAngle, float lerpSpeed = 0.1f)
    {
        var x = Mathf.LerpAngle(firstAngle.x, lastAngle.x, lerpSpeed);
        var y = Mathf.LerpAngle(firstAngle.y, lastAngle.y, lerpSpeed);
        var z = Mathf.LerpAngle(firstAngle.z, lastAngle.z, lerpSpeed);
        return new Vector3(x, y, z);
    }

    public static bool IsObjectOnLeft(Vector3 centerPosition, Vector3 controlPosition)
    {
        var direction = controlPosition - centerPosition;

        var angle = Mathf.Atan2(direction.z, direction.x) * Mathf.Rad2Deg;

        if (angle < 0)
        {
            angle += 360;
        }

        return !(angle < 90f) && !(angle > 270f);
    }
}
