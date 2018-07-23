﻿//**********************************************************************************
//* サンプル アプリ・モデル
//**********************************************************************************

// テスト用クラスなので、必要に応じて流用 or 削除して下さい。

//**********************************************************************************
//* クラス名        ：ShipperViweModel
//* クラス日本語名  ：サンプル アプリ・モデル
//*
//* 作成日時        ：－
//* 作成者          ：生技
//* 更新履歴        ：
//*
//*  日時        更新者            内容
//*  ----------  ----------------  -------------------------------------------------
//*  20xx/xx/xx  ＸＸ ＸＸ         ＸＸＸＸ
//*
//**********************************************************************************

using System;

namespace MVC_Sample.Models.ViewModels
{
    /// <summary>
    /// サンプル アプリ・モデル
    /// </summary>
    public class ShipperViweModel
    {
        /// <summary>ShipperID</summary>
        public Int64 ShipperID { get; set; } // Oracle対応

        /// <summary>CompanyName</summary>
        public string CompanyName { get; set; }
        
        /// <summary>Phone</summary>
        public string Phone { get; set; }
    }
}