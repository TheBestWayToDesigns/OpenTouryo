﻿//**********************************************************************************
//* サンプル アプリ・コントローラ
//**********************************************************************************

// テスト用クラスなので、必要に応じて流用 or 削除して下さい。

//**********************************************************************************
//* クラス名        ：Crud1Controller
//* クラス日本語名  ：Html.BeginForm用サンプル アプリ・コントローラ
//*
//* 作成日時        ：－
//* 作成者          ：生技
//* 更新履歴        ：
//*
//*  日時        更新者            内容
//*  ----------  ----------------  -------------------------------------------------
//*  20xx/xx/xx  ＸＸ ＸＸ         ＸＸＸＸ
//**********************************************************************************

using MVC_Sample.Logic.Business;
using MVC_Sample.Logic.Common;
using MVC_Sample.Models.ViewModels;
using MVC_Sample.Models.DataSets;

using System.Data;
using System.Threading.Tasks;

using System.Web.Mvc;

using Touryo.Infrastructure.Business.Presentation;
using Touryo.Infrastructure.Public.Db;
using Touryo.Infrastructure.Framework.Common;

namespace MVC_Sample.Controllers
{
    /// <summary>
    /// Html.BeginForm用サンプル アプリ・コントローラ
    /// </summary>
    public class Crud1Controller : MyBaseMVController
    {
        /// <summary>
        /// 画面の初期表示
        /// GET: /Crud2/
        /// </summary>
        /// <returns>初期表示状態の画面 (ViewResult)</returns>
        [HttpGet]
        public ActionResult Index(CrudViweModel model)
        {
            return View(model);
        }

        /// <summary>
        /// Shippers テーブルのレコード数をカウントする
        /// </summary>
        /// <returns>再描画（ViewResult）</returns>
        public async Task<ActionResult> SelectCount(CrudViweModel model)
        {
            // 引数クラスを生成。下位（Ｂ・Ｄ層）は、テスト クラスを流用する
            TestParameterValue testParameterValue
                = new TestParameterValue(this.ControllerName, "-", this.ActionName,
                    model.DdlDap + "%" + model.DdlMode1 + "%" + model.DdlMode2 + "%" + model.DdlExRollback, this.UserInfo);
            
            // Ｂ層呼出し＋都度コミット
            LayerB layerB = new LayerB();
            TestReturnValue testReturnValue = (TestReturnValue) await layerB.DoBusinessLogicAsync(testParameterValue, this.SelectIsolationLevel(model.DdlIso));

            // 結果表示するメッセージ
            string message = "";
            
            if (testReturnValue.ErrorFlag == true)
            {
                // 結果（業務続行可能なエラー）
                message = "ErrorMessageID:" + testReturnValue.ErrorMessageID + ";";
                message += "ErrorMessage:" + testReturnValue.ErrorMessage + ";";
                message += "ErrorInfo:" + testReturnValue.ErrorInfo;
            }
            else
            {
                // 結果（正常系）
                message = testReturnValue.Obj.ToString() + "件のデータがあります";
            }

            // メッセージを設定。
            model.Message = message;

            // Html.BeginFormでは、全体更新。
            return View("Index", model);
        }

