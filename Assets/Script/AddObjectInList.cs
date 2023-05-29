using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AddObjectInList : MonoBehaviour
{
    [SerializeField] private GameObject sampleItem;
    [SerializeField] private GameObject content;

    public void onClickAdd()
    {
        var copy = Instantiate(sampleItem);
        //copy.transform.parent = content.transform;
        copy.transform.SetParent(content.transform, false);
    }    
}
