using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoosterPrefab : MonoBehaviour
{
    // Start is called before the first frame update
    //[SerializeField] private Button btnAddResource = null;
    public GameObject goResource = null;
    [SerializeField] private Button btnTitle = null;
    public Image imgBackground = null;
    //public Toggle tgEnergy = null;
    //public Toggle tgGem = null;
    //public Toggle tgCoin = null;
    public InputField ifEnergy = null;
    public InputField ifGem = null;
    public InputField ifCoin = null;
    public InputField ifLimit = null;
    public bool checkIsRemove = true;
    public bool allowClick = true;

    void Start()
    {
        //btnAddResource.onClick.AddListener(onClickAddResource);
        btnTitle.onClick.AddListener(onClickTitle);
        imgBackground.color = new Color32(47, 255, 0, 255);
    }

    private void onClickTitle()
    {
        if(checkIsRemove)
        {
            checkIsRemove = false;
            goResource.SetActive(false);
            imgBackground.color = new Color32(255, 11, 9, 255);
        }    
        else
        {
            checkIsRemove = true;
            goResource.SetActive(true);
            imgBackground.color = new Color32(47, 255, 0, 255);
        }    
    }    

    private void onClickAddResource()
    {
        if (allowClick)
        {
            goResource.SetActive(true);
            gameObject.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 210f);
            allowClick = false;
        }
        else
        {
            goResource.SetActive(false);
            gameObject.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 60f);
            allowClick = true;
        }
        LayoutRebuilder.ForceRebuildLayoutImmediate(gameObject.transform.parent as RectTransform);
    }    

    // Update is called once per frame
    void Update()
    {
        
    }
}