        /// <summary>
        /// Shippers テーブルのレコード全件を DataTable として取得する
        /// </summary>
        /// <returns>再描画（ViewResult）</returns>
        public async Task<ActionResult> SelectAll_DT(CrudViweModel model)
        {
            // 引数クラスを生成。下位（Ｂ・Ｄ層）は、テスト クラスを流用する
            TestParameterValue testParameterValue
                = new TestParameterValue(this.ControllerName, "-", this.ActionName,
                    model.DdlDap + "%" + model.DdlMode1 + "%" + model.DdlMode2 + "%" + model.DdlExRollback, this.UserInfo);
            
            // Ｂ層呼出し＋都度コミット
            LayerB layerB = new LayerB();
            TestReturnValue testReturnValue = (TestReturnValue) await layerB.DoBusinessLogicAsync(testParameterValue, this.SelectIsolationLevel(model.DdlIso));

            // 結果表示するメッセージ
            string message = "";
            
            if (testReturnValue.ErrorFlag == true)
            {
                // 結果（業務続行可能なエラー）
                message = "ErrorMessageID:" + testReturnValue.ErrorMessageID + ";";
                message += "ErrorMessage:" + testReturnValue.ErrorMessage + ";";
                message += "ErrorInfo:" + testReturnValue.ErrorInfo;

                // メッセージを設定。
                model.Message = message;
            }
            else
            {
                // 結果（正常系）
                model.shippers = new DsNorthwind.ShippersDataTable();
                DataTable dt = (DataTable)testReturnValue.Obj;

                foreach (DataRow row in dt.Rows)
                {
                    DsNorthwind.ShippersRow srow = model.shippers.NewShippersRow();
                    srow.ShipperID = int.Parse(row[0].ToString());
                    srow.CompanyName = row[1].ToString();
                    srow.Phone = row[2].ToString();

                    model.shippers.Rows.Add(srow);
                }
            }

            // Html.BeginFormでは、全体更新。
            return View("Index", model);
        }

        /// <summary>
        /// Shippers テーブルのレコード全件を DataSet として取得する
        /// </summary>
        /// <returns>再描画（ViewResult）</returns>
        public async Task<ActionResult> SelectAll_DS(CrudViweModel model)
        {
            // 引数クラスを生成。下位（Ｂ・Ｄ層）は、テスト クラスを流用する
            TestParameterValue testParameterValue
                = new TestParameterValue(this.ControllerName, "-", this.ActionName,
                    model.DdlDap + "%" + model.DdlMode1 + "%" + model.DdlMode2 + "%" + model.DdlExRollback, this.UserInfo);
            
            // Ｂ層呼出し＋都度コミット
            LayerB layerB = new LayerB();
            TestReturnValue testReturnValue = (TestReturnValue) await layerB.DoBusinessLogicAsync(testParameterValue, this.SelectIsolationLevel(model.DdlIso));

            // 結果表示するメッセージ
            string message = "";
            
            if (testReturnValue.ErrorFlag == true)
            {
                // 結果（業務続行可能なエラー）
                message = "ErrorMessageID:" + testReturnValue.ErrorMessageID + ";";
                message += "ErrorMessage:" + testReturnValue.ErrorMessage + ";";
                message += "ErrorInfo:" + testReturnValue.ErrorInfo;

                // メッセージを設定。
                model.Message = message;
            }
            else
            {
                // 結果（正常系）
                model.shippers = new DsNorthwind.ShippersDataTable();
                DataSet ds = (DataSet)testReturnValue.Obj;

                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    DsNorthwind.ShippersRow srow = model.shippers.NewShippersRow();
                    srow.ShipperID = int.Parse(row[0].ToString());
                    srow.CompanyName = row[1].ToString();
                    srow.Phone = row[2].ToString();

                    model.shippers.Rows.Add(srow);
                }
            }

            // Html.BeginFormでは、全体更新。
            return View("Index", model);
        }

        /// <summary>
        /// Shippers テーブルのレコード全件を DataReader として取得する
        /// </summary>
        /// <returns>再描画（ViewResult）</returns>
        public async Task<ActionResult> SelectAll_DR(CrudViweModel model)
        {
            // 引数クラスを生成。下位（Ｂ・Ｄ層）は、テスト クラスを流用する
            TestParameterValue testParameterValue
                = new TestParameterValue(this.ControllerName, "-", this.ActionName,
                    model.DdlDap + "%" + model.DdlMode1 + "%" + model.DdlMode2 + "%" + model.DdlExRollback, this.UserInfo);
            
            // Ｂ層呼出し＋都度コミット
            LayerB layerB = new LayerB();
            TestReturnValue testReturnValue = (TestReturnValue) await layerB.DoBusinessLogicAsync(testParameterValue, this.SelectIsolationLevel(model.DdlIso));

            // 結果表示するメッセージ
            string message = "";
            
            if (testReturnValue.ErrorFlag == true)
            {
                // 結果（業務続行可能なエラー）
                message = "ErrorMessageID:" + testReturnValue.ErrorMessageID + ";";
                message += "ErrorMessage:" + testReturnValue.ErrorMessage + ";";
                message += "ErrorInfo:" + testReturnValue.ErrorInfo;

                // メッセージを設定。
                model.Message = message;
            }
            else
            {
                // 結果（正常系）
                model.shippers = new DsNorthwind.ShippersDataTable();
                DataTable dt = (DataTable)testReturnValue.Obj;

                foreach (DataRow row in dt.Rows)
                {
                    DsNorthwind.ShippersRow srow = model.shippers.NewShippersRow();
                    srow.ShipperID = int.Parse(row[0].ToString());
                    srow.CompanyName = row[1].ToString();
                    srow.Phone = row[2].ToString();

                    model.shippers.Rows.Add(srow);
                }
            }

            // Html.BeginFormでは、全体更新。
            return View("Index", model);
        }

