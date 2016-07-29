#region Copyright Firefly Ltd 2014
/* *********************** DISCLAIMER **************************************
 * This software and documentation constitute an unpublished work and contain 
 * valuable trade secrets and proprietary information belonging to Firefly Ltd. 
 * None of the foregoing material may be copied, duplicated or disclosed without 
 * the express written permission of Firefly Ltd. 
 * FIREFLY LTD EXPRESSLY DISCLAIMS ANY AND ALL WARRANTIES CONCERNING THIS SOFTWARE 
 * AND DOCUMENTATION, INCLUDING ANY WARRANTIES OF MERCHANTABILITY AND/OR FITNESS 
 * FOR ANY PARTICULAR PURPOSE, AND WARRANTIES OF PERFORMANCE, AND ANY WARRANTY 
 * THAT MIGHT OTHERWISE ARISE FROM COURSE OF DEALING OR USAGE OF TRADE. NO WARRANTY 
 * IS EITHER EXPRESS OR IMPLIED WITH RESPECT TO THE USE OF THE SOFTWARE OR 
 * DOCUMENTATION. 
 * Under no circumstances shall Firefly Ltd be liable for incidental, special, 
 * indirect, direct or consequential damages or loss of profits, interruption of 
 * business, or related expenses which may arise from use of software or documentation, 
 * including but not limited to those resulting from defects in software and/or 
 * documentation, or loss or inaccuracy of data of any kind. 
 */
#endregion
using Firefly.Box;
using ENV.Data;
using ENV;
using ENV.IO;
using MandCo.BranchAccounting.Types;
using System.Diagnostics;
using System.IO;

namespace MandCo.BranchAccounting.Reports
{
    
    /// <summary>BranchStatementsHTM(bc213)(P#66)</summary>
    // Last change before Migration: 15/10/2013 12:42:02
    public class BranchStatementsHTMBc213 : BranchAccounting.BusinessProcessBase
    {
        #region Columns

        /// <summary>p:From Run No</summary>
        readonly Types.RunNumber pFromRunNo = new Types.RunNumber
        {
            Caption = "p:From Run No"
        };

        /// <summary>p:To Run No</summary>
        readonly Types.RunNumber pToRunNo = new Types.RunNumber
        {
            Caption = "p:To Run No"
        };

        /// <summary>p:Last Year End Date</summary>
        readonly DateColumn pLastYearEndDate = new DateColumn("p:Last Year End Date");

        /// <summary>p:Last Year Last Run No</summary>
        readonly Types.RunNumber pLastYearLastRunNo = new Types.RunNumber
        {
            Caption = "p:Last Year Last Run No"
        };

        /// <summary>p:Cut-Off Date</summary>
        readonly Types.Date1 pCutOffDate = new Types.Date1
        {
            Caption = "p:Cut-Off Date"
        };

        /// <summary>p:Requested Week</summary>
        readonly Types.CenturyWeek pRequestedWeek = new Types.CenturyWeek
        {
            Caption = "p:Requested Week"
        };

        /// <summary>p:BranchFrom</summary>
        readonly Types.BranchNumber pBranchFrom = new Types.BranchNumber
        {
            Caption = "p:BranchFrom"
        };

        /// <summary>p:BranchTo</summary>
        readonly Types.BranchNumber pBranchTo = new Types.BranchNumber
        {
            Caption = "p:BranchTo"
        };

        /// <summary>p:DetailOrSummary</summary>
        readonly TextColumn pDetailOrSummary = new TextColumn("p:DetailOrSummary", "1");

        /// <summary>===Parent Task===</summary>
        readonly TextColumn ParentTask = new TextColumn("===Parent Task===", "1");

        /// <summary>v:Processing Coy No</summary>
        readonly Types.CompanyNumber vProcessingCoyNo = new Types.CompanyNumber
        {
            Caption = "v:Processing Coy No"
        };

        /// <summary>v:Processing Coy Name</summary>
        readonly Types.Alpha30 vProcessingCoyName = new Types.Alpha30
        {
            Caption = "v:Processing Coy Name"
        };

        /// <summary>v:Processing Branch Number</summary>
        readonly Types.BranchNumber vProcessingBranchNumber = new Types.BranchNumber
        {
            Caption = "v:Processing Branch Number"
        };

        /// <summary>v:Processing Branch Name</summary>
        readonly Types.Alpha20 vProcessingBranchName = new Types.Alpha20
        {
            Caption = "v:Processing Branch Name"
        };

        /// <summary>v:From Run No</summary>
        readonly Types.RunNumber vFromRunNo = new Types.RunNumber
        {
            Caption = "v:From Run No"
        };

        /// <summary>v:To Run No</summary>
        readonly Types.RunNumber vToRunNo = new Types.RunNumber
        {
            Caption = "v:To Run No"
        };

        /// <summary>v:Last Year End Date</summary>
        readonly DateColumn vLastYearEndDate = new DateColumn("v:Last Year End Date");

        /// <summary>v:Last Year Last Run No</summary>
        readonly Types.RunNumber vLastYearLastRunNo = new Types.RunNumber
        {
            Caption = "v:Last Year Last Run No"
        };

        /// <summary>v:Cut-Off Date</summary>
        readonly Types.Date1 vCutOffDate = new Types.Date1
        {
            Caption = "v:Cut-Off Date"
        };

        /// <summary>v:Requested Week</summary>
        readonly Types.CenturyWeek vRequestedWeek = new Types.CenturyWeek
        {
            Caption = "v:Requested Week"
        };

        /// <summary>v:BranchFrom</summary>
        readonly Types.BranchNumber vBranchFrom = new Types.BranchNumber
        {
            Caption = "v:BranchFrom"
        };

        /// <summary>v:BranchTo</summary>
        readonly Types.BranchNumber vBranchTo = new Types.BranchNumber
        {
            Caption = "v:BranchTo"
        };

        /// <summary>v:DetailOrSummary</summary>
        internal bool isFullDetail = false;

        /// <summary>v:SQL Finished</summary>
        readonly BoolColumn vSQLFinished = new BoolColumn("v:SQL Finished");

        /// <summary>v:OK</summary>
        readonly TextColumn vOK = new TextColumn("v:OK", "U");

        /// <summary>v:Exit</summary>
        readonly TextColumn vExit = new TextColumn("v:Exit", "U");
        #endregion

        /// <summary>BranchStatementsHTM(bc213)(P#66)</summary>
        public BranchStatementsHTMBc213()
        {
            Title = "BranchStatementsHTM(bc213)";
            InitializeDataView();
        }

        void InitializeDataView()
        {
            #region Columns

            // In Magic Version 8 this program was executed from the client, the user entering
            // relevant parameters.  For Version 9 the on-line logic is performed by program
            // bc213c which invokes bc213 via CALL REMOTE.
            // Virtuals which are OBVIOUSLY no longer required have been removed.  But there
            // may still be some left which, after more detailed scrutiny of the program, could
            // be eliminated together with relevant coding as they are now redundant.

            // Parameters passed from on-line program bc213c via CALL REMOTE
            Columns.Add(pFromRunNo);
            Columns.Add(pToRunNo);
            Columns.Add(pLastYearEndDate);
            Columns.Add(pLastYearLastRunNo);
            Columns.Add(pCutOffDate);
            Columns.Add(pRequestedWeek);
            Columns.Add(pBranchFrom);
            Columns.Add(pBranchTo);
            Columns.Add(pDetailOrSummary);


            Columns.Add(ParentTask);

            Columns.Add(vProcessingCoyNo);
            Columns.Add(vProcessingCoyName);
            Columns.Add(vProcessingBranchNumber);
            Columns.Add(vProcessingBranchName);

            Columns.Add(vFromRunNo).BindValue(pFromRunNo);
            Columns.Add(vToRunNo).BindValue(pToRunNo);

            Columns.Add(vLastYearEndDate).BindValue(pLastYearEndDate);
            Columns.Add(vLastYearLastRunNo).BindValue(pLastYearLastRunNo);

            Columns.Add(vCutOffDate).BindValue(pCutOffDate);

            Columns.Add(vRequestedWeek).BindValue(pRequestedWeek);

            Columns.Add(vBranchFrom).BindValue(pBranchFrom);

            Columns.Add(vBranchTo).BindValue(pBranchTo);

            Columns.Add(vSQLFinished);
            Columns.Add(vOK);
            Columns.Add(vExit);
            #endregion
        }

        protected override void OnLoad()
        {
            Exit(ExitTiming.AfterRow);
            KeepChildRelationCacheAlive = true;
        }
        protected override void OnEnterRow()
        {
            Message.ShowWarningInStatusBar("bc213");
            Message.ShowWarningInStatusBar("bc213   Parameters from (on-line) program bc213c");
            Message.ShowWarningInStatusBar("bc213");
            Message.ShowWarningInStatusBar("bc213     Run Numbers:-  From      " + u.Str(pFromRunNo, "3P0"));
            Message.ShowWarningInStatusBar("bc213                    To        " + u.Str(pToRunNo, "3P0"));
            Message.ShowWarningInStatusBar("bc213     Last Year:-    End Date  " + u.DStr(pLastYearEndDate, "DD/MM/YYYY"));
            Message.ShowWarningInStatusBar("bc213                    Last Run  " + u.Str(pLastYearLastRunNo, "3P0"));
            Message.ShowWarningInStatusBar("bc213     Cut-off Date:-           " + u.DStr(pCutOffDate, "DD/MM/YYYY"));
            Message.ShowWarningInStatusBar("bc213     Requested Week:-         " + u.Str(pRequestedWeek, "6P0"));
            Message.ShowWarningInStatusBar("bc213     Branches:-     From      " + u.Str(pBranchFrom, "3P0"));
            Message.ShowWarningInStatusBar("bc213                    To        " + u.Str(pBranchTo, "3P0"));
            Message.ShowWarningInStatusBar("bc213     Detail or Summary:-      " + pDetailOrSummary);
            Message.ShowWarningInStatusBar("bc213");
        }
        protected override void OnLeaveRow()
        {
            new FlowControl(this).Run();
            Message.ShowWarningInStatusBar("bc213");
            Message.ShowWarningInStatusBar("bc213   Job complete");
            Message.ShowWarningInStatusBar("bc213");
        }



        /// <summary>Flow Control(P#66.1)</summary>
        // Last change before Migration: 28/08/2013 14:14:01
        internal class FlowControl : BranchAccounting.BusinessProcessBase
        {

            #region Columns

            /// <summary>v:BranchNumber</summary>
            readonly Types.BranchNumber vBranchNumber = new Types.BranchNumber
            {
                Caption = "v:BranchNumber"
            };

