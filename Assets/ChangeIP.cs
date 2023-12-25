using UnityEngine;
using OscJack;
using TMPro;

public class HostInputHandler : MonoBehaviour
{
    public OscConnection oscConnection;
    public TMP_InputField hostInputField;

    void Start()
    {
        hostInputField.onValueChanged.AddListener(SetHost);
    }

    void SetHost(string newHost)
    {
        oscConnection.host = newHost;
    }
}