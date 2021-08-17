using UnityEngine.Rendering;
using TMPro;
using UnityEngine;

public class QualityDropdownHandler : MonoBehaviour
{
    [SerializeField] private RenderPipelineAsset[] _qualityLevels;
    [SerializeField] private HardDriveStorage _storage;
    [SerializeField] private TMP_Dropdown _dropdown;

    void Start()
    {
        _dropdown.value = _storage.GetInt();
        ChangeQualityLevel(_storage.GetInt());
    }

    public void ChangeQualityLevel(int levelIndex)
    {
        QualitySettings.SetQualityLevel(levelIndex);
        QualitySettings.renderPipeline = _qualityLevels[levelIndex];

        _storage.SetInt(levelIndex);
    }
}
