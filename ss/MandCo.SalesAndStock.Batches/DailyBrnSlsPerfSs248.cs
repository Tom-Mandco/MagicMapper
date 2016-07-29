using Firefly.Box;
using ENV.Data;
using ENV;
using Firefly.Box.Data.DataProvider;
using ENV.IO;
using Firefly.Box.Flow;
namespace MandCo.SalesAndStock.Batches
{
    /// <summary>Daily Brn Sls Perf (ss248)(P#46)</summary>
    /// <remark>Last change before Migration: 21/06/2011 11:57:04</remark>
    public class DailyBrnSlsPerfSs248 : SalesAndStock.BusinessProcessBase 
    {
        
        #region Models
        /// <summary>Date</summary>
        readonly Models.Date1 Date1 = new Models.Date1 { ReadOnly = true };
        /// <summary>Date</summary>
        readonly Models.Date1 Date11 = new Models.Date1 { ReadOnly = true };
        #endregion
        
        #region Columns
        /// <summary>p:YYYYWW</summary>
        readonly TextColumn pYYYYWW = new TextColumn("p:YYYYWW", "6");
        /// <summary>v:ReqYYYYWW</summary>
        readonly NumberColumn vReqYYYYWW = new NumberColumn("v:ReqYYYYWW", "6P0");
        /// <summary>v:WeekFound</summary>
        readonly BoolColumn vWeekFound = new BoolColumn("v:WeekFound");
        /// <summary>v:WeekStartTY</summary>
        readonly DateColumn vWeekStartTY = new DateColumn("v:WeekStartTY");
        /// <summary>v:UpToDateTY</summary>
        readonly DateColumn vUpToDateTY = new DateColumn("v:UpToDateTY");
        /// <summary>v:WeekStartPY</summary>
        readonly DateColumn vWeekStartPY = new DateColumn("v:WeekStartPY");
        /// <summary>v:WeekEndPY</summary>
        readonly DateColumn vWeekEndPY = new DateColumn("v:WeekEndPY");
        /// <summary>v:UpToDatePY</summary>
        readonly DateColumn vUpToDatePY = new DateColumn("v:UpToDatePY");
        /// <summary>v:Days Diff</summary>
        readonly NumberColumn vDaysDiff = new NumberColumn("v:Days Diff", "9");
        /// <summary>v:Hdr Week</summary>
        readonly TextColumn vHdrWeek = new TextColumn("v:Hdr Week", "30");
        /// <summary>v:Hdr Branch</summary>
        readonly TextColumn vHdrBranch = new TextColumn("v:Hdr Branch", "55");
        /// <summary>v:Hdr Date</summary>
        readonly TextColumn vHdrDate = new TextColumn("v:Hdr Date", "19");
        /// <summary>v:Current WeekNo</summary>
        readonly NumberColumn vCurrentWeekNo = new NumberColumn("v:Current WeekNo", "6");
        /// <summary>v:ParamError</summary>
        readonly TextColumn vParamError = new TextColumn("v:ParamError", "U");
        #endregion
        
        public DailyBrnSlsPerfSs248()
        {
            Title = "Daily Brn Sls Perf (ss248)";
            InitializeDataView();
        }
        void InitializeDataView()
        {
            
            Relations.Add(Date1, 
            		Date1.CenturyWeek.IsEqualTo(vReqYYYYWW), 
            	Date1.SortByREF_Date_X3);
            
            Relations.Add(Date11, 
            		Date11.WeekEndingDate.IsGreaterOrEqualTo(() => Date.Now), 
            	Date11.SortByREF_Date_X2);
            
            
            
            #region Columns
            
            Columns.Add(pYYYYWW);
            Columns.Add(vReqYYYYWW).BindValue(() => u.Val(pYYYYWW, "6P0"));
            // Get W/E date for requested week
            Columns.Add(vWeekFound);
            Relations[Date1].NotifyRowWasFoundTo(vWeekFound);
            Columns.Add(Date1.CenturyWeek);
            Columns.Add(Date1.WeekEndingDate);
            
            Columns.Add(vWeekStartTY).BindValue(() => u.AddDate(Date1.WeekEndingDate, 0, 0, -(6)));
            Columns.Add(vUpToDateTY);
            // Corresponding dates for previous year
            Columns.Add(vWeekStartPY).BindValue(() => u.AddDate(vWeekStartTY, 0, 0, -(364)));
            Columns.Add(vWeekEndPY);
            Columns.Add(vUpToDatePY);
            
            Columns.Add(vDaysDiff);
            // HTML report heading fields
            Columns.Add(vHdrWeek).BindValue(() => "Week " + u.Mid(pYYYYWW, 5, 2) + " - Year " + u.Left(pYYYYWW, 4));
            Columns.Add(vHdrBranch);
            Columns.Add(vHdrDate).BindValue(() => u.DStr(Date.Now, "DD/MM/YYYY") + " at " + u.TStr(Time.Now, "HH:MM"));
            
            // Fetch current week number
            Columns.Add(Date11.WeekEndingDate);
            Columns.Add(Date11.CenturyWeek);
            Columns.Add(vCurrentWeekNo).BindValue(Date11.CenturyWeek);
            
            Columns.Add(vParamError);
            #endregion
        }
        protected override void OnLoad()
        {
            Exit(ExitTiming.AfterRow);
        }
        protected override void OnEnterRow()
        {
            Message.ShowWarningInStatusBar("ss248");
            Message.ShowWarningInStatusBar("ss248   Program starts");
            pYYYYWW.Value = u.IniGet("[SS]Option");
            if(u.Len(u.Trim(pYYYYWW)) != 0)
            {
                Message.ShowWarningInStatusBar("ss248   Week No. requested = " + pYYYYWW);
            }
            // If no parameter received, determine week to be processed
            if(u.Len(u.Trim(pYYYYWW)) == 0)
            {
                Cached<DetermineWeek>().Run();
            }
            
        }
        protected override void OnLeaveRow()
        {
            // Check parameter week is 6 digits long
            if(u.Len(u.Trim(pYYYYWW)) != 6 || u.Upper(pYYYYWW) != u.Lower(pYYYYWW))
            {
                Message.ShowWarningInStatusBar("ss248   Invalid week parameter - should be in format yyyyww");
                vParamError.Value = "Y";
            }
            // Is week no. Valid  -  is it on Dates tables?
            if(vParamError != "Y" && u.Not(vWeekFound))
            {
                Message.ShowWarningInStatusBar("ss248   Requested week not on Dates table");
                vParamError.Value = "Y";
            }
            // Requested week > current week
            if(vParamError != "Y" && Date.Now < vWeekStartTY)
            {
                Message.ShowWarningInStatusBar("ss248   Requested week is later than the current week");
                vParamError.Value = "Y";
            }
            // ERROR has occurred
            if(vParamError == "Y")
            {
                Message.ShowWarningInStatusBar("ss248 " + "             ABNORMAL TERMINATION");
            }
            
            // Parameter OK
            if(vParamError != "Y")
            {
                vUpToDateTY.Value = Date1.WeekEndingDate;
                vWeekEndPY.Value = u.AddDate(Date1.WeekEndingDate, 0, 0, -(364));
                // If running for current week and current date is before end  of week, modify
                // UpToDate TY to today's date if time is between 18:00 and 24:00. Otherwise, set
                // it to yesterday's date.
                if(vReqYYYYWW == vCurrentWeekNo && Date.Now <= Date1.WeekEndingDate)
                {
                    vUpToDateTY.Value = u.If(u.Range(u.TStr(Time.Now, "HH:MM:SS"), "18:00:00", "24:00:00"), Date.Now, u.AddDate(Date.Now, 0, 0, -(1)));
                }
                vUpToDatePY.Value = u.AddDate(vUpToDateTY, 0, 0, -(364));
                // A value of 6 calculated in the following field denotes that the run covers the
                // whole week.
                vDaysDiff.Value = vUpToDateTY - vWeekStartTY;
                new Control(this).Run();
            }
        }
        protected override void OnEnd()
        {
            Message.ShowWarningInStatusBar("ss248   Program ends");
            Message.ShowWarningInStatusBar("ss248");
        }
        
        
        /// <summary>Control(P#46.1)</summary>
        /// <remark>Last change before Migration: 27/05/2011 11:45:41</remark>
        internal class Control : SalesAndStock.BusinessProcessBase 
        {
            
            #region Columns
            /// <summary>v:MsgToUser</summary>
            readonly TextColumn vMsgToUser = new TextColumn("v:MsgToUser", "X70");
            /// <summary>v:WebDir</summary>
            readonly TextColumn vWebDir = new TextColumn("v:WebDir", "60");
            /// <summary>v:BaseDir</summary>
            readonly TextColumn vBaseDir = new TextColumn("v:BaseDir", "120");
            /// <summary>v:TargetFile</summary>
            readonly TextColumn vTargetFile = new TextColumn("v:TargetFile", "120");
            /// <summary>v:From Applic ID</summary>
            readonly TextColumn vFromApplicID = new TextColumn("v:From Applic ID", "12");
            /// <summary>v:To Applic ID</summary>
            readonly TextColumn vToApplicID = new TextColumn("v:To Applic ID", "12");
            #endregion
            
            DailyBrnSlsPerfSs248 _parent;
            
            public Control(DailyBrnSlsPerfSs248 parent)
            {
                _parent = parent;
                Title = "Control";
                InitializeDataView();
            }
            void InitializeDataView()
            {
                
                
                #region Columns
                
                Columns.Add(vMsgToUser);
                
                Columns.Add(vWebDir).BindValue(() => u.IniGet("[MAGIC_LOGICAL_NAMES]web"));
                Columns.Add(vBaseDir).BindValue(() => u.Trim(vWebDir) + "/branch/");
                Columns.Add(vTargetFile);
                // The following will be used to force SQL to use index on CCC_Detail for
                // Instant Cred counts and will be initialised to take account of late
                // postings to credit cards.
                Columns.Add(vFromApplicID);
                Columns.Add(vToApplicID);
                #endregion
            }
            /// <summary>Control(P#46.1)</summary>
            internal void Run()
            {
                Execute();
            }
            protected override void OnLoad()
            {
                Exit(ExitTiming.AfterRow);
                if(NewViewRequired)
                {
                    View = ()=> new Views.DailyBrnSlsPerfSs248Control(this);
                }
            }
            protected override void OnStart()
            {
                u.SetCrsr(4);
            }
            protected override void OnLeaveRow()
            {
                // Clear work table
                vMsgToUser.Value = "Clearing work table";
                u.Delay(1);
                Message.ShowWarningInStatusBar("ss248   Clearing work tables");
                Cached<TruncateWork1>().Run();
                Cached<DeleteDiaryWork1>().Run();
                
                // Get Sales data for this week
                Message.ShowWarningInStatusBar("ss248   Collating Sales data");
                vMsgToUser.Value = "Collating relevant Sales data";
                u.Delay(1);
                Cached<FetchTYData>().Run();
                // Get Sales data for corresponding week last year
                Cached<FetchPYData>().Run();
                // Get Mackays Card data for this week
                Message.ShowWarningInStatusBar("ss248   Collating Mackays Card details");
                vMsgToUser.Value = "Collating relevant Mackays Card details";
                u.Delay(1);
                Cached<FetchTYMackCard>().Run();
                // Get Mackays Card data for corresponding week last year
                Cached<FetchPYMackCard>().Run();
                // Get Instant Credit data (accounts opened) for this week
                Message.ShowWarningInStatusBar("ss248   Collating Instant Credit details");
                vMsgToUser.Value = "Collating relevant Instant Credit details";
                u.Delay(1);
                vFromApplicID.Value = "RRS" + u.DStr(_parent.vWeekStartTY, "YYYYMMDD") + "0";
                vToApplicID.Value = "RRS" + u.DStr(u.AddDate(_parent.vUpToDateTY, 0, 0, 7), "YYYYMMDD") + "9";
                Cached<FetchTYInstCred>().Run();
                // Get Instant Credit data (accs opened) for corresponding week last year
                vFromApplicID.Value = "RRS" + u.DStr(_parent.vWeekStartPY, "YYYYMMDD") + "0";
                vToApplicID.Value = "RRS" + u.DStr(u.AddDate(_parent.vWeekEndPY, 0, 0, 7), "YYYYMMDD") + "9";
                Cached<FetchPYInstCred>().Run();
                // Get Concession data for this year & last year
                Cached<FetchConcessions>().Run();
                
                // Identify/process branches in the run
                Message.ShowWarningInStatusBar("ss248   Generating branch reports");
                vMsgToUser.Value = "Generating branch reports";
                u.Delay(1);
                Cached<SetUpBranchList>().Run();
                Cached<ProcessBranches>().Run();
            }
            protected override void OnEnd()
            {
                u.SetCrsr(1);
            }
            
            #region Expressions
            internal Text Exp_19()
            {
                return vMsgToUser;
            }
            #endregion
            
            
            /// <summary>Truncate Work1(P#46.1.1)</summary>
            /// <remark>Last change before Migration: 16/05/2006 10:18:06</remark>
            class TruncateWork1 : SalesAndStock.BusinessProcessBase 
            {
                
                #region Models
                DynamicSQLEntity sqlEntity;
                #endregion
                
                public TruncateWork1()
                {
                    Title = "Truncate Work1";
                    InitializeDataView();
                }
                void InitializeDataView()
                {
                    sqlEntity = new DynamicSQLEntity(Models.DataSources.SS, "TRUNCATE TABLE ss_ss248_work1");
                    From = sqlEntity;
                    
                    
                    #region Columns
                    
                    // SQL task to truncate table ss_ss248_work1
                    #endregion
                }
                /// <summary>Truncate Work1(P#46.1.1)</summary>
                internal void Run()
                {
                    Execute();
                }
                protected override void OnLoad()
                {
                    Exit(ExitTiming.AfterRow);
                    TransactionScope = TransactionScopes.Task;
                    AllowInsert = false;
                }
                
                
            }
            /// <summary>Fetch TY Data(P#46.1.2)</summary>
            /// <remark>Last change before Migration: 20/04/2004 11:38:57</remark>
            class FetchTYData : SalesAndStock.BusinessProcessBase 
            {
                
                #region Models
                DynamicSQLEntity sqlEntity;
                /// <summary>ss248 Work1</summary>
                readonly Models.ss248Work1 ss248Work1 = new Models.ss248Work1 { KeepCacheAliveAfterExit = true, AllowRowLocking = true };
                #endregion
                
                #region Columns
                /// <summary>sql:BRANCH_NUMBER</summary>
                readonly NumberColumn sqlBRANCH_NUMBER = new NumberColumn("sql:BRANCH_NUMBER", "N4")
                {
                	AllowNull = true
                };
                /// <summary>sql:RPT_DEPARTMENT</summary>
                readonly TextColumn sqlRPT_DEPARTMENT = new TextColumn("sql:RPT_DEPARTMENT", "2")
                {
                	AllowNull = true
                };
                /// <summary>sql:RPT_SUBDEPARTMENT</summary>
                readonly TextColumn sqlRPT_SUBDEPARTMENT = new TextColumn("sql:RPT_SUBDEPARTMENT", "2")
                {
                	AllowNull = true
                };
                /// <summary>sql:TRANSACTION_DATE</summary>
                readonly DateColumn sqlTRANSACTION_DATE = new DateColumn("sql:TRANSACTION_DATE")
                {
                	AllowNull = true,
                	Storage = new ENV.Data.Storage.DateTimeDateStorage()
                };
                /// <summary>sql:SECT_VAL</summary>
                readonly NumberColumn sqlSECT_VAL = new NumberColumn("sql:SECT_VAL", "9.2")
                {
                	AllowNull = true
                };
                /// <summary>v:Skip</summary>
                readonly NumberColumn vSkip = new NumberColumn("v:Skip", "1");
                #endregion
                
                Control _parent;
                
                public FetchTYData(Control parent)
                {
                    _parent = parent;
                    Title = "Fetch TY Data";
                    InitializeDataView();
                }
                void InitializeDataView()
                {
                    sqlEntity = new DynamicSQLEntity(Models.DataSources.SS, 
@"select s.branch_number, nvl(a.group_1_dept,'X'), nvl(a.group_1_subdepartment,'X'),
       s.transaction_date, sum(nvl(s.section_value,0)) sect_val
from sla_sectsales s, ref_sectconv c, ref_dpt_analysis_grps a
where s.transaction_date between ':1' and ':2'
  and s.pdm_department_code = c.department_code
  and s.pdm_section_code = c.section_code
  and trim(c.dss_department) <> '6'
  and c.dss_department = a.department_code (+)
  and c.dss_subdepartment = a.subdepartment_code (+)
group by s.branch_number, nvl(a.group_1_dept,'X'),
         nvl(a.group_1_subdepartment,'X'), s.transaction_date
order by s.branch_number, nvl(a.group_1_dept,'X'),
         nvl(a.group_1_subdepartment,'X'), s.transaction_date");
                    sqlEntity.AddParameter(() => u.DStr(_parent._parent.vWeekStartTY, "DD-MMM-YYYY")); //:1;
                    sqlEntity.AddParameter(() => u.DStr(_parent._parent.vUpToDateTY, "DD-MMM-YYYY")); //:2;
                    sqlEntity.Columns.Add(sqlBRANCH_NUMBER, sqlRPT_DEPARTMENT, sqlRPT_SUBDEPARTMENT, sqlTRANSACTION_DATE, sqlSECT_VAL);
                    From = sqlEntity;
                    
                    Relations.Add(ss248Work1, RelationType.InsertIfNotFound, 
                    		ss248Work1.FinancialWeek.BindEqualTo(_parent._parent.vReqYYYYWW).And(
                    		ss248Work1.BranchNumber.BindEqualTo(sqlBRANCH_NUMBER)).And(
                    		ss248Work1.ReportGroup.BindEqualTo("A")).And(
                    		ss248Work1.DeptReportSequence.BindEqualTo(1)).And(
                    		ss248Work1.Department1.BindEqualTo(sqlRPT_DEPARTMENT)).And(
                    		ss248Work1.SubDepartment1.BindEqualTo(sqlRPT_SUBDEPARTMENT)), 
                    	ss248Work1.SortByss_ss248_Work1_X1);
                    
                    
                    
                    #region Columns
                    
                    // Data returned by SQL
                    Columns.Add(sqlBRANCH_NUMBER);
                    Columns.Add(sqlRPT_DEPARTMENT);
                    Columns.Add(sqlRPT_SUBDEPARTMENT);
                    Columns.Add(sqlTRANSACTION_DATE);
                    Columns.Add(sqlSECT_VAL);
                    // Create/Update Sales record in work table
                    Columns.Add(ss248Work1.FinancialWeek);
                    Columns.Add(ss248Work1.BranchNumber);
                    Columns.Add(ss248Work1.ReportGroup);
                    Columns.Add(ss248Work1.DeptReportSequence);
                    Columns.Add(ss248Work1.Department1);
                    Columns.Add(ss248Work1.SubDepartment1);
                    Columns.Add(ss248Work1.TYSaturday);
                    Columns.Add(ss248Work1.TYSunday);
                    Columns.Add(ss248Work1.TYMonday);
                    Columns.Add(ss248Work1.TYTuesday);
                    Columns.Add(ss248Work1.TYWednesday);
                    Columns.Add(ss248Work1.TYThursday);
                    Columns.Add(ss248Work1.TYFriday);
                    Columns.Add(ss248Work1.TYWeekToDate);
                    Columns.Add(ss248Work1.LYSaturday);
                    Columns.Add(ss248Work1.LYSunday);
                    Columns.Add(ss248Work1.LYMonday);
                    Columns.Add(ss248Work1.LYTuesday);
                    Columns.Add(ss248Work1.LYWednesday);
                    Columns.Add(ss248Work1.LYThursday);
                    Columns.Add(ss248Work1.LYFriday);
                    Columns.Add(ss248Work1.LYWeekToDate);
                    
                    Columns.Add(vSkip);
                    #endregion
                }
                /// <summary>Fetch TY Data(P#46.1.2)</summary>
                internal void Run()
                {
                    Execute();
                }
                protected override void OnLoad()
                {
                    RowLocking = LockingStrategy.OnRowLoading;
                    TransactionScope = TransactionScopes.Task;
                    KeepChildRelationCacheAlive = true;
                    AllowDelete = false;
                    AllowUserAbort = true;
                }
                protected override void OnLeaveRow()
                {
                    // Init SKIP from day_of_week to allow VARSET logic
                    vSkip.Value = u.If(u.Equals(u.DOW(sqlTRANSACTION_DATE), 7), 0, u.DOW(sqlTRANSACTION_DATE));
                    u.VarSet(u.IndexOf(ss248Work1.TYSaturday) + vSkip, u.CastToNumber(u.VarCurr(u.IndexOf(ss248Work1.TYSaturday) + vSkip)) + sqlSECT_VAL);
                    ss248Work1.TYWeekToDate.Value += sqlSECT_VAL;
                }
                
                
            }
            /// <summary>Fetch PY Data(P#46.1.3)</summary>
            /// <remark>Last change before Migration: 20/04/2004 11:40:31</remark>
            class FetchPYData : SalesAndStock.BusinessProcessBase 
            {
                
                #region Models
                DynamicSQLEntity sqlEntity;
                /// <summary>ss248 Work1</summary>
                readonly Models.ss248Work1 ss248Work1 = new Models.ss248Work1 { KeepCacheAliveAfterExit = true, AllowRowLocking = true };
                #endregion
                
                #region Columns
                /// <summary>sql:BRANCH_NUMBER</summary>
                readonly NumberColumn sqlBRANCH_NUMBER = new NumberColumn("sql:BRANCH_NUMBER", "N4")
                {
                	AllowNull = true
                };
                /// <summary>sql:RPT_DEPARTMENT</summary>
                readonly TextColumn sqlRPT_DEPARTMENT = new TextColumn("sql:RPT_DEPARTMENT", "2")
                {
                	AllowNull = true
                };
                /// <summary>sql:RPT_SUBDEPARTMENT</summary>
                readonly TextColumn sqlRPT_SUBDEPARTMENT = new TextColumn("sql:RPT_SUBDEPARTMENT", "2")
                {
                	AllowNull = true
                };
                /// <summary>sql:TRANSACTION_DATE</summary>
                readonly DateColumn sqlTRANSACTION_DATE = new DateColumn("sql:TRANSACTION_DATE")
                {
                	AllowNull = true,
                	Storage = new ENV.Data.Storage.DateTimeDateStorage()
                };
                /// <summary>sql:SECT_VAL</summary>
                readonly NumberColumn sqlSECT_VAL = new NumberColumn("sql:SECT_VAL", "9.2")
                {
                	AllowNull = true
                };
                /// <summary>v:Skip</summary>
                readonly NumberColumn vSkip = new NumberColumn("v:Skip", "1");
                #endregion
                
                Control _parent;
                