        /// <summary>
        /// Shippers テーブルのレコード全件を、動的 SQL を使用して取得する
        /// </summary>
        /// <returns>再描画（ViewResult）</returns>
        public async Task<ActionResult> SelectAll_DSQL(CrudViweModel model)
        {
            // 引数クラスを生成。下位（Ｂ・Ｄ層）は、テスト クラスを流用する
            TestParameterValue testParameterValue
                = new TestParameterValue(this.ControllerName, "-", this.ActionName,
                    model.DdlDap + "%" + model.DdlMode1 + "%" + model.DdlMode2 + "%" + model.DdlExRollback, this.UserInfo);

            // 動的SQLの要素を設定
            testParameterValue.OrderColumn = model.DdlOrderColumn;
            testParameterValue.OrderSequence = model.DdlOrderSequence;
            
            // Ｂ層呼出し＋都度コミット
            LayerB layerB = new LayerB();
            TestReturnValue testReturnValue = (TestReturnValue) await layerB.DoBusinessLogicAsync(testParameterValue, this.SelectIsolationLevel(model.DdlIso));

            // 結果表示するメッセージ
            string message = "";
            
            if (testReturnValue.ErrorFlag == true)
            {
                // 結果（業務続行可能なエラー）
                message = "ErrorMessageID:" + testReturnValue.ErrorMessageID + ";";
                message += "ErrorMessage:" + testReturnValue.ErrorMessage + ";";
                message += "ErrorInfo:" + testReturnValue.ErrorInfo;

                // メッセージを設定。
                model.Message = message;
            }
            else
            {
                // 結果（正常系）
                model.shippers = new DsNorthwind.ShippersDataTable();
                DataTable dt = (DataTable)testReturnValue.Obj;

                foreach (DataRow row in dt.Rows)
                {
                    DsNorthwind.ShippersRow srow = model.shippers.NewShippersRow();
                    srow.ShipperID = int.Parse(row[0].ToString());
                    srow.CompanyName = row[1].ToString();
                    srow.Phone = row[2].ToString();

                    model.shippers.Rows.Add(srow);
                }
            }

            // Html.BeginFormでは、全体更新。
            return View("Index", model);
        }

        /// <summary>
        /// ShipperId をキーにして Shippers テーブルのレコードを取得する
        /// </summary>
        /// <returns>再描画（ViewResult）</returns>
        public async Task<ActionResult> Select(CrudViweModel model)
        {
            // 引数クラスを生成。下位（Ｂ・Ｄ層）は、テスト クラスを流用する
            TestParameterValue testParameterValue
                = new TestParameterValue(this.ControllerName, "-", this.ActionName,
                    model.DdlDap + "%" + model.DdlMode1 + "%" + model.DdlMode2 + "%" + model.DdlExRollback, this.UserInfo);

            // 動的SQLの要素を設定
            testParameterValue.ShipperID = int.Parse(model.ShipperID);
            
            // Ｂ層呼出し＋都度コミット
            LayerB layerB = new LayerB();
            TestReturnValue testReturnValue = (TestReturnValue) await layerB.DoBusinessLogicAsync(testParameterValue, this.SelectIsolationLevel(model.DdlIso));

            // 結果表示するメッセージ
            string message = "";
            
            if (testReturnValue.ErrorFlag == true)
            {
                // 結果（業務続行可能なエラー）
                message = "ErrorMessageID:" + testReturnValue.ErrorMessageID + ";";
                message += "ErrorMessage:" + testReturnValue.ErrorMessage + ";";
                message += "ErrorInfo:" + testReturnValue.ErrorInfo;

                // メッセージを設定。
                model.Message = message;
            }
            else
            {
                // 結果（正常系）
                ModelState.Clear(); // ErrorのClearをしないと何故か設定できない。
                model.ShipperID = testReturnValue.ShipperID.ToString();
                model.CompanyName = testReturnValue.CompanyName;
                model.Phone = testReturnValue.Phone;
            }

            // Html.BeginFormでは、全体更新。
            return View("Index", model);
        }

