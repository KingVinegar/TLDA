using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabletFunctions : MonoBehaviour {
    
    public GameObject Tablet;

    public void OpenTablet()
    {
        if (Tablet != null)
        {
            bool isActive = Tablet.activeSelf;

            Tablet.SetActive(!isActive);
        }
    }
}
   
