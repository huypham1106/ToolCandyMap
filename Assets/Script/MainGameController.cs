using FullSerializer;
using CandyMatch3.Game.Common;
using SimpleFileBrowser;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
public class MainGameController : MonoBehaviour
{
    private enum BrushType
    {
        Candy,
        Element,
        SpecialCandy,
        SpecialBlock,
        Collectable,
        Empty,
        Hole
    }
    private Level currentLevel;
    private BrushType currentBrushType;
    private CandyType currentCandyType;
    private SpecialCandyType currentSpecialCandyType;
    private SpecialBlockType currentSpecialBlockType;
    private ElementType currentElementType;
    private CollectableType currentCollectableType;
    private MultiBlockType currentMultiBlockType;



    [Header("Level")]
    [SerializeField] private GameObject goLevel = null;
    [SerializeField] private Transform levelContent = null;
    public InputField ifWidth = null;
    [SerializeField] private InputField ifHeight = null;
    public Dropdown ddBrushType = null;
    [SerializeField] private Dropdown ddCandy = null;
    public Dropdown ddBrushMode = null;
    [SerializeField] private Text txtScaleMap = null;
    [SerializeField] private Slider sdScaleMap = null;
    [SerializeField] private Button btnAutoMapByCode = null;
    [SerializeField] private InputField ifCode = null;
    [SerializeField] private Button btnAutoMapByJson = null;
    [SerializeField] private InputField ifJson = null;

    [SerializeField] private CandyPrefab candyPrefab = null;

    public List<string> listCandy;
    public List<string> listCandyGoal;
    public List<string> listElement;
    public List<string> listSpecialCandy;
    public List<string> listSpecialBlock;
    public List<string> listCollectable;
    public List<string> listMultiBlock;
    public List<string> listMap;

    [SerializeField] private Sprite[] listCandySprite;

    public Dictionary<string, Sprite> dictCandySprite = new Dictionary<string, Sprite>();
    public int width;
    public int height;
    string candyNameDropDown;
    private float widthContent;
    private float heightContent;
    public int idCakeBox = 0;

    [Header("EditLevelMap")]
    [SerializeField] private Sprite[] listMapNoNumberSprite;
    public Dictionary<string, Sprite> dictMapNoNumberSprite = new Dictionary<string, Sprite>();

    [SerializeField] private Sprite[] listMapSprite;
    public Dictionary<string, Sprite> dictMapSprite = new Dictionary<string, Sprite>();

    [SerializeField] private GameObject goEditLevelMap = null;
    [SerializeField] private GameObject EditLevelMapContent = null;
    [SerializeField] private BgMapTIlePrefab bgMapTIlePrefab = null;
    [SerializeField] private Toggle tgShowCandy = null;
    [SerializeField] private Button btnAutoSetBg = null;
    public int IdBgMapChoose = 1;
    List<int> ListidBgMapForHole = new List<int> { 1, 22, 23, 24, 25,26,27,28,29,30,31,32,33,34 };
    List<int> ListidBgMap = new List<int> { 2, 3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21 };
    private int left;
    private int up;
    private int down;
    private int right;
    private int leftDown;
    private int leftUp;
    private int rightDown;
    private int rightUp;

    [Header("Goals")]
    [SerializeField] private Dropdown ddGoals = null;
    [SerializeField] private Dropdown ddGoalsVip = null;
    [SerializeField] private GoalPrefab preGoal = null;
    [SerializeField] private Transform tfContetnGoal = null;
    [SerializeField] private Button btnAddGoalPerson = null;
    [SerializeField] private Button btnAddGoalPersonVip = null;
    public Transform goalPersonContent = null;
    public Transform goalContent = null;
    [SerializeField] private Toggle tgAwardSpeacialCandy = null;
    [SerializeField] private Dropdown ddType = null;
    [SerializeField] private Image imgTypeGoal = null;
    [SerializeField] private GoalPersonPrefab goalPersonPrefab = null;
    [SerializeField] private GoalPersonVipPrefab goalPersonVipPrefab = null;
    [SerializeField] private Button btnAddGoal = null;
    [SerializeField] private Button btnAddGoalVip = null;
    [SerializeField] private Button btnCloseEditInformation = null;
    [SerializeField] private Button btnCloseEditInformationVip = null;
    [SerializeField] private InputField ifSkin = null;
    [SerializeField] private InputField ifSkinVip = null;

    //List require customer vip
    [SerializeField] private Dropdown ddListRequire = null;
    [SerializeField] private Button btnAddListRequire = null;
    [SerializeField] private Button btnRemoveListRequire = null;
    private Dictionary<string, List<GoalTemp>> dictListRequireVip = new Dictionary<string, List<GoalTemp>>();
    private int cacheIndexDdListRequire = 0;

    public Text txtGoalCustomer = null;
    private GoalPersonPrefab goalPersonPrefabIsOpen = null;
    private GoalPersonVipPrefab goalPersonVipPrefabIsOpen = null;
    public GameObject goEditInformation = null;
    public GameObject goEditInformationVip = null;
    public InputField ifCharacter = null;
    public InputField ifGift = null;

    //CustomerVip
    public InputField ifCharacterVip = null;
    public InputField ifGiftVip = null;
    public Transform goalContentVip = null;
    public InputField ifRatioVip = null;
    public InputField ifTurnExistVip = null;
    public InputField ifTurnFromVip = null;
    public InputField ifTurnToVip = null;
    public Text txtGoalCustomerVip = null;

    public int countPerson = 1;

    [Header("AvailableColors")]
    [SerializeField] private Button btnAddColor = null;
    [SerializeField] private Dropdown ddColor = null;
    [SerializeField] private ColorPrefab preColor = null;
    [SerializeField] private Transform colorContent = null;
    [SerializeField] private InputField ifCollectableChance = null;
    [SerializeField] private Button btnRefresh = null;

    public List<string> listColor;

    [Header("Other")]
    [SerializeField] private Button btnNew = null;
    [SerializeField] private Button btnOpen = null;
    [SerializeField] private Button btnSave = null;
    [SerializeField] private Button btnCopy = null;
    private string textJsonLoadFile = "";
    private bool allowReset = false;
    private string txtPathFolderAsset = "";
    private string pathfoldersave = "C:\\";

    [Header("General")]
    [SerializeField] private InputField ifLevelNumber = null;
    [SerializeField] private Dropdown ddLimitType = null;
    [SerializeField] private Text txtMoves = null;
    [SerializeField] private InputField ifMoves = null;
    [SerializeField] private InputField ifLimitBooster = null;
    [SerializeField] private InputField ifStar1Score = null;
    [SerializeField] private InputField ifStar2Score = null;
    [SerializeField] private InputField ifStar3Score = null;

    [Header("Boosters")]
    [SerializeField] private Toggle tgLollipop = null;
    //[SerializeField] private Toggle tgBomb = null;
    [SerializeField] private Toggle tgSwitch = null;
    [SerializeField] private Toggle tgColorBomb = null;
    [SerializeField] private Toggle tgHorizontalRocket = null;
    [SerializeField] private Toggle tgVerticalRocket = null;
    [SerializeField] private Toggle tgTNTWrapped = null;
    [SerializeField] private GameObject boosterContent = null;

    private List<BoosterResource> resourceList = new List<BoosterResource>();

    [Header("Failing Hole")]
    [SerializeField] private Toggle tgCheckFailingHole = null;
    [SerializeField] private GameObject goMainAvailableColorHole = null;
    [SerializeField] private Button btnAddColorHole = null;
    [SerializeField] private Dropdown ddColorHole = null;
    [SerializeField] private Transform colorContentHole = null;
    [SerializeField] private Button btnRefreshHole = null;
    [SerializeField] private Button btnCloseFailingHole = null;
    [SerializeField] private Text txtValueX = null;
    [SerializeField] private Text txtValueY = null;
    public GameObject goFalingHole = null;
    private CandyPrefab candyPrefabIsOpen = null;
    private CandyPrefab candyPrefabTemp = null;

    public static MainGameController Instance { get; private set; }
    private void Awake()
    {

        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }

        listCandySprite = Resources.LoadAll<Sprite>("Pieces");
        listMapNoNumberSprite = Resources.LoadAll<Sprite>("MapNoNumber");
        listMapSprite = Resources.LoadAll<Sprite>("Map");

