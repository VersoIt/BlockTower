using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Interfaces;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class GameLogistics : MonoBehaviour, IGame, IDestroyable, ILeave
{

    [Header("Blocks")]
    [SerializeField] private GameObject _cubeToPlace;

    [SerializeField] private ExplosiveCubes _explosiveCubes;

    [Header("FX")]
    [SerializeField] private GameObject _putFX;

    [Header("Camera")]
    [SerializeField] private GameObject _camera;

    [Header("Menu")]
    [SerializeField] private GameObject _exitMenu;
    [SerializeField] private GameObject _mainMenu;
    [SerializeField] private GameObject _gameMenu;
    [SerializeField] private GameObject _pauseMenu;
    [SerializeField] private GameObject _replayMenu;

    [Header("Audio")]
    [SerializeField] private AudioClip _audioClip;

    [Header("Records")]
    private RecordsProvider _records;

    [Header("Block transforms")]
    [SerializeField] private Transform _selectedPlace;
    [SerializeField] private Transform _placedCubes;

    [Header("Currency counters")]
    [SerializeField] private CurrencyCounter _globalGemsCounter;

    [SerializeField] private TextMeshProUGUI _gemsCounter;

    private Block _currentBlock;

    private Vector3 _cubePosition = new Vector3(0, 1, 0);
    private readonly Vector3[] _possibleVectors = new Vector3[6]
    {
        new Vector3(1, 0, 0),
        new Vector3(0, 1, 0),
        new Vector3(0, 0, 1),
        new Vector3(0, 0, -1),
        new Vector3(0, -1, 0),
        new Vector3(-1, 0, 0)
    };
    private List<Vector3> _forbiddenPositions = new List<Vector3>
    {
        new Vector3(0, 0, 0),
        new Vector3(1, 0, 0),
        new Vector3(1, 0, 1),
        new Vector3(0, 0, 1),
        new Vector3(-1, 0, 1),
        new Vector3(-1, 0, 0),
        new Vector3(-1, 0, -1),
        new Vector3(0, 0, -1),
        new Vector3(1, 0, -1)
    };
    private Vector3 _moveAwayCameraPosition;

    private Rigidbody _placedCubesRigidbody;

    private bool _isLose = false;
    public bool IsPause { get; private set; }
    public bool IsStarted { get; private set; } = false;

    private Coroutine _showCubePlaceCoroutine;
    private AudioSource _backgroundMusic;

    private Counter _currentGemsCount;

    // This data is used for the highscore system
    private int _maxHeight;
    private int _maxWidth;

    private int _prevMaxWidth;
    private int _prevMaxHeight;

    private int _cubeCounter;

    private Transform _cameraTransform;

    private CameraController _cameraController;

    private void Awake()
    {
        _records = GetComponent<RecordsProvider>();
        _backgroundMusic = GetComponent<AudioSource>();
        _cameraController = _camera.GetComponent<CameraController>();
        _placedCubesRigidbody = _placedCubes.GetComponent<Rigidbody>();
        _currentGemsCount = GetComponent<Counter>();
    }

    // Start is called before the first frame update
    private void Start()
    {
        _cameraTransform = _camera.gameObject.transform;
        _selectedPlace.GetComponent<Renderer>().material.color = new Color(1.3f, 1.3f, 1.3f, 0.9f);
        _showCubePlaceCoroutine = StartCoroutine(ShowCubePlace());
        _currentBlock = GetComponent<BlockStyleTemplates>().Block;
        _moveAwayCameraPosition = new Vector3(_camera.transform.localPosition.x, _camera.transform.localPosition.y, _camera.transform.localPosition.z - CameraController.CameraMoveSpeed);

        AudioListener.pause = false;

        if (GameMenu.Instance.IsEnabled)
            LaunchGameMenu();
        else
            LaunchMainMenu();
    }

    // Update is called once per frame
    private void Update()
    {

        if ((Input.GetKeyDown(KeyCode.Mouse0) || Input.touchCount > 0) && _selectedPlace != null && !EventSystem.current.IsPointerOverGameObject())
        {

#if !UNITY_EDITOR

        if (Input.GetTouch(0).phase != TouchPhase.Began)
            return;

#endif

            if (!GameMenu.Instance.IsEnabled)
            {
                LaunchGameMenu();
                GameMenu.Instance.IsEnabled = true;
                IsStarted = true;
            }

            CreateNewCube();
            ++_cubeCounter;

            _forbiddenPositions.Add(_cubePosition);
            _moveAwayCameraPosition = new Vector3(_camera.transform.localPosition.x, _camera.transform.localPosition.y, _moveAwayCameraPosition.z);
            _cubePosition = _selectedPlace.transform.position;
            _maxWidth = (int)Math.Max(Math.Max(Math.Abs(_cubePosition.x), Math.Abs(_cubePosition.z)), _maxWidth);
            _maxHeight = Math.Max(Math.Abs(_maxHeight), (int)Math.Abs(_cubePosition.y));
            

            _records.UpdateData(_maxWidth, _maxHeight, 1);
            Destroy(Instantiate(_putFX, _cubePosition, Quaternion.identity), 2.5f);

            // Formula for gems
            if ((_maxWidth % 3 == 0 && _maxWidth != _prevMaxWidth) || ((_maxHeight % 3 == 0) && _maxHeight != _prevMaxHeight) || _cubeCounter % 5 == 0)
            {
                ++_currentGemsCount.Count;
                ++_globalGemsCounter.CurrencyCount;
                _prevMaxWidth = _maxWidth;
                _prevMaxHeight = _maxHeight;
                _gemsCounter.text = $"<sprite=0>{_currentGemsCount.Count}";
            }

            _placedCubesRigidbody.Update();
            DrawSelectedPositions();
        }

        if (_placedCubesRigidbody.velocity.magnitude > 0.1f && !_isLose) 
        {
            Lose();
            StartCoroutine(NonFallingTowerDisable()); 
        }

        if (!_isLose)
        {
            MoveCameraAway(CameraController.CameraMoveSpeed * 3);
            _cameraTransform.localPosition = Vector3.MoveTowards(_cameraTransform.localPosition, new Vector3(_cameraTransform.localPosition.x, _cameraController.StartPosition.y + _cubePosition.y - 1f, _cameraTransform.localPosition.z), CameraController.CameraMoveSpeed * Time.deltaTime);
        }
        else { _cameraTransform.localPosition = Vector3.MoveTowards(_cameraTransform.localPosition, new Vector3(_cameraTransform.localPosition.x, _cameraTransform.localPosition.y, _cameraController.StartPosition.z - _maxHeight - 1f), CameraController.CameraMoveSpeed * Time.deltaTime); }

    }

    public void Back()
    {
        if (!MenuStatement.Instance.IsEnabled)
        {
            if (!GameMenu.Instance.IsEnabled)
            {
                _exitMenu.SetActive(true);
            }
            else
            {
                if (!IsPause && GameMenu.Instance.IsEnabled)
                    Pause();
                else
                    Resume();
            }
        }
    }

    // IGame
    public void Lose()
    {
        if (!_isLose)
        {
            Destroy(_selectedPlace.gameObject);
            StopCoroutine(_showCubePlaceCoroutine);
            _isLose = true;
        }
    }

    private void LaunchGameMenu()
    {
        _mainMenu.SetActive(false);
        _gameMenu.SetActive(true);
        _backgroundMusic.Play();
        IsStarted = true;
    }

    private void LaunchMainMenu()
    {
        _mainMenu.SetActive(true);
        _gameMenu.SetActive(false);
    }

    private void MoveCameraAway(float speed)
    {
        if (_maxWidth % 3 == 0 && _maxWidth != 0 && _camera.transform.localPosition != _moveAwayCameraPosition && _camera.transform.localPosition.z != _moveAwayCameraPosition.z)
        {
            _camera.transform.localPosition = Vector3.MoveTowards(_camera.transform.localPosition, new Vector3(_moveAwayCameraPosition.x, _moveAwayCameraPosition.y, _moveAwayCameraPosition.z), speed * Time.deltaTime);
            _prevMaxWidth = _maxWidth;
        }
        if (_camera.transform.localPosition.z == _moveAwayCameraPosition.z && _prevMaxWidth != _maxWidth)
            _moveAwayCameraPosition = new Vector3(_moveAwayCameraPosition.x, _moveAwayCameraPosition.y, _moveAwayCameraPosition.z - _maxWidth * 1.5f);
    }

    private void CreateNewCube()
    {
        _currentBlock.Sound.PlayOneShot(_audioClip);
        GameObject newCube = Instantiate(_cubeToPlace, _selectedPlace.transform.position, Quaternion.identity);
        newCube.transform.SetParent(_placedCubes);
    }

    private IEnumerator ShowCubePlace()
    {
        while (true)
        {
            DrawSelectedPositions();
            yield return new WaitForSeconds(Block.BlockChangePlaceSpeed);
        }
    }

    public void Reload() => GameMenu.Instance.IsEnabled = true;

    public void Exit() => GameMenu.Instance.IsEnabled = false;

    public void Pause()
    {
        Time.timeScale = 0f;
        _pauseMenu.SetActive(true);
        AudioListener.pause = true;
        IsPause = true;
    }

    public void Resume()
    {
        Time.timeScale = 1f;
        AudioListener.pause = false;
        _pauseMenu.SetActive(false);
        IsPause = false;
    }

    private IEnumerator NonFallingTowerDisable()
    {
        yield return new WaitForSeconds(3.5f);
        if (_placedCubesRigidbody.velocity.magnitude <= 0.5f) { Destroy(); }
    }

    public void Destroy()
    {
        if (!_explosiveCubes.IsDestroyed)
        {
            _explosiveCubes.Destroy();
            _replayMenu.SetActive(true);
            Lose();
        }
    }

    private void DrawSelectedPositions()
    {
        List<Vector3> accessPositions = new List<Vector3>();

        foreach (var possibleVector in _possibleVectors)
        {
            bool isActive = true;
            Vector3 position = _cubePosition + possibleVector;
            foreach (var forbiddenPosition in _forbiddenPositions)
            {
                if (position == forbiddenPosition || position == _selectedPlace.position || position.y <= 0f)
                {
                    isActive = false;
                    break;
                }
            }

            if (isActive)
                accessPositions.Add(position);
        }

        if (accessPositions.Count <= 0) { Destroy(); }

        if (accessPositions.Count != 0) { _selectedPlace.position = accessPositions[UnityEngine.Random.Range(0, accessPositions.Count)]; }
        else 
        {
            _isLose = true;
            _replayMenu.SetActive(true);
        }
    }

    private bool IsPositionAccess(List<Vector3> forbiddenPositions, Vector3 currentPosition)
    {
        foreach (var forbiddenPosition in forbiddenPositions)
        {
            if (currentPosition == forbiddenPosition)
                return false;
        }

        return true;
    }
    
}
