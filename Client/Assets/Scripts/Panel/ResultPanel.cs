using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultPanel : BasePanel
{
    //失败提示图片
    private Image lost;
    //确定按钮
    private Button okBtn;

    //初始化
    public override void OnInit()
    {
        skinPath = "ResultPanel";
        layer = PanelManager.Layer.Tip;
    }

    //显示
    public override void OnShow(params object[] para)
    {
        lost = skin.transform.Find("BG/LostImage").GetComponent<Image>();
        okBtn = skin.transform.Find("OkBtn").GetComponent<Button>();
        okBtn.onClick.AddListener(OnOkClick);
        if (para.Length == 1)
        {
            bool isWin = (bool)para[0];
            lost.gameObject.SetActive(!isWin);
        }
    }

    public override void OnClose()
    {

    }

    private void OnOkClick()
    {
        PanelManager.Open<RoomPanel>();
        Close();
    }
}