        /// <summary>
        /// Shippers テーブルに新規レコードを追加する
        /// </summary>
        /// <returns>再描画（ViewResult）</returns>
        public async Task<ActionResult> Insert(CrudViweModel model)
        {
            // 引数クラスを生成。下位（Ｂ・Ｄ層）は、テスト クラスを流用する
            TestParameterValue testParameterValue
                = new TestParameterValue(this.ControllerName, "-", this.ActionName,
                    model.DdlDap + "%" + model.DdlMode1 + "%" + model.DdlMode2 + "%" + model.DdlExRollback, this.UserInfo);

            // 動的SQLの要素を設定
            testParameterValue.CompanyName = model.CompanyName;
            testParameterValue.Phone = model.Phone;
            
            // Ｂ層呼出し＋都度コミット
            LayerB layerB = new LayerB();
            TestReturnValue testReturnValue = (TestReturnValue) await layerB.DoBusinessLogicAsync(testParameterValue, this.SelectIsolationLevel(model.DdlIso));

            // 結果表示するメッセージ
            string message = "";
            
            if (testReturnValue.ErrorFlag == true)
            {
                // 結果（業務続行可能なエラー）
                message = "ErrorMessageID:" + testReturnValue.ErrorMessageID + ";";
                message += "ErrorMessage:" + testReturnValue.ErrorMessage + ";";
                message += "ErrorInfo:" + testReturnValue.ErrorInfo;
            }
            else
            {
                // 結果（正常系）
                message = testReturnValue.Obj.ToString() + "件追加";
            }

            // メッセージを設定。
            model.Message = message;
            // Html.BeginFormでは、全体更新。
            return View("Index", model);
        }

        /// <summary>
        /// Shippers テーブルに新規レコードを更新する
        /// </summary>
        /// <returns>再描画（ViewResult）</returns>
        public async Task<ActionResult> Update(CrudViweModel model)
        {
            // 引数クラスを生成。下位（Ｂ・Ｄ層）は、テスト クラスを流用する
            TestParameterValue testParameterValue
                = new TestParameterValue(this.ControllerName, "-", this.ActionName,
                    model.DdlDap + "%" + model.DdlMode1 + "%" + model.DdlMode2 + "%" + model.DdlExRollback, this.UserInfo);

            // 動的SQLの要素を設定
            testParameterValue.ShipperID = int.Parse(model.ShipperID);
            testParameterValue.CompanyName = model.CompanyName;
            testParameterValue.Phone = model.Phone;
            
            // Ｂ層呼出し＋都度コミット
            LayerB layerB = new LayerB();
            TestReturnValue testReturnValue = (TestReturnValue) await layerB.DoBusinessLogicAsync(testParameterValue, this.SelectIsolationLevel(model.DdlIso));

            // 結果表示するメッセージ
            string message = "";
            
            if (testReturnValue.ErrorFlag == true)
            {
                // 結果（業務続行可能なエラー）
                message = "ErrorMessageID:" + testReturnValue.ErrorMessageID + ";";
                message += "ErrorMessage:" + testReturnValue.ErrorMessage + ";";
                message += "ErrorInfo:" + testReturnValue.ErrorInfo;
            }
            else
            {
                // 結果（正常系）
                message = testReturnValue.Obj.ToString() + "件更新";
            }

            // メッセージを設定。
            model.Message = message;

            // Html.BeginFormでは、全体更新。
            return View("Index", model);
        }

