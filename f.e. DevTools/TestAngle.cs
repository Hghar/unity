using UnityEngine;

public class TestAngle : MonoBehaviour
{
    public Transform target;
    public float rotationSpeed = 5f;
    public float maxAngle = 45f;

    private void Update()
    {
        TestAngleRotation();
    }

    private void TestAngleRotation()
    {
        Vector3 direction = target.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        angle = Mathf.Clamp(angle, -maxAngle, maxAngle);
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);
    }
}