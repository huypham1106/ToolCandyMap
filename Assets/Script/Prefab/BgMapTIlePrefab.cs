using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BgMapTIlePrefab : MonoBehaviour
{
    [SerializeField] private Button btnBg = null;
    public Image imgChoose = null;
    public int id;

    // Start is called before the first frame update
    void Start()
    {
        imgChoose.gameObject.SetActive(false);
        if (id == 1) imgChoose.gameObject.SetActive(true);
        btnBg.onClick.AddListener(clickBtnBg);
    }
    
    private void clickBtnBg()
    {
        MainGameController.Instance.hideChooseEditBgMapTile();
        this.imgChoose.gameObject.SetActive(true);
        MainGameController.Instance.IdBgMapChoose = this.id;
    }    
    public void intitData(Sprite sprite)
    {
        btnBg.image.sprite = sprite;
        id = int.Parse(sprite.name);

    }    


}