        /// <summary>
        /// Shippers テーブルに新規レコードを削除する
        /// </summary>
        /// <returns>再描画（ViewResult）</returns>
        public async Task<ActionResult> Delete(CrudViweModel model)
        {
            // 引数クラスを生成。下位（Ｂ・Ｄ層）は、テスト クラスを流用する
            TestParameterValue testParameterValue
                = new TestParameterValue(this.ControllerName, "-", this.ActionName,
                    model.DdlDap + "%" + model.DdlMode1 + "%" + model.DdlMode2 + "%" + model.DdlExRollback, this.UserInfo);

            // 動的SQLの要素を設定
            testParameterValue.ShipperID = int.Parse(model.ShipperID);
                        
            // Ｂ層呼出し＋都度コミット
            LayerB layerB = new LayerB();
            TestReturnValue testReturnValue = (TestReturnValue) await layerB.DoBusinessLogicAsync(testParameterValue, this.SelectIsolationLevel(model.DdlIso));

            // 結果表示するメッセージ
            string message = "";

            if (testReturnValue.ErrorFlag == true)
            {
                // 結果（業務続行可能なエラー）
                message = "ErrorMessageID:" + testReturnValue.ErrorMessageID + ";";
                message += "ErrorMessage:" + testReturnValue.ErrorMessage + ";";
                message += "ErrorInfo:" + testReturnValue.ErrorInfo;
            }
            else
            {
                // 結果（正常系）
                message = testReturnValue.Obj.ToString() + "件削除";
            }

            // メッセージを設定。
            model.Message = message;
            
            // Html.BeginFormでは、全体更新。
            return View("Index", model);
        }

        /// <summary>画面遷移する</summary>
        /// <returns>画面遷移のためのRedirectResult</returns>
        public ActionResult Transitions()
        {
            return Redirect(Url.Action("Index", "Crud2"));
        }

        /// <summary>
        /// Sleepを実行し二重送信防止機能をテストする
        /// </summary>        
        /// <returns>再描画（ViewResult）</returns>
        public ActionResult PreventDoubleSubmission(CrudViweModel model)
        {
            System.Threading.Thread.Sleep(5 * 1000);

            // メッセージを設定。

            // 確認用のカウンタ
            if (Session["cnt"] == null)
            {
                Session["cnt"] = 1;
            }
            else
            {
                Session["cnt"] = ((int)Session["cnt"]) + 1;
            }

            model.Message = "PreventDoubleSubmission:" + Session["cnt"].ToString();

            // Html.BeginFormでは、全体更新。
            return View("Index", model);
        }

        /// <summary>分離レベルの設定</summary>
        private DbEnum.IsolationLevelEnum SelectIsolationLevel(string iso)
        {
            if (iso == "NC")
            {
                return DbEnum.IsolationLevelEnum.NotConnect;
            }
            else if (iso == "NT")
            {
                return DbEnum.IsolationLevelEnum.NoTransaction;
            }
            else if (iso == "RU")
            {
                return DbEnum.IsolationLevelEnum.ReadUncommitted;
            }
            else if (iso == "RC")
            {
                return DbEnum.IsolationLevelEnum.ReadCommitted;
            }
            else if (iso == "RR")
            {
                return DbEnum.IsolationLevelEnum.RepeatableRead;
            }
            else if (iso == "SZ")
            {
                return DbEnum.IsolationLevelEnum.Serializable;
            }
            else if (iso == "SS")
            {
                return DbEnum.IsolationLevelEnum.Snapshot;
            }
            else if (iso == "DF")
            {
                return DbEnum.IsolationLevelEnum.DefaultTransaction;
            }
            else
            {
                //throw new Exception("分離レベルの設定がおかしい");
                return DbEnum.IsolationLevelEnum.DefaultTransaction;
            }
        }
    }
}