            /// <summary>v:BranchName</summary>
            readonly TextColumn vBranchName = new TextColumn("v:BranchName", "20");

            /// <summary>v:WebDirA</summary>
            readonly TextColumn vWebDirA = new TextColumn("v:WebDirA", "30");

            /// <summary>v:FirstSlash</summary>
            readonly NumberColumn vFirstSlash = new NumberColumn("v:FirstSlash", "2");

            /// <summary>v:WebDirB</summary>
            readonly TextColumn vWebDirB = new TextColumn("v:WebDirB", "30");
            #endregion

            BranchStatementsHTMBc213 _parent;


            /// <summary>Flow Control(P#66.1)</summary>
            public FlowControl(BranchStatementsHTMBc213 parent)
            {
                _parent = parent;
                Title = "Flow Control";
                InitializeDataView();
            }
            void InitializeDataView()
            {
                #region Columns

                Columns.Add(vBranchNumber);
                Columns.Add(vBranchName);
                // Set up Web Directory Name
                Columns.Add(vWebDirA).BindValue(() => u.IniGet("[MAGIC_LOGICAL_NAMES]web"));
                Columns.Add(vFirstSlash).BindValue(() => u.InStr(vWebDirA, "/"));
                Columns.Add(vWebDirB).BindValue(() => u.Mid(vWebDirA, vFirstSlash, u.Len(u.Trim(vWebDirA)) + 1 - vFirstSlash));
                #endregion
            }

            #region Run Overloads

            /// <summary>Flow Control</summary>
            internal void Run()
            {
                Execute();
            }
            #endregion

            protected override void OnLoad()
            {
                Exit(ExitTiming.AfterRow);
                if (NewViewRequired)
                {
                    View = () => new Views.BranchStatementsHTMBc213FlowControl(this);
                }
            }
            protected override void OnStart()
            {
                u.SetCrsr(4);
                u.Delay(10);
            }
            protected override void OnLeaveRow()
            {
                _parent.vSQLFinished.Value = true;

                Cached<RangeThruBranch>().Run();
            }
            protected override void OnEnd()
            {
                u.SetCrsr(1);
            }

            #region Expressions
            internal Bool Exp_4()
            {
                return u.Not(_parent.vSQLFinished);
            }
            #endregion



            /// <summary>Range Thru Branch(P#66.1.1)</summary>
            // Last change before Migration: 28/08/2013 14:14:01
            internal class RangeThruBranch : BranchAccounting.BusinessProcessBase
            {

                #region Models

                /// <summary>Branch</summary>
                internal readonly Models.Branch Branch = new Models.Branch { ReadOnly = true };

                /// <summary>Company</summary>
                internal readonly Models.Company Company = new Models.Company { ReadOnly = true };

                /// <summary>PvYrEndBFW</summary>
                readonly Models.PvYrEndBFW PvYrEndBFW = new Models.PvYrEndBFW { KeepCacheAliveAfterExit = true, AllowRowLocking = true };

                /// <summary>Transaction Codes</summary>
                readonly Models.TransactionCodes TransactionCodes = new Models.TransactionCodes { ReadOnly = true };

                /// <summary>Summary Codes</summary>
                readonly Models.SummaryCodes SummaryCodes = new Models.SummaryCodes { ReadOnly = true };

                /// <summary>TransDetailsExtractA</summary>
                readonly Models.TransDetailsExtractA TransDetailsExtractA = new Models.TransDetailsExtractA { Cached = false, ReadOnly = true };

                /// <summary>DeptSumExt</summary>
                readonly Models.DeptSumExt DeptSumExt = new Models.DeptSumExt { Cached = false, ReadOnly = true };

                /// <summary>SumCodesExt</summary>
                readonly Models.SumCodesExt SumCodesExt = new Models.SumCodesExt { Cached = false, ReadOnly = true };
                #endregion

                #region Columns

                /// <summary>===Range Thru Branch====</summary>
                readonly TextColumn RangeThruBranch1 = new TextColumn("===Range Thru Branch====", "1");

                /// <summary>v:AIXPath</summary>
                readonly TextColumn vAIXPath = new TextColumn("v:AIXPath", "80");

                /// <summary>v:BranSumPrt</summary>
                readonly BoolColumn vBranSumPrt = new BoolColumn("v:BranSumPrt");

                /// <summary>v:BranSumTotsPrt</summary>
                readonly BoolColumn vBranSumTotsPrt = new BoolColumn("v:BranSumTotsPrt");

                /// <summary>==Prev Virtuals==</summary>
                readonly TextColumn PrevVirtuals = new TextColumn("==Prev Virtuals==", "1");

                /// <summary>v:Prev Coy No</summary>
                readonly Types.CompanyNumber vPrevCoyNo = new Types.CompanyNumber
                {
                    Caption = "v:Prev Coy No"
                };

                /// <summary>v:Prev Company Name</summary>
                readonly Types.Alpha30 vPrevCompanyName = new Types.Alpha30
                {
                    Caption = "v:Prev Company Name"
                };

                /// <summary>v:Prev Transaction Code</summary>
                readonly Types.TransactionCode vPrevTransactionCode = new Types.TransactionCode
                {
                    Caption = "v:Prev Transaction Code"
                };

                /// <summary>v:Prev Tran Code Descr</summary>
                readonly TextColumn vPrevTranCodeDescr = new TextColumn("v:Prev Tran Code Descr", "50");

                /// <summary>v:Prev Summary Code</summary>
                readonly Types.SummaryCode vPrevSummaryCode = new Types.SummaryCode
                {
                    Caption = "v:Prev Summary Code"
                };

                /// <summary>v:Prev Summ Code Descr</summary>
                readonly TextColumn vPrevSummCodeDescr = new TextColumn("v:Prev Summ Code Descr", "50");

                /// <summary>v:Cum to Prev Year Printing</summary>
                readonly BoolColumn vCumToPrevYearPrinting = new BoolColumn("v:Cum to Prev Year Printing");

                /// <summary>==Report Headers==</summary>
                readonly TextColumn ReportHeaders = new TextColumn("==Report Headers==", "1");

                /// <summary>v:Report Week No</summary>
                readonly TextColumn vReportWeekNo = new TextColumn("v:Report Week No", "7");

                /// <summary>v:Report Branch No</summary>
                readonly Types.BranchNumber vReportBranchNo = new Types.BranchNumber
                {
                    Caption = "v:Report Branch No"
                };

                /// <summary>v:Report Branch Name</summary>
                readonly TextColumn vReportBranchName = new TextColumn("v:Report Branch Name", "20");

                /// <summary>v:Report Type</summary>
                readonly TextColumn vReportType = new TextColumn("v:Report Type", "10");

                /// <summary>v:ReportTypeHTML</summary>
                readonly TextColumn vReportTypeHTML = new TextColumn("v:ReportTypeHTML", "25");

                /// <summary>=====Statements=====</summary>
                readonly TextColumn Statements = new TextColumn("=====Statements=====", "1");

                /// <summary>v:Report Trans/Summ Descript</summary>
                readonly TextColumn vReportTransSummDescript = new TextColumn("v:Report Trans/Summ Descript", "50");

                /// <summary>v:Report Tran Total</summary>
                readonly NumberColumn vReportTranTotal = new NumberColumn("v:Report Tran Total", "N12.2");

                /// <summary>v:Report Summ Cd Total</summary>
                readonly NumberColumn vReportSummCdTotal = new NumberColumn("v:Report Summ Cd Total", "N12.2");

                /// <summary>v:Report Stats Pos1 Doc</summary>
                readonly TextColumn vReportStatsPos1Doc = new TextColumn("v:Report Stats Pos1 Doc", "20");

                /// <summary>v:Report Stats Pos 1 Value</summary>
                readonly NumberColumn vReportStatsPos1Value = new NumberColumn("v:Report Stats Pos 1 Value", "N9.2");

                /// <summary>v:Report Stats Pos2 Doc</summary>
                readonly TextColumn vReportStatsPos2Doc = new TextColumn("v:Report Stats Pos2 Doc", "20");

                /// <summary>v:Report Stats Pos2 Value</summary>
                readonly NumberColumn vReportStatsPos2Value = new NumberColumn("v:Report Stats Pos2 Value", "N9.2");

                /// <summary>v:Report Stats Pos3 Doc</summary>
                readonly TextColumn vReportStatsPos3Doc = new TextColumn("v:Report Stats Pos3 Doc", "20");

                /// <summary>v:Report Stats Pos3 Value</summary>
                readonly NumberColumn vReportStatsPos3Value = new NumberColumn("v:Report Stats Pos3 Value", "N9.2");

                /// <summary>v:Report Stats Order Value</summary>
                readonly NumberColumn vReportStatsOrderValue = new NumberColumn("v:Report Stats Order Value", "N9.2");

                /// <summary>====Summ Page Detail====</summary>
                readonly TextColumn SummPageDetail = new TextColumn("====Summ Page Detail====", "1");

                /// <summary>v:Stat Summ Column 1</summary>
                readonly TextColumn vStatSummColumn1 = new TextColumn("v:Stat Summ Column 1", "60");

                /// <summary>v:Stat Summ Column 2</summary>
                readonly NumberColumn vStatSummColumn2 = new NumberColumn("v:Stat Summ Column 2", "N12.2");

                /// <summary>v:Stat Summ Column 3</summary>
                readonly NumberColumn vStatSummColumn3 = new NumberColumn("v:Stat Summ Column 3", "N12.2");

                /// <summary>v:Stat Summ Column 4</summary>
                readonly NumberColumn vStatSummColumn4 = new NumberColumn("v:Stat Summ Column 4", "N12.2");

                /// <summary>===Summ Page Totals===</summary>
                readonly TextColumn SummPageTotals = new TextColumn("===Summ Page Totals===", "1");

                /// <summary>v:Total Goods YTD</summary>
                readonly NumberColumn vTotalGoodsYTD = new NumberColumn("v:Total Goods YTD", "N12.2");

                /// <summary>v:Total Goods TW</summary>
                readonly NumberColumn vTotalGoodsTW = new NumberColumn("v:Total Goods TW", "N12.2");

                /// <summary>v:Total Goods CFW</summary>
                readonly NumberColumn vTotalGoodsCFW = new NumberColumn("v:Total Goods CFW", "N12.2");

                /// <summary>v:Total Voucher YTD</summary>
                readonly NumberColumn vTotalVoucherYTD = new NumberColumn("v:Total Voucher YTD", "N12.2");

                /// <summary>v:Total Voucher TW</summary>
                readonly NumberColumn vTotalVoucherTW = new NumberColumn("v:Total Voucher TW", "N12.2");

