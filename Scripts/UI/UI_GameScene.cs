using UnityEngine;
using UnityEngine.UI;

public class UI_GameScene : UI_Scene
{
    enum Buttons
    {
        Button_GameStart,
    }

    enum GameObjects
    {
        RoulleteBG,
        MatchInfo,
    }

    private RouletteManager _rouletteManager = null;
    public RouletteManager Roulette => _rouletteManager;
    public System.Collections.Generic.List<Define.GunType> GunTypeList => Roulette.GunTypeList;

    public Transform MatchInfoRoot => GetObject(GameObjects.MatchInfo).transform;

    private void Awake()
    {
        Bind<Button>(typeof(Buttons));
        Bind<GameObject>(typeof(GameObjects));

        base.Init();

        LoadData();
        ButtonInit();
    }

    private void LoadData()
    {
        if (_rouletteManager == null) _rouletteManager = Util.FindChild<RouletteManager>(gameObject, "Roullete", true);

        _rouletteManager.Init();
    }

    private void ButtonInit()
    {
        GetButton(Buttons.Button_GameStart).onClick.AddListener(() =>
        {
            Managers.Game.GameStartMatch(Define.GameState.PlayMatch);

            GetButton(Buttons.Button_GameStart).gameObject.SetActive(false);
        });
    }

    public void GameStartMatch()
    {
        Roulette.GameStartMatch();
    }

    public void SetRouletteActive(bool state)
    {
        GetObject(GameObjects.RoulleteBG).SetActive(state);
    }

    public void Clear()
    {
        Roulette.Clear();

        SetRouletteActive(true);
    }
}