        currentLevel = new Level();
        
    }
    void Start()
    {
        goLevel.SetActive(true);

        ifWidth.onValueChanged.AddListener(delegate { generateMap(); });
        ifHeight.onValueChanged.AddListener(delegate { generateMap(); });
        //ddCandy.AddOptions(listCandy);
        ddColor.AddOptions(listColor);
        ddColorHole.AddOptions(listColor);
        btnAddGoalPerson.onClick.AddListener(onClickAddGoalPerson);
        btnAddGoalPersonVip.onClick.AddListener(onClickAddGoalPersonVip);
        btnAddGoal.onClick.AddListener(onClickAddGoal);
        btnAddGoalVip.onClick.AddListener(onClickAddGoalVip);
        btnAddColor.onClick.AddListener(onClickAddColor);
        btnOpen.onClick.AddListener(LoadAsFile);
        btnSave.onClick.AddListener(SaveAsFile);
        btnCopy.onClick.AddListener(CopyAsFile);
        btnNew.onClick.AddListener(onClickNewData);
        btnRefresh.onClick.AddListener(onClickRefreshColorButton);
        btnCloseEditInformation.onClick.AddListener(onClickCloseEditInformation);
        btnCloseEditInformationVip.onClick.AddListener(onClickCloseEditInformationVip);
        btnAutoSetBg.onClick.AddListener(checkBgMapAuto);
        btnAutoMapByCode.onClick.AddListener(onClickAutoMapByCode);
        btnAutoMapByJson.onClick.AddListener(onClickAutoMapByJson);
        btnRefreshHole.onClick.AddListener(onClickRefreshColorButtonHole);
        btnAddColorHole.onClick.AddListener(onClickAddColorHole);
        btnCloseFailingHole.onClick.AddListener(onClickCloseFailingHole);
        btnAddListRequire.onClick.AddListener(onClickAddListRequire);
        btnRemoveListRequire.onClick.AddListener(onClickRemoveListRequire);


        txtPathFolderAsset = Application.dataPath + "/StreamingAssets/pathfoldersave.txt";
        if (File.Exists(txtPathFolderAsset))
        {
            string content = File.ReadAllText(txtPathFolderAsset);
            pathfoldersave = content;
        }

        for (int i = 0; i < listCandySprite.Length; i++)
        {
            dictCandySprite.Add(listCandySprite[i].name, listCandySprite[i]);
        }
        listMap = new List<string>();
        for ( int j = 0; j< listMapSprite.Length; j++)
        {
            dictMapNoNumberSprite.Add(listMapNoNumberSprite[j].name, listMapNoNumberSprite[j]);
            dictMapSprite.Add(listMapSprite[j].name, listMapSprite[j]);
            listMap.Add((j + 1).ToString());
        }    
        setImageDropdown(ddCandy, listCandy,dictCandySprite);
        onClickNewData();
    }

    #region Other

    public void SaveAsFile()
    {
        if (goEditInformation.activeSelf)
        {
            setDataGoalPerson(goalPersonPrefabIsOpen);
            goEditInformation.SetActive(false);
        }
        if (goEditInformationVip.activeSelf)
        {
            setDataGoalPersonVip(goalPersonVipPrefabIsOpen);
            goEditInformationVip.SetActive(false);
        }
        if(goFalingHole.activeSelf)
        {
            setDataCandyFailingHole(candyPrefabIsOpen);
            goFalingHole.SetActive(false);
        }    
        FileBrowser.ShowSaveDialog((paths) => onSuccessSave(paths), null, FileBrowser.PickMode.Files, false, pathfoldersave, ifLevelNumber.text + ".json", "Save As", "Save");
    }
    private void onSuccessSave(string[] paths)
    {
        if (paths.Length > 0)
        {
            pathfoldersave = Path.GetDirectoryName(paths[0]);
             try
            {
                saveLevelData();
                SaveJsonFile(FileBrowser.Result[0], currentLevel);
            }
            catch
            {
                Toast.ShowMessage("Data not correct or missing !!!!",Toast.Position.top, Toast.Time.twoSecond);
            }

        }
    }

    public void LoadAsFile()
    {
        FileBrowser.ShowLoadDialog((paths) => onSuccessLoad(paths), null, FileBrowser.PickMode.Files, false, pathfoldersave, "", "Load File", "Load");
    }
    private void onSuccessLoad(string[] paths)
    {
        if (paths.Length > 0)
        {
            pathfoldersave = Path.GetDirectoryName(paths[0]);
            textJsonLoadFile = File.ReadAllText(FileBrowser.Result[0]);
            try
            {
                currentLevel = LoadJsonFile<Level>(FileBrowser.Result[0]);
            }
            catch
            {
                resetData();
                currentLevel = new Level();
                currentLevel.checkDataOldMap(textJsonLoadFile);
            }
            if (this.currentLevel != null)
            {
                loadLevelData(this.currentLevel);
            }
        }
    }

    private void onClickNewData()
    {
        currentLevel = new Level();
        allowReset = true;
        loadLevelData(currentLevel);
        resetData();
        createColorList();

        //createColorListHole();
    }
    private void resetData()
    {
        ifLimitBooster.text = "2";
        ifCode.text = "";
        ifJson.text = "";
        BoosterPrefab[] boosterPrefabsList = boosterContent.transform.GetComponentsInChildren<BoosterPrefab>();
        BoosterType nameFirstBooster = (BoosterType)Enum.Parse(typeof(BoosterType), boosterPrefabsList[0].name);
        if (!currentLevel.availableBoosters.ContainsKey(nameFirstBooster))
        {
            for (int i = 0; i < boosterPrefabsList.Length; i++)
            {
                boosterPrefabsList[i].goResource.SetActive(true);
                boosterPrefabsList[i].imgBackground.color = new Color32(47, 255, 0, 255);
                boosterPrefabsList[i].checkIsRemove = true;
                boosterPrefabsList[i].ifCoin.text = "";
                boosterPrefabsList[i].ifEnergy.text = "";
                boosterPrefabsList[i].ifGem.text = "";
                boosterPrefabsList[i].ifLimit.text = "";
            }
        }
    }    

       
    void OnApplicationQuit()
    {
        if (File.Exists(txtPathFolderAsset))
        {
            File.WriteAllText(txtPathFolderAsset, pathfoldersave);
        }
        Debug.Log("Application ending after " + Time.time + " seconds");
    }

    #endregion

    #region AvailableColors

    private void onClickAddColor()
    {
        bool allowAdd = true;
        ColorPrefab[] colorList = colorContent.transform.GetComponentsInChildren<ColorPrefab>();
        for (int i = 0; i < colorList.Length; i++)
        {
            if (colorList[i].txtColor.text == ddColor.options[ddColor.value].text)
            {
                allowAdd = false;
                break;
            }
        }

        if (allowAdd)
        {
            var copy = Instantiate(preColor);
            copy.transform.SetParent(colorContent, false);

            setTextColor(copy, ddColor.options[ddColor.value].text);
        }
    }

    private void setTextColor(ColorPrefab colorPre, string color, string ratio = "0", string limit = "-1")
    {
        colorPre.txtColor.text = color;
        colorPre.ifRatio.text = ratio;
        colorPre.ifLimit.text = limit;

        if (dictCandySprite != null && dictCandySprite.ContainsKey(color + "Candy"))  
        colorPre.imgColor.sprite = dictCandySprite[color+"Candy"];
        else
        colorPre.imgColor.sprite = dictCandySprite[color];
    }

    private void createColorList()
    {
        for (int i = 0; i < ddColor.options.Count; i++)
        {
            var copy = Instantiate(preColor);
            copy.transform.SetParent(colorContent, false);

            setTextColor(copy, ddColor.options[i].text);
        }
        ifHeight.text = "9";
        ifWidth.text = "8";
    }

    private void onClickRefreshColorButton()
    {
        CandyPrefab[] candyPrefabList = levelContent.transform.GetComponentsInChildren<CandyPrefab>();
        clearTranform(colorContent);

        List<string> candyNameList = new List<string>();
        for(int i=0; i<candyPrefabList.Length; i++)
        {
            //string[] tempNameCandy = candyPrefabList[i].ImgCandy.name;
            string[] tempImamge = candyPrefabList[i].ImgCandy.sprite.name.Split("_");
            string tempCandyName = string.Concat(tempImamge[0].Select(x => Char.IsUpper(x) ? " " + x : x.ToString())).TrimStart(' ');
            string[] tempColor = tempCandyName.Split(" ");

            if (listColor.Contains(tempColor[0]) && candyPrefabList[i].ImgCandy.gameObject.activeSelf)
            {
                if (!candyNameList.Contains(tempColor[0]))
                {
                    candyNameList.Add(tempColor[0]);
                }
            }
        } 
        
        for(int j = 0; j<candyNameList.Count; j++)
        {
            var copy = Instantiate(preColor);
            copy.transform.SetParent(colorContent, false);

            setTextColor(copy, candyNameList[j]);
        }    
    }

    #endregion

    #region Failing Hole

    private void onClickAddColorHole()
    {
        bool allowAdd = true;
        ColorPrefab[] colorList = colorContentHole.transform.GetComponentsInChildren<ColorPrefab>();
        for (int i = 0; i < colorList.Length; i++)
        {
            if (colorList[i].txtColor.text == ddColorHole.options[ddColorHole.value].text)
            {
                allowAdd = false;
                break;
            }
        }

        if (allowAdd)
        {
            var copy = Instantiate(preColor);
            copy.transform.SetParent(colorContentHole, false);

            setTextColorHole(copy, ddColorHole.options[ddColorHole.value].text);
        }
    }
    private void setTextColorHole(ColorPrefab colorPre, string color, string ratio = "0")
    {
        colorPre.txtColor.text = color;
        colorPre.ifRatio.text = ratio;

        if (dictCandySprite != null && dictCandySprite.ContainsKey(color + "Candy"))
            colorPre.imgColor.sprite = dictCandySprite[color + "Candy"];
        else
            colorPre.imgColor.sprite = dictCandySprite[color];
    }

    private void createColorListHole()
    {
        for (int i = 0; i < ddColorHole.options.Count; i++)
        {
            var copy = Instantiate(preColor);
            copy.transform.SetParent(colorContentHole, false);

            setTextColor(copy, ddColorHole.options[i].text);
            
        }
    }

    private void onClickRefreshColorButtonHole()
    {
        CandyPrefab[] candyPrefabList = levelContent.transform.GetComponentsInChildren<CandyPrefab>();
        clearTranform(colorContentHole);

        List<string> candyNameList = new List<string>();
        for (int i = 0; i < candyPrefabList.Length; i++)
        {
            //string[] tempNameCandy = candyPrefabList[i].ImgCandy.name;
            string[] tempImamge = candyPrefabList[i].ImgCandy.sprite.name.Split("_");
            string tempCandyName = string.Concat(tempImamge[0].Select(x => Char.IsUpper(x) ? " " + x : x.ToString())).TrimStart(' ');
            string[] tempColor = tempCandyName.Split(" ");

            if (listColor.Contains(tempColor[0]) && candyPrefabList[i].ImgCandy.gameObject.activeSelf)
            {
                if (!candyNameList.Contains(tempColor[0]))
                {
                    candyNameList.Add(tempColor[0]);
                }
            }
        }

        for (int j = 0; j < candyNameList.Count; j++)
        {
            var copy = Instantiate(preColor);
            copy.transform.SetParent(colorContentHole, false);

            setTextColor(copy, candyNameList[j]);
        }
    }

    public void onValueChangeTgCheckFailingHole_editor(bool checkHole)
    {
        goMainAvailableColorHole.SetActive(checkHole);
        candyPrefabIsOpen.isFailHole = checkHole;
        candyPrefabIsOpen.setColorButtonFailingHole(checkHole);
    }

    public void CheckClickOtherTile(CandyPrefab candyPrefab)
    {
        candyPrefabIsOpen = candyPrefab;
        if (candyPrefabTemp == null)
        {
            candyPrefabTemp = candyPrefab;
        }
        else if (candyPrefabTemp != candyPrefab)
        {
            setDataCandyFailingHole(candyPrefabTemp);
            candyPrefabTemp = candyPrefab;
        }
        else
        {
            setDataCandyFailingHole(candyPrefab);
        }
    }  
    
    public void getDataCandyFailingHole(CandyPrefab candyPrefab)
    {
        CandyPrefab[] candyPrefabsList = levelContent.transform.GetComponentsInChildren<CandyPrefab>();

        for (int i = 0; i < candyPrefabsList.Length; i++)
        {
            if (candyPrefabsList[i].gameObject.activeSelf)
            {
                candyPrefabsList[i].allowClick = true;
            }
        }
        goFalingHole.SetActive(true);
        txtValueX.text = candyPrefab.x.ToString();
        txtValueY.text = candyPrefab.y.ToString();
        tgCheckFailingHole.isOn = candyPrefab.isFailHole;
        clearTranform(colorContentHole);
        if (candyPrefab.listFailHoleColor.Count > 0)
        {
            for (int j = 0; j < candyPrefab.listFailHoleColor.Count; j++)
            {
                var copy = Instantiate(preColor);
                copy.transform.SetParent(colorContentHole, false);
                if(candyPrefab.listFailHoleColor[j].Limit == 0)
                setTextColor(copy, candyPrefab.listFailHoleColor[j].Type, candyPrefab.listFailHoleColor[j].Ratio.ToString());
                else
                setTextColor(copy, candyPrefab.listFailHoleColor[j].Type, candyPrefab.listFailHoleColor[j].Ratio.ToString(), candyPrefab.listFailHoleColor[j].Limit.ToString());
            }
        }
    }    

    public void setDataCandyFailingHole(CandyPrefab candyPrefab)
    {
        if (goFalingHole.activeSelf)
        {
            ColorPrefab[] colorPrefabList = colorContentHole.GetComponentsInChildren<ColorPrefab>() ;
            candyPrefab.listFailHoleColor.Clear();

            for (int i=0; i< colorPrefabList.Length; i ++)
            {
                AvailableColor failHoleColor = new AvailableColor();
                failHoleColor.Type = colorPrefabList[i].txtColor.text;
                failHoleColor.Ratio = colorPrefabList[i].ifRatio.text.ToInt();
                failHoleColor.Limit = colorPrefabList[i].ifLimit.text.ToInt();
                candyPrefab.listFailHoleColor.Add(failHoleColor);
            }    

        }
    }    

    #endregion

    #region Goals
    private void onClickAddGoalPerson()
    {
        var copy = Instantiate(goalPersonPrefab);
        copy.transform.SetParent(goalPersonContent.transform, false);
        copy.txtAmount.text = countPerson.ToString();
        countPerson++;
    } 
    private void onClickAddGoalPersonVip()
    {
        var copy = Instantiate(goalPersonVipPrefab);
        copy.transform.SetParent(goalPersonContent.transform, false);
        copy.txtAmount.text = countPerson.ToString();
        countPerson++;
    }    

    private void onClickCloseEditInformation()
    {
        setDataGoalPerson(goalPersonPrefabIsOpen);
        goEditInformation.SetActive(false);
    }
    private void onClickCloseEditInformationVip()
    {
        setDataGoalPersonVip(goalPersonVipPrefabIsOpen);
        goEditInformationVip.SetActive(false);
    }
    private void onClickCloseFailingHole()
    {
        setDataCandyFailingHole(candyPrefabIsOpen);
        goFalingHole.SetActive(false);
    }
    
    public void onValueChangeDdListRequire_editor(int index)
    {
        SaveListRequiredIsOpen();
        LoadListRequiredIsOpen(index);


        cacheIndexDdListRequire = index;
    }   
    private void SaveListRequiredIsOpen()
    {
        List<GoalTemp> goalTempListToSave = new List<GoalTemp>();
        GoalPrefab[] goalPrefabList = goalContentVip.GetComponentsInChildren<GoalPrefab>();
        for (int i = 0; i < goalPrefabList.Length; i++)
        {
            GoalTemp goalTemp = new GoalTemp();
            goalTemp.txtAmount = goalPrefabList[i].txtAmount.text;
            goalTemp.txtStatus = goalPrefabList[i].txtStatus.text;
            goalTemp.txtItem = goalPrefabList[i].txtItem.text;
            goalTemp.imgCandy = goalPrefabList[i].imgCandy.sprite;
            goalTemp.StrType = goalPrefabList[i].StrType;
            goalTemp.valueDdType = goalPrefabList[i].ddType.value;
            goalTemp.ddTypeOption = GetOptions(goalPrefabList[i].ddType.options);

            goalTempListToSave.Add(goalTemp);

        }
        if (!dictListRequireVip.ContainsKey(ddListRequire.options[cacheIndexDdListRequire].text))
        {
            dictListRequireVip.Add(ddListRequire.options[cacheIndexDdListRequire].text, goalTempListToSave);
        }
        else
        {
            dictListRequireVip[ddListRequire.options[cacheIndexDdListRequire].text] = goalTempListToSave;
        }
    }  
    private void LoadListRequiredIsOpen(int index)
    {
        clearTranform(goalContentVip);
        List<GoalTemp> goalTempListToload = new List<GoalTemp>();
        if (dictListRequireVip.ContainsKey(ddListRequire.options[index].text))
        {
            goalTempListToload = dictListRequireVip[ddListRequire.options[index].text];
            if (goalTempListToload.Count > 0)
            {
                for (int j = 0; j < goalTempListToload.Count; j++)
                {
                    var copy = Instantiate(preGoal);
                    copy.transform.SetParent(goalContentVip, false);
                    copy.ddType.ClearOptions();

                    copy.txtStatus.text = goalTempListToload[j].txtStatus;
                    copy.txtItem.text = goalTempListToload[j].txtItem;
                    copy.txtAmount.text = goalTempListToload[j].txtAmount;
                    copy.imgCandy.sprite = goalTempListToload[j].imgCandy;
                    copy.StrType = goalTempListToload[j].StrType;
                    if (goalTempListToload[j].ddTypeOption != null)
                    {
                        setImageDropdown(copy.ddType, goalTempListToload[j].ddTypeOption, dictCandySprite);
                        copy.ddType.value = goalTempListToload[j].valueDdType;
                    }
                }
            }
        }
    }    

    private void onClickAddListRequire()
    {
        //int indexListRequire = ++goalPersonVipPrefabIsOpen.indexListRequeire;
        int indexListRequire = (int.Parse(ddListRequire.options[ddListRequire.options.Count - 1].text))+1;
        goalPersonVipPrefabTemp.listRequirelist.Add(indexListRequire.ToString());
        ddListRequire.ClearOptions();
        ddListRequire.AddOptions(goalPersonVipPrefabTemp.listRequirelist);
    }
    private void onClickRemoveListRequire()
    {
        if (ddListRequire.options.Count == 1) return;
        string valueListRequire = ddListRequire.options[ddListRequire.value].text;
        goalPersonVipPrefabTemp.listRequirelist.Remove(valueListRequire);
        dictListRequireVip.Remove(valueListRequire);
        cacheIndexDdListRequire = 0;
        ddListRequire.ClearOptions();
        ddListRequire.AddOptions(goalPersonVipPrefabTemp.listRequirelist);
        LoadListRequiredIsOpen(cacheIndexDdListRequire);
    }    
    
    public void  getDataGoalPerson(GoalPersonPrefab goalPersonPrefab)
    {

        GoalPersonPrefab[] goalPersonPrefabsList= goalPersonContent.transform.GetComponentsInChildren<GoalPersonPrefab>();

        for(int i=0; i<goalPersonPrefabsList.Length; i++)
        {
            if (goalPersonPrefabsList[i].gameObject.activeSelf)
            {
                goalPersonPrefabsList[i].allowClick = true;
            }
        }
        goEditInformation.SetActive(true);
        if (goEditInformationVip.activeSelf)
        {
            setDataGoalPersonVip(goalPersonVipPrefabIsOpen);
            goEditInformationVip.SetActive(false);
        }
        txtGoalCustomer.text = "Customer " + goalPersonPrefab.txtAmount.text;
        if (goalPersonPrefab.txtCharacter != null)
        {
            ifCharacter.text = goalPersonPrefab.txtCharacter;
        }
        if (goalPersonPrefab.txtGift != null)
        {
            ifGift.text = goalPersonPrefab.txtGift;
        }
        if (goalPersonPrefab.txtSkin != null)
        {
            ifSkin.text = goalPersonPrefab.txtSkin;
        }

        clearTranform(goalContent);
        //goalPersonPrefab.goalTempsList = new List<GoalTemp>();
        if (goalPersonPrefab.goalTempsList.Count > 0)
        {
            for(int j=0; j< goalPersonPrefab.goalTempsList.Count; j++)
            {
                var copy = Instantiate(preGoal);
                copy.transform.SetParent(goalContent, false);
                copy.ddType.ClearOptions();

                copy.txtStatus.text = goalPersonPrefab.goalTempsList[j].txtStatus;
                copy.txtItem.text = goalPersonPrefab.goalTempsList[j].txtItem;
                copy.txtAmount.text = goalPersonPrefab.goalTempsList[j].txtAmount;
                copy.imgCandy.sprite = goalPersonPrefab.goalTempsList[j].imgCandy;
                copy.StrType = goalPersonPrefab.goalTempsList[j].StrType;
                if (goalPersonPrefab.goalTempsList[j].ddTypeOption != null)
                {
                    setImageDropdown(copy.ddType, goalPersonPrefab.goalTempsList[j].ddTypeOption, dictCandySprite);
                    copy.ddType.value = goalPersonPrefab.goalTempsList[j].valueDdType;
                }
            }    
        }    
    }

    public void getDataGoalPersonVip(GoalPersonVipPrefab goalPersonVipPrefab)
    {
        GoalPersonVipPrefab[] goalPersonVipPrefabsList = goalPersonContent.transform.GetComponentsInChildren<GoalPersonVipPrefab>();
        cacheIndexDdListRequire = 0;
        for (int i = 0; i < goalPersonVipPrefabsList.Length; i++)
        {
            if (goalPersonVipPrefabsList[i].gameObject.activeSelf)
            {
                goalPersonVipPrefabsList[i].allowClick = true;
            }
        }
        goEditInformationVip.SetActive(true);
        if (goEditInformation.activeSelf)
        {
            setDataGoalPerson(goalPersonPrefabIsOpen);
            goEditInformation.SetActive(false);
        }
        txtGoalCustomerVip.text = "CustomerVip " + goalPersonVipPrefab.txtAmount.text;
        if (goalPersonVipPrefab.txtCharacter != null)
        {
            ifCharacterVip.text = goalPersonVipPrefab.txtCharacter;
        }
        if (goalPersonVipPrefab.txtGift != null)
        {
            ifGiftVip.text = goalPersonVipPrefab.txtGift;
        }
        if (goalPersonVipPrefab.txtRatio != null)
        {
            ifRatioVip.text = goalPersonVipPrefab.txtRatio;
        }
        if (goalPersonVipPrefab.txtTurnExist != null)
        {
            ifTurnExistVip.text = goalPersonVipPrefab.txtTurnExist;
        }
        if (goalPersonVipPrefab.txtTurnFrom != null)
        {
            ifTurnFromVip.text = goalPersonVipPrefab.txtTurnFrom;
        }
        if (goalPersonVipPrefab.txtTurnTo != null)
        {
            ifTurnToVip.text = goalPersonVipPrefab.txtTurnTo;
        }
        if (goalPersonVipPrefab.txtSkin != null)
        {
            ifSkinVip.text = goalPersonVipPrefab.txtSkin;
        }
        ddListRequire.ClearOptions();
        ddListRequire.AddOptions(goalPersonVipPrefab.listRequirelist);
        this.dictListRequireVip = goalPersonVipPrefab.dictListRequireVip;
        clearTranform(goalContentVip);
        //goalPersonPrefab.goalTempsList = new List<GoalTemp>();

        if (goalPersonVipPrefab.goalTempsList.Count > 0)
        {
            List<GoalTemp> listGoalTemp = new List<GoalTemp>();
            listGoalTemp = goalPersonVipPrefab.goalTempsList[0];
            for (int j = 0; j < listGoalTemp.Count; j++)
            {
                var copy = Instantiate(preGoal);
                copy.transform.SetParent(goalContentVip, false);
                copy.ddType.ClearOptions();

                copy.txtStatus.text = listGoalTemp[j].txtStatus;
                copy.txtItem.text = listGoalTemp[j].txtItem;
                copy.txtAmount.text = listGoalTemp[j].txtAmount;
                copy.imgCandy.sprite = listGoalTemp[j].imgCandy;
                copy.StrType = listGoalTemp[j].StrType;
                if (listGoalTemp[j].ddTypeOption != null)
                {
                    setImageDropdown(copy.ddType, listGoalTemp[j].ddTypeOption, dictCandySprite);
                    copy.ddType.value = listGoalTemp[j].valueDdType;
                }
            }
        }
    }

    public void setDataGoalPerson(GoalPersonPrefab goalPersonPrefab)
    {
        if (goEditInformation.activeSelf)
        {
            goalPersonPrefab.txtCharacter = ifCharacter.text;
            goalPersonPrefab.txtGift = ifGift.text;
            goalPersonPrefab.txtSkin = ifSkin.text;
            //goalPersonPrefab.goalTempsList = new List<GoalTemp>();
            goalPersonPrefab.goalTempsList.Clear();
            GoalPrefab[] goalPrefabList = goalContent.GetComponentsInChildren<GoalPrefab>();
            
            for (int i= 0; i< goalPrefabList.Length; i++)
            {
                GoalTemp goalTemp = new GoalTemp();
                goalTemp.txtAmount = goalPrefabList[i].txtAmount.text;
                goalTemp.txtStatus = goalPrefabList[i].txtStatus.text;
                goalTemp.txtItem = goalPrefabList[i].txtItem.text;
                goalTemp.imgCandy = goalPrefabList[i].imgCandy.sprite;
                goalTemp.StrType = goalPrefabList[i].StrType;
                goalTemp.valueDdType = goalPrefabList[i].ddType.value;
                goalTemp.ddTypeOption = GetOptions(goalPrefabList[i].ddType.options);

                goalPersonPrefab.goalTempsList.Add(goalTemp);

            }    
        }

    }
    public void setDataGoalPersonVip(GoalPersonVipPrefab goalPersonVipPrefab)
    {
        if (goEditInformationVip.activeSelf)
        {
            cacheIndexDdListRequire = ddListRequire.value;
            SaveListRequiredIsOpen();
            goalPersonVipPrefab.txtCharacter = ifCharacterVip.text;
            goalPersonVipPrefab.txtGift = ifGiftVip.text;
            goalPersonVipPrefab.txtRatio = ifRatioVip.text;
            goalPersonVipPrefab.txtTurnExist = ifTurnExistVip.text;
            goalPersonVipPrefab.txtTurnFrom = ifTurnFromVip.text;
            goalPersonVipPrefab.txtTurnTo = ifTurnToVip.text;
            goalPersonVipPrefab.txtSkin = ifSkinVip.text;
            goalPersonVipPrefab.dictListRequireVip = this.dictListRequireVip;
            //goalPersonPrefab.goalTempsList = new List<GoalTemp>();
            goalPersonVipPrefab.goalTempsList.Clear();

            foreach(var key in this.dictListRequireVip.Keys)
            {
                goalPersonVipPrefab.goalTempsList.Add(dictListRequireVip[key]);
            }    

        }

    }

    /// </summary>
    /// <returns></returns>
    public static List<string> GetOptions(List<Dropdown.OptionData> listOption)
    {

        List<string> optionListString = new List<string>();

        for (int i = 0; i < listOption.Count; i++)
        {
            //Dropdown.OptionData dataTemp = new Dropdown.OptionData();
            //dataTemp.text = listOption[i].ToString();
            optionListString.Add(listOption[i].text);
        }

        return optionListString;
    }

    private GoalPersonPrefab goalPersonPrefabTemp = null;
    public bool checkClickOtherGoalPerson(GoalPersonPrefab goalPersonPrefab)
    {
        goalPersonPrefabIsOpen = goalPersonPrefab;
        if(goalPersonPrefabTemp == null)
        {
            goalPersonPrefabTemp = goalPersonPrefab;
        }    
        else if(goalPersonPrefabTemp != goalPersonPrefab)
        {
            setDataGoalPerson(goalPersonPrefabTemp);
            goalPersonPrefabTemp = goalPersonPrefab;
        }    
        else
        {
            setDataGoalPerson(goalPersonPrefab);
        }    


        return true;
    }

    private GoalPersonVipPrefab goalPersonVipPrefabTemp = null;
    public bool checkClickOtherGoalPersonVip(GoalPersonVipPrefab goalPersonVipPrefab)
    {
        goalPersonVipPrefabIsOpen = goalPersonVipPrefab;
        cacheIndexDdListRequire = 0;
        if (goalPersonVipPrefabTemp == null)
        {
            goalPersonVipPrefabTemp = goalPersonVipPrefab;
        }
        else if (goalPersonVipPrefabTemp != goalPersonVipPrefab)
        {

            setDataGoalPersonVip(goalPersonVipPrefabTemp);
            goalPersonVipPrefabTemp = goalPersonVipPrefab;
        }
        else
        {
            setDataGoalPersonVip(goalPersonVipPrefab);
        }


        return true;
    }

    private void onClickAddGoal()
    {
        var copy = Instantiate(preGoal);
        copy.transform.SetParent(goalContent, false);
        copy.ddType.ClearOptions();
        if (ddGoals.value == 0)
        {
            setTextGoal(copy, "Reach", "0", "Score", null);
        }
        else if (ddGoals.value == 1)
        {
            setTextGoal(copy, "Collect", "0", listCandy[0], dictCandySprite[removesapce(listCandy[0])]);
            //copy.ddType.AddOptions(listCandyGoal);
            MainGameController.Instance.setImageDropdown(copy.ddType, listCandyGoal, dictCandySprite);
        }
        else if (ddGoals.value == 2)
        {
            setTextGoal(copy, "Collect", "0", listSpecialCandy[0], dictCandySprite[removesapce(listSpecialCandy[0])]);
            //copy.ddType.AddOptions(listSpecialCandy);
            MainGameController.Instance.setImageDropdown(copy.ddType, listSpecialCandy,dictCandySprite);
        }
        else if (ddGoals.value == 3)
        {
            setTextGoal(copy, "Collect", "0", listElement[0], dictCandySprite[removesapce(listElement[0])]);
            //copy.ddType.AddOptions(listElement);
            MainGameController.Instance.setImageDropdown(copy.ddType, listElement,dictCandySprite);

        }
        else if (ddGoals.value == 4)
        {
            setTextGoal(copy, "Collect", "0", listSpecialBlock[0], dictCandySprite[removesapce(listSpecialBlock[0])]);
            //copy.ddType.AddOptions(listSpecialBlock);
            MainGameController.Instance.setImageDropdown(copy.ddType, listSpecialBlock,dictCandySprite);

        }
        else if (ddGoals.value == 5)
        {
            setTextGoal(copy, "Collect", "0", listMultiBlock[0], dictCandySprite[removesapce(listMultiBlock[0])]);
            //copy.ddType.AddOptions(listSpecialBlock);
            MainGameController.Instance.setImageDropdown(copy.ddType, listMultiBlock, dictCandySprite);

        }
        else if (ddGoals.value == 6)
        {
            setTextGoal(copy, "Collect", "0", listCollectable[0], dictCandySprite[removesapce(listCollectable[0])]);
            //copy.ddType.AddOptions(listCollectable);
            MainGameController.Instance.setImageDropdown(copy.ddType, listCollectable,dictCandySprite);

        }
        else if (ddGoals.value == 7)
        {
            setTextGoal(copy, "Destroy", "", "all chocolate", dictCandySprite["Chocolate"]);
            copy.btnGoalItem.interactable = false;
        }
        copy.StrType = ddGoals.options[ddGoals.value].text;
    }
    private void onClickAddGoalVip()
    {
        var copy = Instantiate(preGoal);
        copy.transform.SetParent(goalContentVip, false);
        copy.ddType.ClearOptions();
        if (ddGoalsVip.value == 0)
        {
            setTextGoal(copy, "Reach", "0", "Score", null);
        }
        else if (ddGoalsVip.value == 1)
        {
            setTextGoal(copy, "Collect", "0", listCandy[0], dictCandySprite[removesapce(listCandy[0])]);
            //copy.ddType.AddOptions(listCandyGoal);
            MainGameController.Instance.setImageDropdown(copy.ddType, listCandyGoal, dictCandySprite);
        }
        else if (ddGoalsVip.value == 2)
        {
            setTextGoal(copy, "Collect", "0", listSpecialCandy[0], dictCandySprite[removesapce(listSpecialCandy[0])]);
            //copy.ddType.AddOptions(listSpecialCandy);
            MainGameController.Instance.setImageDropdown(copy.ddType, listSpecialCandy, dictCandySprite);
        }
        else if (ddGoalsVip.value == 3)
        {
            setTextGoal(copy, "Collect", "0", listElement[0], dictCandySprite[removesapce(listElement[0])]);
            //copy.ddType.AddOptions(listElement);
            MainGameController.Instance.setImageDropdown(copy.ddType, listElement, dictCandySprite);

        }
        else if (ddGoalsVip.value == 4)
        {
            setTextGoal(copy, "Collect", "0", listSpecialBlock[0], dictCandySprite[removesapce(listSpecialBlock[0])]);
            //copy.ddType.AddOptions(listSpecialBlock);
            MainGameController.Instance.setImageDropdown(copy.ddType, listSpecialBlock, dictCandySprite);

        }
        else if (ddGoalsVip.value == 5)
        {
            setTextGoal(copy, "Collect", "0", listMultiBlock[0], dictCandySprite[removesapce(listMultiBlock[0])]);
            //copy.ddType.AddOptions(listSpecialBlock);
            MainGameController.Instance.setImageDropdown(copy.ddType, listMultiBlock, dictCandySprite);

        }
        else if (ddGoalsVip.value == 6)
        {
            setTextGoal(copy, "Collect", "0", listCollectable[0], dictCandySprite[removesapce(listCollectable[0])]);
            //copy.ddType.AddOptions(listCollectable);
            MainGameController.Instance.setImageDropdown(copy.ddType, listCollectable, dictCandySprite);

        }
        else if (ddGoalsVip.value == 7)
        {
            setTextGoal(copy, "Destroy", "", "all chocolate", dictCandySprite["Chocolate"]);
            copy.btnGoalItem.interactable = false;
        }
        copy.StrType = ddGoalsVip.options[ddGoalsVip.value].text;
    }
    public void onCheckAwardAndTypeActive(int indexLimitType)
    {
        if (indexLimitType == 0)
        {
            tgAwardSpeacialCandy.transform.parent.gameObject.SetActive(true);
            if (tgAwardSpeacialCandy.isOn == true)
            {
                ddType.transform.parent.gameObject.SetActive(true);
            }
            else ddType.transform.parent.gameObject.SetActive(false);
        }
        else
        {
            tgAwardSpeacialCandy.transform.parent.gameObject.SetActive(false);
            ddType.transform.parent.gameObject.SetActive(false);
        }
        txtMoves.text = ddLimitType.options[ddLimitType.value].text;

    }

    public void onValueChangeTypeGoal(int indexType)
    {
        if (indexType == 0) imgTypeGoal.sprite = dictCandySprite["BlueCandyHorizontalStriped"];
        else if( indexType == 1) imgTypeGoal.sprite = dictCandySprite["BlueCandyWrapped"];
    }    
    public void setTextGoal(GoalPrefab goalPre, string status, string amount, string item, Sprite spriteCandy)
    {
        goalPre.txtStatus.text = status;
        goalPre.txtAmount.text = amount;
        goalPre.txtItem.text = item;
        goalPre.imgCandy.sprite = spriteCandy;
    }

    public void onValueChangeTgAwardCandy (bool checkAwardCandy)
    {
        if (checkAwardCandy && tgAwardSpeacialCandy.transform.parent.gameObject.activeSelf)
        {
            ddType.transform.parent.gameObject.SetActive(true);
        }
        else ddType.transform.parent.gameObject.SetActive(false);

    }

    #endregion

    #region Level

    private void generateMap()
    {
        width = 0;
        height = 0;

       if (ifWidth.text != null && ifHeight.text != null)
       {
            int.TryParse(ifWidth.text, out width);
            int.TryParse(ifHeight.text, out height);

            clearTranform(levelContent.transform);
            levelContent.transform.localScale = new Vector3(1f, 1f, 1f);
            txtScaleMap.text = "1.1x";
            sdScaleMap.value = 1.1f;


            if (width > 0 && height > 0)
            {
                int count = width * height;

                float posX = 0;
                float posY = 0;
                float a = candyPrefab.GetComponent<RectTransform>().rect.width;
                for (int i = 0; i < height; i++)
                {
                    posY = height * a * 1.0f / 2 - (i + 0.5f) * a * 1.0f;
                    for (int j = 0; j < width; j++)
                    {
                        posX = (j + 0.5f) * a * 1.0f - width * a * 1.0f / 2;

                        var copy = Instantiate(candyPrefab);
                        copy.transform.SetParent(levelContent.transform, false);
                        copy.transform.localPosition = new Vector3(posX, posY, 0f);
                        copy.initData((j), (i));
                    }
                }

                widthContent = 50f * width;
                heightContent = 50f * height;
                levelContent.GetComponent<RectTransform>().sizeDelta = new Vector2(widthContent, heightContent);
            }

        }   
    }      
    
    public void setDataDropdownLevel(int indexBrushType)
    {
        ddCandy.ClearOptions();
        if(indexBrushType >= 0 && indexBrushType <5) ddCandy.gameObject.SetActive(true);


        goEditLevelMap.SetActive(false);
        hideImageCandy(false);
        if (indexBrushType == 0)
        {
            //ddCandy.AddOptions(listCandy);
            setImageDropdown(ddCandy, listCandy, dictCandySprite);
        }
        else if (indexBrushType == 1)
        {
            //ddCandy.AddOptions(listElement);
            setImageDropdown(ddCandy, listElement,dictCandySprite);
        }
        else if (indexBrushType == 2)
        {
            //ddCandy.AddOptions(listSpecialCandy);
            setImageDropdown(ddCandy, listSpecialCandy,dictCandySprite);
        }
        else if (indexBrushType == 3)
        {
            //ddCandy.AddOptions(listSpecialBlock);
            setImageDropdown(ddCandy, listSpecialBlock,dictCandySprite);
        }
        else if (indexBrushType == 4)
        {
            //ddCandy.AddOptions(listSpecialBlock);
            setImageDropdown(ddCandy, listMultiBlock, dictCandySprite);
        }
        else if (indexBrushType == 5)
        {
            //ddCandy.AddOptions(listCollectable);
            setImageDropdown(ddCandy, listCollectable,dictCandySprite);
        }
        else if (indexBrushType == 6 || indexBrushType == 7 || indexBrushType == 8)
        {
            ddCandy.gameObject.SetActive(false);
        }
        else if (indexBrushType == 9 )
        {
            goEditLevelMap.SetActive(true);
            ddCandy.gameObject.SetActive(false);
            tgShowCandy.isOn = true;
            BgMapTIlePrefab[] bgMapTIlePrefabsList = EditLevelMapContent.transform.GetComponentsInChildren<BgMapTIlePrefab>();

            if (bgMapTIlePrefabsList.Length == 0)
            {
                for (int i = 0; i < listMap.Count; i++)
                {
                    var bgMapTileIns = Instantiate(bgMapTIlePrefab);
                    bgMapTileIns.transform.SetParent(EditLevelMapContent.transform, false);
                    bgMapTileIns.intitData(dictMapSprite[(i + 1).ToString()]);
                    bgMapTileIns.name = (i + 1).ToString();

                }
            }
        }    
    }
    public void setImageDropdown(Dropdown ddCandy, List<string> listCandyType, Dictionary<string , Sprite> dictSprite)
    {
        List<Dropdown.OptionData> candyItem = new List<Dropdown.OptionData>();
        foreach(string candyType in listCandyType)
        {
            var candyOption = new Dropdown.OptionData(candyType, dictSprite[removesapce(candyType)]);
            candyItem.Add(candyOption);
        }
        ddCandy.AddOptions(candyItem);
    }

    public Sprite getSpriteCandyOnMap(CandyPrefab candyItem)
    {
        if(ddBrushType.options[ddBrushType.value].text != "Element")
        candyItem.StrBrushType = ddBrushType.options[ddBrushType.value].text;

        if (candyNameDropDown == "Empty")
        {
            return null;
        }

        if (ddBrushType.value == 1)
        {
            if (candyItem.ImgCandy.gameObject.activeSelf == false) return null;
            candyNameDropDown = removesapce(candyItem.ImgCandy.sprite.name);
        }

        if (candyNameDropDown == "Hole")
        {
            return dictCandySprite[candyNameDropDown];
        }
        if (candyNameDropDown == "Through")
        {
            return dictCandySprite[candyNameDropDown];
        }

        candyNameDropDown = removesapce(ddCandy.options[ddCandy.value].text);

        if (listElement.Contains(candyNameDropDown))
        {
            string[] temp = candyItem.ImgCandy.sprite.name.Split("_");
            if (candyNameDropDown == "None")
            {
                candyNameDropDown = temp[0];
            }
            else if (listSpecialBlock.Contains(candyItem.ImgCandy.sprite.name))
            {
                candyNameDropDown = candyItem.ImgCandy.sprite.name;
            }
            else
            {
                candyNameDropDown = temp[0] + "_" + candyNameDropDown;
            }
                
        }
        Debug.Log("====  " + candyNameDropDown);
        return dictCandySprite[candyNameDropDown];
    }

    public void getNameCandySprite(int indexCandy)
    {
        candyNameDropDown = ddCandy.options[indexCandy].text;
        candyNameDropDown = removesapce(candyNameDropDown);
    }

    public void getNameBrushType(int indexBrushType)
    {
        candyNameDropDown = null;
        if (ddBrushType.options[indexBrushType].text == "Hole")
        {
            candyNameDropDown = "Hole";
        }
        else if (ddBrushType.options[indexBrushType].text == "Empty")
        {
            candyNameDropDown = "Empty";
        }
        else if (ddBrushType.options[indexBrushType].text == "Through")
        {
            candyNameDropDown = "Through";
        }
    }

    public void onValueChangeScaleMap(float value)
    {
        levelContent.gameObject.transform.localScale = new Vector3(value, value, value);
        txtScaleMap.text = Math.Round(value, 1).ToString()+"x";
    }    

    public void getEventInBrushMode(int indexEvent, int indexColumn, int indexRow, CandyPrefab candyItem)
    {
        if (indexEvent == 1) DrawRow(indexRow, candyItem);
        else if (indexEvent == 2) DrawColumn(indexColumn, candyItem);
        else if (indexEvent == 3) DrawFill(candyItem);
    }    

    private void DrawColumn(int indexColumn, CandyPrefab candyItem)
    {
        CandyPrefab[] candyList = levelContent.transform.GetComponentsInChildren<CandyPrefab>();
        for (int i = 0; i<candyList.Length; i++)
        {
            int temp = i;
            if((temp+1) % int.Parse(ifWidth.text) == indexColumn)
            {
                drawBrushMode(candyList, candyItem, i);
            }    
        }    

    }
    private void DrawRow(int indexRow, CandyPrefab candyItem)
    {
        string[] tempCandyName = candyItem.ImgCandy.sprite.name.Split("_");

        CandyPrefab[] candyList = levelContent.transform.GetComponentsInChildren<CandyPrefab>();
        for (int i = 0; i < candyList.Length; i++)
        {
            int temp = i;
            if ((temp) / int.Parse(ifWidth.text) == indexRow)
            {
                drawBrushMode(candyList, candyItem, i);
            }               
        }
    }
    private void DrawFill(CandyPrefab candyItem)
    {
        CandyPrefab[] candyList = levelContent.transform.GetComponentsInChildren<CandyPrefab>();
        for (int i = 0; i < candyList.Length; i++)
        {
            drawBrushMode(candyList, candyItem, i);
        }
    }

    private void drawBrushMode(CandyPrefab[] candyList, CandyPrefab candyItem, int i)
    {
        if (ddBrushType.value == 1)
        {

            if (ddCandy.value == 0)
            {
                string[] spriteCandyName = candyList[i].ImgCandy.sprite.name.Split("_");
                string tempCandy = spriteCandyName[0];
                candyList[i].ImgCandy.sprite = dictCandySprite[tempCandy];
            }
            else
            {
                if (candyList[i].StrBrushType == "Hole" || candyList[i].StrBrushType == "Through")
                {
                    candyList[i].ImgCandy.sprite = dictCandySprite[candyList[i].StrBrushType];
                }
                else if (candyList[i].StrBrushType != "Special Block" && candyList[i].StrBrushType != "Empty")
                {
                    string[] spriteCandyName = candyList[i].ImgCandy.sprite.name.Split("_");
                    string tempCandy = spriteCandyName[0] + "_" + ddCandy.options[ddCandy.value].text;
                    candyList[i].ImgCandy.sprite = dictCandySprite[tempCandy];
                }
            }
        }
        else
        {
            if (candyItem.ImgCandy.gameObject.activeSelf == true)
            {
                candyList[i].ImgCandy.gameObject.SetActive(true);
                candyList[i].ImgCandy.sprite = candyItem.ImgCandy.sprite;
                candyList[i].StrBrushType = candyItem.StrBrushType;
            }
            else
            {
                candyList[i].ImgCandy.gameObject.SetActive(false);
                candyList[i].StrBrushType = "Empty";
            }
        }
    }

    private void onClickAutoMapByCode()
    {
        // string data = "{\"data\":["+"1,1,1,1,1,1,0,0,1,1"+"]}";

        if (ifWidth.text != null && ifHeight.text != null)
        {
            int.TryParse(ifWidth.text, out width);
            int.TryParse(ifHeight.text, out height);

            if(width > 0 && height >0)
            {
                if (ifCode.text == null || ifCode.text == "") return;
                string dataTemp = "{\"data\":[" + ifCode.text + "]}";
                Dictionary<string, object> data = MiniJSON.Json.Deserialize(dataTemp) as Dictionary<string, object>;
                List<object> listCode = data.GetList("data");

                CandyPrefab[] candyList = levelContent.transform.GetComponentsInChildren<CandyPrefab>();

                if (candyList.Length != listCode.Count)
                {
                    Toast.ShowMessage("Width x Height not equal Amount in Code", Toast.Position.top, Toast.Time.twoSecond);
                }
                else
                {

                    for (int i = 0; i < candyList.Length; i++)
                    {
                        if (listCode[i].ToInt() == 1)
                        {
                            candyList[i].setCandy("Candy", 2, dictMapSprite["2"], dictCandySprite["RandomCandy"]);
                        }
                        else if (listCode[i].ToInt() == 0)
                        {
                            candyList[i].setCandy("Hole", 30, dictMapSprite["30"], dictCandySprite["Hole"]);
                        }
                    }
                }


            }  
            else
            {
                Toast.ShowMessage("** Type Width or Height **", Toast.Position.top, Toast.Time.twoSecond);
            }   
        }        

    }
    private void onClickAutoMapByJson()
    {
        if(!ifJson.text.IsNullOrEmpty())
        {
            string textJson = ifJson.text;
            currentLevel = LoadJsonText<Level>(textJson);
            loadLevelData(this.currentLevel);

            
        }    
    }    

    #endregion Level

    #region EditMapLevel
    public void hideChooseEditBgMapTile()
    {
        BgMapTIlePrefab[] bgMapTIlePrefabsList = EditLevelMapContent.transform.GetComponentsInChildren<BgMapTIlePrefab>();
        
        for(int i = 0; i< bgMapTIlePrefabsList.Length; i++)
        {
            bgMapTIlePrefabsList[i].imgChoose.gameObject.SetActive(false);
        }    
    }  
    
    public void hideImageCandy(bool isHide)
    {
        CandyPrefab[] candyPrefabsList = levelContent.transform.GetComponentsInChildren<CandyPrefab>();

        for(int i = 0; i<candyPrefabsList.Length; i++)
        {
            if(isHide)
            {
                candyPrefabsList[i].ImgCandy.gameObject.SetActive(false);
            }   
            else
            {
                if(candyPrefabsList[i].StrBrushType == "Empty")
                {
                    candyPrefabsList[i].ImgCandy.gameObject.SetActive(false);
                }   
                else
                candyPrefabsList[i].ImgCandy.gameObject.SetActive(true);
            }    
        }    
    }  
    
    public void onValueChangeTgShowCandy_editor(bool value)
    {
        if (value)
        {
            hideImageCandy(false);
        }
        else hideImageCandy(true);

    }   
    public int getSpriteBgMap(CandyPrefab candyItem)
    {
        if((candyItem.StrBrushType == "Hole" || candyItem.StrBrushType == "Through") && !ListidBgMapForHole.Contains(IdBgMapChoose))
        {
            return -1;
        }
        else if( !((candyItem.StrBrushType == "Hole")|| candyItem.StrBrushType == "Through") && !ListidBgMap.Contains(IdBgMapChoose))
        {
            return -2;
        }

        return IdBgMapChoose;
    }  
    
    private void checkBgMapAuto()
    {
        CandyPrefab[] candyPrefabsList = levelContent.transform.GetComponentsInChildren<CandyPrefab>();

        for(int i = 0; i< candyPrefabsList.Length; i++)
        {
            if(candyPrefabsList[i].StrBrushType == "Hole" || candyPrefabsList[i].StrBrushType == "Through")
            {
                (int, int, int ,int) result = getListForCandy(candyPrefabsList[i]);               
                candyPrefabsList[i].ImgBgTile.sprite = getBgMapForHole(result.Item1, result.Item2, result.Item3, result.Item4);
                Debug.Log("Hole :" + (i + 1) + " : left left :" + result.Item1 + "  up: " + result.Item2 + "  right : " + result.Item3 + "  down : " + result.Item4);
                candyPrefabsList[i].idBgMap = int.Parse(candyPrefabsList[i].ImgBgTile.sprite.name);
            }   
            else
            {
                (int, int, int, int) result = getListForCandy(candyPrefabsList[i]);
                int caseSpecialIndex = checkCaseSpecial(result.Item1, result.Item2, result.Item3, result.Item4);
                if (caseSpecialIndex > 0)
                {
                    (int, int, int, int) resultSpecial = getListSpecialForCandy(candyPrefabsList[i]);
                    Debug.Log("Candy " + (i + 1) + " : leftUp :" + resultSpecial.Item1 + "  rightUp: " + resultSpecial.Item2 + "  rightDown : " + resultSpecial.Item3 + "  leftDown : " + resultSpecial.Item4);

                    candyPrefabsList[i].ImgBgTile.sprite = getSpecialBgMap(resultSpecial.Item1, resultSpecial.Item2, resultSpecial.Item3, resultSpecial.Item4, caseSpecialIndex);
                    candyPrefabsList[i].idBgMap = int.Parse(candyPrefabsList[i].ImgBgTile.sprite.name);
                }
                else
                {
                    candyPrefabsList[i].ImgBgTile.sprite = getBgMapForCandy(result.Item1, result.Item2, result.Item3, result.Item4);
                    Debug.Log("Candy " + (i + 1) + " : left :" + result.Item1 + "  up: " + result.Item2 + "  right : " + result.Item3 + "  down : " + result.Item4);
                    candyPrefabsList[i].idBgMap = int.Parse(candyPrefabsList[i].ImgBgTile.sprite.name);
                }
            }    
        }    
    }

    private int checkCaseSpecial(int left, int up, int right, int down)
    {
        if (left == 1 && up == 0 && right == 0 && down == 1)
        {
            return 1;
        }
        else if (left == 1 && up == 1 && right == 0 && down == 0)
        {
            return 2;
        }
        else if (left == 0 && up == 0 && right == 1 && down == 1)
        {
            return 3;
        }
        else if (left == 0 && up == 1 && right == 1 && down == 0)
        {
            return 4;
        }
        return -1;
    }
    private (int, int, int, int) getListSpecialForCandy(CandyPrefab candyPrefab)
    {
        leftUp = leftDown = rightDown = rightUp = 0;

        CandyPrefab[] candyPrefabList = levelContent.transform.GetComponentsInChildren<CandyPrefab>();
        int indexCurrentNode = candyPrefab.transform.GetSiblingIndex();
        if (candyPrefab.x - 1 >= 0)
        {
            // Left Down
            if (candyPrefab.y - 1 >= 0)
            {
                if (candyPrefabList[indexCurrentNode - width- 1].StrBrushType != "Hole" && candyPrefabList[indexCurrentNode - width - 1].StrBrushType != "Through")
                    leftUp= 1;
            }
            // Left Up
            if (candyPrefab.y + 1 < height)
            {
                if (candyPrefabList[indexCurrentNode + width - 1].StrBrushType != "Hole" && candyPrefabList[indexCurrentNode + width - 1].StrBrushType != "Through")
                    leftDown = 1;
            }
        }


        if (candyPrefab.x + 1 < width)
        {

            // Right Down
            if (candyPrefab.y - 1 >= 0)
            {
                if (candyPrefabList[indexCurrentNode - width + 1].StrBrushType != "Hole" && candyPrefabList[indexCurrentNode - width + 1].StrBrushType != "Through")
                    rightUp = 1;
            }
            // Right Up
            if (candyPrefab.y + 1 < height)
            {
                if (candyPrefabList[indexCurrentNode + width + 1].StrBrushType != "Hole" && candyPrefabList[indexCurrentNode + width + 1].StrBrushType != "Through")
                    rightDown = 1;
            }
        }
        return (leftUp, rightUp, rightDown, leftDown);
    }
    private Sprite getSpecialBgMap(int leftUp, int rightUp, int rightDown, int leftDown, int indexCase)
    {
        if(indexCase == 1)
        {
            if(rightUp == 1)
            {
                return dictMapSprite["17"];
            }   
            else
            {
                return dictMapSprite["5"];
            }    
        }
        else if (indexCase == 2)
        {
            if (rightDown == 1)
            {
                return dictMapSprite["18"];
            }
            else
            {
                return dictMapSprite["7"];
            }
        }
        else if (indexCase == 3)
        {
            if (leftUp == 1)
            {
                return dictMapSprite["16"];
            }
            else
            {
                return dictMapSprite["4"];
            }
        }
        else if (indexCase == 4)
        {
            if (leftDown == 1)
            {
                return dictMapSprite["19"];
            }
            else
            {
                return dictMapSprite["6"];
            }
        }
        return dictMapSprite["6"];
    }    

    private (int, int, int, int) getListForCandy(CandyPrefab candyPrefab)
    {
        up = left = right = down = 0;
        CandyPrefab[] candyPrefabList = levelContent.transform.GetComponentsInChildren<CandyPrefab>();
        int indexCurrentNode = candyPrefab.transform.GetSiblingIndex();

        if (candyPrefab.x - 1 >= 0)
        {
            // Left
            if (candyPrefabList[indexCurrentNode - 1].StrBrushType != "Hole" && candyPrefabList[indexCurrentNode - 1].StrBrushType != "Through")
                left = 1;
            // Left Down
            //if (currentNode.y - 1 >= 0) neighbourList.Add(mapTileList[indexCurrentNode - width - 1]);
            // Left Up
            //if (currentNode.y + 1 < height) neighbourList.Add(mapTileList[indexCurrentNode + width - 1]);
        }
        // Up
        if (candyPrefab.y + 1 < height)
        {
            if (candyPrefabList[indexCurrentNode + width].StrBrushType != "Hole" && candyPrefabList[indexCurrentNode + width].StrBrushType != "Through")
                down = 1;
        } 


        if (candyPrefab.x + 1 < width)
        {
            // Right
            if (candyPrefabList[indexCurrentNode + 1].StrBrushType != "Hole" && candyPrefabList[indexCurrentNode + 1].StrBrushType != "Through")
                right = 1;
            // Right Down
            //if (currentNode.y - 1 >= 0) neighbourList.Add(mapTileList[indexCurrentNode - width + 1]);
            // Right Up
            //if (currentNode.y + 1 < height) neighbourList.Add(mapTileList[indexCurrentNode + width + 1]);
        }
        // Down
        if (candyPrefab.y - 1 >= 0)
        {
            if (candyPrefabList[indexCurrentNode - width].StrBrushType != "Hole" && candyPrefabList[indexCurrentNode - width].StrBrushType != "Through")
                up = 1; 
        }

        return (left, up, right, down);
    }