                public FetchPYData(Control parent)
                {
                    _parent = parent;
                    Title = "Fetch PY Data";
                    InitializeDataView();
                }
                void InitializeDataView()
                {
                    sqlEntity = new DynamicSQLEntity(Models.DataSources.SS, 
@"select s.branch_number, nvl(a.group_1_dept,'X'), nvl(a.group_1_subdepartment,'X'),
       s.transaction_date, sum(nvl(s.section_value,0)) sect_val
from sla_sectsales s, ref_sectconv c, ref_dpt_analysis_grps a
where s.transaction_date between ':1' and ':2'
  and s.pdm_department_code = c.department_code
  and s.pdm_section_code = c.section_code
  and trim(c.dss_department) <> '6'
  and c.dss_department = a.department_code (+)
  and c.dss_subdepartment = a.subdepartment_code (+)
  and exists (select 'X' from ss_ss248_work1 w
              where w.financial_week = :3
                and w.branch_number = s.branch_number)
group by s.branch_number, nvl(a.group_1_dept,'X'),
         nvl(a.group_1_subdepartment,'X'), s.transaction_date
order by s.branch_number, nvl(a.group_1_dept,'X'),
         nvl(a.group_1_subdepartment,'X'), s.transaction_date");
                    sqlEntity.AddParameter(() => u.DStr(_parent._parent.vWeekStartPY, "DD-MMM-YYYY")); //:1;
                    sqlEntity.AddParameter(() => u.DStr(_parent._parent.vWeekEndPY, "DD-MMM-YYYY")); //:2;
                    sqlEntity.AddParameter(_parent._parent.vReqYYYYWW); //:3;
                    sqlEntity.Columns.Add(sqlBRANCH_NUMBER, sqlRPT_DEPARTMENT, sqlRPT_SUBDEPARTMENT, sqlTRANSACTION_DATE, sqlSECT_VAL);
                    From = sqlEntity;
                    
                    Relations.Add(ss248Work1, RelationType.InsertIfNotFound, 
                    		ss248Work1.FinancialWeek.BindEqualTo(_parent._parent.vReqYYYYWW).And(
                    		ss248Work1.BranchNumber.BindEqualTo(sqlBRANCH_NUMBER)).And(
                    		ss248Work1.ReportGroup.BindEqualTo("A")).And(
                    		ss248Work1.DeptReportSequence.BindEqualTo(1)).And(
                    		ss248Work1.Department1.BindEqualTo(sqlRPT_DEPARTMENT)).And(
                    		ss248Work1.SubDepartment1.BindEqualTo(sqlRPT_SUBDEPARTMENT)), 
                    	ss248Work1.SortByss_ss248_Work1_X1);
                    
                    
                    
                    #region Columns
                    
                    // Data returned by SQL
                    Columns.Add(sqlBRANCH_NUMBER);
                    Columns.Add(sqlRPT_DEPARTMENT);
                    Columns.Add(sqlRPT_SUBDEPARTMENT);
                    Columns.Add(sqlTRANSACTION_DATE);
                    Columns.Add(sqlSECT_VAL);
                    // Update LY Sales to record in work table
                    Columns.Add(ss248Work1.FinancialWeek);
                    Columns.Add(ss248Work1.BranchNumber);
                    Columns.Add(ss248Work1.ReportGroup);
                    Columns.Add(ss248Work1.DeptReportSequence);
                    Columns.Add(ss248Work1.Department1);
                    Columns.Add(ss248Work1.SubDepartment1);
                    Columns.Add(ss248Work1.TYSaturday);
                    Columns.Add(ss248Work1.TYSunday);
                    Columns.Add(ss248Work1.TYMonday);
                    Columns.Add(ss248Work1.TYTuesday);
                    Columns.Add(ss248Work1.TYWednesday);
                    Columns.Add(ss248Work1.TYThursday);
                    Columns.Add(ss248Work1.TYFriday);
                    Columns.Add(ss248Work1.TYWeekToDate);
                    Columns.Add(ss248Work1.LYSaturday);
                    Columns.Add(ss248Work1.LYSunday);
                    Columns.Add(ss248Work1.LYMonday);
                    Columns.Add(ss248Work1.LYTuesday);
                    Columns.Add(ss248Work1.LYWednesday);
                    Columns.Add(ss248Work1.LYThursday);
                    Columns.Add(ss248Work1.LYFriday);
                    Columns.Add(ss248Work1.LYWeekToDate);
                    
                    Columns.Add(vSkip);
                    #endregion
                }
                /// <summary>Fetch PY Data(P#46.1.3)</summary>
                internal void Run()
                {
                    Execute();
                }
                protected override void OnLoad()
                {
                    RowLocking = LockingStrategy.OnRowLoading;
                    TransactionScope = TransactionScopes.Task;
                    KeepChildRelationCacheAlive = true;
                    AllowDelete = false;
                    AllowUserAbort = true;
                }
                protected override void OnLeaveRow()
                {
                    // Init SKIP from day_of_week to allow VARSET logic
                    vSkip.Value = u.If(u.Equals(u.DOW(sqlTRANSACTION_DATE), 7), 0, u.DOW(sqlTRANSACTION_DATE));
                    u.VarSet(u.IndexOf(ss248Work1.LYSaturday) + vSkip, u.CastToNumber(u.VarCurr(u.IndexOf(ss248Work1.LYSaturday) + vSkip)) + sqlSECT_VAL);
                    // Accumulate LY Week-to-date figure only for days equivalent to those
                    // relevant to TY
                    if(sqlTRANSACTION_DATE <= _parent._parent.vUpToDatePY)
                    {
                        ss248Work1.LYWeekToDate.Value += sqlSECT_VAL;
                    }
                }
                
                
            }
            /// <summary>Fetch TY Mack Card(P#46.1.4)</summary>
            /// <remark>Last change before Migration: 20/04/2004 11:42:04</remark>
            class FetchTYMackCard : SalesAndStock.BusinessProcessBase 
            {
                
                #region Models
                DynamicSQLEntity sqlEntity;
                /// <summary>ss248 Work1</summary>
                readonly Models.ss248Work1 ss248Work1 = new Models.ss248Work1 { KeepCacheAliveAfterExit = true, AllowRowLocking = true };
                #endregion
                
                #region Columns
                /// <summary>sql:BRANCH_NUMBER</summary>
                readonly NumberColumn sqlBRANCH_NUMBER = new NumberColumn("sql:BRANCH_NUMBER", "N4")
                {
                	AllowNull = true
                };
                /// <summary>sql:TILL_DATE</summary>
                readonly DateColumn sqlTILL_DATE = new DateColumn("sql:TILL_DATE")
                {
                	AllowNull = true,
                	Storage = new ENV.Data.Storage.DateTimeDateStorage()
                };
                /// <summary>sql:MACK_CARD_VAL</summary>
                readonly NumberColumn sqlMACK_CARD_VAL = new NumberColumn("sql:MACK_CARD_VAL", "9.2")
                {
                	AllowNull = true
                };
                /// <summary>v:Skip</summary>
                readonly NumberColumn vSkip = new NumberColumn("v:Skip", "1");
                #endregion
                
                Control _parent;
                
                public FetchTYMackCard(Control parent)
                {
                    _parent = parent;
                    Title = "Fetch TY Mack Card";
                    InitializeDataView();
                }
                void InitializeDataView()
                {
                    sqlEntity = new DynamicSQLEntity(Models.DataSources.SS, 
@"select t.branch_number, t.till_date, sum(nvl(t.value,0)) mack_card_val
from sla_tenders t
where t.till_date between ':1' and ':2'
  and t.rtc_tender_type_code in (06,17)
  and exists (select 'X' from ss_ss248_work1 w
              where w.financial_week = :3
                and w.branch_number = t.branch_number)
group by t.branch_number, t.till_date
order by t.branch_number, t.till_date");
                    sqlEntity.AddParameter(() => u.DStr(_parent._parent.vWeekStartTY, "DD-MMM-YYYY")); //:1;
                    sqlEntity.AddParameter(() => u.DStr(_parent._parent.vUpToDateTY, "DD-MMM-YYYY")); //:2;
                    sqlEntity.AddParameter(_parent._parent.vReqYYYYWW); //:3;
                    sqlEntity.Columns.Add(sqlBRANCH_NUMBER, sqlTILL_DATE, sqlMACK_CARD_VAL);
                    From = sqlEntity;
                    
                    Relations.Add(ss248Work1, RelationType.InsertIfNotFound, 
                    		ss248Work1.FinancialWeek.BindEqualTo(_parent._parent.vReqYYYYWW).And(
                    		ss248Work1.BranchNumber.BindEqualTo(sqlBRANCH_NUMBER)).And(
                    		ss248Work1.ReportGroup.BindEqualTo("C")).And(
                    		ss248Work1.DeptReportSequence.BindEqualTo(99)).And(
                    		ss248Work1.Department1.BindEqualTo("99")).And(
                    		ss248Work1.SubDepartment1.BindEqualTo("99")), 
                    	ss248Work1.SortByss_ss248_Work1_X1);
                    
                    
                    
                    #region Columns
                    
                    // Data returned by SQL
                    Columns.Add(sqlBRANCH_NUMBER);
                    Columns.Add(sqlTILL_DATE);
                    Columns.Add(sqlMACK_CARD_VAL);
                    // Create/update Mackays Card record in work table
                    Columns.Add(ss248Work1.FinancialWeek);
                    Columns.Add(ss248Work1.BranchNumber);
                    Columns.Add(ss248Work1.ReportGroup);
                    Columns.Add(ss248Work1.DeptReportSequence);
                    Columns.Add(ss248Work1.Department1);
                    Columns.Add(ss248Work1.SubDepartment1);
                    Columns.Add(ss248Work1.TYSaturday);
                    Columns.Add(ss248Work1.TYSunday);
                    Columns.Add(ss248Work1.TYMonday);
                    Columns.Add(ss248Work1.TYTuesday);
                    Columns.Add(ss248Work1.TYWednesday);
                    Columns.Add(ss248Work1.TYThursday);
                    Columns.Add(ss248Work1.TYFriday);
                    Columns.Add(ss248Work1.TYWeekToDate);
                    Columns.Add(ss248Work1.LYSaturday);
                    Columns.Add(ss248Work1.LYSunday);
                    Columns.Add(ss248Work1.LYMonday);
                    Columns.Add(ss248Work1.LYTuesday);
                    Columns.Add(ss248Work1.LYWednesday);
                    Columns.Add(ss248Work1.LYThursday);
                    Columns.Add(ss248Work1.LYFriday);
                    Columns.Add(ss248Work1.LYWeekToDate);
                    
                    Columns.Add(vSkip);
                    #endregion
                }
                /// <summary>Fetch TY Mack Card(P#46.1.4)</summary>
                internal void Run()
                {
                    Execute();
                }
                protected override void OnLoad()
                {
                    RowLocking = LockingStrategy.OnRowLoading;
                    TransactionScope = TransactionScopes.Task;
                    KeepChildRelationCacheAlive = true;
                    AllowDelete = false;
                    AllowUserAbort = true;
                }
                protected override void OnLeaveRow()
                {
                    // Init SKIP from day_of_week to allow VARSET logic
                    vSkip.Value = u.If(u.Equals(u.DOW(sqlTILL_DATE), 7), 0, u.DOW(sqlTILL_DATE));
                    u.VarSet(u.IndexOf(ss248Work1.TYSaturday) + vSkip, u.CastToNumber(u.VarCurr(u.IndexOf(ss248Work1.TYSaturday) + vSkip)) + sqlMACK_CARD_VAL);
                    ss248Work1.TYWeekToDate.Value += sqlMACK_CARD_VAL;
                }
                
                
            }
            /// <summary>Fetch PY Mack Card(P#46.1.5)</summary>
            /// <remark>Last change before Migration: 21/04/2004 11:53:50</remark>
            class FetchPYMackCard : SalesAndStock.BusinessProcessBase 
            {
                
                #region Models
                DynamicSQLEntity sqlEntity;
                /// <summary>ss248 Work1</summary>
                readonly Models.ss248Work1 ss248Work1 = new Models.ss248Work1 { KeepCacheAliveAfterExit = true, AllowRowLocking = true };
                #endregion
                
                #region Columns
                /// <summary>sql:BRANCH_NUMBER</summary>
                readonly NumberColumn sqlBRANCH_NUMBER = new NumberColumn("sql:BRANCH_NUMBER", "N4")
                {
                	AllowNull = true
                };
                /// <summary>sql:TILL_DATE</summary>
                readonly DateColumn sqlTILL_DATE = new DateColumn("sql:TILL_DATE")
                {
                	AllowNull = true,
                	Storage = new ENV.Data.Storage.DateTimeDateStorage()
                };
                /// <summary>sql:MACK_CARD_VAL</summary>
                readonly NumberColumn sqlMACK_CARD_VAL = new NumberColumn("sql:MACK_CARD_VAL", "9.2")
                {
                	AllowNull = true
                };
                /// <summary>v:Skip</summary>
                readonly NumberColumn vSkip = new NumberColumn("v:Skip", "1");
                #endregion
                
                Control _parent;
                
                public FetchPYMackCard(Control parent)
                {
                    _parent = parent;
                    Title = "Fetch PY Mack Card";
                    InitializeDataView();
                }
                void InitializeDataView()
                {
                    sqlEntity = new DynamicSQLEntity(Models.DataSources.SS, 
@"select t.branch_number, t.till_date, sum(nvl(t.value,0)) mack_card_val
from sla_tenders t
where t.till_date between ':1' and ':2'
  and t.rtc_tender_type_code in (06, 17)
  and exists (select 'X' from ss_ss248_work1 w
              where w.financial_week = :3
                and w.branch_number = t.branch_number)
group by t.branch_number, t.till_date
order by t.branch_number, t.till_date");
                    sqlEntity.AddParameter(() => u.DStr(_parent._parent.vWeekStartPY, "DD-MMM-YYYY")); //:1;
                    sqlEntity.AddParameter(() => u.DStr(_parent._parent.vWeekEndPY, "DD-MMM-YYYY")); //:2;
                    sqlEntity.AddParameter(_parent._parent.vReqYYYYWW); //:3;
                    sqlEntity.Columns.Add(sqlBRANCH_NUMBER, sqlTILL_DATE, sqlMACK_CARD_VAL);
                    From = sqlEntity;
                    
                    Relations.Add(ss248Work1, RelationType.InsertIfNotFound, 
                    		ss248Work1.FinancialWeek.BindEqualTo(_parent._parent.vReqYYYYWW).And(
                    		ss248Work1.BranchNumber.BindEqualTo(sqlBRANCH_NUMBER)).And(
                    		ss248Work1.ReportGroup.BindEqualTo("C")).And(
                    		ss248Work1.DeptReportSequence.BindEqualTo(99)).And(
                    		ss248Work1.Department1.BindEqualTo("99")).And(
                    		ss248Work1.SubDepartment1.BindEqualTo("99")), 
                    	ss248Work1.SortByss_ss248_Work1_X1);
                    
                    
                    
                    #region Columns
                    
                    // Data returned by SQL
                    Columns.Add(sqlBRANCH_NUMBER);
                    Columns.Add(sqlTILL_DATE);
                    Columns.Add(sqlMACK_CARD_VAL);
                    // Create/Update LY Mackays Card to work table
                    Columns.Add(ss248Work1.FinancialWeek);
                    Columns.Add(ss248Work1.BranchNumber);
                    Columns.Add(ss248Work1.ReportGroup);
                    Columns.Add(ss248Work1.DeptReportSequence);
                    Columns.Add(ss248Work1.Department1);
                    Columns.Add(ss248Work1.SubDepartment1);
                    Columns.Add(ss248Work1.TYSaturday);
                    Columns.Add(ss248Work1.TYSunday);
                    Columns.Add(ss248Work1.TYMonday);
                    Columns.Add(ss248Work1.TYTuesday);
                    Columns.Add(ss248Work1.TYWednesday);
                    Columns.Add(ss248Work1.TYThursday);
                    Columns.Add(ss248Work1.TYFriday);
                    Columns.Add(ss248Work1.TYWeekToDate);
                    Columns.Add(ss248Work1.LYSaturday);
                    Columns.Add(ss248Work1.LYSunday);
                    Columns.Add(ss248Work1.LYMonday);
                    Columns.Add(ss248Work1.LYTuesday);
                    Columns.Add(ss248Work1.LYWednesday);
                    Columns.Add(ss248Work1.LYThursday);
                    Columns.Add(ss248Work1.LYFriday);
                    Columns.Add(ss248Work1.LYWeekToDate);
                    
                    Columns.Add(vSkip);
                    #endregion
                }
                /// <summary>Fetch PY Mack Card(P#46.1.5)</summary>
                internal void Run()
                {
                    Execute();
                }
                protected override void OnLoad()
                {
                    RowLocking = LockingStrategy.OnRowLoading;
                    TransactionScope = TransactionScopes.Task;
                    KeepChildRelationCacheAlive = true;
                    AllowDelete = false;
                    AllowUserAbort = true;
                }
                protected override void OnLeaveRow()
                {
                    // Init SKIP from day_of_week to allow VARSET logic
                    vSkip.Value = u.If(u.Equals(u.DOW(sqlTILL_DATE), 7), 0, u.DOW(sqlTILL_DATE));
                    u.VarSet(u.IndexOf(ss248Work1.LYSaturday) + vSkip, u.CastToNumber(u.VarCurr(u.IndexOf(ss248Work1.LYSaturday) + vSkip)) + sqlMACK_CARD_VAL);
                    // Accumulate LY Week-to-date figure only for days equivalent to those
                    // relevant to TY.
                    if(sqlTILL_DATE <= _parent._parent.vUpToDatePY)
                    {
                        ss248Work1.LYWeekToDate.Value += sqlMACK_CARD_VAL;
                    }
                }
                
                
            }
            /// <summary>Set Up Branch List(P#46.1.6)</summary>
            /// <remark>Last change before Migration: 10/05/2006 16:40:42</remark>
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
@"insert into ss_Diary_Work1
select distinct 'ss248', w.branch_number,  ' ', ' '
  from ss_ss248_Work1 w");
                    From = sqlEntity;
                    
                    
                    #region Columns
                    
                    // This cask creates a list of the branches to be processed by creating
                    // entries in table ss_Diary_Work1.
                    #endregion
                }
                /// <summary>Set Up Branch List(P#46.1.6)</summary>
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
            /// <summary>Process Branches(P#46.1.7)</summary>
            /// <remark>Last change before Migration: 27/05/2011 11:45:41</remark>
            internal class ProcessBranches : SalesAndStock.BusinessProcessBase 
            {
                
                #region Models
                /// <summary>Diary Work1</summary>
                internal readonly Models.DiaryWork1 DiaryWork1 = new Models.DiaryWork1 { AllowRowLocking = true };
                /// <summary>Branch</summary>
                readonly Models.Branch Branch = new Models.Branch { ReadOnly = true };
                #endregion
                
                #region Columns
                /// <summary>v:BranchFound</summary>
                readonly BoolColumn vBranchFound = new BoolColumn("v:BranchFound");
                /// <summary>v:TargetDir</summary>
                readonly TextColumn vTargetDir = new TextColumn("v:TargetDir", "120");
                /// <summary>v:Sterling</summary>
                readonly TextColumn vSterling = new TextColumn("v:Sterling", "3");
                /// <summary>v:PC Sign</summary>
                readonly TextColumn vPCSign = new TextColumn("v:PC Sign", "1");
                /// <summary>v:rf983 ReturnStatus</summary>
                readonly NumberColumn vRf983ReturnStatus = new NumberColumn("v:rf983 ReturnStatus", "N1");
                /// <summary>v:Concessions Present</summary>
                readonly TextColumn vConcessionsPresent = new TextColumn("v:Concessions Present", "U");
                #endregion
                
                Control _parent;
                
                public ProcessBranches(Control parent)
                {
                    _parent = parent;
                    Title = "Process Branches";
                    InitializeDataView();
                }
                void InitializeDataView()
                {
                    From = DiaryWork1;
                    
                    Relations.Add(Branch, 
                    		Branch.BranchNumber.IsEqualTo(DiaryWork1.BranchNumber), 
                    	Branch.SortByREF_Branch_X1);
                    
                    
                    Where.Add(DiaryWork1.ProgNo.IsEqualTo("ss248"));
                    
                    OrderBy = DiaryWork1.SortBySS_Diary_Work1_X1;
                    
                    #region Columns
                    
                    Columns.Add(DiaryWork1.ProgNo);
                    Columns.Add(DiaryWork1.BranchNumber);
                    
                    Columns.Add(vBranchFound);
                    Relations[Branch].NotifyRowWasFoundTo(vBranchFound);
                    Columns.Add(Branch.BranchNumber);
                    Columns.Add(Branch.BranchName);
                    
                    Columns.Add(vTargetDir);
                    Columns.Add(vSterling).BindValue(() => "£'s");
                    Columns.Add(vPCSign).BindValue(() => "%");
                    Columns.Add(vRf983ReturnStatus);
                    Columns.Add(vConcessionsPresent);
                    #endregion
                }
                /// <summary>Process Branches(P#46.1.7)</summary>
                internal void Run()
                {
                    Execute();
                }
                protected override void OnLoad()
                {
                    TransactionScope = TransactionScopes.RowLocking;
                    KeepChildRelationCacheAlive = true;
                    AllowUserAbort = true;
                    if(NewViewRequired)
                    {
                        View = ()=> new Views.DailyBrnSlsPerfSs248ProcessBranches(this);
                    }
                }
                protected override void OnEnterRow()
                {
                    Cached<ClearWork3>().Run();
                }
                protected override void OnLeaveRow()
                {
                    if(vBranchFound)
                    {
                        _parent._parent.vHdrBranch.Value = "Daily Sales Performance for " + u.Trim(Branch.BranchName) + " (" + u.Trim(u.Str(DiaryWork1.BranchNumber, "####")) + ")";
                    }
                    if(u.Not(vBranchFound))
                    {
                        _parent._parent.vHdrBranch.Value = "Daily Sales Performance for Store " + u.Trim(u.Str(DiaryWork1.BranchNumber, "####")) + " (Unknown)";
                    }
                    vTargetDir.Value = u.Trim(_parent.vBaseDir) + "b" + u.Str(DiaryWork1.BranchNumber, "4P0") + "/wwwroot/rpt/sls/" + u.Mid(_parent._parent.pYYYYWW, 3, 4);
                    _parent.vTargetFile.Value = u.Trim(vTargetDir) + "/ss248.htm";
                    vConcessionsPresent.Value = "N";
                    // If AIX directory structure not present, create it.
                    if(false)
                    {
                        Windows.OSCommand("%MagicApps%/ss/scr/ssdir.bat " + u.Trim(vTargetDir), true);
                    }
                    ApplicationControllerBase.RunProgramFromAnUnreferencedApplication(u.Translate("%MagicApps%rf/ecf/rf.ecf"),"rf983", u.Translate(u.Trim(vTargetDir)), vRf983ReturnStatus);
                    
                    Cached<SelectRptGroup>().Run();
                    Cached<PrintBranch>().Run();
                }
                
                
                /// <summary>Select Rpt Group(P#46.1.7.1)</summary>
                /// <remark>Last change before Migration: 16/05/2006 15:08:25</remark>
                class SelectRptGroup : SalesAndStock.BusinessProcessBase 
                {
                    