                /// <summary>v:Total Voucher CFW</summary>
                readonly NumberColumn vTotalVoucherCFW = new NumberColumn("v:Total Voucher CFW", "N12.2");

                /// <summary>===Dept Detail===</summary>
                readonly TextColumn DeptDetail = new TextColumn("===Dept Detail===", "1");

                /// <summary>v:Dept 1 Stock Value</summary>
                readonly NumberColumn vDept1StockValue = new NumberColumn("v:Dept 1 Stock Value", "N7.2");

                /// <summary>v:Dept 2 Stock Value</summary>
                readonly NumberColumn vDept2StockValue = new NumberColumn("v:Dept 2 Stock Value", "N7.2");

                /// <summary>v:Dept 3 Stock Value</summary>
                readonly NumberColumn vDept3StockValue = new NumberColumn("v:Dept 3 Stock Value", "N7.2");

                /// <summary>v:Dept 4 Stock Value</summary>
                readonly NumberColumn vDept4StockValue = new NumberColumn("v:Dept 4 Stock Value", "N7.2");

                /// <summary>v:Dept 5 Stock Value</summary>
                readonly NumberColumn vDept5StockValue = new NumberColumn("v:Dept 5 Stock Value", "N7.2");

                /// <summary>v:Dept 6 Stock Value</summary>
                readonly NumberColumn vDept6StockValue = new NumberColumn("v:Dept 6 Stock Value", "N7.2");

                /// <summary>v:Dept 7 Stock Value</summary>
                readonly NumberColumn vDept7StockValue = new NumberColumn("v:Dept 7 Stock Value", "N7.2");

                /// <summary>v:Dept 8 Stock Value</summary>
                readonly NumberColumn vDept8StockValue = new NumberColumn("v:Dept 8 Stock Value", "N7.2");

                /// <summary>v:Dept 9 Stock Value</summary>
                readonly NumberColumn vDept9StockValue = new NumberColumn("v:Dept 9 Stock Value", "N7.2");

                /// <summary>===Cum to Prev Year===</summary>
                readonly TextColumn CumToPrevYear = new TextColumn("===Cum to Prev Year===", "1");

                /// <summary>v:Bran Goods BFW Count</summary>
                readonly NumberColumn vBranGoodsBFWCount = new NumberColumn("v:Bran Goods BFW Count", "N8");

                /// <summary>v:Bran Goods BFW Value</summary>
                readonly NumberColumn vBranGoodsBFWValue = new NumberColumn("v:Bran Goods BFW Value", "N12.2");

                /// <summary>v:Coy Goods BFW Count</summary>
                readonly NumberColumn vCoyGoodsBFWCount = new NumberColumn("v:Coy Goods BFW Count", "N8");

                /// <summary>v:Coy Goods BFW Value</summary>
                readonly NumberColumn vCoyGoodsBFWValue = new NumberColumn("v:Coy Goods BFW Value", "N12.2");

                /// <summary>v:Bran Vouch BFW Count</summary>
                readonly NumberColumn vBranVouchBFWCount = new NumberColumn("v:Bran Vouch BFW Count", "8");

                /// <summary>v:Bran Vouch BFW Value</summary>
                readonly NumberColumn vBranVouchBFWValue = new NumberColumn("v:Bran Vouch BFW Value", "N12.2");

                /// <summary>v:Coy Vouch BFW Count</summary>
                readonly NumberColumn vCoyVouchBFWCount = new NumberColumn("v:Coy Vouch BFW Count", "8");

                /// <summary>v:Coy Vouch BFW Value</summary>
                readonly NumberColumn vCoyVouchBFWValue = new NumberColumn("v:Coy Vouch BFW Value", "N12.2");

                /// <summary>===Here Starteth Processing===</summary>
                readonly TextColumn HereStartethProcessing = new TextColumn("===Here Starteth Processing===", "1");
                #endregion

                #region Layouts

                /// <summary>Branch Stock Statement</summary>
                TextTemplate _viewBranchStockStatement;
                #endregion

                FlowControl _parent;


                /// <summary>Range Thru Branch(P#66.1.1)</summary>
                public RangeThruBranch(FlowControl parent)
                {
                    _parent = parent;
                    Title = "Range Thru Branch";
                    Entities.Add(TransactionCodes);
                    Entities.Add(SummaryCodes);
                    Entities.Add(TransDetailsExtractA);
                    Entities.Add(DeptSumExt);
                    Entities.Add(SumCodesExt);
                    InitializeDataView();
                    var BranchCompanyNumberGroup = Groups.Add(Branch.CompanyNumber);
                    BranchCompanyNumberGroup.Enter += BranchCompanyNumberGroup_Enter;
                    BranchCompanyNumberGroup.Leave += BranchCompanyNumberGroup_Leave;
                }
                void InitializeDataView()
                {
                    From = Branch;
                    Relations.Add(Company,
                            Company.CompanyNumber.IsEqualTo(Branch.CompanyNumber),
                        Company.SortByREF_Company_X1);

                    Relations.Add(PvYrEndBFW,
                            PvYrEndBFW.BranchNumber.IsEqualTo(Branch.BranchNumber),
                        PvYrEndBFW.SortByBAC_PvYrEnd_BFW_X1);

                    Where.Add(Branch.BranchNumber.IsBetween(_parent._parent.vBranchFrom, _parent._parent.vBranchTo));
                    OrderBy = Branch.SortByREF_Branch_X3;

                    #region Columns

                    Columns.Add(RangeThruBranch1);
                    Columns.Add(vAIXPath);
                    Columns.Add(vBranSumPrt);
                    Columns.Add(vBranSumTotsPrt);

                    Columns.Add(PrevVirtuals);
                    Columns.Add(vPrevCoyNo).BindValue(Branch.CompanyNumber);
                    Columns.Add(vPrevCompanyName);
                    Columns.Add(vPrevTransactionCode);
                    Columns.Add(vPrevTranCodeDescr);
                    Columns.Add(vPrevSummaryCode);
                    Columns.Add(vPrevSummCodeDescr);

                    Columns.Add(vCumToPrevYearPrinting);

                    Columns.Add(ReportHeaders);
                    // all reports
                    Columns.Add(vReportWeekNo);
                    Columns.Add(vReportBranchNo);
                    Columns.Add(vReportBranchName);
                    Columns.Add(vReportType);
                    Columns.Add(vReportTypeHTML);

                    Columns.Add(Statements);
                    Columns.Add(vReportTransSummDescript);

                    Columns.Add(vReportTranTotal);
                    Columns.Add(vReportSummCdTotal);

                    Columns.Add(vReportStatsPos1Doc);
                    Columns.Add(vReportStatsPos1Value);
                    Columns.Add(vReportStatsPos2Doc);
                    Columns.Add(vReportStatsPos2Value);
                    Columns.Add(vReportStatsPos3Doc);
                    Columns.Add(vReportStatsPos3Value);
                    Columns.Add(vReportStatsOrderValue);

                    // branch, company, group summary page detail colums
                    Columns.Add(SummPageDetail);
                    Columns.Add(vStatSummColumn1);
                    Columns.Add(vStatSummColumn2);
                    Columns.Add(vStatSummColumn3);
                    Columns.Add(vStatSummColumn4);

                    // sub totals for branch, company, group summary page
                    Columns.Add(SummPageTotals);
                    Columns.Add(vTotalGoodsYTD);
                    Columns.Add(vTotalGoodsTW);
                    Columns.Add(vTotalGoodsCFW);
                    Columns.Add(vTotalVoucherYTD);
                    Columns.Add(vTotalVoucherTW);
                    Columns.Add(vTotalVoucherCFW);

                    // department report fields for branch summary page
                    Columns.Add(DeptDetail);
                    Columns.Add(vDept1StockValue);
                    Columns.Add(vDept2StockValue);
                    Columns.Add(vDept3StockValue);
                    Columns.Add(vDept4StockValue);
                    Columns.Add(vDept5StockValue);
                    Columns.Add(vDept6StockValue);
                    Columns.Add(vDept7StockValue);
                    Columns.Add(vDept8StockValue);
                    Columns.Add(vDept9StockValue);

                    Columns.Add(CumToPrevYear);
                    // cfw virtuals for cum to prev year for summ page report
                    Columns.Add(vBranGoodsBFWCount);
                    Columns.Add(vBranGoodsBFWValue);
                    Columns.Add(vCoyGoodsBFWCount);
                    Columns.Add(vCoyGoodsBFWValue);
                    Columns.Add(vBranVouchBFWCount);
                    Columns.Add(vBranVouchBFWValue);
                    Columns.Add(vCoyVouchBFWCount);
                    Columns.Add(vCoyVouchBFWValue);

                    Columns.Add(HereStartethProcessing);

                    // HERE STARTETH THE PROCESSING
                    Columns.Add(Branch.BranchNumber);
                    Columns.Add(Branch.BranchName);
                    Columns.Add(Branch.CompanyNumber);
                    Columns.Add(Branch.AreaCode);
                    Columns.Add(Branch.BranchStatus);
                    Columns.Add(Branch.EffectiveDate);

                    Columns.Add(Company.CompanyNumber);
                    Columns.Add(Company.CompanyName);

                    Columns.Add(PvYrEndBFW.BranchNumber);
                    Columns.Add(PvYrEndBFW.GoodsValue);
                    Columns.Add(PvYrEndBFW.VoucherValue);
                    #endregion
                }

                #region Run Overloads

                /// <summary>Range Thru Branch</summary>
                internal void Run()
                {
                    Execute();
                }
                #endregion

