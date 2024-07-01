public class Define
{
    public static int SpawnCount = 16;

    public enum GameState : byte
    {
        Ready,
        PlayMatch,
        EndMatch,
        PlayRun,
        End
    }

    public enum SceneName : byte
    {
        Unknown,
        Loading,
        Game,
    }

    public enum SoundType : byte
    {
        Bgm,
        Effect,
        MaxCount,
    }

    public enum UIEvent : byte
    {
        Click,
        Drag,
        Up,
        Down,
    }

    public enum Language : byte
    {
        Kor,
        Eng,
    }
    public enum ObjectType : byte
    {
        NotAssigned,
        GameObject,
        TextMesh,
        Image,
        Button,
        Text,
    }

    public static float Bound = 0.5f;
    public static float RouletteGunImageSpeed = 200f;

    public enum GunName : byte
    {
        HandGun,
        ShotGun,
        AutoRifle,

        MaxCount
    }

    public enum GunType : byte
    {
        HandGun,
        Shotgun,
        AutoRifle,

        MaxCount
    }
    public enum GunSprite : byte
    {
        Count_HandGun00,
        Count_AutoRifle00,
        Count_Shotgun00,

        MaxCount
    }
    public enum GunRunType : byte
    {
        RunHandGun00,
        RunAutoRifle00,
        RunShotgun00,

        MaxCount
    }
    public enum BulletType : byte
    {
        BulletHandgun00,
        BulletAutoRifle00,
        BulletShotgun00,

        MaxCount
    }

    public enum ObstacleType : byte
    {
        Piller,
        EndBlockRunPart,

        MaxCount
    }

    public enum PillarType : byte
    {
        Damage,
        FireRate,
        MaxCount
    }

    public enum SprayType : byte
    {
        Single,
        Triple,
        MaxCount
    }
}
