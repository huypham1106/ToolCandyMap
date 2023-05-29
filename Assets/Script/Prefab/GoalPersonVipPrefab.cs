using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoalPersonVipPrefab : MonoBehaviour
{
    [SerializeField] private Button btnGoalPersonItem = null;
    public InputField ifCharacter = null;
    public InputField ifGift = null;
    [SerializeField] private Button btnAdd = null;
    public GameObject goalContent = null;
    [SerializeField] private GoalPrefab goalPrefab = null;
    //[SerializeField] private Dropdown ddType = null;
    [SerializeField] private Dropdown ddGoals = null;
    [SerializeField] private Button btnDelete = null;
    public GameObject goEditInformation = null;

    [SerializeField] private List<string> listCandyGoal;
    [SerializeField] private List<string> listElementGoal;
    [SerializeField] private List<string> listSpecialGoal;
    [SerializeField] private List<string> listSpecialBlockGoal;
    [SerializeField] private List<string> listCollectableGoal;
    [SerializeField] private List<string> listMultiBlockGoal;

    public string TypeCustomer = "Vip";
    public Text txtAmount;
    public string txtCharacter = null;
    public string txtGift = null;
    public string txtSkin = null;
    public List<List<GoalTemp>> goalTempsList = new List<List<GoalTemp>>();
    public List<string> listRequirelist = new List<string>();
    public Dictionary<string, List<GoalTemp>> dictListRequireVip = new Dictionary<string, List<GoalTemp>>();
    public int indexListRequeire = 1;
    public string txtRatio;
    public string txtTurnExist;
    public string txtTurnFrom;
    public string txtTurnTo;
    public bool allowClick = true;


    // Start is called before the first frame update
    void Start()
    {
        if (listRequirelist.Count == 0)
        {
            listRequirelist.Add(indexListRequeire.ToString());
        }

        btnAdd.onClick.AddListener(OnClickBtnAdd);
        btnGoalPersonItem.onClick.AddListener(onClickGoalPersonItem);
        btnDelete.onClick.AddListener(onClickDelete);

        listCandyGoal = MainGameController.Instance.listCandyGoal;
        listElementGoal = MainGameController.Instance.listElement;
        listSpecialGoal = MainGameController.Instance.listSpecialCandy;
        listSpecialBlockGoal = MainGameController.Instance.listSpecialBlock;
        listCollectableGoal = MainGameController.Instance.listCollectable;
        listMultiBlockGoal = MainGameController.Instance.listMultiBlock;

    }
    private void onClickDelete()
    {
        Destroy(gameObject);
        MainGameController.Instance.goEditInformationVip.SetActive(false);
        GoalPersonPrefab[] goalPersonPrefabsList = MainGameController.Instance.goalPersonContent.GetComponentsInChildren<GoalPersonPrefab>();
        if (goalPersonPrefabsList.Length == 1)
        {
            MainGameController.Instance.countPerson = 1;
        }
    }

    private void onClickGoalPersonItem()
    {
        MainGameController.Instance.checkClickOtherGoalPersonVip(this);
        if (allowClick)
        {
            MainGameController.Instance.getDataGoalPersonVip(this);

            if (MainGameController.Instance.goEditInformationVip.activeSelf)
            {
                this.allowClick = false;
            }
        }
        else
        {
            MainGameController.Instance.goEditInformationVip.SetActive(false);
            this.allowClick = true;
        }
    }

    private void OnClickBtnAdd()
    {
        var copy = Instantiate(goalPrefab);
        copy.transform.SetParent(goalContent.transform, false);
        copy.ddType.ClearOptions();
        if (ddGoals.value == 0)
        {
            setTextGoal(copy, "Reach", "0", "Score", null);
        }
        else if (ddGoals.value == 1)
        {
            setTextGoal(copy, "Collect", "0", listCandyGoal[0], MainGameController.Instance.dictCandySprite[removesapce(listCandyGoal[0])]);
            //copy.ddType.AddOptions(listCandyGoal);
            MainGameController.Instance.setImageDropdown(copy.ddType, listCandyGoal, MainGameController.Instance.dictCandySprite);
        }
        else if (ddGoals.value == 2)
        {
            setTextGoal(copy, "Collect", "0", listElementGoal[0], MainGameController.Instance.dictCandySprite[removesapce(listElementGoal[0])]);
            //copy.ddType.AddOptions(listElementGoal);
            MainGameController.Instance.setImageDropdown(copy.ddType, listElementGoal, MainGameController.Instance.dictCandySprite);
        }
        else if (ddGoals.value == 3)
        {
            setTextGoal(copy, "Collect", "0", listSpecialBlockGoal[0], MainGameController.Instance.dictCandySprite[removesapce(listSpecialBlockGoal[0])]);
            //copy.ddType.AddOptions(listSpecialBlockGoal);
            MainGameController.Instance.setImageDropdown(copy.ddType, listSpecialBlockGoal, MainGameController.Instance.dictCandySprite);
        }
        else if (ddGoals.value == 4)
        {
            setTextGoal(copy, "Collect", "0", listCollectableGoal[0], MainGameController.Instance.dictCandySprite[removesapce(listCollectableGoal[0])]);
            //copy.ddType.AddOptions(listCollectableGoal);
            MainGameController.Instance.setImageDropdown(copy.ddType, listCollectableGoal, MainGameController.Instance.dictCandySprite);
        }
        else if (ddGoals.value == 5)
        {
            setTextGoal(copy, "Destroy", "", "all chocolate", MainGameController.Instance.dictCandySprite["Chocolate"]);
            copy.btnGoalItem.interactable = false;
        }
        copy.StrType = ddGoals.options[ddGoals.value].text;
    }

    public void setTextGoal(GoalPrefab goalPre, string status, string amount, string item, Sprite spriteCandy)
    {
        goalPre.txtStatus.text = status;
        goalPre.txtAmount.text = amount;
        goalPre.txtItem.text = item;
        goalPre.imgCandy.sprite = spriteCandy;
    }

    private string removesapce(string text)
    {
        string textConvert = text.Replace(" ", string.Empty);
        return textConvert;
    }

}
