using UnityEngine;

public class GameManager : MonoBehaviour
{
    private Define.GameState _currentGameState = Define.GameState.Ready;

    public bool GameStateReady => _currentGameState == Define.GameState.Ready;
    public bool GameStateMatchPlay => _currentGameState == Define.GameState.PlayMatch;
    public bool GameStateTimeOver => _currentGameState == Define.GameState.EndMatch;
    public bool GameStateRunPlay => _currentGameState == Define.GameState.PlayRun;
    public bool GameStateEnd => _currentGameState == Define.GameState.End;

    ///<summary>러닝 파트에서 설정하는 조이스틱. 입력 이벤트 연결에 사용</summary>
    public JoystickController ActiveJoystick { get; set; }
    public void SetDownAction(System.Action action)
    {
        ActiveJoystick?.AddDownEvent(action);
    }
    public void SetUpAction(System.Action action)
    {
        ActiveJoystick?.AddUpEvent(action);
    }
    public void SetMoveAction(System.Action<Vector2> action)
    {
        ActiveJoystick?.AddMoveEvent(action);
    }

    private UI_GameScene _uiGameScene = null;
    private CameraManager _cameraManager = null;
    private GunSpawnManager _gunSpawnManager = null;
    private MatchPlayManager _matchPlayManager = null;

    public UI_GameScene GameSceneUI { get { CheckNull(); return _uiGameScene; } }
    public CameraManager CameraManager { get { CheckNull(); return _cameraManager; } }
    public GunSpawnManager GunSpawnManager { get { CheckNull(); return _gunSpawnManager; } }
    public MatchPlayManager MatchPlayManager { get { CheckNull(); return _matchPlayManager; } }

    public string IapName = "";
    private bool _stageClear = false;
    public bool StageClear
    {
        get => _stageClear;
    }


    public void Init()
    {
        GameReady();
    }

    private void CheckNull()
    {
        if (_uiGameScene == null) _uiGameScene = FindObjectOfType<UI_GameScene>();
        if (_cameraManager == null) _cameraManager = FindObjectOfType<CameraManager>();
        if (_gunSpawnManager == null) _gunSpawnManager = FindObjectOfType<GunSpawnManager>();
        if (_matchPlayManager == null) _matchPlayManager = FindObjectOfType<MatchPlayManager>();
    }

    private const int GunPoolCountPerType = 8;

    public void GameReady()
    {
        CheckNull();

        Managers.Resource.Instantiate("MatchMap");

        for (int i = 0; i < (int)Define.GunType.MaxCount; i++)
        {
            GameObject original = Managers.Resource.Load<GameObject>(((Define.GunType)i).ToString());
            Managers.Pool.CreatePool(original, GunPoolCountPerType);
        }

        CameraManager.Init();
        MatchPlayManager.Init();

        _currentGameState = Define.GameState.Ready;
    }

    private System.Action _playMatchAction;
    public void GameStartMatch(Define.GameState state)
    {
        _currentGameState = state;

        _playMatchAction -= CameraManager.GameStartMatch;
        _playMatchAction += CameraManager.GameStartMatch;
        _playMatchAction -= GameSceneUI.GameStartMatch;
        _playMatchAction += GameSceneUI.GameStartMatch;

        _playMatchAction?.Invoke();
    }


    public void Clear()
    {
        if (ActiveJoystick != null)
        {
            ActiveJoystick.DownAction = null;
            ActiveJoystick.UpAction = null;
            ActiveJoystick.JoystickMoveAction = null;
        }
    }
}
