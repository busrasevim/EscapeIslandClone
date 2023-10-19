using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PCInputHandler : MonoBehaviour, IInputHandler
{
    public void OnPressed()
    {
        Debug.Log("PC OnPressed");
    }

    public void OnRelease()
    {
        Debug.Log("PC OnRelease");
    }
}
