using Firefly.Box;
using ENV.Data;
namespace MandCo.SalesAndStock.Models
{
    /// <summary>ss248 Work1(E#211)</summary>
    public class ss248Work1 : OracleEntity 
    {
        
        #region Columns
        /// <summary>Financial Week</summary>
        public readonly NumberColumn FinancialWeek = new NumberColumn("Financial_Week", "6P0A", "Financial Week");
        /// <summary>Branch Number</summary>
        public readonly NumberColumn BranchNumber = new NumberColumn("Branch_Number", "4A", "Branch Number");
        /// <summary>Report Group</summary>
        public readonly TextColumn ReportGroup = new TextColumn("Report_Group", "UA", "Report Group");
        /// <summary>Dept Report Sequence</summary>
        public readonly NumberColumn DeptReportSequence = new NumberColumn("Dept_Report_Sequence", "2A", "Dept Report Sequence");
        /// <summary>Department</summary>
        public readonly TextColumn Department1 = new TextColumn("Department", "2A");
        /// <summary>SubDepartment</summary>
        public readonly TextColumn SubDepartment1 = new TextColumn("SubDepartment", "2A");
        /// <summary>TY Saturday</summary>
        public readonly NumberColumn TYSaturday = new NumberColumn("TY_Saturday", "N9.2A", "TY Saturday");
        /// <summary>TY Sunday</summary>
        public readonly NumberColumn TYSunday = new NumberColumn("TY_Sunday", "N9.2A", "TY Sunday");
        /// <summary>TY Monday</summary>
        public readonly NumberColumn TYMonday = new NumberColumn("TY_Monday", "N9.2A", "TY Monday");
        /// <summary>TY Tuesday</summary>
        public readonly NumberColumn TYTuesday = new NumberColumn("TY_Tuesday", "N9.2A", "TY Tuesday");
        /// <summary>TY Wednesday</summary>
        public readonly NumberColumn TYWednesday = new NumberColumn("TY_Wednesday", "N9.2A", "TY Wednesday");
        /// <summary>TY Thursday</summary>
        public readonly NumberColumn TYThursday = new NumberColumn("TY_Thursday", "N9.2A", "TY Thursday");
        /// <summary>TY Friday</summary>
        public readonly NumberColumn TYFriday = new NumberColumn("TY_Friday", "N9.2A", "TY Friday");
        /// <summary>TY Week To Date</summary>
        public readonly NumberColumn TYWeekToDate = new NumberColumn("TY_Week_To_Date", "N9.2A", "TY Week To Date");
        /// <summary>LY Saturday</summary>
        public readonly NumberColumn LYSaturday = new NumberColumn("LY_Saturday", "N9.2A", "LY Saturday");
        /// <summary>LY Sunday</summary>
        public readonly NumberColumn LYSunday = new NumberColumn("LY_Sunday", "N9.2A", "LY Sunday");
        /// <summary>LY Monday</summary>
        public readonly NumberColumn LYMonday = new NumberColumn("LY_Monday", "N9.2A", "LY Monday");
        /// <summary>LY Tuesday</summary>
        public readonly NumberColumn LYTuesday = new NumberColumn("LY_Tuesday", "N9.2A", "LY Tuesday");
        /// <summary>LY Wednesday</summary>
        public readonly NumberColumn LYWednesday = new NumberColumn("LY_Wednesday", "N9.2A", "LY Wednesday");
        /// <summary>LY Thursday</summary>
        public readonly NumberColumn LYThursday = new NumberColumn("LY_Thursday", "N9.2A", "LY Thursday");
        /// <summary>LY Friday</summary>
        public readonly NumberColumn LYFriday = new NumberColumn("LY_Friday", "N9.2A", "LY Friday");
        /// <summary>LY Week To Date</summary>
        public readonly NumberColumn LYWeekToDate = new NumberColumn("LY_Week_To_Date", "N9.2A", "LY Week To Date");
        #endregion
        
        #region Indexes
        /// <summary>ss_ss248_Work1_X1</summary>
        public readonly Index SortByss_ss248_Work1_X1 = new Index
        {
        	Caption = "ss_ss248_Work1_X1",
        	Name = "ss_ss248_Work1_X1",
        	AutoCreate = true,
        	Unique = true
        };
        #endregion
        
        public ss248Work1():base("ss_ss248_Work1", "ss248 Work1", DataSources.SS)
        {
            UseRowIdAsPrimaryKey();
            InitializeIndexes();
        }
        void InitializeIndexes()
        {
            SortByss_ss248_Work1_X1.Add(FinancialWeek, BranchNumber, ReportGroup, DeptReportSequence, Department1, SubDepartment1);
            
        }
        
        
    }
}
