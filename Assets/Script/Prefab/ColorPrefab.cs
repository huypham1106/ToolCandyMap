using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorPrefab : MonoBehaviour
{
    [SerializeField] private Button btnDelete = null;
    public Text txtColor = null;
    public Image imgColor = null;
    public InputField ifRatio = null;
    public InputField ifLimit = null;

    // Start is called before the first frame update
    void Start()
    {

        btnDelete.onClick.AddListener(onClickDelete);
    }

    
    private void onClickDelete()
    {
        Destroy(gameObject);
    }
}
