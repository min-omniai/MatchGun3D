using UnityEngine;

public class MatchGun : MonoBehaviour
{
    [SerializeField]
    private Define.GunType _gunType;
    public Define.GunType GunType => _gunType;

    private Collider _collider = null;
    private Rigidbody _rigidbody = null;
    private Poolable _poolable = null;


    public void Init(Define.GunType gunType)
    {
        _gunType = gunType;

        if (_collider == null) _collider = GetComponent<Collider>();
        if (_rigidbody == null) _rigidbody = GetComponent<Rigidbody>();
        if (_poolable == null) _poolable = GetComponent<Poolable>();

        // 풀에서 재사용된 총은 수집 때 끈 충돌과 물리를 되살린다
        _collider.enabled = true;
        _rigidbody.isKinematic = false;
    }

    public void Selected(Transform selectedTf)
    {
        _collider.enabled = false;
        _rigidbody.isKinematic = true;

        StartCoroutine(SelectedRoutine(selectedTf));
    }

    private System.Collections.IEnumerator SelectedRoutine(Transform selectedTf)
    {
        transform.localScale = MatchPlayManager.MatchScale;
        transform.localRotation = selectedTf.localRotation;

        while ((transform.position - selectedTf.position).sqrMagnitude > .1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, selectedTf.position, Time.deltaTime * 100.0f);

            yield return null;
        }
    }

    public void Match(Transform matchTf)
    {
        StartCoroutine(MatchRoutine(matchTf));
    }

    private System.Collections.IEnumerator MatchRoutine(Transform matchTf)
    {
        transform.localRotation = matchTf.localRotation;

        while ((transform.position - matchTf.position).sqrMagnitude > .1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, matchTf.position, Time.deltaTime * 25.0f);

            yield return null;
        }

        Managers.Pool.Push(_poolable);
    }

    public void Mismatch(Transform misMatchTf)
    {
        StartCoroutine(MismatchRoutine(misMatchTf));
    }

    private System.Collections.IEnumerator MismatchRoutine(Transform misMatchTf)
    {
        while ((transform.position - misMatchTf.position).sqrMagnitude > .1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, misMatchTf.position, Time.deltaTime * 50.0f);

            yield return null;
        }

        Clear();
    }


    private void Clear()
    {
        transform.localScale = MatchPlayManager.OriginScale;

        _collider.enabled = true;
        _rigidbody.isKinematic = false;
    }
}
