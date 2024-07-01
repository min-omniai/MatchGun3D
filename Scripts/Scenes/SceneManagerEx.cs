using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerEx
{
    public BaseScene CurrentScene { get { return GameObject.FindObjectOfType<BaseScene>(); } }
    public Animator SceneTransitionAnimator { private get; set; }
    private AsyncOperation _asyncLoadingOperation;

    private int _fadeInHash;
    private int _fadeOutHash;

    private bool _isLoadingScene;


    public void Init()
    {
        _fadeInHash = Animator.StringToHash("FadeIn");
        _fadeOutHash = Animator.StringToHash("FadeOut");
    }

    public async void LoadScene(Define.SceneName type)
    {
        if (_isLoadingScene)
            return;

        SceneTransitionAnimator.Play(_fadeOutHash);

        await Task.Delay(500);

        if (!Application.isPlaying)
            return;

        Managers.Clear();

        RuntimeAnimatorController animatorController = Managers.Resource.Load<RuntimeAnimatorController>("SceneTransitionAnimator");
        SceneTransitionAnimator.runtimeAnimatorController = animatorController;

        _asyncLoadingOperation = SceneManager.LoadSceneAsync(GetSceneName(type), LoadSceneMode.Single);
        _asyncLoadingOperation.allowSceneActivation = false;

        while (!_asyncLoadingOperation.isDone)
        {
            if (_asyncLoadingOperation.progress >= .9f)
                break;

            await Task.Delay(1);
        }

        _asyncLoadingOperation.allowSceneActivation = true;
        SceneTransitionAnimator.Play(_fadeInHash);
        _isLoadingScene = false;
    }

    public async void FadeOut(Define.SceneName type)
    {
        if (_isLoadingScene)
            return;

        _isLoadingScene = true;

        SceneTransitionAnimator.Play(_fadeOutHash);

        await Task.Delay(500);

        if (!Application.isPlaying)
            return;

        Managers.Clear();

        RuntimeAnimatorController animatorController = Managers.Resource.Load<RuntimeAnimatorController>("SceneTrasitionAnimator");
        SceneTransitionAnimator.runtimeAnimatorController = animatorController;
        SceneManager.LoadScene(GetSceneName(type), LoadSceneMode.Single);
        _isLoadingScene = false;
    }

    public void FadeIn()
    {
        SceneTransitionAnimator.Play(_fadeInHash);
    }

    public async void LoadSceneInstance(Define.SceneName type)
    {
        if (_isLoadingScene)
            return;

        _isLoadingScene = true;

        _asyncLoadingOperation = SceneManager.LoadSceneAsync(GetSceneName(type), LoadSceneMode.Single);
        _asyncLoadingOperation.allowSceneActivation = false;

        while (!_asyncLoadingOperation.isDone)
        {
            if (_asyncLoadingOperation.progress >= .9f)
                break;

            await Task.Delay(1);

            _asyncLoadingOperation.allowSceneActivation = true;
            _isLoadingScene = false;
        }
    }

    private string GetSceneName(Define.SceneName type)
    {
        string name = System.Enum.GetName(typeof(Define.SceneName), type);

        return name;
    }

    public void Clear()
    {
        CurrentScene.Clear();
    }
}