/*    private (int, int, int, int) getListForHole(CandyPrefab candyPrefab)
    {
        up = left = right = down = 0;
        if (candyPrefab.x - 1 >= 0)
        {
            // Left
            if (candyPrefab.StrBrushType != "Hole")
                left = 1;
            // Left Down
            //if (currentNode.y - 1 >= 0) neighbourList.Add(mapTileList[indexCurrentNode - width - 1]);
            // Left Up
            //if (currentNode.y + 1 < height) neighbourList.Add(mapTileList[indexCurrentNode + width - 1]);
        }
        // Up
        if (candyPrefab.y + 1 < height)
        {
            if (candyPrefab.StrBrushType != "Hole")
                up = 1;
        } 


        if (candyPrefab.x + 1 < width)
        {
            // Right
            if (candyPrefab.StrBrushType != "Hole")
                right = 1;
            // Right Down
            //if (currentNode.y - 1 >= 0) neighbourList.Add(mapTileList[indexCurrentNode - width + 1]);
            // Right Up
            //if (currentNode.y + 1 < height) neighbourList.Add(mapTileList[indexCurrentNode + width + 1]);
        }
        // Down
        if (candyPrefab.y - 1 >= 0)
        {
            if (candyPrefab.StrBrushType != "Hole")
                down = 1;
        }

        return (left, up, right, down);
    }*/

    private Sprite getBgMapForHole(int left, int up, int right, int down)
    {
        if(left == 0 && up == 0 && right == 0 && down == 0 )
        {
            return dictMapSprite["1"];
        }
        else if (left == 0 && up == 0 && right == 0 && down == 1)
        {
            return dictMapSprite["1"];
        }
        else if (left == 0 && up == 0 && right == 1 && down == 0)
        {
            return dictMapSprite["1"];
        }
        else if (left == 0 && up == 0 && right == 1 && down == 1)
        {
            return dictMapSprite["27"];
        }
        else if (left == 0 && up == 1 && right == 0 && down == 0)
        {
            return dictMapSprite["1"];
        }
        else if (left == 0 && up == 1 && right == 0 && down == 1)
        {
            return dictMapSprite["1"];
        }
        else if (left == 0 && up == 1 && right == 1 && down == 0)
        {
            return dictMapSprite["28"];
        }
        else if (left == 0 && up == 1 && right == 1 && down == 1)
        {
            return dictMapSprite["23"];
        }
        else if (left == 1 && up == 0 && right == 0 && down == 0)
        {
            return dictMapSprite["1"];
        }
        else if (left == 1 && up == 0 && right == 0 && down == 1)
        {
            return dictMapSprite["26"];
        }
        else if (left == 1 && up == 0 && right == 1 && down == 0)
        {
            return dictMapSprite["1"];
        }
        else if (left == 1 && up == 0 && right == 1 && down == 1)
        {
            return dictMapSprite["22"];
        }
        else if (left == 1 && up == 1 && right == 0 && down == 0)
        {
            return dictMapSprite["29"];
        }
        else if (left == 1 && up == 1 && right == 0 && down == 1)
        {
            return dictMapSprite["25"];
        }
        else if (left == 1 && up == 1 && right == 1 && down == 0)
        {
            return dictMapSprite["24"];
        }
        else if (left == 1 && up == 1 && right == 1 && down == 1)
        {
            return dictMapSprite["30"];
        }
        return dictMapSprite["1"];
    }
    private Sprite getBgMapForCandy(int left, int up, int right, int down)
    {
        if (left == 0 && up == 0 && right == 0 && down == 0)
        {
            return dictMapSprite["3"];
        }
        else if (left == 0 && up == 0 && right == 0 && down == 1)
        {
            return dictMapSprite["8"];
        }
        else if (left == 0 && up == 0 && right == 1 && down == 0)
        {
            return dictMapSprite["11"];
        }
        else if (left == 0 && up == 0 && right == 1 && down == 1)
        {
            return dictMapSprite["16"];
        }
        else if (left == 0 && up == 1 && right == 0 && down == 0)
        {
            return dictMapSprite["10"];
        }
        else if (left == 0 && up == 1 && right == 0 && down == 1)
        {
            return dictMapSprite["20"];
        }
        else if (left == 0 && up == 1 && right == 1 && down == 0)
        {
            return dictMapSprite["19"];
        }
        else if (left == 0 && up == 1 && right == 1 && down == 1)
        {
            return dictMapSprite["15"];
        }
        else if (left == 1 && up == 0 && right == 0 && down == 0)
        {
            return dictMapSprite["9"];
        }
        else if (left == 1 && up == 0 && right == 0 && down == 1)
        {
            return dictMapSprite["17"];
        }
        else if (left == 1 && up == 0 && right == 1 && down == 0)
        {
            return dictMapSprite["21"];
        }
        else if (left == 1 && up == 0 && right == 1 && down == 1)
        {
            return dictMapSprite["12"];
        }
        else if (left == 1 && up == 1 && right == 0 && down == 0)
        {
            return dictMapSprite["18"];
        }
        else if (left == 1 && up == 1 && right == 0 && down == 1)
        {
            return dictMapSprite["13"];
        }
        else if (left == 1 && up == 1 && right == 1 && down == 0)
        {
            return dictMapSprite["14"];
        }
        else if (left == 1 && up == 1 && right == 1 && down == 1)
        {
            return dictMapSprite["2"];
        }
        return dictMapSprite["2"];
    }

    #endregion

    #region Boosters
    private List<BoosterResource> getListResourceBooster(BoosterPrefab boosterPrefab)
    {
        resourceList = new List<BoosterResource>();
        if(boosterPrefab.checkIsRemove)
        {
            if (boosterPrefab.ifEnergy.text != "" && int.Parse(boosterPrefab.ifEnergy.text) >= 0)
            {
                BoosterResource boosterResource3 = new BoosterResource();
                boosterResource3.id = 3;
                boosterResource3.q = int.Parse(boosterPrefab.ifEnergy.text);
                resourceList.Add(boosterResource3);
            }
            if (boosterPrefab.ifCoin.text != "" && int.Parse(boosterPrefab.ifCoin.text) >= 0)
            {
                BoosterResource boosterResource1 = new BoosterResource();
                boosterResource1.id = 1;
                boosterResource1.q = int.Parse(boosterPrefab.ifCoin.text);
                resourceList.Add(boosterResource1);
            }
            if (boosterPrefab.ifGem.text != "" && int.Parse(boosterPrefab.ifGem.text) >= 0)
            {
                BoosterResource boosterResource2 = new BoosterResource();
                boosterResource2.id = 2;
                boosterResource2.q = int.Parse(boosterPrefab.ifGem.text);
                resourceList.Add(boosterResource2);
            }
            return resourceList;
        }
        return null;

    }    

    #endregion

    #region SaveData
    private void saveLevelData()
    {
        currentLevel.id = int.Parse(ifLevelNumber.text);
        currentLevel.width = int.Parse(ifWidth.text);
        currentLevel.height = int.Parse(ifHeight.text);

        saveTilesObject();
        saveIdBgObject();
        currentLevel.limitType = (LimitType)Enum.Parse(typeof(LimitType), ddLimitType.options[ddLimitType.value].text);
        currentLevel.limit = int.Parse(ifMoves.text);
        currentLevel.limit_booster = int.Parse(ifLimitBooster.text);
        //saveGoalObject();
        saveGoalNewObject();
        saveCustomerVipNewObject();
        //saveAvailableColorsObject();
        saveColorNewObject();
        saveFailingHoleNewObject();
        currentLevel.score1 = int.Parse(ifStar1Score.text);
        currentLevel.score2 = int.Parse(ifStar2Score.text);
        currentLevel.score3 = int.Parse(ifStar3Score.text);
        currentLevel.awardSpecialCandies = tgAwardSpeacialCandy.isOn;
        currentLevel.awardedSpecialCandyType = (AwardedSpecialCandyType)Enum.Parse(typeof(AwardedSpecialCandyType), ddType.options[ddType.value].text);
        currentLevel.collectableChance = int.Parse(ifCollectableChance.text);
        //SaveAvailableBoostersObject();
        //saveAvailableBoostersNewObject();
        currentLevel.availableBoosters = new Dictionary<BoosterType, List<BoosterResource>>();
        saveAvailableBoosters_v2();

    }

    private void saveTilesObject()
    {   
        CandyPrefab[] candyList = levelContent.transform.GetComponentsInChildren<CandyPrefab>();
        currentLevel.tiles = new List<LevelTile>(candyList.Length);

        for (int i = 0; i < candyList.Length; i++)
        {
            string[] spriteName = candyList[i].ImgCandy.sprite.name.Split("_");

            string txtBrushType = candyList[i].StrBrushType;
            if (txtBrushType == "Candy")
            {
                CandyTile candyTile = new CandyTile() { type = (CandyType)Enum.Parse(typeof(CandyType), spriteName[0]) };

                if (spriteName.Length > 1)
                    candyTile.elementType = (ElementType)Enum.Parse(typeof(ElementType), spriteName[1]);
                else
                {
                    candyTile.elementType = ElementType.None;
                }

                currentLevel.tiles.Add(candyTile);
            }
            else if (txtBrushType == "Special Candy")
            {
                SpecialCandyTile specialCandyTile = new SpecialCandyTile() { type = (SpecialCandyType)Enum.Parse(typeof(SpecialCandyType), spriteName[0]) };

                if (spriteName.Length > 1)
                    specialCandyTile.elementType = (ElementType)Enum.Parse(typeof(ElementType), spriteName[1]);
                else
                {
                    specialCandyTile.elementType = ElementType.None;
                }

                currentLevel.tiles.Add(specialCandyTile);
            }
            else if (txtBrushType == "Special Block")
            {
                SpecialBlockTile specialBlockTile = new SpecialBlockTile() { type = (SpecialBlockType)Enum.Parse(typeof(SpecialBlockType), spriteName[0]) };

                if (spriteName.Length > 1)
                    specialBlockTile.elementType = (ElementType)Enum.Parse(typeof(ElementType), spriteName[1]);
                else
                {
                    specialBlockTile.elementType = ElementType.None;
                }

                currentLevel.tiles.Add(specialBlockTile);
            }
            else if (txtBrushType == "Multi Block")
            {
                MultiBlockLevelTile multiBlockTile = new MultiBlockLevelTile() { type = (MultiBlockType)Enum.Parse(typeof(MultiBlockType), spriteName[0]) };

                if (spriteName.Length > 1)
                    multiBlockTile.elementType = (ElementType)Enum.Parse(typeof(ElementType), spriteName[1]);
                else
                {
                    multiBlockTile.elementType = ElementType.None;
                }
                multiBlockTile.id = candyList[i].idCakeBox;

                currentLevel.tiles.Add(multiBlockTile);
            }    
            else if (txtBrushType == "Collectable")
            {
                CollectableTile collectableTile = new CollectableTile() { type = (CollectableType)Enum.Parse(typeof(CollectableType), spriteName[0]) };

                if (spriteName.Length > 1)
                    collectableTile.elementType = (ElementType)Enum.Parse(typeof(ElementType), spriteName[1]);
                else
                {
                    collectableTile.elementType = ElementType.None;
                }

                currentLevel.tiles.Add(collectableTile);
            }
            else if (txtBrushType == "Hole")
            {
                HoleTile holeTile = new HoleTile() { };
                holeTile.elementType = ElementType.None;
                currentLevel.tiles.Add(holeTile);

            }
            else if (txtBrushType == "Through")
            {
                ThroughTile throughTile = new ThroughTile() { };
                throughTile.elementType = ElementType.None;
                currentLevel.tiles.Add(throughTile);

            }
            else if (txtBrushType == "Empty")
            {
                currentLevel.tiles.Add(null);

            }
        }
    } 
    
    private void saveIdBgObject()
    {
        CandyPrefab[] candyList = levelContent.transform.GetComponentsInChildren<CandyPrefab>();
        currentLevel.idBackground = new List<int>();
        for(int i= 0; i<candyList.Length; i++)
        {
            currentLevel.idBackground.Add(candyList[i].idBgMap);
        }    
    }

    /*private void saveGoalObject()
    {
        currentLevel.goals = new List<Goal>(width * height);

        GoalPrefab[] goalList = goalContent.transform.GetComponentsInChildren<GoalPrefab>();


        for(int i=0; i< goalList.Length; i++)
        {
            string typeGoal = goalList[i].StrType;
            string[] txtItem = goalList[i].txtItem.text.Split(" ");
            if (typeGoal == "Reach score goal")
            {
                ReachScoreGoal reachScoreGoal = new ReachScoreGoal() {score = int.Parse(goalList[i].txtAmount.text)};
                currentLevel.goals.Add(reachScoreGoal);
            }   
            else if(typeGoal == "Collect candy goal")
            {
                CollectCandyGoal collectCandyGoal = new CollectCandyGoal()
                {
                    candyType = (CandyColor)Enum.Parse(typeof(CandyColor), txtItem[0])                   
                };
                collectCandyGoal.amount = int.Parse(goalList[i].txtAmount.text);
                currentLevel.goals.Add(collectCandyGoal);
            }
            else if (typeGoal == "Collect element goal")
            {
                CollectElementGoal collectElementGoal = new CollectElementGoal()
                {
                    elementType = (ElementType)Enum.Parse(typeof(ElementType), txtItem[0])
                };
                collectElementGoal.amount = int.Parse(goalList[i].txtAmount.text);
                currentLevel.goals.Add(collectElementGoal);
            }
            else if (typeGoal == "Collect special block goal")
            {
                CollectSpecialBlockGoal collectSpecialBlockGoal = new CollectSpecialBlockGoal()
                {
                    specialBlockType = (SpecialBlockType)Enum.Parse(typeof(SpecialBlockType), txtItem[0])
                };
                collectSpecialBlockGoal.amount = int.Parse(goalList[i].txtAmount.text);
                currentLevel.goals.Add(collectSpecialBlockGoal);
            }
            else if (typeGoal == "Collect collectable goal")
            {
                CollectCollectableGoal collectCollectableGoal = new CollectCollectableGoal()
                {
                    collectableType = (CollectableType)Enum.Parse(typeof(CollectableType), txtItem[0])
                };
                collectCollectableGoal.amount = int.Parse(goalList[i].txtAmount.text);
                currentLevel.goals.Add(collectCollectableGoal);
            }
            else if (typeGoal == "Destroy all chocolate goal")
            {
                DestroyAllChocolateGoal destroyAllChocolateGoal = new DestroyAllChocolateGoal()
                {
                    completed = false
                };
                currentLevel.goals.Add(destroyAllChocolateGoal);
            }
        }    

    }*/

    /*    private void saveAvailableColorsObject()
        {
            ColorPrefab[] colorList = colorContent.transform.GetComponentsInChildren<ColorPrefab>();
            currentLevel.availableColors = new List<CandyColor>(colorList.Length);

            for (int i = 0; i < colorList.Length; i++)
            {
                CandyColor candyColor = (CandyColor)Enum.Parse(typeof(CandyColor), colorList[i].txtColor.text);
                currentLevel.availableColors.Add(candyColor);

            }
        }*/

    /*    private void SaveAvailableBoostersObject()
        {
            currentLevel.availableBoosters.Clear();
            currentLevel.availableBoosters.Add(BoosterType.Lollipop, tgLollipop.isOn);
            currentLevel.availableBoosters.Add(BoosterType.Bomb, tgBomb.isOn);
            currentLevel.availableBoosters.Add(BoosterType.Switch, tgSwitch.isOn);
            currentLevel.availableBoosters.Add(BoosterType.ColorBomb, tgColorBomb.isOn);

        }*/

    private void saveGoalNewObject()
    {
        GoalPersonPrefab[] goalPersonPrefabsList = goalPersonContent.transform.GetComponentsInChildren<GoalPersonPrefab>();
        currentLevel.goals.Clear();

        for (int j = 0; j < goalPersonPrefabsList.Length; j++)
        {
            //if (goalPersonPrefabsList[j].TypeCustomer != "Vip") continue;

            Person person = new Person();
            var dictCharacter = MiniJSON.Json.Deserialize(goalPersonPrefabsList[j].txtCharacter) as Dictionary<string, object>;
            if (dictCharacter != null)
            {
                foreach (var key in dictCharacter.Keys)
                {
                    person.character.Add(key, int.Parse(dictCharacter[key].ToString()));
                }
            }

            var dictGift = MiniJSON.Json.Deserialize(goalPersonPrefabsList[j].txtGift) as List<object>;
            if (dictGift != null)
            {
                foreach (var gift in dictGift)
                {
                    var giftItem = gift as Dictionary<string, object>;
                    BoosterResource boosterResource = new BoosterResource();
                    foreach (var key in giftItem.Keys)
                    {
                        if (key == "id")
                        {
                            boosterResource.id = int.Parse(giftItem[key].ToString());
                        }
                        else if (key == "t")
                        {
                            boosterResource.t = (string)giftItem[key];
                        }
                        else if (key == "q")
                        {
                            boosterResource.q = int.Parse(giftItem[key].ToString());
                        }    
                    }
                    person.gift.Add(boosterResource);

                }
            }
            if (!goalPersonPrefabsList[j].txtSkin.IsNullOrEmpty())
                person.skin = goalPersonPrefabsList[j].txtSkin.ToInt();
            //person.require = new List<Goal>(width * height);

            //GoalPrefab[] goalList = goalPersonPrefabsList[j].goalContent.transform.GetComponentsInChildren<GoalPrefab>();


            for (int i = 0; i < goalPersonPrefabsList[j].goalTempsList.Count; i++)
            {
                string typeGoal = goalPersonPrefabsList[j].goalTempsList[i].StrType;
                string[] txtItem = goalPersonPrefabsList[j].goalTempsList[i].txtItem.Split(" ");
                if (typeGoal == "Reach score goal")
                {
                    ReachScoreGoal reachScoreGoal = new ReachScoreGoal() { score = int.Parse(goalPersonPrefabsList[j].goalTempsList[i].txtAmount) };
                    person.require.Add(reachScoreGoal);
                }
                else if (typeGoal == "Collect candy goal")
                {
                    CollectCandyGoal collectCandyGoal = new CollectCandyGoal()
                    {
                        candyType = (CandyColor)Enum.Parse(typeof(CandyColor), txtItem[0])
                    };
                    collectCandyGoal.amount = int.Parse(goalPersonPrefabsList[j].goalTempsList[i].txtAmount);
                    person.require.Add(collectCandyGoal);
                }
                else if (typeGoal == "Collect special candy goal")
                {
                    string txtItemTemp = removesapce(goalPersonPrefabsList[j].goalTempsList[i].txtItem);
                    CollectSpecialCandyGoal collectSpecialCandyGoal = new CollectSpecialCandyGoal()
                    {

                        type = (SpecialCandyType)Enum.Parse(typeof(SpecialCandyType), txtItemTemp)
                    };
                    collectSpecialCandyGoal.amount = int.Parse(goalPersonPrefabsList[j].goalTempsList[i].txtAmount);
                    person.require.Add(collectSpecialCandyGoal);
                }
                else if (typeGoal == "Collect element goal")
                {
                    CollectElementGoal collectElementGoal = new CollectElementGoal()
                    {
                        elementType = (ElementType)Enum.Parse(typeof(ElementType), txtItem[0])
                    };
                    collectElementGoal.amount = int.Parse(goalPersonPrefabsList[j].goalTempsList[i].txtAmount);
                    person.require.Add(collectElementGoal);
                }
                else if (typeGoal == "Collect special block goal")
                {
                    CollectSpecialBlockGoal collectSpecialBlockGoal = new CollectSpecialBlockGoal()
                    {
                        specialBlockType = (SpecialBlockType)Enum.Parse(typeof(SpecialBlockType), txtItem[0])
                    };
                    collectSpecialBlockGoal.amount = int.Parse(goalPersonPrefabsList[j].goalTempsList[i].txtAmount);
                    person.require.Add(collectSpecialBlockGoal);
                }
                else if (typeGoal == "Collect multi block goal")
                {
                    CollectMultiBlockGoal collectMultiBlockGoal = new CollectMultiBlockGoal()
                    {
                        type = (MultiBlockType)Enum.Parse(typeof(MultiBlockType), txtItem[0])
                    };
                    collectMultiBlockGoal.amount = int.Parse(goalPersonPrefabsList[j].goalTempsList[i].txtAmount);
                    person.require.Add(collectMultiBlockGoal);
                }
                else if (typeGoal == "Collect collectable goal")
                {
                    CollectCollectableGoal collectCollectableGoal = new CollectCollectableGoal()
                    {
                        collectableType = (CollectableType)Enum.Parse(typeof(CollectableType), txtItem[0])
                    };
                    collectCollectableGoal.amount = int.Parse(goalPersonPrefabsList[j].goalTempsList[i].txtAmount);
                    person.require.Add(collectCollectableGoal);
                }
                else if (typeGoal == "Destroy all chocolate goal")
                {
                    DestroyAllChocolateGoal destroyAllChocolateGoal = new DestroyAllChocolateGoal()
                    {
                        completed = false
                    };
                    person.require.Add(destroyAllChocolateGoal);
                }
            }

            currentLevel.goals.Add(person);
        }

    }
    private void saveCustomerVipNewObject()
    {
        GoalPersonVipPrefab[] goalPersonVipPrefabsList = goalPersonContent.transform.GetComponentsInChildren<GoalPersonVipPrefab>();
        currentLevel.customerVip.Clear();

        for (int j = 0; j < goalPersonVipPrefabsList.Length; j++)
        {
            //if (goalPersonVipPrefabsList[j].TypeCustomer != "Normal") continue;

            PersonVip personVip = new PersonVip();
            var dictCharacter = MiniJSON.Json.Deserialize(goalPersonVipPrefabsList[j].txtCharacter) as Dictionary<string, object>;
            if (dictCharacter != null)
            {
                foreach (var key in dictCharacter.Keys)
                {
                    personVip.character.Add(key, int.Parse(dictCharacter[key].ToString()));
                }
            }

            var dictGift = MiniJSON.Json.Deserialize(goalPersonVipPrefabsList[j].txtGift) as List<object>;
            if (dictGift != null)
            {
                foreach (var gift in dictGift)
                {
                    var giftItem = gift as Dictionary<string, object>;
                    BoosterResource boosterResource = new BoosterResource();
                    foreach (var key in giftItem.Keys)
                    {
                        if (key == "id")
                        {
                            boosterResource.id = int.Parse(giftItem[key].ToString());
                        }
                        else if (key == "t")
                        {
                            boosterResource.t = (string)giftItem[key];
                        }
                        else if (key == "q")
                        {
                            boosterResource.q = int.Parse(giftItem[key].ToString());
                        }
                    }
                    personVip.gift.Add(boosterResource);

                }
            }

            if (!goalPersonVipPrefabsList[j].txtRatio.IsNullOrEmpty())
            personVip.ratio = goalPersonVipPrefabsList[j].txtRatio.ToInt();

            if (!goalPersonVipPrefabsList[j].txtTurnFrom.IsNullOrEmpty())
                personVip.times.Add(goalPersonVipPrefabsList[j].txtTurnFrom.ToInt());

            if (!goalPersonVipPrefabsList[j].txtTurnTo.IsNullOrEmpty())
                personVip.times.Add(goalPersonVipPrefabsList[j].txtTurnTo.ToInt());

            if (!goalPersonVipPrefabsList[j].txtTurnExist.IsNullOrEmpty())
                personVip.turn = goalPersonVipPrefabsList[j].txtTurnExist.ToInt();
            if (!goalPersonVipPrefabsList[j].txtSkin.IsNullOrEmpty())
                personVip.skin = goalPersonVipPrefabsList[j].txtSkin.ToInt();
            //personVip.random_require = new List<Goal>(width * height);

            //GoalPrefab[] goalList = goalPersonPrefabsList[j].goalContent.transform.GetComponentsInChildren<GoalPrefab>();
            for (int i = 0; i < goalPersonVipPrefabsList[j].goalTempsList.Count; i++)
            {
                List<GoalTemp> listGoalTemp = new List<GoalTemp>();
                List<Goal> listGoal = new List<Goal>();
                listGoalTemp = goalPersonVipPrefabsList[j].goalTempsList[i];
                for (int k = 0; k < listGoalTemp.Count; k++)
                {
                    string typeGoal = listGoalTemp[k].StrType;
                    string[] txtItem = listGoalTemp[k].txtItem.Split(" ");

                    if (typeGoal == "Reach score goal")
                    {
                        ReachScoreGoal reachScoreGoal = new ReachScoreGoal() { score = int.Parse(listGoalTemp[k].txtAmount) };
                        listGoal.Add(reachScoreGoal);
                    }
                    else if (typeGoal == "Collect candy goal")
                    {
                        CollectCandyGoal collectCandyGoal = new CollectCandyGoal()
                        {
                            candyType = (CandyColor)Enum.Parse(typeof(CandyColor), txtItem[0])
                        };
                        collectCandyGoal.amount = int.Parse(listGoalTemp[k].txtAmount);
                        listGoal.Add(collectCandyGoal);
                    }
                    else if (typeGoal == "Collect special candy goal")
                    {
                        string txtItemTemp = removesapce(listGoalTemp[k].txtItem);
                        CollectSpecialCandyGoal collectSpecialCandyGoal = new CollectSpecialCandyGoal()
                        {

                            type = (SpecialCandyType)Enum.Parse(typeof(SpecialCandyType), txtItemTemp)
                        };
                        collectSpecialCandyGoal.amount = int.Parse(listGoalTemp[k].txtAmount);
                        listGoal.Add(collectSpecialCandyGoal);
                    }
                    else if (typeGoal == "Collect element goal")
                    {
                        CollectElementGoal collectElementGoal = new CollectElementGoal()
                        {
                            elementType = (ElementType)Enum.Parse(typeof(ElementType), txtItem[0])
                        };
                        collectElementGoal.amount = int.Parse(listGoalTemp[k].txtAmount);
                        listGoal.Add(collectElementGoal);
                    }
                    else if (typeGoal == "Collect special block goal")
                    {
                        CollectSpecialBlockGoal collectSpecialBlockGoal = new CollectSpecialBlockGoal()
                        {
                            specialBlockType = (SpecialBlockType)Enum.Parse(typeof(SpecialBlockType), txtItem[0])
                        };
                        collectSpecialBlockGoal.amount = int.Parse(listGoalTemp[k].txtAmount);
                        listGoal.Add(collectSpecialBlockGoal);
                    }
                    else if (typeGoal == "Collect multi block goal")
                    {
                        CollectMultiBlockGoal collectMultiBlockGoal = new CollectMultiBlockGoal()
                        {
                            type = (MultiBlockType)Enum.Parse(typeof(MultiBlockType), txtItem[0])
                        };
                        collectMultiBlockGoal.amount = int.Parse(listGoalTemp[k].txtAmount);
                        listGoal.Add(collectMultiBlockGoal);
                    }
                    else if (typeGoal == "Collect collectable goal")
                    {
                        CollectCollectableGoal collectCollectableGoal = new CollectCollectableGoal()
                        {
                            collectableType = (CollectableType)Enum.Parse(typeof(CollectableType), txtItem[0])
                        };
                        collectCollectableGoal.amount = int.Parse(listGoalTemp[k].txtAmount);
                        listGoal.Add(collectCollectableGoal);
                    }
                    else if (typeGoal == "Destroy all chocolate goal")
                    {
                        DestroyAllChocolateGoal destroyAllChocolateGoal = new DestroyAllChocolateGoal()
                        {
                            completed = false
                        };
                        listGoal.Add(destroyAllChocolateGoal);
                    }
                }
                personVip.random_required.Add(listGoal);
            }
            currentLevel.customerVip.Add(personVip);
        }
    }

    private void saveAvailableBoostersNewObject()
    {
        currentLevel.availableBoosters.Clear();
        BoosterPrefab[] boosterList = boosterContent.transform.GetComponentsInChildren<BoosterPrefab>();

        for(int i =0; i<boosterList.Length; i++)
        {
            if (getListResourceBooster(boosterList[i]) != null)
            {
                if (boosterList[i].name == BoosterType.Lollipop.ToString())
                {
                    currentLevel.availableBoosters.Add(BoosterType.Lollipop, getListResourceBooster(boosterList[i]));
                }
/*                else if (boosterList[i].name == BoosterType.Bomb.ToString())
                {
                    currentLevel.availableBoosters.Add(BoosterType.Bomb, getListResourceBooster(boosterList[i]));
                }*/
                else if (boosterList[i].name == BoosterType.ColorBomb.ToString())
                {
                    currentLevel.availableBoosters.Add(BoosterType.ColorBomb, getListResourceBooster(boosterList[i]));
                }
                else if (boosterList[i].name == BoosterType.Switch.ToString())
                {
                    currentLevel.availableBoosters.Add(BoosterType.Switch, getListResourceBooster(boosterList[i]));
                }
                else if (boosterList[i].name == BoosterType.HorizontalRocket.ToString())
                {
                    currentLevel.availableBoosters.Add(BoosterType.HorizontalRocket, getListResourceBooster(boosterList[i]));
                }
                else if (boosterList[i].name == BoosterType.VerticalRocket.ToString())
                {
                    currentLevel.availableBoosters.Add(BoosterType.VerticalRocket, getListResourceBooster(boosterList[i]));
                }
                else if (boosterList[i].name == BoosterType.TNTWrapped.ToString())
                {
                    currentLevel.availableBoosters.Add(BoosterType.TNTWrapped, getListResourceBooster(boosterList[i]));
                }
            }
        }    
       
    }   

    private void saveAvailableBoosters_v2()
    {
        currentLevel.availableBoosters_v2.Clear();
        BoosterPrefab[] boosterList = boosterContent.transform.GetComponentsInChildren<BoosterPrefab>();

        for (int i = 0; i < boosterList.Length; i++)
        {
            if (getListResourceBooster(boosterList[i]) != null)
            {
                BoosterLimitAndGift boosterLimitAndGift = new BoosterLimitAndGift();
                boosterLimitAndGift.gift = getListResourceBooster(boosterList[i]);

                if ( boosterList[i].ifLimit.text != "")
                    boosterLimitAndGift.limit = int.Parse(boosterList[i].ifLimit.text);
                else
                    boosterLimitAndGift.limit = 0;

                if (boosterLimitAndGift.limit == 0) continue;

                if (boosterList[i].name == BoosterType.Lollipop.ToString())
                {
                    currentLevel.availableBoosters_v2.Add(BoosterType.Lollipop, boosterLimitAndGift);
                }
                /*                else if (boosterList[i].name == BoosterType.Bomb.ToString())
                                {
                                    currentLevel.availableBoosters.Add(BoosterType.Bomb, getListResourceBooster(boosterList[i]));
                                }*/
                else if (boosterList[i].name == BoosterType.ColorBomb.ToString())
                {
                    currentLevel.availableBoosters_v2.Add(BoosterType.ColorBomb, boosterLimitAndGift);
                }
                else if (boosterList[i].name == BoosterType.Switch.ToString())
                {
                    currentLevel.availableBoosters_v2.Add(BoosterType.Switch, boosterLimitAndGift);
                }
                else if (boosterList[i].name == BoosterType.HorizontalRocket.ToString())
                {
                    currentLevel.availableBoosters_v2.Add(BoosterType.HorizontalRocket, boosterLimitAndGift);
                }
                else if (boosterList[i].name == BoosterType.VerticalRocket.ToString())
                {
                    currentLevel.availableBoosters_v2.Add(BoosterType.VerticalRocket, boosterLimitAndGift);
                }
                else if (boosterList[i].name == BoosterType.TNTWrapped.ToString())
                {
                    currentLevel.availableBoosters_v2.Add(BoosterType.TNTWrapped, boosterLimitAndGift);
                }
            }
        }
    }    
    
    private void saveColorNewObject()
    {
        ColorPrefab[] colorList = colorContent.transform.GetComponentsInChildren<ColorPrefab>();
        currentLevel.availableColors = new List<AvailableColor>(colorList.Length);
        
        for(int i=0; i< colorList.Length; i++)
        {
            AvailableColor color = new AvailableColor();

            color.Type = colorList[i].txtColor.text;
            color.Ratio = int.Parse(colorList[i].ifRatio.text);
            color.Limit = int.Parse(colorList[i].ifLimit.text);
            currentLevel.availableColors.Add(color);
        }    
    }   
    
    private void saveFailingHoleNewObject()
    {
        CandyPrefab[] candyList = levelContent.transform.GetComponentsInChildren<CandyPrefab>();
        currentLevel.falling_hole.Clear();
        for (int i=0; i<candyList.Length; i++)
        {
            if(candyList[i].isFailHole == true)
            {
                FailingHole failingHole = new FailingHole();
                failingHole.x = candyList[i].x;
                failingHole.y = candyList[i].y;

                failingHole.cfg = candyList[i].listFailHoleColor;
                currentLevel.falling_hole.Add(failingHole);
            }    
        }    
    }    

    #endregion

    #region LoadData

    private void loadLevelData(Level currentLevel)
    {
        ifLevelNumber.text = currentLevel.id.ToString();
        ifWidth.text = "0";
        ifHeight.text = "0"; // set ao de clear tranform
        ifWidth.text = currentLevel.width.ToString();
        ifHeight.text = currentLevel.height.ToString();
        StartCoroutine(loadTilesObject(currentLevel));

        ddLimitType.value = (int)currentLevel.limitType ;
        ifMoves.text = currentLevel.limit.ToString();
        ifLimitBooster.text = currentLevel.limit_booster.ToString();
        //loadGoalObect(currentLevel);
        loadGoalNewObject(currentLevel);
        loadCustomerVipNewObject(currentLevel);

        //loadAvailableColorsObject(currentLevel);
        loadAvailableColorsNewObject(currentLevel);
        StartCoroutine(loadFailingHoleNewObject(currentLevel));

        ifStar1Score.text = currentLevel.score1.ToString();
        ifStar2Score.text = currentLevel.score2.ToString();
        ifStar3Score.text = currentLevel.score3.ToString();

        tgAwardSpeacialCandy.isOn = currentLevel.awardSpecialCandies;
        ddType.value = (int)currentLevel.awardedSpecialCandyType;

        ifCollectableChance.text = currentLevel.collectableChance.ToString();
        //loadAvailableBoosters(currentLevel);
        if(currentLevel.availableBoosters_v2.Count > 0)
            loadAvailableNewBooster_v2(currentLevel);
        else
            loadAvailableNewBooster(currentLevel);

    }

    IEnumerator loadTilesObject(Level currentLevel)
    {
        yield return new WaitForSeconds(0.01f);
        CandyPrefab[] candyList = levelContent.transform.GetComponentsInChildren<CandyPrefab>();
        for (int i=0; i<currentLevel.tiles.Count; i++)
        {
            if (currentLevel.tiles[i] != null)
            {
                string tileTypeName = string.Empty;
                string BrushType = string.Empty;
                int idCakeBox = 0;
                if (currentLevel.tiles[i] is CandyTile)
                {
                    var candyTile = (CandyTile)currentLevel.tiles[i];
                    tileTypeName = candyTile.type.ToString();
                    BrushType = "Candy";
                }
                else if (currentLevel.tiles[i] is SpecialCandyTile)
                {
                    var specialCandyTile = (SpecialCandyTile)currentLevel.tiles[i];
                    tileTypeName = specialCandyTile.type.ToString();
                    BrushType = "Special Candy";
                }
                else if (currentLevel.tiles[i] is SpecialBlockTile)
                {
                    var specialBlockTile = (SpecialBlockTile)currentLevel.tiles[i];
                    tileTypeName = specialBlockTile.type.ToString();
                    BrushType = "Special Block";
                }
                else if (currentLevel.tiles[i] is MultiBlockLevelTile)
                {
                    var multiBlockTile = (MultiBlockLevelTile)currentLevel.tiles[i];
                    tileTypeName = multiBlockTile.type.ToString();
                    idCakeBox = multiBlockTile.id;
                    BrushType = "Multi Block";
                }
                else if (currentLevel.tiles[i] is CollectableTile)
                {
                    var collectableTile = (CollectableTile)currentLevel.tiles[i];
                    tileTypeName = collectableTile.type.ToString();
                    BrushType = "Collectable";
                }
                else if (currentLevel.tiles[i] is HoleTile)
                {
                    tileTypeName = "Hole";
                    BrushType = "Hole";
                }
                else if (currentLevel.tiles[i] is ThroughTile)
                {
                    tileTypeName = "Through";
                    BrushType = "Through";
                }


                if (currentLevel.tiles[i].elementType == ElementType.Honey)
                {
                    tileTypeName += "_Honey";
                }
                else if (currentLevel.tiles[i].elementType == ElementType.Ice)
                {
                    tileTypeName += "_Ice";
                }
                else if (currentLevel.tiles[i].elementType == ElementType.Syrup1)
                {
                    tileTypeName += "_Syrup1";
                }
                else if (currentLevel.tiles[i].elementType == ElementType.Syrup2)
                {
                    tileTypeName += "_Syrup2";
                }
                else if(currentLevel.tiles[i].elementType == ElementType.Chain1)
                {
                    tileTypeName += "_Chain1";
                }
                else if (currentLevel.tiles[i].elementType == ElementType.Chain2)
                {
                    tileTypeName += "_Chain2";
                }
                else if (currentLevel.tiles[i].elementType == ElementType.IceLayer1)
                {
                    tileTypeName += "_IceLayer1";
                }
                else if (currentLevel.tiles[i].elementType == ElementType.IceLayer2)
                {
                    tileTypeName += "_IceLayer2";
                }

                candyList[i].ImgCandy.sprite = dictCandySprite[tileTypeName];
                candyList[i].StrBrushType = BrushType;
                candyList[i].idCakeBox = idCakeBox;
            }
            else
            {
                candyList[i].ImgCandy.gameObject.SetActive(false);
                candyList[i].StrBrushType = "Empty";
            }
            if (currentLevel.idBackground.Count > 0)
            {
                loadIdBackgroundObject(candyList[i], currentLevel.idBackground[i]);
            }
            else
            {
                loadIdBackgroundObject(candyList[i], 2);
            }    
        }
    }

    private void loadIdBackgroundObject(CandyPrefab candyItem, int idBgMap)
    {
        candyItem.ImgBgTile.sprite = dictMapSprite[idBgMap.ToString()];
        candyItem.idBgMap = idBgMap;
    }    

    /*private void loadGoalObect(Level currentLevel)
    {
        clearTranform(goalContent);
        for(int i=0; i<currentLevel.goals.Count;i++)
        {
            var copy = Instantiate(preGoal);
            copy.transform.SetParent(tfContetnGoal, false);
            copy.ddType.ClearOptions();

            if (currentLevel.goals[i] is ReachScoreGoal)
            {
                setTextGoal(copy, "Reach", ((ReachScoreGoal)currentLevel.goals[i]).score.ToString(), "Points", null);
                copy.StrType = "Reach score goal";
            }
            else if (currentLevel.goals[i] is CollectCandyGoal)
            {
                setTextGoal(copy, "Collect", ((CollectCandyGoal)currentLevel.goals[i]).amount.ToString(), ((CollectCandyGoal)currentLevel.goals[i]).candyType.ToString() + " Candy", dictCandySprite[((CollectCandyGoal)currentLevel.goals[i]).candyType.ToString() + "Candy"]);
                copy.ddType.AddOptions(listCandy);
                copy.StrType = "Collect candy goal";
            }
            else if (currentLevel.goals[i] is CollectElementGoal)
            {
                setTextGoal(copy, "Collect", ((CollectElementGoal)currentLevel.goals[i]).amount.ToString(), ((CollectElementGoal)currentLevel.goals[i]).elementType.ToString(),dictCandySprite[((CollectElementGoal)currentLevel.goals[i]).elementType.ToString()]);
                copy.ddType.AddOptions(listElement);
                copy.StrType = "Collect element goal";
            }
            else if (currentLevel.goals[i] is CollectSpecialBlockGoal)
            {
                setTextGoal(copy, "Collect", ((CollectSpecialBlockGoal)currentLevel.goals[i]).amount.ToString(), ((CollectSpecialBlockGoal)currentLevel.goals[i]).specialBlockType.ToString(), dictCandySprite[((CollectSpecialBlockGoal)currentLevel.goals[i]).specialBlockType.ToString()]);
                copy.ddType.AddOptions(listSpecialBlock);
                copy.StrType = "Collect special block goal";
            }
            else if (currentLevel.goals[i] is CollectCollectableGoal)
            {
                setTextGoal(copy, "Collect", ((CollectCollectableGoal)currentLevel.goals[i]).amount.ToString(), ((CollectCollectableGoal)currentLevel.goals[i]).collectableType.ToString(), dictCandySprite[((CollectCollectableGoal)currentLevel.goals[i]).collectableType.ToString()]);
                copy.ddType.AddOptions(listCollectable);
                copy.StrType = "Collect collectable goal";
            }
            else if (currentLevel.goals[i] is DestroyAllChocolateGoal)
            {
                setTextGoal(copy, "Destroy", "", "all chocolate", dictCandySprite["Chocolate"]);
                copy.btnGoalItem.interactable = false;
                copy.StrType = "Destroy all chocolate goal";
            }           
        }    
    }*/ 
    
    private void loadGoalNewObject(Level currentLevel)
    {
        clearTranform(goalPersonContent);
        countPerson = 1;

        for (int i=0; i< currentLevel.goals.Count; i++)
        {
            var copyPerson = Instantiate(goalPersonPrefab);
            copyPerson.transform.SetParent(goalPersonContent, false);
            copyPerson.txtAmount.text = countPerson.ToString();
            copyPerson.goalTempsList = new List<GoalTemp>();
            countPerson++;

            var jsonData = MiniJSON.Json.Deserialize(textJsonLoadFile) as Dictionary<string, object>;
            var goals = jsonData.GetList("goals");

            var goalItem = goals[i] as Dictionary<string, object>;

            var character = goalItem["character"];
            var gift = goalItem["gift"];

            string strCharacter = MiniJSON.Json.Serialize(character);
            string strGift = MiniJSON.Json.Serialize(gift);

            copyPerson.txtCharacter = strCharacter;
            copyPerson.txtGift = strGift;
            copyPerson.txtSkin = currentLevel.goals[i].skin.ToString();

            for (int j=0; j< currentLevel.goals[i].require.Count; j++)
            {
                GoalTemp goalTemp = new GoalTemp();
                if (currentLevel.goals[i].require[j] is ReachScoreGoal)
                {
                    setDataGoalTemp(goalTemp, "Reach", ((ReachScoreGoal)currentLevel.goals[i].require[j]).score.ToString(), "Points", "Reach score goal" ,null,null);
                }
                else if (currentLevel.goals[i].require[j] is CollectCandyGoal)
                {
                    setDataGoalTemp(goalTemp, "Collect", ((CollectCandyGoal)currentLevel.goals[i].require[j]).amount.ToString(), ((CollectCandyGoal)currentLevel.goals[i].require[j]).candyType.ToString() + " Candy", "Collect candy goal", dictCandySprite[((CollectCandyGoal)currentLevel.goals[i].require[j]).candyType.ToString() + "Candy"], listCandyGoal);
                }
                else if (currentLevel.goals[i].require[j] is CollectSpecialCandyGoal)
                {
                    setDataGoalTemp(goalTemp, "Collect", ((CollectSpecialCandyGoal)currentLevel.goals[i].require[j]).amount.ToString(), ((CollectSpecialCandyGoal)currentLevel.goals[i].require[j]).type.ToString() , "Collect special candy goal", dictCandySprite[((CollectSpecialCandyGoal)currentLevel.goals[i].require[j]).type.ToString()], listSpecialCandy);
                }
                else if (currentLevel.goals[i].require[j] is CollectElementGoal)
                {
                    setDataGoalTemp(goalTemp, "Collect", ((CollectElementGoal)currentLevel.goals[i].require[j]).amount.ToString(), ((CollectElementGoal)currentLevel.goals[i].require[j]).elementType.ToString(), "Collect element goal", dictCandySprite[((CollectElementGoal)currentLevel.goals[i].require[j]).elementType.ToString()], listElement);
                }
                else if (currentLevel.goals[i].require[j] is CollectSpecialBlockGoal)
                {
                    setDataGoalTemp(goalTemp, "Collect", ((CollectSpecialBlockGoal)currentLevel.goals[i].require[j]).amount.ToString(), ((CollectSpecialBlockGoal)currentLevel.goals[i].require[j]).specialBlockType.ToString(), "Collect special block goal", dictCandySprite[((CollectSpecialBlockGoal)currentLevel.goals[i].require[j]).specialBlockType.ToString()], listSpecialBlock);
                }
                else if (currentLevel.goals[i].require[j] is CollectMultiBlockGoal)
                {
                    setDataGoalTemp(goalTemp, "Collect", ((CollectMultiBlockGoal)currentLevel.goals[i].require[j]).amount.ToString(), ((CollectMultiBlockGoal)currentLevel.goals[i].require[j]).type.ToString(), "Collect multi block goal", dictCandySprite[((CollectMultiBlockGoal)currentLevel.goals[i].require[j]).type.ToString()], listMultiBlock);
                }
                else if (currentLevel.goals[i].require[j] is CollectCollectableGoal)
                {
                    setDataGoalTemp(goalTemp, "Collect", ((CollectCollectableGoal)currentLevel.goals[i].require[j]).amount.ToString(), ((CollectCollectableGoal)currentLevel.goals[i].require[j]).collectableType.ToString(), "Collect collectable goal", dictCandySprite[((CollectCollectableGoal)currentLevel.goals[i].require[j]).collectableType.ToString()], listCollectable);
                }
                else if (currentLevel.goals[i].require[j] is DestroyAllChocolateGoal)
                {
                    setDataGoalTemp(goalTemp, "Destroy", "", "all chocolate", "Destroy all chocolate goal", dictCandySprite["Chocolate"], null);
                }
                copyPerson.goalTempsList.Add(goalTemp);
            }    
        }    
    }
    private void loadCustomerVipNewObject(Level currentLevel)
    {
        //clearTranform(goalPersonContent);
        //countPerson = 1;
        for (int i = 0; i < currentLevel.customerVip.Count; i++)
        {
            var copyPerson = Instantiate(goalPersonVipPrefab);
            copyPerson.transform.SetParent(goalPersonContent, false);
            copyPerson.txtAmount.text = countPerson.ToString();
            copyPerson.goalTempsList = new List<List<GoalTemp>>();
            copyPerson.dictListRequireVip = new Dictionary<string, List<GoalTemp>>();
            copyPerson.listRequirelist = new List<string>();
            countPerson++;

            var jsonData = MiniJSON.Json.Deserialize(textJsonLoadFile) as Dictionary<string, object>;
            var goals = jsonData.GetList("customerVip");

            var goalItem = goals[i] as Dictionary<string, object>;

            var character = goalItem["character"];
            var gift = goalItem["gift"];

            string strCharacter = MiniJSON.Json.Serialize(character);
            string strGift = MiniJSON.Json.Serialize(gift);

            copyPerson.txtCharacter = strCharacter;
            copyPerson.txtGift = strGift;
            copyPerson.txtRatio = currentLevel.customerVip[i].ratio.ToString();
            copyPerson.txtTurnExist = currentLevel.customerVip[i].turn.ToString();
            copyPerson.txtTurnFrom = currentLevel.customerVip[i].times[0].ToString(); ;
            copyPerson.txtTurnTo = currentLevel.customerVip[i].times[1].ToString();
            copyPerson.txtSkin = currentLevel.customerVip[i].skin.ToString();

            int countListRequireVip = 0;
            for (int j = 0; j < currentLevel.customerVip[i].random_required.Count; j++)
            {
                List<Goal> listGoal = new List<Goal>();
                List<GoalTemp> listGoalTemp = new List<GoalTemp>();
                listGoal = currentLevel.customerVip[i].random_required[j];
                for (int k = 0; k < listGoal.Count; k++)
                {
                    GoalTemp goalTemp = new GoalTemp();
                    if (listGoal[k] is ReachScoreGoal)
                    {
                        setDataGoalTemp(goalTemp, "Reach", ((ReachScoreGoal)listGoal[k]).score.ToString(), "Score", "Reach score goal", null, null);
                    }
                    else if (listGoal[k] is CollectCandyGoal)
                    {
                        setDataGoalTemp(goalTemp, "Collect", ((CollectCandyGoal)listGoal[k]).amount.ToString(), ((CollectCandyGoal)listGoal[k]).candyType.ToString() + " Candy", "Collect candy goal", dictCandySprite[((CollectCandyGoal)listGoal[k]).candyType.ToString() + "Candy"], listCandyGoal);
                    }
                    else if (listGoal[k] is CollectSpecialCandyGoal)
                    {
                        setDataGoalTemp(goalTemp, "Collect", ((CollectSpecialCandyGoal)listGoal[k]).amount.ToString(), ((CollectSpecialCandyGoal)listGoal[k]).type.ToString() , "Collect special candy goal", dictCandySprite[((CollectSpecialCandyGoal)listGoal[k]).type.ToString()], listSpecialCandy);
                    }
                    else if (listGoal[k] is CollectElementGoal)
                    {
                        setDataGoalTemp(goalTemp, "Collect", ((CollectElementGoal)listGoal[k]).amount.ToString(), ((CollectElementGoal)listGoal[k]).elementType.ToString(), "Collect element goal", dictCandySprite[((CollectElementGoal)listGoal[k]).elementType.ToString()], listElement);
                    }
                    else if (listGoal[k] is CollectSpecialBlockGoal)
                    {
                        setDataGoalTemp(goalTemp, "Collect", ((CollectSpecialBlockGoal)listGoal[k]).amount.ToString(), ((CollectSpecialBlockGoal)listGoal[k]).specialBlockType.ToString(), "Collect special block goal", dictCandySprite[((CollectSpecialBlockGoal)listGoal[k]).specialBlockType.ToString()], listSpecialBlock);
                    }
                    else if (listGoal[k] is CollectMultiBlockGoal)
                    {
                        setDataGoalTemp(goalTemp, "Collect", ((CollectMultiBlockGoal)listGoal[k]).amount.ToString(), ((CollectMultiBlockGoal)listGoal[k]).type.ToString(), "Collect multi block goal", dictCandySprite[((CollectMultiBlockGoal)listGoal[k]).type.ToString()], listMultiBlock);
                    }
                    else if (listGoal[k] is CollectCollectableGoal)
                    {
                        setDataGoalTemp(goalTemp, "Collect", ((CollectCollectableGoal)listGoal[k]).amount.ToString(), ((CollectCollectableGoal)listGoal[k]).collectableType.ToString(), "Collect collectable goal", dictCandySprite[((CollectCollectableGoal)listGoal[k]).collectableType.ToString()], listCollectable);
                    }
                    else if (listGoal[k] is DestroyAllChocolateGoal)
                    {
                        setDataGoalTemp(goalTemp, "Destroy", "", "all chocolate", "Destroy all chocolate goal", dictCandySprite["Chocolate"], null);
                    }
                    listGoalTemp.Add(goalTemp);
                }
                copyPerson.goalTempsList.Add(listGoalTemp);
                ++countListRequireVip;
                copyPerson.listRequirelist.Add(countListRequireVip.ToString());
                copyPerson.dictListRequireVip.Add(countListRequireVip.ToString(), listGoalTemp);
             
            }
        }
    }

    private void setDataGoalTemp(GoalTemp goalTemp, string txtStatus, string txtAmount,string txtItem, string StrType, Sprite spriteCandy, List<string> ddTypeOption)
    {
        goalTemp.txtAmount = txtAmount;
        goalTemp.txtItem = txtItem;
        goalTemp.txtStatus = txtStatus;
        goalTemp.imgCandy = spriteCandy;
        goalTemp.StrType = StrType;
        goalTemp.ddTypeOption = ddTypeOption;
    }    

