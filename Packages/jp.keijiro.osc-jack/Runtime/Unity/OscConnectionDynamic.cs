// OSC Jack - Open Sound Control plugin for Unity
// https://github.com/keijiro/OscJack

// OSC Jack - Open Sound Control plugin for Unity
// https://github.com/keijiro/OscJack

using UnityEngine;
using TMPro;

namespace OscJack
{
    public enum OscConnectionDynamicType { Udp }

    [CreateAssetMenu(fileName = "OscConnection",
                     menuName = "ScriptableObjects/OSC Jack/Connection")]
    public sealed class OscConnectionDynamic : ScriptableObject
    {
        public OscConnectionDynamicType type = OscConnectionDynamicType.Udp;
        public string host = "0.0.0.0";
        public string port = "0000";

        public void HandleInput(TMP_InputField hostInputField, TMP_InputField portInputField)
        {
            hostInputField.onValueChanged.AddListener(UpdateHost);
            portInputField.onValueChanged.AddListener(UpdatePort);
        }

        private void UpdateHost(string newHost)
        {
            host = newHost;
        }

        private void UpdatePort(string newPort)
        {
            port = newPort;
        }
    }
}