                    #region Models
                    /// <summary>Analysis Group 1 Depts</summary>
                    readonly Models.AnalysisGroup1Depts AnalysisGroup1Depts = new Models.AnalysisGroup1Depts { AllowRowLocking = true };
                    /// <summary>ss248 Work3</summary>
                    readonly Models.ss248Work3 ss248Work3 = new Models.ss248Work3 { AllowRowLocking = true };
                    #endregion
                    
                    #region Columns
                    /// <summary>v:Dtl Act Sat</summary>
                    readonly NumberColumn vDtlActSat = new NumberColumn("v:Dtl Act Sat", "N6.2");
                    /// <summary>v:Dtl Act Sun</summary>
                    readonly NumberColumn vDtlActSun = new NumberColumn("v:Dtl Act Sun", "N6.2");
                    /// <summary>v:Dtl Act Mon</summary>
                    readonly NumberColumn vDtlActMon = new NumberColumn("v:Dtl Act Mon", "N6.2");
                    /// <summary>v:Dtl Act Tue</summary>
                    readonly NumberColumn vDtlActTue = new NumberColumn("v:Dtl Act Tue", "N6.2");
                    /// <summary>v:Dtl Act Wed</summary>
                    readonly NumberColumn vDtlActWed = new NumberColumn("v:Dtl Act Wed", "N6.2");
                    /// <summary>v:Dtl Act Thu</summary>
                    readonly NumberColumn vDtlActThu = new NumberColumn("v:Dtl Act Thu", "N6.2");
                    /// <summary>v:Dtl Act Fri</summary>
                    readonly NumberColumn vDtlActFri = new NumberColumn("v:Dtl Act Fri", "N6.2");
                    /// <summary>v:Dtl Act Week</summary>
                    readonly NumberColumn vDtlActWeek = new NumberColumn("v:Dtl Act Week", "N6.2");
                    /// <summary>v:Dtl LY Sat</summary>
                    readonly NumberColumn vDtlLYSat = new NumberColumn("v:Dtl LY Sat", "N6.2");
                    /// <summary>v:Dtl LY Sun</summary>
                    readonly NumberColumn vDtlLYSun = new NumberColumn("v:Dtl LY Sun", "N6.2");
                    /// <summary>v:Dtl LY Mon</summary>
                    readonly NumberColumn vDtlLYMon = new NumberColumn("v:Dtl LY Mon", "N6.2");
                    /// <summary>v:Dtl LY Tue</summary>
                    readonly NumberColumn vDtlLYTue = new NumberColumn("v:Dtl LY Tue", "N6.2");
                    /// <summary>v:Dtl LY Wed</summary>
                    readonly NumberColumn vDtlLYWed = new NumberColumn("v:Dtl LY Wed", "N6.2");
                    /// <summary>v:Dtl LY Thu</summary>
                    readonly NumberColumn vDtlLYThu = new NumberColumn("v:Dtl LY Thu", "N6.2");
                    /// <summary>v:Dtl LY Fri</summary>
                    readonly NumberColumn vDtlLYFri = new NumberColumn("v:Dtl LY Fri", "N6.2");
                    /// <summary>v:Dtl LY Week</summary>
                    readonly NumberColumn vDtlLYWeek = new NumberColumn("v:Dtl LY Week", "N6.2");
                    /// <summary>v:Ladies Act Sat</summary>
                    readonly NumberColumn vLadiesActSat = new NumberColumn("v:Ladies Act Sat", "N6");
                    /// <summary>v:Ladies Act Sun</summary>
                    readonly NumberColumn vLadiesActSun = new NumberColumn("v:Ladies Act Sun", "N6");
                    /// <summary>v:Ladies Act Mon</summary>
                    readonly NumberColumn vLadiesActMon = new NumberColumn("v:Ladies Act Mon", "N6");
                    /// <summary>v:Ladies Act Tue</summary>
                    readonly NumberColumn vLadiesActTue = new NumberColumn("v:Ladies Act Tue", "N6");
                    /// <summary>v:Ladies Act Wed</summary>
                    readonly NumberColumn vLadiesActWed = new NumberColumn("v:Ladies Act Wed", "N6");
                    /// <summary>v:Ladies Act Thu</summary>
                    readonly NumberColumn vLadiesActThu = new NumberColumn("v:Ladies Act Thu", "N6");
                    /// <summary>v:Ladies Act Fri</summary>
                    readonly NumberColumn vLadiesActFri = new NumberColumn("v:Ladies Act Fri", "N6");
                    /// <summary>v:Ladies Act Week</summary>
                    readonly NumberColumn vLadiesActWeek = new NumberColumn("v:Ladies Act Week", "N6");
                    /// <summary>v:Ladies LY Sat</summary>
                    readonly NumberColumn vLadiesLYSat = new NumberColumn("v:Ladies LY Sat", "N6");
                    /// <summary>v:Ladies LY Sun</summary>
                    readonly NumberColumn vLadiesLYSun = new NumberColumn("v:Ladies LY Sun", "N6");
                    /// <summary>v:Ladies LY Mon</summary>
                    readonly NumberColumn vLadiesLYMon = new NumberColumn("v:Ladies LY Mon", "N6");
                    /// <summary>v:Ladies LY Tue</summary>
                    readonly NumberColumn vLadiesLYTue = new NumberColumn("v:Ladies LY Tue", "N6");
                    /// <summary>v:Ladies LY Wed</summary>
                    readonly NumberColumn vLadiesLYWed = new NumberColumn("v:Ladies LY Wed", "N6");
                    /// <summary>v:Ladies LY Thu</summary>
                    readonly NumberColumn vLadiesLYThu = new NumberColumn("v:Ladies LY Thu", "N6");
                    /// <summary>v:Ladies LY Fri</summary>
                    readonly NumberColumn vLadiesLYFri = new NumberColumn("v:Ladies LY Fri", "N6");
                    /// <summary>v:Ladies LY Week</summary>
                    readonly NumberColumn vLadiesLYWeek = new NumberColumn("v:Ladies LY Week", "N6");
                    /// <summary>v:Store Act Sat</summary>
                    readonly NumberColumn vStoreActSat = new NumberColumn("v:Store Act Sat", "N6");
                    /// <summary>v:Store Act Sun</summary>
                    readonly NumberColumn vStoreActSun = new NumberColumn("v:Store Act Sun", "N6");
                    /// <summary>v:Store Act Mon</summary>
                    readonly NumberColumn vStoreActMon = new NumberColumn("v:Store Act Mon", "N6");
                    /// <summary>v:Store Act Tue</summary>
                    readonly NumberColumn vStoreActTue = new NumberColumn("v:Store Act Tue", "N6");
                    /// <summary>v:Store Act Wed</summary>
                    readonly NumberColumn vStoreActWed = new NumberColumn("v:Store Act Wed", "N6");
                    /// <summary>v:Store Act Thu</summary>
                    readonly NumberColumn vStoreActThu = new NumberColumn("v:Store Act Thu", "N6");
                    /// <summary>v:Store Act Fri</summary>
                    readonly NumberColumn vStoreActFri = new NumberColumn("v:Store Act Fri", "N6");
                    /// <summary>v:Stores Act Week</summary>
                    readonly NumberColumn vStoresActWeek = new NumberColumn("v:Stores Act Week", "N6");
                    /// <summary>v:Store LY Sat</summary>
                    readonly NumberColumn vStoreLYSat = new NumberColumn("v:Store LY Sat", "N6");
                    /// <summary>v:Store LY Sun</summary>
                    readonly NumberColumn vStoreLYSun = new NumberColumn("v:Store LY Sun", "N6");
                    /// <summary>v:Store LY Mon</summary>
                    readonly NumberColumn vStoreLYMon = new NumberColumn("v:Store LY Mon", "N6");
                    /// <summary>v:Store LY Tue</summary>
                    readonly NumberColumn vStoreLYTue = new NumberColumn("v:Store LY Tue", "N6");
                    /// <summary>v:Store LY Wed</summary>
                    readonly NumberColumn vStoreLYWed = new NumberColumn("v:Store LY Wed", "N6");
                    /// <summary>v:Store LY Thu</summary>
                    readonly NumberColumn vStoreLYThu = new NumberColumn("v:Store LY Thu", "N6");
                    /// <summary>v:Store LY Fri</summary>
                    readonly NumberColumn vStoreLYFri = new NumberColumn("v:Store LY Fri", "N6");
                    /// <summary>v:Store LY Week</summary>
                    readonly NumberColumn vStoreLYWeek = new NumberColumn("v:Store LY Week", "N6");
                    /// <summary>v:Work Act Sat</summary>
                    readonly NumberColumn vWorkActSat = new NumberColumn("v:Work Act Sat", "N6");
                    /// <summary>v:Work Act Sun</summary>
                    readonly NumberColumn vWorkActSun = new NumberColumn("v:Work Act Sun", "N6");
                    /// <summary>v:Work Act Mon</summary>
                    readonly NumberColumn vWorkActMon = new NumberColumn("v:Work Act Mon", "N6");
                    /// <summary>v:Work Act Tue</summary>
                    readonly NumberColumn vWorkActTue = new NumberColumn("v:Work Act Tue", "N6");
                    /// <summary>v:Work Act Wed</summary>
                    readonly NumberColumn vWorkActWed = new NumberColumn("v:Work Act Wed", "N6");
                    /// <summary>v:Work Act Thu</summary>
                    readonly NumberColumn vWorkActThu = new NumberColumn("v:Work Act Thu", "N6");
                    /// <summary>v:Work Act Fri</summary>
                    readonly NumberColumn vWorkActFri = new NumberColumn("v:Work Act Fri", "N6");
                    /// <summary>v:Work Act Week</summary>
                    readonly NumberColumn vWorkActWeek = new NumberColumn("v:Work Act Week", "N6");
                    /// <summary>v:Work LY Sat</summary>
                    readonly NumberColumn vWorkLYSat = new NumberColumn("v:Work LY Sat", "N6");
                    /// <summary>v:Work LY Sun</summary>
                    readonly NumberColumn vWorkLYSun = new NumberColumn("v:Work LY Sun", "N6");
                    /// <summary>v:Work LY Mon</summary>
                    readonly NumberColumn vWorkLYMon = new NumberColumn("v:Work LY Mon", "N6");
                    /// <summary>v:Work LY Tue</summary>
                    readonly NumberColumn vWorkLYTue = new NumberColumn("v:Work LY Tue", "N6");
                    /// <summary>v:Work LY Wed</summary>
                    readonly NumberColumn vWorkLYWed = new NumberColumn("v:Work LY Wed", "N6");
                    /// <summary>v:Work LY Thu</summary>
                    readonly NumberColumn vWorkLYThu = new NumberColumn("v:Work LY Thu", "N6");
                    /// <summary>v:Work LY Fri</summary>
                    readonly NumberColumn vWorkLYFri = new NumberColumn("v:Work LY Fri", "N6");
                    /// <summary>v:Work LY Week</summary>
                    readonly NumberColumn vWorkLYWeek = new NumberColumn("v:Work LY Week", "N6");
                    /// <summary>v:Work PCent Sat</summary>
                    readonly NumberColumn vWorkPCentSat = new NumberColumn("v:Work PCent Sat", "N6");
                    /// <summary>v:Work PCent Sun</summary>
                    readonly NumberColumn vWorkPCentSun = new NumberColumn("v:Work PCent Sun", "N6");
                    /// <summary>v:Work PCent Mon</summary>
                    readonly NumberColumn vWorkPCentMon = new NumberColumn("v:Work PCent Mon", "N6");
                    /// <summary>v:Work PCent Tue</summary>
                    readonly NumberColumn vWorkPCentTue = new NumberColumn("v:Work PCent Tue", "N6");
                    /// <summary>v:Work PCent Wed</summary>
                    readonly NumberColumn vWorkPCentWed = new NumberColumn("v:Work PCent Wed", "N6");
                    /// <summary>v:Work PCent Thu</summary>
                    readonly NumberColumn vWorkPCentThu = new NumberColumn("v:Work PCent Thu", "N6");
                    /// <summary>v:Work PCent Fri</summary>
                    readonly NumberColumn vWorkPCentFri = new NumberColumn("v:Work PCent Fri", "N6");
                    /// <summary>v:Work PCent Week</summary>
                    readonly NumberColumn vWorkPCentWeek = new NumberColumn("v:Work PCent Week", "N6");
                    /// <summary>v:Work3 Rpt Grp</summary>
                    readonly TextColumn vWork3RptGrp = new TextColumn("v:Work3 Rpt Grp", "U");
                    /// <summary>v:Work3 Rpt Dept</summary>
                    readonly TextColumn vWork3RptDept = new TextColumn("v:Work3 Rpt Dept", "U2");
                    /// <summary>v:Work3 Rpt SubDept</summary>
                    readonly TextColumn vWork3RptSubDept = new TextColumn("v:Work3 Rpt SubDept", "U2");
                    /// <summary>v:Work3 Dept Name</summary>
                    readonly TextColumn vWork3DeptName = new TextColumn("v:Work3 Dept Name", "UX12");
                    #endregion
                    
                    #region Streams
                    /// <summary>Report By Branch</summary>
                    FileWriter _ioReportByBranch;
                    #endregion
                    
                    ProcessBranches _parent;
                    
                    public SelectRptGroup(ProcessBranches parent)
                    {
                        _parent = parent;
                        Title = "Select Rpt Group";
                        Entities.Add(ss248Work3);
                        InitializeDataView();
                        var AnalysisGroup1DeptsGROUP1_DEPTGroup = Groups.Add(AnalysisGroup1Depts.GROUP1_DEPT);
                        AnalysisGroup1DeptsGROUP1_DEPTGroup.Leave += AnalysisGroup1DeptsGROUP1_DEPTGroup_Leave;
                    }
                    void InitializeDataView()
                    {
                        From = AnalysisGroup1Depts;
                        
                        OrderBy = AnalysisGroup1Depts.SortByREF_ANAL_GRP1_DPTS_X1;
                        
                        #region Columns
                        
                        Columns.Add(AnalysisGroup1Depts.GROUP1_DEPT);
                        Columns.Add(AnalysisGroup1Depts.GROUP1_SUBDEPT);
                        Columns.Add(AnalysisGroup1Depts.GROUP1_NAME);
                        
                        // Detail level Totals
                        Columns.Add(vDtlActSat);
                        Columns.Add(vDtlActSun);
                        Columns.Add(vDtlActMon);
                        Columns.Add(vDtlActTue);
                        Columns.Add(vDtlActWed);
                        Columns.Add(vDtlActThu);
                        Columns.Add(vDtlActFri);
                        Columns.Add(vDtlActWeek);
                        Columns.Add(vDtlLYSat);
                        Columns.Add(vDtlLYSun);
                        Columns.Add(vDtlLYMon);
                        Columns.Add(vDtlLYTue);
                        Columns.Add(vDtlLYWed);
                        Columns.Add(vDtlLYThu);
                        Columns.Add(vDtlLYFri);
                        Columns.Add(vDtlLYWeek);
                        
                        // Ladies Totals
                        Columns.Add(vLadiesActSat);
                        Columns.Add(vLadiesActSun);
                        Columns.Add(vLadiesActMon);
                        Columns.Add(vLadiesActTue);
                        Columns.Add(vLadiesActWed);
                        Columns.Add(vLadiesActThu);
                        Columns.Add(vLadiesActFri);
                        Columns.Add(vLadiesActWeek);
                        Columns.Add(vLadiesLYSat);
                        Columns.Add(vLadiesLYSun);
                        Columns.Add(vLadiesLYMon);
                        Columns.Add(vLadiesLYTue);
                        Columns.Add(vLadiesLYWed);
                        Columns.Add(vLadiesLYThu);
                        Columns.Add(vLadiesLYFri);
                        Columns.Add(vLadiesLYWeek);
                        
                        // Store Totals
                        Columns.Add(vStoreActSat);
                        Columns.Add(vStoreActSun);
                        Columns.Add(vStoreActMon);
                        Columns.Add(vStoreActTue);
                        Columns.Add(vStoreActWed);
                        Columns.Add(vStoreActThu);
                        Columns.Add(vStoreActFri);
                        Columns.Add(vStoresActWeek);
                        Columns.Add(vStoreLYSat);
                        Columns.Add(vStoreLYSun);
                        Columns.Add(vStoreLYMon);
                        Columns.Add(vStoreLYTue);
                        Columns.Add(vStoreLYWed);
                        Columns.Add(vStoreLYThu);
                        Columns.Add(vStoreLYFri);
                        Columns.Add(vStoreLYWeek);
                        
                        // Work areas for percentage calculation
                        Columns.Add(vWorkActSat);
                        Columns.Add(vWorkActSun);
                        Columns.Add(vWorkActMon);
                        Columns.Add(vWorkActTue);
                        Columns.Add(vWorkActWed);
                        Columns.Add(vWorkActThu);
                        Columns.Add(vWorkActFri);
                        Columns.Add(vWorkActWeek);
                        Columns.Add(vWorkLYSat);
                        Columns.Add(vWorkLYSun);
                        Columns.Add(vWorkLYMon);
                        Columns.Add(vWorkLYTue);
                        Columns.Add(vWorkLYWed);
                        Columns.Add(vWorkLYThu);
                        Columns.Add(vWorkLYFri);
                        Columns.Add(vWorkLYWeek);
                        Columns.Add(vWorkPCentSat);
                        Columns.Add(vWorkPCentSun);
                        Columns.Add(vWorkPCentMon);
                        Columns.Add(vWorkPCentTue);
                        Columns.Add(vWorkPCentWed);
                        Columns.Add(vWorkPCentThu);
                        Columns.Add(vWorkPCentFri);
                        Columns.Add(vWorkPCentWeek);
                        
                        Columns.Add(vWork3RptGrp);
                        Columns.Add(vWork3RptDept).BindValue(AnalysisGroup1Depts.GROUP1_DEPT);
                        Columns.Add(vWork3RptSubDept).BindValue(AnalysisGroup1Depts.GROUP1_SUBDEPT);
                        Columns.Add(vWork3DeptName);
                        #endregion
                    }
                    /// <summary>Select Rpt Group(P#46.1.7.1)</summary>
                    internal void Run()
                    {
                        Execute();
                    }
                    protected override void OnLoad()
                    {
                        KeepChildRelationCacheAlive = true;
                        AllowUserAbort = true;
                        
                        _ioReportByBranch = new FileWriter(_parent._parent.vTargetFile)
                        			{
                        				Name = "Report By Branch"
                        			};
                        _ioReportByBranch.Open();
                        Streams.Add(_ioReportByBranch);
                    }
                    protected override void OnLeaveRow()
                    {
                        vWork3RptGrp.Value = "A";
                        Cached<ReadWork1>().Run();
                        vWork3DeptName.Value = AnalysisGroup1Depts.GROUP1_NAME;
                        Cached<InitWorkWithDetail>().Run();
                        Cached<CalcSalesPCents>().Run();
                        Cached<WriteSalesWork3>().Run();
                    }
                    void AnalysisGroup1DeptsGROUP1_DEPTGroup_Leave()
                    {
                        // Write Work3 record for Total Ladies
                        if(AnalysisGroup1Depts.GROUP1_DEPT == "1")
                        {
                            Cached<InitWorkWithLadies>().Run();
                            Cached<CalcSalesPCents>().Run();
                            vWork3RptDept.Value = AnalysisGroup1Depts.GROUP1_DEPT;
                            vWork3RptSubDept.Value = "ZZ";
                            vWork3DeptName.Value = "Total Ladies";
                            Cached<WriteSalesWork3>().Run();
                        }
                        
                        // Write Work3 record for Total Store
                        if(AnalysisGroup1Depts.GROUP1_DEPT == "9")
                        {
                            Cached<InitWorkWithStores>().Run();
                            Cached<CalcSalesPCents>().Run();
                            vWork3RptDept.Value = "99";
                            vWork3RptSubDept.Value = "99";
                            vWork3DeptName.Value = "Total Store";
                            Cached<WriteSalesWork3>().Run();
                        }
                    }
                    protected override void OnEnd()
                    {
                        // Write Work3 record for Mackays Card
                        vWork3RptGrp.Value = "C";
                        vWork3RptDept.Value = "99";
                        vWork3RptSubDept.Value = "99";
                        vWork3DeptName.Value = "Mackays Card";
                        Cached<ReadWork1>().Run();
                        Cached<ICROrMackCard>().Run();
                        // Write Work3 record for Instant Credit
                        vWork3RptGrp.Value = "D";
                        vWork3RptDept.Value = "99";
                        vWork3RptSubDept.Value = "99";
                        vWork3DeptName.Value = "Inst Credits";
                        Cached<ReadWork1>().Run();
                        Cached<ICROrMackCard>().Run();
                        // Write Work3 records for Concessions
                        Cached<ReadWork1Concessions>().Run();
                        
                        if(false)
                        {
                            // Now generate Work 3 records for blank lines where required:-
                            // Blank line after 'Total Ladies'
                            vWork3RptGrp.Value = "A";
                            vWork3RptDept.Value = "2";
                            vWork3RptSubDept.Value = "99";
                            vWork3DeptName.Value = " ";
                            Cached<GenerateBlankLine>().Run();
                            // Blank line after 'Total Store'
                            vWork3RptGrp.Value = "B";
                            vWork3RptDept.Value = "99";
                            vWork3RptSubDept.Value = "99";
                            vWork3DeptName.Value = " ";
                            Cached<GenerateBlankLine>().Run();
                            // Blank line after 'Inst Credits'
                            vWork3RptGrp.Value = "D";
                            vWork3RptDept.Value = "99";
                            vWork3RptSubDept.Value = "01";
                            vWork3DeptName.Value = " ";
                            Cached<GenerateBlankLine>().Run();
                            // Generate record denoting Concessions to follow - no data values on this record.
                            vWork3RptSubDept.Value = "02";
                            vWork3DeptName.Value = "Concessions";
                            Cached<GenerateBlankLine>().Run();
                        }
                    }
                    
                    
                    /// <summary>Read Work1(P#46.1.7.1.1)</summary>
                    /// <remark>Last change before Migration: 20/04/2004 11:44:21</remark>
                    class ReadWork1 : SalesAndStock.BusinessProcessBase 
                    {
                        
                        #region Models
                        /// <summary>ss248 Work1</summary>
                        readonly Models.ss248Work1 ss248Work1 = new Models.ss248Work1 { AllowRowLocking = true };
                        #endregion
                        
                        SelectRptGroup _parent;
                        
