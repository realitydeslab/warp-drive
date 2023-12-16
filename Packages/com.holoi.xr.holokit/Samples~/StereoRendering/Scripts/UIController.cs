using UnityEngine;
using TMPro;

namespace HoloKit.Samples.StereoscopicRendering
{
    public class UIController : MonoBehaviour
    {
        [SerializeField] private TMP_Text _renderModeText;

        private void Awake()
        {
            HoloKitCamera.OnHoloKitRenderModeChanged += OnHoloKitRenderModeChanged;
        }

        private void OnDestroy()
        {
            HoloKitCamera.OnHoloKitRenderModeChanged -= OnHoloKitRenderModeChanged;
        }

        public void ToggleRenderMode()
        {
            if (HoloKitCamera.Instance.RenderMode == HoloKitRenderMode.Stereo)
            {
                HoloKitCamera.Instance.RenderMode = HoloKitRenderMode.Mono;
            }
            else
            {
                HoloKitCamera.Instance.RenderMode = HoloKitRenderMode.Stereo;

                // Skip NFC scanning
                HoloKitCamera.Instance.OpenStereoWithoutNFC("SomethingForNothing");
            }
        }

        private void OnHoloKitRenderModeChanged(HoloKitRenderMode renderMode)
        {
            if (renderMode == HoloKitRenderMode.Stereo)
            {
                _renderModeText.text = "Mono";
            }
            else
            {
                _renderModeText.text = "Stereo";
            }
        }

    }
}