/*    private void  loadAvailableColorsObject(Level currentLevel)
    {
        clearTranform(colorContent);
        for(int i=0; i<currentLevel.availableColors.Count; i++)
        {
            var copy = Instantiate(preColor);
            copy.transform.SetParent(colorContent, false);

            setTextColor(copy, currentLevel.availableColors[i].ToString());
        }    
    }*/ 
    
    private void loadAvailableColorsNewObject(Level currentLevel)
    {
        clearTranform(colorContent);
        for(int i=0; i< currentLevel.availableColors.Count; i++)
        {
            var copy = Instantiate(preColor);
            copy.transform.SetParent(colorContent, false);

            if (currentLevel.availableColors[i].Limit == 0)
            setTextColor(copy, currentLevel.availableColors[i].Type, currentLevel.availableColors[i].Ratio.ToString());
            else
            setTextColor(copy, currentLevel.availableColors[i].Type, currentLevel.availableColors[i].Ratio.ToString(), currentLevel.availableColors[i].Limit.ToString());
        }    
    }
    private IEnumerator loadFailingHoleNewObject(Level currentLevel)
    {
        yield return new WaitForSeconds(0.02f);
        clearTranform(colorContentHole);
        CandyPrefab[] candyList = levelContent.transform.GetComponentsInChildren<CandyPrefab>();
        for (int j = 0; j < currentLevel.falling_hole.Count; j++)
        {
            FailingHole failingHoleTemp = currentLevel.falling_hole[j];
            for (int i = 0; i < currentLevel.tiles.Count; i++)
            {
                if (candyList[i].x == failingHoleTemp.x && candyList[i].y == failingHoleTemp.y)
                {
                    candyList[i].isFailHole = true;
                    candyList[i].setColorButtonFailingHole(true);
                    candyList[i].listFailHoleColor = failingHoleTemp.cfg;
                }
            }
        }
    }

