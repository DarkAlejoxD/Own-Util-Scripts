using UnityEngine;
using UnityEngine.Events;

public class AudioTrigger : MonoBehaviour
{
    [SerializeField] UnityEvent _event;

    private void OnTriggerEnter(Collider other)
    {
        _event?.Invoke();
    }
}
