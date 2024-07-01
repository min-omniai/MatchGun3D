using UnityEngine;

public class Managers : MonoBehaviour
{
    private static Managers s_instance = null;
    public static Managers Instance { get { Init(); return s_instance; } }


    [HideInInspector]
    private GameManager _game = null;

    private InputManager _input = new InputManager();
    private ResourceManager _resource = new ResourceManager();
    private UIManager _ui = new UIManager();
    private SceneManagerEx _scene = new SceneManagerEx();
    private SoundManager _sound = new SoundManager();
    private PoolManager _pool = new PoolManager();
    private DataManager _data = new DataManager();

    public static GameManager Game { get { return Instance._game; } }
    public static InputManager InputManager { get { return Instance._input; } }
    public static ResourceManager Resource { get { return Instance._resource; } }
    public static UIManager UI { get { return Instance._ui; } }
    public static SceneManagerEx Scene { get { return Instance._scene; } }
    public static SoundManager Sound { get { return Instance._sound; } }
    public static PoolManager Pool { get { return Instance._pool; } }
    public static DataManager Data { get { return Instance._data; } }



    private void Start()
    {
        // Init();
    }

    private void Update()
    {
        _input?.OnUpdate();
    }


    private static void Init()
    {
        if (s_instance == null)
        {
            // === Default Setting === //
            Application.targetFrameRate = 60;
            QualitySettings.vSyncCount = 0;
            Input.multiTouchEnabled = false;
            // === Default Setting === //

            GameObject managers = new GameObject { name = "@Managers" };
            s_instance = managers.GetOrAddComponent<Managers>();
            DontDestroyOnLoad(managers);

            GameObject sceneTransition = Managers.Resource.Instantiate("SceneTransition");
            sceneTransition.transform.SetParent(s_instance.transform);
            Scene.SceneTransitionAnimator = sceneTransition.GetComponent<Animator>();

            s_instance._sound.Init();
            s_instance._scene.Init();
            s_instance._pool.Init();
            s_instance._data.Init();

            s_instance._game = managers.GetOrAddComponent<GameManager>();

            DontDestroyOnLoad(managers);
        }
    }

    public static void GameInit()
    {
        s_instance._game.Init();
    }

    public static void Clear()
    {
        InputManager.Clear();
        Sound.Clear();
        Scene.Clear();
        UI.Clear();
        Pool.Clear();
    }
}
