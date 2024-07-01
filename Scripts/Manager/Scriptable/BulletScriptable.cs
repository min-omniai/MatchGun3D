using UnityEngine;

[CreateAssetMenu(fileName = "Bullet Data", menuName = "Scriptable Object/Bullet Data")]
public class BulletScriptable : ScriptableObject
{
    [SerializeField] private int _handGunDamage = 1;
    public int HandGunDamage { get { return _handGunDamage; } }
    [SerializeField] private int _shotGunDamage = 4;
    public int ShotGunDamage { get { return _shotGunDamage; } }
    [SerializeField] private int _autoRifleDamage = 1;
    public int AutoRifleDamage { get { return _autoRifleDamage; } }
    [SerializeField] private int _sniperDamage = 10;
    public int SniperDamage { get { return _sniperDamage; } }
    [SerializeField] private int _bazookaDamage = 30;
    public int BazookaDamage { get { return _bazookaDamage; } }

    [SerializeField] private float _handGunFireRate = .5f;
    public float HandGunFireRate { get { return _handGunFireRate; } }
    [SerializeField] private float _shotGunFireRate = .75f;
    public float ShotGunFireRate { get { return _shotGunFireRate; } }
    [SerializeField] private float _autoRifleFireRate = .25f;
    public float AutoRifleFireRate { get { return _autoRifleFireRate; } }
    [SerializeField] private float _sniperFireRate = 1.25f;
    public float SniperFireRate { get { return _sniperFireRate; } }
    [SerializeField] private float _bazookaFireRate = 1.5f;
    public float BazookaFireRate { get { return _bazookaFireRate; } }

    [SerializeField] private Vector3 _handGunBulletSize = new Vector3(6.5f, 6.5f, 6.5f);
    public Vector3 HandGunBulletSize { get { return _handGunBulletSize; } }
    [SerializeField] private Vector3 _shotGunBulletSize = new Vector3(20f, 20f, 20f);
    public Vector3 ShotGunBulletSize { get { return _shotGunBulletSize; } }
    [SerializeField] private Vector3 _autoRifleBulletSize = new Vector3(5.5f, 5f, 5.5f);
    public Vector3 AutoRifleBulletSize { get { return _autoRifleBulletSize; } }
    [SerializeField] private Vector3 _sniperBulletSize = new Vector3(4f, 4f, 4f);
    public Vector3 SniperBulletSize { get { return _sniperBulletSize; } }
    [SerializeField] private Vector3 _bazookaBulletSize = new Vector3(2f, 2f, 2f);
    public Vector3 BazookaBulletSize { get { return _bazookaBulletSize; } }
}