                protected override void OnLoad()
                {
                    KeepChildRelationCacheAlive = true;

                    _viewBranchStockStatement = new TextTemplate("%MagicApps%bc/html/bc213.html");
                    _viewBranchStockStatement.Add(
                                new Tag("v:Report-Week-No", vReportWeekNo),
                                new Tag("v:Cut-Off-Date", _parent._parent.vCutOffDate),
                                new Tag("v:Report-Branch-No", vReportBranchNo),
                                new Tag("v:Report-Branch-Name", vReportBranchName),
                                new Tag("v:ReportTypeHTML", vReportTypeHTML));
                    if (NewViewRequired)
                    {
                        View = () => new Views.BranchStatementsHTMBc213RangeThruBranch(this);
                    }
                }
                protected override void OnStart()
                {
                    vReportWeekNo.Value = u.Mid(u.Str(_parent._parent.vRequestedWeek, "######"), 1, 4) + "/" + u.Mid(u.Str(_parent._parent.vRequestedWeek, "######"), 5, 2);
                    u.Delay(10);
                }
                void BranchCompanyNumberGroup_Enter()
                {
                    _parent._parent.vProcessingCoyNo.Value = Branch.CompanyNumber;
                    _parent._parent.vProcessingCoyName.Value = Company.CompanyName;
                }
                protected override void OnEnterRow()
                {
                    vAIXPath.Value = u.Trim(_parent.vWebDirB) + @"branch\b" + u.Str(Branch.BranchNumber, "4P0") + @"\wwwroot\rpt\ba\" + u.Mid(u.Str(_parent._parent.vRequestedWeek, "6P0"), 3, 2) + u.Mid(u.Str(_parent._parent.vRequestedWeek, "6P0"), 5, 2);
                    if (u.Not(u.FileExist("%AIXserver%" + u.Trim(vAIXPath))) && (Branch.BranchStatus == "O" || Branch.BranchStatus == "C" && u.ToNumber(Branch.EffectiveDate) > _parent._parent.vLastYearLastRunNo || Branch.BranchNumber >= 900))
                    {
                        Directory.CreateDirectory(vAIXPath.Value);
                    }

                    _parent._parent.vProcessingBranchNumber.Value = Branch.BranchNumber;
                    _parent._parent.vProcessingBranchName.Value = Branch.BranchName;

                    vTotalGoodsYTD.Value = 0;
                    vTotalGoodsTW.Value = 0;
                    vTotalGoodsCFW.Value = 0;
                    vTotalVoucherYTD.Value = 0;
                    vTotalVoucherTW.Value = 0;
                    vTotalVoucherCFW.Value = 0;

                    _parent.vBranchNumber.Value = Branch.BranchNumber;
                    _parent.vBranchName.Value = Branch.BranchName;
                }
                protected override void OnLeaveRow()
                {
                    // ALL BRANCHES WILL BE PROCESSED BUT BRANCH STATEMENTS AND
                    // BRANCH SUMMARIES WILL ONLY BE PRINTED FOR THE FOLLOWING -
                    // ~ OPEN BRANCHES
                    // ~ CLOSED BRANCHES WHICH WERE CLOSED DURING THIS YEAR
                    // ~ ALL BRANCH 900 UPWARDS (flagged P or C to prevent accidental
                    // posting but are 'LIVE')
                    // (BLOCK CONDITION REMOVED)

                    Message.ShowWarningInStatusBar("bc213   Processing Branch " + u.Trim(u.Str(Branch.BranchNumber, "#3P0")));
                    // new branch
                    vReportBranchNo.Value = Branch.BranchNumber;
                    vReportBranchName.Value = Branch.BranchName;
                    vReportType.Value = "";

                    vBranGoodsBFWCount.Value = 0;
                    vBranGoodsBFWValue.Value = PvYrEndBFW.GoodsValue;
                    vBranVouchBFWCount.Value = 0;
                    vBranVouchBFWValue.Value = PvYrEndBFW.VoucherValue;

                    // print statement for the branch
                    Cached<BranStatementDtl>().Run();
                    // print summary and acceptance pages for the branch
                    Cached<BranSummary>().Run();
                    Cached<DeptStockValues>().Run();


                    vCoyGoodsBFWCount.Value += vBranGoodsBFWCount;
                    vCoyGoodsBFWValue.Value += vBranGoodsBFWValue;
                    vCoyVouchBFWCount.Value += vBranVouchBFWValue;
                    vCoyVouchBFWValue.Value += vBranVouchBFWValue;
                }
                void BranchCompanyNumberGroup_Leave()
                {
                    vPrevCoyNo.Value = Branch.CompanyNumber;
                    vPrevCompanyName.Value = Company.CompanyName;
                }



                /// <summary>Bran Statement Dtl(P#66.1.1.1)</summary>
                // Last change before Migration: 15/10/2013 12:42:02
                class BranStatementDtl : BranchAccounting.BusinessProcessBase
                {

                    #region Models

                    /// <summary>TransDetailsExtractA</summary>
                    readonly Models.TransDetailsExtractA TransDetailsExtractA = new Models.TransDetailsExtractA { ReadOnly = true };

                    /// <summary>Branch Item Stk Mve Hdr</summary>
                    readonly Models.BranchItemStkMveHdr BranchItemStkMveHdr = new Models.BranchItemStkMveHdr { KeepCacheAliveAfterExit = true, AllowRowLocking = true };

                    /// <summary>Transaction Codes</summary>
                    readonly Models.TransactionCodes TransactionCodes = new Models.TransactionCodes { ReadOnly = true };

                    /// <summary>Summary Codes</summary>
                    readonly Models.SummaryCodes SummaryCodes = new Models.SummaryCodes { ReadOnly = true };
                    #endregion

                    #region Columns

                    /// <summary>===Print Bran Statement===</summary>
                    readonly TextColumn PrintBranStatement = new TextColumn("===Print Bran Statement===", "1");

                    /// <summary>v:SpaceColumn</summary>
                    readonly TextColumn vSpaceColumn = new TextColumn("v:SpaceColumn", "6");

                    /// <summary>v:SummCodeTotal</summary>
                    readonly BoolColumn vSummCodeTotal = new BoolColumn("v:SummCodeTotal");

                    /// <summary>v:Order Number</summary>
                    readonly TextColumn vOrderNumber = new TextColumn("v:Order Number", "20");

                    /// <summary>v:Send Branch Number</summary>
                    readonly NumberColumn vSendBranchNumber = new NumberColumn("v:Send Branch Number", "4P0A");

                    /// <summary>v:Receive Branch Number</summary>
                    readonly NumberColumn vReceiveBranchNumber = new NumberColumn("v:Receive Branch Number", "4P0A");

                    /// <summary>v:NewTransCode</summary>
                    readonly NumberColumn vNewTransCode = new NumberColumn("v:NewTransCode", "3P0A");

                    /// <summary>v:TransDesc</summary>
                    readonly TextColumn vTransDesc = new TextColumn("v:TransDesc", "UX29A");

                    /// <summary>v:Print Header</summary>
                    readonly BoolColumn vPrintHeader = new BoolColumn("v:Print Header");

                    /// <summary>v:Print Header 2</summary>
                    readonly BoolColumn vPrintHeader2 = new BoolColumn("v:Print Header 2");

                    /// <summary>v:Print Footer</summary>
                    readonly BoolColumn vPrintFooter = new BoolColumn("v:Print Footer");

                    /// <summary>v:NoOfOrdersWritten</summary>
                    readonly NumberColumn vNoOfOrdersWritten = new NumberColumn("v:NoOfOrdersWritten", "N6");
                    #endregion

                    #region Streams

                    /// <summary>StatementDetail</summary>
                    FileWriter _ioStatementDetail;
                    #endregion

                    #region Layouts

                    /// <summary>TrnSumCodeDescription</summary>
                    TextTemplate _viewTrnSumCodeDescription;

                    /// <summary>BranchStatementDetail</summary>
                    TextTemplate _viewBranchStatementDetail;

                    /// <summary>DividerLine</summary>
                    TextTemplate _viewDividerLine;
                    #endregion

                    RangeThruBranch _parent;


                    /// <summary>Bran Statement Dtl(P#66.1.1.1)</summary>
                    public BranStatementDtl(RangeThruBranch parent)
                    {
                        _parent = parent;
                        Title = "Bran Statement Dtl";
                        InitializeDataView();
                        var vNewTransCodeGroup = Groups.Add(vNewTransCode);
                        vNewTransCodeGroup.Enter += vNewTransCodeGroup_Enter;
                        vNewTransCodeGroup.Leave += vNewTransCodeGroup_Leave;
                        
                    }
                    public void InitializeDataView()
                    {
                        From = TransDetailsExtractA;
                        Relations.Add(BranchItemStkMveHdr,
                                CndRange(() => TransDetailsExtractA.TransactionCode == 312, BranchItemStkMveHdr.SEND_BR_NUM.IsEqualTo(TransDetailsExtractA.BranchNumber)).And(
                                BranchItemStkMveHdr.STK_MVE_REF.IsEqualTo(() => u.Right(TransDetailsExtractA.Document, 5))).And(
                                BranchItemStkMveHdr.STK_MVE_TYPE.IsEqualTo("IBT")).And(
                                CndRange(() => TransDetailsExtractA.TransactionCode == 311, BranchItemStkMveHdr.SENT_TO_BR_NUM.IsEqualTo(TransDetailsExtractA.BranchNumber))),
                            BranchItemStkMveHdr.SortBySS_BR_ITEM_STK_MVE_HDR_X1);
                        Relations[BranchItemStkMveHdr].BindEnabled(() => Exp_49());
                        Relations[BranchItemStkMveHdr].OrderBy.Reversed = true;

                        Relations.Add(TransactionCodes,
                                TransactionCodes.TransactionCode.IsEqualTo(TransDetailsExtractA.TransactionCode),
                            TransactionCodes.SortByBAC_Trans_Codes_X1);

                        Relations.Add(SummaryCodes,
                                SummaryCodes.SummaryCode.IsEqualTo(TransactionCodes.SummaryCodeB),
                            SummaryCodes.SortByBAC_Summ_Codes_X1);

                        Where.Add(TransDetailsExtractA.BranchNumber.IsEqualTo(_parent.Branch.BranchNumber));
                        Where.Add(TransDetailsExtractA.RunNumber.IsBetween(_parent._parent._parent.vFromRunNo, _parent._parent._parent.vToRunNo));
                        Where.Add(TransDetailsExtractA.TransactionDate.IsLessOrEqualTo(_parent._parent._parent.vCutOffDate.Value));
                        OrderBy.Add(vNewTransCode);
                        OrderBy.Add(TransDetailsExtractA.BranchNumber, TransDetailsExtractA.TransactionCode, TransDetailsExtractA.Document, TransDetailsExtractA.DepartmentCode, TransDetailsExtractA.SectionCode, TransDetailsExtractA.CreationDate, TransDetailsExtractA.CreationTime, TransDetailsExtractA.UniqueId);

                        #region Columns

                        Columns.Add(PrintBranStatement);
                        Columns.Add(vSpaceColumn).BindValue(() => "&nbsp;");
                        Columns.Add(vSummCodeTotal);
                        // STATEMENTS PRINTED FOR
                        // OPEN BRANCHES AND BRANCHES CLOSED THIS YEAR
                        // AND ALL BRANCH NUMBERS 900 UPWARDS

                        Columns.Add(TransDetailsExtractA.BranchNumber);
                        Columns.Add(TransDetailsExtractA.OtherBranchNumber);
                        Columns.Add(TransDetailsExtractA.DepartmentCode);
                        Columns.Add(TransDetailsExtractA.SectionCode);
                        Columns.Add(TransDetailsExtractA.TransactionCode);
                        Columns.Add(TransDetailsExtractA.Document);
                        Columns.Add(TransDetailsExtractA.Value);
                        Columns.Add(TransDetailsExtractA.RunNumber);
                        Columns.Add(TransDetailsExtractA.TransactionDate);
                        Columns.Add(TransDetailsExtractA.Source);

                        Columns.Add(vOrderNumber).BindValue(() => "");
                        Columns.Add(vSendBranchNumber).BindValue(() => 0);
                        Columns.Add(vReceiveBranchNumber).BindValue(() => 0);

                        // -- Check if the latest matching transfer is a customer order (not correct order, used only to create new trans code) --
                        Columns.Add(BranchItemStkMveHdr.SEND_BR_NUM);
                        Columns.Add(BranchItemStkMveHdr.STK_MVE_REF);
                        Columns.Add(BranchItemStkMveHdr.STK_MVE_TYPE);
                        Columns.Add(BranchItemStkMveHdr.SENT_TO_BR_NUM);
                        Columns.Add(BranchItemStkMveHdr.ORDER_NUMBER);

                        Columns.Add(vNewTransCode).BindValue(() => u.If((TransDetailsExtractA.TransactionCode == 311 || TransDetailsExtractA.TransactionCode == 312) && BranchItemStkMveHdr.ORDER_NUMBER.Trim() != "", TransDetailsExtractA.TransactionCode + 2, TransDetailsExtractA.TransactionCode));

                        Columns.Add(TransactionCodes.TransactionCode);
                        Columns.Add(TransactionCodes.TransactionDescription);
                        Columns.Add(TransactionCodes.SummaryCodeB);

                        Columns.Add(vTransDesc).BindValue(() => u.If(vNewTransCode == 313, "Customer Order - Debit", u.If(vNewTransCode == 314, "Customer Order - Credit", TransactionCodes.TransactionDescription)));

                        Columns.Add(SummaryCodes.SummaryCode);
                        Columns.Add(SummaryCodes.SummaryCodeDecription);

                        Columns.Add(vPrintHeader);
                        Columns.Add(vPrintHeader2);
                        Columns.Add(vPrintFooter);
                        Columns.Add(vNoOfOrdersWritten);
                        #endregion
                    }

