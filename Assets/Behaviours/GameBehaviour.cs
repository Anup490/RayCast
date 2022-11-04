using UnityEngine;

public class GameBehaviour : MonoBehaviour
{
    Camera cam;
    AssetBehaviour selectedAsset;
    Device device;
    UIManager uiManager;
    RayCastManager rayCastManager;
    GameMode mode = GameMode.DRAG;

    public enum GameMode
    {
        DRAG, ROTATE
    }

    public void SetGameMode(GameMode gameMode)
    {
        mode = gameMode;
    }

    public AssetBehaviour GetSelectedAsset()
    {
        return selectedAsset;
    }

    public void OnSliderChange(float yaw, float sliderVal)
    {
        if (selectedAsset != null)
        {
            selectedAsset.transform.Rotate(0, yaw, 0);
            selectedAsset.yaw = sliderVal;
        }
    }

    public void OnMouseClick(Vector3 position)
    {
        selectedAsset = rayCastManager.DetectAsset(position, mode == GameMode.DRAG);
        if (selectedAsset != null)
        {
            uiManager.ShowLabelAt(selectedAsset.gameObject.name);
            uiManager.SetSliderValue(selectedAsset.yaw);
        }
        else if (mode == GameMode.DRAG)
            uiManager.ShowHideLabel(false); 
    }

    public void OnMouseDrag(Vector3 position)
    {
        if (mode == GameMode.DRAG && selectedAsset != null)
            selectedAsset.Displace(rayCastManager.GetTargetPosition(position, selectedAsset.transform.position));      
    }

    public void OnMouseRelease()
    {
        if (selectedAsset != null)
            selectedAsset.OnUnselect();            
    }

    void Start()
    {
        cam = GetComponent<Camera>();
        Init();
    }

    void Update()
    {
        device.OnUpdate();
    }

    void Init()
    {      
        device = (SystemInfo.deviceType == DeviceType.Handheld) ? new Android(this) : new Windows(this);
        uiManager = new UIManager(gameObject);
        rayCastManager = new RayCastManager(cam);
    }
}