                        public ReadWork1(SelectRptGroup parent)
                        {
                            _parent = parent;
                            Title = "Read Work1";
                            InitializeDataView();
                        }
                        void InitializeDataView()
                        {
                            From = ss248Work1;
                            
                            #region Where
                            
                            Where.Add(ss248Work1.FinancialWeek.IsEqualTo(_parent._parent._parent._parent.vReqYYYYWW));
                            Where.Add(ss248Work1.BranchNumber.IsEqualTo(_parent._parent.DiaryWork1.BranchNumber));
                            Where.Add(ss248Work1.ReportGroup.IsEqualTo(_parent.vWork3RptGrp));
                            Where.Add(ss248Work1.DeptReportSequence.IsBetween(() => 0, 99));
                            Where.Add(ss248Work1.Department1.IsEqualTo(_parent.AnalysisGroup1Depts.GROUP1_DEPT));
                            Where.Add(ss248Work1.SubDepartment1.IsEqualTo(_parent.AnalysisGroup1Depts.GROUP1_SUBDEPT));
                            #endregion
                            
                            OrderBy = ss248Work1.SortByss_ss248_Work1_X1;
                            
                            #region Columns
                            
                            Columns.Add(ss248Work1.FinancialWeek);
                            Columns.Add(ss248Work1.BranchNumber);
                            Columns.Add(ss248Work1.ReportGroup);
                            Columns.Add(ss248Work1.DeptReportSequence);
                            Columns.Add(ss248Work1.Department1);
                            Columns.Add(ss248Work1.SubDepartment1);
                            Columns.Add(ss248Work1.TYSaturday);
                            Columns.Add(ss248Work1.TYSunday);
                            Columns.Add(ss248Work1.TYMonday);
                            Columns.Add(ss248Work1.TYTuesday);
                            Columns.Add(ss248Work1.TYWednesday);
                            Columns.Add(ss248Work1.TYThursday);
                            Columns.Add(ss248Work1.TYFriday);
                            Columns.Add(ss248Work1.TYWeekToDate);
                            Columns.Add(ss248Work1.LYSaturday);
                            Columns.Add(ss248Work1.LYSunday);
                            Columns.Add(ss248Work1.LYMonday);
                            Columns.Add(ss248Work1.LYTuesday);
                            Columns.Add(ss248Work1.LYWednesday);
                            Columns.Add(ss248Work1.LYThursday);
                            Columns.Add(ss248Work1.LYFriday);
                            Columns.Add(ss248Work1.LYWeekToDate);
                            #endregion
                        }
                        /// <summary>Read Work1(P#46.1.7.1.1)</summary>
                        internal void Run()
                        {
                            Execute();
                        }
                        protected override void OnLoad()
                        {
                            KeepChildRelationCacheAlive = true;
                            AllowUserAbort = true;
                        }
                        protected override void OnStart()
                        {
                            _parent.vDtlActSat.Value = 0;
                            _parent.vDtlActSun.Value = 0;
                            _parent.vDtlActMon.Value = 0;
                            _parent.vDtlActTue.Value = 0;
                            _parent.vDtlActWed.Value = 0;
                            _parent.vDtlActThu.Value = 0;
                            _parent.vDtlActFri.Value = 0;
                            _parent.vDtlActWeek.Value = 0;
                            _parent.vDtlLYSat.Value = 0;
                            _parent.vDtlLYSun.Value = 0;
                            _parent.vDtlLYMon.Value = 0;
                            _parent.vDtlLYTue.Value = 0;
                            _parent.vDtlLYWed.Value = 0;
                            _parent.vDtlLYThu.Value = 0;
                            _parent.vDtlLYFri.Value = 0;
                            _parent.vDtlLYWeek.Value = 0;
                        }
                        protected override void OnEnterRow()
                        {
                            Cached<AccumDetail>().Run();
                        }
                        protected override void OnEnd()
                        {
                            // Accumulate sales totals as required
                            if(u.Trim(_parent.AnalysisGroup1Depts.GROUP1_DEPT) == "1")
                            {
                                Cached<AccumLadies>().Run();
                            }
                            if(_parent.vWork3RptGrp == "A")
                            {
                                Cached<AccumStore>().Run();
                            }
                        }
                        
                        
                        /// <summary>Accum Detail(P#46.1.7.1.1.1)</summary>
                        /// <remark>Last change before Migration: 31/03/2004 10:07:18</remark>
                        class AccumDetail : SalesAndStock.BusinessProcessBase 
                        {
                            
                            #region Columns
                            /// <summary>v:Detail Position</summary>
                            readonly NumberColumn vDetailPosition = new NumberColumn("v:Detail Position", "2");
                            #endregion
                            
                            ReadWork1 _parent;
                            
                            public AccumDetail(ReadWork1 parent)
                            {
                                _parent = parent;
                                Title = "Accum Detail";
                                InitializeDataView();
                            }
                            void InitializeDataView()
                            {
                                
                                
                                #region Columns
                                
                                // This task accumulates the Actual and Last Year values from Work1
                                // into the relevant positions for printing at detail level.
                                Columns.Add(vDetailPosition);
                                #endregion
                            }
                            /// <summary>Accum Detail(P#46.1.7.1.1.1)</summary>
                            internal void Run()
                            {
                                Execute();
                            }
                            protected override void OnLoad()
                            {
                                Exit(ExitTiming.AfterRow, () => Counter == 16);
                                KeepChildRelationCacheAlive = true;
                                AllowUserAbort = true;
                            }
                            protected override void OnLeaveRow()
                            {
                                u.VarSet(u.IndexOf(_parent._parent.vDtlActSat) + vDetailPosition, u.CastToNumber(u.VarCurr(u.IndexOf(_parent._parent.vDtlActSat) + vDetailPosition)) + u.CastToNumber(u.VarCurr(u.IndexOf(_parent.ss248Work1.TYSaturday) + vDetailPosition)));
                                vDetailPosition.Value++;
                            }
                            protected override void OnEnd()
                            {
                                Cached<DebugAccumDtl>().Run();
                            }
                            
                            
                            /// <summary>Debug Accum Dtl(P#46.1.7.1.1.1.1)</summary>
                            /// <remark>Last change before Migration: 31/03/2004 09:59:49</remark>
                            class DebugAccumDtl : SalesAndStock.BusinessProcessBase 
                            {
                                AccumDetail _parent;
                                
                                public DebugAccumDtl(AccumDetail parent)
                                {
                                    _parent = parent;
                                    Title = "Debug Accum Dtl";
                                    InitializeDataView();
                                }
                                void InitializeDataView()
                                {
                                    
                                    
                                }
                                /// <summary>Debug Accum Dtl(P#46.1.7.1.1.1.1)</summary>
                                internal void Run()
                                {
                                    Execute();
                                }
                                protected override void OnLoad()
                                {
                                    Exit(ExitTiming.AfterRow);
                                    Activity = Activities.Browse;
                                    AllowUserAbort = true;
                                }
                                
                                
                            }
                        }
                        /// <summary>Accum Ladies(P#46.1.7.1.1.2)</summary>
                        /// <remark>Last change before Migration: 31/03/2004 10:07:26</remark>
                        class AccumLadies : SalesAndStock.BusinessProcessBase 
                        {
                            
                            #region Columns
                            /// <summary>v:Detail Position</summary>
                            readonly NumberColumn vDetailPosition = new NumberColumn("v:Detail Position", "2");
                            #endregion
                            
                            ReadWork1 _parent;
                            
                            public AccumLadies(ReadWork1 parent)
                            {
                                _parent = parent;
                                Title = "Accum Ladies";
                                InitializeDataView();
                            }
                            void InitializeDataView()
                            {
                                
                                
                                #region Columns
                                
                                // This task accumulates tha Actual and Last Year values for Core, Plus
                                // and Petite into corresponding totals for Ladies.
                                Columns.Add(vDetailPosition);
                                #endregion
                            }
                            /// <summary>Accum Ladies(P#46.1.7.1.1.2)</summary>
                            internal void Run()
                            {
                                Execute();
                            }
                            protected override void OnLoad()
                            {
                                Exit(ExitTiming.AfterRow, () => Counter == 16);
                                KeepChildRelationCacheAlive = true;
                                AllowUserAbort = true;
                            }
                            protected override void OnLeaveRow()
                            {
                                u.VarSet(u.IndexOf(_parent._parent.vLadiesActSat) + vDetailPosition, u.CastToNumber(u.VarCurr(u.IndexOf(_parent._parent.vLadiesActSat) + vDetailPosition)) + u.Round(u.CastToNumber(u.VarCurr(u.IndexOf(_parent._parent.vDtlActSat) + vDetailPosition)), 6, 0));
                                vDetailPosition.Value++;
                            }
                            
                            
                        }
                        /// <summary>Accum Store(P#46.1.7.1.1.3)</summary>
                        /// <remark>Last change before Migration: 31/03/2004 10:07:35</remark>
                        class AccumStore : SalesAndStock.BusinessProcessBase 
                        {
                            
                            #region Columns
                            /// <summary>v:Detail Position</summary>
                            readonly NumberColumn vDetailPosition = new NumberColumn("v:Detail Position", "2");
                            #endregion
                            
                            ReadWork1 _parent;
                            
                            public AccumStore(ReadWork1 parent)
                            {
                                _parent = parent;
                                Title = "Accum Store";
                                InitializeDataView();
                            }
                            void InitializeDataView()
                            {
                                
                                
                                #region Columns
                                
                                // This task accumulates tha Actual and Last Year values for all depts
                                // into corresponding Store totals
                                Columns.Add(vDetailPosition);
                                #endregion
                            }
                            /// <summary>Accum Store(P#46.1.7.1.1.3)</summary>
                            internal void Run()
                            {
                                Execute();
                            }
                            protected override void OnLoad()
                            {
                                Exit(ExitTiming.AfterRow, () => Counter == 16);
                                KeepChildRelationCacheAlive = true;
                                AllowUserAbort = true;
                            }
                            protected override void OnLeaveRow()
                            {
                                u.VarSet(u.IndexOf(_parent._parent.vStoreActSat) + vDetailPosition, u.CastToNumber(u.VarCurr(u.IndexOf(_parent._parent.vStoreActSat) + vDetailPosition)) + u.Round(u.CastToNumber(u.VarCurr(u.IndexOf(_parent._parent.vDtlActSat) + vDetailPosition)), 6, 0));
                                vDetailPosition.Value++;
                            }
                            
                            
                        }
                    }
                    /// <summary>Init Work with Detail(P#46.1.7.1.2)</summary>
                    /// <remark>Last change before Migration: 31/03/2004 10:08:02</remark>
                    class InitWorkWithDetail : SalesAndStock.BusinessProcessBase 
                    {
                        
                        #region Columns
                        /// <summary>v:Detail Position</summary>
                        readonly NumberColumn vDetailPosition = new NumberColumn("v:Detail Position", "2");
                        #endregion
                        
                        SelectRptGroup _parent;
                        
                        public InitWorkWithDetail(SelectRptGroup parent)
                        {
                            _parent = parent;
                            Title = "Init Work with Detail";
                            InitializeDataView();
                        }
                        void InitializeDataView()
                        {
                            
                            
                            #region Columns
                            
                            // This task moves Detail level totals to work areas for
                            // the calculation of %ages
                            Columns.Add(vDetailPosition);
                            #endregion
                        }
                        /// <summary>Init Work with Detail(P#46.1.7.1.2)</summary>
                        internal void Run()
                        {
                            Execute();
                        }
                        protected override void OnLoad()
                        {
                            Exit(ExitTiming.AfterRow, () => Counter == 8);
                            KeepChildRelationCacheAlive = true;
                            AllowUserAbort = true;
                        }
                        protected override void OnLeaveRow()
                        {
                            u.VarSet(u.IndexOf(_parent.vWorkActSat) + vDetailPosition, u.Round(u.CastToNumber(u.VarCurr(u.IndexOf(_parent.vDtlActSat) + vDetailPosition)), 6, 0));
                            u.VarSet(u.IndexOf(_parent.vWorkLYSat) + vDetailPosition, u.Round(u.CastToNumber(u.VarCurr(u.IndexOf(_parent.vDtlLYSat) + vDetailPosition)), 6, 0));
                            vDetailPosition.Value++;
                        }
                        
                        
                    }
                    /// <summary>Init Work with Ladies(P#46.1.7.1.3)</summary>
                    /// <remark>Last change before Migration: 31/03/2004 10:08:11</remark>
                    class InitWorkWithLadies : SalesAndStock.BusinessProcessBase 
                    {
                        
                        #region Columns
                        /// <summary>v:Detail Position</summary>
                        readonly NumberColumn vDetailPosition = new NumberColumn("v:Detail Position", "2");
                        #endregion
                        
                        SelectRptGroup _parent;
                        
                        public InitWorkWithLadies(SelectRptGroup parent)
                        {
                            _parent = parent;
                            Title = "Init Work with Ladies";
                            InitializeDataView();
                        }
                        void InitializeDataView()
                        {
                            
                            
                            #region Columns
                            
                            // This task moves totals for Ladies to work areas for
                            // the calculation of %ages
                            Columns.Add(vDetailPosition);
                            #endregion
                        }
                        /// <summary>Init Work with Ladies(P#46.1.7.1.3)</summary>
                        internal void Run()
                        {
                            Execute();
                        }
                        protected override void OnLoad()
                        {
                            Exit(ExitTiming.AfterRow, () => Counter == 8);
                            KeepChildRelationCacheAlive = true;
                            AllowUserAbort = true;
                        }
                        protected override void OnLeaveRow()
                        {
                            u.VarSet(u.IndexOf(_parent.vWorkActSat) + vDetailPosition, u.VarCurr(u.IndexOf(_parent.vLadiesActSat) + vDetailPosition));
                            u.VarSet(u.IndexOf(_parent.vWorkLYSat) + vDetailPosition, u.VarCurr(u.IndexOf(_parent.vLadiesLYSat) + vDetailPosition));
                            vDetailPosition.Value++;
                        }
                        
                        
                    }
                    /// <summary>Init Work with Stores(P#46.1.7.1.4)</summary>
                    /// <remark>Last change before Migration: 31/03/2004 10:08:19</remark>
                    class InitWorkWithStores : SalesAndStock.BusinessProcessBase 
                    {
                        
                        #region Columns
                        /// <summary>v:Detail Position</summary>
                        readonly NumberColumn vDetailPosition = new NumberColumn("v:Detail Position", "2");
                        #endregion
                        
                        SelectRptGroup _parent;
                        
                        public InitWorkWithStores(SelectRptGroup parent)
                        {
                            _parent = parent;
                            Title = "Init Work with Stores";
                            InitializeDataView();
                        }
                        void InitializeDataView()
                        {
                            
                            
                            #region Columns
                            
                            // This task moves totals for Store to work areas for
                            // the calculation of %ages
                            Columns.Add(vDetailPosition);
                            #endregion
                        }
                        /// <summary>Init Work with Stores(P#46.1.7.1.4)</summary>
                        internal void Run()
                        {
                            Execute();
                        }
                        protected override void OnLoad()
                        {
                            Exit(ExitTiming.AfterRow, () => Counter == 8);
                            KeepChildRelationCacheAlive = true;
                            AllowUserAbort = true;
                        }
                        protected override void OnLeaveRow()
                        {
                            u.VarSet(u.IndexOf(_parent.vWorkActSat) + vDetailPosition, u.VarCurr(u.IndexOf(_parent.vStoreActSat) + vDetailPosition));
                            u.VarSet(u.IndexOf(_parent.vWorkLYSat) + vDetailPosition, u.VarCurr(u.IndexOf(_parent.vStoreLYSat) + vDetailPosition));
                            vDetailPosition.Value++;
                        }
                        
                        
                    }
                    /// <summary>Calc Sales PCents(P#46.1.7.1.5)</summary>
                    /// <remark>Last change before Migration: 27/05/2011 11:54:17</remark>
                    class CalcSalesPCents : SalesAndStock.BusinessProcessBase 
                    {
                        
                        #region Columns
                        /// <summary>v:Detail Position</summary>
                        readonly NumberColumn vDetailPosition = new NumberColumn("v:Detail Position", "2");
                        /// <summary>v:Actual</summary>
                        readonly NumberColumn vActual = new NumberColumn("v:Actual", "N9.2");
                        /// <summary>v:Last Year</summary>
                        readonly NumberColumn vLastYear = new NumberColumn("v:Last Year", "N9.2");
                        /// <summary>v:PCent</summary>
                        readonly NumberColumn vPCent = new NumberColumn("v:PCent", "N6.1");
                        #endregion
                        
                        SelectRptGroup _parent;
                        
                        public CalcSalesPCents(SelectRptGroup parent)
                        {
                            _parent = parent;
                            Title = "Calc Sales PCents";
                            InitializeDataView();
                        }
                        void InitializeDataView()
                        {
                            
                            
                            #region Columns
                            
                            // This task calculates the %age increase/decrease for each day
                            
                            Columns.Add(vDetailPosition);
                            Columns.Add(vActual);
                            Columns.Add(vLastYear);
                            Columns.Add(vPCent);
                            #endregion
                        }
                        /// <summary>Calc Sales PCents(P#46.1.7.1.5)</summary>
                        internal void Run()
                        {
                            Execute();
                        }
                        protected override void OnLoad()
                        {
                            Exit(ExitTiming.AfterRow, () => Counter == 8);
                            KeepChildRelationCacheAlive = true;
                            AllowUserAbort = true;
                        }
                        protected override void OnLeaveRow()
                        {
                            // TO MAKE EXPRESSIONS MORE LEGIBLE:-
                            // Initialise work fields with VARCURR values before calculating %age.
                            // Calculate %age.
                            // Update appropriate %age from the calculated value using VARSET.
                            vActual.Value = u.CastToNumber(u.VarCurr(u.IndexOf(_parent.vWorkActSat) + vDetailPosition));
                            vLastYear.Value = u.CastToNumber(u.VarCurr(u.IndexOf(_parent.vWorkLYSat) + vDetailPosition));
                            vPCent.Value = u.If(vActual < 1 || vLastYear < 1, 0, u.Round((vActual - vLastYear) * 100 / vLastYear, 6, 0));
                            u.VarSet(u.IndexOf(_parent.vWorkPCentSat) + vDetailPosition, vPCent);
                            
                            vDetailPosition.Value++;
                        }
                        
                        
                    }
                    /// <summary>Write Sales Work3(P#46.1.7.1.6)</summary>
                    /// <remark>Last change before Migration: 16/05/2006 10:46:02</remark>
                    class WriteSalesWork3 : SalesAndStock.BusinessProcessBase 
                    {
                        
                        #region Models
                        /// <summary>ss248 Work3</summary>
                        readonly Models.ss248Work3 ss248Work3 = new Models.ss248Work3 { KeepCacheAliveAfterExit = true, AllowRowLocking = true };
                        /// <summary>ss248 Work3</summary>
                        readonly Models.ss248Work3 ss248Work31 = new Models.ss248Work3 { KeepCacheAliveAfterExit = true, AllowRowLocking = true };
                        #endregion
                        
                        #region Columns
                        /// <summary>v:Values Exists</summary>
                        readonly BoolColumn vValuesExists = new BoolColumn("v:Values Exists");
                        /// <summary>v:PCents Exists</summary>
                        readonly BoolColumn vPCentsExists = new BoolColumn("v:PCents Exists");
                        #endregion
                        
                        SelectRptGroup _parent;
                        
                        public WriteSalesWork3(SelectRptGroup parent)
                        {
                            _parent = parent;
                            Title = "Write Sales Work3";
                            InitializeDataView();
                        }
                        void InitializeDataView()
                        {
                            
                            Relations.Add(ss248Work3, RelationType.InsertIfNotFound, 
                            		ss248Work3.ReportGroup.BindEqualTo(_parent.vWork3RptGrp).And(
                            		ss248Work3.ReportDepartment.BindEqualTo(_parent.vWork3RptDept)).And(
                            		ss248Work3.ReportSubDept.BindEqualTo(_parent.vWork3RptSubDept)).And(
                            		ss248Work3.PercentIndicator.BindEqualTo("N")), 
                            	ss248Work3.SortByss_ss248_Work3_X1);
                            
                            Relations.Add(ss248Work31, RelationType.InsertIfNotFound, 
                            		ss248Work31.ReportGroup.BindEqualTo(_parent.vWork3RptGrp).And(
                            		ss248Work31.ReportDepartment.BindEqualTo(_parent.vWork3RptDept)).And(
                            		ss248Work31.ReportSubDept.BindEqualTo(_parent.vWork3RptSubDept)).And(
                            		ss248Work31.PercentIndicator.BindEqualTo("Y")).And(
                            		ss248Work31.DepartmentDesc.BindEqualTo(" ")), 
                            	ss248Work31.SortByss_ss248_Work3_X1);
                            
                            
                            
                            #region Columns
                            
                            // Generate VALUES record
                            Columns.Add(vValuesExists);
                            Relations[ss248Work3].NotifyRowWasFoundTo(vValuesExists);
                            Columns.Add(ss248Work3.ReportGroup);
                            Columns.Add(ss248Work3.ReportDepartment);
                            Columns.Add(ss248Work3.ReportSubDept);
                            Columns.Add(ss248Work3.PercentIndicator);
                            Columns.Add(ss248Work3.DepartmentDesc).BindValue(_parent.vWork3DeptName);
                            Columns.Add(ss248Work3.SatAct).BindValue(_parent.vWorkActSat);
                            Columns.Add(ss248Work3.SatLY).BindValue(_parent.vWorkLYSat);
                            Columns.Add(ss248Work3.SunAct).BindValue(_parent.vWorkActSun);
                            Columns.Add(ss248Work3.SunLY).BindValue(_parent.vWorkLYSun);
                            Columns.Add(ss248Work3.MonAct).BindValue(_parent.vWorkActMon);
                            Columns.Add(ss248Work3.MonLY).BindValue(_parent.vWorkLYMon);
                            Columns.Add(ss248Work3.TueAct).BindValue(_parent.vWorkActTue);
                            Columns.Add(ss248Work3.TueLY).BindValue(_parent.vWorkLYTue);
                            Columns.Add(ss248Work3.WedAct).BindValue(_parent.vWorkActWed);
                            Columns.Add(ss248Work3.WedLY).BindValue(_parent.vWorkLYWed);
                            Columns.Add(ss248Work3.ThuAct).BindValue(_parent.vWorkActThu);
                            Columns.Add(ss248Work3.ThuLY).BindValue(_parent.vWorkLYThu);
                            Columns.Add(ss248Work3.FriAct).BindValue(_parent.vWorkActFri);
                            Columns.Add(ss248Work3.FriLY).BindValue(_parent.vWorkLYFri);
                            Columns.Add(ss248Work3.WeekAct).BindValue(_parent.vWorkActWeek);
                            Columns.Add(ss248Work3.WeekLY).BindValue(_parent.vWorkLYWeek);
                            // Generate PERCENTAGES record
                            Columns.Add(vPCentsExists);
                            Relations[ss248Work31].NotifyRowWasFoundTo(vPCentsExists);
                            Columns.Add(ss248Work31.ReportGroup);
                            Columns.Add(ss248Work31.ReportDepartment);
                            Columns.Add(ss248Work31.ReportSubDept);
                            Columns.Add(ss248Work31.PercentIndicator);
                            Columns.Add(ss248Work31.DepartmentDesc);
                            Columns.Add(ss248Work31.SatAct).BindValue(_parent.vWorkPCentSat);
                            Columns.Add(ss248Work31.SatLY).BindValue(() => 0);
                            Columns.Add(ss248Work31.SunAct).BindValue(_parent.vWorkPCentSun);
                            Columns.Add(ss248Work31.SunLY).BindValue(() => 0);
                            Columns.Add(ss248Work31.MonAct).BindValue(_parent.vWorkPCentMon);
                            Columns.Add(ss248Work31.MonLY).BindValue(() => 0);
                            Columns.Add(ss248Work31.TueAct).BindValue(_parent.vWorkPCentTue);
                            Columns.Add(ss248Work31.TueLY).BindValue(() => 0);
                            Columns.Add(ss248Work31.WedAct).BindValue(_parent.vWorkPCentWed);
                            Columns.Add(ss248Work31.WedLY).BindValue(() => 0);
                            Columns.Add(ss248Work31.ThuAct).BindValue(_parent.vWorkPCentThu);
                            Columns.Add(ss248Work31.ThuLY).BindValue(() => 0);
                            Columns.Add(ss248Work31.FriAct).BindValue(_parent.vWorkPCentFri);
                            Columns.Add(ss248Work31.FriLY).BindValue(() => 0);
                            Columns.Add(ss248Work31.WeekAct).BindValue(_parent.vWorkPCentWeek);
                            Columns.Add(ss248Work31.WeekLY).BindValue(() => 0);
                            #endregion
                        }
                        /// <summary>Write Sales Work3(P#46.1.7.1.6)</summary>
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
                        
                        
                    }
                    /// <summary>ICR or Mack Card(P#46.1.7.1.7)</summary>
                    /// <remark>Last change before Migration: 16/05/2006 13:57:49</remark>
                    class ICROrMackCard : SalesAndStock.BusinessProcessBase 
                    {
                        
