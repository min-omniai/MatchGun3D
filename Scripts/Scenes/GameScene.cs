public class GameScene : BaseScene
{
    protected override void Init()
    {
        base.Init();

        SceneType = Define.SceneName.Game;
        Managers.UI.ShowSceneUI<UI_GameScene>();
        Managers.GameInit();
    }

    public override void Clear()
    {
    }
}
