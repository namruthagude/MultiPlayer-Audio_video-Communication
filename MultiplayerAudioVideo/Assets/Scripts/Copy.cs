using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Copy : MonoBehaviour
{
    public TMP_Text roomName;
    
    public void CopyText()
    {
        GUIUtility.systemCopyBuffer = roomName.text;
    }
}
