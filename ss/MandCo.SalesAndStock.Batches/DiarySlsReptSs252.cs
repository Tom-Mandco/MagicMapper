using Firefly.Box;
using ENV.Data;
using ENV;
using ENV.IO;
using Firefly.Box.Flow;

namespace MandCo.SalesAndStock.Batches
{
    /// <summary>Diary Sls Rept (ss252)(P#62)</summary>
    /// <remarks>
    /// Last change before Migration: 21/06/2011 14:08:16
    /// Last change post-migration: 14/04/2016 16:00:00
    /// 
    /// Brief summary: 
    /// This program produces a report based on the data in [SLA_SectSales]
    /// The parameters passed should be in the following format: YYMM, {no of days}, YYYYMM, {Passed from AIX}
    /// {no of days} = number of days since 01/01/0000. For 12/01/2015 use 735610.
    /// {Passed from AIX} = "A" if passed, any other string passed in here will do nothing.
    /// </remarks>

    public class DiarySlsReptSs252 : SalesAndStock.BusinessProcessBase
    {
        
        #region Models
        /// <summary>Date</summary>
        readonly Models.Date1 Date1 = new Models.Date1 { ReadOnly = true };
        /// <summary>Season</summary>
        readonly Models.Season Season = new Models.Season { ReadOnly = true };
        /// <summary>Date</summary>
        readonly Models.Date1 Date11 = new Models.Date1 { ReadOnly = true };
        /// <summary>Date</summary>
        readonly Models.Date1 Date12 = new Models.Date1 { ReadOnly = true };
        /// <summary>Date</summary>
        readonly Models.Date1 Date13 = new Models.Date1 { ReadOnly = true };
        /// <summary>Date</summary>
        readonly Models.Date1 Date14 = new Models.Date1 { ReadOnly = true };
        #endregion
        
        #region Columns
        /// <summary>p:YYWW</summary>
        readonly NumberColumn pYYWW = new NumberColumn("p:YYWW", "4P0A");
        /// <summary>p:WE Date</summary>
        readonly DateColumn pWEDate = new DateColumn("p:WE Date");
        /// <summary>p:YYYYWW</summary>
        readonly NumberColumn pYYYYWW = new NumberColumn("p:YYYYWW", "6P0");
        /// <summary>p:AixMagic</summary>
        readonly TextColumn pAixMagic = new TextColumn("p:AixMagic", "U");
        /// <summary>v:NbrWeeks</summary>
        readonly NumberColumn vNbrWeeks = new NumberColumn("v:NbrWeeks", "2P0");
        /// <summary>v:TYweekStartDate</summary>
        internal readonly DateColumn vTYweekStartDate = new DateColumn("v:TYweekStartDate");
        /// <summary>v:TYweekEndDate</summary>
        internal readonly DateColumn vTYweekEndDate = new DateColumn("v:TYweekEndDate");
        /// <summary>v:TYreqWeek</summary>
        internal readonly NumberColumn vTYreqWeek = new NumberColumn("v:TYreqWeek", "6P0");
        /// <summary>v:YY01</summary>
        readonly NumberColumn vYY01 = new NumberColumn("v:YY01", "4P0");
        /// <summary>v:TYyearStartDate</summary>
        internal readonly DateColumn vTYyearStartDate = new DateColumn("v:TYyearStartDate");
        /// <summary>v:TYyearStartWeek</summary>
        internal readonly NumberColumn vTYyearStartWeek = new NumberColumn("v:TYyearStartWeek", "6");
        /// <summary>v:TYseasStartWeek</summary>
        internal readonly NumberColumn vTYseasStartWeek = new NumberColumn("v:TYseasStartWeek", "6");
        /// <summary>v:TYseasEndWeek</summary>
        readonly NumberColumn vTYseasEndWeek = new NumberColumn("v:TYseasEndWeek", "6");
        /// <summary>v:TYseasStartDate</summary>
        internal readonly DateColumn vTYseasStartDate = new DateColumn("v:TYseasStartDate");
        /// <summary>v:TYseasEndDate</summary>
        readonly DateColumn vTYseasEndDate = new DateColumn("v:TYseasEndDate");
        /// <summary>v:TempWW</summary>
        readonly NumberColumn vTempWW = new NumberColumn("v:TempWW", "2");
        /// <summary>v:TempPeriod</summary>
        readonly NumberColumn vTempPeriod = new NumberColumn("v:TempPeriod", "2.2");
        /// <summary>v:Period</summary>
        readonly NumberColumn vPeriod = new NumberColumn("v:Period", "2");
        /// <summary>v:TYperiodStartDate</summary>
        internal readonly DateColumn vTYperiodStartDate = new DateColumn("v:TYperiodStartDate");
        /// <summary>v:TYperiodStartWeek</summary>
        internal readonly NumberColumn vTYperiodStartWeek = new NumberColumn("v:TYperiodStartWeek", "6");
        /// <summary>v:PYweekStartDate</summary>
        internal readonly DateColumn vPYweekStartDate = new DateColumn("v:PYweekStartDate");
        /// <summary>v:PYweekEndDate</summary>
        internal readonly DateColumn vPYweekEndDate = new DateColumn("v:PYweekEndDate");
        /// <summary>v:PYperiodStartDate</summary>
        internal readonly DateColumn vPYperiodStartDate = new DateColumn("v:PYperiodStartDate");
        /// <summary>v:PYseasonStartDate</summary>
        internal readonly DateColumn vPYseasonStartDate = new DateColumn("v:PYseasonStartDate");
        /// <summary>v:PYyearStartDate</summary>
        internal readonly DateColumn vPYyearStartDate = new DateColumn("v:PYyearStartDate");
        /// <summary>v:DateTimeStamp</summary>
        readonly TextColumn vDateTimeStamp = new TextColumn("v:DateTimeStamp", "X19");
        /// <summary>v:Days Diff</summary>
        internal readonly NumberColumn vDaysDiff = new NumberColumn("v:Days Diff", "9");
        /// <summary>v:PrevWeek</summary>
        readonly NumberColumn vPrevWeek = new NumberColumn("v:PrevWeek", "4P0");
        /// <summary>v:Weeks Into Period</summary>
        readonly NumberColumn vWeeksIntoPeriod = new NumberColumn("v:Weeks Into Period", "2P0");
        /// <summary>v:Weeks Into Season</summary>
        readonly NumberColumn vWeeksIntoSeason = new NumberColumn("v:Weeks Into Season", "2P0");
        /// <summary>v:Weeks Into Year</summary>
        readonly NumberColumn vWeeksIntoYear = new NumberColumn("v:Weeks Into Year", "2P0");
        #endregion
        
        public DiarySlsReptSs252()
        {
            Title = "Diary Sls Rept (ss252)";
            InitializeDataView();
        }
        void InitializeDataView()
        {
            
            #region Relations
            
            Relations.Add(Date1, 
            		Date1.WeekNumber.IsEqualTo(vYY01), 
            	Date1.SortByREF_Date_X1);
            
            Relations.Add(Season, 
            		Season.SeasonEndCenturyWeek.IsGreaterOrEqualTo(pYYYYWW), 
            	Season.SortByREF_Season_X6);
            
            Relations.Add(Date11, 
            		Date11.CenturyWeek.IsEqualTo(vTYseasStartWeek), 
            	Date11.SortByREF_Date_X3);
            
            Relations.Add(Date12, 
            		Date12.CenturyWeek.IsEqualTo(vTYseasEndWeek), 
            	Date12.SortByREF_Date_X3);
            
            Relations.Add(Date13, 
            		Date13.WeekEndingDate.IsGreaterOrEqualTo(vTYperiodStartDate), 
            	Date13.SortByREF_Date_X2);
            
            Relations.Add(Date14, 
            		Date14.WeekEndingDate.IsEqualTo(() => u.AddDate(pWEDate, 0, 0, -(7))), 
            	Date14.SortByREF_Date_X2);
            
            #endregion
            
            
            #region Columns
            
            // The following parameters are received from the calling program,
            // ss252A (AIX interface) or ss252M (MAGIC interface to run on-line - for
            // testing)
            Columns.Add(pYYWW);
            Columns.Add(pWEDate);
            Columns.Add(pYYYYWW);
            Columns.Add(pAixMagic);
            
            Columns.Add(vNbrWeeks).BindValue(() => u.Fix(pYYWW, 2, 0));
            Columns.Add(vTYweekStartDate).BindValue(() => u.AddDate(pWEDate, 0, 0, -(6)));
            Columns.Add(vTYweekEndDate);
            Columns.Add(vTYreqWeek).BindValue(pYYYYWW);
            // Calc start of year entered
            Columns.Add(vYY01).BindValue(() => u.Val(u.Left(u.Str(pYYWW, "4P0"), 2) + "01", "4P0"));
            Columns.Add(Date1.WeekNumber);
            Columns.Add(Date1.WeekEndingDate);
            Columns.Add(Date1.CenturyWeek);
            Columns.Add(vTYyearStartDate).BindValue(() => u.AddDate(Date1.WeekEndingDate, 0, 0, -(6)));
            Columns.Add(vTYyearStartWeek).BindValue(Date1.CenturyWeek);
            // Determine Season details for week requested
            Columns.Add(Season.SeasonEndCenturyWeek);
            Columns.Add(Season.SeasonStartCenturyWeek);
            Columns.Add(vTYseasStartWeek).BindValue(Season.SeasonStartCenturyWeek);
            Columns.Add(vTYseasEndWeek).BindValue(Season.SeasonEndCenturyWeek);
            Columns.Add(Date11.CenturyWeek);
            Columns.Add(Date11.WeekEndingDate);
            Columns.Add(vTYseasStartDate).BindValue(() => u.AddDate(Date11.WeekEndingDate, 0, 0, -(6)));
            Columns.Add(Date12.CenturyWeek);
            Columns.Add(Date12.WeekEndingDate);
            Columns.Add(vTYseasEndDate).BindValue(Date12.WeekEndingDate);
            
            
            // Determine Period details for week requested
            Columns.Add(vTempWW).BindValue(() => u.Fix(pYYWW, 2, 0));
            Columns.Add(vTempPeriod).BindValue(() => vTempWW / 4);
            Columns.Add(vPeriod).BindValue(() => u.If(vTempWW == 53, 13, u.If(u.Fix(vTempPeriod, 0, 2) > 0, u.Fix(vTempPeriod, 2, 0) + 1, u.Fix(vTempPeriod, 2, 0))));
            Columns.Add(vTYperiodStartDate).BindValue(() => u.AddDate(vTYyearStartDate, 0, 0, (vPeriod - 1) * 28));
            Columns.Add(Date13.WeekEndingDate);
            Columns.Add(Date13.CenturyWeek);
            Columns.Add(vTYperiodStartWeek).BindValue(Date13.CenturyWeek);
            // Calc corresponding dates for previous year
            Columns.Add(vPYweekStartDate).BindValue(() => u.AddDate(vTYweekStartDate, 0, 0, -(364)));
            Columns.Add(vPYweekEndDate);
            Columns.Add(vPYperiodStartDate).BindValue(() => u.AddDate(vTYperiodStartDate, 0, 0, -(364)));
            Columns.Add(vPYseasonStartDate).BindValue(() => u.AddDate(vTYseasStartDate, 0, 0, -(364)));
            Columns.Add(vPYyearStartDate).BindValue(() => u.AddDate(vTYyearStartDate, 0, 0, -(364)));
            
            Columns.Add(vDateTimeStamp);
            Columns.Add(vDaysDiff);
            
            // Determine week number prior to the one requested.   This is used to
            // EMail that week's data to N. Bennet together with the Saturday figures
            // for the current week
            Columns.Add(Date14.WeekEndingDate);
            Columns.Add(Date14.WeekNumber);
            Columns.Add(vPrevWeek).BindValue(Date14.WeekNumber);
            
            Columns.Add(vWeeksIntoPeriod).BindValue(() => vTYreqWeek - vTYperiodStartWeek + 1);
            Columns.Add(vWeeksIntoSeason).BindValue(() => vTYreqWeek - vTYseasStartWeek + 1);
            Columns.Add(vWeeksIntoYear).BindValue(() => u.Fix(pYYWW, 2, 0));
            #endregion
        }
        /// <summary>Diary Sls Rept (ss252)(P#62)</summary>
        #region Parameters Original Names
        /// <param name="ppYYWW">p:YYWW</param>
        /// <param name="ppWEDate">p:WE Date</param>
        /// <param name="ppYYYYWW">p:YYYYWW</param>
        /// <param name="ppAixMagic">p:AixMagic</param>
        #endregion
        public void Run(NumberParameter ppYYWW, DateParameter ppWEDate, NumberParameter ppYYYYWW, TextParameter ppAixMagic)
        {
            #region Bind Parameters
            
            BindParameter(pYYWW, ppYYWW);
            BindParameter(pWEDate, ppWEDate);
            BindParameter(pYYYYWW, ppYYYYWW);
            BindParameter(pAixMagic, ppAixMagic);
            #endregion
            Execute();
        }
        protected override void OnLoad()
        {
            Exit(ExitTiming.AfterRow);
            if(NewViewRequired)
            {
                View = ()=> new Views.DiarySlsReptSs252SalesVarReportsSs242(this);
            }
        }
        protected override void OnStart()
        {
            Message.ShowWarningInStatusBar("ss252");
        }
        protected override void OnLeaveRow()
        {
            vTYweekEndDate.Value = pWEDate;
            vPYweekEndDate.Value = u.AddDate(vTYweekEndDate, 0, 0, -(364));
            vDateTimeStamp.Value = u.DStr(Date.Now, "DD/MM/YYYY") + " at " + u.TStr(Time.Now, "HH:MM");
            // A value of 6 calculated in the following field denotes the run
            // includes the whole week.
            vDaysDiff.Value = vTYweekEndDate - vTYweekStartDate;
            new Control(this).Run();
        }
        protected override void OnEnd()
        {
            Message.ShowWarningInStatusBar("ss252    -  Program ends");
            Message.ShowWarningInStatusBar("ss252");
        }
        
        
        /// <summary>Control(P#62.1)</summary>
        /// <remark>Last change before Migration: 21/06/2011 13:52:20</remark>
        internal class Control : SalesAndStock.BusinessProcessBase 
        {
            
            #region Models
            /// <summary>Date</summary>
            readonly Models.Date1 Date1 = new Models.Date1 { ReadOnly = true };
            /// <summary>Date</summary>
            readonly Models.Date1 Date11 = new Models.Date1 { ReadOnly = true };
            /// <summary>Date</summary>
            readonly Models.Date1 Date12 = new Models.Date1 { ReadOnly = true };
            /// <summary>Date</summary>
            readonly Models.Date1 Date13 = new Models.Date1 { ReadOnly = true };
            #endregion
            
            #region Columns
            /// <summary>v:PYreqWeek</summary>
            readonly NumberColumn vPYreqWeek = new NumberColumn("v:PYreqWeek", "6P0");
            /// <summary>v:PYperiodStartWeek</summary>
            readonly NumberColumn vPYperiodStartWeek = new NumberColumn("v:PYperiodStartWeek", "6P0");
            /// <summary>v:PYseasStartWeek</summary>
            readonly NumberColumn vPYseasStartWeek = new NumberColumn("v:PYseasStartWeek", "6P0");
            /// <summary>v:PYyearStartWeek</summary>
            readonly NumberColumn vPYyearStartWeek = new NumberColumn("v:PYyearStartWeek", "6P0");
            /// <summary>v:MsgToUser</summary>
            readonly TextColumn vMsgToUser = new TextColumn("v:MsgToUser", "X70");
            /// <summary>v:FromDatePrevWeek</summary>
            readonly TextColumn vFromDatePrevWeek = new TextColumn("v:FromDatePrevWeek", "X11");
            /// <summary>v:ToDatePrevWeek</summary>
            readonly TextColumn vToDatePrevWeek = new TextColumn("v:ToDatePrevWeek", "X11");
            /// <summary>v:WorkTableName</summary>
            readonly TextColumn vWorkTableName = new TextColumn("v:WorkTableName", "X20");
            /// <summary>v:WebDir</summary>
            readonly TextColumn vWebDir = new TextColumn("v:WebDir", "30");
            /// <summary>v:BaseDir</summary>
            readonly TextColumn vBaseDir = new TextColumn("v:BaseDir", "60");
            /// <summary>v:TargetDir</summary>
            readonly TextColumn vTargetDir = new TextColumn("v:TargetDir", "70");
            /// <summary>v:TargetFile</summary>
            readonly TextColumn vTargetFile = new TextColumn("v:TargetFile", "70");
            /// <summary>v:TargetDirSuffix</summary>
            readonly TextColumn vTargetDirSuffix = new TextColumn("v:TargetDirSuffix", "60");
            /// <summary>v:HdrWeek</summary>
            readonly TextColumn vHdrWeek = new TextColumn("v:HdrWeek", "20");
            /// <summary>v:HdrBranch</summary>
            readonly TextColumn vHdrBranch = new TextColumn("v:HdrBranch", "55");
            /// <summary>v:HdrRunDate</summary>
            readonly TextColumn vHdrRunDate = new TextColumn("v:HdrRunDate", "20");
            #endregion
            
            DiarySlsReptSs252 _parent;
            
            public Control(DiarySlsReptSs252 parent)
            {
                _parent = parent;
                Title = "Control";
                InitializeDataView();
            }
            void InitializeDataView()
            {
                
                #region Relations
                
                Relations.Add(Date1, 
                		Date1.WeekEndingDate.IsGreaterOrEqualTo(_parent.vPYweekStartDate), 
                	Date1.SortByREF_Date_X2);
                
                Relations.Add(Date11, 
                		Date11.WeekEndingDate.IsGreaterOrEqualTo(_parent.vPYperiodStartDate), 
                	Date11.SortByREF_Date_X2);
                
                Relations.Add(Date12, 
                		Date12.WeekEndingDate.IsGreaterOrEqualTo(_parent.vPYseasonStartDate), 
                	Date12.SortByREF_Date_X2);
                
                Relations.Add(Date13, 
                		Date13.WeekEndingDate.IsGreaterOrEqualTo(_parent.vPYyearStartDate), 
                	Date13.SortByREF_Date_X2);
                
                #endregion
                
                
                #region Columns
                
                // Determine relevant week numbers for previous year from PY dates
                // already calculated in task above.
                
                Columns.Add(Date1.WeekEndingDate);
                Columns.Add(Date1.CenturyWeek);
                Columns.Add(vPYreqWeek).BindValue(Date1.CenturyWeek);
                
                Columns.Add(Date11.WeekEndingDate);
                Columns.Add(Date11.CenturyWeek);
                Columns.Add(vPYperiodStartWeek).BindValue(Date11.CenturyWeek);
                
                Columns.Add(Date12.WeekEndingDate);
                Columns.Add(Date12.CenturyWeek);
                Columns.Add(vPYseasStartWeek).BindValue(Date12.CenturyWeek);
                
                Columns.Add(Date13.WeekEndingDate);
                Columns.Add(Date13.CenturyWeek);
                Columns.Add(vPYyearStartWeek).BindValue(Date13.CenturyWeek);
                
                Columns.Add(vMsgToUser);
                Columns.Add(vFromDatePrevWeek).BindValue(() => u.DStr(u.AddDate(_parent.vTYweekStartDate, 0, 0, -(7)), "DD-MMM-YYYY"));
                Columns.Add(vToDatePrevWeek).BindValue(() => u.DStr(u.AddDate(_parent.vTYweekEndDate, 0, 0, -(7)), "DD-MMM-YYYY"));
                Columns.Add(vWorkTableName).BindValue(() => u.DBName(typeof(Models.ss252AreaWork)));
                
                Columns.Add(vWebDir).BindValue(() => u.IniGet("[MAGIC_LOGICAL_NAMES]web"));
                Columns.Add(vBaseDir).BindValue(() => u.Trim(vWebDir) + "branch/");
                Columns.Add(vTargetDir);
                Columns.Add(vTargetFile);
                Columns.Add(vTargetDirSuffix).BindValue(() => "/wwwroot/rpt/sls/" + u.Str(_parent.pYYWW, "4P0"));
                // Report header fields
                Columns.Add(vHdrWeek).BindValue(() => " Week " + u.Str(u.Fix(_parent.vTYreqWeek, 2, 0), "2P0") + " - Year " + u.Left(u.Str(_parent.vTYreqWeek, "6P0"), 4));
                Columns.Add(vHdrBranch);
                Columns.Add(vHdrRunDate).BindValue(() => u.DStr(Date.Now, "DD/MM/YYYY") + " at " + u.TStr(Time.Now, "HH:MM"));
                
                #endregion
            }
            /// <summary>Control(P#62.1)</summary>
            internal void Run()
            {
                Execute();
            }
            protected override void OnLoad()
            {
                Exit(ExitTiming.AfterRow);
                if(NewViewRequired)
                {
                    View = ()=> new Views.DiarySlsReptSs252Control(this);
                }
            }
            protected override void OnStart()
            {
                u.SetCrsr(4);
                // Clear Oracle  & memory work tables
                Message.ShowWarningInStatusBar("ss252    -  Clearing work tables");
                vMsgToUser.Value = "Clearing work tables";
                u.Delay(1);
                Cached<DeleteDiaryWork1>().Run();
                vWorkTableName.Value = u.DBName(typeof(Models.ss252AreaWork));
                Cached<SqlTruncateWork>().Run();
                vWorkTableName.Value = u.DBName(typeof(Models.ss252BranchWork));
                Cached<SqlTruncateWork>().Run();
                
                // Truncate altered ( re Season data) work tables
                vWorkTableName.Value = u.DBName(typeof(Models.ss252AreaWorkMk2));
                Cached<SqlTruncateWork>().Run();
                vWorkTableName.Value = u.DBName(typeof(Models.ss252BranchWorkMk2));
                Cached<SqlTruncateWork>().Run();
                
                vWorkTableName.Value = u.DBName(typeof(Models.ss252PrintWork));
                Cached<SqlTruncateWork>().Run();
            }
            protected override void OnLeaveRow()
            {
                // Assemble Sales and Budget data
                vMsgToUser.Value = "Assembling Sales & Budget data";
                Cached<GetBranchData>().Run();
            }
            protected override void OnEnd()
            {
                u.SetCrsr(1);
            }
            
            #region Expressions
            internal Text Exp_27()
            {
                return vMsgToUser;
            }
            #endregion
            
            
            /// <summary>Sql Truncate Work(P#62.1.1)</summary>
            /// <remark>Last change before Migration: 25/06/2004 14:59:57</remark>
            class SqlTruncateWork : SalesAndStock.BusinessProcessBase 
            {
                
                #region Models
                DynamicSQLEntity sqlEntity;
                #endregion
                
                Control _parent;
                
                public SqlTruncateWork(Control parent)
                {
                    _parent = parent;
                    Title = "Sql Truncate Work";
                    InitializeDataView();
                }
                void InitializeDataView()
                {
                    sqlEntity = new DynamicSQLEntity(Models.DataSources.SS, 
@"--  Called by task 'Sql Truncate Work' in MAGIC program ss252
--
TRUNCATE TABLE :1");
                    sqlEntity.AddParameter(() => u.Trim(_parent.vWorkTableName)); //:1;
                    From = sqlEntity;
                    
                    
                    #region Columns
                    
                    // SQL task to truncate table ss_sale_var_work
                    #endregion
                }
                /// <summary>Sql Truncate Work(P#62.1.1)</summary>
                internal void Run()
                {
                    Execute();
                }
                protected override void OnLoad()
                {
                    Exit(ExitTiming.AfterRow);
                    TransactionScope = TransactionScopes.Task;
                    KeepChildRelationCacheAlive = true;
                    AllowInsert = false;
                }
                
                
            }
            /// <summary>Delete Diary Work1(P#62.1.2)</summary>
            /// <remark>Last change before Migration: 12/05/2006 09:51:04</remark>
            class DeleteDiaryWork1 : SalesAndStock.BusinessProcessBase 
            {
                
                #region Models
                DynamicSQLEntity sqlEntity;
                #endregion
                
                public DeleteDiaryWork1()
                {
                    Title = "Delete Diary Work1";
                    InitializeDataView();
                }
                void InitializeDataView()
                {
                    sqlEntity = new DynamicSQLEntity(Models.DataSources.SS, 
@"--  Called by task 'Delete Diary Work1' in MAGIC program ss252
--
delete ss_diary_work1 d
 where d.prog_no = 'ss252'");
                    From = sqlEntity;
                    
                    
                }
                /// <summary>Delete Diary Work1(P#62.1.2)</summary>
                internal void Run()
                {
                    Execute();
                }
                protected override void OnLoad()
                {
                    Exit(ExitTiming.AfterRow);
                    TransactionScope = TransactionScopes.Task;
                    AllowInsert = false;
                    AllowUserAbort = true;
                }
                
                
            }
            /// <summary>Get Branch Data(P#62.1.3)</summary>
            /// <remark>Last change before Migration: 21/06/2011 13:52:20</remark>
            internal class GetBranchData : SalesAndStock.BusinessProcessBase 
            {
                
                #region Models
                /// <summary>Branch</summary>
                readonly Models.Branch Branch = new Models.Branch { ReadOnly = true };
                /// <summary>Branch Floor Codes</summary>
                readonly Models.BranchFloorCodes BranchFloorCodes = new Models.BranchFloorCodes { ReadOnly = true };
                /// <summary>Analysis Group 1 Depts</summary>
                readonly Models.AnalysisGroup1Depts AnalysisGroup1Depts = new Models.AnalysisGroup1Depts { ReadOnly = true };
                /// <summary>ss252 Area Work</summary>
                readonly Models.ss252AreaWork ss252AreaWork = new Models.ss252AreaWork { AllowRowLocking = true };
                /// <summary>ss252 Branch Work</summary>
                readonly Models.ss252BranchWork ss252BranchWork = new Models.ss252BranchWork { AllowRowLocking = true };
                /// <summary>Diary Work1</summary>
                readonly Models.DiaryWork1 DiaryWork1 = new Models.DiaryWork1 { AllowRowLocking = true };
                /// <summary>ss252 Print Work</summary>
                readonly Models.ss252PrintWork ss252PrintWork = new Models.ss252PrintWork { AllowRowLocking = true };
                #endregion
                
                #region Columns
                /// <summary>v:SQL Ran OK</summary>
                readonly BoolColumn vSQLRanOK = new BoolColumn("v:SQL Ran OK");
                /// <summary>v:DBerr</summary>
                readonly TextColumn vDBerr = new TextColumn("v:DBerr", "120");
                /// <summary>v:SQL Task</summary>
                readonly TextColumn vSQLTask = new TextColumn("v:SQL Task", "30");
                #endregion
                
                Control _parent;
                
                public GetBranchData(Control parent)
                {
                    _parent = parent;
                    Title = "Get Branch Data";
                    Entities.Add(Branch);
                    Entities.Add(BranchFloorCodes);
                    Entities.Add(AnalysisGroup1Depts);
                    Entities.Add(ss252AreaWork);
                    Entities.Add(ss252BranchWork);
                    Entities.Add(DiaryWork1);
                    Entities.Add(ss252PrintWork);
                    InitializeDataView();
                }
                void InitializeDataView()
                {
                    
                    
                    #region Columns
                    
                    // This task controls the assembly of weekly, period and YTD sales
                    // figures for this year and last year.
                    
                    Columns.Add(vSQLRanOK);
                    Columns.Add(vDBerr);
                    Columns.Add(vSQLTask);
                    #endregion
                }
                /// <summary>Get Branch Data(P#62.1.3)</summary>
                internal void Run()
                {
                    Execute();
                }
                protected override void OnLoad()
                {
                    Exit(ExitTiming.AfterRow);
                    AllowUserAbort = true;
                }
                protected override void OnLeaveRow()
                {
                    
                    // Get TY sales figures with corrresponding LY figures
                    _parent.vMsgToUser.Value = "Assembling sales for this year";
                    Message.ShowWarningInStatusBar("ss252    -  Fetching TY Sales");
                    vSQLTask.Value = "SQL Insert TY Sales";
                    Cached<SQLInsertTYSales>().Run();
                    if(vSQLRanOK)
                    {
                        
                        _parent.vMsgToUser.Value = "Assembling sales for last year";
                        Message.ShowWarningInStatusBar("ss252    -  Fetching LY Sales");
                        Cached<UpdateLY>().Run();
                        
                        // Assemble Bay Counts
                        _parent.vMsgToUser.Value = "Assembling Bay Counts";
                        Message.ShowWarningInStatusBar("ss252    -  Assembling Bay Counts");
                        Cached<EncodeBayTotals>().Run();
                        // Set up control list of branches having sales
                        Message.ShowWarningInStatusBar("ss252    -  Setting up List of Branches");
                        vSQLTask.Value = "Set Up Branch List";
                        Cached<SetUpBranchList>().Run();
                    }
                    // Merge sales details from Work1 to Work2
                    if(vSQLRanOK)
                    {
                        _parent.vMsgToUser.Value = "Insert dummies for Missing Depts";
                        Message.ShowWarningInStatusBar("ss252    -  Missing Depts");
                        vSQLTask.Value = "Missing Depts";
                        Cached<MissingDepts>().Run();
                    }
                    // Encode budget figures to Work2
                    if (vSQLRanOK)
                    {
                        _parent.vMsgToUser.Value = "Assembling budgets by reporting department";
                        Message.ShowWarningInStatusBar("ss252    -  Encoding Budgets");
                        vSQLTask.Value = "Encode Budgets";
                        Cached<EncodeBudgets>().Run();
                    }
                    // Generate sales totals by Area
                    if(vSQLRanOK)
                    {
                        _parent.vMsgToUser.Value = "Assembling area sales totals for COMP stores";
                        Message.ShowWarningInStatusBar("ss252    -  Calculating Area Sales");
                        vSQLTask.Value = "Area Sales";
                        Cached<AreaTotals>().Run();
                    }
                    if(vSQLRanOK)
                    {
                        // Move departmental figures to Print Work table
                        _parent.vMsgToUser.Value = "Move Dept sales to Print Work";
                        Message.ShowWarningInStatusBar("ss252    -  Move Dept Sales to Print Work");
                        Cached<DeptsToPrtWork>().Run();
                    }
                    // Assemble Mackays Card sales figures
                    if(vSQLRanOK)
                    {
                        _parent.vMsgToUser.Value = "Assemble Mackays Card sales figures";
                        Message.ShowWarningInStatusBar("ss252    -  Assemble Mackays Card sales figures");
                        Cached<MackaysCard>().Run();
                    }
                    // Assemble Customer Count figures
                    if(vSQLRanOK)
                    {
                        _parent.vMsgToUser.Value = "Assemble Customer Counts";
                        Message.ShowWarningInStatusBar("ss252    -  Assemble Customer Counts");
                        Cached<CustomerCounts>().Run();
                    }
                    // Assemble Concession figures
                    if(vSQLRanOK)
                    {
                        _parent.vMsgToUser.Value = "Assemble Concession figures";
                        Message.ShowWarningInStatusBar("ss252    -  Assemble Concession figures");
                        Cached<Concessions>().Run();
                    }
                    // Assemble Avg. Unit Sales figures
                    if(vSQLRanOK)
                    {
                        _parent.vMsgToUser.Value = "Assemble Average Sales figures";
                        Message.ShowWarningInStatusBar("ss252    -  Assemble Average Sales figures");
                        new SoldUnits(this).Run();
                    }
                    // Generate HTML report files
                    if(vSQLRanOK)
                    {
                        _parent.vMsgToUser.Value = "Generating HTML files";
                        Message.ShowWarningInStatusBar("ss252    -  Creating HTML files in AIX");
                        Cached<CreateHTMLFiles>().Run();
                    }
                    // Get details of SQL insert/update error
                    if(u.Not(vSQLRanOK))
                    {
                        vDBerr.Value = u.DbERR("SS");
                        Message.ShowWarningInStatusBar("ss252   SQL error has occurred in task " + u.Trim(vSQLTask));
                        Message.ShowWarningInStatusBar("ss252     " + u.Left(vDBerr, 60));
                        Message.ShowWarningInStatusBar("ss252     " + u.Mid(vDBerr, 61, 60));
                        Message.ShowErrorInStatusBar("ss252               JOB ABORTS");
                    }
                }
                
                
                /// <summary>Depts To Prt Work(P#62.1.3.1)</summary>
                /// <remark>Last change before Migration: 21/06/2011 13:52:20</remark>
                internal class DeptsToPrtWork : SalesAndStock.BusinessProcessBase 
                {
                    
                    #region Models
                    /// <summary>Diary Work1</summary>
                    readonly Models.DiaryWork1 DiaryWork1 = new Models.DiaryWork1 { AllowRowLocking = true };
                    /// <summary>SLA_SectSales</summary>
                    readonly Models.SLA_SectSales SLA_SectSales = new Models.SLA_SectSales { ReadOnly = true };
                    /// <summary>Date</summary>
                    readonly Models.Date1 Date1 = new Models.Date1 { ReadOnly = true };
                    #endregion
                    
                    #region Columns
                    /// <summary>v:First Traded Week</summary>
                    readonly NumberColumn vFirstTradedWeek = new NumberColumn("v:First Traded Week", "6");
                    #endregion
                    
                    GetBranchData _parent;
                    
                    public DeptsToPrtWork(GetBranchData parent)
                    {
                        _parent = parent;
                        Title = "Depts To Prt Work";
                        InitializeDataView();
                    }
                    void InitializeDataView()
                    {
                        From = DiaryWork1;
                        
                        Relations.Add(SLA_SectSales, 
                        		SLA_SectSales.TRANSACTION_DATE.IsGreaterOrEqualTo(_parent._parent._parent.vTYyearStartDate).And(
                        		SLA_SectSales.BRANCH_NUMBER.IsEqualTo(DiaryWork1.BranchNumber)), 
                        	SLA_SectSales.SortBySLA_SECTSALES_X3);
                        
                        Relations.Add(Date1, 
                        		Date1.WeekEndingDate.IsGreaterOrEqualTo(SLA_SectSales.TRANSACTION_DATE), 
                        	Date1.SortByREF_Date_X2);
                        
                        
                        Where.Add(DiaryWork1.ProgNo.IsEqualTo("ss252"));
                        
                        OrderBy = DiaryWork1.SortBySS_Diary_Work1_X1;
                        
                        #region Columns
                        
                        Columns.Add(DiaryWork1.ProgNo);
                        Columns.Add(DiaryWork1.BranchNumber);
                        Columns.Add(DiaryWork1.AreaCode);
                        Columns.Add(DiaryWork1.CompStore);
                        // Find store's earliest trading week.  If it is later than the first week of the
                        // relevant Mackays year, the number of weeks involved in the calculation of
                        // '£ per Bay' and 'Area £ Per Bay'  in the YTD Report will be reduced accordingly.
                        // Similar logic may apply to the Period Report.
                        Columns.Add(SLA_SectSales.TRANSACTION_DATE);
                        Columns.Add(SLA_SectSales.BRANCH_NUMBER);
                        Columns.Add(SLA_SectSales.PDM_DEPARTMENT_CODE);
                        Columns.Add(SLA_SectSales.PDM_SECTION_CODE);
                        Columns.Add(SLA_SectSales.RTC_TRANSACTION_TYPE);
                        Columns.Add(SLA_SectSales.RTC_TRANSACTION_SUBTYPE);
                        Columns.Add(Date1.WeekEndingDate);
                        Columns.Add(Date1.CenturyWeek);
                        Columns.Add(vFirstTradedWeek).BindValue(Date1.CenturyWeek);
                        #endregion
                    }
                    /// <summary>Depts To Prt Work(P#62.1.3.1)</summary>
                    internal void Run()
                    {
                        Execute();
                    }
                    protected override void OnLoad()
                    {
                        AllowUserAbort = true;
                    }
                    protected override void OnLeaveRow()
                    {
                        // For TESTING Mod 10    (BLOCK condition to be set as required)
                        
                        Cached<DeptOnMultFloors>().Run();
                        Cached<ReadBranchWork>().Run();
                    }
                    
                    
                    /// <summary>Dept On Mult Floors(P#62.1.3.1.1)</summary>
                    /// <remark>Last change before Migration: 21/06/2011 13:52:20</remark>
                    class DeptOnMultFloors : SalesAndStock.BusinessProcessBase 
                    {
                        
                        #region Models
                        DynamicSQLEntity sqlEntity;
                        #endregion
                        
                        #region Columns
                        /// <summary>AREA_CODE</summary>
                        readonly TextColumn AREA_CODE = new TextColumn("AREA_CODE", "2")
                        {
                        	AllowNull = true
                        };
                        /// <summary>BRANCH_NUMBER</summary>
                        readonly NumberColumn BRANCH_NUMBER = new NumberColumn("BRANCH_NUMBER", "N4")
                        {
                        	AllowNull = true
                        };
                        /// <summary>REPORT_GROUP</summary>
                        readonly TextColumn REPORT_GROUP = new TextColumn("REPORT_GROUP", "1")
                        {
                        	AllowNull = true
                        };
                        /// <summary>FLOOR_CODE</summary>
                        readonly NumberColumn FLOOR_CODE = new NumberColumn("FLOOR_CODE", "N1")
                        {
                        	AllowNull = true
                        };
                        /// <summary>REPORT_DEPARTMENT</summary>
                        readonly TextColumn REPORT_DEPARTMENT = new TextColumn("REPORT_DEPARTMENT", "2")
                        {
                        	AllowNull = true
                        };
                        /// <summary>REPORT_SUBDEPARTMENT</summary>
                        readonly TextColumn REPORT_SUBDEPARTMENT = new TextColumn("REPORT_SUBDEPARTMENT", "2")
                        {
                        	AllowNull = true
                        };
                        /// <summary>TY_WEEK</summary>
                        readonly NumberColumn TY_WEEK = new NumberColumn("TY_WEEK", "N8")
                        {
                        	AllowNull = true
                        };
                        /// <summary>TY_PERIOD</summary>
                        readonly NumberColumn TY_PERIOD = new NumberColumn("TY_PERIOD", "N8")
                        {
                        	AllowNull = true
                        };
                        /// <summary>TY_SEASON</summary>
                        readonly NumberColumn TY_SEASON = new NumberColumn("TY_SEASON", "N8")
                        {
                        	AllowNull = true
                        };
                        /// <summary>TY_YEAR</summary>
                        readonly NumberColumn TY_YEAR = new NumberColumn("TY_YEAR", "N8")
                        {
                        	AllowNull = true
                        };
                        /// <summary>LY_WEEK</summary>
                        readonly NumberColumn LY_WEEK = new NumberColumn("LY_WEEK", "N8")
                        {
                        	AllowNull = true
                        };
                        /// <summary>LY_PERIOD</summary>
                        readonly NumberColumn LY_PERIOD = new NumberColumn("LY_PERIOD", "N8")
                        {
                        	AllowNull = true
                        };
                        /// <summary>LY_SEASON</summary>
                        readonly NumberColumn LY_SEASON = new NumberColumn("LY_SEASON", "N8")
                        {
                        	AllowNull = true
                        };
                        /// <summary>LY_YEAR</summary>
                        readonly NumberColumn LY_YEAR = new NumberColumn("LY_YEAR", "N8")
                        {
                        	AllowNull = true
                        };
                        /// <summary>NUMBER_OF_BAYS</summary>
                        readonly NumberColumn NUMBER_OF_BAYS = new NumberColumn("NUMBER_OF_BAYS", "N4")
                        {
                        	AllowNull = true
                        };
                        /// <summary>v:TY Week</summary>
                        readonly NumberColumn vTYWeek = new NumberColumn("v:TY Week", "N8");
                        /// <summary>v:TY Period</summary>
                        readonly NumberColumn vTYPeriod = new NumberColumn("v:TY Period", "N8");
                        /// <summary>v:TY Season</summary>
                        readonly NumberColumn vTYSeason = new NumberColumn("v:TY Season", "N8");
                        /// <summary>v:TY Year</summary>
                        readonly NumberColumn vTYYear = new NumberColumn("v:TY Year", "N8");
                        /// <summary>v:LY Week</summary>
                        readonly NumberColumn vLYWeek = new NumberColumn("v:LY Week", "N8");
                        /// <summary>v:LY Period</summary>
                        readonly NumberColumn vLYPeriod = new NumberColumn("v:LY Period", "N8");
                        /// <summary>v:LY Season</summary>
                        readonly NumberColumn vLYSeason = new NumberColumn("v:LY Season", "N8");
                        /// <summary>v:LY Year</summary>
                        readonly NumberColumn vLYYear = new NumberColumn("v:LY Year", "N8");
                        /// <summary>v:NumberOfBays</summary>
                        readonly NumberColumn vNumberOfBays = new NumberColumn("v:NumberOfBays", "N4");
                        /// <summary>v:Area</summary>
                        readonly TextColumn vArea = new TextColumn("v:Area", "U2");
                        /// <summary>v:Branch</summary>
                        readonly NumberColumn vBranch = new NumberColumn("v:Branch", "4");
                        /// <summary>v:Report Group</summary>
                        readonly TextColumn vReportGroup = new TextColumn("v:Report Group", "1");
                        /// <summary>v:Floor</summary>
                        readonly NumberColumn vFloor = new NumberColumn("v:Floor", "N1");
                        /// <summary>v:Report Dept</summary>
                        readonly TextColumn vReportDept = new TextColumn("v:Report Dept", "U2");
                        /// <summary>v:Report SubDept</summary>
                        readonly TextColumn vReportSubDept = new TextColumn("v:Report SubDept", "U2");
                        #endregion
                        
                        DeptsToPrtWork _parent;
                        
                        public DeptOnMultFloors(DeptsToPrtWork parent)
                        {
                            _parent = parent;
                            Title = "Dept On Mult Floors";
                            InitializeDataView();
                            var AREA_CODEGroup = Groups.Add(AREA_CODE);
                            var BRANCH_NUMBERGroup = Groups.Add(BRANCH_NUMBER);
                            var REPORT_GROUPGroup = Groups.Add(REPORT_GROUP);
                            var REPORT_DEPARTMENTGroup = Groups.Add(REPORT_DEPARTMENT);
                            var REPORT_SUBDEPARTMENTGroup = Groups.Add(REPORT_SUBDEPARTMENT);
                            REPORT_SUBDEPARTMENTGroup.Enter += REPORT_SUBDEPARTMENTGroup_Enter;
                            REPORT_SUBDEPARTMENTGroup.Leave += REPORT_SUBDEPARTMENTGroup_Leave;
                        }
                        void InitializeDataView()
                        {
                            sqlEntity = new DynamicSQLEntity(Models.DataSources.SS, 
@"--  Called by task 'Dept On Mult Floors' in MAGIC program ss252
--      Where more than 1 record exists for a department within a branch (i.e. the department
--      is split over 2 or more floors) select the data concerned and sort such that the record
--      with the highest bay count will be first.  The ensuing MAGIC coding will then merge data
--      from the other records into the first record.
-- 
select    q1.area_code, q1.branch_number, q1.report_group, q1.floor_code,
          q1.report_department, q1.report_subdepartment,
          q1.ty_week, q1.ty_period, q1.ty_season, q1.ty_year,
          q1.ly_week, q1.ly_period, q1.ly_season, q1.ly_year, q1.number_of_bays
from

   (select   ww.area_code, ww.branch_number, ww.report_group, ww.floor_code,
             ww.report_department, ww.report_subdepartment,
             ww.ty_week, ww.ty_period, ww.ty_season, ww.ty_year,
             ww.ly_week, ww.ly_period, ww.ly_season, ww.ly_year, ww.number_of_bays
    from     mackays.ss252_branch_mk2 ww
    where    ww.area_code = ':1'
      and    ww.branch_number = :2
      and    ww.report_group = 'A')                                      Q1,

   (select   w.area_code, w.branch_number, w.report_group,
             w.report_department, w.report_subdepartment, count(*) recs
    from     mackays.ss252_branch_mk2 w
    where    w.area_code = ':1'
      and    w .branch_number = :2
      and    w.report_group = 'A'
    group by w.area_code, w.branch_number, w.report_group,
             w.report_department, w.report_subdepartment)               Q2
where    q2.recs > 1
  and    q2.area_code = q1.area_code
  and    q2.branch_number = q1.branch_number
  and    q2.report_group = q1.report_group
  and    q2.report_department = q1.report_department
  and    q2.report_subdepartment = q1.report_subdepartment
order by q1.area_code, q1.branch_number, q1.report_group,
         q1.report_department, q1.report_subdepartment,
         q1.number_of_bays DESC, q1.floor_code");
                            sqlEntity.AddParameter(() => u.Trim(_parent.DiaryWork1.AreaCode)); //:1;
                            sqlEntity.AddParameter(_parent.DiaryWork1.BranchNumber); //:2;
                            sqlEntity.Columns.Add(AREA_CODE, BRANCH_NUMBER, REPORT_GROUP, FLOOR_CODE, REPORT_DEPARTMENT, REPORT_SUBDEPARTMENT, TY_WEEK, TY_PERIOD, TY_SEASON, TY_YEAR, LY_WEEK, LY_PERIOD, LY_SEASON, LY_YEAR, NUMBER_OF_BAYS);
                            From = sqlEntity;
                            
                            
                            #region Columns
                            
                            // This task will merge relevant details of a dept. which is split over
                            // more than 1 floor within a branch into 1 record to avoid problems when
                            // comparing Budget figures and area percentages.
                            // The data will be merged into the floor with the biggest Bay Count.
                            // If no floor has the biggest By Count, data will be merged into the lowest
                            // floor in which the dept. is located.
                            // SQL will present the input sorted such that the first record for a
                            // Dept/SubDept is the one into which the data will be merged.
                            Columns.Add(AREA_CODE);
                            Columns.Add(BRANCH_NUMBER);
                            Columns.Add(REPORT_GROUP);
                            Columns.Add(FLOOR_CODE);
                            Columns.Add(REPORT_DEPARTMENT);
                            Columns.Add(REPORT_SUBDEPARTMENT);
                            Columns.Add(TY_WEEK);
                            Columns.Add(TY_PERIOD);
                            Columns.Add(TY_SEASON);
                            Columns.Add(TY_YEAR);
                            Columns.Add(LY_WEEK);
                            Columns.Add(LY_PERIOD);
                            Columns.Add(LY_SEASON);
                            Columns.Add(LY_YEAR);
                            Columns.Add(NUMBER_OF_BAYS);
                            // Accumulators
                            Columns.Add(vTYWeek);
                            Columns.Add(vTYPeriod);
                            Columns.Add(vTYSeason);
                            Columns.Add(vTYYear);
                            Columns.Add(vLYWeek);
                            Columns.Add(vLYPeriod);
                            Columns.Add(vLYSeason);
                            Columns.Add(vLYYear);
                            Columns.Add(vNumberOfBays);
                            // Key fields for final merged record
                            Columns.Add(vArea);
                            Columns.Add(vBranch);
                            Columns.Add(vReportGroup);
                            Columns.Add(vFloor);
                            Columns.Add(vReportDept);
                            Columns.Add(vReportSubDept);
                            #endregion
                        }
                        /// <summary>Dept On Mult Floors(P#62.1.3.1.1)</summary>
                        internal void Run()
                        {
                            Execute();
                        }
                        protected override void OnLoad()
                        {
                            TransactionScope = TransactionScopes.Task;
                            AllowDelete = false;
                            AllowUserAbort = true;
                        }
                        void REPORT_SUBDEPARTMENTGroup_Enter()
                        {
                            // Clear totals
                            vTYWeek.Value = 0;
                            vTYPeriod.Value = 0;
                            vTYSeason.Value = 0;
                            vTYYear.Value = 0;
                            vLYWeek.Value = 0;
                            vLYPeriod.Value = 0;
                            vLYSeason.Value = 0;
                            vLYYear.Value = 0;
                            vNumberOfBays.Value = 0;
                            // Store key columns for final merge
                            vArea.Value = AREA_CODE;
                            vBranch.Value = BRANCH_NUMBER;
                            vReportGroup.Value = REPORT_GROUP;
                            vFloor.Value = FLOOR_CODE;
                            vReportDept.Value = REPORT_DEPARTMENT;
                            vReportSubDept.Value = REPORT_SUBDEPARTMENT;
                        }
                        protected override void OnLeaveRow()
                        {
                            vTYWeek.Value += TY_WEEK;
                            vTYPeriod.Value += TY_PERIOD;
                            vTYSeason.Value += TY_SEASON;
                            vTYYear.Value += TY_YEAR;
                            vLYWeek.Value += LY_WEEK;
                            vLYPeriod.Value += LY_PERIOD;
                            vLYSeason.Value += LY_SEASON;
                            vLYYear.Value += LY_YEAR;
                            vNumberOfBays.Value += NUMBER_OF_BAYS;
                            // Delete record if not first for the department
                            if(u.Not(u.And(u.And(u.And(u.And(u.And(u.Equals(AREA_CODE, vArea), u.Equals(BRANCH_NUMBER, vBranch)), u.Equals(REPORT_GROUP, vReportGroup)), u.Equals(FLOOR_CODE, vFloor)), u.Equals(REPORT_DEPARTMENT, vReportDept)), u.Equals(REPORT_SUBDEPARTMENT, vReportSubDept))))
                            {
                                Cached<DeleteFloor>().Run();
                            }
                        }
                        void REPORT_SUBDEPARTMENTGroup_Leave()
                        {
                            Cached<MergeFloors>().Run();
                        }
                        
                        
                        /// <summary>Delete Floor(P#62.1.3.1.1.1)</summary>
                        /// <remark>Last change before Migration: 26/10/2006 09:20:22</remark>
                        class DeleteFloor : SalesAndStock.BusinessProcessBase 
                        {
                            
                            #region Models
                            /// <summary>ss252 Branch Work Mk2</summary>
                            readonly Models.ss252BranchWorkMk2 ss252BranchWorkMk2 = new Models.ss252BranchWorkMk2 { AllowRowLocking = true };
                            #endregion
                            
                            DeptOnMultFloors _parent;
                            
                            public DeleteFloor(DeptOnMultFloors parent)
                            {
                                _parent = parent;
                                Title = "Delete Floor";
                                InitializeDataView();
                            }
                            void InitializeDataView()
                            {
                                From = ss252BranchWorkMk2;
                                
                                #region Where
                                
                                Where.Add(ss252BranchWorkMk2.AreaCode.IsEqualTo(_parent.AREA_CODE));
                                Where.Add(ss252BranchWorkMk2.BranchNumber.IsEqualTo(_parent.BRANCH_NUMBER));
                                Where.Add(ss252BranchWorkMk2.ReportGroup.IsEqualTo(_parent.REPORT_GROUP));
                                Where.Add(ss252BranchWorkMk2.FloorCode.IsEqualTo(_parent.FLOOR_CODE));
                                Where.Add(ss252BranchWorkMk2.ReportDepartment.IsEqualTo(_parent.REPORT_DEPARTMENT));
                                Where.Add(ss252BranchWorkMk2.ReportSubDepartment.IsEqualTo(_parent.REPORT_SUBDEPARTMENT));
                                #endregion
                                
                                OrderBy = ss252BranchWorkMk2.SortByss252_Branch_Mk2_X1;
                                
                                #region Columns
                                
                                // Delete records for floors which are being merged into one floor.
                                Columns.Add(ss252BranchWorkMk2.AreaCode);
                                Columns.Add(ss252BranchWorkMk2.BranchNumber);
                                Columns.Add(ss252BranchWorkMk2.ReportGroup);
                                Columns.Add(ss252BranchWorkMk2.FloorCode);
                                Columns.Add(ss252BranchWorkMk2.ReportDepartment);
                                Columns.Add(ss252BranchWorkMk2.ReportSubDepartment);
                                #endregion
                            }
                            /// <summary>Delete Floor(P#62.1.3.1.1.1)</summary>
                            internal void Run()
                            {
                                Execute();
                            }
                            protected override void OnLoad()
                            {
                                Exit(ExitTiming.AfterRow);
                                TransactionScope = TransactionScopes.RowLocking;
                                KeepChildRelationCacheAlive = true;
                                Activity = Activities.Delete;
                                AllowUserAbort = true;
                            }
                            
                            
                        }
                        /// <summary>Merge Floors(P#62.1.3.1.1.2)</summary>
                        /// <remark>Last change before Migration: 26/10/2006 09:35:53</remark>
                        class MergeFloors : SalesAndStock.BusinessProcessBase 
                        {
                            
                            #region Models
                            /// <summary>ss252 Branch Work Mk2</summary>
                            readonly Models.ss252BranchWorkMk2 ss252BranchWorkMk2 = new Models.ss252BranchWorkMk2 { AllowRowLocking = true };
                            #endregion
                            
                            DeptOnMultFloors _parent;
                            
                            public MergeFloors(DeptOnMultFloors parent)
                            {
                                _parent = parent;
                                Title = "Merge Floors";
                                InitializeDataView();
                            }
                            void InitializeDataView()
                            {
                                From = ss252BranchWorkMk2;
                                
                                #region Where
                                
                                Where.Add(ss252BranchWorkMk2.AreaCode.IsEqualTo(_parent.vArea));
                                Where.Add(ss252BranchWorkMk2.BranchNumber.IsEqualTo(_parent.vBranch));
                                Where.Add(ss252BranchWorkMk2.ReportGroup.IsEqualTo(_parent.vReportGroup));
                                Where.Add(ss252BranchWorkMk2.FloorCode.IsEqualTo(_parent.vFloor));
                                Where.Add(ss252BranchWorkMk2.ReportDepartment.IsEqualTo(_parent.vReportDept));
                                Where.Add(ss252BranchWorkMk2.ReportSubDepartment.IsEqualTo(_parent.vReportSubDept));
                                #endregion
                                
                                OrderBy = ss252BranchWorkMk2.SortByss252_Branch_Mk2_X1;
                                
                                #region Columns
                                
                                // Replace totals on remaining record with those accumulated
                                Columns.Add(ss252BranchWorkMk2.AreaCode);
                                Columns.Add(ss252BranchWorkMk2.BranchNumber);
                                Columns.Add(ss252BranchWorkMk2.ReportGroup);
                                Columns.Add(ss252BranchWorkMk2.FloorCode);
                                Columns.Add(ss252BranchWorkMk2.ReportDepartment);
                                Columns.Add(ss252BranchWorkMk2.ReportSubDepartment);
                                Columns.Add(ss252BranchWorkMk2.TYWeek);
                                Columns.Add(ss252BranchWorkMk2.TYPeriod);
                                Columns.Add(ss252BranchWorkMk2.TYSeason);
                                Columns.Add(ss252BranchWorkMk2.TYYear);
                                Columns.Add(ss252BranchWorkMk2.LYWeek);
                                Columns.Add(ss252BranchWorkMk2.LYPeriod);
                                Columns.Add(ss252BranchWorkMk2.LYSeason);
                                Columns.Add(ss252BranchWorkMk2.LYYear);
                                Columns.Add(ss252BranchWorkMk2.NumberOfBays);
                                #endregion
                            }
                            /// <summary>Merge Floors(P#62.1.3.1.1.2)</summary>
                            internal void Run()
                            {
                                Execute();
                            }
                            protected override void OnLoad()
                            {
                                Exit(ExitTiming.AfterRow);
                                RowLocking = LockingStrategy.OnRowLoading;
                                TransactionScope = TransactionScopes.Task;
                                KeepChildRelationCacheAlive = true;
                                AllowUserAbort = true;
                            }
                            protected override void OnLeaveRow()
                            {
                                // REPLACE values as totals include the existing values from this record
                                ss252BranchWorkMk2.TYWeek.Value = _parent.vTYWeek;
                                ss252BranchWorkMk2.TYPeriod.Value = _parent.vTYPeriod;
                                ss252BranchWorkMk2.TYSeason.Value = _parent.vTYSeason;
                                ss252BranchWorkMk2.TYYear.Value = _parent.vTYYear;
                                ss252BranchWorkMk2.LYWeek.Value = _parent.vLYWeek;
                                ss252BranchWorkMk2.LYPeriod.Value = _parent.vLYPeriod;
                                ss252BranchWorkMk2.LYSeason.Value = _parent.vLYSeason;
                                ss252BranchWorkMk2.LYYear.Value = _parent.vLYYear;
                                ss252BranchWorkMk2.NumberOfBays.Value = _parent.vNumberOfBays;
                            }
                            
                            
                        }
                    }
                    /// <summary>Read Branch Work(P#62.1.3.1.2)</summary>
                    /// <remark>Last change before Migration: 08/11/2006 10:57:56</remark>
                    internal class ReadBranchWork : SalesAndStock.BusinessProcessBase 
                    {
                        
                        #region Models
                        /// <summary>ss252 Branch Work Mk2</summary>
                        readonly Models.ss252BranchWorkMk2 ss252BranchWorkMk2 = new Models.ss252BranchWorkMk2 { AllowRowLocking = true };
                        /// <summary>ss252 Area Work Mk2</summary>
                        readonly Models.ss252AreaWorkMk2 ss252AreaWorkMk2 = new Models.ss252AreaWorkMk2 { AllowRowLocking = true };
                        #endregion
                        
                        #region Columns
                        /// <summary>v:Floor Store TY Week</summary>
                        internal readonly NumberColumn vFloorStoreTYWeek = new NumberColumn("v:Floor Store TY Week", "N9");
                        /// <summary>v:Floor Store TY Period</summary>
                        internal readonly NumberColumn vFloorStoreTYPeriod = new NumberColumn("v:Floor Store TY Period", "N9");
                        /// <summary>v:Floor Store TY Season</summary>
                        readonly NumberColumn vFloorStoreTYSeason = new NumberColumn("v:Floor Store TY Season", "N9");
                        /// <summary>v:Floor Store TY Year</summary>
                        readonly NumberColumn vFloorStoreTYYear = new NumberColumn("v:Floor Store TY Year", "N9");
                        /// <summary>v:Floor Store Bud Week</summary>
                        internal readonly NumberColumn vFloorStoreBudWeek = new NumberColumn("v:Floor Store Bud Week", "N9");
                        /// <summary>v:Floor Store Bud Period</summary>
                        internal readonly NumberColumn vFloorStoreBudPeriod = new NumberColumn("v:Floor Store Bud Period", "N9");
                        /// <summary>v:Floor Store Bud Season</summary>
                        readonly NumberColumn vFloorStoreBudSeason = new NumberColumn("v:Floor Store Bud Season", "N9");
                        /// <summary>v:Floor Store Bud Year</summary>
                        internal readonly NumberColumn vFloorStoreBudYear = new NumberColumn("v:Floor Store Bud Year", "N9");
                        /// <summary>v:Floor Store LY Week</summary>
                        internal readonly NumberColumn vFloorStoreLYWeek = new NumberColumn("v:Floor Store LY Week", "N9");
                        /// <summary>v:Floor Store LY Period</summary>
                        internal readonly NumberColumn vFloorStoreLYPeriod = new NumberColumn("v:Floor Store LY Period", "N9");
                        /// <summary>v:Floor Store LY Season</summary>
                        readonly NumberColumn vFloorStoreLYSeason = new NumberColumn("v:Floor Store LY Season", "N9");
                        /// <summary>v:Floor Store LY Year</summary>
                        internal readonly NumberColumn vFloorStoreLYYear = new NumberColumn("v:Floor Store LY Year", "N9");
                        /// <summary>v:Floor Store Bays</summary>
                        internal readonly NumberColumn vFloorStoreBays = new NumberColumn("v:Floor Store Bays", "4");
                        /// <summary>v:Floor Area TY Week</summary>
                        readonly NumberColumn vFloorAreaTYWeek = new NumberColumn("v:Floor Area TY Week", "N9");
                        /// <summary>v:Floor Area TY Period</summary>
                        readonly NumberColumn vFloorAreaTYPeriod = new NumberColumn("v:Floor Area TY Period", "N9");
                        /// <summary>v:Floor Area TY Season</summary>
                        readonly NumberColumn vFloorAreaTYSeason = new NumberColumn("v:Floor Area TY Season", "N9");
                        /// <summary>v:Floor Area TY Year</summary>
                        readonly NumberColumn vFloorAreaTYYear = new NumberColumn("v:Floor Area TY Year", "N9");
                        /// <summary>v:Floor Area LY Week</summary>
                        readonly NumberColumn vFloorAreaLYWeek = new NumberColumn("v:Floor Area LY Week", "N9");
                        /// <summary>v:Floor Area LY Period</summary>
                        readonly NumberColumn vFloorAreaLYPeriod = new NumberColumn("v:Floor Area LY Period", "N9");
                        /// <summary>v:Floor Area LY Season</summary>
                        readonly NumberColumn vFloorAreaLYSeason = new NumberColumn("v:Floor Area LY Season", "N9");
                        /// <summary>v:Floor Area LY Year</summary>
                        readonly NumberColumn vFloorAreaLYYear = new NumberColumn("v:Floor Area LY Year", "N9");
                        /// <summary>v:Floor Area Bays</summary>
                        readonly NumberColumn vFloorAreaBays = new NumberColumn("v:Floor Area Bays", "4");
                        /// <summary>v:Total Store TY Week</summary>
                        internal readonly NumberColumn vTotalStoreTYWeek = new NumberColumn("v:Total Store TY Week", "N9");
                        /// <summary>v:Total Store TY Period</summary>
                        internal readonly NumberColumn vTotalStoreTYPeriod = new NumberColumn("v:Total Store TY Period", "N9");
                        /// <summary>v:Total Store TY Season</summary>
                        readonly NumberColumn vTotalStoreTYSeason = new NumberColumn("v:Total Store TY Season", "N9");
                        /// <summary>v:Total Store TY Year</summary>
                        internal readonly NumberColumn vTotalStoreTYYear = new NumberColumn("v:Total Store TY Year", "N9");
                        /// <summary>v:Total Store Bud Week</summary>
                        internal readonly NumberColumn vTotalStoreBudWeek = new NumberColumn("v:Total Store Bud Week", "N9");
                        /// <summary>v:Total Store Bud Period</summary>
                        internal readonly NumberColumn vTotalStoreBudPeriod = new NumberColumn("v:Total Store Bud Period", "N9");
                        /// <summary>v:Total Store Bud Season</summary>
                        readonly NumberColumn vTotalStoreBudSeason = new NumberColumn("v:Total Store Bud Season", "N9");
                        /// <summary>v:Total Store Bud Year</summary>
                        internal readonly NumberColumn vTotalStoreBudYear = new NumberColumn("v:Total Store Bud Year", "N9");
                        /// <summary>v:Total Store LY Week</summary>
                        internal readonly NumberColumn vTotalStoreLYWeek = new NumberColumn("v:Total Store LY Week", "N9");
                        /// <summary>v:Total Store LY Period</summary>
                        internal readonly NumberColumn vTotalStoreLYPeriod = new NumberColumn("v:Total Store LY Period", "N9");
                        /// <summary>v:Total Store LY Season</summary>
                        readonly NumberColumn vTotalStoreLYSeason = new NumberColumn("v:Total Store LY Season", "N9");
                        /// <summary>v:Total Store LY Year</summary>
                        internal readonly NumberColumn vTotalStoreLYYear = new NumberColumn("v:Total Store LY Year", "N9");
                        /// <summary>v:Total Store Bays</summary>
                        internal readonly NumberColumn vTotalStoreBays = new NumberColumn("v:Total Store Bays", "4");
                        /// <summary>v:Total Area TY Week</summary>
                        readonly NumberColumn vTotalAreaTYWeek = new NumberColumn("v:Total Area TY Week", "N9");
                        /// <summary>v:Total Area TY Period</summary>
                        readonly NumberColumn vTotalAreaTYPeriod = new NumberColumn("v:Total Area TY Period", "N9");
                        /// <summary>v:Total Area TY Season</summary>
                        readonly NumberColumn vTotalAreaTYSeason = new NumberColumn("v:Total Area TY Season", "N9");
                        /// <summary>v:Total Area TY Year</summary>
                        readonly NumberColumn vTotalAreaTYYear = new NumberColumn("v:Total Area TY Year", "N9");
                        /// <summary>v:Total Area LY Week</summary>
                        readonly NumberColumn vTotalAreaLYWeek = new NumberColumn("v:Total Area LY Week", "N9");
                        /// <summary>v:Total Area LY Period</summary>
                        readonly NumberColumn vTotalAreaLYPeriod = new NumberColumn("v:Total Area LY Period", "N9");
                        /// <summary>v:Total Area LY Season</summary>
                        readonly NumberColumn vTotalAreaLYSeason = new NumberColumn("v:Total Area LY Season", "N9");
                        /// <summary>v:Total Area LY Year</summary>
                        readonly NumberColumn vTotalAreaLYYear = new NumberColumn("v:Total Area LY Year", "N9");
                        /// <summary>v:Total Area Bays</summary>
                        readonly NumberColumn vTotalAreaBays = new NumberColumn("v:Total Area Bays", "4");
                        /// <summary>v:Tmp RecTyp</summary>
                        readonly TextColumn vTmpRecTyp = new TextColumn("v:Tmp RecTyp", "U");
                        /// <summary>v:Tmp FloorCode</summary>
                        readonly NumberColumn vTmpFloorCode = new NumberColumn("v:Tmp FloorCode", "N1A");
                        /// <summary>v:TmpRptDept</summary>
                        readonly TextColumn vTmpRptDept = new TextColumn("v:TmpRptDept", "U2");
                        /// <summary>v:TmpRptSubD</summary>
                        readonly TextColumn vTmpRptSubD = new TextColumn("v:TmpRptSubD", "U2");
                        /// <summary>v:Tmp Desc</summary>
                        readonly TextColumn vTmpDesc = new TextColumn("v:Tmp Desc", "18");
                        /// <summary>v:Tmp Store TY</summary>
                        readonly NumberColumn vTmpStoreTY = new NumberColumn("v:Tmp Store TY", "N7");
                        /// <summary>v:Tmp Store Bud</summary>
                        readonly NumberColumn vTmpStoreBud = new NumberColumn("v:Tmp Store Bud", "N7");
                        /// <summary>v:Tmp Store LY</summary>
                        readonly NumberColumn vTmpStoreLY = new NumberColumn("v:Tmp Store LY", "N7");
                        /// <summary>v:Tmp Store Bays</summary>
                        readonly NumberColumn vTmpStoreBays = new NumberColumn("v:Tmp Store Bays", "N7");
                        /// <summary>v:Tmp Area TY</summary>
                        readonly NumberColumn vTmpAreaTY = new NumberColumn("v:Tmp Area TY", "N7");
                        /// <summary>v:Tmp Area LY</summary>
                        readonly NumberColumn vTmpAreaLY = new NumberColumn("v:Tmp Area LY", "N7");
                        /// <summary>v:Tmp Area Bays</summary>
                        readonly NumberColumn vTmpAreaBays = new NumberColumn("v:Tmp Area Bays", "N7");
                        /// <summary>v:Tmp Floor Sales</summary>
                        readonly NumberColumn vTmpFloorSales = new NumberColumn("v:Tmp Floor Sales", "N7");
                        /// <summary>v:Tmp Floor Bays</summary>
                        readonly NumberColumn vTmpFloorBays = new NumberColumn("v:Tmp Floor Bays", "N7");
                        /// <summary>v:Level</summary>
                        readonly TextColumn vLevel = new TextColumn("v:Level", "U");
                        /// <summary>v:Skip</summary>
                        readonly NumberColumn vSkip = new NumberColumn("v:Skip", "1");
                        /// <summary>v:Brn Wks Involved</summary>
                        readonly NumberColumn vBrnWksInvolved = new NumberColumn("v:Brn Wks Involved", "2P0");
                        /// <summary>v:Area Wks Involved</summary>
                        readonly NumberColumn vAreaWksInvolved = new NumberColumn("v:Area Wks Involved", "2P0");
                        #endregion
                        
                        DeptsToPrtWork _parent;
                        
                        public ReadBranchWork(DeptsToPrtWork parent)
                        {
                            _parent = parent;
                            Title = "Read Branch Work";
                            InitializeDataView();
                            var ss252BranchWorkMk2AreaCodeGroup = Groups.Add(ss252BranchWorkMk2.AreaCode);
                            var ss252BranchWorkMk2BranchNumberGroup = Groups.Add(ss252BranchWorkMk2.BranchNumber);
                            var ss252BranchWorkMk2ReportGroupGroup = Groups.Add(ss252BranchWorkMk2.ReportGroup);
                            var ss252BranchWorkMk2FloorCodeGroup = Groups.Add(ss252BranchWorkMk2.FloorCode);
                            ss252BranchWorkMk2FloorCodeGroup.Enter += ss252BranchWorkMk2FloorCodeGroup_Enter;
                            ss252BranchWorkMk2FloorCodeGroup.Leave += ss252BranchWorkMk2FloorCodeGroup_Leave;
                        }
                        void InitializeDataView()
                        {
                            From = ss252BranchWorkMk2;
                            
                            Relations.Add(ss252AreaWorkMk2, 
                            		ss252AreaWorkMk2.AreaCode.IsEqualTo(ss252BranchWorkMk2.AreaCode).And(
                            		ss252AreaWorkMk2.ReportGroup.IsEqualTo(ss252BranchWorkMk2.ReportGroup)).And(
                            		ss252AreaWorkMk2.ReportDepartment.IsEqualTo(ss252BranchWorkMk2.ReportDepartment)).And(
                            		ss252AreaWorkMk2.ReportSubDepartment.IsEqualTo(ss252BranchWorkMk2.ReportSubDepartment)), 
                            	ss252AreaWorkMk2.SortByss252_Area_Mk2_X1);
                            
                            
                            Where.Add(ss252BranchWorkMk2.AreaCode.IsEqualTo(_parent.DiaryWork1.AreaCode));
                            Where.Add(ss252BranchWorkMk2.BranchNumber.IsEqualTo(_parent.DiaryWork1.BranchNumber));
                            Where.Add(ss252BranchWorkMk2.ReportGroup.IsEqualTo("A"));
                            
                            OrderBy = ss252BranchWorkMk2.SortByss252_Branch_Mk2_X1;
                            
                            #region Columns
                            
                            Columns.Add(ss252BranchWorkMk2.AreaCode);
                            Columns.Add(ss252BranchWorkMk2.BranchNumber);
                            Columns.Add(ss252BranchWorkMk2.ReportGroup);
                            Columns.Add(ss252BranchWorkMk2.FloorCode);
                            Columns.Add(ss252BranchWorkMk2.ReportDepartment);
                            Columns.Add(ss252BranchWorkMk2.ReportSubDepartment);
                            Columns.Add(ss252BranchWorkMk2.TYWeek);
                            Columns.Add(ss252BranchWorkMk2.TYPeriod);
                            Columns.Add(ss252BranchWorkMk2.TYSeason);
                            Columns.Add(ss252BranchWorkMk2.TYYear);
                            Columns.Add(ss252BranchWorkMk2.BudgetWeek);
                            Columns.Add(ss252BranchWorkMk2.BudgetPeriod);
                            Columns.Add(ss252BranchWorkMk2.BudgetSeason);
                            Columns.Add(ss252BranchWorkMk2.BudgetYear);
                            Columns.Add(ss252BranchWorkMk2.LYWeek);
                            Columns.Add(ss252BranchWorkMk2.LYPeriod);
                            Columns.Add(ss252BranchWorkMk2.LYSeason);
                            Columns.Add(ss252BranchWorkMk2.LYYear);
                            Columns.Add(ss252BranchWorkMk2.NumberOfBays);
                            Columns.Add(ss252BranchWorkMk2.CompStore);
                            // Get corresponding area totals
                            Columns.Add(ss252AreaWorkMk2.AreaCode);
                            Columns.Add(ss252AreaWorkMk2.ReportGroup);
                            Columns.Add(ss252AreaWorkMk2.ReportDepartment);
                            Columns.Add(ss252AreaWorkMk2.ReportSubDepartment);
                            Columns.Add(ss252AreaWorkMk2.TYWeek);
                            Columns.Add(ss252AreaWorkMk2.TYPeriod);
                            Columns.Add(ss252AreaWorkMk2.TYSeason);
                            Columns.Add(ss252AreaWorkMk2.TYYear);
                            Columns.Add(ss252AreaWorkMk2.LYWeek);
                            Columns.Add(ss252AreaWorkMk2.LYPeriod);
                            Columns.Add(ss252AreaWorkMk2.LYSeason);
                            Columns.Add(ss252AreaWorkMk2.LYYear);
                            Columns.Add(ss252AreaWorkMk2.NumberOfBays);
                            
                            // Floor totals for store
                            Columns.Add(vFloorStoreTYWeek);
                            Columns.Add(vFloorStoreTYPeriod);
                            Columns.Add(vFloorStoreTYSeason);
                            Columns.Add(vFloorStoreTYYear);
                            Columns.Add(vFloorStoreBudWeek);
                            Columns.Add(vFloorStoreBudPeriod);
                            Columns.Add(vFloorStoreBudSeason);
                            Columns.Add(vFloorStoreBudYear);
                            Columns.Add(vFloorStoreLYWeek);
                            Columns.Add(vFloorStoreLYPeriod);
                            Columns.Add(vFloorStoreLYSeason);
                            Columns.Add(vFloorStoreLYYear);
                            Columns.Add(vFloorStoreBays);
                            // Floor totals for area
                            Columns.Add(vFloorAreaTYWeek);
                            Columns.Add(vFloorAreaTYPeriod);
                            Columns.Add(vFloorAreaTYSeason);
                            Columns.Add(vFloorAreaTYYear);
                            Columns.Add(vFloorAreaLYWeek);
                            Columns.Add(vFloorAreaLYPeriod);
                            Columns.Add(vFloorAreaLYSeason);
                            Columns.Add(vFloorAreaLYYear);
                            Columns.Add(vFloorAreaBays);
                            // Overall totals for store
                            Columns.Add(vTotalStoreTYWeek);
                            Columns.Add(vTotalStoreTYPeriod);
                            Columns.Add(vTotalStoreTYSeason);
                            Columns.Add(vTotalStoreTYYear);
                            Columns.Add(vTotalStoreBudWeek);
                            Columns.Add(vTotalStoreBudPeriod);
                            Columns.Add(vTotalStoreBudSeason);
                            Columns.Add(vTotalStoreBudYear);
                            Columns.Add(vTotalStoreLYWeek);
                            Columns.Add(vTotalStoreLYPeriod);
                            Columns.Add(vTotalStoreLYSeason);
                            Columns.Add(vTotalStoreLYYear);
                            Columns.Add(vTotalStoreBays);
                            // Overall totals for area
                            Columns.Add(vTotalAreaTYWeek);
                            Columns.Add(vTotalAreaTYPeriod);
                            Columns.Add(vTotalAreaTYSeason);
                            Columns.Add(vTotalAreaTYYear);
                            Columns.Add(vTotalAreaLYWeek);
                            Columns.Add(vTotalAreaLYPeriod);
                            Columns.Add(vTotalAreaLYSeason);
                            Columns.Add(vTotalAreaLYYear);
                            Columns.Add(vTotalAreaBays);
                            // Temporary fields for building Print Work record
                            Columns.Add(vTmpRecTyp);
                            Columns.Add(vTmpFloorCode);
                            Columns.Add(vTmpRptDept);
                            Columns.Add(vTmpRptSubD);
                            Columns.Add(vTmpDesc);
                            Columns.Add(vTmpStoreTY);
                            Columns.Add(vTmpStoreBud);
                            Columns.Add(vTmpStoreLY);
                            Columns.Add(vTmpStoreBays);
                            Columns.Add(vTmpAreaTY);
                            Columns.Add(vTmpAreaLY);
                            Columns.Add(vTmpAreaBays);
                            Columns.Add(vTmpFloorSales);
                            Columns.Add(vTmpFloorBays);
                            
                            Columns.Add(vLevel);
                            Columns.Add(vSkip);
                            Columns.Add(vBrnWksInvolved);
                            Columns.Add(vAreaWksInvolved);
                            #endregion
                        }
                        /// <summary>Read Branch Work(P#62.1.3.1.2)</summary>
                        internal void Run()
                        {
                            Execute();
                        }
                        protected override void OnLoad()
                        {
                            AllowUserAbort = true;
                        }
                        void ss252BranchWorkMk2FloorCodeGroup_Enter()
                        {
                            vFloorStoreTYWeek.Value = 0;
                            vFloorStoreTYPeriod.Value = 0;
                            vFloorStoreTYSeason.Value = 0;
                            vFloorStoreTYYear.Value = 0;
                            vFloorStoreBudWeek.Value = 0;
                            vFloorStoreBudPeriod.Value = 0;
                            vFloorStoreBudSeason.Value = 0;
                            vFloorStoreBudYear.Value = 0;
                            vFloorStoreLYWeek.Value = 0;
                            vFloorStoreLYPeriod.Value = 0;
                            vFloorStoreLYSeason.Value = 0;
                            vFloorStoreLYYear.Value = 0;
                            vFloorStoreBays.Value = 0;
                            vFloorAreaTYWeek.Value = 0;
                            vFloorAreaTYPeriod.Value = 0;
                            vFloorAreaTYSeason.Value = 0;
                            vFloorAreaTYYear.Value = 0;
                            vFloorAreaLYWeek.Value = 0;
                            vFloorAreaLYPeriod.Value = 0;
                            vFloorAreaLYSeason.Value = 0;
                            vFloorAreaLYYear.Value = 0;
                            vFloorAreaBays.Value = 0;
                            // For each department (other than those on Floor 9)
                            // calculate store floor and overall totals
                            if(u.Trim(ss252BranchWorkMk2.ReportGroup) == "A" && ss252BranchWorkMk2.FloorCode != 9)
                            {
                                Cached<FloorTotals>().Run();
                            }
                        }
                        protected override void OnLeaveRow()
                        {
                            vLevel.Value = "D";
                            
                            // STORE FLOOR totals already accumulated via a subordinate task.  Now accumulate
                            // corresponding AREA totals.
                            // (Floor Code = 9 denotes a dept for which branch has no sales)
                            if(ss252BranchWorkMk2.FloorCode != 9)
                            {
                                vFloorAreaTYWeek.Value += ss252AreaWorkMk2.TYWeek;
                                vFloorAreaTYPeriod.Value += ss252AreaWorkMk2.TYPeriod;
                                vFloorAreaTYSeason.Value += ss252AreaWorkMk2.TYSeason;
                                vFloorAreaTYYear.Value += ss252AreaWorkMk2.TYYear;
                                vFloorAreaLYWeek.Value += ss252AreaWorkMk2.LYWeek;
                                vFloorAreaLYPeriod.Value += ss252AreaWorkMk2.LYPeriod;
                                vFloorAreaLYSeason.Value += ss252AreaWorkMk2.LYSeason;
                                vFloorAreaLYYear.Value += ss252AreaWorkMk2.LYYear;
                                vFloorAreaBays.Value += ss252AreaWorkMk2.NumberOfBays;
                                vTotalAreaTYWeek.Value += ss252AreaWorkMk2.TYWeek;
                                vTotalAreaTYPeriod.Value += ss252AreaWorkMk2.TYPeriod;
                                vTotalAreaTYSeason.Value += ss252AreaWorkMk2.TYSeason;
                                vTotalAreaTYYear.Value += ss252AreaWorkMk2.TYYear;
                                vTotalAreaLYWeek.Value += ss252AreaWorkMk2.LYWeek;
                                vTotalAreaLYPeriod.Value += ss252AreaWorkMk2.LYPeriod;
                                vTotalAreaLYSeason.Value += ss252AreaWorkMk2.LYSeason;
                                vTotalAreaLYYear.Value += ss252AreaWorkMk2.LYYear;
                                vTotalAreaBays.Value += ss252AreaWorkMk2.NumberOfBays;
                            }
                            // Generate WEEK print work at Dept level
                            vSkip.Value = 0;
                            Cached<InitTempFields>().Run();
                            Cached<WritePrintWork>().Run();
                            // Generate PERIOD print work  at Dept level
                            vSkip.Value = 1;
                            Cached<InitTempFields>().Run();
                            Cached<WritePrintWork>().Run();
                            // Generate SEASON print work  at Dept level
                            vSkip.Value = 2;
                            Cached<InitTempFields>().Run();
                            Cached<WritePrintWork>().Run();
                            // Generate YEAR print work at Dept level
                            vSkip.Value = 3;
                            Cached<InitTempFields>().Run();
                            Cached<WritePrintWork>().Run();
                        }
                        void ss252BranchWorkMk2FloorCodeGroup_Leave()
                        {
                            vLevel.Value = "F";
                            
                            if(ss252BranchWorkMk2.FloorCode != 9)
                            {
                                // Generate WEEK print work at Floor level
                                vSkip.Value = 0;
                                Cached<InitTempFields>().Run();
                                Cached<WritePrintWork>().Run();
                                // Generate PERIOD print work at Floor level
                                vSkip.Value = 1;
                                Cached<InitTempFields>().Run();
                                Cached<WritePrintWork>().Run();
                                // Generate SEASON print work at Floor level
                                vSkip.Value = 2;
                                Cached<InitTempFields>().Run();
                                Cached<WritePrintWork>().Run();
                                // Generate YEAR print work at Floor level
                                vSkip.Value = 3;
                                Cached<InitTempFields>().Run();
                                Cached<WritePrintWork>().Run();
                            }
                        }
                        protected override void OnEnd()
                        {
                            vLevel.Value = "S";
                            // Generate WEEK print work for store totals
                            vSkip.Value = 0;
                            Cached<InitTempFields>().Run();
                            Cached<WritePrintWork>().Run();
                            // Generate PERIOD print work for store totals
                            vSkip.Value = 1;
                            Cached<InitTempFields>().Run();
                            Cached<WritePrintWork>().Run();
                            // Generate SEASON print work for store totals
                            vSkip.Value = 2;
                            Cached<InitTempFields>().Run();
                            Cached<WritePrintWork>().Run();
                            // Generate YEAR print work for store totals
                            vSkip.Value = 3;
                            Cached<InitTempFields>().Run();
                            Cached<WritePrintWork>().Run();
                        }
                        
                        
                        /// <summary>Init Temp Fields(P#62.1.3.1.2.1)</summary>
                        /// <remark>Last change before Migration: 08/11/2006 10:45:21</remark>
                        class InitTempFields : SalesAndStock.BusinessProcessBase 
                        {
                            
                            #region Models
                            /// <summary>Analysis Group 1 Depts</summary>
                            readonly Models.AnalysisGroup1Depts AnalysisGroup1Depts = new Models.AnalysisGroup1Depts { KeepCacheAliveAfterExit = true, ReadOnly = true };
                            /// <summary>Branch Floor Codes</summary>
                            readonly Models.BranchFloorCodes BranchFloorCodes = new Models.BranchFloorCodes { KeepCacheAliveAfterExit = true, ReadOnly = true };
                            #endregion
                            
                            #region Columns
                            /// <summary>v:Anal Grp Found</summary>
                            readonly BoolColumn vAnalGrpFound = new BoolColumn("v:Anal Grp Found");
                            /// <summary>v:Floor Code Found</summary>
                            readonly BoolColumn vFloorCodeFound = new BoolColumn("v:Floor Code Found");
                            #endregion
                            
                            ReadBranchWork _parent;
                            
                            public InitTempFields(ReadBranchWork parent)
                            {
                                _parent = parent;
                                Title = "Init Temp Fields";
                                InitializeDataView();
                            }
                            void InitializeDataView()
                            {
                                
                                Relations.Add(AnalysisGroup1Depts, 
                                		AnalysisGroup1Depts.GROUP1_DEPT.IsEqualTo(_parent.ss252BranchWorkMk2.ReportDepartment).And(
                                		AnalysisGroup1Depts.GROUP1_SUBDEPT.IsEqualTo(_parent.ss252BranchWorkMk2.ReportSubDepartment)), 
                                	AnalysisGroup1Depts.SortByREF_ANAL_GRP1_DPTS_X1);
                                
                                Relations.Add(BranchFloorCodes, 
                                		BranchFloorCodes.FLOOR_CODE.IsEqualTo(_parent.ss252BranchWorkMk2.FloorCode), 
                                	BranchFloorCodes.SortByREF_BR_FLOOR_CODES_X1);
                                
                                
                                
                                #region Columns
                                
                                // Depending on from which level in the upper task this task is called
                                // communal work areas are initialised by this task for Department, Floor
                                // and Store totals.
                                // These work areas will be used in initialising Print Work records (see
                                // task 'Write Print Work')
                                
                                // Get department description
                                Columns.Add(vAnalGrpFound);
                                Relations[AnalysisGroup1Depts].NotifyRowWasFoundTo(vAnalGrpFound);
                                Columns.Add(AnalysisGroup1Depts.GROUP1_DEPT);
                                Columns.Add(AnalysisGroup1Depts.GROUP1_SUBDEPT);
                                Columns.Add(AnalysisGroup1Depts.GROUP1_NAME);
                                // Get name of Floor
                                Columns.Add(vFloorCodeFound);
                                Relations[BranchFloorCodes].NotifyRowWasFoundTo(vFloorCodeFound);
                                Columns.Add(BranchFloorCodes.FLOOR_CODE);
                                Columns.Add(BranchFloorCodes.FLOOR_NAME);
                                #endregion
                            }
                            /// <summary>Init Temp Fields(P#62.1.3.1.2.1)</summary>
                            internal void Run()
                            {
                                Execute();
                            }
                            protected override void OnLoad()
                            {
                                Exit(ExitTiming.AfterRow);
                                KeepChildRelationCacheAlive = true;
                                AllowUserAbort = true;
                            }
                            protected override void OnLeaveRow()
                            {
                                _parent.vTmpRecTyp.Value = u.If(_parent.vSkip == 0, "W", u.If(_parent.vSkip == 1, "P", u.If(_parent.vSkip == 2, "S", "Y")));
                                _parent.vBrnWksInvolved.Value = Exp_6();
                                _parent.vAreaWksInvolved.Value = Exp_6();
                                
                                // Re Period & YTD reports  Weeks Involved (used to calculate '£ Per Bay') requires
                                // adjustment if branch has not been trading since start of period or year resp.
                                if(_parent.vSkip == 1 && _parent._parent.vFirstTradedWeek > _parent._parent._parent._parent._parent.vTYperiodStartWeek || _parent.vSkip == 2 && _parent._parent.vFirstTradedWeek > _parent._parent._parent._parent._parent.vTYseasStartWeek || _parent.vSkip == 3 && _parent._parent.vFirstTradedWeek > _parent._parent._parent._parent._parent.vTYyearStartWeek)
                                {
                                    _parent.vBrnWksInvolved.Value = _parent._parent._parent._parent._parent.vTYreqWeek - _parent._parent.vFirstTradedWeek + 1;
                                }
                                
                                // Department Level
                                if(_parent.vLevel == "D")
                                {
                                    _parent.vTmpFloorCode.Value = _parent.ss252BranchWorkMk2.FloorCode;
                                    _parent.vTmpRptDept.Value = _parent.ss252BranchWorkMk2.ReportDepartment;
                                    _parent.vTmpRptSubD.Value = _parent.ss252BranchWorkMk2.ReportSubDepartment;
                                    _parent.vTmpDesc.Value = u.If(vAnalGrpFound, AnalysisGroup1Depts.GROUP1_NAME, "DEPT " + _parent.ss252AreaWorkMk2.ReportDepartment);
                                    _parent.vTmpStoreTY.Value = u.CastToNumber(u.VarCurr(u.IndexOf(_parent.ss252BranchWorkMk2.TYWeek) + _parent.vSkip));
                                    _parent.vTmpStoreBud.Value = u.CastToNumber(u.VarCurr(u.IndexOf(_parent.ss252BranchWorkMk2.BudgetWeek) + _parent.vSkip));
                                    _parent.vTmpStoreLY.Value = u.CastToNumber(u.VarCurr(u.IndexOf(_parent.ss252BranchWorkMk2.LYWeek) + _parent.vSkip));
                                    _parent.vTmpStoreBays.Value = _parent.ss252BranchWorkMk2.NumberOfBays;
                                    _parent.vTmpAreaTY.Value = u.If(_parent.ss252BranchWorkMk2.FloorCode == 9, 0, u.CastToNumber(u.VarCurr(u.IndexOf(_parent.ss252AreaWorkMk2.TYWeek) + _parent.vSkip)));
                                    _parent.vTmpAreaLY.Value = u.If(_parent.ss252BranchWorkMk2.FloorCode == 9, 0, u.CastToNumber(u.VarCurr(u.IndexOf(_parent.ss252AreaWorkMk2.LYWeek) + _parent.vSkip)));
                                    _parent.vTmpAreaBays.Value = _parent.ss252AreaWorkMk2.NumberOfBays;
                                    _parent.vTmpFloorSales.Value = u.CastToNumber(u.VarCurr(u.IndexOf(_parent.vFloorStoreTYWeek) + _parent.vSkip));
                                    _parent.vTmpFloorBays.Value = _parent.vFloorStoreBays;
                                }
                                // Floor Level
                                if(_parent.vLevel == "F")
                                {
                                    _parent.vTmpFloorCode.Value = _parent.ss252BranchWorkMk2.FloorCode;
                                    _parent.vTmpRptDept.Value = "XX";
                                    _parent.vTmpRptSubD.Value = "XX";
                                    _parent.vTmpDesc.Value = u.If(vFloorCodeFound, BranchFloorCodes.FLOOR_NAME, "FLOOR " + u.Str(_parent.ss252BranchWorkMk2.FloorCode, "N1"));
                                    _parent.vTmpStoreTY.Value = u.CastToNumber(u.VarCurr(u.IndexOf(_parent.vFloorStoreTYWeek) + _parent.vSkip));
                                    _parent.vTmpStoreBud.Value = u.CastToNumber(u.VarCurr(u.IndexOf(_parent.vFloorStoreBudWeek) + _parent.vSkip));
                                    _parent.vTmpStoreLY.Value = u.CastToNumber(u.VarCurr(u.IndexOf(_parent.vFloorStoreLYWeek) + _parent.vSkip));
                                    _parent.vTmpStoreBays.Value = _parent.vFloorStoreBays;
                                    _parent.vTmpAreaTY.Value = u.CastToNumber(u.VarCurr(u.IndexOf(_parent.vFloorAreaTYWeek) + _parent.vSkip));
                                    _parent.vTmpAreaLY.Value = u.CastToNumber(u.VarCurr(u.IndexOf(_parent.vFloorAreaLYWeek) + _parent.vSkip));
                                    _parent.vTmpAreaBays.Value = _parent.vFloorAreaBays;
                                    _parent.vTmpFloorSales.Value = u.CastToNumber(u.VarCurr(u.IndexOf(_parent.vFloorStoreTYWeek) + _parent.vSkip));
                                    _parent.vTmpFloorBays.Value = _parent.vFloorStoreBays;
                                }
                                // Store Level
                                if(_parent.vLevel == "S")
                                {
                                    _parent.vTmpFloorCode.Value = 9;
                                    _parent.vTmpRptDept.Value = "ZZ";
                                    _parent.vTmpRptSubD.Value = "ZA";
                                    _parent.vTmpDesc.Value = "Total Store";
                                    _parent.vTmpStoreTY.Value = u.CastToNumber(u.VarCurr(u.IndexOf(_parent.vTotalStoreTYWeek) + _parent.vSkip));
                                    _parent.vTmpStoreBud.Value = u.CastToNumber(u.VarCurr(u.IndexOf(_parent.vTotalStoreBudWeek) + _parent.vSkip));
                                    _parent.vTmpStoreLY.Value = u.CastToNumber(u.VarCurr(u.IndexOf(_parent.vTotalStoreLYWeek) + _parent.vSkip));
                                    _parent.vTmpStoreBays.Value = _parent.vTotalStoreBays;
                                    _parent.vTmpAreaTY.Value = u.CastToNumber(u.VarCurr(u.IndexOf(_parent.vTotalAreaTYWeek) + _parent.vSkip));
                                    _parent.vTmpAreaLY.Value = u.CastToNumber(u.VarCurr(u.IndexOf(_parent.vTotalAreaLYWeek) + _parent.vSkip));
                                    _parent.vTmpAreaBays.Value = _parent.vTotalAreaBays;
                                    _parent.vTmpFloorSales.Value = 0;
                                    _parent.vTmpFloorBays.Value = 0;
                                }
                            }
                            
                            #region Expressions
                            Number Exp_6()
                            {
                                return u.If(_parent.vSkip == 0, 1, u.If(_parent.vSkip == 1, _parent._parent._parent._parent._parent.vWeeksIntoPeriod, u.If(_parent.vSkip == 2, _parent._parent._parent._parent._parent.vWeeksIntoSeason, _parent._parent._parent._parent._parent.vWeeksIntoYear)));
                            }
                            #endregion
                            
                            
                        }
                        /// <summary>Write Print Work(P#62.1.3.1.2.2)</summary>
                        /// <remark>Last change before Migration: 01/08/2006 09:48:16</remark>
                        class WritePrintWork : SalesAndStock.BusinessProcessBase 
                        {
                            
                            #region Models
                            /// <summary>ss252 Print Work</summary>
                            readonly Models.ss252PrintWork ss252PrintWork = new Models.ss252PrintWork { KeepCacheAliveAfterExit = true, AllowRowLocking = true };
                            #endregion
                            
                            #region Columns
                            /// <summary>v:DBUG ErrA</summary>
                            readonly TextColumn vDBUGErrA = new TextColumn("v:DBUG ErrA", "120");
                            #endregion
                            
                            ReadBranchWork _parent;
                            
                            public WritePrintWork(ReadBranchWork parent)
                            {
                                _parent = parent;
                                Title = "Write Print Work";
                                InitializeDataView();
                            }
                            void InitializeDataView()
                            {
                                
                                Relations.Add(ss252PrintWork, RelationType.InsertIfNotFound, 
                                		ss252PrintWork.BranchNumber.BindEqualTo(_parent._parent.DiaryWork1.BranchNumber).And(
                                		ss252PrintWork.RecordType.BindEqualTo(_parent.vTmpRecTyp)).And(
                                		ss252PrintWork.ReportGroup.BindEqualTo("A")).And(
                                		ss252PrintWork.FloorCode.BindEqualTo(_parent.vTmpFloorCode)).And(
                                		ss252PrintWork.ReportDepartment.BindEqualTo(_parent.vTmpRptDept)).And(
                                		ss252PrintWork.ReportSubDepartment.BindEqualTo(_parent.vTmpRptSubD)), 
                                	ss252PrintWork.SortByss252_Print_Work_x1);
                                
                                
                                
                                #region Columns
                                
                                Columns.Add(ss252PrintWork.BranchNumber);
                                Columns.Add(ss252PrintWork.RecordType);
                                Columns.Add(ss252PrintWork.ReportGroup);
                                Columns.Add(ss252PrintWork.FloorCode);
                                Columns.Add(ss252PrintWork.ReportDepartment);
                                Columns.Add(ss252PrintWork.ReportSubDepartment);
                                Columns.Add(ss252PrintWork.Description);
                                Columns.Add(ss252PrintWork.ThisYear);
                                Columns.Add(ss252PrintWork.Budget);
                                Columns.Add(ss252PrintWork.LastYear);
                                Columns.Add(ss252PrintWork.PCentToBudget);
                                Columns.Add(ss252PrintWork.PCentToLY);
                                Columns.Add(ss252PrintWork.AreaPCentToLY);
                                Columns.Add(ss252PrintWork.PCentVarToArea);
                                Columns.Add(ss252PrintWork.PCentSalesByFloor);
                                Columns.Add(ss252PrintWork.PCentBayByFloor);
                                Columns.Add(ss252PrintWork.SalesPerBay);
                                Columns.Add(ss252PrintWork.AreaSalesPerBay);
                                Columns.Add(ss252PrintWork.BaySalesVarToArea);
                                Columns.Add(ss252PrintWork.BayPCentVarToArea);
                                Columns.Add(ss252PrintWork.BayCount);
                                Columns.Add(vDBUGErrA);
                                #endregion
                            }
                            /// <summary>Write Print Work(P#62.1.3.1.2.2)</summary>
                            internal void Run()
                            {
                                Execute();
                            }
                            protected override void OnLoad()
                            {
                                Exit(ExitTiming.AfterRow);
                                RowLocking = LockingStrategy.OnRowLoading;
                                TransactionScope = TransactionScopes.Task;
                                KeepChildRelationCacheAlive = true;
                                AllowUserAbort = true;
                            }
                            protected override void OnEnterRow()
                            {
                                ss252PrintWork.PCentToLY.Value = 0;
                                ss252PrintWork.AreaPCentToLY.Value = 0;
                                ss252PrintWork.PCentVarToArea.Value = 0;
                                ss252PrintWork.PCentSalesByFloor.Value = 0;
                                ss252PrintWork.PCentBayByFloor.Value = 0;
                                ss252PrintWork.SalesPerBay.Value = 0;
                                ss252PrintWork.AreaSalesPerBay.Value = 0;
                                ss252PrintWork.BaySalesVarToArea.Value = 0;
                                ss252PrintWork.BayPCentVarToArea.Value = 0;
                            }
                            protected override void OnLeaveRow()
                            {
                                ss252PrintWork.Description.Value = _parent.vTmpDesc;
                                ss252PrintWork.ThisYear.Value = _parent.vTmpStoreTY;
                                ss252PrintWork.Budget.Value = _parent.vTmpStoreBud;
                                ss252PrintWork.LastYear.Value = _parent.vTmpStoreLY;
                                if(_parent.vTmpStoreBud != 0)
                                {
                                    ss252PrintWork.PCentToBudget.Value = u.If(u.Abs((_parent.vTmpStoreTY - _parent.vTmpStoreBud) * 100 / _parent.vTmpStoreBud) > 999.99, 999.99, u.Round((_parent.vTmpStoreTY - _parent.vTmpStoreBud) * 100 / _parent.vTmpStoreBud, 3, 2));
                                }
                                if(_parent.vTmpStoreLY != 0)
                                {
                                    ss252PrintWork.PCentToLY.Value = u.If(u.Abs((_parent.vTmpStoreTY - _parent.vTmpStoreLY) * 100 / _parent.vTmpStoreLY) > 999.99, 999.99, u.Round((_parent.vTmpStoreTY - _parent.vTmpStoreLY) * 100 / _parent.vTmpStoreLY, 3, 2));
                                }
                                if(_parent.vTmpAreaLY != 0)
                                {
                                    ss252PrintWork.AreaPCentToLY.Value = u.If(u.Abs((_parent.vTmpAreaTY - _parent.vTmpAreaLY) * 100 / _parent.vTmpAreaLY) > 999.99, 999.99, u.Round((_parent.vTmpAreaTY - _parent.vTmpAreaLY) * 100 / _parent.vTmpAreaLY, 3, 2));
                                }
                                ss252PrintWork.PCentVarToArea.Value = u.If(u.Abs(ss252PrintWork.PCentToLY - ss252PrintWork.AreaPCentToLY) > 999.99, 999.99, ss252PrintWork.PCentToLY - ss252PrintWork.AreaPCentToLY);
                                if(_parent.vTmpFloorSales != 0)
                                {
                                    ss252PrintWork.PCentSalesByFloor.Value = u.Round(_parent.vTmpStoreTY * 100 / _parent.vTmpFloorSales, 3, 1);
                                }
                                if(_parent.vTmpFloorBays != 0)
                                {
                                    ss252PrintWork.PCentBayByFloor.Value = u.Round(_parent.vTmpStoreBays * 100 / _parent.vTmpFloorBays, 3, 1);
                                }
                                if(_parent.vTmpStoreBays != 0)
                                {
                                    ss252PrintWork.SalesPerBay.Value = u.Round(_parent.vTmpStoreTY / _parent.vTmpStoreBays / _parent.vBrnWksInvolved, 8, 0);
                                }
                                if(_parent.vTmpAreaBays != 0)
                                {
                                    ss252PrintWork.AreaSalesPerBay.Value = u.Round(_parent.vTmpAreaTY / _parent.vTmpAreaBays / _parent.vAreaWksInvolved, 8, 0);
                                }
                                ss252PrintWork.BaySalesVarToArea.Value = ss252PrintWork.SalesPerBay - ss252PrintWork.AreaSalesPerBay;
                                if(ss252PrintWork.AreaSalesPerBay != 0)
                                {
                                    ss252PrintWork.BayPCentVarToArea.Value = u.If((ss252PrintWork.SalesPerBay - ss252PrintWork.AreaSalesPerBay) * 100 / ss252PrintWork.AreaSalesPerBay > 999.99, 999.99, u.Round((ss252PrintWork.SalesPerBay - ss252PrintWork.AreaSalesPerBay) * 100 / ss252PrintWork.AreaSalesPerBay, 3, 2));
                                }
                                ss252PrintWork.BayCount.Value = _parent.vTmpStoreBays;
                                // If this is a 'Total Store' line, generate a blank report line after it
                                if(_parent.vTmpRptDept == "ZZ" && _parent.vTmpRptSubD == "ZA")
                                {
                                    Cached<WriteBlankLine>().Run();
                                }
                            }
                            protected override void OnEnd()
                            {
                                vDBUGErrA.Value = u.DbERR("SS");
                                if(u.Len(u.Trim(vDBUGErrA)) > 0)
                                {
                                    Message.ShowWarningInStatusBar("Insert failed in task Write Print Work");
                                    Message.ShowWarningInStatusBar(u.Left(vDBUGErrA, 60));
                                    Message.ShowWarningInStatusBar(u.Mid(vDBUGErrA, 61, 60));
                                }
                            }
                            
                            
                            /// <summary>Write Blank Line(P#62.1.3.1.2.2.1)</summary>
                            /// <remark>Last change before Migration: 25/05/2005 11:42:12</remark>
                            class WriteBlankLine : SalesAndStock.BusinessProcessBase 
                            {
                                
                                #region Models
                                /// <summary>ss252 Print Work</summary>
                                readonly Models.ss252PrintWork ss252PrintWork = new Models.ss252PrintWork { KeepCacheAliveAfterExit = true, AllowRowLocking = true };
                                #endregion
                                
                                #region Columns
                                /// <summary>v:DBUG ErrB</summary>
                                readonly TextColumn vDBUGErrB = new TextColumn("v:DBUG ErrB", "120");
                                #endregion
                                
                                WritePrintWork _parent;
                                
                                public WriteBlankLine(WritePrintWork parent)
                                {
                                    _parent = parent;
                                    Title = "Write Blank Line";
                                    InitializeDataView();
                                }
                                void InitializeDataView()
                                {
                                    
                                    Relations.Add(ss252PrintWork, RelationType.InsertIfNotFound, 
                                    		ss252PrintWork.BranchNumber.BindEqualTo(_parent._parent._parent.DiaryWork1.BranchNumber).And(
                                    		ss252PrintWork.RecordType.BindEqualTo(_parent._parent.vTmpRecTyp)).And(
                                    		ss252PrintWork.ReportGroup.BindEqualTo("A")).And(
                                    		ss252PrintWork.FloorCode.BindEqualTo(_parent._parent.vTmpFloorCode)).And(
                                    		ss252PrintWork.ReportDepartment.BindEqualTo(_parent._parent.vTmpRptDept)).And(
                                    		ss252PrintWork.ReportSubDepartment.BindEqualTo("ZB")), 
                                    	ss252PrintWork.SortByss252_Print_Work_x1);
                                    
                                    
                                    
                                    #region Columns
                                    
                                    // In print work table generate a blank line after 'Total Store' line.
                                    Columns.Add(ss252PrintWork.BranchNumber);
                                    Columns.Add(ss252PrintWork.RecordType);
                                    Columns.Add(ss252PrintWork.ReportGroup);
                                    Columns.Add(ss252PrintWork.FloorCode);
                                    Columns.Add(ss252PrintWork.ReportDepartment);
                                    Columns.Add(ss252PrintWork.ReportSubDepartment);
                                    Columns.Add(ss252PrintWork.Description);
                                    Columns.Add(ss252PrintWork.ThisYear);
                                    Columns.Add(ss252PrintWork.Budget);
                                    Columns.Add(ss252PrintWork.LastYear);
                                    Columns.Add(ss252PrintWork.PCentToBudget);
                                    Columns.Add(ss252PrintWork.PCentToLY);
                                    Columns.Add(ss252PrintWork.AreaPCentToLY);
                                    Columns.Add(ss252PrintWork.PCentVarToArea);
                                    Columns.Add(ss252PrintWork.PCentSalesByFloor);
                                    Columns.Add(ss252PrintWork.PCentBayByFloor);
                                    Columns.Add(ss252PrintWork.SalesPerBay);
                                    Columns.Add(ss252PrintWork.AreaSalesPerBay);
                                    Columns.Add(ss252PrintWork.BaySalesVarToArea);
                                    Columns.Add(ss252PrintWork.BayPCentVarToArea);
                                    Columns.Add(ss252PrintWork.BayCount);
                                    Columns.Add(vDBUGErrB);
                                    #endregion
                                }
                                /// <summary>Write Blank Line(P#62.1.3.1.2.2.1)</summary>
                                internal void Run()
                                {
                                    Execute();
                                }
                                protected override void OnLoad()
                                {
                                    Exit(ExitTiming.AfterRow);
                                    RowLocking = LockingStrategy.OnRowLoading;
                                    TransactionScope = TransactionScopes.Task;
                                    KeepChildRelationCacheAlive = true;
                                    AllowUserAbort = true;
                                }
                                protected override void OnEnd()
                                {
                                    vDBUGErrB.Value = u.DbERR("SS");
                                    if(u.Len(u.Trim(vDBUGErrB)) > 0)
                                    {
                                        Message.ShowWarningInStatusBar("Insert failed in task Write Blank Line");
                                        Message.ShowWarningInStatusBar(u.Left(vDBUGErrB, 60));
                                        Message.ShowWarningInStatusBar(u.Mid(vDBUGErrB, 61, 60));
                                    }
                                }
                                
                                
                            }
                        }
                        /// <summary>Floor Totals(P#62.1.3.1.2.3)</summary>
                        /// <remark>Last change before Migration: 21/06/2011 13:53:05</remark>
                        internal class FloorTotals : SalesAndStock.BusinessProcessBase 
                        {
                            
                            #region Models
                            DynamicSQLEntity sqlEntity;
                            #endregion
                            
                            #region Columns
                            /// <summary>sql:TY_WK</summary>
                            internal readonly NumberColumn sqlTY_WK = new NumberColumn("sql:TY_WK", "N9")
                            {
                            	AllowNull = true
                            };
                            /// <summary>sql:TY_PER</summary>
                            internal readonly NumberColumn sqlTY_PER = new NumberColumn("sql:TY_PER", "N9")
                            {
                            	AllowNull = true
                            };
                            /// <summary>sql:TY_SEAS</summary>
                            readonly NumberColumn sqlTY_SEAS = new NumberColumn("sql:TY_SEAS", "N9")
                            {
                            	AllowNull = true
                            };
                            /// <summary>sql:TY_YR</summary>
                            internal readonly NumberColumn sqlTY_YR = new NumberColumn("sql:TY_YR", "N9")
                            {
                            	AllowNull = true
                            };
                            /// <summary>sql:Bud_WK</summary>
                            internal readonly NumberColumn sqlBud_WK = new NumberColumn("sql:Bud_WK", "N9")
                            {
                            	AllowNull = true
                            };
                            /// <summary>sql:Bud_PER</summary>
                            internal readonly NumberColumn sqlBud_PER = new NumberColumn("sql:Bud_PER", "N9")
                            {
                            	AllowNull = true
                            };
                            /// <summary>sql:Bud_SEAS</summary>
                            readonly NumberColumn sqlBud_SEAS = new NumberColumn("sql:Bud_SEAS", "N9")
                            {
                            	AllowNull = true
                            };
                            /// <summary>sql:Bud_YR</summary>
                            readonly NumberColumn sqlBud_YR = new NumberColumn("sql:Bud_YR", "N9")
                            {
                            	AllowNull = true
                            };
                            /// <summary>sql:LY_WK</summary>
                            internal readonly NumberColumn sqlLY_WK = new NumberColumn("sql:LY_WK", "N9")
                            {
                            	AllowNull = true
                            };
                            /// <summary>sql:LY_PER</summary>
                            internal readonly NumberColumn sqlLY_PER = new NumberColumn("sql:LY_PER", "N9")
                            {
                            	AllowNull = true
                            };
                            /// <summary>sql:LY_SEAS</summary>
                            readonly NumberColumn sqlLY_SEAS = new NumberColumn("sql:LY_SEAS", "N9")
                            {
                            	AllowNull = true
                            };
                            /// <summary>sql:LY_YR</summary>
                            internal readonly NumberColumn sqlLY_YR = new NumberColumn("sql:LY_YR", "N9")
                            {
                            	AllowNull = true
                            };
                            /// <summary>sql:BAYS</summary>
                            internal readonly NumberColumn sqlBAYS = new NumberColumn("sql:BAYS", "N4")
                            {
                            	AllowNull = true
                            };
                            #endregion
                            
                            internal ReadBranchWork _parent;
                            
                            public FloorTotals(ReadBranchWork parent)
                            {
                                _parent = parent;
                                Title = "Floor Totals";
                                InitializeDataView();
                            }
                            void InitializeDataView()
                            {
                                sqlEntity = new DynamicSQLEntity(Models.DataSources.SS, 
@"--  Called by task 'Floor Totals' of MAGIC program ss252
--
select nvl(sum(w.ty_week),0) ty_wk,
       nvl(sum(w.ty_period),0) ty_per,
       nvl(sum(w.ty_season),0) ty_seas,
       nvl(sum(w.ty_year),0) ty_yr,
       nvl(sum(w.budget_week),0) bud_wk,
       nvl(sum(w.budget_period),0) bud_per,
       nvl(sum(w.budget_season),0) bud_seas,
       nvl(sum(w.budget_year),0) bud_yr,
       nvl(sum(w.ly_week),0) ly_wk,
       nvl(sum(w.ly_period),0) ly_per,
       nvl(sum(w.ly_season),0) ly_seas,
       nvl(sum(w.ly_year),0) ly_yr,
       nvl(sum(w.number_of_bays),0) bays
from ss252_branch_Mk2 w
where w.area_code = ':1'
  and w.branch_number = :2
  and w.report_group = ':3'
  and w.floor_code = :4");
                                sqlEntity.AddParameter(() => u.Trim(_parent.ss252BranchWorkMk2.AreaCode)); //:1;
                                sqlEntity.AddParameter(() => _parent.ss252BranchWorkMk2.BranchNumber); //:2;
                                sqlEntity.AddParameter(() => u.Trim(_parent.ss252BranchWorkMk2.ReportGroup)); //:3;
                                sqlEntity.AddParameter(() => _parent.ss252BranchWorkMk2.FloorCode); //:4;
                                sqlEntity.Columns.Add(sqlTY_WK, sqlTY_PER, sqlTY_SEAS, sqlTY_YR, sqlBud_WK, sqlBud_PER, sqlBud_SEAS, sqlBud_YR, sqlLY_WK, sqlLY_PER, sqlLY_SEAS, sqlLY_YR, sqlBAYS);
                                From = sqlEntity;
                                
                                
                                #region Columns
                                
                                Columns.Add(sqlTY_WK);
                                Columns.Add(sqlTY_PER);
                                Columns.Add(sqlTY_SEAS);
                                Columns.Add(sqlTY_YR);
                                Columns.Add(sqlBud_WK);
                                Columns.Add(sqlBud_PER);
                                Columns.Add(sqlBud_SEAS);
                                Columns.Add(sqlBud_YR);
                                Columns.Add(sqlLY_WK);
                                Columns.Add(sqlLY_PER);
                                Columns.Add(sqlLY_SEAS);
                                Columns.Add(sqlLY_YR);
                                Columns.Add(sqlBAYS);
                                #endregion
                            }
                            /// <summary>Floor Totals(P#62.1.3.1.2.3)</summary>
                            internal void Run()
                            {
                                Execute();
                            }
                            protected override void OnLoad()
                            {
                                Exit(ExitTiming.AfterRow);
                                TransactionScope = TransactionScopes.Task;
                                KeepChildRelationCacheAlive = true;
                                AllowDelete = false;
                                AllowInsert = false;
                                AllowUserAbort = true;
                            }
                            protected override void OnLeaveRow()
                            {
                                // Accumulate store floor totals determined by SQL
                                _parent.vFloorStoreTYWeek.Value = sqlTY_WK;
                                _parent.vFloorStoreTYPeriod.Value = sqlTY_PER;
                                _parent.vFloorStoreTYSeason.Value = sqlTY_SEAS;
                                _parent.vFloorStoreTYYear.Value = sqlTY_YR;
                                _parent.vFloorStoreBudWeek.Value = sqlBud_WK;
                                _parent.vFloorStoreBudPeriod.Value = sqlBud_PER;
                                _parent.vFloorStoreBudSeason.Value = sqlBud_SEAS;
                                _parent.vFloorStoreBudYear.Value = sqlBud_YR;
                                _parent.vFloorStoreLYWeek.Value = sqlLY_WK;
                                _parent.vFloorStoreLYPeriod.Value = sqlLY_PER;
                                _parent.vFloorStoreLYSeason.Value = sqlLY_SEAS;
                                _parent.vFloorStoreLYYear.Value = sqlLY_YR;
                                _parent.vFloorStoreBays.Value = sqlBAYS;
                                // Update overall store  totals
                                _parent.vTotalStoreTYWeek.Value += sqlTY_WK;
                                _parent.vTotalStoreTYPeriod.Value += sqlTY_PER;
                                _parent.vTotalStoreTYSeason.Value += sqlTY_SEAS;
                                _parent.vTotalStoreTYYear.Value += sqlTY_YR;
                                _parent.vTotalStoreBudWeek.Value += sqlBud_WK;
                                _parent.vTotalStoreBudPeriod.Value += sqlBud_PER;
                                _parent.vTotalStoreBudSeason.Value += sqlBud_SEAS;
                                _parent.vTotalStoreBudYear.Value += sqlBud_YR;
                                _parent.vTotalStoreLYWeek.Value += sqlLY_WK;
                                _parent.vTotalStoreLYPeriod.Value += sqlLY_PER;
                                _parent.vTotalStoreLYSeason.Value += sqlLY_SEAS;
                                _parent.vTotalStoreLYYear.Value += sqlLY_YR;
                                _parent.vTotalStoreBays.Value += sqlBAYS;
                            }
                            protected override void OnEnd()
                            {
                            }
                            
                            
                            /// <summary>Debug(P#62.1.3.1.2.3.1)</summary>
                            /// <remark>Last change before Migration: 08/11/2006 10:57:56</remark>
                            internal class Debug : SalesAndStock.BusinessProcessBase 
                            {
                                internal FloorTotals _parent;
                                
                                public Debug(FloorTotals parent)
                                {
                                    _parent = parent;
                                    Title = "Debug";
                                    ConfirmExecution = true;
                                    InitializeDataView();
                                }
                                void InitializeDataView()
                                {
                                    
                                    
                                }
                                /// <summary>Debug(P#62.1.3.1.2.3.1)</summary>
                                internal void Run()
                                {
                                    Execute();
                                }
                                protected override void OnLoad()
                                {
                                    Exit(ExitTiming.AfterRow);
                                    AllowUserAbort = true;
                                    if(NewViewRequired)
                                    {
                                        View = ()=> new Views.DiarySlsReptSs252Debug(this);
                                    }
                                }
                                
                                
                            }
                        }
                    }
                }
                /// <summary>Create HTML Files(P#62.1.3.2)</summary>
                /// <remark>Last change before Migration: 17/12/2010 11:27:08</remark>
                class CreateHTMLFiles : SalesAndStock.BusinessProcessBase 
                {
                    
                    #region Models
                    /// <summary>Diary Work1</summary>
                    readonly Models.DiaryWork1 DiaryWork1 = new Models.DiaryWork1 { AllowRowLocking = true };
                    /// <summary>Branch</summary>
                    readonly Models.Branch Branch = new Models.Branch { ReadOnly = true };
                    #endregion
                    
                    #region Columns
                    /// <summary>v:BranchFound</summary>
                    readonly BoolColumn vBranchFound = new BoolColumn("v:BranchFound");
                    /// <summary>v:Report Ind</summary>
                    readonly TextColumn vReportInd = new TextColumn("v:Report Ind", "U");
                    /// <summary>v:rf983 Return Status</summary>
                    readonly NumberColumn vRf983ReturnStatus = new NumberColumn("v:rf983 Return Status", "N1");
                    #endregion
                    
                    GetBranchData _parent;
                    
                    public CreateHTMLFiles(GetBranchData parent)
                    {
                        _parent = parent;
                        Title = "Create HTML Files";
                        InitializeDataView();
                    }
                    void InitializeDataView()
                    {
                        From = DiaryWork1;
                        
                        Relations.Add(Branch, 
                        		Branch.BranchNumber.IsEqualTo(DiaryWork1.BranchNumber), 
                        	Branch.SortByREF_Branch_X1);
                        
                        
                        Where.Add(DiaryWork1.ProgNo.IsEqualTo("ss252"));
                        
                        OrderBy = DiaryWork1.SortBySS_Diary_Work1_X1;
                        
                        #region Columns
                        
                        Columns.Add(DiaryWork1.ProgNo);
                        Columns.Add(DiaryWork1.BranchNumber);
                        Columns.Add(DiaryWork1.AreaCode);
                        Columns.Add(DiaryWork1.CompStore);
                        
                        Columns.Add(vBranchFound);
                        Relations[Branch].NotifyRowWasFoundTo(vBranchFound);
                        Columns.Add(Branch.BranchNumber);
                        Columns.Add(Branch.BranchName);
                        
                        Columns.Add(vReportInd);
                        Columns.Add(vRf983ReturnStatus);
                        #endregion
                    }
                    /// <summary>Create HTML Files(P#62.1.3.2)</summary>
                    internal void Run()
                    {
                        Execute();
                    }
                    protected override void OnLoad()
                    {
                        TransactionScope = TransactionScopes.RowLocking;
                        AllowUserAbort = true;
                    }
                    protected override void OnLeaveRow()
                    {
                        _parent._parent.vTargetDir.Value = u.Trim(_parent._parent.vBaseDir) + "b" + u.Str(DiaryWork1.BranchNumber, "4P0") + u.Trim(_parent._parent.vTargetDirSuffix);
                        _parent._parent.vTargetFile.Value = u.Trim(_parent._parent.vTargetDir) + "/ss252.htm";
                        if(false)
                        {
                            Message.ShowWarningInStatusBar("ss252   Generating " + _parent._parent.vTargetFile);
                        }
                        // Ensure target directory exists
                        if(false)
                        {
                            Windows.OSCommand("%MagicApps%ss/scr/ssdir.bat " + u.Trim(_parent._parent.vTargetDir), true);
                        }
                        if(_parent._parent._parent.pAixMagic == "A")
                        {
                            ApplicationControllerBase.RunProgramFromAnUnreferencedApplication(u.Translate("%MagicApps%rf/ecf/rf.ecf"),"rf983", u.Translate(u.Trim(_parent._parent.vTargetDir)), vRf983ReturnStatus);
                        }
                        Cached<SetIndicator>().Run();
                    }
                    
                    
                    /// <summary>Set Indicator(P#62.1.3.2.1)</summary>
                    /// <remark>Last change before Migration: 20/01/2011 09:46:04</remark>
                    class SetIndicator : SalesAndStock.BusinessProcessBase 
                    {
                        
                        #region Columns
                        /// <summary>v:Dept X Present</summary>
                        readonly TextColumn vDeptXPresent = new TextColumn("v:Dept X Present", "U");
                        #endregion
                        
                        #region Streams
                        /// <summary>Branch Report</summary>
                        FileWriter _ioBranchReport;
                        #endregion
                        
                        CreateHTMLFiles _parent;
                        
                        public SetIndicator(CreateHTMLFiles parent)
                        {
                            _parent = parent;
                            Title = "Set Indicator";
                            InitializeDataView();
                        }
                        void InitializeDataView()
                        {
                            
                            
                            #region Columns
                            
                            // This task implicitly opens the HTML file (in AIX) and controls the
                            // creation of the Weekly, Period and Year-to-date elements of the
                            // report.
                            Columns.Add(vDeptXPresent);
                            #endregion
                        }
                        /// <summary>Set Indicator(P#62.1.3.2.1)</summary>
                        internal void Run()
                        {
                            Execute();
                        }
                        protected override void OnLoad()
                        {
                            Exit(ExitTiming.AfterRow);
                            KeepChildRelationCacheAlive = true;
                            AllowUserAbort = true;
                            
                            _ioBranchReport = new FileWriter(_parent._parent._parent.vTargetFile)
                            			{
                            				Name = "Branch Report"
                            			};
                            _ioBranchReport.Open();
                            Streams.Add(_ioBranchReport);
                        }
                        protected override void OnLeaveRow()
                        {
                            vDeptXPresent.Value = "N";
                            
                            // Generate WEEK report on the HTML file
                            _parent.vReportInd.Value = "W";
                            Cached<ReadPrintWork>().Run();
                            
                            // Generate PERIOD report on the HTML file
                            _parent.vReportInd.Value = "P";
                            Cached<ReadPrintWork>().Run();
                            
                            // Generate SEASON report on the HTML file
                            _parent.vReportInd.Value = "S";
                            Cached<ReadPrintWork>().Run();
                            
                            // Generate YEAR report on the HTML file
                            _parent.vReportInd.Value = "Y";
                            Cached<ReadPrintWork>().Run();
                        }
                        
                        
                        /// <summary>Read Print Work(P#62.1.3.2.1.1)</summary>
                        /// <remark>Last change before Migration: 25/01/2011 10:03:25</remark>
                        class ReadPrintWork : SalesAndStock.BusinessProcessBase 
                        {
                            
                            #region Models
                            /// <summary>ss252 Print Work</summary>
                            readonly Models.ss252PrintWork ss252PrintWork = new Models.ss252PrintWork { AllowRowLocking = true };
                            #endregion
                            
                            #region Columns
                            /// <summary>v:Prt Desc</summary>
                            readonly TextColumn vPrtDesc = new TextColumn("v:Prt Desc", "18");
                            /// <summary>v:Prt TY</summary>
                            readonly TextColumn vPrtTY = new TextColumn("v:Prt TY", "8");
                            /// <summary>v:Prt Bud</summary>
                            readonly TextColumn vPrtBud = new TextColumn("v:Prt Bud", "8");
                            /// <summary>v:Prt LY</summary>
                            readonly TextColumn vPrtLY = new TextColumn("v:Prt LY", "8");
                            /// <summary>v:Prt Bud PCent</summary>
                            readonly TextColumn vPrtBudPCent = new TextColumn("v:Prt Bud PCent", "8");
                            /// <summary>v:Prt LY PCent</summary>
                            readonly TextColumn vPrtLYPCent = new TextColumn("v:Prt LY PCent", "8");
                            /// <summary>v:Prt Area PCent LY</summary>
                            readonly TextColumn vPrtAreaPCentLY = new TextColumn("v:Prt Area PCent LY", "8");
                            /// <summary>v:Prt Var To Area</summary>
                            readonly TextColumn vPrtVarToArea = new TextColumn("v:Prt Var To Area", "8");
                            /// <summary>v:Prt PCent Sales Floor</summary>
                            readonly TextColumn vPrtPCentSalesFloor = new TextColumn("v:Prt PCent Sales Floor", "7");
                            /// <summary>v:Prt PCent Bay Floor</summary>
                            readonly TextColumn vPrtPCentBayFloor = new TextColumn("v:Prt PCent Bay Floor", "7");
                            /// <summary>v:Prt Bay Count</summary>
                            readonly TextColumn vPrtBayCount = new TextColumn("v:Prt Bay Count", "9");
                            /// <summary>v:Prt Sales Per Bay</summary>
                            readonly TextColumn vPrtSalesPerBay = new TextColumn("v:Prt Sales Per Bay", "8");
                            /// <summary>v:Prt Area Sales Per Bay</summary>
                            readonly TextColumn vPrtAreaSalesPerBay = new TextColumn("v:Prt Area Sales Per Bay", "8");
                            /// <summary>v:Prt Bay Sales Var To Area</summary>
                            readonly TextColumn vPrtBaySalesVarToArea = new TextColumn("v:Prt Bay Sales Var To Area", "8");
                            /// <summary>v:Prt Bay PCent Var To Area</summary>
                            readonly TextColumn vPrtBayPCentVarToArea = new TextColumn("v:Prt Bay PCent Var To Area", "8");
                            /// <summary>v:Set Desc To Bold</summary>
                            readonly TextColumn vSetDescToBold = new TextColumn("v:Set Desc To Bold", "U");
                            /// <summary>v:ParticDiffHigh</summary>
                            readonly TextColumn vParticDiffHigh = new TextColumn("v:ParticDiffHigh", "U");
                            /// <summary>v:BudgetNegative</summary>
                            readonly TextColumn vBudgetNegative = new TextColumn("v:BudgetNegative", "U");
                            /// <summary>v:ToAreaNegative</summary>
                            readonly TextColumn vToAreaNegative = new TextColumn("v:ToAreaNegative", "U");
                            /// <summary>v:BayToAreaNegative</summary>
                            readonly TextColumn vBayToAreaNegative = new TextColumn("v:BayToAreaNegative", "U");
                            #endregion
                            
                            #region Layouts
                            /// <summary>ss252W Header</summary>
                            TextTemplate _viewSs252WHeader;
                            /// <summary>ss252P Header</summary>
                            TextTemplate _viewSs252PHeader;
                            /// <summary>ss252S Header</summary>
                            TextTemplate _viewSs252SHeader;
                            /// <summary>ss252Y Header</summary>
                            TextTemplate _viewSs252YHeader;
                            /// <summary>ss252 Details</summary>
                            TextTemplate _viewSs252Details;
                            /// <summary>ss252Footer</summary>
                            TextTemplate _viewSs252Footer;
                            /// <summary>ss252 DeptX Footer</summary>
                            TextTemplate _viewSs252DeptXFooter;
                            #endregion
                            
                            SetIndicator _parent;
                            
                            public ReadPrintWork(SetIndicator parent)
                            {
                                _parent = parent;
                                Title = "Read Print Work";
                                InitializeDataView();
                            }
                            void InitializeDataView()
                            {
                                From = ss252PrintWork;
                                
                                Where.Add(ss252PrintWork.BranchNumber.IsEqualTo(_parent._parent.DiaryWork1.BranchNumber));
                                Where.Add(ss252PrintWork.RecordType.IsEqualTo(_parent._parent.vReportInd));
                                
                                OrderBy = ss252PrintWork.SortByss252_Print_Work_x1;
                                
                                #region Columns
                                
                                Columns.Add(ss252PrintWork.BranchNumber);
                                Columns.Add(ss252PrintWork.RecordType);
                                Columns.Add(ss252PrintWork.ReportGroup);
                                Columns.Add(ss252PrintWork.FloorCode);
                                Columns.Add(ss252PrintWork.ReportDepartment);
                                Columns.Add(ss252PrintWork.ReportSubDepartment);
                                Columns.Add(ss252PrintWork.Description);
                                Columns.Add(ss252PrintWork.ThisYear);
                                Columns.Add(ss252PrintWork.Budget);
                                Columns.Add(ss252PrintWork.LastYear);
                                Columns.Add(ss252PrintWork.PCentToBudget);
                                Columns.Add(ss252PrintWork.PCentToLY);
                                Columns.Add(ss252PrintWork.AreaPCentToLY);
                                Columns.Add(ss252PrintWork.PCentVarToArea);
                                Columns.Add(ss252PrintWork.PCentSalesByFloor);
                                Columns.Add(ss252PrintWork.PCentBayByFloor);
                                Columns.Add(ss252PrintWork.SalesPerBay);
                                Columns.Add(ss252PrintWork.AreaSalesPerBay);
                                Columns.Add(ss252PrintWork.BaySalesVarToArea);
                                Columns.Add(ss252PrintWork.BayPCentVarToArea);
                                Columns.Add(ss252PrintWork.BayCount);
                                // Fields output to HTML file
                                Columns.Add(vPrtDesc).BindValue(ss252PrintWork.Description);
                                Columns.Add(vPrtTY).BindValue(() => u.If(ss252PrintWork.ReportGroup == "D", u.Str(ss252PrintWork.ThisYear, "N3.1Z") + "%", u.If(ss252PrintWork.ReportGroup == "F" || ss252PrintWork.ReportGroup == "G" || ss252PrintWork.ReportGroup == "H", u.Str(ss252PrintWork.ThisYear, "N4.2Z"), u.Str(ss252PrintWork.ThisYear, "N7Z"))));
                                Columns.Add(vPrtBud).BindValue(() => u.Str(ss252PrintWork.Budget, "N7Z"));
                                Columns.Add(vPrtLY).BindValue(() => u.If(ss252PrintWork.ReportGroup == "D", u.Str(ss252PrintWork.LastYear, "N3.1Z") + "%", u.If(ss252PrintWork.ReportGroup == "F" || ss252PrintWork.ReportGroup == "G" || ss252PrintWork.ReportGroup == "H", u.Str(ss252PrintWork.LastYear, "N4.2Z"), u.Str(ss252PrintWork.LastYear, "N7Z"))));
                                Columns.Add(vPrtBudPCent).BindValue(() => u.If(ss252PrintWork.PCentToBudget == 0, " ", u.If(ss252PrintWork.PCentToBudget == 999.99, "********", u.Str(ss252PrintWork.PCentToBudget, "N3.2Z") + "%")));
                                Columns.Add(vPrtLYPCent).BindValue(() => u.If(ss252PrintWork.PCentToLY == 0, " ", u.If(ss252PrintWork.PCentToLY == 999.99, "********", u.Str(ss252PrintWork.PCentToLY, "N3.2Z") + "%")));
                                Columns.Add(vPrtAreaPCentLY).BindValue(() => u.If(ss252PrintWork.AreaPCentToLY == 0, " ", u.If(ss252PrintWork.AreaPCentToLY == 999.99, "********", u.Str(ss252PrintWork.AreaPCentToLY, "N3.2Z") + "%")));
                                Columns.Add(vPrtVarToArea).BindValue(() => u.If(ss252PrintWork.PCentVarToArea == 0, " ", u.If(ss252PrintWork.PCentVarToArea == 999.99, "********", u.Str(ss252PrintWork.PCentVarToArea, "N3.2Z") + "%")));
                                Columns.Add(vPrtPCentSalesFloor).BindValue(() => u.If(ss252PrintWork.PCentSalesByFloor == 0, " ", u.Str(ss252PrintWork.PCentSalesByFloor, "N3.1Z") + "%"));
                                Columns.Add(vPrtPCentBayFloor).BindValue(() => u.If(ss252PrintWork.PCentBayByFloor == 0, " ", u.Str(ss252PrintWork.PCentBayByFloor, "N3.1Z") + "%"));
                                Columns.Add(vPrtBayCount).BindValue(() => u.Str(ss252PrintWork.BayCount, "N8Z"));
                                Columns.Add(vPrtSalesPerBay).BindValue(() => u.Str(ss252PrintWork.SalesPerBay, "N7Z"));
                                Columns.Add(vPrtAreaSalesPerBay).BindValue(() => u.Str(ss252PrintWork.AreaSalesPerBay, "N7Z"));
                                Columns.Add(vPrtBaySalesVarToArea).BindValue(() => u.Str(ss252PrintWork.BaySalesVarToArea, "N7Z"));
                                Columns.Add(vPrtBayPCentVarToArea).BindValue(() => u.If(ss252PrintWork.BayPCentVarToArea == 0, " ", u.If(ss252PrintWork.BayPCentVarToArea == 999.99, "********", u.Str(ss252PrintWork.BayPCentVarToArea, "N3.2Z") + "%")));
                                // Flags required for BOLD or HIGHLIGHTING on HTML report
                                Columns.Add(vSetDescToBold);
                                Columns.Add(vParticDiffHigh);
                                Columns.Add(vBudgetNegative);
                                Columns.Add(vToAreaNegative);
                                Columns.Add(vBayToAreaNegative);
                                #endregion
                            }
                            /// <summary>Read Print Work(P#62.1.3.2.1.1)</summary>
                            internal void Run()
                            {
                                Execute();
                            }
                            protected override void OnLoad()
                            {
                                KeepChildRelationCacheAlive = true;
                                AllowUserAbort = true;
                                
                                _viewSs252WHeader = new TextTemplate("%MagicApps%ss/html/ss252.htm");
                                _viewSs252WHeader.Add(
                                			new Tag("IsHeadW", true, "5"), 
                                			new Tag("IsHead", true, "5"), 
                                			new Tag("HdrWeek", _parent._parent._parent._parent.vHdrWeek), 
                                			new Tag("HdrBranch", _parent._parent._parent._parent.vHdrBranch), 
                                			new Tag("HdrRunDate", _parent._parent._parent._parent.vHdrRunDate));
                                
                                _viewSs252PHeader = new TextTemplate("%MagicApps%ss/html/ss252.htm");
                                _viewSs252PHeader.Add(
                                			new Tag("IsHeadP", true, "5"), 
                                			new Tag("IsHead", true, "5"), 
                                			new Tag("HdrWeek", _parent._parent._parent._parent.vHdrWeek), 
                                			new Tag("HdrBranch", _parent._parent._parent._parent.vHdrBranch), 
                                			new Tag("HdrRunDate", _parent._parent._parent._parent.vHdrRunDate));
                                
                                _viewSs252SHeader = new TextTemplate("%MagicApps%ss/html/ss252.htm");
                                _viewSs252SHeader.Add(
                                			new Tag("IsHeadS", true, "5"), 
                                			new Tag("IsHead", true, "5"), 
                                			new Tag("HdrWeek", _parent._parent._parent._parent.vHdrWeek), 
                                			new Tag("HdrBranch", _parent._parent._parent._parent.vHdrBranch), 
                                			new Tag("HdrRunDate", _parent._parent._parent._parent.vHdrRunDate));
                                
                                _viewSs252YHeader = new TextTemplate("%MagicApps%ss/html/ss252.htm");
                                _viewSs252YHeader.Add(
                                			new Tag("IsHeadY", true, "5"), 
                                			new Tag("IsHead", true, "5"), 
                                			new Tag("HdrWeek", _parent._parent._parent._parent.vHdrWeek), 
                                			new Tag("HdrBranch", _parent._parent._parent._parent.vHdrBranch), 
                                			new Tag("HdrRunDate", _parent._parent._parent._parent.vHdrRunDate));
                                
                                _viewSs252Details = new TextTemplate("%MagicApps%ss/html/ss252.htm");
                                _viewSs252Details.Add(
                                			new Tag("IsDetail", true, "5"), 
                                			new Tag("Desc", vPrtDesc), 
                                			new Tag("TY", vPrtTY), 
                                			new Tag("Bud", vPrtBud), 
                                			new Tag("LY", vPrtLY), 
                                			new Tag("BudPcent", vPrtBudPCent), 
                                			new Tag("LYPcent", vPrtLYPCent), 
                                			new Tag("AreaPcentLY", vPrtAreaPCentLY), 
                                			new Tag("VarToArea", vPrtVarToArea), 
                                			new Tag("PcentSalesFloor", vPrtPCentSalesFloor), 
                                			new Tag("PcentBayFloor", vPrtPCentBayFloor), 
                                			new Tag("BayCount", vPrtBayCount), 
                                			new Tag("SalesPerBay", vPrtSalesPerBay), 
                                			new Tag("AreaSalesPerBay", vPrtAreaSalesPerBay), 
                                			new Tag("BaySalesVarToArea", vPrtBaySalesVarToArea), 
                                			new Tag("BayPcentVarToArea", vPrtBayPCentVarToArea));
                                
                                _viewSs252Footer = new TextTemplate("%MagicApps%ss/html/ss252.htm");
                                _viewSs252Footer.Add(new Tag("IsFooter", true, "5"));
                                
                                _viewSs252DeptXFooter = new TextTemplate("%MagicApps%ss/html/ss252.htm");
                                _viewSs252DeptXFooter.Add(new Tag("IsDeptX", () => u.If(_parent.vDeptXPresent == "Y", true, false), "5"));
                            }
                            protected override void OnStart()
                            {
                                if(_parent._parent.vBranchFound)
                                {
                                    _parent._parent._parent._parent.vHdrBranch.Value = u.If(_parent._parent.vReportInd == "W", "Weekly", u.If(_parent._parent.vReportInd == "P", "Period", u.If(_parent._parent.vReportInd == "S", "Season", "YTD"))) + " Sales Report for " + u.Trim(_parent._parent.Branch.BranchName) + " (" + u.Trim(u.Str(_parent._parent.DiaryWork1.BranchNumber, "####")) + ")";
                                }
                                if(u.Not(_parent._parent.vBranchFound))
                                {
                                    _parent._parent._parent._parent.vHdrBranch.Value = u.If(_parent._parent.vReportInd == "W", "Weekly", u.If(_parent._parent.vReportInd == "P", "Period", u.If(_parent._parent.vReportInd == "S", "Season", "YTD"))) + " Sales Report for store " + u.Trim(u.Str(_parent._parent.DiaryWork1.BranchNumber, "####")) + " (Unknown)";
                                }
                                
                                if(_parent._parent.vReportInd == "W")
                                {
                                    _viewSs252WHeader.WriteTo(_parent._ioBranchReport);
                                }
                                if(_parent._parent.vReportInd == "P")
                                {
                                    _viewSs252PHeader.WriteTo(_parent._ioBranchReport);
                                }
                                if(_parent._parent.vReportInd == "S")
                                {
                                    _viewSs252SHeader.WriteTo(_parent._ioBranchReport);
                                }
                                if(_parent._parent.vReportInd == "Y")
                                {
                                    _viewSs252YHeader.WriteTo(_parent._ioBranchReport);
                                }
                            }
                            protected override void OnEnterRow()
                            {
                                vSetDescToBold.Value = "N";
                                vParticDiffHigh.Value = "N";
                                vBudgetNegative.Value = "N";
                                vToAreaNegative.Value = "N";
                                vBayToAreaNegative.Value = "N";
                            }
                            protected override void OnLeaveRow()
                            {
                                if(u.Trim(ss252PrintWork.Description) == "DEPT X")
                                {
                                    _parent.vDeptXPresent.Value = "Y";
                                }
                                vSetDescToBold.Value = u.If(ss252PrintWork.ReportGroup == "A" && (ss252PrintWork.ReportDepartment == "XX" || ss252PrintWork.ReportDepartment == "ZZ") || ss252PrintWork.ReportGroup == "J" && ss252PrintWork.ReportSubDepartment == "ZZ", "Y", "N");
                                // If  printing dept. figures & associated totals
                                // set Bold & Highlight flags.
                                if(ss252PrintWork.ReportGroup == "A")
                                {
                                    vParticDiffHigh.Value = u.If(u.Abs(ss252PrintWork.PCentSalesByFloor - ss252PrintWork.PCentBayByFloor) > 5, "Y", "N");
                                    vBudgetNegative.Value = u.If(ss252PrintWork.PCentToBudget < 0, "Y", "N");
                                    vToAreaNegative.Value = Exp_35();
                                    vBayToAreaNegative.Value = u.If(ss252PrintWork.BayPCentVarToArea < 0, "Y", "N");
                                }
                                // For OTHER THAN Dept figures & associated totals
                                if(ss252PrintWork.ReportGroup != "A")
                                {
                                    vToAreaNegative.Value = Exp_35();
                                }
                                
                                // NOTE
                                // The report column 'Bay Count' was added to a version of the program after
                                // conversion from MAGIC V8.  The column heading therefore meets with V8 standards
                                // and font size cannot be set as for the other columns.  It is therefore being
                                // initialised via an expression containing HTML directives.
                                // WARNING: If changing the form elsewhere, this column may extend in width to
                                // match the width of the expression and may have to be reset to 7.5.
                                _viewSs252Details.WriteTo(_parent._ioBranchReport);
                            }
                            protected override void OnEnd()
                            {
                                if(_parent.vDeptXPresent == "Y")
                                {
                                    _viewSs252DeptXFooter.WriteTo(_parent._ioBranchReport);
                                }
                                else 
                                {
                                    _viewSs252Footer.WriteTo(_parent._ioBranchReport);
                                }
                            }
                            
                            #region Expressions
                            Text Exp_35()
                            {
                                return u.If(ss252PrintWork.PCentVarToArea < 0, "Y", "N");
                            }
                            #endregion
                            
                            
                        }
                    }
                }
                /// <summary>Customer Counts(P#62.1.3.3)</summary>
                /// <remark>Last change before Migration: 08/11/2006 10:46:17</remark>
                class CustomerCounts : SalesAndStock.BusinessProcessBase 
                {
                    GetBranchData _parent;
                    
                    public CustomerCounts(GetBranchData parent)
                    {
                        _parent = parent;
                        Title = "Customer Counts";
                        InitializeDataView();
                    }
                    void InitializeDataView()
                    {
                        
                        
                    }
                    /// <summary>Customer Counts(P#62.1.3.3)</summary>
                    internal void Run()
                    {
                        Execute();
                    }
                    protected override void OnLoad()
                    {
                        Exit(ExitTiming.AfterRow);
                        AllowUserAbort = true;
                    }
                    protected override void OnLeaveRow()
                    {
                        // Init SQL parameters to accumulate No. Of Customers by store.
                        _parent.vSQLTask.Value = "CC Store Tots";
                        
                        Cached<CCStoreTots>().Run();
                        // Accumulate the area totals for No. Of Customers (COMP stores only)
                        if(_parent.vSQLRanOK)
                        {
                            _parent._parent.vMsgToUser.Value = "Assembling Customer Counts by Area";
                            _parent.vSQLTask.Value = "CC Area Tots";
                            Cached<CCAreaTots>().Run();
                        }
                        // Move Nos Of Customers to Print Work table
                        if(_parent.vSQLRanOK)
                        {
                            _parent._parent.vMsgToUser.Value = "Moving Customer  Counts to Print Work";
                            _parent.vSQLTask.Value = "CC Build Print Work";
                            Cached<CCBuildPrintWork>().Run();
                        }
                    }
                    
                    
                    /// <summary>CC Store Tots(P#62.1.3.3.1)</summary>
                    /// <remark>Last change before Migration: 08/11/2006 10:46:00</remark>
                    class CCStoreTots : SalesAndStock.BusinessProcessBase 
                    {
                        
                        #region Models
                        DynamicSQLEntity sqlEntity;
                        #endregion
                        
                        CustomerCounts _parent;
                        
                        public CCStoreTots(CustomerCounts parent)
                        {
                            _parent = parent;
                            Title = "CC Store Tots";
                            InitializeDataView();
                        }
                        void InitializeDataView()
                        {
                            sqlEntity = new DynamicSQLEntity(Models.DataSources.SS, 
@"insert into ss252_branch_mk2

select q1.area_code, q1.branch_number, 'I', 0, ' ', ' ',
       nvl(q2.TY_week,0) TY_wk, nvl(q3.TY_period,0) TY_per,
          nvl(q4.TY_Season,0) TY_seas, nvl(q5.TY_year,0) TY_yr, 0,0,0,0,
       nvl(q6.LY_week,0) LY_wk, nvl(q7.LY_period,0) LY_per,
          nvl(q8.LY_Season,0) LY_seas, nvl(q9.LY_year,0) LY_yr,
       0, q1.comp_store 
from

   (select w1.branch_number, w1.area_code, w1.comp_store
    from mackays.ss_diary_work1 w1
    where w1.prog_no = 'ss252')                                                   q1,
    
   (select s.branch_number, sum(s.transaction_count) TY_Week
     from mackays.ss_pos_trn_wk_summ s
    where s.week_no = :1
      and ((s.trans_type = 1 and s.trans_sub_type in (' ', 'S', 'E'))
         OR (s.trans_type = 6 and s.trans_sub_type in ('1', 'S', 'E')))
    group by s.branch_number)                                                     q2,
    
   (select s.branch_number, sum(s.transaction_count) TY_Period
     from mackays.ss_pos_trn_wk_summ s
    where s.week_no between :2 and :1
      and ((s.trans_type = 1 and s.trans_sub_type in (' ', 'S', 'E'))
         OR (s.trans_type = 6 and s.trans_sub_type in ('1', 'S', 'E')))
    group by s.branch_number)                                                     q3,
    
   (select s.branch_number, sum(s.transaction_count) TY_Season
     from mackays.ss_pos_trn_wk_summ s
    where s.week_no between :3 and :1
      and ((s.trans_type = 1 and s.trans_sub_type in (' ', 'S', 'E'))
         OR (s.trans_type = 6 and s.trans_sub_type in ('1', 'S', 'E')))
    group by s.branch_number)                                                     q4,    
    
   (select s.branch_number, sum(s.transaction_count) TY_Year
     from mackays.ss_pos_trn_wk_summ s
    where s.week_no between :4 and :1
      and ((s.trans_type = 1 and s.trans_sub_type in (' ', 'S', 'E'))
         OR (s.trans_type = 6 and s.trans_sub_type in ('1', 'S', 'E')))
    group by s.branch_number)                                                     q5,
    
   (select s.branch_number, sum(s.transaction_count) LY_Week
     from mackays.ss_pos_trn_wk_summ s
    where s.week_no = :5
      and ((s.trans_type = 1 and s.trans_sub_type in (' ', 'S', 'E'))
         OR (s.trans_type = 6 and s.trans_sub_type in ('1', 'S', 'E')))
    group by s.branch_number)                                                     q6,
    
   (select s.branch_number, sum(s.transaction_count) LY_Period
     from mackays.ss_pos_trn_wk_summ s
    where s.week_no between :6 and :5
      and ((s.trans_type = 1 and s.trans_sub_type in (' ', 'S', 'E'))
         OR (s.trans_type = 6 and s.trans_sub_type in ('1', 'S', 'E')))
    group by s.branch_number)                                                     q7,
    
   (select s.branch_number, sum(s.transaction_count) LY_Season
     from mackays.ss_pos_trn_wk_summ s
    where s.week_no between :7 and :5
      and ((s.trans_type = 1 and s.trans_sub_type in (' ', 'S', 'E'))
         OR (s.trans_type = 6 and s.trans_sub_type in ('1', 'S', 'E')))
    group by s.branch_number)                                                     q8,
    
   (select s.branch_number, sum(s.transaction_count) LY_Year
     from mackays.ss_pos_trn_wk_summ s
    where s.week_no between :8 and :5
      and ((s.trans_type = 1 and s.trans_sub_type in (' ', 'S', 'E'))
         OR (s.trans_type = 6 and s.trans_sub_type in ('1', 'S', 'E')))
    group by s.branch_number)                                                     q9
     
where q1.branch_number = q2.branch_number (+)
  and q1.branch_number = q3.branch_number (+)
  and q1.branch_number = q4.branch_number (+)
  and q1.branch_number = q5.branch_number (+)
  and q1.branch_number = q6.branch_number (+)
  and q1.branch_number = q7.branch_number (+)
  and q1.branch_number = q8.branch_number (+)
  and q1.branch_number = q9.branch_number (+)");
                            sqlEntity.AddParameter(_parent._parent._parent._parent.vTYreqWeek); //:1;
                            sqlEntity.AddParameter(_parent._parent._parent._parent.vTYperiodStartWeek); //:2;
                            sqlEntity.AddParameter(_parent._parent._parent._parent.vTYseasStartWeek); //:3;
                            sqlEntity.AddParameter(_parent._parent._parent._parent.vTYyearStartWeek); //:4;
                            sqlEntity.AddParameter(_parent._parent._parent.vPYreqWeek); //:5;
                            sqlEntity.AddParameter(_parent._parent._parent.vPYperiodStartWeek); //:6;
                            sqlEntity.AddParameter(_parent._parent._parent.vPYseasStartWeek); //:7;
                            sqlEntity.AddParameter(_parent._parent._parent.vPYyearStartWeek); //:8;
                            sqlEntity.SuccessColumn = _parent._parent.vSQLRanOK;
                            From = sqlEntity;
                            
                            
                            #region Columns
                            
                            // This SQL task assembles Customer Counts by Branch for each report level
                            // & writes them to table ss252_Branch Mk2 with Report Group set to 'I'
                            
                            // Resulting figures will be for This Year and Last Year.
                            #endregion
                        }
                        /// <summary>CC Store Tots(P#62.1.3.3.1)</summary>
                        internal void Run()
                        {
                            Execute();
                        }
                        protected override void OnLoad()
                        {
                            Exit(ExitTiming.AfterRow);
                            TransactionScope = TransactionScopes.Task;
                            KeepChildRelationCacheAlive = true;
                            AllowDelete = false;
                            AllowUserAbort = true;
                        }
                        
                        
                    }
                    /// <summary>CC Area Tots(P#62.1.3.3.2)</summary>
                    /// <remark>Last change before Migration: 08/11/2006 10:46:05</remark>
                    class CCAreaTots : SalesAndStock.BusinessProcessBase 
                    {
                        //sqlEntity
                        #region Models
                        DynamicSQLEntity sqlEntity;
                        #endregion
                        
                        CustomerCounts _parent;
                        
                        public CCAreaTots(CustomerCounts parent)
                        {
                            _parent = parent;
                            Title = "CC Area Tots";
                            InitializeDataView();
                        }
                        void InitializeDataView()
                        {
                            sqlEntity = new DynamicSQLEntity(Models.DataSources.SS, 
@"--
--  Called by task 'CC Area Tots' in MAGIC program ss252
--
insert into ss252_area_mk2
  select w.area_code, w.report_group, w.report_department, w.report_subdepartment,
         nvl(sum(w.ty_week),0) ty_week, nvl(sum(w.ty_period),0) ty_period,
            nvl(sum(w.ty_season),0) ty_seas, nvl(sum(w.ty_year),0) ty_year,
         nvl(sum(w.ly_week),0) ly_week, nvl(sum(w.ly_period),0) ly_period,
            nvl(sum(w.ly_season),0) ly_seas, nvl(sum(w.ly_year),0) ly_year, 0
  from ss252_branch_mk2 w
  where w.comp_store = 'Y'
    and w.report_group = 'I'
  group by w.area_code, w.report_group, w.report_department, w.report_subdepartment");
                            sqlEntity.SuccessColumn = _parent._parent.vSQLRanOK;
                            From = sqlEntity;
                            
                            
                            #region Columns
                            
                            // This SQL task assembles the Customer Count figures by Area by rolling
                            // up relevant data (for COMP stores only) on ss252_Branch_Work to table
                            // ss252_area_Mk2.
                            // Resulting figures will be for This Year and Last Year.
                            #endregion
                        }
                        /// <summary>CC Area Tots(P#62.1.3.3.2)</summary>
                        internal void Run()
                        {
                            Execute();
                        }
                        protected override void OnLoad()
                        {
                            Exit(ExitTiming.AfterRow);
                            TransactionScope = TransactionScopes.Task;
                            KeepChildRelationCacheAlive = true;
                            Activity = Activities.Browse;
                            AllowDelete = false;
                            AllowInsert = false;
                            AllowUpdate = false;
                            AllowUserAbort = true;
                        }
                        
                        
                    }
                    /// <summary>CC Build Print Work(P#62.1.3.3.3)</summary>
                    /// <remark>Last change before Migration: 08/11/2006 10:46:17</remark>
                    class CCBuildPrintWork : SalesAndStock.BusinessProcessBase 
                    {
                        
                        #region Models
                        DynamicSQLEntity sqlEntity;
                        #endregion
                        
                        #region Columns
                        /// <summary>AREA_CODE</summary>
                        readonly TextColumn AREA_CODE = new TextColumn("AREA_CODE", "2")
                        {
                        	AllowNull = true
                        };
                        /// <summary>BRANCH_NUMBER</summary>
                        readonly NumberColumn BRANCH_NUMBER = new NumberColumn("BRANCH_NUMBER", "N4")
                        {
                        	AllowNull = true
                        };
                        /// <summary>REPORT_GROUP</summary>
                        readonly TextColumn REPORT_GROUP = new TextColumn("REPORT_GROUP", "1")
                        {
                        	AllowNull = true
                        };
                        /// <summary>FLOOR_CODE</summary>
                        readonly NumberColumn FLOOR_CODE = new NumberColumn("FLOOR_CODE", "N1")
                        {
                        	AllowNull = true
                        };
                        /// <summary>REPORT_DEPARTMENT</summary>
                        readonly TextColumn REPORT_DEPARTMENT = new TextColumn("REPORT_DEPARTMENT", "2")
                        {
                        	AllowNull = true
                        };
                        /// <summary>REPORT_SUBDEPARTMENT</summary>
                        readonly TextColumn REPORT_SUBDEPARTMENT = new TextColumn("REPORT_SUBDEPARTMENT", "2")
                        {
                        	AllowNull = true
                        };
                        /// <summary>BRN_TY_WEEK</summary>
                        readonly NumberColumn BRN_TY_WEEK = new NumberColumn("BRN_TY_WEEK", "N8")
                        {
                        	AllowNull = true
                        };
                        /// <summary>BRN_TY_PER</summary>
                        readonly NumberColumn BRN_TY_PER = new NumberColumn("BRN_TY_PER", "N8")
                        {
                        	AllowNull = true
                        };
                        /// <summary>BRN_TY_SEAS</summary>
                        readonly NumberColumn BRN_TY_SEAS = new NumberColumn("BRN_TY_SEAS", "N8")
                        {
                        	AllowNull = true
                        };
                        /// <summary>BRN_TY_YR</summary>
                        readonly NumberColumn BRN_TY_YR = new NumberColumn("BRN_TY_YR", "N8")
                        {
                        	AllowNull = true
                        };
                        /// <summary>BRN_LY_WEEK</summary>
                        readonly NumberColumn BRN_LY_WEEK = new NumberColumn("BRN_LY_WEEK", "N8")
                        {
                        	AllowNull = true
                        };
                        /// <summary>BRN_LY_PER</summary>
                        readonly NumberColumn BRN_LY_PER = new NumberColumn("BRN_LY_PER", "N8")
                        {
                        	AllowNull = true
                        };
                        /// <summary>BRN_LY_SEAS</summary>
                        readonly NumberColumn BRN_LY_SEAS = new NumberColumn("BRN_LY_SEAS", "N8")
                        {
                        	AllowNull = true
                        };
                        /// <summary>BRN_LY_YR</summary>
                        readonly NumberColumn BRN_LY_YR = new NumberColumn("BRN_LY_YR", "N8")
                        {
                        	AllowNull = true
                        };
                        /// <summary>A_TY_WEEK</summary>
                        readonly NumberColumn A_TY_WEEK = new NumberColumn("A_TY_WEEK", "N9.2")
                        {
                        	AllowNull = true
                        };
                        /// <summary>A_TY_PER</summary>
                        readonly NumberColumn A_TY_PER = new NumberColumn("A_TY_PER", "N9.2")
                        {
                        	AllowNull = true
                        };
                        /// <summary>A_TY_SEAS</summary>
                        readonly NumberColumn A_TY_SEAS = new NumberColumn("A_TY_SEAS", "N9.2")
                        {
                        	AllowNull = true
                        };
                        /// <summary>A_TY_YR</summary>
                        readonly NumberColumn A_TY_YR = new NumberColumn("A_TY_YR", "N9.2")
                        {
                        	AllowNull = true
                        };
                        /// <summary>A_LY_WEEK</summary>
                        readonly NumberColumn A_LY_WEEK = new NumberColumn("A_LY_WEEK", "N9.2")
                        {
                        	AllowNull = true
                        };
                        /// <summary>A_LY_PER</summary>
                        readonly NumberColumn A_LY_PER = new NumberColumn("A_LY_PER", "N9.2")
                        {
                        	AllowNull = true
                        };
                        /// <summary>A_LY_SEAS</summary>
                        readonly NumberColumn A_LY_SEAS = new NumberColumn("A_LY_SEAS", "N9.2")
                        {
                        	AllowNull = true
                        };
                        /// <summary>A_LY_YR</summary>
                        readonly NumberColumn A_LY_YR = new NumberColumn("A_LY_YR", "N9.2")
                        {
                        	AllowNull = true
                        };
                        /// <summary>STORE_TY_WEEK</summary>
                        readonly NumberColumn STORE_TY_WEEK = new NumberColumn("STORE_TY_WEEK", "10.3")
                        {
                        	AllowNull = true
                        };
                        /// <summary>STORE_TY_PER</summary>
                        readonly NumberColumn STORE_TY_PER = new NumberColumn("STORE_TY_PER", "10.3")
                        {
                        	AllowNull = true
                        };
                        /// <summary>STORE_TY_SEAS</summary>
                        readonly NumberColumn STORE_TY_SEAS = new NumberColumn("STORE_TY_SEAS", "10.3")
                        {
                        	AllowNull = true
                        };
                        /// <summary>STORE_TY_YEAR</summary>
                        readonly NumberColumn STORE_TY_YEAR = new NumberColumn("STORE_TY_YEAR", "10.3")
                        {
                        	AllowNull = true
                        };
                        /// <summary>STORE_LY_WEEK</summary>
                        readonly NumberColumn STORE_LY_WEEK = new NumberColumn("STORE_LY_WEEK", "10.3")
                        {
                        	AllowNull = true
                        };
                        /// <summary>STORE_LY_PER</summary>
                        readonly NumberColumn STORE_LY_PER = new NumberColumn("STORE_LY_PER", "10.3")
                        {
                        	AllowNull = true
                        };
                        /// <summary>STORE_LY_SEAS</summary>
                        readonly NumberColumn STORE_LY_SEAS = new NumberColumn("STORE_LY_SEAS", "10.3")
                        {
                        	AllowNull = true
                        };
                        /// <summary>STORE_LY_YEAR</summary>
                        readonly NumberColumn STORE_LY_YEAR = new NumberColumn("STORE_LY_YEAR", "10.3")
                        {
                        	AllowNull = true
                        };
                        /// <summary>AREA_TY_WEEK</summary>
                        readonly NumberColumn AREA_TY_WEEK = new NumberColumn("AREA_TY_WEEK", "10.3")
                        {
                        	AllowNull = true
                        };
                        /// <summary>AREA_TY_PER</summary>
                        readonly NumberColumn AREA_TY_PER = new NumberColumn("AREA_TY_PER", "10.3")
                        {
                        	AllowNull = true
                        };
                        /// <summary>AREA_TY_SEAS</summary>
                        readonly NumberColumn AREA_TY_SEAS = new NumberColumn("AREA_TY_SEAS", "10.3")
                        {
                        	AllowNull = true
                        };
                        /// <summary>AREA_TY_YR</summary>
                        readonly NumberColumn AREA_TY_YR = new NumberColumn("AREA_TY_YR", "10.3")
                        {
                        	AllowNull = true
                        };
                        /// <summary>AREA_LY_WEEK</summary>
                        readonly NumberColumn AREA_LY_WEEK = new NumberColumn("AREA_LY_WEEK", "10.3")
                        {
                        	AllowNull = true
                        };
                        /// <summary>AREA_LY_PER</summary>
                        readonly NumberColumn AREA_LY_PER = new NumberColumn("AREA_LY_PER", "10.3")
                        {
                        	AllowNull = true
                        };
                        /// <summary>AREA_LY_SEAS</summary>
                        readonly NumberColumn AREA_LY_SEAS = new NumberColumn("AREA_LY_SEAS", "10.3")
                        {
                        	AllowNull = true
                        };
                        /// <summary>AREA_LY_YR</summary>
                        readonly NumberColumn AREA_LY_YR = new NumberColumn("AREA_LY_YR", "10.3")
                        {
                        	AllowNull = true
                        };
                        /// <summary>v:Tmp Rec Typ</summary>
                        readonly TextColumn vTmpRecTyp = new TextColumn("v:Tmp Rec Typ", "U");
                        /// <summary>v:Tmp Brn Cust TY</summary>
                        readonly NumberColumn vTmpBrnCustTY = new NumberColumn("v:Tmp Brn Cust TY", "N7");
                        /// <summary>v:Tmp Brn Cust LY</summary>
                        readonly NumberColumn vTmpBrnCustLY = new NumberColumn("v:Tmp Brn Cust LY", "N7");
                        /// <summary>v:Tmp Area Cust TY</summary>
                        readonly NumberColumn vTmpAreaCustTY = new NumberColumn("v:Tmp Area Cust TY", "N7");
                        /// <summary>v:Tmp Area Cust LY</summary>
                        readonly NumberColumn vTmpAreaCustLY = new NumberColumn("v:Tmp Area Cust LY", "N7");
                        /// <summary>v:Tmp Brn Sales TY</summary>
                        readonly NumberColumn vTmpBrnSalesTY = new NumberColumn("v:Tmp Brn Sales TY", "N7");
                        /// <summary>v:Tmp Brn Sales LY</summary>
                        readonly NumberColumn vTmpBrnSalesLY = new NumberColumn("v:Tmp Brn Sales LY", "N7");
                        /// <summary>v:Tmp Area Sales TY</summary>
                        readonly NumberColumn vTmpAreaSalesTY = new NumberColumn("v:Tmp Area Sales TY", "N7");
                        /// <summary>v:Tmp Area Sales LY</summary>
                        readonly NumberColumn vTmpAreaSalesLY = new NumberColumn("v:Tmp Area Sales LY", "N7");
                        #endregion
                        
                        public CCBuildPrintWork()
                        {
                            Title = "CC Build Print Work";
                            InitializeDataView();
                        }
                        void InitializeDataView()
                        {
                            sqlEntity = new DynamicSQLEntity(Models.DataSources.SS, 
@"--  Called by task 'CC Build Print Work' in MAGIC program ss252
--
select q1.area_code, q1.branch_number, q1.report_group,
       q1.floor_code, q1.report_department, q1.report_subdepartment,
       q1.ty_week brn_ty_week, q1.ty_period brn_ty_per, q1.ty_season brn_ty_seas, q1.ty_year brn_ty_yr,
       q1.ly_week brn_ly_week, q1.ly_period brn_ly_per, q1.ly_season brn_ly_seas, q1.ly_year brn_ly_yr,
       q2.ty_week a_ty_week, q2.ty_period a_ty_per, q2.ty_season a_ty_seas, q2.ty_year a_ty_yr,
       q2.ly_week a_ly_week, q2.ly_period a_ly_per, q2.ly_season a_ly_seas, q2.ly_year a_ly_yr,
       nvl(q3.This_Year,0) store_ty_week, nvl(q4.This_Year,0) store_ty_per,
          nvl(q5.This_Year,0) store_ty_seas, nvl(q6.This_Year,0) store_ty_year, 
       nvl(q3.Last_Year,0) store_ly_week, nvl(q4.Last_Year,0) store_ly_per,
          nvl(q5.Last_Year,0) store_ly_seas, nvl(q6.Last_Year,0) store_ly_year,
       q7.ty_week area_ty_week, q7.ty_period area_ty_per,
          q7.ty_season area_ty_seas, q7.ty_year area_ty_yr,
       q7.ly_week area_ly_week, q7.ly_period area_ly_per,
          q7.ly_season area_ly_seas, q7.ly_year area_ly_yr
       
from

/*                         Select Customer Counts by Store                              */
   (select b.area_code, b.branch_number, b.report_group,
          b.floor_code, b.report_department, b.report_subdepartment,
          b.ty_week, b.ty_period, b.ty_season, b.ty_year,
          b.ly_week, b.ly_period, b.ly_season, b.ly_year
    from ss252_branch_mk2 b
    where b.report_group = 'I')                                      q1,

/*                     Select corresponding Area Customer Counts                        */
   (select a.area_code, a.report_group, a.report_department, a.report_subdepartment,
           a.ty_week, a.ty_period, a.ty_season, a.ty_year,
           a.ly_week, a.ly_period, a.ly_season, a.ly_year
    from ss252_area_mk2 a
    where a.report_group = 'I')                                      q2,

/*                      Select total Store sales for Week                               */
   (select p.branch_number, p.This_Year, p.Last_Year
    from ss252_print_work p
    where p.record_type = 'W'
      and p.report_group = 'A'
      and p.Floor_Code = 9
      and p.report_department = 'ZZ'
      and p.report_subdepartment = 'ZA')                             q3,

/*                      Select total Store sales for Period                             */
   (select p.branch_number, p.This_Year, p.Last_Year
    from ss252_print_work p
    where p.record_type = 'P'
      and p.report_group = 'A'
      and p.Floor_Code = 9
      and p.report_department = 'ZZ'
      and p.report_subdepartment = 'ZA')                             q4,

/*                      Select total Store sales for Season                             */
   (select p.branch_number, p.This_Year, p.Last_Year
    from ss252_print_work p
    where p.record_type = 'S'
      and p.report_group = 'A'
      and p.Floor_Code = 9
      and p.report_department = 'ZZ'
      and p.report_subdepartment = 'ZA')                             q5,

/*                      Select total Store sales for Year                               */
   (select p.branch_number, p.This_Year, p.Last_Year
    from ss252_print_work p
    where p.record_type = 'Y'
      and p.report_group = 'A'
      and p.Floor_Code = 9
      and p.report_department = 'ZZ'
      and p.report_subdepartment = 'ZA')                             q6,

/*                     Select Area Sales for Week, Period, Year                         */
   (select a.area_code,
           sum(a.ty_week) ty_week, sum(a.ty_period) ty_period,
           sum(a.ty_season) ty_season, sum(a.ty_year) ty_year,
           sum(a.ly_week) ly_week, sum(a.ly_period) ly_period,
           sum(a.ly_season) ly_season, sum(a.ly_year) ly_year
    from ss252_area_mk2 a
    where a.report_group = 'A'
    group by a.area_code)                                      q7
      

where q1.area_code = q2.area_code
  and q1.report_group = q2.report_group
  and q1.report_department = q2.report_department
  and q1.report_subdepartment = q2.report_subdepartment
  and q1.branch_number = q3.branch_number (+)
  and q1.branch_number = q4.branch_number (+)
  and q1.branch_number = q5.branch_number (+)
  and q1.branch_number = q6.branch_number (+)
  and q1.area_code = q7.area_code (+)");
                            sqlEntity.Columns.Add(AREA_CODE, BRANCH_NUMBER, REPORT_GROUP, FLOOR_CODE, REPORT_DEPARTMENT, REPORT_SUBDEPARTMENT, BRN_TY_WEEK, BRN_TY_PER, BRN_TY_SEAS, BRN_TY_YR, BRN_LY_WEEK, BRN_LY_PER, BRN_LY_SEAS, BRN_LY_YR, A_TY_WEEK, A_TY_PER, A_TY_SEAS, A_TY_YR, A_LY_WEEK, A_LY_PER, A_LY_SEAS, A_LY_YR, STORE_TY_WEEK, STORE_TY_PER, STORE_TY_SEAS, STORE_TY_YEAR, STORE_LY_WEEK, STORE_LY_PER, STORE_LY_SEAS, STORE_LY_YEAR, AREA_TY_WEEK, AREA_TY_PER, AREA_TY_SEAS, AREA_TY_YR, AREA_LY_WEEK, AREA_LY_PER, AREA_LY_SEAS, AREA_LY_YR);
                            From = sqlEntity;
                            
                            
                            #region Columns
                            
                            // This task selects from the assembled work tables details of Customer Counts
                            // together with total sales at Branch and Area level.
                            // SQL provides the figures for requested year and the previous year at Week,
                            // Period, Season and Year-to-date levels.
                            
                            // Identification columns
                            Columns.Add(AREA_CODE);
                            Columns.Add(BRANCH_NUMBER);
                            Columns.Add(REPORT_GROUP);
                            Columns.Add(FLOOR_CODE);
                            Columns.Add(REPORT_DEPARTMENT);
                            Columns.Add(REPORT_SUBDEPARTMENT);
                            // Customer counts for Branch
                            Columns.Add(BRN_TY_WEEK);
                            Columns.Add(BRN_TY_PER);
                            Columns.Add(BRN_TY_SEAS);
                            Columns.Add(BRN_TY_YR);
                            Columns.Add(BRN_LY_WEEK);
                            Columns.Add(BRN_LY_PER);
                            Columns.Add(BRN_LY_SEAS);
                            Columns.Add(BRN_LY_YR);
                            // Customer counts for corresponding Area
                            Columns.Add(A_TY_WEEK);
                            Columns.Add(A_TY_PER);
                            Columns.Add(A_TY_SEAS);
                            Columns.Add(A_TY_YR);
                            Columns.Add(A_LY_WEEK);
                            Columns.Add(A_LY_PER);
                            Columns.Add(A_LY_SEAS);
                            Columns.Add(A_LY_YR);
                            // Overall sales totals for Branch
                            Columns.Add(STORE_TY_WEEK);
                            Columns.Add(STORE_TY_PER);
                            Columns.Add(STORE_TY_SEAS);
                            Columns.Add(STORE_TY_YEAR);
                            Columns.Add(STORE_LY_WEEK);
                            Columns.Add(STORE_LY_PER);
                            Columns.Add(STORE_LY_SEAS);
                            Columns.Add(STORE_LY_YEAR);
                            // Overall sales totals for Area
                            Columns.Add(AREA_TY_WEEK);
                            Columns.Add(AREA_TY_PER);
                            Columns.Add(AREA_TY_SEAS);
                            Columns.Add(AREA_TY_YR);
                            Columns.Add(AREA_LY_WEEK);
                            Columns.Add(AREA_LY_PER);
                            Columns.Add(AREA_LY_SEAS);
                            Columns.Add(AREA_LY_YR);
                            
                            // Temporary fields for building Print Work records
                            Columns.Add(vTmpRecTyp);
                            Columns.Add(vTmpBrnCustTY);
                            Columns.Add(vTmpBrnCustLY);
                            Columns.Add(vTmpAreaCustTY);
                            Columns.Add(vTmpAreaCustLY);
                            Columns.Add(vTmpBrnSalesTY);
                            Columns.Add(vTmpBrnSalesLY);
                            Columns.Add(vTmpAreaSalesTY);
                            Columns.Add(vTmpAreaSalesLY);
                            #endregion
                        }
                        /// <summary>CC Build Print Work(P#62.1.3.3.3)</summary>
                        internal void Run()
                        {
                            Execute();
                        }
                        protected override void OnLoad()
                        {
                            TransactionScope = TransactionScopes.Task;
                            AllowDelete = false;
                            AllowUserAbort = true;
                        }
                        protected override void OnLeaveRow()
                        {
                            // Generate WEEK print work
                            vTmpRecTyp.Value = "W";
                            vTmpBrnCustTY.Value = BRN_TY_WEEK;
                            vTmpBrnCustLY.Value = BRN_LY_WEEK;
                            vTmpAreaCustTY.Value = A_TY_WEEK;
                            vTmpAreaCustLY.Value = A_LY_WEEK;
                            vTmpBrnSalesTY.Value = STORE_TY_WEEK;
                            vTmpBrnSalesLY.Value = STORE_LY_WEEK;
                            vTmpAreaSalesTY.Value = AREA_TY_WEEK;
                            vTmpAreaSalesLY.Value = AREA_LY_WEEK;
                            Cached<CCWritePrintWork>().Run();
                            // Generate PERIOD print work
                            vTmpRecTyp.Value = "P";
                            vTmpBrnCustTY.Value = BRN_TY_PER;
                            vTmpBrnCustLY.Value = BRN_LY_PER;
                            vTmpAreaCustTY.Value = A_TY_PER;
                            vTmpAreaCustLY.Value = A_LY_PER;
                            vTmpBrnSalesTY.Value = STORE_TY_PER;
                            vTmpBrnSalesLY.Value = STORE_LY_PER;
                            vTmpAreaSalesTY.Value = AREA_TY_PER;
                            vTmpAreaSalesLY.Value = AREA_LY_PER;
                            Cached<CCWritePrintWork>().Run();
                            // Generate SEASON print work
                            vTmpRecTyp.Value = "S";
                            vTmpBrnCustTY.Value = BRN_TY_SEAS;
                            vTmpBrnCustLY.Value = BRN_LY_SEAS;
                            vTmpAreaCustTY.Value = A_TY_SEAS;
                            vTmpAreaCustLY.Value = A_LY_SEAS;
                            vTmpBrnSalesTY.Value = STORE_TY_SEAS;
                            vTmpBrnSalesLY.Value = STORE_LY_SEAS;
                            vTmpAreaSalesTY.Value = AREA_TY_SEAS;
                            vTmpAreaSalesLY.Value = AREA_LY_SEAS;
                            Cached<CCWritePrintWork>().Run();
                            // Generate YEAR print work
                            vTmpRecTyp.Value = "Y";
                            vTmpBrnCustTY.Value = BRN_TY_YR;
                            vTmpBrnCustLY.Value = BRN_LY_YR;
                            vTmpAreaCustTY.Value = A_TY_YR;
                            vTmpAreaCustLY.Value = A_LY_YR;
                            vTmpBrnSalesTY.Value = STORE_TY_YEAR;
                            vTmpBrnSalesLY.Value = STORE_LY_YEAR;
                            vTmpAreaSalesTY.Value = AREA_TY_YR;
                            vTmpAreaSalesLY.Value = AREA_LY_YR;
                            Cached<CCWritePrintWork>().Run();
                        }
                        
                        
                        /// <summary>CC Write Print Work(P#62.1.3.3.3.1)</summary>
                        /// <remark>Last change before Migration: 08/11/2006 10:46:17</remark>
                        class CCWritePrintWork : SalesAndStock.BusinessProcessBase 
                        {
                            
                            #region Models
                            /// <summary>ss252 Print Work</summary>
                            readonly Models.ss252PrintWork ss252PrintWork = new Models.ss252PrintWork { KeepCacheAliveAfterExit = true, AllowRowLocking = true };
                            /// <summary>ss252 Print Work</summary>
                            readonly Models.ss252PrintWork ss252PrintWork1 = new Models.ss252PrintWork { KeepCacheAliveAfterExit = true, AllowRowLocking = true };
                            #endregion
                            
                            #region Columns
                            /// <summary>v:Avg Area Sale TY</summary>
                            readonly NumberColumn vAvgAreaSaleTY = new NumberColumn("v:Avg Area Sale TY", "N8.2");
                            /// <summary>v:Avg Area Sale LY</summary>
                            readonly NumberColumn vAvgAreaSaleLY = new NumberColumn("v:Avg Area Sale LY", "N8.2");
                            #endregion
                            
                            CCBuildPrintWork _parent;
                            
                            public CCWritePrintWork(CCBuildPrintWork parent)
                            {
                                _parent = parent;
                                Title = "CC Write Print Work";
                                InitializeDataView();
                            }
                            void InitializeDataView()
                            {
                                
                                Relations.Add(ss252PrintWork, RelationType.InsertIfNotFound, 
                                		ss252PrintWork.BranchNumber.BindEqualTo(_parent.BRANCH_NUMBER).And(
                                		ss252PrintWork.RecordType.BindEqualTo(_parent.vTmpRecTyp)).And(
                                		ss252PrintWork.ReportGroup.BindEqualTo("I")).And(
                                		ss252PrintWork.FloorCode.BindEqualTo(() => 0)).And(
                                		ss252PrintWork.ReportDepartment.BindEqualTo(_parent.REPORT_DEPARTMENT)).And(
                                		ss252PrintWork.ReportSubDepartment.BindEqualTo(_parent.REPORT_SUBDEPARTMENT)), 
                                	ss252PrintWork.SortByss252_Print_Work_x1);
                                
                                Relations.Add(ss252PrintWork1, RelationType.InsertIfNotFound, 
                                		ss252PrintWork1.BranchNumber.BindEqualTo(_parent.BRANCH_NUMBER).And(
                                		ss252PrintWork1.RecordType.BindEqualTo(_parent.vTmpRecTyp)).And(
                                		ss252PrintWork1.ReportGroup.BindEqualTo("G")).And(
                                		ss252PrintWork1.FloorCode.BindEqualTo(() => 0)).And(
                                		ss252PrintWork1.ReportDepartment.BindEqualTo(_parent.REPORT_DEPARTMENT)).And(
                                		ss252PrintWork1.ReportSubDepartment.BindEqualTo(_parent.REPORT_SUBDEPARTMENT)), 
                                	ss252PrintWork1.SortByss252_Print_Work_x1);
                                
                                
                                
                                #region Columns
                                
                                // This task will generate Print Work table records for:-
                                // No. Of Customers (Report Group 'I') and corresponding
                                // Average Trans Value (Report Group 'G')
                                
                                // Write NO. OF CUSTOMERS record
                                Columns.Add(ss252PrintWork.BranchNumber);
                                Columns.Add(ss252PrintWork.RecordType);
                                Columns.Add(ss252PrintWork.ReportGroup);
                                Columns.Add(ss252PrintWork.FloorCode);
                                Columns.Add(ss252PrintWork.ReportDepartment);
                                Columns.Add(ss252PrintWork.ReportSubDepartment);
                                Columns.Add(ss252PrintWork.Description);
                                Columns.Add(ss252PrintWork.ThisYear);
                                Columns.Add(ss252PrintWork.LastYear);
                                Columns.Add(ss252PrintWork.PCentToLY);
                                Columns.Add(ss252PrintWork.AreaPCentToLY);
                                Columns.Add(ss252PrintWork.PCentVarToArea);
                                
                                // Write AVERAGE TRANS VALUE record.
                                Columns.Add(ss252PrintWork1.BranchNumber);
                                Columns.Add(ss252PrintWork1.RecordType);
                                Columns.Add(ss252PrintWork1.ReportGroup);
                                Columns.Add(ss252PrintWork1.FloorCode);
                                Columns.Add(ss252PrintWork1.ReportDepartment);
                                Columns.Add(ss252PrintWork1.ReportSubDepartment);
                                Columns.Add(ss252PrintWork1.Description);
                                Columns.Add(ss252PrintWork1.ThisYear);
                                Columns.Add(ss252PrintWork1.LastYear);
                                Columns.Add(ss252PrintWork1.PCentToLY);
                                Columns.Add(ss252PrintWork1.AreaPCentToLY);
                                Columns.Add(ss252PrintWork1.PCentVarToArea);
                                
                                Columns.Add(vAvgAreaSaleTY).BindValue(() => u.Round(_parent.vTmpAreaSalesTY / _parent.vTmpAreaCustTY, 8, 2));
                                Columns.Add(vAvgAreaSaleLY).BindValue(() => u.Round(_parent.vTmpAreaSalesLY / _parent.vTmpAreaCustLY, 8, 2));
                                #endregion
                            }
                            /// <summary>CC Write Print Work(P#62.1.3.3.3.1)</summary>
                            internal void Run()
                            {
                                Execute();
                            }
                            protected override void OnLoad()
                            {
                                Exit(ExitTiming.AfterRow);
                                KeepChildRelationCacheAlive = true;
                            }
                            protected override void OnEnterRow()
                            {
                                // For NO. OF CUSTOMERS record
                                ss252PrintWork.PCentToLY.Value = 0;
                                ss252PrintWork.AreaPCentToLY.Value = 0;
                                ss252PrintWork.PCentVarToArea.Value = 0;
                                // For  AVG  record
                                ss252PrintWork1.PCentToLY.Value = 0;
                                ss252PrintWork1.AreaPCentToLY.Value = 0;
                                ss252PrintWork1.PCentVarToArea.Value = 0;
                            }
                            protected override void OnLeaveRow()
                            {
                                // Init NO. OF CUSTOMERS values
                                ss252PrintWork.Description.Value = "No. of Customers";
                                ss252PrintWork.ThisYear.Value = _parent.vTmpBrnCustTY;
                                ss252PrintWork.LastYear.Value = _parent.vTmpBrnCustLY;
                                if(_parent.vTmpBrnCustLY != 0)
                                {
                                    ss252PrintWork.PCentToLY.Value = u.If(u.Abs((_parent.vTmpBrnCustTY - _parent.vTmpBrnCustLY) * 100 / _parent.vTmpBrnCustLY) > 999.99, 999.99, u.Round((_parent.vTmpBrnCustTY - _parent.vTmpBrnCustLY) * 100 / _parent.vTmpBrnCustLY, 3, 2));
                                }
                                if(_parent.vTmpAreaCustLY != 0)
                                {
                                    ss252PrintWork.AreaPCentToLY.Value = u.If(u.Abs((_parent.vTmpAreaCustTY - _parent.vTmpAreaCustLY) * 100 / _parent.vTmpAreaCustLY) > 999.99, 999.99, u.Round((_parent.vTmpAreaCustTY - _parent.vTmpAreaCustLY) * 100 / _parent.vTmpAreaCustLY, 3, 2));
                                }
                                ss252PrintWork.PCentVarToArea.Value = u.If(ss252PrintWork.PCentToLY == 999.99 || ss252PrintWork.AreaPCentToLY == 999.99, 0, u.If(u.Abs(ss252PrintWork.PCentToLY - ss252PrintWork.AreaPCentToLY) > 999.99, 999.99, ss252PrintWork.PCentToLY - ss252PrintWork.AreaPCentToLY));
                                // Init  ATV  values
                                ss252PrintWork1.Description.Value = "ATV";
                                ss252PrintWork1.ThisYear.Value = u.Round(_parent.vTmpBrnSalesTY / _parent.vTmpBrnCustTY, 8, 2);
                                ss252PrintWork1.LastYear.Value = u.Round(_parent.vTmpBrnSalesLY / _parent.vTmpBrnCustLY, 8, 2);
                                if(ss252PrintWork1.LastYear != 0)
                                {
                                    ss252PrintWork1.PCentToLY.Value = u.If(u.Abs((ss252PrintWork1.ThisYear - ss252PrintWork1.LastYear) * 100 / ss252PrintWork1.LastYear) > 999.99, 999.99, u.Round((ss252PrintWork1.ThisYear - ss252PrintWork1.LastYear) * 100 / ss252PrintWork1.LastYear, 3, 2));
                                }
                                if(vAvgAreaSaleLY != 0)
                                {
                                    ss252PrintWork1.AreaPCentToLY.Value = u.If(u.Abs((vAvgAreaSaleTY - vAvgAreaSaleLY) * 100 / vAvgAreaSaleLY) > 999.99, 999.99, u.Round((vAvgAreaSaleTY - vAvgAreaSaleLY) * 100 / vAvgAreaSaleLY, 3, 2));
                                }
                                ss252PrintWork1.PCentVarToArea.Value = u.If(ss252PrintWork1.PCentToLY == 999.99 || ss252PrintWork1.AreaPCentToLY == 999.99, 0, u.If(u.Abs(ss252PrintWork1.PCentToLY - ss252PrintWork1.AreaPCentToLY) > 999.99, 999.99, ss252PrintWork1.PCentToLY - ss252PrintWork1.AreaPCentToLY));
                            }
                            
                            
                        }
                    }
                }
                /// <summary>Concessions(P#62.1.3.4)</summary>
                /// <remark>Last change before Migration: 23/09/2009 10:21:07</remark>
                class Concessions : SalesAndStock.BusinessProcessBase 
                {
                    
                    #region Columns
                    /// <summary>v:Conc TY Start Week</summary>
                    readonly TextColumn vConcTYStartWeek = new TextColumn("v:Conc TY Start Week", "11");
                    /// <summary>v:Conc TY End Week</summary>
                    readonly TextColumn vConcTYEndWeek = new TextColumn("v:Conc TY End Week", "11");
                    /// <summary>v:Conc TY Start Period</summary>
                    readonly TextColumn vConcTYStartPeriod = new TextColumn("v:Conc TY Start Period", "11");
                    /// <summary>v:Conc TY Start Season</summary>
                    readonly TextColumn vConcTYStartSeason = new TextColumn("v:Conc TY Start Season", "11");
                    /// <summary>v:Conc TY Start Year</summary>
                    readonly TextColumn vConcTYStartYear = new TextColumn("v:Conc TY Start Year", "11");
                    /// <summary>v:Conc LY Start Week</summary>
                    readonly TextColumn vConcLYStartWeek = new TextColumn("v:Conc LY Start Week", "11");
                    /// <summary>v:Conc LY End Week</summary>
                    readonly TextColumn vConcLYEndWeek = new TextColumn("v:Conc LY End Week", "11");
                    /// <summary>v:Conc LY Start Period</summary>
                    readonly TextColumn vConcLYStartPeriod = new TextColumn("v:Conc LY Start Period", "11");
                    /// <summary>v:Conc LY Start Season</summary>
                    readonly TextColumn vConcLYStartSeason = new TextColumn("v:Conc LY Start Season", "11");
                    /// <summary>v:Conc LY Start Year</summary>
                    readonly TextColumn vConcLYStartYear = new TextColumn("v:Conc LY Start Year", "11");
                    #endregion
                    
                    GetBranchData _parent;
                    
                    public Concessions(GetBranchData parent)
                    {
                        _parent = parent;
                        Title = "Concessions";
                        InitializeDataView();
                    }
                    void InitializeDataView()
                    {
                        
                        
                        #region Columns
                        
                        // Init SQL parameters for accumulation of Concession figures by store.
                        Columns.Add(vConcTYStartWeek).BindValue(() => u.DStr(_parent._parent._parent.vTYweekStartDate, "DD-MMM-YYYY"));
                        Columns.Add(vConcTYEndWeek).BindValue(() => u.DStr(_parent._parent._parent.vTYweekEndDate, "DD-MMM-YYYY"));
                        Columns.Add(vConcTYStartPeriod).BindValue(() => u.DStr(_parent._parent._parent.vTYperiodStartDate, "DD-MMM-YYYY"));
                        Columns.Add(vConcTYStartSeason).BindValue(() => u.DStr(_parent._parent._parent.vTYseasStartDate, "DD-MMM-YYYY"));
                        Columns.Add(vConcTYStartYear).BindValue(() => u.DStr(_parent._parent._parent.vTYyearStartDate, "DD-MMM-YYYY"));
                        Columns.Add(vConcLYStartWeek).BindValue(() => u.DStr(_parent._parent._parent.vPYweekStartDate, "DD-MMM-YYYY"));
                        Columns.Add(vConcLYEndWeek).BindValue(() => u.DStr(_parent._parent._parent.vPYweekEndDate, "DD-MMM-YYYY"));
                        Columns.Add(vConcLYStartPeriod).BindValue(() => u.DStr(_parent._parent._parent.vPYperiodStartDate, "DD-MMM-YYYY"));
                        Columns.Add(vConcLYStartSeason).BindValue(() => u.DStr(_parent._parent._parent.vPYseasonStartDate, "DD-MMM-YYYY"));
                        Columns.Add(vConcLYStartYear).BindValue(() => u.DStr(_parent._parent._parent.vPYyearStartDate, "DD-MMM-YYYY"));
                        #endregion
                    }
                    /// <summary>Concessions(P#62.1.3.4)</summary>
                    internal void Run()
                    {
                        Execute();
                    }
                    protected override void OnLoad()
                    {
                        Exit(ExitTiming.AfterRow);
                        AllowUserAbort = true;
                    }
                    protected override void OnStart()
                    {
                        Cached<ConcHeaders>().Run();
                    }
                    protected override void OnLeaveRow()
                    {
                        _parent.vSQLTask.Value = "Conc Store Tots";
                        Cached<ConcStoreTots>().Run();
                        
                        // Encode Lloyd budget figures to Branch work table
                        if(_parent.vSQLRanOK)
                        {
                            _parent._parent.vMsgToUser.Value = "Encoding Lloyd Budgets";
                            _parent.vSQLTask.Value = "Conc LS Budgets";
                            Cached<ConcLSBudgets>().Run();
                        }
                        
                        // Encode Logo budget figures to Branch work table
                        if(_parent.vSQLRanOK)
                        {
                            _parent._parent.vMsgToUser.Value = "Encoding Logo Budgets";
                            _parent.vSQLTask.Value = "Conc Logo Budgets";
                            Cached<ConcLogoBudgets>().Run();
                        }
                        
                        // Accumulate the area totals of Concession figures (COMP stores only)
                        if(_parent.vSQLRanOK)
                        {
                            _parent._parent.vMsgToUser.Value = "Assembling Concession figures by Area";
                            _parent.vSQLTask.Value = "Conc Area Tots";
                            Cached<ConcAreaTots>().Run();
                        }
                        // Move Concession figures to Print Work table
                        if(_parent.vSQLRanOK)
                        {
                            _parent._parent.vMsgToUser.Value = "Moving Concession figures to Print Work";
                            _parent.vSQLTask.Value = "Conc Build Print Work";
                            Cached<ConcBuildPrintWork>().Run();
                        }
                    }
                    
                    
                    /// <summary>Conc Headers(P#62.1.3.4.1)</summary>
                    /// <remark>Last change before Migration: 08/11/2006 10:34:28</remark>
                    class ConcHeaders : SalesAndStock.BusinessProcessBase 
                    {
                        
                        #region Models
                        DynamicSQLEntity sqlEntity;
                        #endregion
                        
                        #region Columns
                        /// <summary>BRANCH_NUMBER</summary>
                        readonly NumberColumn BRANCH_NUMBER = new NumberColumn("BRANCH_NUMBER", "N4")
                        {
                        	AllowNull = true
                        };
                        /// <summary>v:Tmp Type</summary>
                        readonly TextColumn vTmpType = new TextColumn("v:Tmp Type", "U");
                        #endregion
                        
                        public ConcHeaders()
                        {
                            Title = "Conc Headers";
                            InitializeDataView();
                        }
                        void InitializeDataView()
                        {
                            sqlEntity = new DynamicSQLEntity(Models.DataSources.SS, 
@"select distinct  w.branch_number
from ss252_print_work w
order by w.branch_number");
                            sqlEntity.Columns.Add(BRANCH_NUMBER);
                            From = sqlEntity;
                            
                            
                            #region Columns
                            
                            // For each branch for which data has already been created on the Print
                            // work table this task will generate 2 additional records thereon for each of the
                            // 3 reports (week, period, year):-
                            // (a)   a blank line record,
                            // (b)   header record bearing description of 'Concessions'.
                            // These lines will appear on the resulting report prior to first actual concession
                            // for each branch.
                            
                            // From SQL
                            Columns.Add(BRANCH_NUMBER);
                            
                            Columns.Add(vTmpType);
                            #endregion
                        }
                        /// <summary>Conc Headers(P#62.1.3.4.1)</summary>
                        internal void Run()
                        {
                            Execute();
                        }
                        protected override void OnLoad()
                        {
                            TransactionScope = TransactionScopes.Task;
                            AllowDelete = false;
                            AllowUserAbort = true;
                        }
                        protected override void OnLeaveRow()
                        {
                            vTmpType.Value = "W";
                            Cached<ConcWriteHdrLines>().Run();
                            
                            vTmpType.Value = "P";
                            Cached<ConcWriteHdrLines>().Run();
                            
                            vTmpType.Value = "S";
                            Cached<ConcWriteHdrLines>().Run();
                            
                            vTmpType.Value = "Y";
                            Cached<ConcWriteHdrLines>().Run();
                        }
                        
                        
                        /// <summary>Conc Write Hdr Lines(P#62.1.3.4.1.1)</summary>
                        /// <remark>Last change before Migration: 17/05/2006 15:51:58</remark>
                        class ConcWriteHdrLines : SalesAndStock.BusinessProcessBase 
                        {
                            
                            #region Models
                            /// <summary>ss252 Print Work</summary>
                            readonly Models.ss252PrintWork ss252PrintWork = new Models.ss252PrintWork { KeepCacheAliveAfterExit = true, AllowRowLocking = true };
                            /// <summary>ss252 Print Work</summary>
                            readonly Models.ss252PrintWork ss252PrintWork1 = new Models.ss252PrintWork { KeepCacheAliveAfterExit = true, AllowRowLocking = true };
                            #endregion
                            
                            ConcHeaders _parent;
                            
                            public ConcWriteHdrLines(ConcHeaders parent)
                            {
                                _parent = parent;
                                Title = "Conc Write Hdr Lines";
                                InitializeDataView();
                            }
                            void InitializeDataView()
                            {
                                
                                Relations.Add(ss252PrintWork, RelationType.InsertIfNotFound, 
                                		ss252PrintWork.BranchNumber.BindEqualTo(_parent.BRANCH_NUMBER).And(
                                		ss252PrintWork.RecordType.BindEqualTo(_parent.vTmpType)).And(
                                		ss252PrintWork.ReportGroup.BindEqualTo("J")).And(
                                		ss252PrintWork.FloorCode.BindEqualTo(() => 0)).And(
                                		ss252PrintWork.ReportDepartment.BindEqualTo(" ")).And(
                                		ss252PrintWork.ReportSubDepartment.BindEqualTo(" ")), 
                                	ss252PrintWork.SortByss252_Print_Work_x1);
                                
                                Relations.Add(ss252PrintWork1, RelationType.InsertIfNotFound, 
                                		ss252PrintWork1.BranchNumber.BindEqualTo(_parent.BRANCH_NUMBER).And(
                                		ss252PrintWork1.RecordType.BindEqualTo(_parent.vTmpType)).And(
                                		ss252PrintWork1.ReportGroup.BindEqualTo("J")).And(
                                		ss252PrintWork1.FloorCode.BindEqualTo(1)).And(
                                		ss252PrintWork1.ReportDepartment.BindEqualTo(" ")).And(
                                		ss252PrintWork1.ReportSubDepartment.BindEqualTo("ZZ")), 
                                	ss252PrintWork1.SortByss252_Print_Work_x1);
                                
                                
                                
                                #region Columns
                                
                                // Generate blank line on report
                                Columns.Add(ss252PrintWork.BranchNumber);
                                Columns.Add(ss252PrintWork.RecordType);
                                Columns.Add(ss252PrintWork.ReportGroup);
                                Columns.Add(ss252PrintWork.FloorCode);
                                Columns.Add(ss252PrintWork.ReportDepartment);
                                Columns.Add(ss252PrintWork.ReportSubDepartment);
                                Columns.Add(ss252PrintWork.Description).BindValue(() => " ");
                                Columns.Add(ss252PrintWork.ThisYear).BindValue(() => 0);
                                Columns.Add(ss252PrintWork.Budget).BindValue(() => 0);
                                Columns.Add(ss252PrintWork.LastYear).BindValue(() => 0);
                                Columns.Add(ss252PrintWork.PCentToBudget).BindValue(() => 0);
                                Columns.Add(ss252PrintWork.PCentToLY).BindValue(() => 0);
                                Columns.Add(ss252PrintWork.AreaPCentToLY).BindValue(() => 0);
                                Columns.Add(ss252PrintWork.PCentVarToArea).BindValue(() => 0);
                                Columns.Add(ss252PrintWork.PCentSalesByFloor).BindValue(() => 0);
                                Columns.Add(ss252PrintWork.PCentBayByFloor).BindValue(() => 0);
                                Columns.Add(ss252PrintWork.SalesPerBay).BindValue(() => 0);
                                Columns.Add(ss252PrintWork.AreaSalesPerBay).BindValue(() => 0);
                                Columns.Add(ss252PrintWork.BaySalesVarToArea).BindValue(() => 0);
                                Columns.Add(ss252PrintWork.BayPCentVarToArea).BindValue(() => 0);
                                Columns.Add(ss252PrintWork.BayCount).BindValue(() => 0);
                                // Generate description 'Concessions' on report
                                Columns.Add(ss252PrintWork1.BranchNumber);
                                Columns.Add(ss252PrintWork1.RecordType);
                                Columns.Add(ss252PrintWork1.ReportGroup);
                                Columns.Add(ss252PrintWork1.FloorCode);
                                Columns.Add(ss252PrintWork1.ReportDepartment);
                                Columns.Add(ss252PrintWork1.ReportSubDepartment);
                                Columns.Add(ss252PrintWork1.Description).BindValue(() => "Concessions");
                                Columns.Add(ss252PrintWork1.ThisYear).BindValue(() => 0);
                                Columns.Add(ss252PrintWork1.Budget).BindValue(() => 0);
                                Columns.Add(ss252PrintWork1.LastYear).BindValue(() => 0);
                                Columns.Add(ss252PrintWork1.PCentToBudget).BindValue(() => 0);
                                Columns.Add(ss252PrintWork1.PCentToLY).BindValue(() => 0);
                                Columns.Add(ss252PrintWork1.AreaPCentToLY).BindValue(() => 0);
                                Columns.Add(ss252PrintWork1.PCentVarToArea).BindValue(() => 0);
                                Columns.Add(ss252PrintWork1.PCentSalesByFloor).BindValue(() => 0);
                                Columns.Add(ss252PrintWork1.PCentBayByFloor).BindValue(() => 0);
                                Columns.Add(ss252PrintWork1.SalesPerBay).BindValue(() => 0);
                                Columns.Add(ss252PrintWork1.AreaSalesPerBay).BindValue(() => 0);
                                Columns.Add(ss252PrintWork1.BaySalesVarToArea).BindValue(() => 0);
                                Columns.Add(ss252PrintWork1.BayPCentVarToArea).BindValue(() => 0);
                                Columns.Add(ss252PrintWork1.BayCount).BindValue(() => 0);
                                #endregion
                            }
                            /// <summary>Conc Write Hdr Lines(P#62.1.3.4.1.1)</summary>
                            internal void Run()
                            {
                                Execute();
                            }
                            protected override void OnLoad()
                            {
                                Exit(ExitTiming.AfterRow);
                                RowLocking = LockingStrategy.OnRowLoading;
                                TransactionScope = TransactionScopes.Task;
                                KeepChildRelationCacheAlive = true;
                            }
                            
                            
                        }
                    }
                    /// <summary>Conc Store Tots(P#62.1.3.4.2)</summary>
                    /// <remark>Last change before Migration: 21/06/2011 13:59:30</remark>
                    class ConcStoreTots : SalesAndStock.BusinessProcessBase 
                    {
                        
                        #region Models
                        DynamicSQLEntity sqlEntity;
                        #endregion
                        
                        Concessions _parent;
                        
                        public ConcStoreTots(Concessions parent)
                        {
                            _parent = parent;
                            Title = "Conc Store Tots";
                            InitializeDataView();
                        }
                        void InitializeDataView()
                        {
                            sqlEntity = new DynamicSQLEntity(Models.DataSources.SS, 
@"--  Called by task 'Conc Store Totals' in MAGIC program ss252
--
insert into ss252_branch_mk2

select q1.area_code, q1.branch_number, 'J', 0, 
       lpad(trim(to_char(x1.concession_code,'99')),2,'0'), ' ',
       nvl(q2.TY_week,0) TY_wk, nvl(q3.TY_period,0) TY_per,
          nvl(q8.TY_season,0) TY_seas, nvl(q4.TY_year,0) TY_yr, 0,0,0,0,
       nvl(q5.LY_week,0) LY_wk, nvl(q6.LY_period,0) LY_per,
          nvl(q9.LY_season,0) LY_seas, nvl(q7.LY_year,0) LY_yr,
       0, q1.comp_store 
from

   (select w1.branch_number, w1.area_code, w1.comp_store
    from mackays.ss_diary_work1 w1
    where w1.prog_no = 'ss252')                                                   q1,
    
    (select d.branch_number, d.concession_code, sum(d.value) 
     from mackays.sla_details d
    where d.till_date between ':8' and ':2'
      and d.concession_code <> 0
    group by d.branch_number, d.concession_code)                                  x1,
        
   (select d.branch_number, d.concession_code, sum(d.value) TY_Week
     from mackays.sla_details d
    where d.till_date between ':1' and ':2'
      and d.concession_code <> 0
    group by d.branch_number, d.concession_code)                                  q2,
    
   (select d.branch_number, d.concession_code, sum(d.value) TY_Period
     from mackays.sla_details d
    where d.till_date between ':3' and ':2'
      and d.concession_code <> 0
    group by d.branch_number, d.concession_code)                                  q3,
    
   (select d.branch_number, d.concession_code, sum(d.value) TY_Year
     from mackays.sla_details d
    where d.till_date between ':4' and ':2'
      and d.concession_code <> 0
    group by d.branch_number, d.concession_code)                                  q4,
    
   (select d.branch_number, d.concession_code, sum(d.value) LY_Week
     from mackays.sla_details d
    where d.till_date between ':5' and ':6'
      and d.concession_code <> 0
    group by d.branch_number, d.concession_code)                                  q5,
    
   (select d.branch_number, d.concession_code, sum(d.value) LY_Period
     from mackays.sla_details d
    where d.till_date between ':7' and ':6'
      and d.concession_code <> 0
    group by d.branch_number, d.concession_code)                                  q6,
    
   (select d.branch_number, d.concession_code, sum(d.value) LY_Year
     from mackays.sla_details d
    where d.till_date between ':8' and ':6'
      and d.concession_code <> 0
    group by d.branch_number, d.concession_code)                                  q7,
    
   (select d.branch_number, d.concession_code, sum(d.value) TY_Season
     from mackays.sla_details d
    where d.till_date between ':9' and ':2'
      and d.concession_code <> 0
    group by d.branch_number, d.concession_code)                                  q8,
        
   (select d.branch_number, d.concession_code, sum(d.value) LY_Season
     from mackays.sla_details d
    where d.till_date between ':10' and ':6'
      and d.concession_code <> 0
    group by d.branch_number, d.concession_code)                                  q9
         
where q1.branch_number = x1.branch_number
  and x1.branch_number = q4.branch_number (+)
  and x1.concession_code = q4.concession_code (+)
  and x1.branch_number = q3.branch_number (+)
  and x1.concession_code = q3.concession_code (+)
  and x1.branch_number = q2.branch_number (+)
  and x1.concession_code = q2.concession_code (+)
  and x1.branch_number = q5.branch_number (+)
  and x1.concession_code = q5.concession_code (+)
  and x1.branch_number = q6.branch_number (+)
  and x1.concession_code = q6.concession_code (+)
  and x1.branch_number = q7.branch_number (+)
  and x1.concession_code = q7.concession_code (+)
  and x1.branch_number = q8.branch_number (+)
  and x1.concession_code = q8.concession_code (+)
  and x1.branch_number = q9.branch_number (+)
  and x1.concession_code = q9.concession_code (+)");
                            sqlEntity.AddParameter(() => u.Trim(_parent.vConcTYStartWeek)); //:1;
                            sqlEntity.AddParameter(() => u.Trim(_parent.vConcTYEndWeek)); //:2;
                            sqlEntity.AddParameter(() => u.Trim(_parent.vConcTYStartPeriod)); //:3;
                            sqlEntity.AddParameter(() => u.Trim(_parent.vConcTYStartYear)); //:4;
                            sqlEntity.AddParameter(() => u.Trim(_parent.vConcLYStartWeek)); //:5;
                            sqlEntity.AddParameter(() => u.Trim(_parent.vConcLYEndWeek)); //:6;
                            sqlEntity.AddParameter(() => u.Trim(_parent.vConcLYStartPeriod)); //:7;
                            sqlEntity.AddParameter(() => u.Trim(_parent.vConcLYStartYear)); //:8;
                            sqlEntity.AddParameter(() => u.Trim(_parent.vConcTYStartSeason)); //:9;
                            sqlEntity.AddParameter(() => u.Trim(_parent.vConcLYStartSeason)); //:10;
                            sqlEntity.SuccessColumn = _parent._parent.vSQLRanOK;
                            From = sqlEntity;
                            
                            
                            #region Columns
                            
                            // This SQL task assembles Concession figures for each branch to be reported &
                            // writes them to table ss252_Branch_Mk2 with Report Group set to 'J'
                            
                            // Resulting figures will be for this year and Last Year.
                            
                            // N.B.   The relevant Concession Code will be recorded in column 'Report
                            // Department' on the work table.
                            #endregion
                        }
                        /// <summary>Conc Store Tots(P#62.1.3.4.2)</summary>
                        internal void Run()
                        {
                            Execute();
                        }
                        protected override void OnLoad()
                        {
                            Exit(ExitTiming.AfterRow);
                            TransactionScope = TransactionScopes.Task;
                            KeepChildRelationCacheAlive = true;
                            Activity = Activities.Browse;
                            AllowDelete = false;
                            AllowInsert = false;
                            AllowUpdate = false;
                            AllowUserAbort = true;
                        }
                        
                        
                    }
                    /// <summary>Conc Area Tots(P#62.1.3.4.3)</summary>
                    /// <remark>Last change before Migration: 08/11/2006 10:46:46</remark>
                    class ConcAreaTots : SalesAndStock.BusinessProcessBase 
                    {
                        
                        #region Models
                        DynamicSQLEntity sqlEntity;
                        #endregion
                        
                        Concessions _parent;
                        
                        public ConcAreaTots(Concessions parent)
                        {
                            _parent = parent;
                            Title = "Conc Area Tots";
                            InitializeDataView();
                        }
                        void InitializeDataView()
                        {
                            sqlEntity = new DynamicSQLEntity(Models.DataSources.SS, 
@"--  Called by task 'Conc Area Totals' in MAGIC program ss252
--
insert into ss252_area_mk2
  select w.area_code, w.report_group, w.report_department, w.report_subdepartment,
         nvl(sum(w.ty_week),0) ty_week, nvl(sum(w.ty_period),0) ty_period,
            nvl(sum(w.ty_season),0) ty_seas, nvl(sum(w.ty_year),0) ty_year,
         nvl(sum(w.ly_week),0) ly_week, nvl(sum(w.ly_period),0) ly_period,
            nvl(sum(w.ly_season),0) ly_seas, nvl(sum(w.ly_year),0) ly_year, 0
  from ss252_branch_mk2 w
  where w.comp_store = 'Y'
    and w.report_group = 'J'
  group by w.area_code, w.report_group, w.report_department, w.report_subdepartment");
                            sqlEntity.SuccessColumn = _parent._parent.vSQLRanOK;
                            From = sqlEntity;
                            
                            
                            #region Columns
                            
                            // SQL task to roll up assembled Branch Concession sales on table ss252_Branch Mk2
                            // to Area level on ss252_Area_Mk2.
                            
                            // Figures will be for Requested Year & Previous Year.
                            #endregion
                        }
                        /// <summary>Conc Area Tots(P#62.1.3.4.3)</summary>
                        internal void Run()
                        {
                            Execute();
                        }
                        protected override void OnLoad()
                        {
                            Exit(ExitTiming.AfterRow);
                            TransactionScope = TransactionScopes.Task;
                            Activity = Activities.Browse;
                            AllowDelete = false;
                            AllowInsert = false;
                            AllowUpdate = false;
                            AllowUserAbort = true;
                        }
                        
                        
                    }
                    /// <summary>Conc LS Budgets(P#62.1.3.4.4)</summary>
                    /// <remark>Last change before Migration: 08/11/2006 10:46:51</remark>
                    class ConcLSBudgets : SalesAndStock.BusinessProcessBase 
                    {
                        
                        #region Models
                        DynamicSQLEntity sqlEntity;
                        #endregion
                        
                        Concessions _parent;
                        
                        public ConcLSBudgets(Concessions parent)
                        {
                            _parent = parent;
                            Title = "Conc LS Budgets";
                            InitializeDataView();
                        }
                        void InitializeDataView()
                        {
                            sqlEntity = new DynamicSQLEntity(Models.DataSources.SS, 
@"--  Called by task 'Conc LS Budgets' of MAGIC program ss252
--
update ss252_branch_mk2 w
  set
    w.budget_week =
       (select round(nvl(sum(b.ladies_sales_budget + b.childs_sales_budget),0),0)
        from   pos_ls_sales_budgets b
        where  b.centuryweek = :1
          and  b.branch_number = w.branch_number),

    w.budget_period =
       (select round(nvl(sum(b.ladies_sales_budget + b.childs_sales_budget),0),0)
        from   pos_ls_sales_budgets b
        where  b.centuryweek between :2 and :1
          and  b.branch_number = w.branch_number),
       
    w.budget_season =
       (select round(nvl(sum(b.ladies_sales_budget + b.childs_sales_budget),0),0)
        from   pos_ls_sales_budgets b
        where  b.centuryweek between :3 and :1
          and  b.branch_number = w.branch_number),
       
    w.budget_year =
       (select round(nvl(sum(b.ladies_sales_budget + b.childs_sales_budget),0),0)
        from   pos_ls_sales_budgets b
        where  b.centuryweek between :4 and :1
          and  b.branch_number = w.branch_number)
where w.report_group = 'J'
  and w.report_department = '80'");
                            sqlEntity.AddParameter(_parent._parent._parent._parent.vTYreqWeek); //:1;
                            sqlEntity.AddParameter(_parent._parent._parent._parent.vTYperiodStartWeek); //:2;
                            sqlEntity.AddParameter(_parent._parent._parent._parent.vTYseasStartWeek); //:3;
                            sqlEntity.AddParameter(_parent._parent._parent._parent.vTYyearStartWeek); //:4;
                            sqlEntity.SuccessColumn = _parent._parent.vSQLRanOK;
                            From = sqlEntity;
                            
                            
                            #region Columns
                            
                            // Use SQL to encode budget figures for Lloyd Shoes to branch work table.
                            #endregion
                        }
                        /// <summary>Conc LS Budgets(P#62.1.3.4.4)</summary>
                        internal void Run()
                        {
                            Execute();
                        }
                        protected override void OnLoad()
                        {
                            Exit(ExitTiming.AfterRow);
                            TransactionScope = TransactionScopes.Task;
                            Activity = Activities.Browse;
                            AllowDelete = false;
                            AllowInsert = false;
                            AllowUpdate = false;
                            AllowUserAbort = true;
                        }
                        
                        
                    }
                    /// <summary>Conc Logo Budgets(P#62.1.3.4.5)</summary>
                    /// <remark>Last change before Migration: 08/11/2006 10:46:56</remark>
                    class ConcLogoBudgets : SalesAndStock.BusinessProcessBase 
                    {
                        
                        #region Models
                        DynamicSQLEntity sqlEntity;
                        #endregion
                        
                        Concessions _parent;
                        
                        public ConcLogoBudgets(Concessions parent)
                        {
                            _parent = parent;
                            Title = "Conc Logo Budgets";
                            InitializeDataView();
                        }
                        void InitializeDataView()
                        {
                            sqlEntity = new DynamicSQLEntity(Models.DataSources.SS,
@"--  Called by task 'Conc Logo Budgets' of MAGIC program ss252
--
update ss252_branch_mk2 w
  set
    w.budget_week =
       (select round(nvl(sum(b.sales_budget),0),0)
        from   pos_logo_sales_budgets b
        where  b.centuryweek = :1
          and  b.branch_number = w.branch_number),

    w.budget_period =
       (select round(nvl(sum(b.sales_budget),0),0)
        from   pos_logo_sales_budgets b
        where  b.centuryweek between :2 and :1
          and  b.branch_number = w.branch_number),

    w.budget_season =
       (select round(nvl(sum(b.sales_budget),0),0)
        from   pos_logo_sales_budgets b
        where  b.centuryweek between :3 and :1
          and  b.branch_number = w.branch_number),          
       
    w.budget_year =
       (select round(nvl(sum(b.sales_budget),0),0)
        from   pos_logo_sales_budgets b
        where  b.centuryweek between :4 and :1
          and  b.branch_number = w.branch_number)
where w.report_group = 'J'
  and w.report_department = '30'");
                            sqlEntity.AddParameter(_parent._parent._parent._parent.vTYreqWeek); //:1;
                            sqlEntity.AddParameter(_parent._parent._parent._parent.vTYperiodStartWeek); //:2;
                            sqlEntity.AddParameter(_parent._parent._parent._parent.vTYseasStartWeek); //:3;
                            sqlEntity.AddParameter(_parent._parent._parent._parent.vTYyearStartWeek); //:4;
                            sqlEntity.SuccessColumn = _parent._parent.vSQLRanOK;
                            From = sqlEntity;
                            
                            
                            #region Columns
                            
                            // Use SQL to encode budget figures for Logo to branch work table.
                            #endregion
                        }
                        /// <summary>Conc Logo Budgets(P#62.1.3.4.5)</summary>
                        internal void Run()
                        {
                            Execute();
                        }
                        protected override void OnLoad()
                        {
                            Exit(ExitTiming.AfterRow);
                            TransactionScope = TransactionScopes.Task;
                            Activity = Activities.Browse;
                            AllowDelete = false;
                            AllowInsert = false;
                            AllowUpdate = false;
                            AllowUserAbort = true;
                        }
                        
                        
                    }
                    /// <summary>Conc Build Print Work(P#62.1.3.4.6)</summary>
                    /// <remark>Last change before Migration: 18/12/2006 14:10:39</remark>
                    class ConcBuildPrintWork : SalesAndStock.BusinessProcessBase 
                    {
                        
                        #region Models
                        DynamicSQLEntity sqlEntity;
                        #endregion
                        
                        #region Columns
                        /// <summary>AREA_CODE</summary>
                        readonly TextColumn AREA_CODE = new TextColumn("AREA_CODE", "2")
                        {
                        	AllowNull = true
                        };
                        /// <summary>BRANCH_NUMBER</summary>
                        readonly NumberColumn BRANCH_NUMBER = new NumberColumn("BRANCH_NUMBER", "N4")
                        {
                        	AllowNull = true
                        };
                        /// <summary>REPORT_GROUP</summary>
                        readonly TextColumn REPORT_GROUP = new TextColumn("REPORT_GROUP", "1")
                        {
                        	AllowNull = true
                        };
                        /// <summary>FLOOR_CODE</summary>
                        readonly NumberColumn FLOOR_CODE = new NumberColumn("FLOOR_CODE", "N1")
                        {
                        	AllowNull = true
                        };
                        /// <summary>REPORT_DEPARTMENT</summary>
                        readonly TextColumn REPORT_DEPARTMENT = new TextColumn("REPORT_DEPARTMENT", "2")
                        {
                        	AllowNull = true
                        };
                        /// <summary>REPORT_SUBDEPARTMENT</summary>
                        readonly TextColumn REPORT_SUBDEPARTMENT = new TextColumn("REPORT_SUBDEPARTMENT", "2")
                        {
                        	AllowNull = true
                        };
                        /// <summary>CONC_NAME</summary>
                        readonly TextColumn CONC_NAME = new TextColumn("CONC_NAME", "20")
                        {
                        	AllowNull = true
                        };
                        /// <summary>BRN_TY_WEEK</summary>
                        readonly NumberColumn BRN_TY_WEEK = new NumberColumn("BRN_TY_WEEK", "N8")
                        {
                        	AllowNull = true
                        };
                        /// <summary>BRN_TY_PER</summary>
                        readonly NumberColumn BRN_TY_PER = new NumberColumn("BRN_TY_PER", "N8")
                        {
                        	AllowNull = true
                        };
                        /// <summary>BRN_TY_SEAS</summary>
                        readonly NumberColumn BRN_TY_SEAS = new NumberColumn("BRN_TY_SEAS", "N8")
                        {
                        	AllowNull = true
                        };
                        /// <summary>BRN_TY_YR</summary>
                        readonly NumberColumn BRN_TY_YR = new NumberColumn("BRN_TY_YR", "N8")
                        {
                        	AllowNull = true
                        };
                        /// <summary>BUDGET_WEEK</summary>
                        readonly NumberColumn BUDGET_WEEK = new NumberColumn("BUDGET_WEEK", "N8")
                        {
                        	AllowNull = true
                        };
                        /// <summary>BUDGET_PERIOD</summary>
                        readonly NumberColumn BUDGET_PERIOD = new NumberColumn("BUDGET_PERIOD", "N8")
                        {
                        	AllowNull = true
                        };
                        /// <summary>BUDGET_SEASON</summary>
                        readonly NumberColumn BUDGET_SEASON = new NumberColumn("BUDGET_SEASON", "N8")
                        {
                        	AllowNull = true
                        };
                        /// <summary>BUDGET_YEAR</summary>
                        readonly NumberColumn BUDGET_YEAR = new NumberColumn("BUDGET_YEAR", "N8")
                        {
                        	AllowNull = true
                        };
                        /// <summary>BRN_LY_WEEK</summary>
                        readonly NumberColumn BRN_LY_WEEK = new NumberColumn("BRN_LY_WEEK", "N8")
                        {
                        	AllowNull = true
                        };
                        /// <summary>BRN_LY_PER</summary>
                        readonly NumberColumn BRN_LY_PER = new NumberColumn("BRN_LY_PER", "N8")
                        {
                        	AllowNull = true
                        };
                        /// <summary>BRN_LY_SEAS</summary>
                        readonly NumberColumn BRN_LY_SEAS = new NumberColumn("BRN_LY_SEAS", "N8")
                        {
                        	AllowNull = true
                        };
                        /// <summary>BRN_LY_YR</summary>
                        readonly NumberColumn BRN_LY_YR = new NumberColumn("BRN_LY_YR", "N8")
                        {
                        	AllowNull = true
                        };
                        /// <summary>A_TY_WEEK</summary>
                        readonly NumberColumn A_TY_WEEK = new NumberColumn("A_TY_WEEK", "N9.2")
                        {
                        	AllowNull = true
                        };
                        /// <summary>A_TY_PER</summary>
                        readonly NumberColumn A_TY_PER = new NumberColumn("A_TY_PER", "N9.2")
                        {
                        	AllowNull = true
                        };
                        /// <summary>A_TY_SEAS</summary>
                        readonly NumberColumn A_TY_SEAS = new NumberColumn("A_TY_SEAS", "N9.2")
                        {
                        	AllowNull = true
                        };
                        /// <summary>A_TY_YR</summary>
                        readonly NumberColumn A_TY_YR = new NumberColumn("A_TY_YR", "N9.2")
                        {
                        	AllowNull = true
                        };
                        /// <summary>A_LY_WEEK</summary>
                        readonly NumberColumn A_LY_WEEK = new NumberColumn("A_LY_WEEK", "N9.2")
                        {
                        	AllowNull = true
                        };
                        /// <summary>A_LY_PER</summary>
                        readonly NumberColumn A_LY_PER = new NumberColumn("A_LY_PER", "N9.2")
                        {
                        	AllowNull = true
                        };
                        /// <summary>A_LY_SEAS</summary>
                        readonly NumberColumn A_LY_SEAS = new NumberColumn("A_LY_SEAS", "N9.2")
                        {
                        	AllowNull = true
                        };
                        /// <summary>A_LY_YR</summary>
                        readonly NumberColumn A_LY_YR = new NumberColumn("A_LY_YR", "N9.2")
                        {
                        	AllowNull = true
                        };
                        /// <summary>v:Tmp Rec Typ</summary>
                        readonly TextColumn vTmpRecTyp = new TextColumn("v:Tmp Rec Typ", "U");
                        /// <summary>v:Tmp Brn Conc TY</summary>
                        readonly NumberColumn vTmpBrnConcTY = new NumberColumn("v:Tmp Brn Conc TY", "N7");
                        /// <summary>v:Tmp Brn Conc LY</summary>
                        readonly NumberColumn vTmpBrnConcLY = new NumberColumn("v:Tmp Brn Conc LY", "N7");
                        /// <summary>v:Tmp Area Conc TY</summary>
                        readonly NumberColumn vTmpAreaConcTY = new NumberColumn("v:Tmp Area Conc TY", "N7");
                        /// <summary>v:Tmp Area Conc LY</summary>
                        readonly NumberColumn vTmpAreaConcLY = new NumberColumn("v:Tmp Area Conc LY", "N7");
                        /// <summary>v:Tmp Budget</summary>
                        readonly NumberColumn vTmpBudget = new NumberColumn("v:Tmp Budget", "N7");
                        #endregion
                        
                        public ConcBuildPrintWork()
                        {
                            Title = "Conc Build Print Work";
                            InitializeDataView();
                        }
                        void InitializeDataView()
                        {
                            sqlEntity = new DynamicSQLEntity(Models.DataSources.SS, 
@"--  Called by task 'Conc Build Print Work' in MAGIC program ss252
--
select q1.area_code, q1.branch_number, q1.report_group,
       q1.floor_code, q1.report_department, q1.report_subdepartment, q1.conc_name,
       q1.ty_week brn_ty_week, q1.ty_period brn_ty_per, q1.ty_season brn_ty_seas, q1.ty_year brn_ty_yr,
       q1.budget_week, q1.budget_period, q1.budget_season, q1.budget_year,
       q1.ly_week brn_ly_week, q1.ly_period brn_ly_per, q1.ly_season brn_ly_seas, q1.ly_year brn_ly_yr,
       nvl(q2.ty_week,0) a_ty_week, nvl(q2.ty_period,0) a_ty_per,
         nvl(q2.ty_season,0) a_ty_seas, nvl(q2.ty_year,0) a_ty_yr,
       nvl(q2.ly_week,0) a_ly_week, nvl(q2.ly_period,0) a_ly_per,
         nvl(q2.ly_season,0) a_ly_seas, nvl(q2.ly_year,0) a_ly_yr
       
from

/*                     Select Concessions by Store           (N.B. Concession Code is stored                       
                                                              in Column 'report department') */
   (select b.area_code, b.branch_number, b.report_group,
           b.floor_code, b.report_department, b.report_subdepartment,
           nvl(c.concession_name,'Concesssion ' || b.report_department) conc_name,
           b.ty_week, b.ty_period, b.ty_season, b.ty_year,
           b.budget_week, b.budget_period, b.budget_season, b.budget_year,
           b.ly_week, b.ly_period, b.ly_season, b.ly_year
    from   ss252_branch_mk2 b, ref_concess c
    where  b.report_group = 'J'
      and  to_number(b.report_department) = c.concession_code (+))    q1,

/*                     Select corresponding Area Concessions                        */
   (select a.area_code, a.report_group, a.report_department, a.report_subdepartment,
           a.ty_week, a.ty_period, a.ty_season, a.ty_year,
           a.ly_week, a.ly_period, a.ly_season, a.ly_year
    from   ss252_area_mk2 a
    where  a.report_group = 'J')                                      q2
      
where q1.area_code = q2.area_code (+)
  and q1.report_group = q2.report_group (+)
  and q1.report_department = q2.report_department (+)
  and q1.report_subdepartment = q2.report_subdepartment (+)
order by q1.branch_number, q1.report_department");
                            sqlEntity.Columns.Add(AREA_CODE, BRANCH_NUMBER, REPORT_GROUP, FLOOR_CODE, REPORT_DEPARTMENT, REPORT_SUBDEPARTMENT, CONC_NAME, BRN_TY_WEEK, BRN_TY_PER, BRN_TY_SEAS, BRN_TY_YR, BUDGET_WEEK, BUDGET_PERIOD, BUDGET_SEASON, BUDGET_YEAR, BRN_LY_WEEK, BRN_LY_PER, BRN_LY_SEAS, BRN_LY_YR, A_TY_WEEK, A_TY_PER, A_TY_SEAS, A_TY_YR, A_LY_WEEK, A_LY_PER, A_LY_SEAS, A_LY_YR);
                            From = sqlEntity;
                            
                            
                            #region Columns
                            
                            // Select assembled Branch/Area Concession figures for TY and LY using SQL to allow
                            // reformatting into the Print Work table.
                            
                            // From SQL
                            Columns.Add(AREA_CODE);
                            Columns.Add(BRANCH_NUMBER);
                            Columns.Add(REPORT_GROUP);
                            Columns.Add(FLOOR_CODE);
                            Columns.Add(REPORT_DEPARTMENT);
                            Columns.Add(REPORT_SUBDEPARTMENT);
                            Columns.Add(CONC_NAME);
                            Columns.Add(BRN_TY_WEEK);
                            Columns.Add(BRN_TY_PER);
                            Columns.Add(BRN_TY_SEAS);
                            Columns.Add(BRN_TY_YR);
                            Columns.Add(BUDGET_WEEK);
                            Columns.Add(BUDGET_PERIOD);
                            Columns.Add(BUDGET_SEASON);
                            Columns.Add(BUDGET_YEAR);
                            Columns.Add(BRN_LY_WEEK);
                            Columns.Add(BRN_LY_PER);
                            Columns.Add(BRN_LY_SEAS);
                            Columns.Add(BRN_LY_YR);
                            Columns.Add(A_TY_WEEK);
                            Columns.Add(A_TY_PER);
                            Columns.Add(A_TY_SEAS);
                            Columns.Add(A_TY_YR);
                            Columns.Add(A_LY_WEEK);
                            Columns.Add(A_LY_PER);
                            Columns.Add(A_LY_SEAS);
                            Columns.Add(A_LY_YR);
                            
                            // Temporary fields used for building Print Work records for each of the report
                            // levels required  -  Week, Period, Season & YTD.
                            Columns.Add(vTmpRecTyp);
                            Columns.Add(vTmpBrnConcTY);
                            Columns.Add(vTmpBrnConcLY);
                            Columns.Add(vTmpAreaConcTY);
                            Columns.Add(vTmpAreaConcLY);
                            Columns.Add(vTmpBudget);
                            #endregion
                        }
                        /// <summary>Conc Build Print Work(P#62.1.3.4.6)</summary>
                        internal void Run()
                        {
                            Execute();
                        }
                        protected override void OnLoad()
                        {
                            TransactionScope = TransactionScopes.Task;
                            AllowDelete = false;
                            AllowUserAbort = true;
                        }
                        protected override void OnLeaveRow()
                        {
                            // Generate WEEK print work record.
                            vTmpRecTyp.Value = "W";
                            vTmpBrnConcTY.Value = BRN_TY_WEEK;
                            vTmpBrnConcLY.Value = BRN_LY_WEEK;
                            vTmpAreaConcTY.Value = A_TY_WEEK;
                            vTmpAreaConcLY.Value = A_LY_WEEK;
                            vTmpBudget.Value = BUDGET_WEEK;
                            Cached<ConcWritePrintWork>().Run();
                            
                            // Generate PERIOD print work record.
                            vTmpRecTyp.Value = "P";
                            vTmpBrnConcTY.Value = BRN_TY_PER;
                            vTmpBrnConcLY.Value = BRN_LY_PER;
                            vTmpAreaConcTY.Value = A_TY_PER;
                            vTmpAreaConcLY.Value = A_LY_PER;
                            vTmpBudget.Value = BUDGET_PERIOD;
                            Cached<ConcWritePrintWork>().Run();
                            
                            // Generate SEASON print work record.
                            vTmpRecTyp.Value = "S";
                            vTmpBrnConcTY.Value = BRN_TY_SEAS;
                            vTmpBrnConcLY.Value = BRN_LY_SEAS;
                            vTmpAreaConcTY.Value = A_TY_SEAS;
                            vTmpAreaConcLY.Value = A_LY_SEAS;
                            vTmpBudget.Value = BUDGET_SEASON;
                            Cached<ConcWritePrintWork>().Run();
                            
                            // Generate YEAR print work record.
                            vTmpRecTyp.Value = "Y";
                            vTmpBrnConcTY.Value = BRN_TY_YR;
                            vTmpBrnConcLY.Value = BRN_LY_YR;
                            vTmpAreaConcTY.Value = A_TY_YR;
                            vTmpAreaConcLY.Value = A_LY_YR;
                            vTmpBudget.Value = BUDGET_YEAR;
                            Cached<ConcWritePrintWork>().Run();
                        }
                        
                        
                        /// <summary>Conc Write Print Work(P#62.1.3.4.6.1)</summary>
                        /// <remark>Last change before Migration: 08/11/2006 10:47:07</remark>
                        class ConcWritePrintWork : SalesAndStock.BusinessProcessBase 
                        {
                            
                            #region Models
                            /// <summary>ss252 Print Work</summary>
                            readonly Models.ss252PrintWork ss252PrintWork = new Models.ss252PrintWork { KeepCacheAliveAfterExit = true, AllowRowLocking = true };
                            #endregion
                            
                            ConcBuildPrintWork _parent;
                            
                            public ConcWritePrintWork(ConcBuildPrintWork parent)
                            {
                                _parent = parent;
                                Title = "Conc Write Print Work";
                                InitializeDataView();
                            }
                            void InitializeDataView()
                            {
                                
                                Relations.Add(ss252PrintWork, RelationType.InsertIfNotFound, 
                                		ss252PrintWork.BranchNumber.BindEqualTo(_parent.BRANCH_NUMBER).And(
                                		ss252PrintWork.RecordType.BindEqualTo(_parent.vTmpRecTyp)).And(
                                		ss252PrintWork.ReportGroup.BindEqualTo("J")).And(
                                		ss252PrintWork.FloorCode.BindEqualTo(9)).And(
                                		ss252PrintWork.ReportDepartment.BindEqualTo(_parent.REPORT_DEPARTMENT)).And(
                                		ss252PrintWork.ReportSubDepartment.BindEqualTo(_parent.REPORT_SUBDEPARTMENT)), 
                                	ss252PrintWork.SortByss252_Print_Work_x1);
                                
                                
                                
                                #region Columns
                                
                                // This task will generate Print Work table records for Concessions (Rept Grp 'J').
                                
                                // Floor Code is set to 9 to allow a 'Concession' header line to be inserted prior
                                // to any concession lines relevant to a branch (see task 'Conc Headers').
                                
                                Columns.Add(ss252PrintWork.BranchNumber);
                                Columns.Add(ss252PrintWork.RecordType);
                                Columns.Add(ss252PrintWork.ReportGroup);
                                Columns.Add(ss252PrintWork.FloorCode);
                                Columns.Add(ss252PrintWork.ReportDepartment);
                                Columns.Add(ss252PrintWork.ReportSubDepartment);
                                Columns.Add(ss252PrintWork.Description);
                                Columns.Add(ss252PrintWork.ThisYear);
                                Columns.Add(ss252PrintWork.Budget);
                                Columns.Add(ss252PrintWork.LastYear);
                                Columns.Add(ss252PrintWork.PCentToBudget);
                                Columns.Add(ss252PrintWork.PCentToLY);
                                Columns.Add(ss252PrintWork.AreaPCentToLY);
                                Columns.Add(ss252PrintWork.PCentVarToArea);
                                #endregion
                            }
                            /// <summary>Conc Write Print Work(P#62.1.3.4.6.1)</summary>
                            internal void Run()
                            {
                                Execute();
                            }
                            protected override void OnLoad()
                            {
                                Exit(ExitTiming.AfterRow);
                                KeepChildRelationCacheAlive = true;
                                AllowUserAbort = true;
                            }
                            protected override void OnEnterRow()
                            {
                                ss252PrintWork.Budget.Value = 0;
                                ss252PrintWork.PCentToBudget.Value = 0;
                                ss252PrintWork.PCentToLY.Value = 0;
                                ss252PrintWork.AreaPCentToLY.Value = 0;
                                ss252PrintWork.PCentVarToArea.Value = 0;
                            }
                            protected override void OnLeaveRow()
                            {
                                // Init Concession details
                                ss252PrintWork.Description.Value = _parent.CONC_NAME;
                                ss252PrintWork.ThisYear.Value = _parent.vTmpBrnConcTY;
                                ss252PrintWork.Budget.Value = _parent.vTmpBudget;
                                ss252PrintWork.LastYear.Value = _parent.vTmpBrnConcLY;
                                if(_parent.vTmpBudget != 0)
                                {
                                    ss252PrintWork.PCentToBudget.Value = u.If(u.Abs((_parent.vTmpBrnConcTY - _parent.vTmpBudget) * 100 / _parent.vTmpBudget) > 999.99, 999.99, u.Round((_parent.vTmpBrnConcTY - _parent.vTmpBudget) * 100 / _parent.vTmpBudget, 3, 2));
                                }
                                if(_parent.vTmpBrnConcLY != 0)
                                {
                                    ss252PrintWork.PCentToLY.Value = u.If(u.Abs((_parent.vTmpBrnConcTY - _parent.vTmpBrnConcLY) * 100 / _parent.vTmpBrnConcLY) > 999.99, 999.99, u.Round((_parent.vTmpBrnConcTY - _parent.vTmpBrnConcLY) * 100 / _parent.vTmpBrnConcLY, 3, 2));
                                }
                                if(_parent.vTmpAreaConcLY != 0)
                                {
                                    ss252PrintWork.AreaPCentToLY.Value = u.If(u.Abs((_parent.vTmpAreaConcTY - _parent.vTmpAreaConcLY) * 100 / _parent.vTmpAreaConcLY) > 999.99, 999.99, u.Round((_parent.vTmpAreaConcTY - _parent.vTmpAreaConcLY) * 100 / _parent.vTmpAreaConcLY, 3, 2));
                                }
                                ss252PrintWork.PCentVarToArea.Value = u.If(ss252PrintWork.PCentToLY == 999.99 || ss252PrintWork.AreaPCentToLY == 999.99, 0, u.If(u.Abs(ss252PrintWork.PCentToLY - ss252PrintWork.AreaPCentToLY) > 999.99, 999.99, ss252PrintWork.PCentToLY - ss252PrintWork.AreaPCentToLY));
                            }
                            
                            
                        }
                    }
                }
                /// <summary>Sold Units(P#62.1.3.5)</summary>
                /// <remark>Last change before Migration: 08/11/2006 10:47:37</remark>
                internal class SoldUnits : SalesAndStock.BusinessProcessBase 
                {
                    GetBranchData _parent;
                    
                    public SoldUnits(GetBranchData parent)
                    {
                        _parent = parent;
                        Title = "Sold Units";
                        InitializeDataView();
                    }
                    void InitializeDataView()
                    {
                        
                        
                    }
                    /// <summary>Sold Units(P#62.1.3.5)</summary>
                    internal void Run()
                    {
                        Execute();
                    }
                    protected override void OnLoad()
                    {
                        Exit(ExitTiming.AfterRow);
                        AllowUserAbort = true;
                        if(NewViewRequired)
                        {
                            View = ()=> new Views.DiarySlsReptSs252SoldUnits(this);
                        }
                    }
                    protected override void OnLeaveRow()
                    {
                        // Accumulate BRANCH totals of Sold Units.
                        _parent._parent.vMsgToUser.Value = "Assembling Sold Units by Branch";
                        _parent.vSQLTask.Value = "SU Store Tots";
                        Cached<SUStoreTots>().Run();
                        // Accumulate the AREA totals of Sold Units (COMP stores only)
                        if(_parent.vSQLRanOK)
                        {
                            _parent._parent.vMsgToUser.Value = "Assembling Sold Units by Area";
                            _parent.vSQLTask.Value = "SU Area Tots";
                            Cached<SUAreaTots>().Run();
                        }
                        // Move Sold Units details to Print Work table
                        if(_parent.vSQLRanOK)
                        {
                            _parent._parent.vMsgToUser.Value = "Moving Avg. Unit Sales to Print Work";
                            _parent.vSQLTask.Value = "SU Build Print Work";
                            Cached<SUBuildPrintWork>().Run();
                        }
                    }
                    
                    
                    /// <summary>SU Store Tots(P#62.1.3.5.1)</summary>
                    /// <remark>Last change before Migration: 08/11/2006 10:47:17</remark>
                    class SUStoreTots : SalesAndStock.BusinessProcessBase 
                    {
                        
                        #region Models
                        DynamicSQLEntity sqlEntity;
                        #endregion
                        
                        SoldUnits _parent;
                        
                        public SUStoreTots(SoldUnits parent)
                        {
                            _parent = parent;
                            Title = "SU Store Tots";
                            InitializeDataView();
                        }
                        void InitializeDataView()
                        {
                            sqlEntity = new DynamicSQLEntity(Models.DataSources.SS, 
@"--  Called by task 'SU Store Tots' in MAGIC program ss252
--
insert into ss252_branch_mk2

select q1.area_code, q1.branch_number, 'H', 0, ' ', ' ',
       round(nvl(q2.TY_week,0)) TY_wk, round(nvl(q3.TY_period,0)) TY_per,
          round(nvl(q8.TY_season,0)) TY_seas, round(nvl(q4.TY_year,0)) TY_yr,
       0,0,0,0,
       round(nvl(q5.LY_week,0)) LY_wk, round(nvl(q6.LY_period,0)) LY_per,
          round(nvl(q9.LY_season,0)) LY_seas, round(nvl(q7.LY_year,0)) LY_yr,
       0, q1.comp_store 
from

   (select w1.branch_number, w1.area_code, w1.comp_store
    from mackays.ss_diary_work1 w1
    where w1.prog_no = 'ss252')                                                   q1,
    
   (select s.branch_number, sum(s.total_units) TY_Week
     from mackays.ss_pos_trn_wk_summ s
    where s.week_no = :1
      and ((s.trans_type = 1 and s.trans_sub_type in (' ', 'S', 'E'))
         OR (s.trans_type = 6 and s.trans_sub_type in ('1', 'S', 'E')))
    group by s.branch_number)                                                     q2,
    
   (select s.branch_number, sum(s.total_units) TY_Period
     from mackays.ss_pos_trn_wk_summ s
    where s.week_no between :2 and :1
      and ((s.trans_type = 1 and s.trans_sub_type in (' ', 'S', 'E'))
         OR (s.trans_type = 6 and s.trans_sub_type in ('1', 'S', 'E')))
    group by s.branch_number)                                                     q3,
    
   (select s.branch_number, sum(s.total_units) TY_Year
     from mackays.ss_pos_trn_wk_summ s
    where s.week_no between :3 and :1
      and ((s.trans_type = 1 and s.trans_sub_type in (' ', 'S', 'E'))
         OR (s.trans_type = 6 and s.trans_sub_type in ('1', 'S', 'E')))
    group by s.branch_number)                                                     q4,
    
   (select s.branch_number, sum(s.total_units) LY_Week
     from mackays.ss_pos_trn_wk_summ s
    where s.week_no = :4
      and ((s.trans_type = 1 and s.trans_sub_type in (' ', 'S', 'E'))
         OR (s.trans_type = 6 and s.trans_sub_type in ('1', 'S', 'E')))
    group by s.branch_number)                                                     q5,
    
   (select s.branch_number, sum(s.total_units) LY_Period
     from mackays.ss_pos_trn_wk_summ s
    where s.week_no between :5 and :4
      and ((s.trans_type = 1 and s.trans_sub_type in (' ', 'S', 'E'))
         OR (s.trans_type = 6 and s.trans_sub_type in ('1', 'S', 'E')))
    group by s.branch_number)                                                     q6,
    
   (select s.branch_number, sum(s.total_units) LY_Year
     from mackays.ss_pos_trn_wk_summ s
    where s.week_no between :6 and :4
      and ((s.trans_type = 1 and s.trans_sub_type in (' ', 'S', 'E'))
         OR (s.trans_type = 6 and s.trans_sub_type in ('1', 'S', 'E')))
    group by s.branch_number)                                                     q7,
    
   (select s.branch_number, sum(s.total_units) TY_Season
     from mackays.ss_pos_trn_wk_summ s
    where s.week_no between :7 and :1
      and ((s.trans_type = 1 and s.trans_sub_type in (' ', 'S', 'E'))
         OR (s.trans_type = 6 and s.trans_sub_type in ('1', 'S', 'E')))
    group by s.branch_number)                                                     q8,    
    
   (select s.branch_number, sum(s.total_units) LY_Season
     from mackays.ss_pos_trn_wk_summ s
    where s.week_no between :8 and :4
      and ((s.trans_type = 1 and s.trans_sub_type in (' ', 'S', 'E'))
         OR (s.trans_type = 6 and s.trans_sub_type in ('1', 'S', 'E')))
    group by s.branch_number)                                                     q9     
where q1.branch_number = q2.branch_number (+)
  and q1.branch_number = q3.branch_number (+)
  and q1.branch_number = q4.branch_number (+)
  and q1.branch_number = q5.branch_number (+)
  and q1.branch_number = q6.branch_number (+)
  and q1.branch_number = q7.branch_number (+)
  and q1.branch_number = q8.branch_number (+)
  and q1.branch_number = q9.branch_number (+)");
                            sqlEntity.AddParameter(_parent._parent._parent._parent.vTYreqWeek); //:1;
                            sqlEntity.AddParameter(_parent._parent._parent._parent.vTYperiodStartWeek); //:2;
                            sqlEntity.AddParameter(_parent._parent._parent._parent.vTYyearStartWeek); //:3;
                            sqlEntity.AddParameter(_parent._parent._parent.vPYreqWeek); //:4;
                            sqlEntity.AddParameter(_parent._parent._parent.vPYperiodStartWeek); //:5;
                            sqlEntity.AddParameter(_parent._parent._parent.vPYyearStartWeek); //:6;
                            sqlEntity.AddParameter(_parent._parent._parent._parent.vTYseasStartWeek); //:7;
                            sqlEntity.AddParameter(_parent._parent._parent.vPYseasStartWeek); //:8;
                            sqlEntity.SuccessColumn = _parent._parent.vSQLRanOK;
                            From = sqlEntity;
                            
                            
                            #region Columns
                            
                            // This SQL task assembles Sold Units by Branch & writes them to
                            // table ss252_Branch Work with Report Group set to 'H'.
                            
                            // Resulting figures will be for This Year and Last Year.
                            #endregion
                        }
                        /// <summary>SU Store Tots(P#62.1.3.5.1)</summary>
                        internal void Run()
                        {
                            Execute();
                        }
                        protected override void OnLoad()
                        {
                            Exit(ExitTiming.AfterRow);
                            TransactionScope = TransactionScopes.Task;
                            Activity = Activities.Browse;
                            AllowDelete = false;
                            AllowInsert = false;
                            AllowUpdate = false;
                            AllowUserAbort = true;
                        }
                        
                        
                    }
                    /// <summary>SU Area Tots(P#62.1.3.5.2)</summary>
                    /// <remark>Last change before Migration: 08/11/2006 10:47:22</remark>
                    class SUAreaTots : SalesAndStock.BusinessProcessBase 
                    {
                        
                        #region Models
                        DynamicSQLEntity sqlEntity;
                        #endregion
                        
                        SoldUnits _parent;
                        
                        public SUAreaTots(SoldUnits parent)
                        {
                            _parent = parent;
                            Title = "SU Area Tots";
                            InitializeDataView();
                        }
                        void InitializeDataView()
                        {
                            sqlEntity = new DynamicSQLEntity(Models.DataSources.SS, 
@"--  Called by task 'SU Area Tots' in MAGIC program ss252
--
insert into ss252_area_mk2
  select w.area_code, w.report_group, w.report_department, w.report_subdepartment,
         nvl(sum(w.ty_week),0) ty_week, nvl(sum(w.ty_period),0) ty_period,
         nvl(sum(w.ty_season),0) ty_season, nvl(sum(w.ty_year),0) ty_year,
         nvl(sum(w.ly_week),0) ly_week, nvl(sum(w.ly_period),0) ly_period,
         nvl(sum(w.ly_season),0) ly_season, nvl(sum(w.ly_year),0) ly_year, 0
  from ss252_branch_mk2 w
  where w.comp_store = 'Y'
    and w.report_group = 'H'
  group by w.area_code, w.report_group, w.report_department, w.report_subdepartment");
                            sqlEntity.SuccessColumn = _parent._parent.vSQLRanOK;
                            From = sqlEntity;
                            
                            
                            #region Columns
                            
                            // This SQL task assembles the Sold Units by Area by rolling up
                            // table ss252_Branch Work with Report Group set to 'H'.
                            
                            // Resulting figures will be for This Year and Last Year.
                            #endregion
                        }
                        /// <summary>SU Area Tots(P#62.1.3.5.2)</summary>
                        internal void Run()
                        {
                            Execute();
                        }
                        protected override void OnLoad()
                        {
                            Exit(ExitTiming.AfterRow);
                            TransactionScope = TransactionScopes.Task;
                            Activity = Activities.Browse;
                            AllowDelete = false;
                            AllowInsert = false;
                            AllowUpdate = false;
                            AllowUserAbort = true;
                        }
                        
                        
                    }
                    /// <summary>SU Build print Work(P#62.1.3.5.3)</summary>
                    /// <remark>Last change before Migration: 08/11/2006 10:47:37</remark>
                    class SUBuildPrintWork : SalesAndStock.BusinessProcessBase 
                    {
                        
                        #region Models
                        DynamicSQLEntity sqlEntity;
                        #endregion
                        
                        #region Columns
                        /// <summary>AREA</summary>
                        readonly TextColumn AREA = new TextColumn("AREA", "2")
                        {
                        	AllowNull = true
                        };
                        /// <summary>BRN</summary>
                        readonly NumberColumn BRN = new NumberColumn("BRN", "N4")
                        {
                        	AllowNull = true
                        };
                        /// <summary>RGRP</summary>
                        readonly TextColumn RGRP = new TextColumn("RGRP", "1")
                        {
                        	AllowNull = true
                        };
                        /// <summary>FLR</summary>
                        readonly NumberColumn FLR = new NumberColumn("FLR", "N1")
                        {
                        	AllowNull = true
                        };
                        /// <summary>RDEPT</summary>
                        readonly TextColumn RDEPT = new TextColumn("RDEPT", "2")
                        {
                        	AllowNull = true
                        };
                        /// <summary>RSUBD</summary>
                        readonly TextColumn RSUBD = new TextColumn("RSUBD", "2")
                        {
                        	AllowNull = true
                        };
                        /// <summary>BRN_TY_WEEK</summary>
                        readonly NumberColumn BRN_TY_WEEK = new NumberColumn("BRN_TY_WEEK", "N8")
                        {
                        	AllowNull = true
                        };
                        /// <summary>BRN_TY_PER</summary>
                        readonly NumberColumn BRN_TY_PER = new NumberColumn("BRN_TY_PER", "N8")
                        {
                        	AllowNull = true
                        };
                        /// <summary>BRN_TY_SEAS</summary>
                        readonly NumberColumn BRN_TY_SEAS = new NumberColumn("BRN_TY_SEAS", "N8")
                        {
                        	AllowNull = true
                        };
                        /// <summary>BRN_TY_YR</summary>
                        readonly NumberColumn BRN_TY_YR = new NumberColumn("BRN_TY_YR", "N8")
                        {
                        	AllowNull = true
                        };
                        /// <summary>BRN_LY_WEEK</summary>
                        readonly NumberColumn BRN_LY_WEEK = new NumberColumn("BRN_LY_WEEK", "N8")
                        {
                        	AllowNull = true
                        };
                        /// <summary>BRN_LY_PER</summary>
                        readonly NumberColumn BRN_LY_PER = new NumberColumn("BRN_LY_PER", "N8")
                        {
                        	AllowNull = true
                        };
                        /// <summary>BRN_LY_SEAS</summary>
                        readonly NumberColumn BRN_LY_SEAS = new NumberColumn("BRN_LY_SEAS", "N8")
                        {
                        	AllowNull = true
                        };
                        /// <summary>BRN_LY_YR</summary>
                        readonly NumberColumn BRN_LY_YR = new NumberColumn("BRN_LY_YR", "N8")
                        {
                        	AllowNull = true
                        };
                        /// <summary>A_TY_WEEK</summary>
                        readonly NumberColumn A_TY_WEEK = new NumberColumn("A_TY_WEEK", "N9.2")
                        {
                        	AllowNull = true
                        };
                        /// <summary>A_TY_PER</summary>
                        readonly NumberColumn A_TY_PER = new NumberColumn("A_TY_PER", "N9.2")
                        {
                        	AllowNull = true
                        };
                        /// <summary>A_TY_SEAS</summary>
                        readonly NumberColumn A_TY_SEAS = new NumberColumn("A_TY_SEAS", "N9.2")
                        {
                        	AllowNull = true
                        };
                        /// <summary>A_TY_YR</summary>
                        readonly NumberColumn A_TY_YR = new NumberColumn("A_TY_YR", "N9.2")
                        {
                        	AllowNull = true
                        };
                        /// <summary>A_LY_WEEK</summary>
                        readonly NumberColumn A_LY_WEEK = new NumberColumn("A_LY_WEEK", "N9.2")
                        {
                        	AllowNull = true
                        };
                        /// <summary>A_LY_PER</summary>
                        readonly NumberColumn A_LY_PER = new NumberColumn("A_LY_PER", "N9.2")
                        {
                        	AllowNull = true
                        };
                        /// <summary>A_LY_SEAS</summary>
                        readonly NumberColumn A_LY_SEAS = new NumberColumn("A_LY_SEAS", "N9.2")
                        {
                        	AllowNull = true
                        };
                        /// <summary>A_LY_YR</summary>
                        readonly NumberColumn A_LY_YR = new NumberColumn("A_LY_YR", "N9.2")
                        {
                        	AllowNull = true
                        };
                        /// <summary>STORE_TY_WEEK</summary>
                        readonly NumberColumn STORE_TY_WEEK = new NumberColumn("STORE_TY_WEEK", "10.3")
                        {
                        	AllowNull = true
                        };
                        /// <summary>STORE_TY_PER</summary>
                        readonly NumberColumn STORE_TY_PER = new NumberColumn("STORE_TY_PER", "10.3")
                        {
                        	AllowNull = true
                        };
                        /// <summary>STORE_TY_SEAS</summary>
                        readonly NumberColumn STORE_TY_SEAS = new NumberColumn("STORE_TY_SEAS", "10.3")
                        {
                        	AllowNull = true
                        };
                        /// <summary>STORE_TY_YEAR</summary>
                        readonly NumberColumn STORE_TY_YEAR = new NumberColumn("STORE_TY_YEAR", "10.3")
                        {
                        	AllowNull = true
                        };
                        /// <summary>STORE_LY_WEEK</summary>
                        readonly NumberColumn STORE_LY_WEEK = new NumberColumn("STORE_LY_WEEK", "10.3")
                        {
                        	AllowNull = true
                        };
                        /// <summary>STORE_LY_PER</summary>
                        readonly NumberColumn STORE_LY_PER = new NumberColumn("STORE_LY_PER", "10.3")
                        {
                        	AllowNull = true
                        };
                        /// <summary>STORE_LY_SEAS</summary>
                        readonly NumberColumn STORE_LY_SEAS = new NumberColumn("STORE_LY_SEAS", "10.3")
                        {
                        	AllowNull = true
                        };
                        /// <summary>STORE_LY_YEAR</summary>
                        readonly NumberColumn STORE_LY_YEAR = new NumberColumn("STORE_LY_YEAR", "10.3")
                        {
                        	AllowNull = true
                        };
                        /// <summary>AREA_TY_WEEK</summary>
                        readonly NumberColumn AREA_TY_WEEK = new NumberColumn("AREA_TY_WEEK", "10.3")
                        {
                        	AllowNull = true
                        };
                        /// <summary>AREA_TY_PER</summary>
                        readonly NumberColumn AREA_TY_PER = new NumberColumn("AREA_TY_PER", "10.3")
                        {
                        	AllowNull = true
                        };
                        /// <summary>AREA_TY_SEAS</summary>
                        readonly NumberColumn AREA_TY_SEAS = new NumberColumn("AREA_TY_SEAS", "10.3")
                        {
                        	AllowNull = true
                        };
                        /// <summary>AREA_TY_YR</summary>
                        readonly NumberColumn AREA_TY_YR = new NumberColumn("AREA_TY_YR", "10.3")
                        {
                        	AllowNull = true
                        };
                        /// <summary>AREA_LY_WEEK</summary>
                        readonly NumberColumn AREA_LY_WEEK = new NumberColumn("AREA_LY_WEEK", "10.3")
                        {
                        	AllowNull = true
                        };
                        /// <summary>AREA_LY_PER</summary>
                        readonly NumberColumn AREA_LY_PER = new NumberColumn("AREA_LY_PER", "10.3")
                        {
                        	AllowNull = true
                        };
                        /// <summary>AREA_LY_SEAS</summary>
                        readonly NumberColumn AREA_LY_SEAS = new NumberColumn("AREA_LY_SEAS", "10.3")
                        {
                        	AllowNull = true
                        };
                        /// <summary>AREA_LY_YR</summary>
                        readonly NumberColumn AREA_LY_YR = new NumberColumn("AREA_LY_YR", "10.3")
                        {
                        	AllowNull = true
                        };
                        /// <summary>v:Tmp Rec Typ</summary>
                        readonly TextColumn vTmpRecTyp = new TextColumn("v:Tmp Rec Typ", "U");
                        /// <summary>v:Tmp Brn SU TY</summary>
                        readonly NumberColumn vTmpBrnSUTY = new NumberColumn("v:Tmp Brn SU TY", "N7");
                        /// <summary>v:Tmp Brn SU LY</summary>
                        readonly NumberColumn vTmpBrnSULY = new NumberColumn("v:Tmp Brn SU LY", "N7");
                        /// <summary>v:Tmp Area SU TY</summary>
                        readonly NumberColumn vTmpAreaSUTY = new NumberColumn("v:Tmp Area SU TY", "N7");
                        /// <summary>v:Tmp Area SU LY</summary>
                        readonly NumberColumn vTmpAreaSULY = new NumberColumn("v:Tmp Area SU LY", "N7");
                        /// <summary>v:Tmp Brn Sales TY</summary>
                        readonly NumberColumn vTmpBrnSalesTY = new NumberColumn("v:Tmp Brn Sales TY", "N7");
                        /// <summary>v:Tmp Brn Sales LY</summary>
                        readonly NumberColumn vTmpBrnSalesLY = new NumberColumn("v:Tmp Brn Sales LY", "N7");
                        /// <summary>v:Tmp Area Sales TY</summary>
                        readonly NumberColumn vTmpAreaSalesTY = new NumberColumn("v:Tmp Area Sales TY", "N7");
                        /// <summary>v:Tmp Area Sales LY</summary>
                        readonly NumberColumn vTmpAreaSalesLY = new NumberColumn("v:Tmp Area Sales LY", "N7");
                        #endregion
                        
                        public SUBuildPrintWork()
                        {
                            Title = "SU Build print Work";
                            InitializeDataView();
                        }
                        void InitializeDataView()
                        {
                            sqlEntity = new DynamicSQLEntity(Models.DataSources.SS, 
@"--  Called by task 'SU Build Print Work' in MAGIC program ss252
--
select q1.area_code area, q1.branch_number brn, q1.report_group rgrp,
       q1.floor_code flr, q1.report_department rdept, q1.report_subdepartment rsubd,
       q1.ty_week brn_ty_week, q1.ty_period brn_ty_per, q1.ty_season brn_ty_seas, q1.ty_year brn_ty_yr,
       q1.ly_week brn_ly_week, q1.ly_period brn_ly_per, q1.ly_season brn_ly_seas, q1.ly_year brn_ly_yr,
       q2.ty_week a_ty_week, q2.ty_period a_ty_per, q2.ty_season a_ty_seas, q2.ty_year a_ty_yr,
       q2.ly_week a_ly_week, q2.ly_period a_ly_per, q2.ly_season a_ly_seas, q2.ly_year a_ly_yr,
       nvl(q3.This_Year,0) store_ty_week, nvl(q4.This_Year,0) store_ty_per,
          nvl(q5.This_Year,0) store_ty_seas, nvl(q6.This_Year,0) store_ty_year, 
       nvl(q3.Last_Year,0) store_ly_week, nvl(q4.Last_Year,0) store_ly_per,
          nvl(q5.Last_Year,0) store_ly_seas, nvl(q6.Last_Year,0) store_ly_year,
       q7.ty_week area_ty_week, q7.ty_period area_ty_per, q7.ty_season area_ty_seas, q7.ty_year area_ty_yr,
       q7.ly_week area_ly_week, q7.ly_period area_ly_per, q7.ly_season area_ly_seas, q7.ly_year area_ly_yr
       
from

/*                         Select Sold Units by Store                              */
   (select b.area_code, b.branch_number, b.report_group,
           b.floor_code, b.report_department, b.report_subdepartment,
           b.ty_week, b.ty_period, b.ty_season, b.ty_year,
           b.ly_week, b.ly_period, b.ly_season, b.ly_year
    from ss252_branch_mk2 b
    where b.report_group = 'H')                                      q1,

/*                     Select corresponding Area Sold Units                        */
   (select a.area_code, a.report_group, a.report_department, a.report_subdepartment,
           a.ty_week, a.ty_period, a.ty_season, a.ty_year,
           a.ly_week, a.ly_period, a.ly_season, a.ly_year
    from ss252_area_mk2 a
    where a.report_group = 'H')                                      q2,

/*                      Select total Store sales for Week                               */
   (select p.branch_number, p.This_Year, p.Last_Year
    from ss252_print_work p
    where p.record_type = 'W'
      and p.report_group = 'A'
      and p.Floor_Code = 9
      and p.report_department = 'ZZ'
      and p.report_subdepartment = 'ZA')                             q3,

/*                      Select total Store sales for Period                             */
   (select p.branch_number, p.This_Year, p.Last_Year
    from ss252_print_work p
    where p.record_type = 'P'
      and p.report_group = 'A'
      and p.Floor_Code = 9
      and p.report_department = 'ZZ'
      and p.report_subdepartment = 'ZA')                             q4,

/*                      Select total Store sales for Season                             */
   (select p.branch_number, p.This_Year, p.Last_Year
    from ss252_print_work p
    where p.record_type = 'S'
      and p.report_group = 'A'
      and p.Floor_Code = 9
      and p.report_department = 'ZZ'
      and p.report_subdepartment = 'ZA')                             q5,      

/*                      Select total Store sales for Year                               */
   (select p.branch_number, p.This_Year, p.Last_Year
    from ss252_print_work p
    where p.record_type = 'Y'
      and p.report_group = 'A'
      and p.Floor_Code = 9
      and p.report_department = 'ZZ'
      and p.report_subdepartment = 'ZA')                             q6,

/*                     Select Area Sales for Week, Period, Year                         */
   (select a.area_code,
           sum(a.ty_week) ty_week, sum(a.ty_period) ty_period, sum(a.ty_season) ty_season, sum(a.ty_year) ty_year,
           sum(a.ly_week) ly_week, sum(a.ly_period) ly_period, sum(a.ly_season) ly_season, sum(a.ly_year) ly_year
    from ss252_area_mk2 a
    where a.report_group = 'A'
    group by a.area_code)                                            q7
      

where q1.area_code = q2.area_code
  and q1.report_group = q2.report_group
  and q1.report_department = q2.report_department
  and q1.report_subdepartment = q2.report_subdepartment
  and q1.branch_number = q3.branch_number (+)
  and q1.branch_number = q4.branch_number (+)
  and q1.branch_number = q5.branch_number (+)
  and q1.branch_number = q6.branch_number (+)
  and q1.area_code = q7.area_code (+)");
                            sqlEntity.Columns.Add(AREA, BRN, RGRP, FLR, RDEPT, RSUBD, BRN_TY_WEEK, BRN_TY_PER, BRN_TY_SEAS, BRN_TY_YR, BRN_LY_WEEK, BRN_LY_PER, BRN_LY_SEAS, BRN_LY_YR, A_TY_WEEK, A_TY_PER, A_TY_SEAS, A_TY_YR, A_LY_WEEK, A_LY_PER, A_LY_SEAS, A_LY_YR, STORE_TY_WEEK, STORE_TY_PER, STORE_TY_SEAS, STORE_TY_YEAR, STORE_LY_WEEK, STORE_LY_PER, STORE_LY_SEAS, STORE_LY_YEAR, AREA_TY_WEEK, AREA_TY_PER, AREA_TY_SEAS, AREA_TY_YR, AREA_LY_WEEK, AREA_LY_PER, AREA_LY_SEAS, AREA_LY_YR);
                            From = sqlEntity;
                            
                            
                            #region Columns
                            
                            // SQL task to select both Branch & Area totals of Sold Units and Total Sales for
                            // each of the report levels required.
                            // Figures for This Year and Last Year will be returned.
                            
                            Columns.Add(AREA);
                            Columns.Add(BRN);
                            Columns.Add(RGRP);
                            Columns.Add(FLR);
                            Columns.Add(RDEPT);
                            Columns.Add(RSUBD);
                            // Sold Units by Branch
                            Columns.Add(BRN_TY_WEEK);
                            Columns.Add(BRN_TY_PER);
                            Columns.Add(BRN_TY_SEAS);
                            Columns.Add(BRN_TY_YR);
                            Columns.Add(BRN_LY_WEEK);
                            Columns.Add(BRN_LY_PER);
                            Columns.Add(BRN_LY_SEAS);
                            Columns.Add(BRN_LY_YR);
                            // Sold Units by Area
                            Columns.Add(A_TY_WEEK);
                            Columns.Add(A_TY_PER);
                            Columns.Add(A_TY_SEAS);
                            Columns.Add(A_TY_YR);
                            Columns.Add(A_LY_WEEK);
                            Columns.Add(A_LY_PER);
                            Columns.Add(A_LY_SEAS);
                            Columns.Add(A_LY_YR);
                            // Sales Totals by Branch
                            Columns.Add(STORE_TY_WEEK);
                            Columns.Add(STORE_TY_PER);
                            Columns.Add(STORE_TY_SEAS);
                            Columns.Add(STORE_TY_YEAR);
                            Columns.Add(STORE_LY_WEEK);
                            Columns.Add(STORE_LY_PER);
                            Columns.Add(STORE_LY_SEAS);
                            Columns.Add(STORE_LY_YEAR);
                            // Sales Totals by Area
                            Columns.Add(AREA_TY_WEEK);
                            Columns.Add(AREA_TY_PER);
                            Columns.Add(AREA_TY_SEAS);
                            Columns.Add(AREA_TY_YR);
                            Columns.Add(AREA_LY_WEEK);
                            Columns.Add(AREA_LY_PER);
                            Columns.Add(AREA_LY_SEAS);
                            Columns.Add(AREA_LY_YR);
                            
                            // Temporary fields for building Print Work records
                            Columns.Add(vTmpRecTyp);
                            Columns.Add(vTmpBrnSUTY);
                            Columns.Add(vTmpBrnSULY);
                            Columns.Add(vTmpAreaSUTY);
                            Columns.Add(vTmpAreaSULY);
                            Columns.Add(vTmpBrnSalesTY);
                            Columns.Add(vTmpBrnSalesLY);
                            Columns.Add(vTmpAreaSalesTY);
                            Columns.Add(vTmpAreaSalesLY);
                            #endregion
                        }
                        /// <summary>SU Build print Work(P#62.1.3.5.3)</summary>
                        internal void Run()
                        {
                            Execute();
                        }
                        protected override void OnLoad()
                        {
                            TransactionScope = TransactionScopes.Task;
                            AllowDelete = false;
                            AllowInsert = false;
                            AllowUpdate = false;
                            AllowUserAbort = true;
                        }
                        protected override void OnLeaveRow()
                        {
                            // Generate WEEK print work.
                            vTmpRecTyp.Value = "W";
                            vTmpBrnSUTY.Value = BRN_TY_WEEK;
                            vTmpBrnSULY.Value = BRN_LY_WEEK;
                            vTmpAreaSUTY.Value = A_TY_WEEK;
                            vTmpAreaSULY.Value = A_LY_WEEK;
                            vTmpBrnSalesTY.Value = STORE_TY_WEEK;
                            vTmpBrnSalesLY.Value = STORE_LY_WEEK;
                            vTmpAreaSalesTY.Value = AREA_TY_WEEK;
                            vTmpAreaSalesLY.Value = AREA_LY_WEEK;
                            Cached<SUWritePrintWork>().Run();
                            
                            // Generate PERIOD print work.
                            vTmpRecTyp.Value = "P";
                            vTmpBrnSUTY.Value = BRN_TY_PER;
                            vTmpBrnSULY.Value = BRN_LY_PER;
                            vTmpAreaSUTY.Value = A_TY_PER;
                            vTmpAreaSULY.Value = A_LY_PER;
                            vTmpBrnSalesTY.Value = STORE_TY_PER;
                            vTmpBrnSalesLY.Value = STORE_LY_PER;
                            vTmpAreaSalesTY.Value = AREA_TY_PER;
                            vTmpAreaSalesLY.Value = AREA_LY_PER;
                            Cached<SUWritePrintWork>().Run();
                            
                            // Generate SEASON print work.
                            vTmpRecTyp.Value = "S";
                            vTmpBrnSUTY.Value = BRN_TY_SEAS;
                            vTmpBrnSULY.Value = BRN_LY_SEAS;
                            vTmpAreaSUTY.Value = A_TY_SEAS;
                            vTmpAreaSULY.Value = A_LY_SEAS;
                            vTmpBrnSalesTY.Value = STORE_TY_SEAS;
                            vTmpBrnSalesLY.Value = STORE_LY_SEAS;
                            vTmpAreaSalesTY.Value = AREA_TY_SEAS;
                            vTmpAreaSalesLY.Value = AREA_LY_SEAS;
                            Cached<SUWritePrintWork>().Run();
                            
                            // Generate YEAR print work.
                            vTmpRecTyp.Value = "Y";
                            vTmpBrnSUTY.Value = BRN_TY_YR;
                            vTmpBrnSULY.Value = BRN_LY_YR;
                            vTmpAreaSUTY.Value = A_TY_YR;
                            vTmpAreaSULY.Value = A_LY_YR;
                            vTmpBrnSalesTY.Value = STORE_TY_YEAR;
                            vTmpBrnSalesLY.Value = STORE_LY_YEAR;
                            vTmpAreaSalesTY.Value = AREA_TY_YR;
                            vTmpAreaSalesLY.Value = AREA_LY_YR;
                            Cached<SUWritePrintWork>().Run();
                        }
                        
                        
                        /// <summary>SU Write Print Work(P#62.1.3.5.3.1)</summary>
                        /// <remark>Last change before Migration: 08/11/2006 10:47:37</remark>
                        class SUWritePrintWork : SalesAndStock.BusinessProcessBase 
                        {
                            
                            #region Models
                            /// <summary>ss252 Print Work</summary>
                            readonly Models.ss252PrintWork ss252PrintWork = new Models.ss252PrintWork { KeepCacheAliveAfterExit = true, AllowRowLocking = true };
                            #endregion
                            
                            #region Columns
                            /// <summary>v:Area Unit Sale TY</summary>
                            readonly NumberColumn vAreaUnitSaleTY = new NumberColumn("v:Area Unit Sale TY", "N8.2");
                            /// <summary>v:Area Unit Sale LY</summary>
                            readonly NumberColumn vAreaUnitSaleLY = new NumberColumn("v:Area Unit Sale LY", "N8.2");
                            #endregion
                            
                            SUBuildPrintWork _parent;
                            
                            public SUWritePrintWork(SUBuildPrintWork parent)
                            {
                                _parent = parent;
                                Title = "SU Write Print Work";
                                InitializeDataView();
                            }
                            void InitializeDataView()
                            {
                                
                                Relations.Add(ss252PrintWork, RelationType.InsertIfNotFound, 
                                		ss252PrintWork.BranchNumber.BindEqualTo(_parent.BRN).And(
                                		ss252PrintWork.RecordType.BindEqualTo(_parent.vTmpRecTyp)).And(
                                		ss252PrintWork.ReportGroup.BindEqualTo("H")).And(
                                		ss252PrintWork.FloorCode.BindEqualTo(() => 0)).And(
                                		ss252PrintWork.ReportDepartment.BindEqualTo(_parent.RDEPT)).And(
                                		ss252PrintWork.ReportSubDepartment.BindEqualTo(_parent.RSUBD)), 
                                	ss252PrintWork.SortByss252_Print_Work_x1);
                                
                                
                                
                                #region Columns
                                
                                // This task will generate Print Work table records for
                                // Average Unit Sale (Report Group 'H')
                                
                                Columns.Add(ss252PrintWork.BranchNumber);
                                Columns.Add(ss252PrintWork.RecordType);
                                Columns.Add(ss252PrintWork.ReportGroup);
                                Columns.Add(ss252PrintWork.FloorCode);
                                Columns.Add(ss252PrintWork.ReportDepartment);
                                Columns.Add(ss252PrintWork.ReportSubDepartment);
                                Columns.Add(ss252PrintWork.Description);
                                Columns.Add(ss252PrintWork.ThisYear);
                                Columns.Add(ss252PrintWork.LastYear);
                                Columns.Add(ss252PrintWork.PCentToLY);
                                Columns.Add(ss252PrintWork.AreaPCentToLY);
                                Columns.Add(ss252PrintWork.PCentVarToArea);
                                
                                Columns.Add(vAreaUnitSaleTY).BindValue(() => u.Round(_parent.vTmpAreaSalesTY / _parent.vTmpAreaSUTY, 8, 2));
                                Columns.Add(vAreaUnitSaleLY).BindValue(() => u.Round(_parent.vTmpAreaSalesLY / _parent.vTmpAreaSULY, 8, 2));
                                #endregion
                            }
                            /// <summary>SU Write Print Work(P#62.1.3.5.3.1)</summary>
                            internal void Run()
                            {
                                Execute();
                            }
                            protected override void OnLoad()
                            {
                                Exit(ExitTiming.AfterRow);
                                KeepChildRelationCacheAlive = true;
                                AllowUserAbort = true;
                            }
                            protected override void OnEnterRow()
                            {
                                ss252PrintWork.PCentToLY.Value = 0;
                                ss252PrintWork.AreaPCentToLY.Value = 0;
                                ss252PrintWork.PCentVarToArea.Value = 0;
                            }
                            protected override void OnLeaveRow()
                            {
                                // Init  ATV  values
                                ss252PrintWork.Description.Value = "Avg. Unit Sale";
                                ss252PrintWork.ThisYear.Value = u.Round(_parent.vTmpBrnSalesTY / _parent.vTmpBrnSUTY, 8, 2);
                                ss252PrintWork.LastYear.Value = u.Round(_parent.vTmpBrnSalesLY / _parent.vTmpBrnSULY, 8, 2);
                                if(ss252PrintWork.LastYear != 0)
                                {
                                    ss252PrintWork.PCentToLY.Value = u.If(u.Abs((ss252PrintWork.ThisYear - ss252PrintWork.LastYear) * 100 / ss252PrintWork.LastYear) > 999.99, 999.99, u.Round((ss252PrintWork.ThisYear - ss252PrintWork.LastYear) * 100 / ss252PrintWork.LastYear, 3, 2));
                                }
                                if(vAreaUnitSaleLY != 0)
                                {
                                    ss252PrintWork.AreaPCentToLY.Value = u.If(u.Abs((vAreaUnitSaleTY - vAreaUnitSaleLY) * 100 / vAreaUnitSaleLY) > 999.99, 999.99, u.Round((vAreaUnitSaleTY - vAreaUnitSaleLY) * 100 / vAreaUnitSaleLY, 3, 2));
                                }
                                ss252PrintWork.PCentVarToArea.Value = u.If(ss252PrintWork.PCentToLY == 999.99 || ss252PrintWork.AreaPCentToLY == 999.99, 0, u.If(u.Abs(ss252PrintWork.PCentToLY - ss252PrintWork.AreaPCentToLY) > 999.99, 999.99, ss252PrintWork.PCentToLY - ss252PrintWork.AreaPCentToLY));
                            }
                            
                            
                        }
                    }
                }
                /// <summary>Update LY(P#62.1.3.7)</summary>
                /// <remark>Last change before Migration: 08/11/2006 10:48:17</remark>
                class UpdateLY : SalesAndStock.BusinessProcessBase 
                {
                    
                    #region Models
                    DynamicSQLEntity sqlEntity;
                    /// <summary>ss252 Branch Work Mk2</summary>
                    readonly Models.ss252BranchWorkMk2 ss252BranchWorkMk2 = new Models.ss252BranchWorkMk2 { AllowRowLocking = true };
                    #endregion
                    
                    #region Columns
                    /// <summary>sql:AREA_CODE</summary>
                    readonly TextColumn sqlAREA_CODE = new TextColumn("sql:AREA_CODE", "2")
                    {
                    	AllowNull = true
                    };
                    /// <summary>sql:BRANCH_NUMBER</summary>
                    readonly NumberColumn sqlBRANCH_NUMBER = new NumberColumn("sql:BRANCH_NUMBER", "N4")
                    {
                    	AllowNull = true
                    };
                    /// <summary>sql:RPTGRP</summary>
                    readonly TextColumn sqlRPTGRP = new TextColumn("sql:RPTGRP", "1")
                    {
                    	AllowNull = true
                    };
                    /// <summary>sql:FLR</summary>
                    readonly NumberColumn sqlFLR = new NumberColumn("sql:FLR", "N1")
                    {
                    	AllowNull = true
                    };
                    /// <summary>sql:RPT_DEPT</summary>
                    readonly TextColumn sqlRPT_DEPT = new TextColumn("sql:RPT_DEPT", "2")
                    {
                    	AllowNull = true
                    };
                    /// <summary>sql:RPT_SUBD</summary>
                    readonly TextColumn sqlRPT_SUBD = new TextColumn("sql:RPT_SUBD", "2")
                    {
                    	AllowNull = true
                    };
                    /// <summary>sql:LY_WK</summary>
                    readonly NumberColumn sqlLY_WK = new NumberColumn("sql:LY_WK", "N8")
                    {
                    	AllowNull = true
                    };
                    /// <summary>sql:LY_PER</summary>
                    readonly NumberColumn sqlLY_PER = new NumberColumn("sql:LY_PER", "N8")
                    {
                    	AllowNull = true
                    };
                    /// <summary>sql:LY_SEAS</summary>
                    readonly NumberColumn sqlLY_SEAS = new NumberColumn("sql:LY_SEAS", "N8")
                    {
                    	AllowNull = true
                    };
                    /// <summary>sql:LY_YR</summary>
                    readonly NumberColumn sqlLY_YR = new NumberColumn("sql:LY_YR", "N8")
                    {
                    	AllowNull = true
                    };
                    /// <summary>sql:BAYS</summary>
                    readonly NumberColumn sqlBAYS = new NumberColumn("sql:BAYS", "N4")
                    {
                    	AllowNull = true
                    };
                    /// <summary>sql:COMP</summary>
                    readonly TextColumn sqlCOMP = new TextColumn("sql:COMP", "1")
                    {
                    	AllowNull = true
                    };
                    /// <summary>v:Work Found</summary>
                    readonly BoolColumn vWorkFound = new BoolColumn("v:Work Found");
                    #endregion
                    
                    GetBranchData _parent;
                    
                    public UpdateLY(GetBranchData parent)
                    {
                        _parent = parent;
                        Title = "Update LY";
                        InitializeDataView();
                    }
                    void InitializeDataView()
                    {
                        sqlEntity = new DynamicSQLEntity(Models.DataSources.SLA, 
@"select
    q5.area_code, q5.branch_number, q5.rptgrp, nvl(q4.floor_code,0) flr, q5.rpt_dept, q5.rpt_subd,
    round(sum(q5.LY_Wk)) LY_Wk, round(sum(q5.LY_Per)) LY_Per,
    round(sum(q5.LY_Seas)) LY_Seas, round(sum(q5.LY_Yr)) LY_Yr,
    0, q5.Comp
from

(select
    q1.area_code, q1.branch_number, q1.rptgrp,
    q1.dss_department, q1.dss_subdepartment, q1.rpt_dept, q1.rpt_subd,
    nvl(sum(q3.LY_Week),0) LY_Wk, nvl(sum(q2.LY_Period),0) LY_Per,
    nvl(sum(q6.LY_Seas),0) ly_Seas, nvl(sum(q1.LY_Year),0) LY_Yr,
    q1.Comp
  from

/*                                                                   Determine LY Year Sales to date  */
  (select distinct
      b.area_code , s.branch_number, 'A' rptgrp, s.pdm_department_code, s.pdm_section_code,
      c.dss_department, c.dss_subdepartment,
      nvl(a.group_1_dept,'X') rpt_dept, nvl(a.group_1_subdepartment,'X') rpt_subd,
      sum(s.section_value) LY_Year,
      (case when exists
         (select 'x'
            from ref_bra_atts att1
           where att1.branch_number = s.branch_number
             and att1.attribute_type = 60
             and att1.attribute_code = 'FCOMP'
             and att1.brn_attr_start_date =
                 (select max (att2.brn_attr_start_date)
                    from ref_bra_atts att2
                    where att2.attribute_type = 60
                      and att2.branch_number = s.branch_number
                      and att2.brn_attr_start_date <= ':5')
             and not exists
                (select 'y'
                   from ref_bra_atts att3
                  where att3.branch_number = att1.branch_number
                    and att3.attribute_type = 61
                    and att3.attribute_code = 'SQFTG'
                    and
                     (att3.brn_attr_end_date is null
                      or
                      ':5' between att3.brn_attr_start_date and att3.brn_attr_end_date)))
          then 'Y' else 'N' end) Comp
   from
     SLA_SectSales s, REF_Branch b, REF_SectConv c, ref_dpt_analysis_grps a
  where s.transaction_date between ':2' and ':1'
    and s.section_value <> 0
    and s.pdm_department_code = c.department_code
    and s.pdm_section_code = c.section_code
    and s.branch_number = b.branch_number
    and b.branch_status = 'O'
    and trim(c.dss_department) <> '6'
    and c.dss_department = a.department_code (+)
    and c.dss_subdepartment = a.subdepartment_code (+)
  group by
        b.area_code , s.branch_number, 'A', 9,
        s.pdm_department_code, s.pdm_section_code, c.dss_department, c.dss_subdepartment,
        nvl(a.group_1_dept,'X'), nvl(a.group_1_subdepartment,'X'),
        (case when exists
         (select 'x'
            from ref_bra_atts att1
           where att1.branch_number = s.branch_number
             and att1.attribute_type = 60
             and att1.attribute_code = 'FCOMP'
             and att1.brn_attr_start_date =
                 (select max (att2.brn_attr_start_date)
                    from ref_bra_atts att2
                    where att2.attribute_type = 60
                      and att2.branch_number = s.branch_number
                      and att2.brn_attr_start_date <= ':5')
             and not exists
                (select 'y'
                   from ref_bra_atts att3
                  where att3.branch_number = att1.branch_number
                    and att3.attribute_type = 61
                    and att3.attribute_code = 'SQFTG'
                    and
                     (att3.brn_attr_end_date is null
                      or
                      ':5' between att3.brn_attr_start_date and att3.brn_attr_end_date)))
           then 'Y' else 'N' end)
                                                                     ) q1,

/*                                                                   Determine LY Period Sales to date  */
  (select s.branch_number,s.pdm_department_code, s.pdm_section_code,
          sum(s.section_value) LY_Period
   from sla_sectsales s
   where s.transaction_date between ':3' and ':1'
   group by s.branch_number,s.pdm_department_code, s.pdm_section_code) q2,
                                                                     
/*                                                                   Determine LY Week Sales           */
  (select s.branch_number,s.pdm_department_code, s.pdm_section_code,
          sum(s.section_value) LY_Week
   from sla_sectsales s
   where s.transaction_date between ':4' and ':1'
   group by s.branch_number,s.pdm_department_code, s.pdm_section_code) q3,
                                                                     
/*                                                                   Determine LY Season Sales           */
  (select s.branch_number,s.pdm_department_code, s.pdm_section_code,
          sum(s.section_value) LY_Seas
   from sla_sectsales s
   where s.transaction_date between ':6' and ':1'
   group by s.branch_number,s.pdm_department_code, s.pdm_section_code) q6

 where q1.branch_number = q2.branch_number (+)
   and q1.pdm_department_code = q2.pdm_department_code (+)
   and q1.pdm_section_code = q2.pdm_section_code (+)
   
   and q1.branch_number = q3.branch_number (+)
   and q1.pdm_department_code = q3.pdm_department_code (+)
   and q1.pdm_section_code = q3.pdm_section_code (+)
   
   and q1.branch_number = q6.branch_number (+)
   and q1.pdm_department_code = q6.pdm_department_code (+)
   and q1.pdm_section_code = q6.pdm_section_code (+)
   
 group by
   q1.area_code, q1.branch_number, q1.rptgrp, q1.rpt_dept, q1.rpt_subd,
   q1.dss_department, q1.dss_subdepartment, q1.Comp)                                               q5,
                                                                       
/*                                                                   Determine Floor Code                   */
  (select distinct bb.branch, bb.department, bb.subdepartment, bb.floor_code
   from ref_branch_bays bb
   where bb.eff_date = (select max(bb1.eff_date)
                        from ref_branch_bays bb1
                        where bb1.branch = bb.branch
                          and bb1.department = bb.department
                          and bb1.subdepartment = bb.subdepartment
                          and bb1.eff_date <= ':5'))                                q4
    

 where q5.branch_number = q4.branch (+)
   and q5.dss_department = q4.department (+)
   and q5.dss_subdepartment = q4.subdepartment (+)
 group by q5.area_code, q5.branch_number, q5.rptgrp, nvl(q4.floor_code,0),
          q5.rpt_dept, q5.rpt_subd, q5.Comp
 order by q5.area_code, q5.branch_number, q5.rptgrp, nvl(q4.floor_code,0),
          q5.rpt_dept, q5.rpt_subd, q5.Comp");
                        sqlEntity.AddParameter(() => u.DStr(_parent._parent._parent.vPYweekEndDate, "DD-MMM-YYYY")); //:1;
                        sqlEntity.AddParameter(() => u.DStr(_parent._parent._parent.vPYyearStartDate, "DD-MMM-YYYY")); //:2;
                        sqlEntity.AddParameter(() => u.DStr(_parent._parent._parent.vPYperiodStartDate, "DD-MMM-YYYY")); //:3;
                        sqlEntity.AddParameter(() => u.DStr(_parent._parent._parent.vPYweekStartDate, "DD-MMM-YYYY")); //:4;
                        sqlEntity.AddParameter(() => u.DStr(_parent._parent._parent.vTYweekEndDate, "DD-MMM-YYYY")); //:5;
                        sqlEntity.AddParameter(() => u.DStr(_parent._parent._parent.vPYseasonStartDate, "DD-MMM-YYYY")); //:6;
                        sqlEntity.Columns.Add(sqlAREA_CODE, sqlBRANCH_NUMBER, sqlRPTGRP, sqlFLR, sqlRPT_DEPT, sqlRPT_SUBD, sqlLY_WK, sqlLY_PER, sqlLY_SEAS, sqlLY_YR, sqlBAYS, sqlCOMP);
                        From = sqlEntity;
                        
                        Relations.Add(ss252BranchWorkMk2, RelationType.InsertIfNotFound, 
                        		ss252BranchWorkMk2.AreaCode.BindEqualTo(sqlAREA_CODE).And(
                        		ss252BranchWorkMk2.BranchNumber.BindEqualTo(sqlBRANCH_NUMBER)).And(
                        		ss252BranchWorkMk2.ReportGroup.BindEqualTo(sqlRPTGRP)).And(
                        		ss252BranchWorkMk2.FloorCode.BindEqualTo(sqlFLR)).And(
                        		ss252BranchWorkMk2.ReportDepartment.BindEqualTo(sqlRPT_DEPT)).And(
                        		ss252BranchWorkMk2.ReportSubDepartment.BindEqualTo(sqlRPT_SUBD)), 
                        	ss252BranchWorkMk2.SortByss252_Branch_Mk2_X1);
                        
                        
                        
                        #region Columns
                        
                        // Returned by SQL
                        Columns.Add(sqlAREA_CODE);
                        Columns.Add(sqlBRANCH_NUMBER);
                        Columns.Add(sqlRPTGRP);
                        Columns.Add(sqlFLR);
                        Columns.Add(sqlRPT_DEPT);
                        Columns.Add(sqlRPT_SUBD);
                        Columns.Add(sqlLY_WK);
                        Columns.Add(sqlLY_PER);
                        Columns.Add(sqlLY_SEAS);
                        Columns.Add(sqlLY_YR);
                        Columns.Add(sqlBAYS);
                        Columns.Add(sqlCOMP);
                        // Create/update branch work record
                        Columns.Add(vWorkFound);
                        Relations[ss252BranchWorkMk2].NotifyRowWasFoundTo(vWorkFound);
                        Columns.Add(ss252BranchWorkMk2.AreaCode);
                        Columns.Add(ss252BranchWorkMk2.BranchNumber);
                        Columns.Add(ss252BranchWorkMk2.ReportGroup);
                        Columns.Add(ss252BranchWorkMk2.FloorCode);
                        Columns.Add(ss252BranchWorkMk2.ReportDepartment);
                        Columns.Add(ss252BranchWorkMk2.ReportSubDepartment);
                        Columns.Add(ss252BranchWorkMk2.TYWeek);
                        Columns.Add(ss252BranchWorkMk2.TYPeriod);
                        Columns.Add(ss252BranchWorkMk2.TYSeason);
                        Columns.Add(ss252BranchWorkMk2.TYYear);
                        Columns.Add(ss252BranchWorkMk2.BudgetWeek);
                        Columns.Add(ss252BranchWorkMk2.BudgetPeriod);
                        Columns.Add(ss252BranchWorkMk2.BudgetSeason);
                        Columns.Add(ss252BranchWorkMk2.BudgetYear);
                        Columns.Add(ss252BranchWorkMk2.LYWeek);
                        Columns.Add(ss252BranchWorkMk2.LYPeriod);
                        Columns.Add(ss252BranchWorkMk2.LYSeason);
                        Columns.Add(ss252BranchWorkMk2.LYYear);
                        Columns.Add(ss252BranchWorkMk2.NumberOfBays);
                        Columns.Add(ss252BranchWorkMk2.CompStore);
                        #endregion
                    }
                    /// <summary>Update LY(P#62.1.3.7)</summary>
                    internal void Run()
                    {
                        Execute();
                    }
                    protected override void OnLoad()
                    {
                        RowLocking = LockingStrategy.OnRowLoading;
                        TransactionScope = TransactionScopes.Task;
                        AllowDelete = false;
                        AllowUserAbort = true;
                    }
                    protected override void OnLeaveRow()
                    {
                        ss252BranchWorkMk2.LYWeek.Value += sqlLY_WK;
                        ss252BranchWorkMk2.LYPeriod.Value += sqlLY_PER;
                        ss252BranchWorkMk2.LYSeason.Value += sqlLY_SEAS;
                        ss252BranchWorkMk2.LYYear.Value += sqlLY_YR;
                        // If creating - update Bays & Comp.
                        if(u.Not(vWorkFound))
                        {
                            ss252BranchWorkMk2.NumberOfBays.Value = sqlBAYS;
                            ss252BranchWorkMk2.CompStore.Value = sqlCOMP;
                        }
                    }
                    
                    
                }
                /// <summary>SQL Insert TY Sales(P#62.1.3.8)</summary>
                /// <remark>Last change before Migration: 08/11/2006 10:48:23</remark>
                class SQLInsertTYSales : SalesAndStock.BusinessProcessBase 
                {
                    
                    #region Models
                    DynamicSQLEntity sqlEntity;
                    #endregion
                    
                    GetBranchData _parent;
                    
                    public SQLInsertTYSales(GetBranchData parent)
                    {
                        _parent = parent;
                        Title = "SQL Insert TY Sales";
                        InitializeDataView();
                    }
                    void InitializeDataView()
                    {
                        sqlEntity = new DynamicSQLEntity(Models.DataSources.SS, 
@"--    Called from task 'SQL Insert TY Sales NEW' of MAGIC program ss252   NEW
--
insert into ss252_branch_mk2
select LEV1.area_code, LEV1.branch_number, LEV1.rptgrp, LEV1.floor, LEV1.rpt_dept, LEV1.rpt_subd,
       round(sum(LEV1.wk_sales)), round(sum(LEV1.per_sales)),
       round(sum(LEV1.seas_sales)), round(sum(LEV1.ytd_sales)),
       0, 0, 0, 0, 0, 0, 0, 0, 0, nvl(CMP.comp,'N')

from
(select YTD.area_code, YTD.branch_number, YTD.rptgrp, YTD.pdm_department_code, YTD.pdm_section_code,
       YTD.dss_department, YTD.dss_subdepartment, YTD.rpt_dept, YTD.rpt_subd, nvl(FLR.floor_code,0) floor,
       nvl(sum(WEEK.TY_Week),0) wk_sales,  nvl(sum(PER.TY_Period),0) per_sales,
       nvl(sum(SEAS.TY_Seas),0) seas_sales,  nvl(sum(YTD.TY_Year),0) YTD_sales
from
--                                                                   Determine TY Year Sales to date
(select distinct
      b.area_code , s.branch_number, 'A' rptgrp, s.pdm_department_code, s.pdm_section_code,
      c.dss_department, c.dss_subdepartment,
      nvl(a.group_1_dept,'X') rpt_dept, nvl(a.group_1_subdepartment,'X') rpt_subd,
      sum(s.section_value) TY_Year
   from
     SLA_SectSales s, REF_Branch b, REF_SectConv c, ref_dpt_analysis_grps a
  where s.transaction_date between ':2' and ':1'
    and s.section_value <> 0
    and s.pdm_department_code = c.department_code
    and s.pdm_section_code = c.section_code
    and s.branch_number = b.branch_number
    and b.branch_status = 'O'
    and trim(c.dss_department) <> '6'
    and c.dss_department = a.department_code (+)
    and c.dss_subdepartment = a.subdepartment_code (+)
  group by
       b.area_code , s.branch_number, 'A', 9,
       s.pdm_department_code, s.pdm_section_code, c.dss_department, c.dss_subdepartment,
       nvl(a.group_1_dept,'X'), nvl(a.group_1_subdepartment,'X'))                             YTD,

--                                                                   Determine TY Season Sales to date
  (select s.branch_number,s.pdm_department_code, s.pdm_section_code,
          sum(s.section_value) TY_Seas
   from sla_sectsales s
   where s.transaction_date between ':5' and ':1'
   group by s.branch_number,s.pdm_department_code, s.pdm_section_code)                        SEAS,
  
--                                                                   Determine TY Period Sales to date
  (select s.branch_number,s.pdm_department_code, s.pdm_section_code,
          sum(s.section_value) TY_Period
   from sla_sectsales s
   where s.transaction_date between ':3' and ':1'
   group by s.branch_number,s.pdm_department_code, s.pdm_section_code)                        PER,
                                                                     
--                                                                   Determine TY Week Sales
  (select s.branch_number,s.pdm_department_code, s.pdm_section_code,
          sum(s.section_value) TY_Week
   from sla_sectsales s
   where s.transaction_date between ':4' and ':1'
   group by s.branch_number,s.pdm_department_code, s.pdm_section_code)                        WEEK,
   
--                                                                   Determine Floor Code   
  (select distinct bb.branch, bb.department, bb.subdepartment, bb.floor_code
     from ref_branch_bays bb
    where bb.eff_date = (select max(bb1.eff_date)
                           from ref_branch_bays bb1
                          where bb1.branch = bb.branch
                            and bb1.department = bb.department
                            and bb1.subdepartment = bb.subdepartment
                            and bb1.eff_date <= ':1'))                               FLR
       
where YTD.branch_number = PER.branch_number (+)
  and YTD.pdm_department_code = PER.pdm_department_code (+)
  and YTD.pdm_section_code = PER.pdm_section_code (+)
   
  and YTD.branch_number = SEAS.branch_number (+)
  and YTD.pdm_department_code = SEAS.pdm_department_code (+)
  and YTD.pdm_section_code = SEAS.pdm_section_code (+)
  
  and YTD.branch_number = WEEK.branch_number (+)
  and YTD.pdm_department_code = WEEK.pdm_department_code (+)
  and YTD.pdm_section_code = WEEK.pdm_section_code (+)
  
  and YTD.branch_number = FLR.branch (+)
  and YTD.dss_department = FLR.department (+)
  and YTD.dss_subdepartment = FLR.subdepartment (+)
  
group by YTD.area_code, YTD.branch_number, YTD.rptgrp, YTD.pdm_department_code,
         YTD.pdm_section_code, YTD.dss_department, YTD.dss_subdepartment,
         YTD.rpt_dept, YTD.rpt_subd, nvl(FLR.floor_code,0))                                   LEV1,

--                                                                   Identify COMP branches
(select ba.branch_number, 'Y' comp
   from ref_bra_atts ba
   where ba.attribute_type = 60
   and ba.attribute_code = 'FCOMP'
   and ba.brn_attr_start_date = 
          (select max(ba1.brn_attr_start_date)
             from ref_bra_atts ba1
            where ba1.attribute_type = 60
              and ba1.branch_number = ba.branch_number
              and ba1.brn_attr_start_date <= ':1')
   and not exists
           (select 'Y' from ref_bra_atts ba2
             where ba2.branch_number = ba.branch_number
               and ba2.attribute_type = 61
               and ba2.attribute_code = 'SQFTG'
               and (ba2.brn_attr_end_date is null  OR
                    ':1' between ba2.brn_attr_start_date and ba2.brn_attr_end_date)))   CMP

where LEV1.branch_number = CMP.branch_number (+)                    

group by LEV1.area_code, LEV1.branch_number, LEV1.rptgrp, LEV1.floor,
         LEV1.rpt_dept, LEV1.rpt_subd, nvl(CMP.comp,'N')");
                        sqlEntity.AddParameter(() => u.DStr(_parent._parent._parent.vTYweekEndDate, "DD-MMM-YYYY")); //:1;
                        sqlEntity.AddParameter(() => u.DStr(_parent._parent._parent.vTYyearStartDate, "DD-MMM-YYYY")); //:2;
                        sqlEntity.AddParameter(() => u.DStr(_parent._parent._parent.vTYperiodStartDate, "DD-MMM-YYYY")); //:3;
                        sqlEntity.AddParameter(() => u.DStr(_parent._parent._parent.vTYweekStartDate, "DD-MMM-YYYY")); //:4;
                        sqlEntity.AddParameter(() => u.DStr(_parent._parent._parent.vTYseasStartDate, "DD-MMM-YYYY")); //:5;
                        sqlEntity.SuccessColumn = _parent.vSQLRanOK;
                        From = sqlEntity;
                        
                        
                        #region Columns
                        
                        // Via SQL inserts on Branch work table this year's sales figures accumulated to
                        // Week, Period-to-date, Season-to-date and Year-to-date levels.   The figures are
                        // accumulated to the Department levels to be reported.   Also, encoded are the
                        // relevant No Of Bays and Floor Code as well as an indicator to denote whether or
                        // not the branch is COMP.
                        
                        // Reporting Department will default to X
                        #endregion
                    }
                    /// <summary>SQL Insert TY Sales(P#62.1.3.8)</summary>
                    internal void Run()
                    {
                        Execute();
                    }
                    protected override void OnLoad()
                    {
                        Exit(ExitTiming.AfterRow);
                        TransactionScope = TransactionScopes.Task;
                        Activity = Activities.Browse;
                        AllowDelete = false;
                        AllowInsert = false;
                        AllowUpdate = false;
                        AllowUserAbort = true;
                    }
                    
                    
                }
                /// <summary>Encode Budgets(P#62.1.3.9)</summary>
                /// <remark>Last change before Migration: 08/11/2006 10:48:29</remark>
                class EncodeBudgets : SalesAndStock.BusinessProcessBase 
                {
                    
                    #region Models
                    DynamicSQLEntity sqlEntity;
                    #endregion
                    
                    GetBranchData _parent;
                    
                    public EncodeBudgets(GetBranchData parent)
                    {
                        _parent = parent;
                        Title = "Encode Budgets";
                        InitializeDataView();
                    }
                    void InitializeDataView()
                    {
                        sqlEntity = new DynamicSQLEntity(Models.DataSources.SS, 
@"--  Called by task 'Update Budgets' of MAGIC program ss252
--
update ss252_branch_mk2 w
  set
    w.budget_week =   (select round(nvl(sum(b.sales_budget),0),0)
                       from ss_br_dpt_sls_bud b
                       where b.centuryweek = ':1'
                         and b.branch_number = w.branch_number
                         and b.department = trim(w.report_department) ||
                                            trim(w.report_subdepartment)),
       
    w.budget_period = (select round(nvl(sum(b.sales_budget),0),0)
                       from ss_br_dpt_sls_bud b
                       where b.centuryweek between ':2' and ':1'
                         and b.branch_number = w.branch_number
                         and b.department = trim(w.report_department) ||
                                            trim(w.report_subdepartment)),
       
    w.budget_season = (select round(nvl(sum(b.sales_budget),0),0)
                       from ss_br_dpt_sls_bud b
                       where b.centuryweek between ':4' and ':1'
                         and b.branch_number = w.branch_number
                         and b.department = trim(w.report_department) ||
                                            trim(w.report_subdepartment)),
                                              
    w.budget_year =   (select round(nvl(sum(b.sales_budget),0),0)
                       from ss_br_dpt_sls_bud b
                       where b.centuryweek between ':3' and ':1'
                         and b.branch_number = w.branch_number
                         and b.department = trim(w.report_department) ||
                                            trim(w.report_subdepartment))");
                        sqlEntity.AddParameter(_parent._parent._parent.vTYreqWeek); //:1;
                        sqlEntity.AddParameter(_parent._parent._parent.vTYperiodStartWeek); //:2;
                        sqlEntity.AddParameter(_parent._parent._parent.vTYyearStartWeek); //:3;
                        sqlEntity.AddParameter(_parent._parent._parent.vTYseasStartWeek); //:4;
                        sqlEntity.SuccessColumn = _parent.vSQLRanOK;
                        From = sqlEntity;
                        
                        
                        #region Columns
                        
                        // Update Budget figures to Branch Work via SQL
                        #endregion
                    }
                    /// <summary>Encode Budgets(P#62.1.3.9)</summary>
                    internal void Run()
                    {
                        Execute();
                    }
                    protected override void OnLoad()
                    {
                        Exit(ExitTiming.AfterRow);
                        TransactionScope = TransactionScopes.Task;
                        Activity = Activities.Browse;
                        AllowDelete = false;
                        AllowInsert = false;
                        AllowUpdate = false;
                        AllowUserAbort = true;
                    }
                    
                    
                }
                /// <summary>Area Totals(P#62.1.3.10)</summary>
                /// <remark>Last change before Migration: 08/11/2006 10:48:37</remark>
                class AreaTotals : SalesAndStock.BusinessProcessBase 
                {
                    
                    #region Models
                    DynamicSQLEntity sqlEntity;
                    #endregion
                    
                    GetBranchData _parent;
                    
                    public AreaTotals(GetBranchData parent)
                    {
                        _parent = parent;
                        Title = "Area Totals";
                        InitializeDataView();
                    }
                    void InitializeDataView()
                    {
                        sqlEntity = new DynamicSQLEntity(Models.DataSources.SS, 
@"--  Called by task 'Area Sales' of MAGIC program ss252
--
insert into ss252_area_mk2
  select w.area_code, w.report_group, w.report_department, w.report_subdepartment,
         nvl(sum(w.ty_week),0) ty_week, nvl(sum(w.ty_period),0) ty_period,
         nvl(sum(w.ty_season),0) ty_season, nvl(sum(w.ty_year),0) ty_year,
         nvl(sum(w.ly_week),0) ly_week, nvl(sum(w.ly_period),0) ly_period,
         nvl(sum(w.ly_season),0) ly_season, nvl(sum(w.ly_year),0) ly_year,
         nvl(sum(w.number_of_bays),0) bays
  from ss252_branch_mk2 w
  where w.comp_store = 'Y'
    and w.report_group = 'A'
  group by w.area_code, w.report_group, w.report_department, w.report_subdepartment");
                        sqlEntity.SuccessColumn = _parent.vSQLRanOK;
                        From = sqlEntity;
                        
                        
                        #region Columns
                        
                        // For COMP branches only, roll up sales figures to AREA level via SQL.
                        #endregion
                    }
                    /// <summary>Area Totals(P#62.1.3.10)</summary>
                    internal void Run()
                    {
                        Execute();
                    }
                    protected override void OnLoad()
                    {
                        Exit(ExitTiming.AfterRow);
                        TransactionScope = TransactionScopes.Task;
                        Activity = Activities.Browse;
                        AllowDelete = false;
                        AllowInsert = false;
                        AllowUpdate = false;
                        AllowUserAbort = true;
                    }
                    
                    
                }
                /// <summary>Encode Bay Totals(P#62.1.3.11)</summary>
                /// <remark>Last change before Migration: 08/11/2006 10:48:42</remark>
                class EncodeBayTotals : SalesAndStock.BusinessProcessBase 
                {
                    
                    #region Models
                    DynamicSQLEntity sqlEntity;
                    /// <summary>ss252 Branch Work Mk2</summary>
                    readonly Models.ss252BranchWorkMk2 ss252BranchWorkMk2 = new Models.ss252BranchWorkMk2 { AllowRowLocking = true };
                    #endregion
                    
                    #region Columns
                    /// <summary>AREA_CODE</summary>
                    readonly TextColumn AREA_CODE = new TextColumn("AREA_CODE", "2")
                    {
                    	AllowNull = true
                    };
                    /// <summary>BRANCH</summary>
                    readonly NumberColumn BRANCH = new NumberColumn("BRANCH", "N4")
                    {
                    	AllowNull = true
                    };
                    /// <summary>RPT_DEPT</summary>
                    readonly TextColumn RPT_DEPT = new TextColumn("RPT_DEPT", "2")
                    {
                    	AllowNull = true
                    };
                    /// <summary>RPT_SUBD</summary>
                    readonly TextColumn RPT_SUBD = new TextColumn("RPT_SUBD", "2")
                    {
                    	AllowNull = true
                    };
                    /// <summary>FLOOR_CODE</summary>
                    readonly NumberColumn FLOOR_CODE = new NumberColumn("FLOOR_CODE", "N1")
                    {
                    	AllowNull = true
                    };
                    /// <summary>NO_OF_BAYS</summary>
                    readonly NumberColumn NO_OF_BAYS = new NumberColumn("NO_OF_BAYS", "N4")
                    {
                    	AllowNull = true
                    };
                    /// <summary>COMP</summary>
                    readonly TextColumn COMP = new TextColumn("COMP", "1")
                    {
                    	AllowNull = true
                    };
                    #endregion
                    
                    GetBranchData _parent;
                    
                    public EncodeBayTotals(GetBranchData parent)
                    {
                        _parent = parent;
                        Title = "Encode Bay Totals";
                        InitializeDataView();
                    }
                    void InitializeDataView()
                    {
                        sqlEntity = new DynamicSQLEntity(Models.DataSources.Ref1, 
@"--  Called by task 'Encode Bay Totals' in MAGIC program ss252
--
select q1.area_code, q1.branch, q1.rpt_dept, q1.rpt_subd, q1.floor_code, sum(q1.no_of_bays), q1.comp
from
  (select b.area_code, bb.branch, bb.department, bb.subdepartment, bb.floor_code, bb.no_of_bays,
         nvl(a.group_1_dept,'X') rpt_dept, nvl(a.group_1_subdepartment,'X') rpt_subd,
      (case when exists
         (select 'x'
            from ref_bra_atts att1
           where att1.branch_number = bb.branch
             and att1.attribute_type = 60
             and att1.attribute_code = 'FCOMP'
             and att1.brn_attr_start_date =
                 (select max (att2.brn_attr_start_date)
                    from ref_bra_atts att2
                    where att2.attribute_type = 60
                      and att2.branch_number = bb.branch
                      and att2.brn_attr_start_date <= ':1')
             and not exists
                (select 'y'
                   from ref_bra_atts att3
                  where att3.branch_number = att1.branch_number
                    and att3.attribute_type = 61
                    and att3.attribute_code = 'SQFTG'
                    and
                     (att3.brn_attr_end_date is null
                      or
                      ':1' between att3.brn_attr_start_date and att3.brn_attr_end_date)))
          then 'Y' else 'N' end) Comp
  from ref_branch_bays bb, ref_dpt_analysis_grps a, ref_branch b
  where bb.eff_date = (select max(bb1.eff_date)
                       from ref_branch_bays bb1
                       where bb1.branch = bb.branch
                         and bb1.department = bb.department
                         and bb1.subdepartment = bb.subdepartment
                         and bb1.eff_date <= ':1')
    and bb.department <> '98'
    and trim(bb.last_upd_user) = 'RF552'
    and bb.branch = b.branch_number
    and b.branch_status = 'O'
    and bb.department = a.department_code (+)
    and bb.subdepartment = a.subdepartment_code (+))    q1
group by q1.area_code, q1.branch, q1.rpt_dept, q1.rpt_subd, q1.floor_code, q1.comp
order by q1.area_code, q1.branch, q1.rpt_dept, q1.rpt_subd, q1.floor_code, q1.comp");
                        sqlEntity.AddParameter(() => u.DStr(_parent._parent._parent.vTYweekEndDate, "DD-MMM-YYYY")); //:1;
                        sqlEntity.Columns.Add(AREA_CODE, BRANCH, RPT_DEPT, RPT_SUBD, FLOOR_CODE, NO_OF_BAYS, COMP);
                        From = sqlEntity;
                        
                        Relations.Add(ss252BranchWorkMk2, RelationType.InsertIfNotFound, 
                        		ss252BranchWorkMk2.AreaCode.BindEqualTo(AREA_CODE).And(
                        		ss252BranchWorkMk2.BranchNumber.BindEqualTo(BRANCH)).And(
                        		ss252BranchWorkMk2.ReportGroup.BindEqualTo("A")).And(
                        		ss252BranchWorkMk2.FloorCode.BindEqualTo(FLOOR_CODE)).And(
                        		ss252BranchWorkMk2.ReportDepartment.BindEqualTo(RPT_DEPT)).And(
                        		ss252BranchWorkMk2.ReportSubDepartment.BindEqualTo(RPT_SUBD)), 
                        	ss252BranchWorkMk2.SortByss252_Branch_Mk2_X1);
                        
                        
                        
                        #region Columns
                        
                        // Returned by SQL
                        Columns.Add(AREA_CODE);
                        Columns.Add(BRANCH);
                        Columns.Add(RPT_DEPT);
                        Columns.Add(RPT_SUBD);
                        Columns.Add(FLOOR_CODE);
                        Columns.Add(NO_OF_BAYS);
                        Columns.Add(COMP);
                        
                        Columns.Add(ss252BranchWorkMk2.AreaCode);
                        Columns.Add(ss252BranchWorkMk2.BranchNumber);
                        Columns.Add(ss252BranchWorkMk2.ReportGroup);
                        Columns.Add(ss252BranchWorkMk2.FloorCode);
                        Columns.Add(ss252BranchWorkMk2.ReportDepartment);
                        Columns.Add(ss252BranchWorkMk2.ReportSubDepartment);
                        Columns.Add(ss252BranchWorkMk2.TYWeek);
                        Columns.Add(ss252BranchWorkMk2.TYPeriod);
                        Columns.Add(ss252BranchWorkMk2.TYSeason);
                        Columns.Add(ss252BranchWorkMk2.TYYear);
                        Columns.Add(ss252BranchWorkMk2.BudgetWeek);
                        Columns.Add(ss252BranchWorkMk2.BudgetPeriod);
                        Columns.Add(ss252BranchWorkMk2.BudgetSeason);
                        Columns.Add(ss252BranchWorkMk2.BudgetYear);
                        Columns.Add(ss252BranchWorkMk2.LYWeek);
                        Columns.Add(ss252BranchWorkMk2.LYPeriod);
                        Columns.Add(ss252BranchWorkMk2.LYSeason);
                        Columns.Add(ss252BranchWorkMk2.LYYear);
                        Columns.Add(ss252BranchWorkMk2.NumberOfBays);
                        Columns.Add(ss252BranchWorkMk2.CompStore).BindValue(COMP);
                        #endregion
                    }
                    /// <summary>Encode Bay Totals(P#62.1.3.11)</summary>
                    internal void Run()
                    {
                        Execute();
                    }
                    protected override void OnLoad()
                    {
                        RowLocking = LockingStrategy.OnRowLoading;
                        TransactionScope = TransactionScopes.Task;
                        AllowDelete = false;
                        AllowUserAbort = true;
                    }
                    protected override void OnLeaveRow()
                    {
                        ss252BranchWorkMk2.NumberOfBays.Value = NO_OF_BAYS;
                    }
                    
                    
                }
                /// <summary>Set Up Branch List(P#62.1.3.12)</summary>
                /// <remark>Last change before Migration: 08/11/2006 10:48:49</remark>
                class SetUpBranchList : SalesAndStock.BusinessProcessBase 
                {
                    
                    #region Models
                    DynamicSQLEntity sqlEntity;
                    #endregion
                    
                    public SetUpBranchList()
                    {
                        Title = "Set Up Branch List";
                        InitializeDataView();
                    }
                    void InitializeDataView()
                    {
                        sqlEntity = new DynamicSQLEntity(Models.DataSources.SS, 
@"--  Called by task 'Set Up Branch List' of MAGIC program ss252
--
insert into ss_Diary_Work1
select distinct 'ss252', w.branch_number,  w.area_code, w.comp_store
  from ss252_Branch_Mk2 w");
                        From = sqlEntity;
                        
                        
                        #region Columns
                        
                        // This SQL task will generate a control table of the branches for which sales data
                        // has been collated.  It will hold for each branch the relevant COMP status and
                        // AREA code.
                        #endregion
                    }
                    /// <summary>Set Up Branch List(P#62.1.3.12)</summary>
                    internal void Run()
                    {
                        Execute();
                    }
                    protected override void OnLoad()
                    {
                        Exit(ExitTiming.AfterRow);
                        TransactionScope = TransactionScopes.Task;
                        AllowDelete = false;
                        AllowUserAbort = true;
                    }
                    
                    
                }
                /// <summary>Missing Depts(P#62.1.3.13)</summary>
                /// <remark>Last change before Migration: 08/11/2006 10:48:55</remark>
                class MissingDepts : SalesAndStock.BusinessProcessBase 
                {
                    
                    #region Models
                    DynamicSQLEntity sqlEntity;
                    #endregion
                    
                    public MissingDepts()
                    {
                        Title = "Missing Depts";
                        InitializeDataView();
                    }
                    void InitializeDataView()
                    {
                        sqlEntity = new DynamicSQLEntity(Models.DataSources.SS, 
@"--  Called by task 'Missing Depts' of MAGIC program ss252
--
insert into ss252_branch_mk2
select b.area_code, b.branch_number, 'A', 9, a.group1_dept, a.group1_subdept,
       0,0,0,0,0,0,0,0,0,0,0,0,0, w1.comp_store
from
ref_anal_grp1_dpts a, ref_branch b, ss_diary_work1 w1
where b.branch_number = w1.branch_number
  and b.branch_status = 'O'
  and w1.prog_no = 'ss252'
  and not exists
       (select 'Y' from ss252_branch_mk2 w
             where w.area_code = b.area_code
               and w.branch_number = b.branch_number
               and w.report_group = 'A'
               and w.report_department = a.group1_dept
               and w.report_subdepartment = a.group1_subdept)
order by b.area_code, b.branch_number, a.group1_dept, a.group1_subdept");
                        From = sqlEntity;
                        
                        
                        #region Columns
                        
                        // Via SQL set up dummy records for Depts for which a Branch has no sales
                        // Floor Code 9 will be allocated to these records.
                        #endregion
                    }
                    /// <summary>Missing Depts(P#62.1.3.13)</summary>
                    internal void Run()
                    {
                        Execute();
                    }
                    protected override void OnLoad()
                    {
                        Exit(ExitTiming.AfterRow);
                        TransactionScope = TransactionScopes.Task;
                        AllowDelete = false;
                        AllowUserAbort = true;
                    }
                    
                    
                }
                    
                /// <summary>Mackays Card(P#62.1.3.15)</summary>
                /// <remark>Last change before Migration: 02/09/2008 16:46:35</remark>
                internal class MackaysCard : SalesAndStock.BusinessProcessBase 
                {
                    
                    #region Columns
                    /// <summary>v:Mack TY Start Week</summary>
                    readonly DateColumn vMackTYStartWeek = new DateColumn("v:Mack TY Start Week");
                    /// <summary>v:Mack TY Start Period</summary>
                    readonly DateColumn vMackTYStartPeriod = new DateColumn("v:Mack TY Start Period");
                    /// <summary>v:Mack TY Start Season</summary>
                    readonly DateColumn vMackTYStartSeason = new DateColumn("v:Mack TY Start Season");
                    /// <summary>v:Mack TY Start Year</summary>
                    readonly DateColumn vMackTYStartYear = new DateColumn("v:Mack TY Start Year");
                    /// <summary>v:Mack TY End Week</summary>
                    readonly DateColumn vMackTYEndWeek = new DateColumn("v:Mack TY End Week");
                    /// <summary>v:Mack LY Start Week</summary>
                    readonly DateColumn vMackLYStartWeek = new DateColumn("v:Mack LY Start Week");
                    /// <summary>v:Mack LY Start Period</summary>
                    readonly DateColumn vMackLYStartPeriod = new DateColumn("v:Mack LY Start Period");
                    /// <summary>v:Mack LY Start Season</summary>
                    readonly DateColumn vMackLYStartSeason = new DateColumn("v:Mack LY Start Season");
                    /// <summary>v:Mack LY Start Year</summary>
                    readonly DateColumn vMackLYStartYear = new DateColumn("v:Mack LY Start Year");
                    /// <summary>v:Mack LY End Week</summary>
                    readonly DateColumn vMackLYEndWeek = new DateColumn("v:Mack LY End Week");
                    #endregion
                    
                    GetBranchData _parent;
                    
                    public MackaysCard(GetBranchData parent)
                    {
                        _parent = parent;
                        Title = "Mackays Card";
                        InitializeDataView();
                    }
                    void InitializeDataView()
                    {
                        
                        
                        #region Columns
                        
                        Columns.Add(vMackTYStartWeek);
                        Columns.Add(vMackTYStartPeriod);
                        Columns.Add(vMackTYStartSeason);
                        Columns.Add(vMackTYStartYear);
                        Columns.Add(vMackTYEndWeek);
                        Columns.Add(vMackLYStartWeek);
                        Columns.Add(vMackLYStartPeriod);
                        Columns.Add(vMackLYStartSeason);
                        Columns.Add(vMackLYStartYear);
                        Columns.Add(vMackLYEndWeek);
                        #endregion
                    }
                    /// <summary>Mackays Card(P#62.1.3.15)</summary>
                    internal void Run()
                    {
                        Execute();
                    }
                    protected override void OnLoad()
                    {
                        Exit(ExitTiming.AfterRow);
                        AllowUserAbort = true;
                    }
                    protected override void OnLeaveRow()
                    {
                        // Init SQL parameters to accumulate store totals of Mackays Card sales.
                        vMackTYStartWeek.Value = _parent._parent._parent.vTYweekStartDate;
                        vMackTYStartPeriod.Value = _parent._parent._parent.vTYperiodStartDate;
                        vMackTYStartSeason.Value = _parent._parent._parent.vTYseasStartDate;
                        vMackTYStartYear.Value = _parent._parent._parent.vTYyearStartDate;
                        vMackTYEndWeek.Value = _parent._parent._parent.vTYweekEndDate;
                        vMackLYStartWeek.Value = _parent._parent._parent.vPYweekStartDate;
                        vMackLYStartPeriod.Value = _parent._parent._parent.vPYperiodStartDate;
                        vMackLYStartSeason.Value = _parent._parent._parent.vPYseasonStartDate;
                        vMackLYStartYear.Value = _parent._parent._parent.vPYyearStartDate;
                        vMackLYEndWeek.Value = _parent._parent._parent.vPYweekEndDate;
                        _parent.vSQLTask.Value = "MC Store Tots";
                        Cached<MCStoreTots>().Run();
                        // Accumulate the area totals for Mackays Card figures (COMP stores only)
                        if(_parent.vSQLRanOK)
                        {
                            _parent._parent.vMsgToUser.Value = "Assembling Mackays Card area totals";
                            _parent.vSQLTask.Value = "MC Area Tots";
                            Cached<MCAreaTots>().Run();
                        }
                        // Move Mackays Card sales to Print Work table
                        if(_parent.vSQLRanOK)
                        {
                            _parent._parent.vMsgToUser.Value = "Moving Mackays Card figures to Print Work";
                            Cached<MCBuildPrintWork>().Run();
                        }
                    }
                    
                    
                    /// <summary>MC Store Tots(P#62.1.3.15.1)</summary>
                    /// <remark>Last change before Migration: 02/09/2008 15:42:52</remark>
                    class MCStoreTots : SalesAndStock.BusinessProcessBase 
                    {
                        
                        #region Models
                        DynamicSQLEntity sqlEntity;
                        #endregion
                        
                        MackaysCard _parent;
                        
                        public MCStoreTots(MackaysCard parent)
                        {
                            _parent = parent;
                            Title = "MC Store Tots";
                            InitializeDataView();
                        }
                        void InitializeDataView()
                        {
                            sqlEntity = new DynamicSQLEntity(Models.DataSources.SS, 
@"--  Called by task 'MC Store Tots' of MAGIC program ss252
--
insert into ss252_branch_Mk2

select q1.area_code, q1.branch_number, 'C', 0, ' ', ' ',
       round(nvl(q2.TY_week,0)) TY_wk, round(nvl(q3.TY_period,0)) TY_per,
       round(nvl(q4.TY_seas,0)) TY_seas, round(nvl(q5.TY_year,0)) TY_yr,
       0, 0, 0, 0,
       round(nvl(q6.LY_week,0)) LY_wk, round(nvl(q7.LY_period,0)) LY_per,
       round(nvl(q8.LY_seas,0)) LY_seas, round(nvl(q9.LY_year,0)) LY_yr,
       0, q1.comp_store
from

   (select w1.branch_number, w1.area_code, w1.comp_store
    from ss_diary_work1 w1
    where w1.prog_no = 'ss252')                                           q1,
 
   (select t.branch_number, sum(nvl(t.value,0)) TY_week
    from sla_tenders t
    where t.till_date between ':1' and ':5'
      and t.rtc_tender_type_code in (6,17,18)
    group by t.branch_number)                                             q2,

   (select t1.branch_number, sum(nvl(t1.value,0)) TY_period
    from sla_tenders t1
    where t1.till_date between ':2' and ':5'
      and t1.rtc_tender_type_code in (6,17,18)
    group by t1.branch_number)                                            q3,
    
   (select t11.branch_number, sum(nvl(t11.value,0)) TY_seas
    from sla_tenders t11
    where t11.till_date between ':3' and ':5'
      and t11.rtc_tender_type_code in (6,17,18)
    group by t11.branch_number)                                           q4,
   
   (select t2.branch_number, sum(nvl(t2.value,0)) TY_year
    from sla_tenders t2
    where t2.till_date between ':4' and ':5'
      and t2.rtc_tender_type_code in (6,17,18)
    group by t2.branch_number)                                            q5,
   
   (select t3.branch_number, sum(nvl(t3.value,0)) LY_week
    from sla_tenders t3
    where t3.till_date between ':6' and ':10'
      and t3.rtc_tender_type_code in (6,17,18)
    group by t3.branch_number)                                            q6,

   (select t4.branch_number, sum(nvl(t4.value,0)) LY_period
    from sla_tenders t4
    where t4.till_date between ':7' and ':10'
      and t4.rtc_tender_type_code in (6,17,18)
    group by t4.branch_number)                                            q7,
    
    (select t12.branch_number, sum(nvl(t12.value,0)) LY_seas
    from sla_tenders t12
    where t12.till_date between ':8' and ':10'
      and t12.rtc_tender_type_code in (6,17,18)
    group by t12.branch_number)                                           q8,
   
   (select t5.branch_number, sum(nvl(t5.value,0)) LY_year
    from sla_tenders t5
    where t5.till_date between ':9' and ':10'
      and t5.rtc_tender_type_code in (6,17,18)
    group by t5.branch_number)                                            q9
 
where q1.branch_number = q2.branch_number (+)
  and q1.branch_number = q3.branch_number (+)
  and q1.branch_number = q4.branch_number (+)
  and q1.branch_number = q5.branch_number (+)
  and q1.branch_number = q6.branch_number (+)
  and q1.branch_number = q7.branch_number (+)
  and q1.branch_number = q9.branch_number (+)
  and q1.branch_number = q8.branch_number (+)");
                            sqlEntity.AddParameter(() => u.DStr(_parent.vMackTYStartWeek, "DD-MMM-YYYY")); //:1;
                            sqlEntity.AddParameter(() => u.DStr(_parent.vMackTYStartPeriod, "DD-MMM-YYYY")); //:2;
                            sqlEntity.AddParameter(() => u.DStr(_parent.vMackTYStartSeason, "DD-MMM-YYYY")); //:3;
                            sqlEntity.AddParameter(() => u.DStr(_parent.vMackTYStartYear, "DD-MMM-YYYY")); //:4;
                            sqlEntity.AddParameter(() => u.DStr(_parent.vMackTYEndWeek, "DD-MMM-YYYY")); //:5;
                            sqlEntity.AddParameter(() => u.DStr(_parent.vMackLYStartWeek, "DD-MMM-YYYY")); //:6;
                            sqlEntity.AddParameter(() => u.DStr(_parent.vMackLYStartPeriod, "DD-MMM-YYYY")); //:7;
                            sqlEntity.AddParameter(() => u.DStr(_parent.vMackLYStartSeason, "DD-MMM-YYYY")); //:8;
                            sqlEntity.AddParameter(() => u.DStr(_parent.vMackLYStartYear, "DD-MMM-YYYY")); //:9;
                            sqlEntity.AddParameter(() => u.DStr(_parent.vMackLYEndWeek, "DD-MMM-YYYY")); //:10;
                            sqlEntity.SuccessColumn = _parent._parent.vSQLRanOK;
                            From = sqlEntity;
                            
                            
                            #region Columns
                            
                            // This SQL task assembles Branch totals for Mackays Card data and writes them to
                            // table ss252_Branch_Mk2 with Report Group set to 'C'.
                            
                            // Week, Period, Season & Year-to-date SALES figures will be collated for this year
                            // and last year.
                            #endregion
                        }
                        /// <summary>MC Store Tots(P#62.1.3.15.1)</summary>
                        internal void Run()
                        {
                            Execute();
                        }
                        protected override void OnLoad()
                        {
                            Exit(ExitTiming.AfterRow);
                            TransactionScope = TransactionScopes.Task;
                            Activity = Activities.Browse;
                            AllowDelete = false;
                            AllowInsert = false;
                            AllowUpdate = false;
                            AllowUserAbort = true;
                        }
                        protected override void OnEnd()
                        {
                        }
                        
                        
                    }
                    /// <summary>MC Area Tots(P#62.1.3.15.2)</summary>
                    /// <remark>Last change before Migration: 08/11/2006 10:53:26</remark>
                    class MCAreaTots : SalesAndStock.BusinessProcessBase 
                    {
                        
                        #region Models
                        DynamicSQLEntity sqlEntity;
                        #endregion
                        
                        MackaysCard _parent;
                        
                        public MCAreaTots(MackaysCard parent)
                        {
                            _parent = parent;
                            Title = "MC Area Tots";
                            InitializeDataView();
                        }
                        void InitializeDataView()
                        {
                            sqlEntity = new DynamicSQLEntity(Models.DataSources.SS, 
@"--  Called by task 'MC Area Tots' of MAGIC program ss252
--
insert into ss252_area_mk2
  select w.area_code, w.report_group, w.report_department, w.report_subdepartment,
         nvl(sum(w.ty_week),0) ty_week, nvl(sum(w.ty_period),0) ty_period,
         nvl(sum(w.ty_season),0) ty_seas, nvl(sum(w.ty_year),0) ty_year,
         nvl(sum(w.ly_week),0) ly_week, nvl(sum(w.ly_period),0) ly_period,
         nvl(sum(w.ly_season),0) ly_seas, nvl(sum(w.ly_year),0) ly_year, 0
  from ss252_branch_mk2 w
  where w.comp_store = 'Y'
    and w.report_group = 'C'
  group by w.area_code, w.report_group, w.report_department, w.report_subdepartment");
                            sqlEntity.SuccessColumn = _parent._parent.vSQLRanOK;
                            From = sqlEntity;
                            
                            
                            #region Columns
                            
                            // This task assembles the Mackays Card figures by Area by rolling up relevant data
                            // (for COMP stores only) from ss252_Branch_Mk2 to table ss252_Area_Mk2.
                            
                            // Resulting figures will be for This Year and Last Year.
                            #endregion
                        }
                        /// <summary>MC Area Tots(P#62.1.3.15.2)</summary>
                        internal void Run()
                        {
                            Execute();
                        }
                        protected override void OnLoad()
                        {
                            Exit(ExitTiming.AfterRow);
                            TransactionScope = TransactionScopes.Task;
                            Activity = Activities.Browse;
                            AllowDelete = false;
                            AllowInsert = false;
                            AllowUpdate = false;
                            AllowUserAbort = true;
                        }
                        
                        
                    }
                    /// <summary>MC Build Print Work(P#62.1.3.15.3)</summary>
                    /// <remark>Last change before Migration: 08/11/2006 10:53:50</remark>
                    internal class MCBuildPrintWork : SalesAndStock.BusinessProcessBase 
                    {
                        
                        #region Models
                        DynamicSQLEntity sqlEntity;
                        #endregion
                        
                        #region Columns
                        /// <summary>AREA_CODE</summary>
                        internal readonly TextColumn AREA_CODE = new TextColumn("AREA_CODE", "2")
                        {
                        	AllowNull = true
                        };
                        /// <summary>BRANCH_NUMBER</summary>
                        internal readonly NumberColumn BRANCH_NUMBER = new NumberColumn("BRANCH_NUMBER", "N4")
                        {
                        	AllowNull = true
                        };
                        /// <summary>REPORT_GROUP</summary>
                        internal readonly TextColumn REPORT_GROUP = new TextColumn("REPORT_GROUP", "1")
                        {
                        	AllowNull = true
                        };
                        /// <summary>FLOOR_CODE</summary>
                        internal readonly NumberColumn FLOOR_CODE = new NumberColumn("FLOOR_CODE", "N1")
                        {
                        	AllowNull = true
                        };
                        /// <summary>REPORT_DEPARTMENT</summary>
                        internal readonly TextColumn REPORT_DEPARTMENT = new TextColumn("REPORT_DEPARTMENT", "2")
                        {
                        	AllowNull = true
                        };
                        /// <summary>REPORT_SUBDEPARTMENT</summary>
                        internal readonly TextColumn REPORT_SUBDEPARTMENT = new TextColumn("REPORT_SUBDEPARTMENT", "2")
                        {
                        	AllowNull = true
                        };
                        /// <summary>BRN_TY_WEEK</summary>
                        internal readonly NumberColumn BRN_TY_WEEK = new NumberColumn("BRN_TY_WEEK", "N8")
                        {
                        	AllowNull = true
                        };
                        /// <summary>BRN_TY_PER</summary>
                        internal readonly NumberColumn BRN_TY_PER = new NumberColumn("BRN_TY_PER", "N8")
                        {
                        	AllowNull = true
                        };
                        /// <summary>BRN_TY_SEAS</summary>
                        internal readonly NumberColumn BRN_TY_SEAS = new NumberColumn("BRN_TY_SEAS", "N8")
                        {
                        	AllowNull = true
                        };
                        /// <summary>BRN_TY_YR</summary>
                        internal readonly NumberColumn BRN_TY_YR = new NumberColumn("BRN_TY_YR", "N8")
                        {
                        	AllowNull = true
                        };
                        /// <summary>BRN_LY_WEEK</summary>
                        internal readonly NumberColumn BRN_LY_WEEK = new NumberColumn("BRN_LY_WEEK", "N8")
                        {
                        	AllowNull = true
                        };
                        /// <summary>BRN_LY_PER</summary>
                        internal readonly NumberColumn BRN_LY_PER = new NumberColumn("BRN_LY_PER", "N8")
                        {
                        	AllowNull = true
                        };
                        /// <summary>BRN_LY_SEAS</summary>
                        internal readonly NumberColumn BRN_LY_SEAS = new NumberColumn("BRN_LY_SEAS", "N8")
                        {
                        	AllowNull = true
                        };
                        /// <summary>BRN_LY_YR</summary>
                        internal readonly NumberColumn BRN_LY_YR = new NumberColumn("BRN_LY_YR", "N8")
                        {
                        	AllowNull = true
                        };
                        /// <summary>A_TY_WEEK</summary>
                        internal readonly NumberColumn A_TY_WEEK = new NumberColumn("A_TY_WEEK", "N9.2")
                        {
                        	AllowNull = true
                        };
                        /// <summary>A_TY_PER</summary>
                        internal readonly NumberColumn A_TY_PER = new NumberColumn("A_TY_PER", "N9.2")
                        {
                        	AllowNull = true
                        };
                        /// <summary>A_TY_SEAS</summary>
                        internal readonly NumberColumn A_TY_SEAS = new NumberColumn("A_TY_SEAS", "N9.2")
                        {
                        	AllowNull = true
                        };
                        /// <summary>A_TY_YR</summary>
                        internal readonly NumberColumn A_TY_YR = new NumberColumn("A_TY_YR", "N9.2")
                        {
                        	AllowNull = true
                        };
                        /// <summary>A_LY_WEEK</summary>
                        internal readonly NumberColumn A_LY_WEEK = new NumberColumn("A_LY_WEEK", "N9.2")
                        {
                        	AllowNull = true
                        };
                        /// <summary>A_LY_PER</summary>
                        internal readonly NumberColumn A_LY_PER = new NumberColumn("A_LY_PER", "N9.2")
                        {
                        	AllowNull = true
                        };
                        /// <summary>A_LY_SEAS</summary>
                        internal readonly NumberColumn A_LY_SEAS = new NumberColumn("A_LY_SEAS", "N9.2")
                        {
                        	AllowNull = true
                        };
                        /// <summary>A_LY_YR</summary>
                        internal readonly NumberColumn A_LY_YR = new NumberColumn("A_LY_YR", "N9.2")
                        {
                        	AllowNull = true
                        };
                        /// <summary>STORE_TY_WEEK</summary>
                        internal readonly NumberColumn STORE_TY_WEEK = new NumberColumn("STORE_TY_WEEK", "10.3")
                        {
                        	AllowNull = true
                        };
                        /// <summary>STORE_TY_PER</summary>
                        internal readonly NumberColumn STORE_TY_PER = new NumberColumn("STORE_TY_PER", "10.3")
                        {
                        	AllowNull = true
                        };
                        /// <summary>STORE_TY_SEAS</summary>
                        internal readonly NumberColumn STORE_TY_SEAS = new NumberColumn("STORE_TY_SEAS", "10.3")
                        {
                        	AllowNull = true
                        };
                        /// <summary>STORE_TY_YEAR</summary>
                        internal readonly NumberColumn STORE_TY_YEAR = new NumberColumn("STORE_TY_YEAR", "10.3")
                        {
                        	AllowNull = true
                        };
                        /// <summary>STORE_LY_WEEK</summary>
                        internal readonly NumberColumn STORE_LY_WEEK = new NumberColumn("STORE_LY_WEEK", "10.3")
                        {
                        	AllowNull = true
                        };
                        /// <summary>STORE_LY_PER</summary>
                        internal readonly NumberColumn STORE_LY_PER = new NumberColumn("STORE_LY_PER", "10.3")
                        {
                        	AllowNull = true
                        };
                        /// <summary>STORE_LY_SEAS</summary>
                        internal readonly NumberColumn STORE_LY_SEAS = new NumberColumn("STORE_LY_SEAS", "10.3")
                        {
                        	AllowNull = true
                        };
                        /// <summary>STORE_LY_YEAR</summary>
                        internal readonly NumberColumn STORE_LY_YEAR = new NumberColumn("STORE_LY_YEAR", "10.3")
                        {
                        	AllowNull = true
                        };
                        /// <summary>v:Tmp Rec Type</summary>
                        internal readonly TextColumn vTmpRecType = new TextColumn("v:Tmp Rec Type", "U");
                        /// <summary>v:Tmp Brn Mack TY</summary>
                        internal readonly NumberColumn vTmpBrnMackTY = new NumberColumn("v:Tmp Brn Mack TY", "N7");
                        /// <summary>v:Tmp Brn Mack LY</summary>
                        internal readonly NumberColumn vTmpBrnMackLY = new NumberColumn("v:Tmp Brn Mack LY", "N7");
                        /// <summary>v:Tmp Area Mack TY</summary>
                        internal readonly NumberColumn vTmpAreaMackTY = new NumberColumn("v:Tmp Area Mack TY", "N7");
                        /// <summary>v:Tmp Area Mack LY</summary>
                        internal readonly NumberColumn vTmpAreaMackLY = new NumberColumn("v:Tmp Area Mack LY", "N7");
                        /// <summary>v:Tmp Brn Sales TY</summary>
                        internal readonly NumberColumn vTmpBrnSalesTY = new NumberColumn("v:Tmp Brn Sales TY", "N7");
                        /// <summary>v:Tmp Brn Sales LY</summary>
                        internal readonly NumberColumn vTmpBrnSalesLY = new NumberColumn("v:Tmp Brn Sales LY", "N7");
                        #endregion
                        
                        public MCBuildPrintWork()
                        {
                            Title = "MC Build Print Work";
                            InitializeDataView();
                        }
                        void InitializeDataView()
                        {
                            sqlEntity = new DynamicSQLEntity(Models.DataSources.SS, 
@"--  Called by task 'MC Build Print Work' in MAGIC program ss252
--
select q1.area_code, q1.branch_number, q1.report_group,
       q1.floor_code, q1.report_department, q1.report_subdepartment,
       q1.ty_week brn_ty_week, q1.ty_period brn_ty_per, q1.ty_season brn_ty_seas, q1.ty_year brn_ty_yr,
       q1.ly_week brn_ly_week, q1.ly_period brn_ly_per, q1.ly_season brn_ly_seas, q1.ly_year brn_ly_yr,
       q2.ty_week a_ty_week, q2.ty_period a_ty_per, q2.ty_season a_ty_seas, q2.ty_year a_ty_yr,
       q2.ly_week a_ly_week, q2.ly_period a_ly_per, q2.ly_season a_ly_seas, q2.ly_year a_ly_yr,
       nvl(q3.This_Year,0) store_ty_week, nvl(q4.This_Year,0) store_ty_per,
       nvl(q5.This_Year,0) store_ty_seas, nvl(q6.This_Year,0) store_ty_year, 
       nvl(q3.Last_Year,0) store_ly_week, nvl(q4.Last_Year,0) store_ly_per,
       nvl(q5.Last_Year,0) store_ly_seas, nvl(q6.Last_Year,0) store_ly_year
from

/*                         Select Mackays Card sales figures by Store                   */
   (select b.area_code, b.branch_number, b.report_group,
          b.floor_code, b.report_department, b.report_subdepartment,
          b.ty_week, b.ty_period, b.ty_season, b.ty_year,
          b.ly_week, b.ly_period,b.ly_season,  b.ly_year
    from ss252_branch_mk2 b
    where b.report_group = 'C')                                      q1,

/*                     Select corresponding Area Mackays Card                           */
   (select a.area_code, a.report_group, a.report_department, a.report_subdepartment,
           a.ty_week, a.ty_period, a.ty_season, a.ty_year,
           a.ly_week, a.ly_period, a.ly_season, a.ly_year
    from ss252_area_mk2 a
    where a.report_group = 'C')                                      q2,

/*                      Select total Store sales for Week                               */
   (select p.branch_number, p.This_Year, p.Last_Year
    from ss252_print_work p
    where p.record_type = 'W'
      and p.report_group = 'A'
      and p.Floor_Code = 9
      and p.report_department = 'ZZ'
      and p.report_subdepartment = 'ZA')                             q3,

/*                      Select total Store sales for Period                             */
   (select p.branch_number, p.This_Year, p.Last_Year
    from ss252_print_work p
    where p.record_type = 'P'
      and p.report_group = 'A'
      and p.Floor_Code = 9
      and p.report_department = 'ZZ'
      and p.report_subdepartment = 'ZA')                             q4,

/*                      Select total Store sales for Season                             */
   (select p.branch_number, p.This_Year, p.Last_Year
    from ss252_print_work p
    where p.record_type = 'S'
      and p.report_group = 'A'
      and p.Floor_Code = 9
      and p.report_department = 'ZZ'
      and p.report_subdepartment = 'ZA')                             q5,
      
/*                      Select total Store sales for Year                               */
   (select p.branch_number, p.This_Year, p.Last_Year
    from ss252_print_work p
    where p.record_type = 'Y'
      and p.report_group = 'A'
      and p.Floor_Code = 9
      and p.report_department = 'ZZ'
      and p.report_subdepartment = 'ZA')                             q6

where q1.area_code = q2.area_code
  and q1.report_group = q2.report_group
  and q1.report_department = q2.report_department
  and q1.report_subdepartment = q2.report_subdepartment
  and q1.branch_number = q3.branch_number (+)
  and q1.branch_number = q4.branch_number (+)
  and q1.branch_number = q5.branch_number (+)
  and q1.branch_number = q6.branch_number (+)");
                            sqlEntity.Columns.Add(AREA_CODE, BRANCH_NUMBER, REPORT_GROUP, FLOOR_CODE, REPORT_DEPARTMENT, REPORT_SUBDEPARTMENT, BRN_TY_WEEK, BRN_TY_PER, BRN_TY_SEAS, BRN_TY_YR, BRN_LY_WEEK, BRN_LY_PER, BRN_LY_SEAS, BRN_LY_YR, A_TY_WEEK, A_TY_PER, A_TY_SEAS, A_TY_YR, A_LY_WEEK, A_LY_PER, A_LY_SEAS, A_LY_YR, STORE_TY_WEEK, STORE_TY_PER, STORE_TY_SEAS, STORE_TY_YEAR, STORE_LY_WEEK, STORE_LY_PER, STORE_LY_SEAS, STORE_LY_YEAR);
                            From = sqlEntity;
                            
                            
                            #region Columns
                            
                            // This task assembles Mackays Card SALES data by the branch at Week, Period,
                            // Season & Year-to-date levels for this year and last.  Alongside these are the
                            // corresponding overall sales for the branch.
                            
                            Columns.Add(AREA_CODE);
                            Columns.Add(BRANCH_NUMBER);
                            Columns.Add(REPORT_GROUP);
                            Columns.Add(FLOOR_CODE);
                            Columns.Add(REPORT_DEPARTMENT);
                            Columns.Add(REPORT_SUBDEPARTMENT);
                            // Mackays Card  sales by the BRANCH
                            Columns.Add(BRN_TY_WEEK);
                            Columns.Add(BRN_TY_PER);
                            Columns.Add(BRN_TY_SEAS);
                            Columns.Add(BRN_TY_YR);
                            Columns.Add(BRN_LY_WEEK);
                            Columns.Add(BRN_LY_PER);
                            Columns.Add(BRN_LY_SEAS);
                            Columns.Add(BRN_LY_YR);
                            // Mackays Card  sales at the corresponding AREA level
                            Columns.Add(A_TY_WEEK);
                            Columns.Add(A_TY_PER);
                            Columns.Add(A_TY_SEAS);
                            Columns.Add(A_TY_YR);
                            Columns.Add(A_LY_WEEK);
                            Columns.Add(A_LY_PER);
                            Columns.Add(A_LY_SEAS);
                            Columns.Add(A_LY_YR);
                            // Overall sales totals for the BRANCH
                            Columns.Add(STORE_TY_WEEK);
                            Columns.Add(STORE_TY_PER);
                            Columns.Add(STORE_TY_SEAS);
                            Columns.Add(STORE_TY_YEAR);
                            Columns.Add(STORE_LY_WEEK);
                            Columns.Add(STORE_LY_PER);
                            Columns.Add(STORE_LY_SEAS);
                            Columns.Add(STORE_LY_YEAR);
                            
                            // Temporary fields used to build Print Work records
                            Columns.Add(vTmpRecType);
                            Columns.Add(vTmpBrnMackTY);
                            Columns.Add(vTmpBrnMackLY);
                            Columns.Add(vTmpAreaMackTY);
                            Columns.Add(vTmpAreaMackLY);
                            Columns.Add(vTmpBrnSalesTY);
                            Columns.Add(vTmpBrnSalesLY);
                            #endregion
                        }
                        /// <summary>MC Build Print Work(P#62.1.3.15.3)</summary>
                        internal void Run()
                        {
                            Execute();
                        }
                        protected override void OnLoad()
                        {
                            TransactionScope = TransactionScopes.Task;
                            AllowDelete = false;
                            AllowUserAbort = true;
                        }
                        protected override void OnLeaveRow()
                        {
                            // Generate WEEK print work record
                            vTmpRecType.Value = "W";
                            vTmpBrnMackTY.Value = BRN_TY_WEEK;
                            vTmpBrnMackLY.Value = BRN_LY_WEEK;
                            vTmpAreaMackTY.Value = A_TY_WEEK;
                            vTmpAreaMackLY.Value = A_LY_WEEK;
                            vTmpBrnSalesTY.Value = STORE_TY_WEEK;
                            vTmpBrnSalesLY.Value = STORE_LY_WEEK;
                            Cached<MCWritePrintWork>().Run();
                            
                            // Generate PERIOD print work record
                            vTmpRecType.Value = "P";
                            vTmpBrnMackTY.Value = BRN_TY_PER;
                            vTmpBrnMackLY.Value = BRN_LY_PER;
                            vTmpAreaMackTY.Value = A_TY_PER;
                            vTmpAreaMackLY.Value = A_LY_PER;
                            vTmpBrnSalesTY.Value = STORE_TY_PER;
                            vTmpBrnSalesLY.Value = STORE_LY_PER;
                            Cached<MCWritePrintWork>().Run();
                            
                            // Generate SEASON print work record
                            vTmpRecType.Value = "S";
                            vTmpBrnMackTY.Value = BRN_TY_SEAS;
                            vTmpBrnMackLY.Value = BRN_LY_SEAS;
                            vTmpAreaMackTY.Value = A_TY_SEAS;
                            vTmpAreaMackLY.Value = A_LY_SEAS;
                            vTmpBrnSalesTY.Value = STORE_TY_SEAS;
                            vTmpBrnSalesLY.Value = STORE_LY_SEAS;
                            Cached<MCWritePrintWork>().Run();
                            
                            // Generate YEAR print work record
                            vTmpRecType.Value = "Y";
                            vTmpBrnMackTY.Value = BRN_TY_YR;
                            vTmpBrnMackLY.Value = BRN_LY_YR;
                            vTmpAreaMackTY.Value = A_TY_YR;
                            vTmpAreaMackLY.Value = A_LY_YR;
                            vTmpBrnSalesTY.Value = STORE_TY_YEAR;
                            vTmpBrnSalesLY.Value = STORE_LY_YEAR;
                            Cached<MCWritePrintWork>().Run();
                        }
                        
                        
                        /// <summary>MC Write Print Work(P#62.1.3.15.3.1)</summary>
                        /// <remark>Last change before Migration: 08/11/2006 10:49:45</remark>
                        internal class MCWritePrintWork : SalesAndStock.BusinessProcessBase 
                        {
                            
                            #region Models
                            /// <summary>ss252 Print Work</summary>
                            internal readonly Models.ss252PrintWork ss252PrintWork = new Models.ss252PrintWork { KeepCacheAliveAfterExit = true, AllowRowLocking = true };
                            /// <summary>ss252 Print Work</summary>
                            internal readonly Models.ss252PrintWork ss252PrintWork1 = new Models.ss252PrintWork { KeepCacheAliveAfterExit = true, AllowRowLocking = true };
                            #endregion
                            
                            internal MCBuildPrintWork _parent;
                            
                            public MCWritePrintWork(MCBuildPrintWork parent)
                            {
                                _parent = parent;
                                Title = "MC Write Print Work";
                                InitializeDataView();
                            }
                            void InitializeDataView()
                            {
                                
                                Relations.Add(ss252PrintWork, RelationType.InsertIfNotFound, 
                                		ss252PrintWork.BranchNumber.BindEqualTo(_parent.BRANCH_NUMBER).And(
                                		ss252PrintWork.RecordType.BindEqualTo(_parent.vTmpRecType)).And(
                                		ss252PrintWork.ReportGroup.BindEqualTo("C")).And(
                                		ss252PrintWork.FloorCode.BindEqualTo(() => 0)).And(
                                		ss252PrintWork.ReportDepartment.BindEqualTo(_parent.REPORT_DEPARTMENT)).And(
                                		ss252PrintWork.ReportSubDepartment.BindEqualTo(_parent.REPORT_SUBDEPARTMENT)), 
                                	ss252PrintWork.SortByss252_Print_Work_x1);
                                
                                Relations.Add(ss252PrintWork1, RelationType.InsertIfNotFound, 
                                		ss252PrintWork1.BranchNumber.BindEqualTo(_parent.BRANCH_NUMBER).And(
                                		ss252PrintWork1.RecordType.BindEqualTo(_parent.vTmpRecType)).And(
                                		ss252PrintWork1.ReportGroup.BindEqualTo("D")).And(
                                		ss252PrintWork1.FloorCode.BindEqualTo(() => 0)).And(
                                		ss252PrintWork1.ReportDepartment.BindEqualTo(_parent.REPORT_DEPARTMENT)).And(
                                		ss252PrintWork1.ReportSubDepartment.BindEqualTo(_parent.REPORT_SUBDEPARTMENT)), 
                                	ss252PrintWork1.SortByss252_Print_Work_x1);
                                
                                
                                
                                #region Columns
                                
                                // This task will generate Print Work table records for:-
                                // Mackays Card Sales (Report Group 'C') and corresponding
                                // Mackays Card % Participation (Report Group 'D')
                                
                                // Write Mackays Card Sales record
                                Columns.Add(ss252PrintWork.BranchNumber);
                                Columns.Add(ss252PrintWork.RecordType);
                                Columns.Add(ss252PrintWork.ReportGroup);
                                Columns.Add(ss252PrintWork.FloorCode);
                                Columns.Add(ss252PrintWork.ReportDepartment);
                                Columns.Add(ss252PrintWork.ReportSubDepartment);
                                Columns.Add(ss252PrintWork.Description);
                                Columns.Add(ss252PrintWork.ThisYear);
                                Columns.Add(ss252PrintWork.Budget);
                                Columns.Add(ss252PrintWork.LastYear);
                                Columns.Add(ss252PrintWork.PCentToBudget);
                                Columns.Add(ss252PrintWork.PCentToLY);
                                Columns.Add(ss252PrintWork.AreaPCentToLY);
                                Columns.Add(ss252PrintWork.PCentVarToArea);
                                Columns.Add(ss252PrintWork.PCentSalesByFloor);
                                Columns.Add(ss252PrintWork.PCentBayByFloor);
                                Columns.Add(ss252PrintWork.SalesPerBay);
                                Columns.Add(ss252PrintWork.AreaSalesPerBay);
                                Columns.Add(ss252PrintWork.BaySalesVarToArea);
                                Columns.Add(ss252PrintWork.BayPCentVarToArea);
                                Columns.Add(ss252PrintWork.BayCount);
                                
                                // Write Mackays Card % Participation record.
                                Columns.Add(ss252PrintWork1.BranchNumber);
                                Columns.Add(ss252PrintWork1.RecordType);
                                Columns.Add(ss252PrintWork1.ReportGroup);
                                Columns.Add(ss252PrintWork1.FloorCode);
                                Columns.Add(ss252PrintWork1.ReportDepartment);
                                Columns.Add(ss252PrintWork1.ReportSubDepartment);
                                Columns.Add(ss252PrintWork1.Description);
                                Columns.Add(ss252PrintWork1.ThisYear);
                                Columns.Add(ss252PrintWork1.LastYear);
                                #endregion
                            }
                            /// <summary>MC Write Print Work(P#62.1.3.15.3.1)</summary>
                            internal void Run()
                            {
                                Execute();
                            }
                            protected override void OnLoad()
                            {
                                Exit(ExitTiming.AfterRow);
                                KeepChildRelationCacheAlive = true;
                                AllowUserAbort = true;
                            }
                            protected override void OnEnterRow()
                            {
                                ss252PrintWork.Budget.Value = 0;
                                ss252PrintWork.PCentToBudget.Value = 0;
                                ss252PrintWork.PCentToLY.Value = 0;
                                ss252PrintWork.AreaPCentToLY.Value = 0;
                                ss252PrintWork.PCentVarToArea.Value = 0;
                                ss252PrintWork.PCentSalesByFloor.Value = 0;
                                ss252PrintWork.PCentBayByFloor.Value = 0;
                                ss252PrintWork.SalesPerBay.Value = 0;
                                ss252PrintWork.AreaSalesPerBay.Value = 0;
                                ss252PrintWork.BaySalesVarToArea.Value = 0;
                                ss252PrintWork.BayPCentVarToArea.Value = 0;
                            }
                            protected override void OnLeaveRow()
                            {
                                // Init Mackays Card Sales values
                                ss252PrintWork.Description.Value = "Mackays Card";
                                ss252PrintWork.ThisYear.Value = _parent.vTmpBrnMackTY;
                                ss252PrintWork.LastYear.Value = _parent.vTmpBrnMackLY;
                                if(_parent.vTmpBrnMackTY != 0)
                                {
                                    ss252PrintWork.PCentToLY.Value = u.If(u.Abs((_parent.vTmpBrnMackTY - _parent.vTmpBrnMackLY) * 100 / _parent.vTmpBrnMackLY) > 999.99, 999.99, u.Round((_parent.vTmpBrnMackTY - _parent.vTmpBrnMackLY) * 100 / _parent.vTmpBrnMackLY, 3, 2));
                                }
                                if(_parent.vTmpBrnMackTY != 0)
                                {
                                    ss252PrintWork.AreaPCentToLY.Value = u.If(u.Abs((_parent.vTmpAreaMackTY - _parent.vTmpAreaMackLY) * 100 / _parent.vTmpAreaMackLY) > 999.99, 999.99, u.Round((_parent.vTmpAreaMackTY - _parent.vTmpAreaMackLY) * 100 / _parent.vTmpAreaMackLY, 3, 2));
                                }
                                ss252PrintWork.PCentVarToArea.Value = u.If(ss252PrintWork.PCentToLY == 999.99 || ss252PrintWork.AreaPCentToLY == 999.99, 0, u.If(u.Abs(ss252PrintWork.PCentToLY - ss252PrintWork.AreaPCentToLY) > 999.99, 999.99, ss252PrintWork.PCentToLY - ss252PrintWork.AreaPCentToLY));
                                // Init Mackays Card Participation values
                                ss252PrintWork1.Description.Value = "% Participation";
                                ss252PrintWork1.ThisYear.Value = u.Round(ss252PrintWork.ThisYear * 100 / _parent.vTmpBrnSalesTY, 3, 1);
                                ss252PrintWork1.LastYear.Value = u.Round(ss252PrintWork.LastYear * 100 / _parent.vTmpBrnSalesLY, 3, 1);
                                
                                // DEBUG
                                if(u.Or(u.Or(u.Equals(_parent.BRANCH_NUMBER, 2), u.Equals(_parent.BRANCH_NUMBER, 9)), u.Equals(_parent.BRANCH_NUMBER, 29)))
                                {
                                    new MCDEBUG(this).Run();
                                }
                            }
                            
                            
                            /// <summary>MC DEBUG(P#62.1.3.15.3.1.1)</summary>
                            /// <remark>Last change before Migration: 03/11/2006 16:21:45</remark>
                            internal class MCDEBUG : SalesAndStock.BusinessProcessBase 
                            {
                                internal MCWritePrintWork _parent;
                                
                                public MCDEBUG(MCWritePrintWork parent)
                                {
                                    _parent = parent;
                                    Title = "MC DEBUG";
                                    ConfirmExecution = true;
                                    InitializeDataView();
                                }
                                void InitializeDataView()
                                {
                                    
                                    
                                }
                                /// <summary>MC DEBUG(P#62.1.3.15.3.1.1)</summary>
                                internal void Run()
                                {
                                    Execute();
                                }
                                protected override void OnLoad()
                                {
                                    Exit(ExitTiming.AfterRow);
                                    Activity = Activities.Browse;
                                    AllowUserAbort = true;
                                    if(NewViewRequired)
                                    {
                                        View = ()=> new Views.DiarySlsReptSs252MCDEBUG(this);
                                    }
                                }
                                
                                
                            }
                        }
                    }
                }
            }
        }
        /// <summary>SQL Xref Dummy(P#62.2)</summary>
        /// <remark>Last change before Migration: 08/11/2006 10:55:04</remark>
        internal class SQLXrefDummy : SalesAndStock.UIControllerBase
        {

            #region Models
            /// <summary>Branch</summary>
            readonly Models.Branch Branch = new Models.Branch { ReadOnly = true };
            /// <summary>Section Conversion</summary>
            readonly Models.SectionConversion SectionConversion = new Models.SectionConversion { ReadOnly = true };
            /// <summary>Branch Attributes</summary>
            readonly Models.BranchAttributes BranchAttributes = new Models.BranchAttributes { ReadOnly = true };
            /// <summary>SLA_Tenders</summary>
            readonly Models.SLA_Tenders SLA_Tenders = new Models.SLA_Tenders { ReadOnly = true };
            /// <summary>SLA_Details</summary>
            readonly Models.SLA_Details SLA_Details = new Models.SLA_Details { Cached = false, ReadOnly = true };
            /// <summary>SLA_SectSales</summary>
            readonly Models.SLA_SectSales SLA_SectSales = new Models.SLA_SectSales { ReadOnly = true };
            /// <summary>Branch Bays</summary>
            readonly Models.BranchBays BranchBays = new Models.BranchBays { ReadOnly = true };
            /// <summary>Dept Analysis Grps</summary>
            readonly Models.DeptAnalysisGrps DeptAnalysisGrps = new Models.DeptAnalysisGrps { ReadOnly = true };
            /// <summary>Analysis Group 1 Depts</summary>
            readonly Models.AnalysisGroup1Depts AnalysisGroup1Depts = new Models.AnalysisGroup1Depts { ReadOnly = true };
            /// <summary>BrnDptSlsBud</summary>
            readonly Models.BrnDptSlsBud BrnDptSlsBud = new Models.BrnDptSlsBud { ReadOnly = true };
            /// <summary>Credit Card Details</summary>
            readonly Models.CreditCardDetails CreditCardDetails = new Models.CreditCardDetails { ReadOnly = true };
            /// <summary>ss252 Hours Work</summary>
            readonly Models.ss252HoursWork ss252HoursWork = new Models.ss252HoursWork { AllowRowLocking = true };
            /// <summary>Diary Work1</summary>
            readonly Models.DiaryWork1 DiaryWork1 = new Models.DiaryWork1 { AllowRowLocking = true };
            /// <summary>ss252 Print Work</summary>
            readonly Models.ss252PrintWork ss252PrintWork = new Models.ss252PrintWork { AllowRowLocking = true };
            /// <summary>POS Trn Wk Summ</summary>
            readonly Models.POSTrnWkSumm POSTrnWkSumm = new Models.POSTrnWkSumm { ReadOnly = true };
            /// <summary>ss252 Area Work Mk2</summary>
            readonly Models.ss252AreaWorkMk2 ss252AreaWorkMk2 = new Models.ss252AreaWorkMk2 { Cached = false, AllowRowLocking = true };
            /// <summary>ss252 Branch Work Mk2</summary>
            readonly Models.ss252BranchWorkMk2 ss252BranchWorkMk2 = new Models.ss252BranchWorkMk2 { Cached = false, AllowRowLocking = true };
            #endregion

            public SQLXrefDummy()
            {
                Title = "SQL Xref Dummy";
                Entities.Add(Branch);
                Entities.Add(SectionConversion);
                Entities.Add(BranchAttributes);
                Entities.Add(SLA_Tenders);
                Entities.Add(SLA_Details);
                Entities.Add(SLA_SectSales);
                Entities.Add(BranchBays);
                Entities.Add(DeptAnalysisGrps);
                Entities.Add(AnalysisGroup1Depts);
                Entities.Add(BrnDptSlsBud);
                Entities.Add(CreditCardDetails);
                Entities.Add(ss252HoursWork);
                Entities.Add(DiaryWork1);
                Entities.Add(ss252PrintWork);
                Entities.Add(POSTrnWkSumm);
                Entities.Add(ss252AreaWorkMk2);
                Entities.Add(ss252BranchWorkMk2);
                InitializeDataView();
            }
            void InitializeDataView()
            {


                #region Columns

                // This is a dummy task listing tables accessed by SQL in other tasks.

                // It is set up so that the tables involved will appear in any XREF
                // procedures whether or not they are accessed directly by MAGIC.
                #endregion
            }
            protected override void OnLoad()
            {
                OnDatabaseErrorRetry = false;
                SwitchToInsertWhenNoRows = true;
                View = () => new Views.DiarySlsReptSs252SQLXrefDummy(this);
            }
        }
    }

}
