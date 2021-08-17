using UnityEngine;

public class Platform : MonoBehaviour
{
    [SerializeField] private Texture[] _textures;

    private Transform _transform;

    // Start is called before the first frame update
    void Start()
    {
        _transform = GetComponent<Transform>();
        SetTexture();
    }

    void SetTexture()
    {
        for (int index = 0; index < _transform.childCount; ++index)
        {
            _transform.GetChild(index).GetComponent<Renderer>().material.mainTexture = _textures[0];
        }
    }
}
