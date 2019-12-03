// GENERATED AUTOMATICALLY FROM 'Assets/Controls/Controls.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @Controls : IInputActionCollection, IDisposable
{
    private InputActionAsset asset;
    public @Controls()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""Controls"",
    ""maps"": [
        {
            ""name"": ""PlayerControls"",
            ""id"": ""19d5d016-8713-4cd5-ba8e-767f73bb7513"",
            ""actions"": [
                {
                    ""name"": ""Click"",
                    ""type"": ""Button"",
                    ""id"": ""6292ecd5-6dea-4f80-9a09-a998d8b10408"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": ""Press""
                },
                {
                    ""name"": ""RotateCamera"",
                    ""type"": ""PassThrough"",
                    ""id"": ""281c1cd8-61ea-445c-a2c1-f570edef5e50"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Zoom"",
                    ""type"": ""PassThrough"",
                    ""id"": ""ada413de-6adf-4130-9b2b-d1c01230395b"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Pause"",
                    ""type"": ""Button"",
                    ""id"": ""274c2f09-88a3-45a8-874c-e5c74128fe90"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""f202e9f7-4869-4da8-95b4-355497fc0fdd"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Click"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""LeftClickAndDeltaX"",
                    ""id"": ""fade2be7-c2bd-42af-b93a-4e424fa03b0c"",
                    ""path"": ""ButtonWithOneModifier"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""RotateCamera"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""Modifier"",
                    ""id"": ""a51a8544-93fc-47ea-b3c2-45875bbaf40d"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""RotateCamera"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Button"",
                    ""id"": ""606dcdc7-cf61-4f1f-b09b-549c1934a75b"",
                    ""path"": ""<Mouse>/delta/x"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""RotateCamera"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""a420c22f-acbb-4c2d-a681-c088e0618d9b"",
                    ""path"": ""<Mouse>/scroll/y"",
                    ""interactions"": """",
                    ""processors"": ""Clamp(min=-1,max=1),Invert"",
                    ""groups"": """",
                    ""action"": ""Zoom"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""429f07fa-b78a-4211-988f-2e90bafee669"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Pause"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""PauseMenuControls"",
            ""id"": ""a6a41147-49bf-4fb5-8546-221dd7b1942a"",
            ""actions"": [
                {
                    ""name"": ""ExitPause"",
                    ""type"": ""Button"",
                    ""id"": ""d8cbfb34-b255-4070-a1ae-bcc91f82f5eb"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""2656ce76-5060-401d-8a65-09dd56067e2b"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ExitPause"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // PlayerControls
        m_PlayerControls = asset.FindActionMap("PlayerControls", throwIfNotFound: true);
        m_PlayerControls_Click = m_PlayerControls.FindAction("Click", throwIfNotFound: true);
        m_PlayerControls_RotateCamera = m_PlayerControls.FindAction("RotateCamera", throwIfNotFound: true);
        m_PlayerControls_Zoom = m_PlayerControls.FindAction("Zoom", throwIfNotFound: true);
        m_PlayerControls_Pause = m_PlayerControls.FindAction("Pause", throwIfNotFound: true);
        // PauseMenuControls
        m_PauseMenuControls = asset.FindActionMap("PauseMenuControls", throwIfNotFound: true);
        m_PauseMenuControls_ExitPause = m_PauseMenuControls.FindAction("ExitPause", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }

    // PlayerControls
    private readonly InputActionMap m_PlayerControls;
    private IPlayerControlsActions m_PlayerControlsActionsCallbackInterface;
    private readonly InputAction m_PlayerControls_Click;
    private readonly InputAction m_PlayerControls_RotateCamera;
    private readonly InputAction m_PlayerControls_Zoom;
    private readonly InputAction m_PlayerControls_Pause;
    public struct PlayerControlsActions
    {
        private @Controls m_Wrapper;
        public PlayerControlsActions(@Controls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Click => m_Wrapper.m_PlayerControls_Click;
        public InputAction @RotateCamera => m_Wrapper.m_PlayerControls_RotateCamera;
        public InputAction @Zoom => m_Wrapper.m_PlayerControls_Zoom;
        public InputAction @Pause => m_Wrapper.m_PlayerControls_Pause;
        public InputActionMap Get() { return m_Wrapper.m_PlayerControls; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayerControlsActions set) { return set.Get(); }
        public void SetCallbacks(IPlayerControlsActions instance)
        {
            if (m_Wrapper.m_PlayerControlsActionsCallbackInterface != null)
            {
                @Click.started -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnClick;
                @Click.performed -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnClick;
                @Click.canceled -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnClick;
                @RotateCamera.started -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnRotateCamera;
                @RotateCamera.performed -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnRotateCamera;
                @RotateCamera.canceled -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnRotateCamera;
                @Zoom.started -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnZoom;
                @Zoom.performed -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnZoom;
                @Zoom.canceled -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnZoom;
                @Pause.started -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnPause;
                @Pause.performed -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnPause;
                @Pause.canceled -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnPause;
            }
            m_Wrapper.m_PlayerControlsActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Click.started += instance.OnClick;
                @Click.performed += instance.OnClick;
                @Click.canceled += instance.OnClick;
                @RotateCamera.started += instance.OnRotateCamera;
                @RotateCamera.performed += instance.OnRotateCamera;
                @RotateCamera.canceled += instance.OnRotateCamera;
                @Zoom.started += instance.OnZoom;
                @Zoom.performed += instance.OnZoom;
                @Zoom.canceled += instance.OnZoom;
                @Pause.started += instance.OnPause;
                @Pause.performed += instance.OnPause;
                @Pause.canceled += instance.OnPause;
            }
        }
    }
    public PlayerControlsActions @PlayerControls => new PlayerControlsActions(this);

    // PauseMenuControls
    private readonly InputActionMap m_PauseMenuControls;
    private IPauseMenuControlsActions m_PauseMenuControlsActionsCallbackInterface;
    private readonly InputAction m_PauseMenuControls_ExitPause;
    public struct PauseMenuControlsActions
    {
        private @Controls m_Wrapper;
        public PauseMenuControlsActions(@Controls wrapper) { m_Wrapper = wrapper; }
        public InputAction @ExitPause => m_Wrapper.m_PauseMenuControls_ExitPause;
        public InputActionMap Get() { return m_Wrapper.m_PauseMenuControls; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PauseMenuControlsActions set) { return set.Get(); }
        public void SetCallbacks(IPauseMenuControlsActions instance)
        {
            if (m_Wrapper.m_PauseMenuControlsActionsCallbackInterface != null)
            {
                @ExitPause.started -= m_Wrapper.m_PauseMenuControlsActionsCallbackInterface.OnExitPause;
                @ExitPause.performed -= m_Wrapper.m_PauseMenuControlsActionsCallbackInterface.OnExitPause;
                @ExitPause.canceled -= m_Wrapper.m_PauseMenuControlsActionsCallbackInterface.OnExitPause;
            }
            m_Wrapper.m_PauseMenuControlsActionsCallbackInterface = instance;
            if (instance != null)
            {
                @ExitPause.started += instance.OnExitPause;
                @ExitPause.performed += instance.OnExitPause;
                @ExitPause.canceled += instance.OnExitPause;
            }
        }
    }
    public PauseMenuControlsActions @PauseMenuControls => new PauseMenuControlsActions(this);
    public interface IPlayerControlsActions
    {
        void OnClick(InputAction.CallbackContext context);
        void OnRotateCamera(InputAction.CallbackContext context);
        void OnZoom(InputAction.CallbackContext context);
        void OnPause(InputAction.CallbackContext context);
    }
    public interface IPauseMenuControlsActions
    {
        void OnExitPause(InputAction.CallbackContext context);
    }
}
