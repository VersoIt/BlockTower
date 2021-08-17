using Assets.Scripts.Interfaces;
using UnityEngine;
using UnityEngine.Events;

public class ExplosiveCubes : MonoBehaviour, IDestroyable
{
    [Header("GameObject")]
    [SerializeField] private GameObject _placedCubes;
    [SerializeField] private GameObject _replayMenu;
    [SerializeField] private GameObject _destroyFX;

    [Header("Audio")]
    [SerializeField] private AudioSource _backgroundMusic;
    [SerializeField] private AudioSource _crashSound;

    [Header("Events")]
    public UnityEvent DestroyEvent;

    public AudioSource CrashSound
    { 
        get => _crashSound; 
        private set => _crashSound = value; 
    }

    public bool IsDestroyed { get; private set; } = false;

    public void OnCollisionEnter(Collision collision)
    {
        // Setting physics on child cubes
        if (collision.gameObject.tag == _placedCubes.gameObject.tag)
        {
            _crashSound.transform.position = collision.contacts[0].point;
            Destroy();
            Instantiate(_destroyFX, collision.contacts[0].point, Quaternion.identity);
        }

        _replayMenu.SetActive(true);
        Camera.main.gameObject.AddComponent<Shaker>();
    }

    public void Destroy()
    {
        _backgroundMusic.Stop();
        _crashSound.Play();

        while (_placedCubes.transform.childCount != 0)
        {
            Transform child = _placedCubes.transform.GetChild(0);
            SetPhysical(child);
            child.SetParent(null);
        }

        DestroyEvent?.Invoke();
        IsDestroyed = true;
    }

    private void SetPhysical(Transform throwingObject)
    {
        throwingObject.gameObject.AddComponent<Rigidbody>();
        throwingObject.gameObject.GetComponent<Rigidbody>().AddExplosionForce(70f, Vector3.up, 5f);
    }
}