                        #region Models
                        /// <summary>ss248 Work1</summary>
                        readonly Models.ss248Work1 ss248Work1 = new Models.ss248Work1 { KeepCacheAliveAfterExit = true, AllowRowLocking = true };
                        /// <summary>ss248 Work3</summary>
                        readonly Models.ss248Work3 ss248Work3 = new Models.ss248Work3 { KeepCacheAliveAfterExit = true, AllowRowLocking = true };
                        /// <summary>ss248 Work3</summary>
                        readonly Models.ss248Work3 ss248Work31 = new Models.ss248Work3 { KeepCacheAliveAfterExit = true, AllowRowLocking = true };
                        #endregion
                        
                        SelectRptGroup _parent;
                        
                        public ICROrMackCard(SelectRptGroup parent)
                        {
                            _parent = parent;
                            Title = "ICR or Mack Card";
                            InitializeDataView();
                        }
                        void InitializeDataView()
                        {
                            
                            #region Relations
                            
                            Relations.Add(ss248Work1, 
                            		ss248Work1.FinancialWeek.IsEqualTo(_parent._parent._parent._parent.vReqYYYYWW).And(
                            		ss248Work1.BranchNumber.IsEqualTo(_parent._parent.DiaryWork1.BranchNumber)).And(
                            		ss248Work1.ReportGroup.IsEqualTo(_parent.vWork3RptGrp)).And(
                            		ss248Work1.DeptReportSequence.IsEqualTo(99)).And(
                            		ss248Work1.Department1.IsEqualTo(_parent.vWork3RptDept)).And(
                            		ss248Work1.SubDepartment1.IsEqualTo(_parent.vWork3RptSubDept)), 
                            	ss248Work1.SortByss_ss248_Work1_X1);
                            
                            Relations.Add(ss248Work3, RelationType.InsertIfNotFound, 
                            		ss248Work3.ReportGroup.BindEqualTo(_parent.vWork3RptGrp).And(
                            		ss248Work3.ReportDepartment.BindEqualTo(_parent.vWork3RptDept)).And(
                            		ss248Work3.ReportSubDept.BindEqualTo(_parent.vWork3RptSubDept)).And(
                            		ss248Work3.PercentIndicator.BindEqualTo("N")).And(
                            		ss248Work3.DepartmentDesc.BindEqualTo(_parent.vWork3DeptName)), 
                            	ss248Work3.SortByss_ss248_Work3_X1);
                            
                            Relations.Add(ss248Work31, RelationType.InsertIfNotFound, 
                            		ss248Work31.ReportGroup.BindEqualTo(_parent.vWork3RptGrp).And(
                            		ss248Work31.ReportDepartment.BindEqualTo(_parent.vWork3RptDept)).And(
                            		ss248Work31.ReportSubDept.BindEqualTo(_parent.vWork3RptSubDept)).And(
                            		ss248Work31.PercentIndicator.BindEqualTo("Y")).And(
                            		ss248Work31.DepartmentDesc.BindEqualTo(" ")), 
                            	ss248Work31.SortByss_ss248_Work3_X1);
                            
                            #endregion
                            
                            
                            #region Columns
                            
                            Columns.Add(ss248Work1.FinancialWeek);
                            Columns.Add(ss248Work1.BranchNumber);
                            Columns.Add(ss248Work1.ReportGroup);
                            Columns.Add(ss248Work1.DeptReportSequence);
                            Columns.Add(ss248Work1.Department1);
                            Columns.Add(ss248Work1.SubDepartment1);
                            Columns.Add(ss248Work1.TYSaturday);
                            Columns.Add(ss248Work1.TYSunday);
                            Columns.Add(ss248Work1.TYMonday);
                            Columns.Add(ss248Work1.TYTuesday);
                            Columns.Add(ss248Work1.TYWednesday);
                            Columns.Add(ss248Work1.TYThursday);
                            Columns.Add(ss248Work1.TYFriday);
                            Columns.Add(ss248Work1.TYWeekToDate);
                            Columns.Add(ss248Work1.LYSaturday);
                            Columns.Add(ss248Work1.LYSunday);
                            Columns.Add(ss248Work1.LYMonday);
                            Columns.Add(ss248Work1.LYTuesday);
                            Columns.Add(ss248Work1.LYWednesday);
                            Columns.Add(ss248Work1.LYThursday);
                            Columns.Add(ss248Work1.LYFriday);
                            Columns.Add(ss248Work1.LYWeekToDate);
                            
                            // Write VALUES record
                            Columns.Add(ss248Work3.ReportGroup);
                            Columns.Add(ss248Work3.ReportDepartment);
                            Columns.Add(ss248Work3.ReportSubDept);
                            Columns.Add(ss248Work3.PercentIndicator);
                            Columns.Add(ss248Work3.DepartmentDesc);
                            Columns.Add(ss248Work3.SatAct).BindValue(() => u.Round(ss248Work1.TYSaturday, 6, 0));
                            Columns.Add(ss248Work3.SatLY).BindValue(() => u.Round(ss248Work1.LYSaturday, 6, 0));
                            Columns.Add(ss248Work3.SunAct).BindValue(() => u.Round(ss248Work1.TYSunday, 6, 0));
                            Columns.Add(ss248Work3.SunLY).BindValue(() => u.Round(ss248Work1.LYSunday, 6, 0));
                            Columns.Add(ss248Work3.MonAct).BindValue(() => u.Round(ss248Work1.TYMonday, 6, 0));
                            Columns.Add(ss248Work3.MonLY).BindValue(() => u.Round(ss248Work1.LYMonday, 6, 0));
                            Columns.Add(ss248Work3.TueAct).BindValue(() => u.Round(ss248Work1.TYTuesday, 6, 0));
                            Columns.Add(ss248Work3.TueLY).BindValue(() => u.Round(ss248Work1.LYTuesday, 6, 0));
                            Columns.Add(ss248Work3.WedAct).BindValue(() => u.Round(ss248Work1.TYWednesday, 6, 0));
                            Columns.Add(ss248Work3.WedLY).BindValue(() => u.Round(ss248Work1.LYWednesday, 6, 0));
                            Columns.Add(ss248Work3.ThuAct).BindValue(() => u.Round(ss248Work1.TYThursday, 6, 0));
                            Columns.Add(ss248Work3.ThuLY).BindValue(() => u.Round(ss248Work1.LYThursday, 6, 0));
                            Columns.Add(ss248Work3.FriAct).BindValue(() => u.Round(ss248Work1.TYFriday, 6, 0));
                            Columns.Add(ss248Work3.FriLY).BindValue(() => u.Round(ss248Work1.LYFriday, 6, 0));
                            Columns.Add(ss248Work3.WeekAct).BindValue(() => u.Round(ss248Work1.TYWeekToDate, 6, 0));
                            Columns.Add(ss248Work3.WeekLY).BindValue(() => u.Round(ss248Work1.LYWeekToDate, 6, 0));
                            // Write PERCENTAGES record
                            Columns.Add(ss248Work31.ReportGroup);
                            Columns.Add(ss248Work31.ReportDepartment);
                            Columns.Add(ss248Work31.ReportSubDept);
                            Columns.Add(ss248Work31.PercentIndicator);
                            Columns.Add(ss248Work31.DepartmentDesc);
                            Columns.Add(ss248Work31.SatAct);
                            Columns.Add(ss248Work31.SatLY);
                            Columns.Add(ss248Work31.SunAct);
                            Columns.Add(ss248Work31.SunLY);
                            Columns.Add(ss248Work31.MonAct);
                            Columns.Add(ss248Work31.MonLY);
                            Columns.Add(ss248Work31.TueAct);
                            Columns.Add(ss248Work31.TueLY);
                            Columns.Add(ss248Work31.WedAct);
                            Columns.Add(ss248Work31.WedLY);
                            Columns.Add(ss248Work31.ThuAct);
                            Columns.Add(ss248Work31.ThuLY);
                            Columns.Add(ss248Work31.FriAct);
                            Columns.Add(ss248Work31.FriLY);
                            Columns.Add(ss248Work31.WeekAct);
                            Columns.Add(ss248Work31.WeekLY);
                            #endregion
                        }
                        /// <summary>ICR or Mack Card(P#46.1.7.1.7)</summary>
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
                            // Generate percentage figures on second record being written.
                            // These percentages are not required for Instant Credits.
                            if(_parent.vWork3RptGrp == "C")
                            {
                                ss248Work31.SatAct.Value = u.If(ss248Work3.SatAct == 0 || _parent.vStoreActSat == 0, 0, u.Round(ss248Work3.SatAct * 100 / _parent.vStoreActSat, 4, 1));
                                ss248Work31.SatLY.Value = u.If(ss248Work3.SatLY == 0 || _parent.vStoreLYSat == 0, 0, u.Round(ss248Work3.SatLY * 100 / _parent.vStoreLYSat, 4, 1));
                                ss248Work31.SunAct.Value = u.If(ss248Work3.SunAct == 0 || _parent.vStoreActSun == 0, 0, u.Round(ss248Work3.SunAct * 100 / _parent.vStoreActSun, 4, 1));
                                ss248Work31.SunLY.Value = u.If(ss248Work3.SunLY == 0 || _parent.vStoreLYSun == 0, 0, u.Round(ss248Work3.SunLY * 100 / _parent.vStoreLYSun, 4, 1));
                                ss248Work31.MonAct.Value = u.If(ss248Work3.MonAct == 0 || _parent.vStoreActMon == 0, 0, u.Round(ss248Work3.MonAct * 100 / _parent.vStoreActMon, 4, 1));
                                ss248Work31.MonLY.Value = u.If(ss248Work3.MonLY == 0 || _parent.vStoreLYMon == 0, 0, u.Round(ss248Work3.MonLY * 100 / _parent.vStoreLYMon, 4, 1));
                                ss248Work31.TueAct.Value = u.If(ss248Work3.TueAct == 0 || _parent.vStoreActTue == 0, 0, u.Round(ss248Work3.TueAct * 100 / _parent.vStoreActTue, 4, 1));
                                ss248Work31.TueLY.Value = u.If(ss248Work3.TueLY == 0 || _parent.vStoreLYTue == 0, 0, u.Round(ss248Work3.TueLY * 100 / _parent.vStoreLYTue, 4, 1));
                                ss248Work31.WedAct.Value = u.If(ss248Work3.WedAct == 0 || _parent.vStoreActWed == 0, 0, u.Round(ss248Work3.WedAct * 100 / _parent.vStoreActWed, 4, 1));
                                ss248Work31.WedLY.Value = u.If(ss248Work3.WedLY == 0 || _parent.vStoreLYWed == 0, 0, u.Round(ss248Work3.WedLY * 100 / _parent.vStoreLYWed, 4, 1));
                                ss248Work31.ThuAct.Value = u.If(ss248Work3.ThuAct == 0 || _parent.vStoreActThu == 0, 0, u.Round(ss248Work3.ThuAct * 100 / _parent.vStoreActThu, 4, 1));
                                ss248Work31.ThuLY.Value = u.If(ss248Work3.ThuLY == 0 || _parent.vStoreLYThu == 0, 0, u.Round(ss248Work3.ThuLY * 100 / _parent.vStoreLYThu, 4, 1));
                                ss248Work31.FriAct.Value = u.If(ss248Work3.FriAct == 0 || _parent.vStoreActFri == 0, 0, u.Round(ss248Work3.FriAct * 100 / _parent.vStoreActFri, 4, 1));
                                ss248Work31.FriLY.Value = u.If(ss248Work3.FriLY == 0 || _parent.vStoreLYFri == 0, 0, u.Round(ss248Work3.FriLY * 100 / _parent.vStoreLYFri, 4, 1));
                                ss248Work31.WeekAct.Value = u.If(ss248Work3.WeekAct == 0 || _parent.vStoresActWeek == 0, 0, u.Round(ss248Work3.WeekAct * 100 / _parent.vStoresActWeek, 4, 1));
                                ss248Work31.WeekLY.Value = u.If(ss248Work3.WeekLY == 0 || _parent.vStoreLYWeek == 0, 0, u.Round(ss248Work3.WeekLY * 100 / _parent.vStoreLYWeek, 4, 1));
                            }
                        }
                        
                        
                    }
                    /// <summary>Generate Blank Line(P#46.1.7.1.8)</summary>
                    /// <remark>Last change before Migration: 16/05/2006 10:47:11</remark>
                    class GenerateBlankLine : SalesAndStock.BusinessProcessBase 
                    {
                        
                        #region Models
                        /// <summary>ss248 Work3</summary>
                        readonly Models.ss248Work3 ss248Work3 = new Models.ss248Work3 { KeepCacheAliveAfterExit = true, AllowRowLocking = true };
                        #endregion
                        
                        SelectRptGroup _parent;
                        
                        public GenerateBlankLine(SelectRptGroup parent)
                        {
                            _parent = parent;
                            Title = "Generate Blank Line";
                            InitializeDataView();
                        }
                        void InitializeDataView()
                        {
                            
                            Relations.Add(ss248Work3, RelationType.InsertIfNotFound, 
                            		ss248Work3.ReportGroup.BindEqualTo(_parent.vWork3RptGrp).And(
                            		ss248Work3.ReportDepartment.BindEqualTo(_parent.vWork3RptDept)).And(
                            		ss248Work3.ReportSubDept.BindEqualTo(_parent.vWork3RptSubDept)).And(
                            		ss248Work3.PercentIndicator.BindEqualTo("B")).And(
                            		ss248Work3.DepartmentDesc.BindEqualTo(_parent.vWork3DeptName)), 
                            	ss248Work3.SortByss_ss248_Work3_X1);
                            
                            
                            
                            #region Columns
                            
                            // Write Work3 record to cause a blank line on the resulting report/
                            Columns.Add(ss248Work3.ReportGroup);
                            Columns.Add(ss248Work3.ReportDepartment);
                            Columns.Add(ss248Work3.ReportSubDept);
                            Columns.Add(ss248Work3.PercentIndicator);
                            Columns.Add(ss248Work3.DepartmentDesc);
                            #endregion
                        }
                        /// <summary>Generate Blank Line(P#46.1.7.1.8)</summary>
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
                        
                        
                    }
                    /// <summary>Read Work1 Concessions(P#46.1.7.1.9)</summary>
                    /// <remark>Last change before Migration: 16/05/2006 15:08:25</remark>
                    class ReadWork1Concessions : SalesAndStock.BusinessProcessBase 
                    {
                        
                        #region Models
                        /// <summary>ss248 Work1</summary>
                        readonly Models.ss248Work1 ss248Work1 = new Models.ss248Work1 { AllowRowLocking = true };
                        #endregion
                        
                        #region Columns
                        /// <summary>v:Concessions Written</summary>
                        readonly NumberColumn vConcessionsWritten = new NumberColumn("v:Concessions Written", "4");
                        #endregion
                        
                        SelectRptGroup _parent;
                        
                        public ReadWork1Concessions(SelectRptGroup parent)
                        {
                            _parent = parent;
                            Title = "Read Work1 Concessions";
                            InitializeDataView();
                            var ss248Work1DeptReportSequenceGroup = Groups.Add(ss248Work1.DeptReportSequence);
                            ss248Work1DeptReportSequenceGroup.Enter += ss248Work1DeptReportSequenceGroup_Enter;
                            ss248Work1DeptReportSequenceGroup.Leave += ss248Work1DeptReportSequenceGroup_Leave;
                        }
                        void InitializeDataView()
                        {
                            From = ss248Work1;
                            
                            #region Where
                            
                            Where.Add(ss248Work1.FinancialWeek.IsEqualTo(_parent._parent._parent._parent.vReqYYYYWW));
                            Where.Add(ss248Work1.BranchNumber.IsEqualTo(_parent._parent.DiaryWork1.BranchNumber));
                            Where.Add(ss248Work1.ReportGroup.IsEqualTo("E"));
                            Where.Add(ss248Work1.DeptReportSequence.IsBetween(1, 99));
                            Where.Add(ss248Work1.Department1.IsEqualTo("99"));
                            Where.Add(ss248Work1.SubDepartment1.IsEqualTo("99"));
                            #endregion
                            
                            OrderBy = ss248Work1.SortByss_ss248_Work1_X1;
                            
                            #region Columns
                            
                            // This subtree has been added to handle the reformatting of Concession data from
                            // table Work1 into table Work3 prior to printing.  It has been adapted from
                            // existing task 'Read Work1'.   Strictly speaking no accumulation of data should
                            // be required for Concessions.  However, already-established logic can be applied.
                            
                            Columns.Add(ss248Work1.FinancialWeek);
                            Columns.Add(ss248Work1.BranchNumber);
                            Columns.Add(ss248Work1.ReportGroup);
                            Columns.Add(ss248Work1.DeptReportSequence);
                            Columns.Add(ss248Work1.Department1);
                            Columns.Add(ss248Work1.SubDepartment1);
                            Columns.Add(ss248Work1.TYSaturday);
                            Columns.Add(ss248Work1.TYSunday);
                            Columns.Add(ss248Work1.TYMonday);
                            Columns.Add(ss248Work1.TYTuesday);
                            Columns.Add(ss248Work1.TYWednesday);
                            Columns.Add(ss248Work1.TYThursday);
                            Columns.Add(ss248Work1.TYFriday);
                            Columns.Add(ss248Work1.TYWeekToDate);
                            Columns.Add(ss248Work1.LYSaturday);
                            Columns.Add(ss248Work1.LYSunday);
                            Columns.Add(ss248Work1.LYMonday);
                            Columns.Add(ss248Work1.LYTuesday);
                            Columns.Add(ss248Work1.LYWednesday);
                            Columns.Add(ss248Work1.LYThursday);
                            Columns.Add(ss248Work1.LYFriday);
                            Columns.Add(ss248Work1.LYWeekToDate);
                            
                            Columns.Add(vConcessionsWritten);
                            #endregion
                        }
                        /// <summary>Read Work1 Concessions(P#46.1.7.1.9)</summary>
                        internal void Run()
                        {
                            Execute();
                        }
                        protected override void OnLoad()
                        {
                            KeepChildRelationCacheAlive = true;
                            AllowUserAbort = true;
                        }
                        protected override void OnStart()
                        {
                            Cached<WriteConcDesc>().Run();
                        }
                        void ss248Work1DeptReportSequenceGroup_Enter()
                        {
                            _parent.vDtlActSat.Value = 0;
                            _parent.vDtlActSun.Value = 0;
                            _parent.vDtlActMon.Value = 0;
                            _parent.vDtlActTue.Value = 0;
                            _parent.vDtlActWed.Value = 0;
                            _parent.vDtlActThu.Value = 0;
                            _parent.vDtlActFri.Value = 0;
                            _parent.vDtlActWeek.Value = 0;
                            _parent.vDtlLYSat.Value = 0;
                            _parent.vDtlLYSun.Value = 0;
                            _parent.vDtlLYMon.Value = 0;
                            _parent.vDtlLYTue.Value = 0;
                            _parent.vDtlLYWed.Value = 0;
                            _parent.vDtlLYThu.Value = 0;
                            _parent.vDtlLYFri.Value = 0;
                            _parent.vDtlLYWeek.Value = 0;
                        }
                        protected override void OnEnterRow()
                        {
                            Cached<AccumConc>().Run();
                        }
                        void ss248Work1DeptReportSequenceGroup_Leave()
                        {
                            Cached<InitWorkWithConcDetail>().Run();
                            Cached<CalcConcPCents>().Run();
                            Cached<WriteConcWork3>().Run();
                        }
                        protected override void OnEnd()
                        {
                            if(vConcessionsWritten != 0)
                            {
                                _parent._parent.vConcessionsPresent.Value = "Y";
                            }
                        }
                        
                        
                        /// <summary>Accum Conc(P#46.1.7.1.9.1)</summary>
                        /// <remark>Last change before Migration: 11/05/2006 11:15:23</remark>
                        class AccumConc : SalesAndStock.BusinessProcessBase 
                        {
                            
                            #region Columns
                            /// <summary>v:Detail Position</summary>
                            readonly NumberColumn vDetailPosition = new NumberColumn("v:Detail Position", "2");
                            #endregion
                            
                            ReadWork1Concessions _parent;
                            
                            public AccumConc(ReadWork1Concessions parent)
                            {
                                _parent = parent;
                                Title = "Accum Conc";
                                InitializeDataView();
                            }
                            void InitializeDataView()
                            {
                                
                                
                                #region Columns
                                
                                // This task accumulates the Actual and Last Year values from Work1
                                // into the relevant positions for printing at detail level.
                                Columns.Add(vDetailPosition);
                                #endregion
                            }
                            /// <summary>Accum Conc(P#46.1.7.1.9.1)</summary>
                            internal void Run()
                            {
                                Execute();
                            }
                            protected override void OnLoad()
                            {
                                Exit(ExitTiming.AfterRow, () => Counter == 16);
                                KeepChildRelationCacheAlive = true;
                                AllowUserAbort = true;
                            }
                            protected override void OnLeaveRow()
                            {
                                u.VarSet(u.IndexOf(_parent._parent.vDtlActSat) + vDetailPosition, u.CastToNumber(u.VarCurr(u.IndexOf(_parent._parent.vDtlActSat) + vDetailPosition)) + u.CastToNumber(u.VarCurr(u.IndexOf(_parent.ss248Work1.TYSaturday) + vDetailPosition)));
                                vDetailPosition.Value++;
                            }
                            protected override void OnEnd()
                            {
                                Cached<DebugAccumDtl>().Run();
                            }
                            
                            
                            /// <summary>Debug Accum Dtl(P#46.1.7.1.9.1.1)</summary>
                            /// <remark>Last change before Migration: 31/03/2004 09:59:49</remark>
                            class DebugAccumDtl : SalesAndStock.BusinessProcessBase 
                            {
                                AccumConc _parent;
                                
