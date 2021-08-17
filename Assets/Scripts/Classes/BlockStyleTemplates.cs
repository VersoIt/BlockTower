using UnityEngine;

class BlockStyleTemplates : MonoBehaviour
{
    [SerializeField] private Material _blockMaterial;
    [SerializeField] private Material _platformMaterial;

    [SerializeField] private Texture[] _blockTextures;
    [SerializeField] private Texture[] _platformBlocksTextures;

    [SerializeField] public AudioSource[] _soundsOfBlocks;

    [SerializeField] private HardDriveStorage _currentStyleStorage;

    public Block Block { get; set; }

    private void Awake()
    {
        Block = new Block();
    }

    private void Start()
    {
        _blockMaterial.mainTexture = _blockTextures[_currentStyleStorage.GetInt()];
        _platformMaterial.mainTexture = _platformBlocksTextures[_currentStyleStorage.GetInt()];

        Block.Material = _blockMaterial;
        Block.Sound = _soundsOfBlocks[_currentStyleStorage.GetInt()];
    }
}

