using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    #region Pool
    class Pool
    {
        public GameObject Original { get; private set; }
        public Transform Root { get; set; }

        private Stack<Poolable> _poolStack = new Stack<Poolable>();

        public void Init(GameObject original, int count = 5)
        {
            Original = original;
            Root = new GameObject().transform;
            Root.name = $"{original.name}_Root";

            for (int i = 0; i < count; i++)
                Push(Create());
        }

        private Poolable Create()
        {
            GameObject go = Object.Instantiate<GameObject>(Original);
            go.name = Original.name;

            return go.GetOrAddComponent<Poolable>();
        }

        public void Push(Poolable poolable)
        {
            if (poolable == null)
                return;

            poolable.transform.SetParent(Root);
            poolable.gameObject.SetActive(false);
            poolable.IsUsing = false;

            _poolStack.Push(poolable);
        }

        public Poolable Pop(Transform parent)
        {
            Poolable poolable;

            if (_poolStack.Count > 0)
                poolable = _poolStack.Pop();
            else
                poolable = Create();

            poolable.gameObject.SetActive(true);

            if (parent == null)
                poolable.transform.SetParent(Managers.Scene.CurrentScene.transform);

            poolable.transform.SetParent(parent);
            poolable.IsUsing = true;

            return poolable;
        }
    }
    #endregion


    private Dictionary<string, Pool> _pools = new Dictionary<string, Pool>();
    private Transform _root = null;

    public void Init()
    {
        _root = new GameObject { name = "@PoolRoot" }.transform;
        Object.DontDestroyOnLoad(_root);
    }

    public void CreatePool(GameObject original, int count = 5)
    {
        if (_pools.ContainsKey(original.name))
            return;

        Pool pool = new Pool();
        pool.Init(original, count);
        pool.Root.SetParent(_root);

        _pools.Add(original.name, pool);
    }

    public void Push(Poolable poolable)
    {
        string name = poolable.gameObject.name;

        if (!_pools.ContainsKey(name))
        {
            GameObject.Destroy(poolable.gameObject);

            return;
        }

        _pools[name].Push(poolable);
    }

    public Poolable Pop(GameObject original, Transform parent = null)
    {
        if (!_pools.ContainsKey(original.name))
            CreatePool(original);

        return _pools[original.name].Pop(parent);
    }

    public GameObject GetOriginal(string name)
    {
        if (!_pools.ContainsKey(name))
            return null;

        return _pools[name].Original;
    }

    public void Clear()
    {
        if (_root == null)
            return;

        foreach (Transform child in _root)
            GameObject.Destroy(child.gameObject);

        _pools.Clear();
    }
}