                                public DebugAccumDtl(AccumConc parent)
                                {
                                    _parent = parent;
                                    Title = "Debug Accum Dtl";
                                    InitializeDataView();
                                }
                                void InitializeDataView()
                                {
                                    
                                    
                                }
                                /// <summary>Debug Accum Dtl(P#46.1.7.1.9.1.1)</summary>
                                internal void Run()
                                {
                                    Execute();
                                }
                                protected override void OnLoad()
                                {
                                    Exit(ExitTiming.AfterRow);
                                    Activity = Activities.Browse;
                                    AllowUserAbort = true;
                                }
                                
                                
                            }
                        }
                        /// <summary>Init Work with Conc Detail(P#46.1.7.1.9.2)</summary>
                        /// <remark>Last change before Migration: 11/05/2006 11:27:59</remark>
                        class InitWorkWithConcDetail : SalesAndStock.BusinessProcessBase 
                        {
                            
                            #region Columns
                            /// <summary>v:Detail Position</summary>
                            readonly NumberColumn vDetailPosition = new NumberColumn("v:Detail Position", "2");
                            #endregion
                            
                            ReadWork1Concessions _parent;
                            
                            public InitWorkWithConcDetail(ReadWork1Concessions parent)
                            {
                                _parent = parent;
                                Title = "Init Work with Conc Detail";
                                InitializeDataView();
                            }
                            void InitializeDataView()
                            {
                                
                                
                                #region Columns
                                
                                // This task moves Detail level totals for a Concession to work areas for
                                // the calculation of %ages
                                Columns.Add(vDetailPosition);
                                #endregion
                            }
                            /// <summary>Init Work with Conc Detail(P#46.1.7.1.9.2)</summary>
                            internal void Run()
                            {
                                Execute();
                            }
                            protected override void OnLoad()
                            {
                                Exit(ExitTiming.AfterRow, () => Counter == 8);
                                KeepChildRelationCacheAlive = true;
                                AllowUserAbort = true;
                            }
                            protected override void OnLeaveRow()
                            {
                                u.VarSet(u.IndexOf(_parent._parent.vWorkActSat) + vDetailPosition, u.Round(u.CastToNumber(u.VarCurr(u.IndexOf(_parent._parent.vDtlActSat) + vDetailPosition)), 6, 0));
                                u.VarSet(u.IndexOf(_parent._parent.vWorkLYSat) + vDetailPosition, u.Round(u.CastToNumber(u.VarCurr(u.IndexOf(_parent._parent.vDtlLYSat) + vDetailPosition)), 6, 0));
                                vDetailPosition.Value++;
                            }
                            
                            
                        }
                        /// <summary>Calc Conc PCents(P#46.1.7.1.9.3)</summary>
                        /// <remark>Last change before Migration: 11/05/2006 11:36:52</remark>
                        class CalcConcPCents : SalesAndStock.BusinessProcessBase 
                        {
                            
                            #region Columns
                            /// <summary>v:Detail Position</summary>
                            readonly NumberColumn vDetailPosition = new NumberColumn("v:Detail Position", "2");
                            /// <summary>v:Actual</summary>
                            readonly NumberColumn vActual = new NumberColumn("v:Actual", "N9.2");
                            /// <summary>v:Last Year</summary>
                            readonly NumberColumn vLastYear = new NumberColumn("v:Last Year", "N9.2");
                            /// <summary>v:PCent</summary>
                            readonly NumberColumn vPCent = new NumberColumn("v:PCent", "N6.1");
                            #endregion
                            
                            ReadWork1Concessions _parent;
                            
                            public CalcConcPCents(ReadWork1Concessions parent)
                            {
                                _parent = parent;
                                Title = "Calc Conc PCents";
                                InitializeDataView();
                            }
                            void InitializeDataView()
                            {
                                
                                
                                #region Columns
                                
                                // This task calculates the %age increase/decrease for each day's Concessions.
                                
                                Columns.Add(vDetailPosition);
                                Columns.Add(vActual);
                                Columns.Add(vLastYear);
                                Columns.Add(vPCent);
                                #endregion
                            }
                            /// <summary>Calc Conc PCents(P#46.1.7.1.9.3)</summary>
                            internal void Run()
                            {
                                Execute();
                            }
                            protected override void OnLoad()
                            {
                                Exit(ExitTiming.AfterRow, () => Counter == 8);
                                KeepChildRelationCacheAlive = true;
                                AllowUserAbort = true;
                            }
                            protected override void OnLeaveRow()
                            {
                                // TO MAKE EXPRESSIONS MORE LEGIBLE:-
                                // Initialise work fields with VARCURR values before calculating %age.
                                // Calculate %age.
                                // Update appropriate %age from the calculated value using VARSET.
                                vActual.Value = u.CastToNumber(u.VarCurr(u.IndexOf(_parent._parent.vWorkActSat) + vDetailPosition));
                                vLastYear.Value = u.CastToNumber(u.VarCurr(u.IndexOf(_parent._parent.vWorkLYSat) + vDetailPosition));
                                vPCent.Value = u.If(vActual == 0 || vLastYear == 0, 0, u.Round((vActual - vLastYear) * 100 / vLastYear, 6, 0));
                                u.VarSet(u.IndexOf(_parent._parent.vWorkPCentSat) + vDetailPosition, vPCent);
                                
                                vDetailPosition.Value++;
                            }
                            
                            
                        }
                        /// <summary>Write Conc Work3(P#46.1.7.1.9.4)</summary>
                        /// <remark>Last change before Migration: 16/05/2006 10:47:42</remark>
                        class WriteConcWork3 : SalesAndStock.BusinessProcessBase 
                        {
                            
                            #region Models
                            /// <summary>Concession Code</summary>
                            readonly Models.ConcessionCode ConcessionCode = new Models.ConcessionCode { ReadOnly = true };
                            /// <summary>ss248 Work3</summary>
                            readonly Models.ss248Work3 ss248Work3 = new Models.ss248Work3 { KeepCacheAliveAfterExit = true, AllowRowLocking = true };
                            /// <summary>ss248 Work3</summary>
                            readonly Models.ss248Work3 ss248Work31 = new Models.ss248Work3 { KeepCacheAliveAfterExit = true, AllowRowLocking = true };
                            #endregion
                            
                            #region Columns
                            /// <summary>v:Conc Found</summary>
                            readonly BoolColumn vConcFound = new BoolColumn("v:Conc Found");
                            /// <summary>v:Conc Name</summary>
                            readonly TextColumn vConcName = new TextColumn("v:Conc Name", "12");
                            /// <summary>v:Values Exists</summary>
                            readonly BoolColumn vValuesExists = new BoolColumn("v:Values Exists");
                            /// <summary>v:PCents Exists</summary>
                            readonly BoolColumn vPCentsExists = new BoolColumn("v:PCents Exists");
                            #endregion
                            
                            ReadWork1Concessions _parent;
                            
                            public WriteConcWork3(ReadWork1Concessions parent)
                            {
                                _parent = parent;
                                Title = "Write Conc Work3";
                                InitializeDataView();
                            }
                            void InitializeDataView()
                            {
                                
                                #region Relations
                                
                                Relations.Add(ConcessionCode, 
                                		ConcessionCode.ConcessionCode1.IsEqualTo(_parent.ss248Work1.DeptReportSequence), 
                                	ConcessionCode.SortByREF_Concess_X1);
                                
                                Relations.Add(ss248Work3, RelationType.InsertIfNotFound, 
                                		ss248Work3.ReportGroup.BindEqualTo("E").And(
                                		ss248Work3.ReportDepartment.BindEqualTo(() => u.Str(_parent.ss248Work1.DeptReportSequence, "2P0"))).And(
                                		ss248Work3.ReportSubDept.BindEqualTo(_parent._parent.vWork3RptSubDept)).And(
                                		ss248Work3.PercentIndicator.BindEqualTo("N")), 
                                	ss248Work3.SortByss_ss248_Work3_X1);
                                
                                Relations.Add(ss248Work31, RelationType.InsertIfNotFound, 
                                		ss248Work31.ReportGroup.BindEqualTo("E").And(
                                		ss248Work31.ReportDepartment.BindEqualTo(() => u.Str(_parent.ss248Work1.DeptReportSequence, "2P0"))).And(
                                		ss248Work31.ReportSubDept.BindEqualTo(_parent._parent.vWork3RptSubDept)).And(
                                		ss248Work31.PercentIndicator.BindEqualTo("Y")).And(
                                		ss248Work31.DepartmentDesc.BindEqualTo(" ")), 
                                	ss248Work31.SortByss_ss248_Work3_X1);
                                
                                #endregion
                                
                                
                                #region Columns
                                
                                // Get Concession name
                                Columns.Add(vConcFound);
                                Relations[ConcessionCode].NotifyRowWasFoundTo(vConcFound);
                                Columns.Add(ConcessionCode.ConcessionCode1);
                                Columns.Add(ConcessionCode.ConcessionName);
                                Columns.Add(vConcName).BindValue(() => u.If(vConcFound, u.Left(ConcessionCode.ConcessionName, 12), "Concn " + u.Str(_parent.ss248Work1.DeptReportSequence, "2P0")));
                                // Generate VALUES record
                                Columns.Add(vValuesExists);
                                Relations[ss248Work3].NotifyRowWasFoundTo(vValuesExists);
                                Columns.Add(ss248Work3.ReportGroup);
                                Columns.Add(ss248Work3.ReportDepartment);
                                Columns.Add(ss248Work3.ReportSubDept);
                                Columns.Add(ss248Work3.PercentIndicator);
                                Columns.Add(ss248Work3.DepartmentDesc).BindValue(vConcName);
                                Columns.Add(ss248Work3.SatAct).BindValue(_parent._parent.vWorkActSat);
                                Columns.Add(ss248Work3.SatLY).BindValue(_parent._parent.vWorkLYSat);
                                Columns.Add(ss248Work3.SunAct).BindValue(_parent._parent.vWorkActSun);
                                Columns.Add(ss248Work3.SunLY).BindValue(_parent._parent.vWorkLYSun);
                                Columns.Add(ss248Work3.MonAct).BindValue(_parent._parent.vWorkActMon);
                                Columns.Add(ss248Work3.MonLY).BindValue(_parent._parent.vWorkLYMon);
                                Columns.Add(ss248Work3.TueAct).BindValue(_parent._parent.vWorkActTue);
                                Columns.Add(ss248Work3.TueLY).BindValue(_parent._parent.vWorkLYTue);
                                Columns.Add(ss248Work3.WedAct).BindValue(_parent._parent.vWorkActWed);
                                Columns.Add(ss248Work3.WedLY).BindValue(_parent._parent.vWorkLYWed);
                                Columns.Add(ss248Work3.ThuAct).BindValue(_parent._parent.vWorkActThu);
                                Columns.Add(ss248Work3.ThuLY).BindValue(_parent._parent.vWorkLYThu);
                                Columns.Add(ss248Work3.FriAct).BindValue(_parent._parent.vWorkActFri);
                                Columns.Add(ss248Work3.FriLY).BindValue(_parent._parent.vWorkLYFri);
                                Columns.Add(ss248Work3.WeekAct).BindValue(_parent._parent.vWorkActWeek);
                                Columns.Add(ss248Work3.WeekLY).BindValue(_parent._parent.vWorkLYWeek);
                                // Generate PERCENTAGES record
                                Columns.Add(vPCentsExists);
                                Relations[ss248Work31].NotifyRowWasFoundTo(vPCentsExists);
                                Columns.Add(ss248Work31.ReportGroup);
                                Columns.Add(ss248Work31.ReportDepartment);
                                Columns.Add(ss248Work31.ReportSubDept);
                                Columns.Add(ss248Work31.PercentIndicator);
                                Columns.Add(ss248Work31.DepartmentDesc);
                                Columns.Add(ss248Work31.SatAct).BindValue(_parent._parent.vWorkPCentSat);
                                Columns.Add(ss248Work31.SatLY).BindValue(() => 0);
                                Columns.Add(ss248Work31.SunAct).BindValue(_parent._parent.vWorkPCentSun);
                                Columns.Add(ss248Work31.SunLY).BindValue(() => 0);
                                Columns.Add(ss248Work31.MonAct).BindValue(_parent._parent.vWorkPCentMon);
                                Columns.Add(ss248Work31.MonLY).BindValue(() => 0);
                                Columns.Add(ss248Work31.TueAct).BindValue(_parent._parent.vWorkPCentTue);
                                Columns.Add(ss248Work31.TueLY).BindValue(() => 0);
                                Columns.Add(ss248Work31.WedAct).BindValue(_parent._parent.vWorkPCentWed);
                                Columns.Add(ss248Work31.WedLY).BindValue(() => 0);
                                Columns.Add(ss248Work31.ThuAct).BindValue(_parent._parent.vWorkPCentThu);
                                Columns.Add(ss248Work31.ThuLY).BindValue(() => 0);
                                Columns.Add(ss248Work31.FriAct).BindValue(_parent._parent.vWorkPCentFri);
                                Columns.Add(ss248Work31.FriLY).BindValue(() => 0);
                                Columns.Add(ss248Work31.WeekAct).BindValue(_parent._parent.vWorkPCentWeek);
                                Columns.Add(ss248Work31.WeekLY).BindValue(() => 0);
                                #endregion
                            }
                            /// <summary>Write Conc Work3(P#46.1.7.1.9.4)</summary>
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
                                _parent.vConcessionsWritten.Value++;
                            }
                            
                            
                        }
                        /// <summary>Write Conc Desc(P#46.1.7.1.9.5)</summary>
                        /// <remark>Last change before Migration: 16/05/2006 15:08:25</remark>
                        class WriteConcDesc : SalesAndStock.BusinessProcessBase 
                        {
                            
                            #region Models
                            /// <summary>ss248 Work3</summary>
                            readonly Models.ss248Work3 ss248Work3 = new Models.ss248Work3 { KeepCacheAliveAfterExit = true, AllowRowLocking = true };
                            /// <summary>ss248 Work3</summary>
                            readonly Models.ss248Work3 ss248Work31 = new Models.ss248Work3 { KeepCacheAliveAfterExit = true, AllowRowLocking = true };
                            #endregion
                            
                            #region Columns
                            /// <summary>v:Write1 ok</summary>
                            readonly BoolColumn vWrite1Ok = new BoolColumn("v:Write1 ok");
                            /// <summary>v:Write2 ok</summary>
                            readonly BoolColumn vWrite2Ok = new BoolColumn("v:Write2 ok");
                            /// <summary>e:TableName</summary>
                            readonly TextColumn eTableName = new TextColumn("e:TableName", "30");
                            /// <summary>e:DbmsCode</summary>
                            readonly NumberColumn eDbmsCode = new NumberColumn("e:DbmsCode", "8");
                            /// <summary>e:DbmsMsg</summary>
                            readonly TextColumn eDbmsMsg = new TextColumn("e:DbmsMsg", "200");
                            #endregion
                            
                            ReadWork1Concessions _parent;
                            
                            public WriteConcDesc(ReadWork1Concessions parent)
                            {
                                _parent = parent;
                                Title = "Write Conc Desc";
                                InitializeDataView();
                                #region Event Handlers
                                
                                var h = Handlers.AddDatabaseErrorHandler(DatabaseErrorType.ConstraintFailed);
                                h.Invokes += ConstraintFailedHandler;
                                
                                #endregion
                            }
                            void InitializeDataView()
                            {
                                
                                Relations.Add(ss248Work3, RelationType.InsertIfNotFound, 
                                		ss248Work3.ReportGroup.BindEqualTo("E").And(
                                		ss248Work3.ReportDepartment.BindEqualTo("00")).And(
                                		ss248Work3.ReportSubDept.BindEqualTo("00")).And(
                                		ss248Work3.PercentIndicator.BindEqualTo("B")), 
                                	ss248Work3.SortByss_ss248_Work3_X1);
                                
                                Relations.Add(ss248Work31, RelationType.InsertIfNotFound, 
                                		ss248Work31.ReportGroup.BindEqualTo("E").And(
                                		ss248Work31.ReportDepartment.BindEqualTo("00")).And(
                                		ss248Work31.ReportSubDept.BindEqualTo("ZZ")).And(
                                		ss248Work31.PercentIndicator.BindEqualTo("B")), 
                                	ss248Work31.SortByss_ss248_Work3_X1);
                                
                                
                                
                                #region Columns
                                
                                // Write blank print line to Work3
                                Columns.Add(vWrite1Ok);
                                Relations[ss248Work3].NotifyRowWasFoundTo(vWrite1Ok);
                                Columns.Add(ss248Work3.ReportGroup);
                                Columns.Add(ss248Work3.ReportDepartment);
                                Columns.Add(ss248Work3.ReportSubDept);
                                Columns.Add(ss248Work3.PercentIndicator);
                                Columns.Add(ss248Work3.DepartmentDesc).BindValue(() => " ");
                                Columns.Add(ss248Work3.SatAct).BindValue(() => 0);
                                Columns.Add(ss248Work3.SatLY).BindValue(() => 0);
                                Columns.Add(ss248Work3.SunAct).BindValue(() => 0);
                                Columns.Add(ss248Work3.SunLY).BindValue(() => 0);
                                Columns.Add(ss248Work3.MonAct).BindValue(() => 0);
                                Columns.Add(ss248Work3.MonLY).BindValue(() => 0);
                                Columns.Add(ss248Work3.TueAct).BindValue(() => 0);
                                Columns.Add(ss248Work3.TueLY).BindValue(() => 0);
                                Columns.Add(ss248Work3.WedAct).BindValue(() => 0);
                                Columns.Add(ss248Work3.WedLY).BindValue(() => 0);
                                Columns.Add(ss248Work3.ThuAct).BindValue(() => 0);
                                Columns.Add(ss248Work3.ThuLY).BindValue(() => 0);
                                Columns.Add(ss248Work3.FriAct).BindValue(() => 0);
                                Columns.Add(ss248Work3.FriLY).BindValue(() => 0);
                                Columns.Add(ss248Work3.WeekAct).BindValue(() => 0);
                                Columns.Add(ss248Work3.WeekLY).BindValue(() => 0);
                                
                                // Write 'Concessions' description to Work3
                                Columns.Add(vWrite2Ok);
                                Relations[ss248Work31].NotifyRowWasFoundTo(vWrite2Ok);
                                Columns.Add(ss248Work31.ReportGroup);
                                Columns.Add(ss248Work31.ReportDepartment);
                                Columns.Add(ss248Work31.ReportSubDept);
                                Columns.Add(ss248Work31.PercentIndicator);
                                Columns.Add(ss248Work31.DepartmentDesc).BindValue(() => "Concessions");
                                Columns.Add(ss248Work31.SatAct).BindValue(() => 0);
                                Columns.Add(ss248Work31.SatLY).BindValue(() => 0);
                                Columns.Add(ss248Work31.SunAct).BindValue(() => 0);
                                Columns.Add(ss248Work31.SunLY).BindValue(() => 0);
                                Columns.Add(ss248Work31.MonAct).BindValue(() => 0);
                                Columns.Add(ss248Work31.MonLY).BindValue(() => 0);
                                Columns.Add(ss248Work31.TueAct).BindValue(() => 0);
                                Columns.Add(ss248Work31.TueLY).BindValue(() => 0);
                                Columns.Add(ss248Work31.WedAct).BindValue(() => 0);
                                Columns.Add(ss248Work31.WedLY).BindValue(() => 0);
                                Columns.Add(ss248Work31.ThuAct).BindValue(() => 0);
                                Columns.Add(ss248Work31.ThuLY).BindValue(() => 0);
                                Columns.Add(ss248Work31.FriAct).BindValue(() => 0);
                                Columns.Add(ss248Work31.FriLY).BindValue(() => 0);
                                Columns.Add(ss248Work31.WeekAct).BindValue(() => 0);
                                Columns.Add(ss248Work31.WeekLY).BindValue(() => 0);
                                #endregion
                            }
                            /// <summary>Write Conc Desc(P#46.1.7.1.9.5)</summary>
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
                            
                            #region Handlers
                            void ConstraintFailedHandler(DatabaseErrorEventArgs e)
                            {
                                Common.ShowDatabaseErrorMessage = true;
                                
                                Message.ShowWarningInStatusBar("Processing branch:- " + u.Str(_parent._parent._parent.DiaryWork1.BranchNumber, "4P0"));
                                Message.ShowWarningInStatusBar("Table:- " + u.Trim(eTableName) + "  Error:- " + u.Trim(u.Str(eDbmsCode, "8#")));
                                Message.ShowWarningInStatusBar(u.Trim(u.Left(eDbmsMsg, 50)));
                                Message.ShowWarningInStatusBar(u.Trim(u.Mid(eDbmsMsg, 51, 50)));
                                e.Handled = true;
                            }
                            #endregion
                            
                            
                        }
                    }
                }
                /// <summary>Clear Work3(P#46.1.7.2)</summary>
                /// <remark>Last change before Migration: 31/03/2004 10:09:02</remark>
                class ClearWork3 : SalesAndStock.BusinessProcessBase 
                {
                    
                    public ClearWork3()
                    {
                        Title = "Clear Work3";
                        InitializeDataView();
                    }
                    void InitializeDataView()
                    {
                        
                        
                        #region Columns
                        
                        // Clear Work3 table for each branch encountered
                        #endregion
                    }
                    /// <summary>Clear Work3(P#46.1.7.2)</summary>
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
                        u.DBDel(typeof(Models.ss248Work3), "");
                    }
                    
                    
                }
                /// <summary>Print Branch(P#46.1.7.3)</summary>
                /// <remark>Last change before Migration: 27/05/2011 11:45:41</remark>
                class PrintBranch : SalesAndStock.BusinessProcessBase 
                {
                    
                    #region Models
                    /// <summary>ss248 Work3</summary>
                    readonly Models.ss248Work3 ss248Work3 = new Models.ss248Work3 { AllowRowLocking = true };
                    #endregion
                    
