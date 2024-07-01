using UnityEngine;

public class DataManager
{
    public void Init()
    {
        GetData();
    }

    #region Sound
    private bool _useSound = true;
    public bool UseSound
    {
        get => _useSound;
        set
        {
            _useSound = value;

            Managers.Sound.BgmState(_useSound);
        }
    }

    private bool _useHaptic = true;
    public bool UseHaptic
    {
        get => _useHaptic;
        set
        {
            _useHaptic = value;
        }
    }
    #endregion

    public bool Tutorial
    {
        get => System.Convert.ToBoolean(PlayerPrefs.GetInt("EndTutorial"));
        set => PlayerPrefs.SetInt("EndTutorial", value ? 1 : 0);
    }

    public void SaveData()
    {

    }

    public void GetData()
    {

    }
}
