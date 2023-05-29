using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CandyPrefab : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] public  Image ImgCandy = null;
    [SerializeField] public  Button BtnCandyMap = null;
    [SerializeField] public Image ImgBgTile = null;
    [SerializeField] public Button btnFailHole = null;
    public string StrBrushType;
    public int x;
    public int y;
    public int idBgMap ;
    public int idCakeBox = 0;
    public bool isFailHole;
    private int cacheIdMap;
    private int cacheIdMapHole;
    public bool allowClick = true;
    public List<AvailableColor> listFailHoleColor = new List<AvailableColor>();

    private int downRight;
    private int down;
    private int right;
    CandyMatch3.Game.Common.LevelTile tile = null;


    void Start()
    {
        BtnCandyMap.onClick.AddListener(onClickCandyMap);
        btnFailHole.onClick.AddListener(onClickCandyHoleItem);
        tile = new CandyMatch3.Game.Common.LevelTile();
        isFailHole = false;
        CandyMatch3.Game.Common.CandyTile candy = tile as CandyMatch3.Game.Common.CandyTile;
      //  tile.elementType =
    }

    public void initData(int x, int y)
    {
        this.x = x;
        this.y = y;
    }    


    private void onClickCandyMap()
    {
        int valueBrushType = MainGameController.Instance.ddBrushType.value;

        if (valueBrushType < 9)
        {
            Sprite spriteCandy = MainGameController.Instance.getSpriteCandyOnMap(this);
            if (spriteCandy == null)
            {
                ImgCandy.gameObject.SetActive(false);
            }
            else
            {
                ImgCandy.gameObject.SetActive(true);
                ImgCandy.sprite = spriteCandy;
            }

           if (this.StrBrushType == "Hole" || this.StrBrushType == "Through")
            {
                if (cacheIdMapHole == 0)
                {
                    ImgBgTile.sprite = MainGameController.Instance.dictMapSprite["30"];
                    idBgMap = 30;
                }
                else
                {
                    idBgMap = cacheIdMapHole;
                    ImgBgTile.sprite = MainGameController.Instance.dictMapSprite[idBgMap.ToString()];
                }
            }
            else
            {
                if (cacheIdMap == 0)
                {
                    ImgBgTile.sprite = MainGameController.Instance.dictMapSprite["2"];
                    idBgMap = 2;
                }
                else
                {
                    idBgMap = cacheIdMap;
                    ImgBgTile.sprite = MainGameController.Instance.dictMapSprite[idBgMap.ToString()];
                }
            }

            if (this.StrBrushType == "Multi Block")
            {
                (int, int, int) result = getListForCandy(this);
                Debug.Log("Candy "  + " : right :" + result.Item1 + "  down: " + result.Item2 + "  downRight : " + result.Item3  );
                if (result.Item1 == 1 && result.Item2 == 1 && result.Item3 == 1)
                {
                    CandyPrefab[] candyPrefabList = transform.parent.transform.GetComponentsInChildren<CandyPrefab>();
                    int siblingIndex = this.transform.GetSiblingIndex();
                    int id = MainGameController.Instance.idCakeBox++;
                    setDataCakeBox(this, id);
                    setDataCakeBox(candyPrefabList[siblingIndex + 1], id);
                    setDataCakeBox(candyPrefabList[siblingIndex + MainGameController.Instance.width], id);
                    setDataCakeBox(candyPrefabList[siblingIndex + MainGameController.Instance.width + 1], id);


                }
                else 
                {
                    this.StrBrushType = "Candy";
                    this.ImgCandy.sprite = MainGameController.Instance.dictCandySprite["RandomCandy"];
                    Toast.ShowMessage("** Not space for 4 item cake box **", Toast.Position.top, Toast.Time.twoSecond);
                } 
                    
            }
            else
            {
                int indexColumn = (this.transform.GetSiblingIndex() + 1) % int.Parse(MainGameController.Instance.ifWidth.text);
                int indexRow = (this.transform.GetSiblingIndex()) / int.Parse(MainGameController.Instance.ifWidth.text);
                Debug.Log("------  " + indexRow);
                MainGameController.Instance.getEventInBrushMode(MainGameController.Instance.ddBrushMode.value, indexColumn, indexRow, this);
            }
        }
        else if(valueBrushType == 8)
        {

            int idBgMapChoose = MainGameController.Instance.getSpriteBgMap(this);
            if (idBgMapChoose == -1)
            {
                Toast.ShowMessage("** Not allow, Type candy is Hole **", Toast.Position.top, Toast.Time.twoSecond);
            }
            else if (idBgMapChoose == -2)
            {
                Toast.ShowMessage("** Not allow, Type candy is not Hole **", Toast.Position.top, Toast.Time.twoSecond);
            }    
            else
            {
                ImgBgTile.sprite = MainGameController.Instance.dictMapSprite[idBgMapChoose.ToString()];
                idBgMap = idBgMapChoose;
                if (StrBrushType == "Hole" || StrBrushType == "Through")
                {
                    cacheIdMapHole = idBgMap;
                }
                else
                {
                    cacheIdMap = idBgMap;
                }
            }
        }    
    }

    private void setDataCakeBox(CandyPrefab candy, int id)
    {
        candy.StrBrushType = "Multi Block";
        candy.ImgCandy.sprite = MainGameController.Instance.dictCandySprite["CakeBox"];
        candy.idCakeBox = id;
    }    

    private (int, int, int) getListForCandy(CandyPrefab candyPrefab)
    {
        right = down = downRight =  0;
        CandyPrefab[] candyPrefabList = transform.parent.transform.GetComponentsInChildren<CandyPrefab>();
        int indexCurrentNode = candyPrefab.transform.GetSiblingIndex();

        if (candyPrefab.x - 1 >= 0)
        {
            // Left
/*            if (candyPrefabList[indexCurrentNode - 1].StrBrushType != "Hole")
                left = 1;*/
            // Left Down
            //if (currentNode.y - 1 >= 0) neighbourList.Add(mapTileList[indexCurrentNode - width - 1]);
            // Left Up
            //if (currentNode.y + 1 < height) neighbourList.Add(mapTileList[indexCurrentNode + width - 1]);
        }
        // Up
        if (candyPrefab.y + 1 < MainGameController.Instance.height)
        {
            if (candyPrefabList[indexCurrentNode + MainGameController.Instance.width].StrBrushType != "Multi Block")
                down = 1;
        }


        if (candyPrefab.x + 1 < MainGameController.Instance.width)
        {
            // Right
            if (candyPrefabList[indexCurrentNode + 1].StrBrushType != "Multi Block")
                right = 1;
            // Right Down
            //if (currentNode.y - 1 >= 0) neighbourList.Add(mapTileList[indexCurrentNode - width + 1]);
             //Right Up
            if (candyPrefab.y + 1 < MainGameController.Instance.height)
            {
                if((candyPrefabList[indexCurrentNode + MainGameController.Instance.width + 1].StrBrushType != "Multi Block"))
                downRight = 1;

            } 
                
                
                
        }
        // Down
/*        if (candyPrefab.y - 1 >= 0)
        {
            if (candyPrefabList[indexCurrentNode - width].StrBrushType != "Hole")
                up = 1;
        }*/

        return (right, down, downRight);
    }    

    private void onClickCandyHoleItem()
    {
        MainGameController.Instance.CheckClickOtherTile(this);
        if (allowClick)
        {
            MainGameController.Instance.getDataCandyFailingHole(this);

            if (MainGameController.Instance.goFalingHole.activeSelf)
            {
                this.allowClick = false;
            }
        }
        else
        {
            MainGameController.Instance.goFalingHole.SetActive(false);
            this.allowClick = true;
        }
    }

    public void setColorButtonFailingHole(bool isFailingHole)
    {
        if (isFailingHole)
        {
            btnFailHole.GetComponent<Image>().color = Color.black;
            isFailHole = true;
        }
        else
        {
            btnFailHole.GetComponent<Image>().color = Color.white;
            isFailHole = false;
            listFailHoleColor.Clear();
        }

    }    
    public void setCandy(string StrBrushType,int idBgMap, Sprite bgTile, Sprite imgCandy)
    {
        this.StrBrushType = StrBrushType;
        this.ImgBgTile.sprite = bgTile;
        this.idBgMap = idBgMap;
        this.ImgCandy.sprite = imgCandy;
    }   
     

}
 