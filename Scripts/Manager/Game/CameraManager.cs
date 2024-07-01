using System.Collections;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    private readonly Vector3 _cameraStartPosition = new Vector3(0, 16.5f, -.6f);
    private readonly Vector3 _matchOffset = new Vector3(0, 14.6f, -3.4f);
    private Vector3 _matchPlayPosition = Vector3.zero;
    private float _moveSpeed = 20.0f;
    private float _distanceThreshold = 0.001f;

    private Camera _camera = null;
    private Ray _ray;
    private RaycastHit _hit;

    private void Update()
    {
        if (Managers.Game.GameStateMatchPlay)
        {
            if (Input.GetMouseButtonDown(0))
            {
                _ray = _camera.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(_ray, out _hit, 100.0f, 1 << 6))
                {
                    GameObject hitObj = _hit.transform.gameObject;

                    Managers.Game.MatchPlayManager.Selected(hitObj);
                }
            }
        }
    }


    public void Init()
    {
        _camera = Camera.main;

        transform.localPosition = _cameraStartPosition;
    }

    public void GameStartMatch()
    {
        _matchPlayPosition = transform.position + _matchOffset;

        StartCoroutine(GameStartMatchRoutine());
    }

    private IEnumerator GameStartMatchRoutine()
    {
        float distanceThresholdSquared = _distanceThreshold * _distanceThreshold;
        float step = Time.deltaTime * _moveSpeed;

        while (Vector3.SqrMagnitude(transform.position - _matchPlayPosition) > distanceThresholdSquared)
        {
            transform.position = Vector3.MoveTowards(transform.position, _matchPlayPosition, step);

            yield return null;
        }
    }
}
