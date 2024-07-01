using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private int _order = 10;

    private Stack<UI_Popup> _popupStack = new Stack<UI_Popup>();
    private UI_Scene _mainUI = null;
    public UI_Scene MainUI => _mainUI;

    private Camera _uiCamera = null;
    public Camera UiCamera
    {
        get
        {
            if (_uiCamera == null)
            {
                UniversalAdditionalCameraData cameraData = Camera.main.GetUniversalAdditionalCameraData();
                if (cameraData.cameraStack.Count == 0)
                {
                    GameObject uiCameraObj = new GameObject { name = "@UI_Camera" };
                    _uiCamera = uiCameraObj.GetOrAddComponent<Camera>();

                    UniversalAdditionalCameraData universalCamera = _uiCamera.gameObject.GetOrAddComponent<UniversalAdditionalCameraData>();
                    universalCamera.renderType = CameraRenderType.Overlay;
                    _uiCamera.cullingMask = 1 << LayerMask.NameToLayer("UI");
                    cameraData.cameraStack.Add(_uiCamera);
                }
                else
                {
                    _uiCamera = cameraData.cameraStack[0];
                }
            }

            return _uiCamera;
        }
    }

    public GameObject Root
    {
        get
        {
            GameObject root = GameObject.Find("@UI_Root");
            if (root == null)
                root = new GameObject { name = "@UI_Root" };

            return root;
        }
    }


    public void SetCanvas(GameObject go, bool sort = true)
    {
        Canvas canvas = Util.GetOrAddComponent<Canvas>(go);
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.overrideSorting = true;

        if (sort)
        {
            canvas.sortingOrder = _order;
            _order++;
        }
        else
        {
            canvas.sortingOrder = 0;
        }
    }

    public T ShowSceneUI<T>(string name = null) where T : UI_Scene
    {
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;

        GameObject go = Managers.Resource.Instantiate(name);
        T sceneUI = Util.GetOrAddComponent<T>(go);
        _mainUI = sceneUI;

        go.transform.SetParent(Root.transform);

        go.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceCamera;
        go.GetComponent<Canvas>().worldCamera = UiCamera;

        return sceneUI;
    }

    public T ShowPopupUI<T>(string name = null) where T : UI_Popup
    {
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;

        GameObject go = Managers.Resource.Instantiate(name);

        go.GetOrAddComponent<GraphicRaycaster>();
        CanvasScaler scaler = go.GetOrAddComponent<CanvasScaler>();
        CanvasScaler sceneScaler = _mainUI.GetComponent<CanvasScaler>();
        scaler.uiScaleMode = sceneScaler.uiScaleMode;
        scaler.referenceResolution = sceneScaler.referenceResolution;
        scaler.screenMatchMode = sceneScaler.screenMatchMode;
        scaler.matchWidthOrHeight = sceneScaler.matchWidthOrHeight;

        T popup = Util.GetOrAddComponent<T>(go);
        _popupStack.Push(popup);

        go.GetOrAddComponent<Canvas>().renderMode = RenderMode.ScreenSpaceCamera;
        go.GetOrAddComponent<Canvas>().worldCamera = UiCamera;

        go.transform.SetParent(Root.transform);
        popup.Init();

        return popup;
    }

    public void ClosePopupUI(UI_Popup popup)
    {
        if (_popupStack.Count == 0)
            return;

        if (_popupStack.Peek() != popup)
        {
            Managers.Resource.Destroy(popup.gameObject);
            popup = null;
            _order--;

            return;
        }

        ClosePopupUI();
    }

    public void ClosePopupUI()
    {
        if (_popupStack.Count == 0)
            return;

        UI_Popup popup = _popupStack.Pop();
        Managers.Resource.Destroy(popup.gameObject);
        popup = null;
        _order--;
    }

    public void CloseAllPopupUI()
    {
        while (_popupStack.Count > 0)
        {
            ClosePopupUI();
        }
    }

    public void Clear()
    {
        CloseAllPopupUI();
        _mainUI = null;
    }
}
