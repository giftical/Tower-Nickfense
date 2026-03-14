using UnityEngine;

public class PreviewModelSpinner : MonoBehaviour
{
    [SerializeField] private float spinSpeed = 30f;

    private void Update()
    {
        transform.Rotate(0f, spinSpeed * Time.deltaTime, 0f, Space.World);
    }
}