                    #region Run Overloads

                    /// <summary>Bran Statement Dtl</summary>
                    internal void Run()
                    {
                        Execute();
                    }
                    #endregion

                    protected override void OnLoad()
                    {
                        KeepChildRelationCacheAlive = true;

                        _ioStatementDetail = new FileWriter("%AIXserver%" + u.Trim(_parent.vAIXPath) + "/Detail.htm")
                        {
                            Name = "StatementDetail"
                        };
                        _ioStatementDetail.Open();
                        Streams.Add(_ioStatementDetail);

                        _viewTrnSumCodeDescription = new TextTemplate("%MagicApps%bc/html/bc213.html");
                        _viewTrnSumCodeDescription.Add(
                                    new Tag("v:Report-Trans/Summ-Descript", _parent.vReportTransSummDescript),
                                    new Tag("OUTPUT_FORM_1_7", true, "5"),
                                    new Tag("SUBTASK1", true, "5"),
                                    new Tag("WRITE_TABLE_FOOTER", vPrintFooter, "5"));

                        _viewBranchStatementDetail = new TextTemplate("%MagicApps%bc/html/bc213.html");

                        _viewBranchStatementDetail.Add(
                                    new Tag("v:Report-Stats-Pos1-Doc", _parent.vReportStatsPos1Doc),
                                    new Tag("v:Report-Stats-Pos-1-Value", () => u.If(_parent.vReportStatsPos1Value != 0, u.Trim(u.Str(_parent.vReportStatsPos1Value, "N9.2")), " "), "12"),
                                    new Tag("v:SpaceColumn1", vSpaceColumn),
                                    new Tag("v:Report-Stats-Pos2-Doc", _parent.vReportStatsPos2Doc),
                                    new Tag("v:Report-Stats-Pos2-Value", () => u.If(_parent.vReportStatsPos2Value != 0, u.Trim(u.Str(_parent.vReportStatsPos2Value, "N9.2")), " "), "12"),
                                    new Tag("v:SpaceColumn2", vSpaceColumn),
                                    new Tag("v:Report-Stats-Pos3-Doc", _parent.vReportStatsPos3Doc),
                                    new Tag("v:Report-Stats-Pos3-Value", () => u.If(_parent.vReportStatsPos3Value != 0, u.Trim(u.Str(_parent.vReportStatsPos3Value, "N9.2")), " "), "12"),

                                    new Tag("OUTPUT_FORM_1_8", true, "5"),
                            new Tag("SUBTASK1", true, "5"),
                            new Tag("WRITE_TABLE_HEADER", vPrintHeader, "5"),
                            new Tag("TABLETYPE1", () => Exp_40(), "5"),
                            new Tag("WRITE_TABLE_HEADER2", vPrintHeader2, "5"),
                            new Tag("TABLETYPE2", () => Exp_41(), "5"),
                            new Tag("v:Order-Number", vOrderNumber),
                            new Tag("v:Branch-Number", () => u.If(vNewTransCode == 313, vSendBranchNumber, vReceiveBranchNumber), "4"),
                            new Tag("v:Department", () => u.Left(u.Str(TransDetailsExtractA.DepartmentCode, "##"), 1) + "0", "2"),
                            new Tag("v:Transfer No", () => u.Right(TransDetailsExtractA.Document, 5), "8"),
                                    new Tag("v:Transfer Value", () => u.If(_parent.vReportStatsOrderValue != 0, u.Trim(u.Str(_parent.vReportStatsOrderValue, "N9.2")), " "), "12"));



                        _viewDividerLine = new TextTemplate("%MagicApps%bc/html/bc213.html");
                        _viewDividerLine.Add(
                                    new Tag("OUTPUT_FORM_1_9", true, "5"),
                                    new Tag("SUBTASK1", true, "5"));
                    }
                    protected override void OnStart()
                    {
                        _parent.vReportTypeHTML.Value = "Statement Details";
                        Message.ShowWarningInStatusBar("Exp 4 currently at : " + Exp_4() + " Marker 0 (on Start)");
                        Message.ShowWarningInStatusBar("NewTransCode currently at : " + vNewTransCode.Value + " Marker 0 (on Start)");
                        Message.ShowWarningInStatusBar("TDetailsExA.TransCode currently at : " + TransDetailsExtractA.TransactionCode + " Marker 0 (on Start)");
                        Message.ShowWarningInStatusBar("BrItemStkMveHeader.OrderNumber currently at : " + BranchItemStkMveHdr.ORDER_NUMBER.Trim() + " Marker 0 (on Start)");

                        if (Exp_4())
                        {
                            _parent._viewBranchStockStatement.WriteTo(_ioStatementDetail);
                        }
                        _parent.vPrevSummaryCode.Value = "";


                    }
                    void vNewTransCodeGroup_Enter()
                    {
                        // if summ code has changed and not the first for the branch
                        // print summary code descript line with totals
                        if (TransactionCodes.SummaryCodeB != _parent.vPrevSummaryCode && u.Trim(_parent.vPrevSummaryCode) != "")
                        {
                            vSummCodeTotal.Value = true;
                            _parent.vReportTransSummDescript.Value = Exp_14();
                            Message.ShowWarningInStatusBar("Exp 4 currently at : " + Exp_4() + " Marker 1");
                            if (Exp_4())
                            {
                                _viewTrnSumCodeDescription.WriteTo(_ioStatementDetail);
                            }
                            _viewDividerLine.WriteTo(_ioStatementDetail);
                            vSummCodeTotal.Value = false;
                            _parent.vReportSummCdTotal.Value = 0;
                        }
                        // blank and zeroise the 3 report areas
                        _parent.vReportStatsPos1Doc.Value = "";
                        _parent.vReportStatsPos1Value.Value = 0;
                        _parent.vReportStatsPos2Doc.Value = "";
                        _parent.vReportStatsPos2Value.Value = 0;
                        _parent.vReportStatsPos3Doc.Value = "";
                        _parent.vReportStatsPos3Value.Value = 0;
                        // write the header
                        _parent.vReportTransSummDescript.Value = @"<A NAME=""SummCode" + SummaryCodes.SummaryCode + @"""/A>" + vTransDesc;
                        if (Exp_40())
                        {
                            vPrintHeader.Value = true;
                        }
                        if (Exp_41())
                        {
                            vPrintHeader2.Value = true;
                        }
                        Message.ShowWarningInStatusBar("Exp 4 currently at : " + Exp_4() + " Marker 2");
                        if (Exp_4())
                        {
                            _viewTrnSumCodeDescription.WriteTo(_ioStatementDetail);
                        }

                    }
                    protected override void OnEnterRow()
                    {
                        // -- Where the transaction is a transfer, match up to the other half and check if it is a customer order --
                        if (Exp_49())
                        {
                            Cached<CheckCustomerOrder>().Run();
                        }

                    }
                    protected override void OnLeaveRow()
                    {
                        // -- If the transaction is not a customer order --
                        if (vNewTransCode != 313 && vNewTransCode != 314)
                        {
                            // print report detail line if full
                            if (_parent.vReportStatsPos3Doc != "")
                            {
                                Message.ShowWarningInStatusBar("Exp 4 currently at : " + Exp_4() + " Marker 3");
                                if (Exp_4())
                                {
                                    _viewBranchStatementDetail.WriteTo(_ioStatementDetail);
                                }
                                vPrintHeader.Value = false;
                                // blank and zeroise 3 report areas
                                _parent.vReportStatsPos1Doc.Value = "";
                                _parent.vReportStatsPos1Value.Value = 0;
                                _parent.vReportStatsPos2Doc.Value = "";
                                _parent.vReportStatsPos2Value.Value = 0;
                                _parent.vReportStatsPos3Doc.Value = "";
                                _parent.vReportStatsPos3Value.Value = 0;
                            }

                            // move incoming to relevant report area
                            if (_parent.vReportStatsPos1Doc != "" && _parent.vReportStatsPos2Doc != "" && _parent.vReportStatsPos3Doc == "")
                            {
                                _parent.vReportStatsPos3Doc.Value = Exp_19();
                                _parent.vReportStatsPos3Value.Value = TransDetailsExtractA.Value;
                                _parent.vReportTranTotal.Value += TransDetailsExtractA.Value;
                            }

                            if (_parent.vReportStatsPos1Doc != "" && _parent.vReportStatsPos2Doc == "")
                            {
                                _parent.vReportStatsPos2Doc.Value = Exp_19();
                                _parent.vReportStatsPos2Value.Value = TransDetailsExtractA.Value;
                                _parent.vReportTranTotal.Value += TransDetailsExtractA.Value;
                            }

                            if (_parent.vReportStatsPos1Doc == "")
                            {
                                _parent.vReportStatsPos1Doc.Value = Exp_19();
                                _parent.vReportStatsPos1Value.Value = TransDetailsExtractA.Value;
                                _parent.vReportTranTotal.Value += TransDetailsExtractA.Value;
                            }
                            // -- Where the transaction is a customer order --
                        }
                        else
                        {
                            // -- Update the value variable with the value --
                            _parent.vReportStatsOrderValue.Value = TransDetailsExtractA.Value;
                            _parent.vReportTranTotal.Value += TransDetailsExtractA.Value;
                            // --Write out the line --
                            Message.ShowWarningInStatusBar("Exp 4 currently at : " + Exp_4() + " Marker 4");
                            if (Exp_4())
                            {
                                _viewBranchStatementDetail.WriteTo(_ioStatementDetail);
                            }
                            // -- Turn off the header --
                            vPrintHeader2.Value = false;
                        }

                    }
                    void vNewTransCodeGroup_Leave()
                    {
                        // print report detail line
                        if (vNewTransCode != 313 && vNewTransCode != 314 && (_parent.Branch.BranchStatus == "O" || _parent.Branch.BranchStatus == "C" && _parent.Branch.EffectiveDate > _parent._parent._parent.vLastYearEndDate || _parent.Branch.BranchNumber >= 900))
                        {
                            _viewBranchStatementDetail.WriteTo(_ioStatementDetail);
                        }
                        vPrintHeader.Value = false;
                        vPrintHeader2.Value = false;
                        // print tran descript line with totals
                        vPrintFooter.Value = true;
                        _parent.vReportTransSummDescript.Value = u.RTrim(vTransDesc) + " Total   " + u.LTrim(u.Str(_parent.vReportTranTotal, "N12.2"));
                        Message.ShowWarningInStatusBar("Exp 4 currently at : " + Exp_4() + " Marker 5");
                        if (Exp_4())
                        {
                            _viewTrnSumCodeDescription.WriteTo(_ioStatementDetail);
                        }
                        vPrintFooter.Value = false;
                        // update summ code total then zeroise tran total
                        _parent.vPrevSummaryCode.Value = TransactionCodes.SummaryCodeB;
                        _parent.vPrevSummCodeDescr.Value = u.Trim(SummaryCodes.SummaryCodeDecription);
                        _parent.vReportSummCdTotal.Value += _parent.vReportTranTotal;
                        _parent.vReportTranTotal.Value = 0;

                    }
                    protected override void OnEnd()
                    {
                        // write out the final summary descript line with totals
                        vSummCodeTotal.Value = true;
                        _parent.vReportTransSummDescript.Value = Exp_14();
                        Message.ShowWarningInStatusBar("Exp 4 currently at : " + Exp_4() + " Marker 6 (on End)");
                        Message.ShowWarningInStatusBar("Trans Code currently at : " + vNewTransCode.Value + " Marker 6 (on End)");
                        if (Exp_4())
                        {
                            _viewTrnSumCodeDescription.WriteTo(_ioStatementDetail);
                        }
                        _viewDividerLine.WriteTo(_ioStatementDetail);
                        vSummCodeTotal.Value = false;
                        _parent.vReportSummCdTotal.Value = 0;
                        _parent.vPrevSummaryCode.Value = "";
                        _parent.vPrevSummCodeDescr.Value = "";
                        _parent.vReportTransSummDescript.Value = "";
                    }

                    #region Expressions
                    Bool Exp_4()
                    {
                        return _parent.Branch.BranchStatus == "O" || _parent.Branch.BranchStatus == "C" && _parent.Branch.EffectiveDate > _parent._parent._parent.vLastYearEndDate || _parent.Branch.BranchNumber >= 900;
                    }
                    Text Exp_14()
                    {
                        return u.RTrim(_parent.vPrevSummCodeDescr) + "  Total  " + u.LTrim(u.Str(_parent.vReportSummCdTotal, "N12.2"));
                    }
                    Text Exp_19()
                    {
                        return TransDetailsExtractA.Document + "/" + u.Str(TransDetailsExtractA.RunNumber, "P03") + "-" + u.RTrim(TransDetailsExtractA.Source) + "/" + u.Str(TransDetailsExtractA.OtherBranchNumber, "P03");
                    }
                    Bool Exp_40()
                    {
                        return u.If(vNewTransCode == TransDetailsExtractA.TransactionCode, true, false);
                    }
                    Bool Exp_41()
                    {
                        return u.If(vNewTransCode != TransDetailsExtractA.TransactionCode, true, false);
                    }
                    Bool Exp_49()
                    {
                        return TransDetailsExtractA.TransactionCode == 311 || TransDetailsExtractA.TransactionCode == 312;
                    }
                    #endregion



                    /// <summary>Check Customer Order(P#66.1.1.1.1)</summary>
                    // Last change before Migration: 28/08/2013 14:14:01
                    class CheckCustomerOrder : BranchAccounting.BusinessProcessBase
                    {

                        #region Models

                        /// <summary>TransDetailsExtractA</summary>
                        readonly Models.TransDetailsExtractA TransDetailsExtractA = new Models.TransDetailsExtractA { AllowRowLocking = true };

                        /// <summary>Branch Item Stk Mve Hdr</summary>
                        readonly Models.BranchItemStkMveHdr BranchItemStkMveHdr = new Models.BranchItemStkMveHdr { ReadOnly = true };
                        #endregion

                        #region Columns

                        /// <summary>v:CustomerOrderFound</summary>
                        readonly BoolColumn vCustomerOrderFound = new BoolColumn("v:CustomerOrderFound");
                        #endregion

                        BranStatementDtl _parent;


                        /// <summary>Check Customer Order(P#66.1.1.1.1)</summary>
                        public CheckCustomerOrder(BranStatementDtl parent)
                        {
                            _parent = parent;
                            Title = "Check Customer Order";
                            InitializeDataView();
                        }
                        void InitializeDataView()
                        {
                            From = TransDetailsExtractA;
                            Relations.Add(BranchItemStkMveHdr,
                                    BranchItemStkMveHdr.SEND_BR_NUM.IsEqualTo(() => u.If(_parent.TransDetailsExtractA.TransactionCode == 311, TransDetailsExtractA.BranchNumber, _parent.TransDetailsExtractA.BranchNumber)).And(
                                    BranchItemStkMveHdr.STK_MVE_REF.IsEqualTo(() => u.Right(TransDetailsExtractA.Document, 5))).And(
                                    BranchItemStkMveHdr.STK_MVE_TYPE.IsEqualTo("IBT")).And(
                                    BranchItemStkMveHdr.SENT_DATETIME.IsBetween(() => u.DStr(TransDetailsExtractA.TransactionDate, "YYYY-MM-DD") + " " + u.TStr(u.TVal("00:00:01", "HH:MM:SS"), "HH:MM:SS"), () => u.DStr(TransDetailsExtractA.TransactionDate, "YYYY-MM-DD") + " " + u.TStr(u.TVal("23:59:59", "HH:MM:SS"), "HH:MM:SS"))).And(
                                    BranchItemStkMveHdr.SENT_TO_BR_NUM.IsEqualTo(() => u.If(_parent.TransDetailsExtractA.TransactionCode == 312, TransDetailsExtractA.BranchNumber, _parent.TransDetailsExtractA.BranchNumber))),
                                BranchItemStkMveHdr.SortBySS_BR_ITEM_STK_MVE_HDR_X1);

                            Where.Add(TransDetailsExtractA.DepartmentCode.IsEqualTo(_parent.TransDetailsExtractA.DepartmentCode));
                            Where.Add(TransDetailsExtractA.SectionCode.IsEqualTo(_parent.TransDetailsExtractA.SectionCode));
                            Where.Add(TransDetailsExtractA.TransactionCode.IsEqualTo(() => u.If(_parent.TransDetailsExtractA.TransactionCode == 312, 311, 312)));
                            Where.Add(TransDetailsExtractA.Document.IsEqualTo(_parent.TransDetailsExtractA.Document));
                            Where.Add(TransDetailsExtractA.Value.IsEqualTo(() => _parent.TransDetailsExtractA.Value * -(1)));
                            Where.Add(TransDetailsExtractA.TransactionDate.IsEqualTo(_parent.TransDetailsExtractA.TransactionDate));
                            Where.Add(TransDetailsExtractA.RunNumber.IsEqualTo(_parent.TransDetailsExtractA.RunNumber));
                            OrderBy = TransDetailsExtractA.SortByBAC_Trans_Details_ExtA_X1;

                            #region Columns

                            Columns.Add(TransDetailsExtractA.BranchNumber).Caption = "Opp Branch Number";
                            Columns.Add(TransDetailsExtractA.OtherBranchNumber).Caption = "Opp Other Branch Number";
                            Columns.Add(TransDetailsExtractA.DepartmentCode).Caption = "Opp Department Code";
                            Columns.Add(TransDetailsExtractA.SectionCode).Caption = "Opp Section Code";
                            Columns.Add(TransDetailsExtractA.TransactionCode).Caption = "Opp Transaction Code";
                            Columns.Add(TransDetailsExtractA.Document).Caption = "Opp Document";
                            Columns.Add(TransDetailsExtractA.Value).Caption = "Opp Value";
                            Columns.Add(TransDetailsExtractA.TransactionDate).Caption = "Opp Transaction Date";
                            Columns.Add(TransDetailsExtractA.RunNumber).Caption = "Opp Run Number";


                            Columns.Add(vCustomerOrderFound);
                            Relations[BranchItemStkMveHdr].NotifyRowWasFoundTo(vCustomerOrderFound);
                            Columns.Add(BranchItemStkMveHdr.SEND_BR_NUM);
                            Columns.Add(BranchItemStkMveHdr.STK_MVE_REF);
                            Columns.Add(BranchItemStkMveHdr.STK_MVE_TYPE);
                            Columns.Add(BranchItemStkMveHdr.SENT_DATETIME);
                            Columns.Add(BranchItemStkMveHdr.SENT_TO_BR_NUM);
                            Columns.Add(BranchItemStkMveHdr.SOURCE_CHANNEL);
                            Columns.Add(BranchItemStkMveHdr.ORDER_NUMBER);
                            #endregion
                        }

                        #region Run Overloads

                        /// <summary>Check Customer Order</summary>
                        internal void Run()
                        {
                            Execute();
                        }
                        #endregion

                        protected override void OnLoad()
                        {
                            Activity = Activities.Browse;
                            AllowUserAbort = true;
                        }
                        protected override void OnStart()
                        {
                            Message.ShowWarningInStatusBar("Starting Order Check");
                        }
                        protected override void OnEnterRow()
                        {
                            // -- If a matching customer order transfer is found --
                            if (vCustomerOrderFound)
                            {
                                _parent.vOrderNumber.Value = u.Trim(BranchItemStkMveHdr.ORDER_NUMBER);
                                _parent.vSendBranchNumber.Value = BranchItemStkMveHdr.SEND_BR_NUM;
                                _parent.vReceiveBranchNumber.Value = BranchItemStkMveHdr.SENT_TO_BR_NUM;
                                // -- Match found exit the task --
                                Raise(Command.Exit);
                            }

                        }


                    }
                }

                /// <summary>Bran Summary(P#66.1.1.2)</summary>
                // Last change before Migration: 17/07/2013 15:47:40
                class BranSummary : BranchAccounting.BusinessProcessBase
                {

                    #region Models

                    /// <summary>Summary Codes</summary>
                    readonly Models.SummaryCodes SummaryCodes = new Models.SummaryCodes { ReadOnly = true };

                    /// <summary>SumCodesExt</summary>
                    readonly Models.SumCodesExt SumCodesExt = new Models.SumCodesExt { KeepCacheAliveAfterExit = true, ReadOnly = true };
                    #endregion

                    #region Columns

                    /// <summary>===Print Bran Summary===</summary>
                    readonly TextColumn PrintBranSummary = new TextColumn("===Print Bran Summary===", "1");

                    /// <summary>v:BlankRow</summary>
                    readonly BoolColumn vBlankRow = new BoolColumn("v:BlankRow");

                    /// <summary>v:Prev Sum Code Type</summary>
                    readonly TextColumn vPrevSumCodeType = new TextColumn("v:Prev Sum Code Type", "1");

                    /// <summary>====UseThis SummaryCode====</summary>
                    readonly TextColumn UseThisSummaryCode = new TextColumn("====UseThis SummaryCode====", "60");

                    /// <summary>====UseTheseSummaryValues====</summary>
                    readonly TextColumn UseTheseSummaryValues = new TextColumn("====UseTheseSummaryValues====", "60");
                    #endregion

                    #region Streams

                    /// <summary>BranchSummary</summary>
                    FileWriter _ioBranchSummary;

                    /// <summary>BranchAcceptance</summary>
                    FileWriter _ioBranchAcceptance;
                    #endregion

                    #region Layouts

                    /// <summary>BranchSummary</summary>
                    TextTemplate _viewBranchSummary;

                    /// <summary>BranchAcceptance</summary>
                    TextTemplate _viewBranchAcceptance;
                    #endregion

                    RangeThruBranch _parent;


                    /// <summary>Bran Summary(P#66.1.1.2)</summary>
                    public BranSummary(RangeThruBranch parent)
                    {
                        _parent = parent;
                        Title = "Bran Summary";
                        InitializeDataView();
                    }
                    void InitializeDataView()
                    {
                        From = SummaryCodes;
                        Relations.Add(SumCodesExt,
                                SumCodesExt.BranchNumber.IsEqualTo(_parent.Branch.BranchNumber).And(
                                SumCodesExt.SummaryCode.IsEqualTo(SummaryCodes.SummaryCode)),
                            SumCodesExt.SortByBAC_Summ_Codes_ExtA_X1);

                        OrderBy = SummaryCodes.SortByBAC_Summ_Codes_X1;

                        #region Columns

                        Columns.Add(PrintBranSummary);
                        // BRANCH SUMMARY PRINTED FOR
                        // OPEN BRANCHES AND BRANCHES CLOSED THIS YEAR
                        // AND ALL BRANCHES 900 UPWARDS
                        Columns.Add(vBlankRow);
                        Columns.Add(vPrevSumCodeType);
                        Columns.Add(UseThisSummaryCode);
                        Columns.Add(SummaryCodes.SummaryCode);
                        Columns.Add(SummaryCodes.SummaryCodeDecription);

                        Columns.Add(UseTheseSummaryValues);
                        Columns.Add(SumCodesExt.BranchNumber);
                        Columns.Add(SumCodesExt.SummaryCode);
                        Columns.Add(SumCodesExt.BFWValue);
                        Columns.Add(SumCodesExt.TWKValue);
                        #endregion
                    }

                    #region Run Overloads

                    /// <summary>Bran Summary</summary>
                    internal void Run()
                    {
                        Execute();
                    }
                    #endregion

                    protected override void OnLoad()
                    {
                        KeepChildRelationCacheAlive = true;
                        KeepViewVisibleAfterExit = true;

                        _ioBranchSummary = new FileWriter("%AIXserver%" + u.Trim(_parent.vAIXPath) + "/Summary.htm")
                        {
                            Name = "BranchSummary"
                        };
                        _ioBranchSummary.Open();
                        Streams.Add(_ioBranchSummary);

                        _ioBranchAcceptance = new FileWriter("%AIXserver%" + u.Trim(_parent.vAIXPath) + "/Accept.htm")
                        {
                            Name = "BranchAcceptance"
                        };
                        _ioBranchAcceptance.Open();
                        Streams.Add(_ioBranchAcceptance);

                        _viewBranchSummary = new TextTemplate("%MagicApps%bc/html/bc213.html");
                        _viewBranchSummary.Add(
                                    new Tag("SUBTASK2", true, "5"),
                                    new Tag("OUTPUT_FORM_2_7", true, "5"),
                                    new Tag("v:Stat-Summ-Column-1", () => u.If(u.Not(vBlankRow), _parent.vStatSummColumn1, " "), "60"),
                                    new Tag("v:Stat-Summ-Column-2", () => u.If(u.Not(vBlankRow), _parent.vStatSummColumn2, 0), "N12.2CZ"),
                                    new Tag("v:Stat-Summ-Column-3", () => u.If(u.Not(vBlankRow) && u.Not(_parent.vCumToPrevYearPrinting), _parent.vStatSummColumn3, 0), "N12.2CZ"),
                                    new Tag("v:Stat-Summ-Column-4", () => u.If(u.Not(vBlankRow) && u.Not(_parent.vCumToPrevYearPrinting), _parent.vStatSummColumn4, 0), "N12.2CZ"));

                        _viewBranchAcceptance = new TextTemplate("%MagicApps%bc/html/bc213.html");
                        _viewBranchAcceptance.Add(
                                    new Tag("v:Total-Goods-YTD", _parent.vTotalGoodsYTD),
                                    new Tag("v:Total-Goods-TW", _parent.vTotalGoodsTW),
                                    new Tag("v:Total-Goods-CFW", _parent.vTotalGoodsCFW),
                                    new Tag("v:Total-Voucher-YTD", _parent.vTotalVoucherYTD),
                                    new Tag("v:Total-Voucher-TW", _parent.vTotalVoucherTW),
                                    new Tag("v:Total-Voucher-CFW", _parent.vTotalVoucherCFW),
                                    new Tag("SUBTASK2", true, "5"),
                                    new Tag("OUTPUT_FORM_2_8", true, "5"));
                    }
                    protected override void OnStart()
                    {
                        _parent.vBranSumTotsPrt.Value = false;
                        _parent.vBranSumPrt.Value = true;
                        // set up col headings
                        _parent.vReportType.Value = "SUMMARY";
                        _parent.vReportTypeHTML.Value = "Branch Summary";

                        _parent.vStatSummColumn1.Value = "";
                        _parent.vStatSummColumn2.Value = 0;
                        _parent.vStatSummColumn3.Value = 0;
                        _parent.vStatSummColumn4.Value = 0;
                        if (Exp_5())
                        {
                            _parent._viewBranchStockStatement.WriteTo(_ioBranchSummary);
                        }

                        _parent.vTotalGoodsYTD.Value = 0;
                        _parent.vTotalGoodsTW.Value = 0;
                        _parent.vTotalGoodsCFW.Value = 0;
                        _parent.vTotalVoucherYTD.Value = 0;
                        _parent.vTotalVoucherTW.Value = 0;
                        _parent.vTotalVoucherCFW.Value = 0;

                        // report branch goods cum to prev year
                        _parent.vStatSummColumn1.Value = "Cumulative to Prev Year End";
                        _parent.vStatSummColumn2.Value = _parent.vBranGoodsBFWValue;
                        // cols 3 & 4 are not printed when printing cum to prev year
                        _parent.vCumToPrevYearPrinting.Value = true;
                        if (Exp_5())
                        {
                            _viewBranchSummary.WriteTo(_ioBranchSummary);
                        }
                        _parent.vCumToPrevYearPrinting.Value = false;
                        _parent.vTotalGoodsYTD.Value += _parent.vBranGoodsBFWValue;
                        _parent.vTotalGoodsCFW.Value += _parent.vBranGoodsBFWValue;
                    }
                    protected override void OnLeaveRow()
                    {
                        // do while goods type summary codes (first char = A)
                        if (u.Mid(SummaryCodes.SummaryCode, 1, 1) == "A")
                        {
                            _parent.vStatSummColumn1.Value = Exp_11();
                            _parent.vStatSummColumn2.Value = SumCodesExt.BFWValue;
                            _parent.vStatSummColumn3.Value = SumCodesExt.TWKValue;
                            _parent.vStatSummColumn4.Value = SumCodesExt.BFWValue + SumCodesExt.TWKValue;

                            _parent.vTotalGoodsYTD.Value += SumCodesExt.BFWValue;
                            _parent.vTotalGoodsTW.Value += SumCodesExt.TWKValue;
                            _parent.vTotalGoodsCFW.Value = _parent.vTotalGoodsCFW + SumCodesExt.BFWValue + SumCodesExt.TWKValue;

                            if (Exp_5())
                            {
                                _viewBranchSummary.WriteTo(_ioBranchSummary);
                            }

                            vPrevSumCodeType.Value = u.Mid(SummaryCodes.SummaryCode, 1, 1);
                        }

                        // do if sum code type changes (must be b)
                        if (u.Mid(SummaryCodes.SummaryCode, 1, 1) != vPrevSumCodeType)
                        {
                            // report goods totals
                            if (Exp_5())
                            {
                                vBlankRow.Value = true;
                                _viewBranchSummary.WriteTo(_ioBranchSummary);
                                vBlankRow.Value = false;
                            }
                            _parent.vBranSumTotsPrt.Value = true;
                            _parent.vStatSummColumn1.Value = "GOODS TOTAL";
                            _parent.vStatSummColumn2.Value = _parent.vTotalGoodsYTD;
                            _parent.vStatSummColumn3.Value = _parent.vTotalGoodsTW;
                            _parent.vStatSummColumn4.Value = _parent.vTotalGoodsCFW;
                            if (Exp_5())
                            {
                                _viewBranchSummary.WriteTo(_ioBranchSummary);
                                _parent.vBranSumTotsPrt.Value = false;
                                vBlankRow.Value = true;
                                _viewBranchSummary.WriteTo(_ioBranchSummary);
                                vBlankRow.Value = false;
                            }
                            vPrevSumCodeType.Value = u.Mid(SummaryCodes.SummaryCode, 1, 1);

                            // report branch voucher cum to prev year
                            _parent.vStatSummColumn1.Value = "Cumulative to Prev Year End";
                            _parent.vStatSummColumn2.Value = _parent.vBranVouchBFWValue;
                            // cols 3 & 4 are not printed when printing cum to prev year
                            _parent.vCumToPrevYearPrinting.Value = true;
                            if (Exp_5())
                            {
                                _viewBranchSummary.WriteTo(_ioBranchSummary);
                            }
                            _parent.vCumToPrevYearPrinting.Value = false;
                            _parent.vTotalVoucherYTD.Value += _parent.vBranVouchBFWValue;
                            _parent.vTotalVoucherCFW.Value += _parent.vBranVouchBFWValue;
                        }

                        // do for voucher type sum codes
                        if (u.Mid(SummaryCodes.SummaryCode, 1, 1) == "B")
                        {
                            _parent.vStatSummColumn1.Value = Exp_11();
                            _parent.vStatSummColumn2.Value = SumCodesExt.BFWValue;
                            _parent.vStatSummColumn3.Value = SumCodesExt.TWKValue;
                            _parent.vStatSummColumn4.Value = SumCodesExt.BFWValue + SumCodesExt.TWKValue;

                            _parent.vTotalVoucherYTD.Value += SumCodesExt.BFWValue;
                            _parent.vTotalVoucherTW.Value += SumCodesExt.TWKValue;
                            _parent.vTotalVoucherCFW.Value = _parent.vTotalVoucherCFW + SumCodesExt.BFWValue + SumCodesExt.TWKValue;

                            if (Exp_5())
                            {
                                _viewBranchSummary.WriteTo(_ioBranchSummary);
                            }

                        }
                    }
                    protected override void OnEnd()
                    {
                        // do totals for voucher type sum codes and do department print
                        if (Exp_5())
                        {
                            vBlankRow.Value = true;
                            _parent.vBranSumTotsPrt.Value = true;
                            _viewBranchSummary.WriteTo(_ioBranchSummary);
                            vBlankRow.Value = false;
                            _parent.vStatSummColumn1.Value = "VOUCHER TOTAL";
                            _parent.vStatSummColumn2.Value = _parent.vTotalVoucherYTD;
                            _parent.vStatSummColumn3.Value = _parent.vTotalVoucherTW;
                            _parent.vStatSummColumn4.Value = _parent.vTotalVoucherCFW;
                            _viewBranchSummary.WriteTo(_ioBranchSummary);
                            vBlankRow.Value = true;
                            _viewBranchSummary.WriteTo(_ioBranchSummary);
                            vBlankRow.Value = false;
                            // Branch Acceptance
                            _parent.vReportTypeHTML.Value = "Acceptance";
                            _parent._viewBranchStockStatement.WriteTo(_ioBranchAcceptance);
                            _viewBranchAcceptance.WriteTo(_ioBranchAcceptance);
                        }
                    }

                    #region Expressions
                    Bool Exp_5()
                    {
                        return _parent.Branch.BranchStatus == "O" || _parent.Branch.BranchStatus == "C" && _parent.Branch.EffectiveDate > _parent._parent._parent.vLastYearEndDate || _parent.Branch.BranchNumber >= 900;
                    }
                    Text Exp_11()
                    {
                        return @"<A href=""Detail.htm#SummCode" + u.Trim(SummaryCodes.SummaryCode) + @""">" + u.Trim(SummaryCodes.SummaryCodeDecription) + "</A>";
                    }
                    #endregion


                }

