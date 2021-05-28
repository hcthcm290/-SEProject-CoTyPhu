using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusHerbStudy : BaseStatus
{
    public StatusHerbStudy()
    {
        _id = 2;
        _name = "Bảo hộ thảo dược";
        _description = "Nhận được sự bảo hộ từ thảo dược. Chống lại các hiệu ứng xấu lên bản thân 1 lần. Tồn tại 1 round.";
        _isConditional = true;
    }

    public override bool LoadData()
    {
        try
        {
            ExpiredOnRound e = new ExpiredOnRound(this, 1);
            return true;
        }
        catch (Exception e)
        {
            return false;
        }
    }

    public override bool StartListen()
    {
        return true;
    }

    public override bool ExcuteAction()
    {
        return true;
    }
}