                    #region Columns
                    /// <summary>v:Prt Sat Act</summary>
                    readonly TextColumn vPrtSatAct = new TextColumn("v:Prt Sat Act", "7");
                    /// <summary>v:Prt Sat LY</summary>
                    readonly TextColumn vPrtSatLY = new TextColumn("v:Prt Sat LY", "7");
                    /// <summary>v:Prt Sun Act</summary>
                    readonly TextColumn vPrtSunAct = new TextColumn("v:Prt Sun Act", "7");
                    /// <summary>v:Prt Sun LY</summary>
                    readonly TextColumn vPrtSunLY = new TextColumn("v:Prt Sun LY", "7");
                    /// <summary>v:Prt Mon Act</summary>
                    readonly TextColumn vPrtMonAct = new TextColumn("v:Prt Mon Act", "7");
                    /// <summary>v:Prt Mon LY</summary>
                    readonly TextColumn vPrtMonLY = new TextColumn("v:Prt Mon LY", "7");
                    /// <summary>v:Prt Tue Act</summary>
                    readonly TextColumn vPrtTueAct = new TextColumn("v:Prt Tue Act", "7");
                    /// <summary>v:Prt Tue LY</summary>
                    readonly TextColumn vPrtTueLY = new TextColumn("v:Prt Tue LY", "7");
                    /// <summary>v:Prt Wed Act</summary>
                    readonly TextColumn vPrtWedAct = new TextColumn("v:Prt Wed Act", "7");
                    /// <summary>v:Prt Wed LY</summary>
                    readonly TextColumn vPrtWedLY = new TextColumn("v:Prt Wed LY", "7");
                    /// <summary>v:Prt Thu Act</summary>
                    readonly TextColumn vPrtThuAct = new TextColumn("v:Prt Thu Act", "7");
                    /// <summary>v:Prt Thu LY</summary>
                    readonly TextColumn vPrtThuLY = new TextColumn("v:Prt Thu LY", "7");
                    /// <summary>v:Prt Fri Act</summary>
                    readonly TextColumn vPrtFriAct = new TextColumn("v:Prt Fri Act", "7");
                    /// <summary>v:Prt Fri LY</summary>
                    readonly TextColumn vPrtFriLY = new TextColumn("v:Prt Fri LY", "7");
                    /// <summary>v:Prt Week Act</summary>
                    readonly TextColumn vPrtWeekAct = new TextColumn("v:Prt Week Act", "7");
                    /// <summary>v:Prt Week LY</summary>
                    readonly TextColumn vPrtWeekLY = new TextColumn("v:Prt Week LY", "7");
                    /// <summary>v:Dummy Prt Col</summary>
                    readonly TextColumn vDummyPrtCol = new TextColumn("v:Dummy Prt Col", "2");
                    #endregion
                    
                    #region Streams
                    /// <summary>Report By Branch</summary>
                    FileWriter _ioReportByBranch;
                    #endregion
                    
                    #region Layouts
                    /// <summary>Report ss248</summary>
                    TextTemplate _viewReportSs248;
                    /// <summary>ss248 Detail</summary>
                    TextTemplate _viewSs248Detail;
                    #endregion
                    
                    ProcessBranches _parent;
                    
                    public PrintBranch(ProcessBranches parent)
                    {
                        _parent = parent;
                        Title = "Print Branch";
                        InitializeDataView();
                    }
                    void InitializeDataView()
                    {
                        From = ss248Work3;
                        
                        OrderBy = ss248Work3.SortByss_ss248_Work3_X1;
                        
                        #region Columns
                        
                        Columns.Add(ss248Work3.ReportGroup);
                        Columns.Add(ss248Work3.ReportDepartment);
                        Columns.Add(ss248Work3.ReportSubDept);
                        Columns.Add(ss248Work3.PercentIndicator);
                        Columns.Add(ss248Work3.DepartmentDesc);
                        Columns.Add(ss248Work3.SatAct);
                        Columns.Add(ss248Work3.SatLY);
                        Columns.Add(ss248Work3.SunAct);
                        Columns.Add(ss248Work3.SunLY);
                        Columns.Add(ss248Work3.MonAct);
                        Columns.Add(ss248Work3.MonLY);
                        Columns.Add(ss248Work3.TueAct);
                        Columns.Add(ss248Work3.TueLY);
                        Columns.Add(ss248Work3.WedAct);
                        Columns.Add(ss248Work3.WedLY);
                        Columns.Add(ss248Work3.ThuAct);
                        Columns.Add(ss248Work3.ThuLY);
                        Columns.Add(ss248Work3.FriAct);
                        Columns.Add(ss248Work3.FriLY);
                        Columns.Add(ss248Work3.WeekAct);
                        Columns.Add(ss248Work3.WeekLY);
                        // Values/percentages will be formatted into the following fields
                        Columns.Add(vPrtSatAct);
                        Columns.Add(vPrtSatLY);
                        Columns.Add(vPrtSunAct);
                        Columns.Add(vPrtSunLY);
                        Columns.Add(vPrtMonAct);
                        Columns.Add(vPrtMonLY);
                        Columns.Add(vPrtTueAct);
                        Columns.Add(vPrtTueLY);
                        Columns.Add(vPrtWedAct);
                        Columns.Add(vPrtWedLY);
                        Columns.Add(vPrtThuAct);
                        Columns.Add(vPrtThuLY);
                        Columns.Add(vPrtFriAct);
                        Columns.Add(vPrtFriLY);
                        Columns.Add(vPrtWeekAct);
                        Columns.Add(vPrtWeekLY);
                        
                        Columns.Add(vDummyPrtCol).BindValue(() => "  ");
                        #endregion
                    }
                    /// <summary>Print Branch(P#46.1.7.3)</summary>
                    internal void Run()
                    {
                        Execute();
                    }
                    protected override void OnLoad()
                    {
                        KeepChildRelationCacheAlive = true;
                        AllowUserAbort = true;
                        
                        _ioReportByBranch = new FileWriter(_parent._parent.vTargetFile)
                        			{
                        				Name = "Report By Branch"
                        			};
                        _ioReportByBranch.Open();
                        Streams.Add(_ioReportByBranch);
                        
                        _viewReportSs248 = new TextTemplate("%MagicApps%ss/html/ss248.htm");
                        _viewReportSs248.Add(
                        			new Tag("IS_HEADER", true, "5"), 
                        			new Tag("v:Header_Week", _parent._parent._parent.vHdrWeek, "20"), 
                        			new Tag("v:Header_Branch", _parent._parent._parent.vHdrBranch), 
                        			new Tag("v:Header_Date", _parent._parent._parent.vHdrDate));
                        
                        _viewSs248Detail = new TextTemplate("%MagicApps%ss/html/ss248.htm");
                        _viewSs248Detail.Add(
                        			new Tag("IS_DETAIL", true, "5"), 
                        			new Tag("v:Department", () => u.If(ss248Work3.DepartmentDesc == "", "&nbsp;", u.If(ss248Work3.ReportDepartment == "99" || ss248Work3.ReportSubDept == "ZZ", "<b><i>" + ss248Work3.DepartmentDesc + "</b></i>", ss248Work3.DepartmentDesc)), "30"), 
                        			new Tag("v:Pound_Or_Pcent", () => u.If(ss248Work3.ReportGroup == "D" && ss248Work3.ReportSubDept == "00", "Open", u.If(ss248Work3.PercentIndicator == "Y", "%", u.If(ss248Work3.PercentIndicator == "N", "£'s", "&nbsp;"))), "5"), 
                        			new Tag("v:Sat_Actual", () => u.If(vPrtSatAct == "", "#000000>&nbsp;", u.If(ss248Work3.PercentIndicator == "Y" && ss248Work3.SatAct < 0, "#FF00FF>" + vPrtSatAct, "#000000>" + vPrtSatAct)), "15"), 
                        			new Tag("v:Sat_LY", () => u.If(vPrtSatLY == "", "#000000>&nbsp;", u.If(ss248Work3.PercentIndicator == "Y" && ss248Work3.SatLY < 0, "#FF00FF>" + vPrtSatLY, "#000000>" + vPrtSatLY)), "15"), 
                        			new Tag("v:Sun_Actual", () => u.If(vPrtSunAct == "", "#000000>&nbsp;", u.If(ss248Work3.PercentIndicator == "Y" && ss248Work3.SunAct < 0, "#FF00FF>" + vPrtSunAct, "#000000>" + vPrtSunAct)), "15"), 
                        			new Tag("v:Sun_LY", () => u.If(vPrtSunLY == "", "#000000>&nbsp;", u.If(ss248Work3.PercentIndicator == "Y" && ss248Work3.SunLY < 0, "#FF00FF>" + vPrtSunLY, "#000000>" + vPrtSunLY)), "15"), 
                        			new Tag("v:Mon_Actual", () => u.If(vPrtMonAct == "", "#000000>&nbsp;", u.If(ss248Work3.PercentIndicator == "Y" && ss248Work3.MonAct < 0, "#FF00FF>" + vPrtMonAct, "#000000>" + vPrtMonAct)), "15"), 
                        			new Tag("v:Mon_LY", () => u.If(vPrtMonLY == "", "#000000>&nbsp;", u.If(ss248Work3.PercentIndicator == "Y" && ss248Work3.MonLY < 0, "#FF00FF>" + vPrtMonLY, "#000000>" + vPrtMonLY)), "15"), 
                        			new Tag("v:Tue_Actual", () => u.If(vPrtTueAct == "", "#000000>&nbsp;", u.If(ss248Work3.PercentIndicator == "Y" && ss248Work3.TueAct < 0, "#FF00FF>" + vPrtTueAct, "#000000>" + vPrtTueAct)), "15"), 
                        			new Tag("v:Tue_LY", () => u.If(vPrtTueLY == "", "#000000>&nbsp;", u.If(ss248Work3.PercentIndicator == "Y" && ss248Work3.TueLY < 0, "#FF00FF>" + vPrtTueLY, "#000000>" + vPrtTueLY)), "15"), 
                        			new Tag("v:Wed_Actual", () => u.If(vPrtWedAct == "", "#000000>&nbsp;", u.If(ss248Work3.PercentIndicator == "Y" && ss248Work3.WedAct < 0, "#FF00FF>" + vPrtWedAct, "#000000>" + vPrtWedAct)), "15"), 
                        			new Tag("v:Wed_LY", () => u.If(vPrtWedLY == "", "#000000>&nbsp;", u.If(ss248Work3.PercentIndicator == "Y" && ss248Work3.WedLY < 0, "#FF00FF>" + vPrtWedLY, "#000000>" + vPrtWedLY)), "15"), 
                        			new Tag("v:Thu_Actual", () => u.If(vPrtThuAct == "", "#000000>&nbsp;", u.If(ss248Work3.PercentIndicator == "Y" && ss248Work3.ThuAct < 0, "#FF00FF>" + vPrtThuAct, "#000000>" + vPrtThuAct)), "15"), 
                        			new Tag("v:Thu_LY", () => u.If(vPrtThuLY == "", "#000000>&nbsp;", u.If(ss248Work3.PercentIndicator == "Y" && ss248Work3.ThuLY < 0, "#FF00FF>" + vPrtThuLY, "#000000>" + vPrtThuLY)), "15"), 
                        			new Tag("v:Fri_Actual", () => u.If(vPrtFriAct == "", "#000000>&nbsp;", u.If(ss248Work3.PercentIndicator == "Y" && ss248Work3.FriAct < 0, "#FF00FF>" + vPrtFriAct, "#000000>" + vPrtFriAct)), "15"), 
                        			new Tag("v:Fri_LY", () => u.If(vPrtFriLY == "", "#000000>&nbsp;", u.If(ss248Work3.PercentIndicator == "Y" && ss248Work3.FriLY < 0, "#FF00FF>" + vPrtFriLY, "#000000>" + vPrtFriLY)), "15"), 
                        			new Tag("v:Week_Actual", () => u.If(vPrtWeekAct == "", "#000000>&nbsp;", u.If(ss248Work3.PercentIndicator == "Y" && ss248Work3.WeekAct < 0, "#FF00FF>" + vPrtWeekAct, "#000000>" + vPrtWeekAct)), "15"), 
                        			new Tag("v:Week_LY", () => u.If(vPrtWeekLY == "", "#000000>&nbsp;", u.If(ss248Work3.PercentIndicator == "Y" && ss248Work3.WeekLY < 0, "#FF00FF>" + vPrtWeekLY, "#000000>" + vPrtWeekLY)), "15"));
                    }
                    protected override void OnStart()
                    {
                        _viewReportSs248.WriteTo(_ioReportByBranch);
                    }
                    protected override void OnLeaveRow()
                    {
                        // Format VALUES
                        if(ss248Work3.PercentIndicator != "Y")
                        {
                            Cached<FormatValues>().Run();
                        }
                        // Format PERCENTAGES
                        if(ss248Work3.PercentIndicator == "Y")
                        {
                            Cached<FormatPCents>().Run();
                        }
                        // Write printline
                        if(ss248Work3.PercentIndicator != "Y" || ss248Work3.PercentIndicator == "Y" && ss248Work3.ReportGroup != "D")
                        {
                            _viewSs248Detail.WriteTo(_ioReportByBranch);
                        }
                        
                        // DEBUGGING
                        if(false)
                        {
                            Message.ShowWarningInStatusBar(ss248Work3.ReportGroup + " " + ss248Work3.ReportDepartment + " " + ss248Work3.ReportSubDept + " " + ss248Work3.PercentIndicator + " " + ss248Work3.DepartmentDesc + " " + vPrtSatAct + " " + vPrtSunAct + " " + vPrtMonAct + " " + vPrtTueAct + " " + vPrtWedAct + " " + vPrtThuAct + " " + vPrtFriAct);
                            Message.ShowWarningInStatusBar(ss248Work3.ReportGroup + " " + ss248Work3.ReportDepartment + " " + ss248Work3.ReportSubDept + " " + ss248Work3.PercentIndicator + " " + ss248Work3.DepartmentDesc + " " + vPrtSatLY + " " + vPrtSunLY + " " + vPrtMonLY + " " + vPrtTueLY + " " + vPrtWedLY + " " + vPrtThuLY + " " + vPrtFriLY);
                            Message.ShowWarningInStatusBar("");
                        }
                    }
                    
                    
                    /// <summary>Format Values(P#46.1.7.3.1)</summary>
                    /// <remark>Last change before Migration: 31/03/2004 10:09:22</remark>
                    class FormatValues : SalesAndStock.BusinessProcessBase 
                    {
                        
                        #region Columns
                        /// <summary>v:Prt Detail Position</summary>
                        readonly NumberColumn vPrtDetailPosition = new NumberColumn("v:Prt Detail Position", "2");
                        /// <summary>v:Work</summary>
                        readonly NumberColumn vWork = new NumberColumn("v:Work", "N6");
                        #endregion
                        
                        PrintBranch _parent;
                        
                        public FormatValues(PrintBranch parent)
                        {
                            _parent = parent;
                            Title = "Format Values";
                            InitializeDataView();
                        }
                        void InitializeDataView()
                        {
                            
                            
                            #region Columns
                            
                            Columns.Add(vPrtDetailPosition);
                            Columns.Add(vWork);
                            #endregion
                        }
                        /// <summary>Format Values(P#46.1.7.3.1)</summary>
                        internal void Run()
                        {
                            Execute();
                        }
                        protected override void OnLoad()
                        {
                            Exit(ExitTiming.AfterRow, () => Counter == 16);
                            KeepChildRelationCacheAlive = true;
                            AllowUserAbort = true;
                        }
                        protected override void OnLeaveRow()
                        {
                            vWork.Value = u.CastToNumber(u.VarCurr(u.IndexOf(_parent.ss248Work3.SatAct) + vPrtDetailPosition));
                            u.VarSet(u.IndexOf(_parent.vPrtSatAct) + vPrtDetailPosition, u.Str(vWork, "N6Z"));
                            vPrtDetailPosition.Value++;
                        }
                        
                        
                    }
                    /// <summary>Format PCents(P#46.1.7.3.2)</summary>
                    /// <remark>Last change before Migration: 27/05/2011 11:45:41</remark>
                    class FormatPCents : SalesAndStock.BusinessProcessBase 
                    {
                        
                        #region Columns
                        /// <summary>v:Prt Detail Position</summary>
                        readonly NumberColumn vPrtDetailPosition = new NumberColumn("v:Prt Detail Position", "2");
                        /// <summary>v:Work</summary>
                        readonly NumberColumn vWork = new NumberColumn("v:Work", "N6.2");
                        #endregion
                        
                        PrintBranch _parent;
                        
                        public FormatPCents(PrintBranch parent)
                        {
                            _parent = parent;
                            Title = "Format PCents";
                            InitializeDataView();
                        }
                        void InitializeDataView()
                        {
                            
                            
                            #region Columns
                            
                            Columns.Add(vPrtDetailPosition);
                            Columns.Add(vWork);
                            #endregion
                        }
                        /// <summary>Format PCents(P#46.1.7.3.2)</summary>
                        internal void Run()
                        {
                            Execute();
                        }
                        protected override void OnLoad()
                        {
                            Exit(ExitTiming.AfterRow, () => Counter == 16);
                            KeepChildRelationCacheAlive = true;
                            AllowUserAbort = true;
                        }
                        protected override void OnLeaveRow()
                        {
                            vWork.Value = u.CastToNumber(u.VarCurr(u.IndexOf(_parent.ss248Work3.SatAct) + vPrtDetailPosition));
                            u.VarSet(u.IndexOf(_parent.vPrtSatAct) + vPrtDetailPosition, u.If(_parent.ss248Work3.ReportGroup == "C", u.Str(vWork, "N4.1Z"), u.Str(vWork, "N5Z")) + u.If(vWork == 0, " ", "%"));
                            vPrtDetailPosition.Value++;
                        }
                        
                        
                    }
                }
            }
            /// <summary>Delete Diary Work1(P#46.1.8)</summary>
            /// <remark>Last change before Migration: 11/05/2006 09:49:39</remark>
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
@"delete ss_diary_work1 d
 where d.prog_no = 'ss248'");
                    From = sqlEntity;
                    
                    
                }
                /// <summary>Delete Diary Work1(P#46.1.8)</summary>
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
            /// <summary>Fetch TY Inst Cred(P#46.1.9)</summary>
            /// <remark>Last change before Migration: 20/05/2005 11:13:49</remark>
            class FetchTYInstCred : SalesAndStock.BusinessProcessBase 
            {
                
                #region Models
                DynamicSQLEntity sqlEntity;
                /// <summary>ss248 Work1</summary>
                readonly Models.ss248Work1 ss248Work1 = new Models.ss248Work1 { KeepCacheAliveAfterExit = true, AllowRowLocking = true };
                #endregion
                
                #region Columns
                /// <summary>sql:BRANCH_NUMBER</summary>
                readonly NumberColumn sqlBRANCH_NUMBER = new NumberColumn("sql:BRANCH_NUMBER", "N4")
                {
                	AllowNull = true
                };
                /// <summary>sql:TILL_DATE</summary>
                readonly DateColumn sqlTILL_DATE = new DateColumn("sql:TILL_DATE")
                {
                	AllowNull = true,
                	Storage = new ENV.Data.Storage.DateTimeDateStorage()
                };
                /// <summary>sql:ICR_OPEN</summary>
                readonly NumberColumn sqlICR_OPEN = new NumberColumn("sql:ICR_OPEN", "9")
                {
                	AllowNull = true
                };
                /// <summary>v:Skip</summary>
                readonly NumberColumn vSkip = new NumberColumn("v:Skip", "1");
                #endregion
                
                Control _parent;
                
                public FetchTYInstCred(Control parent)
                {
                    _parent = parent;
                    Title = "Fetch TY Inst Cred";
                    InitializeDataView();
                }
                void InitializeDataView()
                {
                    sqlEntity = new DynamicSQLEntity(Models.DataSources.SS, 
@"select c.branch_number, c.transaction_date, count(*)
from mackays.ccc_detail c
where c.application_id between ':1' and ':2'
  and (c.credit_card_number like '50016313%'  OR c.credit_card_number like '50016314%')
  and c.instant_credit = '1'
  and c.transaction_value > 0
  and c.transaction_date between ':3' and ':4'
  and exists (select 'X' from mackays.ss_ss248_work1 w
              where w.financial_week = :5
                and w.branch_number = c.branch_number)
group by c.branch_number, c.transaction_date
order by c.branch_number, c.transaction_date");
                    sqlEntity.AddParameter(_parent.vFromApplicID); //:1;
                    sqlEntity.AddParameter(_parent.vToApplicID); //:2;
                    sqlEntity.AddParameter(() => u.DStr(_parent._parent.vWeekStartTY, "DD-MMM-YYYY")); //:3;
                    sqlEntity.AddParameter(() => u.DStr(_parent._parent.vUpToDateTY, "DD-MMM-YYYY")); //:4;
                    sqlEntity.AddParameter(_parent._parent.vReqYYYYWW); //:5;
                    sqlEntity.Columns.Add(sqlBRANCH_NUMBER, sqlTILL_DATE, sqlICR_OPEN);
                    From = sqlEntity;
                    
                    Relations.Add(ss248Work1, RelationType.InsertIfNotFound, 
                    		ss248Work1.FinancialWeek.BindEqualTo(_parent._parent.vReqYYYYWW).And(
                    		ss248Work1.BranchNumber.BindEqualTo(sqlBRANCH_NUMBER)).And(
                    		ss248Work1.ReportGroup.BindEqualTo("D")).And(
                    		ss248Work1.DeptReportSequence.BindEqualTo(99)).And(
                    		ss248Work1.Department1.BindEqualTo("99")).And(
                    		ss248Work1.SubDepartment1.BindEqualTo("99")), 
                    	ss248Work1.SortByss_ss248_Work1_X1);
                    
                    
                    
                    #region Columns
                    
                    // Data returned by SQL
                    Columns.Add(sqlBRANCH_NUMBER);
                    Columns.Add(sqlTILL_DATE);
                    Columns.Add(sqlICR_OPEN);
                    // Create/update Instant Credit record in work table
                    Columns.Add(ss248Work1.FinancialWeek);
                    Columns.Add(ss248Work1.BranchNumber);
                    Columns.Add(ss248Work1.ReportGroup);
                    Columns.Add(ss248Work1.DeptReportSequence);
                    Columns.Add(ss248Work1.Department1);
                    Columns.Add(ss248Work1.SubDepartment1);
                    Columns.Add(ss248Work1.TYSaturday);
                    Columns.Add(ss248Work1.TYSunday);
                    Columns.Add(ss248Work1.TYMonday);
                    Columns.Add(ss248Work1.TYTuesday);
                    Columns.Add(ss248Work1.TYWednesday);
                    Columns.Add(ss248Work1.TYThursday);
                    Columns.Add(ss248Work1.TYFriday);
                    Columns.Add(ss248Work1.TYWeekToDate);
                    Columns.Add(ss248Work1.LYSaturday);
                    Columns.Add(ss248Work1.LYSunday);
                    Columns.Add(ss248Work1.LYMonday);
                    Columns.Add(ss248Work1.LYTuesday);
                    Columns.Add(ss248Work1.LYWednesday);
                    Columns.Add(ss248Work1.LYThursday);
                    Columns.Add(ss248Work1.LYFriday);
                    Columns.Add(ss248Work1.LYWeekToDate);
                    
                    Columns.Add(vSkip);
                    #endregion
                }
                /// <summary>Fetch TY Inst Cred(P#46.1.9)</summary>
                internal void Run()
                {
                    Execute();
                }
                protected override void OnLoad()
                {
                    RowLocking = LockingStrategy.OnRowLoading;
                    TransactionScope = TransactionScopes.Task;
                    KeepChildRelationCacheAlive = true;
                    AllowDelete = false;
                    AllowUserAbort = true;
                }
                protected override void OnLeaveRow()
                {
                    // Init SKIP from day_of_week to allow VARSET logic
                    vSkip.Value = u.If(u.Equals(u.DOW(sqlTILL_DATE), 7), 0, u.DOW(sqlTILL_DATE));
                    u.VarSet(u.IndexOf(ss248Work1.TYSaturday) + vSkip, u.CastToNumber(u.VarCurr(u.IndexOf(ss248Work1.TYSaturday) + vSkip)) + sqlICR_OPEN);
                    ss248Work1.TYWeekToDate.Value += sqlICR_OPEN;
                }
                
                
            }
            /// <summary>Fetch PY Inst Cred(P#46.1.10)</summary>
            /// <remark>Last change before Migration: 20/05/2005 11:14:32</remark>
            internal class FetchPYInstCred : SalesAndStock.BusinessProcessBase 
            {
                
