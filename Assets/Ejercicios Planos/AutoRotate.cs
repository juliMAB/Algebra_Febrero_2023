using UnityEngine;

public class AutoRotate : MonoBehaviour
{
    public float speed;
    void Update()
    {
        this.transform.Rotate(Vector3.up*speed*Time.deltaTime);
    }
}