/*    private void loadAvailableBoosters(Level currentLevel)
    {
        if (allowReset)
        {
            foreach (var type in Enum.GetValues(typeof(BoosterType)))
            {
                currentLevel.availableBoosters.Add((BoosterType)type, true);
            }
            allowReset = false;
        }

        tgLollipop.isOn = currentLevel.availableBoosters[BoosterType.Lollipop];
        tgBomb.isOn = currentLevel.availableBoosters[BoosterType.Bomb];
        tgSwitch.isOn = currentLevel.availableBoosters[BoosterType.Switch];
        tgColorBomb.isOn = currentLevel.availableBoosters[BoosterType.ColorBomb];

    }*/

    private void loadAvailableNewBooster(Level currentLevel)
    {
        BoosterPrefab[] boosterPrefabsList = boosterContent.transform.GetComponentsInChildren<BoosterPrefab>();
        BoosterType nameFirstBooster = (BoosterType)Enum.Parse(typeof(BoosterType), boosterPrefabsList[0].name);
        if (allowReset || !currentLevel.availableBoosters.ContainsKey(nameFirstBooster))
        {
            for(int i=0; i<boosterPrefabsList.Length; i++)
            {
                boosterPrefabsList[i].goResource.SetActive(true);
                boosterPrefabsList[i].imgBackground.color = new Color32(47, 255, 0, 255);
                boosterPrefabsList[i].checkIsRemove = true;
                boosterPrefabsList[i].ifCoin.text = "";
                boosterPrefabsList[i].ifEnergy.text = "";
                boosterPrefabsList[i].ifGem.text = "";
                boosterPrefabsList[i].ifLimit.text = "";
            }
            allowReset = false;
        }
        else
        {
           
            for (int i = 0; i < boosterPrefabsList.Length; i++)
            {
                BoosterType nameBooster = (BoosterType)Enum.Parse(typeof(BoosterType), boosterPrefabsList[i].name);
                if (nameBooster.ToString() == "Bomb") continue; // bo truong hop bomb
                List<BoosterResource> boosterResourceList = currentLevel.availableBoosters[nameBooster];
                boosterPrefabsList[i].ifEnergy.text = "";
                for (int j = 0; j < boosterResourceList.Count; j++)
                {                 
                    if (boosterResourceList[j].id == 1)
                    {
                        boosterPrefabsList[i].ifCoin.text = boosterResourceList[j].q.ToString();
                    }
                    else if (boosterResourceList[j].id == 2)
                    {
                        boosterPrefabsList[i].ifGem.text = boosterResourceList[j].q.ToString();
                    }
                    else if (boosterResourceList[j].id == 3)
                    {
                        boosterPrefabsList[i].ifEnergy.text = boosterResourceList[j].q.ToString();
                    }
                }
            }
        }

    }

    private void loadAvailableNewBooster_v2(Level currentLevel)
    {
        BoosterPrefab[] boosterPrefabsList = boosterContent.transform.GetComponentsInChildren<BoosterPrefab>();
        BoosterType nameFirstBooster = (BoosterType)Enum.Parse(typeof(BoosterType), boosterPrefabsList[0].name);
        if (allowReset || !currentLevel.availableBoosters_v2.ContainsKey(nameFirstBooster))
        {
            for (int i = 0; i < boosterPrefabsList.Length; i++)
            {
                boosterPrefabsList[i].goResource.SetActive(true);
                boosterPrefabsList[i].imgBackground.color = new Color32(47, 255, 0, 255);
                boosterPrefabsList[i].checkIsRemove = true;
                boosterPrefabsList[i].ifCoin.text = "";
                boosterPrefabsList[i].ifEnergy.text = "1";
                boosterPrefabsList[i].ifGem.text = "";
                boosterPrefabsList[i].ifLimit.text = "";
            }
            allowReset = false;
        }
        else
        {

            for (int i = 0; i < boosterPrefabsList.Length; i++)
            {
                BoosterType nameBooster = (BoosterType)Enum.Parse(typeof(BoosterType), boosterPrefabsList[i].name);

                BoosterLimitAndGift boosterLimitAndGift = new BoosterLimitAndGift();
                if (!currentLevel.availableBoosters_v2.ContainsKey(nameBooster)) continue;
                boosterLimitAndGift = currentLevel.availableBoosters_v2[nameBooster];
                List<BoosterResource> boosterResourceList = boosterLimitAndGift.gift;
                for (int j = 0; j < boosterResourceList.Count; j++)
                {
                    if (boosterResourceList[j].id == 1)
                    {
                        boosterPrefabsList[i].ifCoin.text = boosterResourceList[j].q.ToString();
                    }
                    else if (boosterResourceList[j].id == 2)
                    {
                        boosterPrefabsList[i].ifGem.text = boosterResourceList[j].q.ToString();
                    }
                    else if (boosterResourceList[j].id == 3)
                    {
                        boosterPrefabsList[i].ifEnergy.text = boosterResourceList[j].q.ToString();
                    }
                }
                boosterPrefabsList[i].ifLimit.text = boosterLimitAndGift.limit.ToString();
            }
        }

    }

    #endregion

    protected void SaveJsonFile<T>(string path, T data) where T : class
    {
        fsData serializedData;
        var serializer = new fsSerializer();
        fsResult  result = serializer.TrySerialize(data, out serializedData).AssertSuccessWithoutWarnings();
        if (result.Failed)
        {
            Toast.ShowMessage("Save error !!!!", Toast.Position.top, Toast.Time.twoSecond);
        }
        else
        {
            Toast.ShowMessage("** Save successfully **", Toast.Position.top, Toast.Time.twoSecond);
        }
        var file = new StreamWriter(path);
        var json = fsJsonPrinter.PrettyJson(serializedData);
        file.WriteLine(json);
        file.Close();
    }

    protected T LoadJsonFile<T>(string path) where T : class
    {
        if (!File.Exists(path))
        {
            return null;
        }

        var file = new StreamReader(path);
        fsData data = null;
        try
        {
            var fileContents = file.ReadToEnd();
             data = fsJsonParser.Parse(fileContents);
        }
        catch
        {
            Toast.ShowMessage(" Wrong file !!!!", Toast.Position.top, Toast.Time.twoSecond);
        }

        object deserialized = null;
        var serializer = new fsSerializer();
        if (data != null)
        {

            fsResult result = serializer.TryDeserialize(data, typeof(T), ref deserialized).AssertSuccessWithoutWarnings();
            
            if (result.Failed)
            {
                Toast.ShowMessage("Load error !!!!", Toast.Position.top, Toast.Time.twoSecond);
            }
            else
            {
                Toast.ShowMessage("** Load successfully **", Toast.Position.top, Toast.Time.twoSecond);
            }
        }
        file.Close();
        return deserialized as T;
    }
    protected T LoadJsonText<T>(string json) where T : class
    {

        fsData data = null;
        data = fsJsonParser.Parse(json);
        object deserialized = null;
        var serializer = new fsSerializer();
        if (data != null)
        {

            fsResult result = serializer.TryDeserialize(data, typeof(T), ref deserialized).AssertSuccessWithoutWarnings();

            if (result.Failed)
            {
                Toast.ShowMessage("Wrong error !!!!", Toast.Position.top, Toast.Time.twoSecond);
            }
            else
            {
                Toast.ShowMessage("** Load successfully **", Toast.Position.top, Toast.Time.twoSecond);
            }
        }
        return deserialized as T;
    }

    public void CopyAsFile()
    {
        if (goEditInformation.activeSelf)
        {
            setDataGoalPerson(goalPersonPrefabIsOpen);
            goEditInformation.SetActive(false);
        }
        if (goEditInformationVip.activeSelf)
        {
            setDataGoalPersonVip(goalPersonVipPrefabIsOpen);
            goEditInformationVip.SetActive(false);
        }
        if(goFalingHole.activeSelf)
        {
            setDataCandyFailingHole(candyPrefabIsOpen);
            goFalingHole.SetActive(false);
        }    
        onSuccessCopy();
    }
    private void onSuccessCopy()
    {

        try
        {
            saveLevelData();
            CopyJsonFile( currentLevel);
        }
        catch
        {
            Toast.ShowMessage("Data not correct or missing !!!!", Toast.Position.top, Toast.Time.twoSecond);
        }

    }
    protected void CopyJsonFile<T>( T data) where T : class
    {
        fsData serializedData;
        var serializer = new fsSerializer();
        fsResult result = serializer.TrySerialize(data, out serializedData).AssertSuccessWithoutWarnings();
        if (result.Failed)
        {
            Toast.ShowMessage("Copy error !!!!", Toast.Position.top, Toast.Time.twoSecond);
        }
        else
        {
            Toast.ShowMessage("** Copy successfully **", Toast.Position.top, Toast.Time.twoSecond);
        }
        var json = fsJsonPrinter.PrettyJson(serializedData);
        TextEditor te = new TextEditor();
        te.text = json;
        te.SelectAll();
        te.Copy();
    }

    private string removesapce(string text)
    {
        string textConvert = text.Replace(" ", string.Empty);
        return textConvert;
    }    
    
    private void clearTranform(Transform transform)
    {
        foreach (Transform child in transform)
        {
            GameObject.Destroy(child.gameObject);
        }
    }
}