                #region Models
                DynamicSQLEntity sqlEntity;
                /// <summary>ss248 Work1</summary>
                readonly Models.ss248Work1 ss248Work1 = new Models.ss248Work1 { KeepCacheAliveAfterExit = true, AllowRowLocking = true };
                #endregion
                
                #region Columns
                /// <summary>sql:BRANCH_NUMBER</summary>
                readonly NumberColumn sqlBRANCH_NUMBER = new NumberColumn("sql:BRANCH_NUMBER", "N4")
                {
                	AllowNull = true
                };
                /// <summary>sql:TILL_DATE</summary>
                readonly DateColumn sqlTILL_DATE = new DateColumn("sql:TILL_DATE")
                {
                	AllowNull = true,
                	Storage = new ENV.Data.Storage.DateTimeDateStorage()
                };
                /// <summary>sql:ICR_OPEN</summary>
                readonly NumberColumn sqlICR_OPEN = new NumberColumn("sql:ICR_OPEN", "9")
                {
                	AllowNull = true
                };
                /// <summary>v:Skip</summary>
                readonly NumberColumn vSkip = new NumberColumn("v:Skip", "1");
                #endregion
                
                Control _parent;
                
                public FetchPYInstCred(Control parent)
                {
                    _parent = parent;
                    Title = "Fetch PY Inst Cred";
                    InitializeDataView();
                }
                void InitializeDataView()
                {
                    sqlEntity = new DynamicSQLEntity(Models.DataSources.SS, 
@"select c.branch_number, c.transaction_date, count(*)
from mackays.ccc_detail c
where c.application_id between ':1' and ':2'
  and (c.credit_card_number like '50016313%'  OR c.credit_card_number like '50016314%')
  and c.instant_credit = '1'
  and c.transaction_value > 0
  and c.transaction_date between ':3' and ':4'
  and exists (select 'X' from mackays.ss_ss248_work1 w
              where w.financial_week = :5
                and w.branch_number = c.branch_number)
group by c.branch_number, c.transaction_date
order by c.branch_number, c.transaction_date");
                    sqlEntity.AddParameter(_parent.vFromApplicID); //:1;
                    sqlEntity.AddParameter(_parent.vToApplicID); //:2;
                    sqlEntity.AddParameter(() => u.DStr(_parent._parent.vWeekStartPY, "DD-MMM-YYYY")); //:3;
                    sqlEntity.AddParameter(() => u.DStr(_parent._parent.vWeekEndPY, "DD-MMM-YYYY")); //:4;
                    sqlEntity.AddParameter(_parent._parent.vReqYYYYWW); //:5;
                    sqlEntity.Columns.Add(sqlBRANCH_NUMBER, sqlTILL_DATE, sqlICR_OPEN);
                    From = sqlEntity;
                    
                    Relations.Add(ss248Work1, RelationType.InsertIfNotFound, 
                    		ss248Work1.FinancialWeek.BindEqualTo(_parent._parent.vReqYYYYWW).And(
                    		ss248Work1.BranchNumber.BindEqualTo(sqlBRANCH_NUMBER)).And(
                    		ss248Work1.ReportGroup.BindEqualTo("D")).And(
                    		ss248Work1.DeptReportSequence.BindEqualTo(99)).And(
                    		ss248Work1.Department1.BindEqualTo("99")).And(
                    		ss248Work1.SubDepartment1.BindEqualTo("99")), 
                    	ss248Work1.SortByss_ss248_Work1_X1);
                    
                    
                    
                    #region Columns
                    
                    // Data returned by SQL
                    Columns.Add(sqlBRANCH_NUMBER);
                    Columns.Add(sqlTILL_DATE);
                    Columns.Add(sqlICR_OPEN);
                    // Create/Update LY Instant Credit to work table
                    Columns.Add(ss248Work1.FinancialWeek);
                    Columns.Add(ss248Work1.BranchNumber);
                    Columns.Add(ss248Work1.ReportGroup);
                    Columns.Add(ss248Work1.DeptReportSequence);
                    Columns.Add(ss248Work1.Department1);
                    Columns.Add(ss248Work1.SubDepartment1);
                    Columns.Add(ss248Work1.TYSaturday);
                    Columns.Add(ss248Work1.TYSunday);
                    Columns.Add(ss248Work1.TYMonday);
                    Columns.Add(ss248Work1.TYTuesday);
                    Columns.Add(ss248Work1.TYWednesday);
                    Columns.Add(ss248Work1.TYThursday);
                    Columns.Add(ss248Work1.TYFriday);
                    Columns.Add(ss248Work1.TYWeekToDate);
                    Columns.Add(ss248Work1.LYSaturday);
                    Columns.Add(ss248Work1.LYSunday);
                    Columns.Add(ss248Work1.LYMonday);
                    Columns.Add(ss248Work1.LYTuesday);
                    Columns.Add(ss248Work1.LYWednesday);
                    Columns.Add(ss248Work1.LYThursday);
                    Columns.Add(ss248Work1.LYFriday);
                    Columns.Add(ss248Work1.LYWeekToDate);
                    
                    Columns.Add(vSkip);
                    #endregion
                }
                /// <summary>Fetch PY Inst Cred(P#46.1.10)</summary>
                internal void Run()
                {
                    Execute();
                }
                protected override void OnLoad()
                {
                    RowLocking = LockingStrategy.OnRowLoading;
                    TransactionScope = TransactionScopes.Task;
                    KeepChildRelationCacheAlive = true;
                    AllowDelete = false;
                    AllowUserAbort = true;
                }
                protected override void OnLeaveRow()
                {
                    // Init SKIP from day_of_week to allow VARSET logic
                    vSkip.Value = u.If(u.Equals(u.DOW(sqlTILL_DATE), 7), 0, u.DOW(sqlTILL_DATE));
                    u.VarSet(u.IndexOf(ss248Work1.LYSaturday) + vSkip, u.CastToNumber(u.VarCurr(u.IndexOf(ss248Work1.LYSaturday) + vSkip)) + sqlICR_OPEN);
                    // Accumulate LY Week-to-date figure only for days equivalent to those
                    // relevant to TY.
                    if(sqlTILL_DATE <= _parent._parent.vUpToDatePY)
                    {
                        ss248Work1.LYWeekToDate.Value += sqlICR_OPEN;
                    }
                }
                protected override void OnEnd()
                {
                    if(false)
                    {
                        new BrowseSs248Work1().Run();
                    }
                }
                
                
                /// <summary>Browse - ss248 Work1(P#46.1.10.1)</summary>
                /// <remark>Last change before Migration: 17/02/2004 15:17:17</remark>
                internal class BrowseSs248Work1 : SalesAndStock.UIControllerBase 
                {
                    
                    #region Models
                    /// <summary>ss248 Work1</summary>
                    internal readonly Models.ss248Work1 ss248Work1 = new Models.ss248Work1 { AllowRowLocking = true };
                    #endregion
                    
                    public BrowseSs248Work1()
                    {
                        Title = "Browse - ss248 Work1";
                        InitializeDataView();
                    }
                    void InitializeDataView()
                    {
                        From = ss248Work1;
                        
                        OrderBy = ss248Work1.SortByss_ss248_Work1_X1;
                        
                        #region Columns
                        
                        Columns.Add(ss248Work1.FinancialWeek);
                        Columns.Add(ss248Work1.BranchNumber);
                        Columns.Add(ss248Work1.DeptReportSequence);
                        Columns.Add(ss248Work1.Department1);
                        Columns.Add(ss248Work1.SubDepartment1);
                        Columns.Add(ss248Work1.TYSaturday);
                        Columns.Add(ss248Work1.TYSunday);
                        Columns.Add(ss248Work1.TYMonday);
                        Columns.Add(ss248Work1.TYTuesday);
                        Columns.Add(ss248Work1.TYWednesday);
                        Columns.Add(ss248Work1.TYThursday);
                        Columns.Add(ss248Work1.TYFriday);
                        Columns.Add(ss248Work1.TYWeekToDate);
                        Columns.Add(ss248Work1.LYSaturday);
                        Columns.Add(ss248Work1.LYSunday);
                        Columns.Add(ss248Work1.LYMonday);
                        Columns.Add(ss248Work1.LYTuesday);
                        Columns.Add(ss248Work1.LYWednesday);
                        Columns.Add(ss248Work1.LYThursday);
                        Columns.Add(ss248Work1.LYFriday);
                        Columns.Add(ss248Work1.LYWeekToDate);
                        #endregion
                    }
                    /// <summary>Browse - ss248 Work1(P#46.1.10.1)</summary>
                    internal void Run()
                    {
                        Execute();
                    }
                    protected override void OnLoad()
                    {
                        RowLocking = LockingStrategy.OnUserEdit;
                        TransactionScope = TransactionScopes.RowLocking;
                        OnDatabaseErrorRetry = false;
                        SwitchToInsertWhenNoRows = true;
                        View = ()=> new Views.DailyBrnSlsPerfSs248BrowseSs248Work1(this);
                    }
                    
                    
                }
            }
            /// <summary>Fetch Concessions(P#46.1.11)</summary>
            /// <remark>Last change before Migration: 16/11/2007 12:31:45</remark>
            class FetchConcessions : SalesAndStock.BusinessProcessBase 
            {
                
                #region Columns
                /// <summary>v:TY PY Indicator</summary>
                readonly TextColumn vTYPYIndicator = new TextColumn("v:TY PY Indicator", "U");
                /// <summary>v:From Date</summary>
                readonly DateColumn vFromDate = new DateColumn("v:From Date");
                /// <summary>v:To Date</summary>
                readonly DateColumn vToDate = new DateColumn("v:To Date");
                #endregion
                
                Control _parent;
                
                public FetchConcessions(Control parent)
                {
                    _parent = parent;
                    Title = "Fetch Concessions";
                    InitializeDataView();
                }
                void InitializeDataView()
                {
                    
                    
                    #region Columns
                    
                    // This task fetches the Concession data for TY and LY by executing the subordinate
                    // task via 2 appropriate sets of parameters for the SQL routine.
                    Columns.Add(vTYPYIndicator);
                    Columns.Add(vFromDate);
                    Columns.Add(vToDate);
                    #endregion
                }
                /// <summary>Fetch Concessions(P#46.1.11)</summary>
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
                    // For TY data
                    vTYPYIndicator.Value = "T";
                    vFromDate.Value = _parent._parent.vWeekStartTY;
                    vToDate.Value = _parent._parent.vUpToDateTY;
                    Cached<ConcessionSQL>().Run();
                    
                    // For PY data
                    vTYPYIndicator.Value = "P";
                    vFromDate.Value = _parent._parent.vWeekStartPY;
                    vToDate.Value = _parent._parent.vWeekEndPY;
                    Cached<ConcessionSQL>().Run();
                }
                
                
                /// <summary>Concession SQL(P#46.1.11.1)</summary>
                /// <remark>Last change before Migration: 16/11/2007 12:31:45</remark>
                class ConcessionSQL : SalesAndStock.BusinessProcessBase 
                {
                    
                    #region Models
                    DynamicSQLEntity sqlEntity;
                    /// <summary>ss248 Work1</summary>
                    readonly Models.ss248Work1 ss248Work1 = new Models.ss248Work1 { AllowRowLocking = true };
                    #endregion
                    
                    #region Columns
                    /// <summary>BRANCH_NUMBER</summary>
                    readonly NumberColumn BRANCH_NUMBER = new NumberColumn("BRANCH_NUMBER", "N4")
                    {
                    	AllowNull = true
                    };
                    /// <summary>CONCESSION_CODE</summary>
                    readonly NumberColumn CONCESSION_CODE = new NumberColumn("CONCESSION_CODE", "N2")
                    {
                    	AllowNull = true
                    };
                    /// <summary>TILL_DATE</summary>
                    readonly DateColumn TILL_DATE = new DateColumn("TILL_DATE", "DD/MM/YYYY")
                    {
                    	AllowNull = true,
                    	Storage = new ENV.Data.Storage.DateTimeDateStorage()
                    };
                    /// <summary>CONCESSION_VALUE</summary>
                    readonly NumberColumn CONCESSION_VALUE = new NumberColumn("CONCESSION_VALUE", "N9.2")
                    {
                    	AllowNull = true
                    };
                    /// <summary>v:Skip</summary>
                    readonly NumberColumn vSkip = new NumberColumn("v:Skip", "1");
                    #endregion
                    
                    FetchConcessions _parent;
                    
                    public ConcessionSQL(FetchConcessions parent)
                    {
                        _parent = parent;
                        Title = "Concession SQL";
                        InitializeDataView();
                    }
                    void InitializeDataView()
                    {
                        sqlEntity = new DynamicSQLEntity(Models.DataSources.SS, 
@"select d.branch_number, d.concession_code, d.till_date, sum(d.value) concession_value
from mackays.sla_details d
where d.till_date between ':1' and ':2'
  and d.concession_code <> 0
  and exists (select 'X' from mackays.ss_ss248_work1 w
              where w.financial_week = :3
                and w.branch_number = d.branch_number)
group by d.branch_number, d.concession_code, d.till_date
order by d.branch_number, d.concession_code, d.till_date");
                        sqlEntity.AddParameter(() => u.DStr(_parent.vFromDate, "DD-MMM-YYYY")); //:1;
                        sqlEntity.AddParameter(() => u.DStr(_parent.vToDate, "DD-MMM-YYYY")); //:2;
                        sqlEntity.AddParameter(_parent._parent._parent.vReqYYYYWW); //:3;
                        sqlEntity.Columns.Add(BRANCH_NUMBER, CONCESSION_CODE, TILL_DATE, CONCESSION_VALUE);
                        From = sqlEntity;
                        
                        Relations.Add(ss248Work1, RelationType.InsertIfNotFound, 
                        		ss248Work1.FinancialWeek.BindEqualTo(_parent._parent._parent.vReqYYYYWW).And(
                        		ss248Work1.BranchNumber.BindEqualTo(BRANCH_NUMBER)).And(
                        		ss248Work1.ReportGroup.BindEqualTo("E")).And(
                        		ss248Work1.DeptReportSequence.BindEqualTo(CONCESSION_CODE)).And(
                        		ss248Work1.Department1.BindEqualTo("99")).And(
                        		ss248Work1.SubDepartment1.BindEqualTo("99")), 
                        	ss248Work1.SortByss_ss248_Work1_X1);
                        
                        
                        
                        #region Columns
                        
                        // From SQL
                        Columns.Add(BRANCH_NUMBER);
                        Columns.Add(CONCESSION_CODE);
                        Columns.Add(TILL_DATE);
                        Columns.Add(CONCESSION_VALUE);
                        // N.B.  For Concessions column 'Dept Report Sequence' is initialised with
                        // 'CONCESSION VALUE' (from SQL input).
                        Columns.Add(ss248Work1.FinancialWeek);
                        Columns.Add(ss248Work1.BranchNumber);
                        Columns.Add(ss248Work1.ReportGroup);
                        Columns.Add(ss248Work1.DeptReportSequence);
                        Columns.Add(ss248Work1.Department1);
                        Columns.Add(ss248Work1.SubDepartment1);
                        Columns.Add(ss248Work1.TYSaturday);
                        Columns.Add(ss248Work1.TYSunday);
                        Columns.Add(ss248Work1.TYMonday);
                        Columns.Add(ss248Work1.TYTuesday);
                        Columns.Add(ss248Work1.TYWednesday);
                        Columns.Add(ss248Work1.TYThursday);
                        Columns.Add(ss248Work1.TYFriday);
                        Columns.Add(ss248Work1.TYWeekToDate);
                        Columns.Add(ss248Work1.LYSaturday);
                        Columns.Add(ss248Work1.LYSunday);
                        Columns.Add(ss248Work1.LYMonday);
                        Columns.Add(ss248Work1.LYTuesday);
                        Columns.Add(ss248Work1.LYWednesday);
                        Columns.Add(ss248Work1.LYThursday);
                        Columns.Add(ss248Work1.LYFriday);
                        Columns.Add(ss248Work1.LYWeekToDate);
                        
                        Columns.Add(vSkip);
                        #endregion
                    }
                    /// <summary>Concession SQL(P#46.1.11.1)</summary>
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
                        // Init SKIP from day-of-week to allow VARSET logic.
                        vSkip.Value = u.If(u.Equals(u.DOW(TILL_DATE), 7), 0, u.DOW(TILL_DATE));
                        
                        // Processing This Year data
                        if(_parent.vTYPYIndicator == "T")
                        {
                            u.VarSet(u.IndexOf(ss248Work1.TYSaturday) + vSkip, u.CastToNumber(u.VarCurr(u.IndexOf(ss248Work1.TYSaturday) + vSkip)) + CONCESSION_VALUE);
                            ss248Work1.TYWeekToDate.Value += CONCESSION_VALUE;
                        }
                        
                        // Processing Previous Year data
                        if(_parent.vTYPYIndicator == "P")
                        {
                            u.VarSet(u.IndexOf(ss248Work1.LYSaturday) + vSkip, u.CastToNumber(u.VarCurr(u.IndexOf(ss248Work1.LYSaturday) + vSkip)) + CONCESSION_VALUE);
                            if(TILL_DATE <= _parent._parent._parent.vUpToDatePY)
                            {
                                ss248Work1.LYWeekToDate.Value += CONCESSION_VALUE;
                            }
                        }
                    }
                    
                    
                }
            }
        }
        /// <summary>SQL Xref Dummy(P#46.2)</summary>
        /// <remark>Last change before Migration: 11/05/2006 16:47:24</remark>
        internal class SQLXrefDummy : SalesAndStock.UIControllerBase 
        {
            
            #region Models
            /// <summary>Section Conversion</summary>
            readonly Models.SectionConversion SectionConversion = new Models.SectionConversion { ReadOnly = true };
            /// <summary>SLA_Tenders</summary>
            readonly Models.SLA_Tenders SLA_Tenders = new Models.SLA_Tenders { ReadOnly = true };
            /// <summary>SLA_Details</summary>
            readonly Models.SLA_Details SLA_Details = new Models.SLA_Details { Cached = false, ReadOnly = true };
            /// <summary>SLA_SectSales</summary>
            readonly Models.SLA_SectSales SLA_SectSales = new Models.SLA_SectSales { ReadOnly = true };
            /// <summary>Credit Card Details</summary>
            readonly Models.CreditCardDetails CreditCardDetails = new Models.CreditCardDetails { ReadOnly = true };
            /// <summary>ss248 Work1</summary>
            readonly Models.ss248Work1 ss248Work1 = new Models.ss248Work1 { ReadOnly = true };
            /// <summary>Diary Work1</summary>
            readonly Models.DiaryWork1 DiaryWork1 = new Models.DiaryWork1 { AllowRowLocking = true };
            #endregion
            
            public SQLXrefDummy()
            {
                Title = "SQL Xref Dummy";
                Entities.Add(SectionConversion);
                Entities.Add(SLA_Tenders);
                Entities.Add(SLA_Details);
                Entities.Add(SLA_SectSales);
                Entities.Add(CreditCardDetails);
                Entities.Add(ss248Work1);
                Entities.Add(DiaryWork1);
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
                View = ()=> new Views.DailyBrnSlsPerfSs248SQLXrefDummy(this);
            }
            
            
        }
        /// <summary>Determine Week(P#46.3)</summary>
        /// <remark>Last change before Migration: 05/05/2008 11:33:20</remark>
        class DetermineWeek : SalesAndStock.BusinessProcessBase 
        {
            
            #region Models
            /// <summary>Date</summary>
            readonly Models.Date1 Date1 = new Models.Date1 { ReadOnly = true };
            #endregion
            
            #region Columns
            /// <summary>v:RelevantDate</summary>
            readonly DateColumn vRelevantDate = new DateColumn("v:RelevantDate");
            /// <summary>v:WeekFound</summary>
            readonly BoolColumn vWeekFound = new BoolColumn("v:WeekFound");
            #endregion
            
            DailyBrnSlsPerfSs248 _parent;
            
            public DetermineWeek(DailyBrnSlsPerfSs248 parent)
            {
                _parent = parent;
                Title = "Determine Week";
                InitializeDataView();
            }
            void InitializeDataView()
            {
                
                Relations.Add(Date1, 
                		Date1.WeekEndingDate.IsGreaterOrEqualTo(vRelevantDate), 
                	Date1.SortByREF_Date_X2);
                
                
                
                #region Columns
                
                // This task decides automatically the week number to be processed.
                
                // If current time is between midnight and noon, week no. will be that
                // equating to yesterday.   Otherwise, it will equate to today's date.
                Columns.Add(vRelevantDate).BindValue(() => u.If(u.Range(u.TStr(Time.Now, "HH:MM:SS"), "00:00:01", "12:00:00"), u.AddDate(Date.Now, 0, 0, -(1)), Date.Now));
                
                Columns.Add(vWeekFound);
                Relations[Date1].NotifyRowWasFoundTo(vWeekFound);
                Columns.Add(Date1.WeekEndingDate);
                Columns.Add(Date1.WeekNumber);
                Columns.Add(Date1.CenturyWeek);
                #endregion
            }
            /// <summary>Determine Week(P#46.3)</summary>
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
                if(u.Not(vWeekFound))
                {
                    Message.ShowWarningInStatusBar("ss248   Week no. for " + u.DStr(vRelevantDate, "DD/MM/YYYY") + " not on Dates table");
                    _parent.vParamError.Value = "Y";
                }
                
                if(vWeekFound)
                {
                    _parent.pYYYYWW.Value = u.Str(Date1.CenturyWeek, "6P0");
                    Message.ShowWarningInStatusBar("ss248   Week No. determined by program = " + _parent.pYYYYWW);
                }
            }
            
            
        }
    }
}
