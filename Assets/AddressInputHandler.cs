using UnityEngine;
using OscJack;
using TMPro;

public class AddressInputHandler : MonoBehaviour
{
    public TMP_InputField addressInputField;
    public string targetOscAddress;
    public GameObject targetObject; // Add this line

    void Start()
    {
        Debug.Log("targetOscAddress: " + targetOscAddress);
        addressInputField.onEndEdit.AddListener(SetAddress); // Change this line
    }

    void SetAddress(string newAddress)
    {
        Debug.Log("SetAddress called with: " + newAddress);

        // Use GetComponent instead of GetComponents
        OscPropertySender propertySender = targetObject.GetComponent<OscPropertySender>();
        if (propertySender != null && propertySender.enabled && propertySender.OscAddress == targetOscAddress)
        {
            Debug.Log("Found matching PropertySender, changing OscAddress");
            propertySender.OscAddress = newAddress;
        }
    }
}
