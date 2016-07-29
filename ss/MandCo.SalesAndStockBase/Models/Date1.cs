using Firefly.Box;
using ENV.Data;
namespace MandCo.SalesAndStock.Models
{
    /// <summary>Date(E#12)</summary>
    public class Date1 : OracleEntity 
    {
        
        #region Columns
        /// <summary>Week Number</summary>
        public readonly Types.WeekNumber WeekNumber = new Types.WeekNumber
        {
        	Name = "Week_Number"
        };
        /// <summary>Week Ending Date</summary>
        public readonly Types.Date1 WeekEndingDate = new Types.Date1
        {
        	Caption = "Week Ending Date",
        	Name = "Week_Ending_Date",
        	Storage = new ENV.Data.Storage.DateTimeDateStorage()
        };
        /// <summary>Current Year Ind</summary>
        public readonly Types.CurrentYearInd CurrentYearInd = new Types.CurrentYearInd
        {
        	Name = "Current_Year_Ind",
        	InputRange = "Y,N"
        };
        /// <summary>End of Week Done</summary>
        public readonly Types.EndOfWeekDone EndOfWeekDone = new Types.EndOfWeekDone
        {
        	Name = "End_of_Week_Done",
        	InputRange = "Y,N"
        };
        /// <summary>Cost of Sales Xfered</summary>
        public readonly Types.CostOfSalesXfered CostOfSalesXfered = new Types.CostOfSalesXfered
        {
        	Name = "Cost_of_Sales_Xfered",
        	InputRange = "Y,N"
        };
        /// <summary>Sales Audit Process</summary>
        public readonly Types.SalesAuditProcess SalesAuditProcess = new Types.SalesAuditProcess
        {
        	Name = "Sales_Audit_Process"
        };
        /// <summary>Dept Sls Nom Xfered</summary>
        public readonly Types.DeptSlsNomXfered DeptSlsNomXfered = new Types.DeptSlsNomXfered
        {
        	Name = "Dept_Sls_Nom_Xfered",
        	InputRange = "Y,N"
        };
        /// <summary>DeprecDisc Xfered</summary>
        public readonly Types.DeprecDiscXfered DeprecDiscXfered = new Types.DeprecDiscXfered
        {
        	Caption = "DeprecDisc Xfered",
        	Name = "DeprecDisc_Xfered",
        	InputRange = "Y,N"
        };
        /// <summary>Br AC 1 Xfered</summary>
        public readonly Types.BrAcc1Xfered BrAC1Xfered = new Types.BrAcc1Xfered
        {
        	Caption = "Br AC 1 Xfered",
        	Name = "Br_AC_1_Xfered",
        	InputRange = "Y,N"
        };
        /// <summary>Bank WeekEnd Date</summary>
        public readonly Types.Date1 BankWeekEndDate = new Types.Date1
        {
        	Caption = "Bank WeekEnd Date",
        	Name = "Bank_WeekEnd_Date",
        	Storage = new ENV.Data.Storage.DateTimeDateStorage()
        };
        /// <summary>CenturyWeek</summary>
        public readonly Types.CenturyWeek CenturyWeek = new Types.CenturyWeek
        {
        	Name = "CenturyWeek"
        };
        #endregion
        
        #region Indexes
        /// <summary>REF_Date_X1</summary>
        public readonly Index SortByREF_Date_X1 = new Index
        {
        	Caption = "REF_Date_X1",
        	Name = "REF_Date_X1",
        	AutoCreate = true,
        	Unique = true
        };
        /// <summary>REF_Date_X2</summary>
        public readonly Index SortByREF_Date_X2 = new Index
        {
        	Caption = "REF_Date_X2",
        	Name = "REF_Date_X2",
        	AutoCreate = true,
        	Unique = true
        };
        /// <summary>REF_Date_X3</summary>
        public readonly Index SortByREF_Date_X3 = new Index
        {
        	Caption = "REF_Date_X3",
        	Name = "REF_Date_X3",
        	AutoCreate = true,
        	Unique = true
        };
        #endregion
        
        public Date1():base("REF_Date", "Date", DataSources.Ref1)
        {
            Cached = false;
            UseRowIdAsPrimaryKey();
            InitializeIndexes();
        }
        void InitializeIndexes()
        {
            SortByREF_Date_X1.Add(WeekNumber);
            
            SortByREF_Date_X2.Add(WeekEndingDate);
            
            SortByREF_Date_X3.Add(CenturyWeek);
            
        }
        
        
    }
}