                /// <summary>DeptStockValues(P#66.1.1.3)</summary>
                // Last change before Migration: 17/07/2013 15:36:33
                class DeptStockValues : BranchAccounting.BusinessProcessBase
                {

                    #region Models

                    /// <summary>DeptSumExt</summary>
                    readonly Models.DeptSumExt DeptSumExt = new Models.DeptSumExt { ReadOnly = true };
                    #endregion

                    #region Streams

                    /// <summary>DptStockValuesHTML</summary>
                    FileWriter _ioDptStockValuesHTML;
                    #endregion

                    #region Layouts

                    /// <summary>Dept Stock Values HTML</summary>
                    TextTemplate _viewDeptStockValuesHTML;
                    #endregion

                    RangeThruBranch _parent;


                    /// <summary>DeptStockValues(P#66.1.1.3)</summary>
                    public DeptStockValues(RangeThruBranch parent)
                    {
                        _parent = parent;
                        Title = "DeptStockValues";
                        InitializeDataView();
                    }
                    void InitializeDataView()
                    {
                        From = DeptSumExt;
                        Where.Add(DeptSumExt.BranchNumber.IsEqualTo(_parent.Branch.BranchNumber));
                        Where.Add(DeptSumExt.DepartmentCode.IsBetween(10, 99));
                        OrderBy = DeptSumExt.SortByBAC_Dept_Sum_Ext_X1;

                        #region Columns

                        Columns.Add(DeptSumExt.BranchNumber);
                        // Exclude Vouchers (Dept 0)
                        Columns.Add(DeptSumExt.DepartmentCode);
                        Columns.Add(DeptSumExt.Value);
                        #endregion
                    }

