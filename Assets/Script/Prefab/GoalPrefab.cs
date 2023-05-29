using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoalPrefab : MonoBehaviour
{
    // Start is called before the first frame update
    public Button btnGoalItem = null;
    [SerializeField] private Button btnDelete = null;
    public GameObject goEditInformation = null;
    public Dropdown ddType = null;
    public string StrType;
    [SerializeField] private InputField ifAmount = null;

    public Text txtStatus;
    public Text txtAmount;
    public Text txtItem;
    public Image imgCandy;
    public bool allowClick = true;

    void Start()
    {
        btnGoalItem.onClick.AddListener(onClickGoalitem);
        btnDelete.onClick.AddListener(onClickDelete);
    }

    private void onClickDelete()
    {
        Destroy(gameObject);
    }    
    private void onClickGoalitem()
    {
        if (allowClick)
        {
            goEditInformation.SetActive(true);
            gameObject.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 210f);
            allowClick = false;
        }
        else
        {
            goEditInformation.SetActive(false);
            gameObject.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 75f);
            allowClick = true;
        }
        LayoutRebuilder.ForceRebuildLayoutImmediate(gameObject.transform.parent as RectTransform);
    }    

    public void onValueChangeType(int indexType)
    {
        txtItem.text = ddType.options[indexType].text;
        imgCandy.sprite = MainGameController.Instance.dictCandySprite[removesapce(txtItem.text)];
    }  
    public void onValueChangeAmount()
    {
        txtAmount.text = ifAmount.text;
        if (string.IsNullOrEmpty(txtAmount.text))
        {
            Toast.ShowMessage("Data not allow empty !!!!", Toast.Position.top, Toast.Time.twoSecond);
            ifAmount.text = "0";
            txtAmount.text = ifAmount.text;
        }    
    }

    private string removesapce(string text)
    {
        string textConvert = text.Replace(" ", string.Empty);
        return textConvert;
    }
}
