using UnityEngine;
using TMPro;
using OscJack;

public class SenderController : MonoBehaviour
{
    public OscConnectionDynamic oscConnection;
    public TMP_InputField hostInputField;
    public TMP_InputField portInputField;

    void Start()
    {
        hostInputField.onValueChanged.AddListener((value) => oscConnection.host = value);
        portInputField.onValueChanged.AddListener((value) => oscConnection.port = value);
    }
}