                    #region Run Overloads

                    /// <summary>DeptStockValues</summary>
                    internal void Run()
                    {
                        Execute();
                    }
                    #endregion

                    protected override void OnLoad()
                    {
                        KeepChildRelationCacheAlive = true;

                        _ioDptStockValuesHTML = new FileWriter("%AIXserver%" + u.Trim(_parent.vAIXPath) + "/DeptSumm.htm")
                        {
                            Name = "DptStockValuesHTML"
                        };
                        _ioDptStockValuesHTML.Open();
                        Streams.Add(_ioDptStockValuesHTML);

                        _viewDeptStockValuesHTML = new TextTemplate("%MagicApps%bc/html/bc213.html");
                        _viewDeptStockValuesHTML.Add(
                                    new Tag("SUBTASK3", true, "5"),
                                    new Tag("v:Dept-1-Stock-Value", _parent.vDept1StockValue),
                                    new Tag("v:Dept-2-Stock-Value", _parent.vDept2StockValue),
                                    new Tag("v:Dept-3-Stock-Value", _parent.vDept3StockValue),
                                    new Tag("v:Dept-4-Stock-Value", _parent.vDept4StockValue),
                                    new Tag("v:Dept-5-Stock-Value", _parent.vDept5StockValue),
                                    new Tag("v:Dept-6-Stock-Value", _parent.vDept6StockValue),
                                    new Tag("v:Dept-7-Stock-Value", _parent.vDept7StockValue),
                                    new Tag("v:Dept-8-Stock-Value", _parent.vDept8StockValue),
                                    new Tag("v:Dept-9-Stock-Value", _parent.vDept9StockValue),
                                    new Tag("Total", () => u.LTrim(u.Str(_parent.vDept1StockValue + _parent.vDept2StockValue + _parent.vDept3StockValue + _parent.vDept4StockValue + _parent.vDept5StockValue + _parent.vDept6StockValue + _parent.vDept7StockValue + _parent.vDept8StockValue + _parent.vDept9StockValue, "N8.2")), "15"));
                    }
                    protected override void OnStart()
                    {
                        _parent.vDept1StockValue.Value = 0;
                        _parent.vDept2StockValue.Value = 0;
                        _parent.vDept3StockValue.Value = 0;
                        _parent.vDept4StockValue.Value = 0;
                        _parent.vDept5StockValue.Value = 0;
                        _parent.vDept6StockValue.Value = 0;
                        _parent.vDept7StockValue.Value = 0;
                        _parent.vDept8StockValue.Value = 0;
                        _parent.vDept9StockValue.Value = 0;

                        _parent.vReportTypeHTML.Value = "Department Stock Values";
                    }
                    protected override void OnLeaveRow()
                    {
                        if (u.Left(u.Str(DeptSumExt.DepartmentCode, "2P0"), 1) == "1")
                        {
                            _parent.vDept1StockValue.Value += DeptSumExt.Value;
                        }
                        if (u.Left(u.Str(DeptSumExt.DepartmentCode, "2P0"), 1) == "2")
                        {
                            _parent.vDept2StockValue.Value += DeptSumExt.Value;
                        }
                        if (u.Left(u.Str(DeptSumExt.DepartmentCode, "2P0"), 1) == "3")
                        {
                            _parent.vDept3StockValue.Value += DeptSumExt.Value;
                        }
                        if (u.Left(u.Str(DeptSumExt.DepartmentCode, "2P0"), 1) == "4")
                        {
                            _parent.vDept4StockValue.Value += DeptSumExt.Value;
                        }
                        if (u.Left(u.Str(DeptSumExt.DepartmentCode, "2P0"), 1) == "5")
                        {
                            _parent.vDept5StockValue.Value += DeptSumExt.Value;
                        }
                        if (u.Left(u.Str(DeptSumExt.DepartmentCode, "2P0"), 1) == "6")
                        {
                            _parent.vDept6StockValue.Value += DeptSumExt.Value;
                        }
                        if (u.Left(u.Str(DeptSumExt.DepartmentCode, "2P0"), 1) == "7")
                        {
                            _parent.vDept7StockValue.Value += DeptSumExt.Value;
                        }
                        if (u.Left(u.Str(DeptSumExt.DepartmentCode, "2P0"), 1) == "8")
                        {
                            _parent.vDept8StockValue.Value += DeptSumExt.Value;
                        }
                        if (u.Left(u.Str(DeptSumExt.DepartmentCode, "2P0"), 1) == "9")
                        {
                            _parent.vDept9StockValue.Value += DeptSumExt.Value;
                        }
                    }
                    protected override void OnEnd()
                    {
                        if (_parent.Branch.BranchStatus == "O" || _parent.Branch.BranchStatus == "C" && _parent.Branch.EffectiveDate > _parent._parent._parent.vLastYearEndDate || _parent.Branch.BranchNumber >= 900)
                        {
                            _parent._viewBranchStockStatement.WriteTo(_ioDptStockValuesHTML);
                            _viewDeptStockValuesHTML.WriteTo(_ioDptStockValuesHTML);
                        }
                    }


                }
            }
        }
    }
}
