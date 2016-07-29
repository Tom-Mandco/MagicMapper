using Firefly.Box;
using ENV.Data;
namespace MandCo.SalesAndStock.Models
{
    /// <summary>ss252 Branch Work(E#214)</summary>
    public class ss252BranchWork : OracleEntity 
    {
        
        #region Columns
        /// <summary>Area Code</summary>
        public readonly TextColumn AreaCode = new TextColumn("Area_Code", "U2A", "Area Code");
        /// <summary>Branch Number</summary>
        public readonly NumberColumn BranchNumber = new NumberColumn("Branch_Number", "4A", "Branch Number");
        /// <summary>Report Group</summary>
        public readonly TextColumn ReportGroup = new TextColumn("Report_Group", "1A", "Report Group");
        /// <summary>Floor Code</summary>
        public readonly NumberColumn FloorCode = new NumberColumn("Floor_Code", "N1A", "Floor Code");
        /// <summary>Report Department</summary>
        public readonly TextColumn ReportDepartment = new TextColumn("Report_Department", "U2A", "Report Department");
        /// <summary>Report SubDepartment</summary>
        public readonly TextColumn ReportSubDepartment = new TextColumn("Report_SubDepartment", "U2A", "Report SubDepartment");
        /// <summary>TY Week</summary>
        public readonly NumberColumn TYWeek = new NumberColumn("TY_Week", "N8A", "TY Week");
        /// <summary>TY Period</summary>
        public readonly NumberColumn TYPeriod = new NumberColumn("TY_Period", "N8A", "TY Period");
        /// <summary>TY Year</summary>
        public readonly NumberColumn TYYear = new NumberColumn("TY_Year", "N8A", "TY Year");
        /// <summary>Budget Week</summary>
        public readonly NumberColumn BudgetWeek = new NumberColumn("Budget_Week", "N8A", "Budget Week");
        /// <summary>Budget Period</summary>
        public readonly NumberColumn BudgetPeriod = new NumberColumn("Budget_Period", "N8A", "Budget Period");
        /// <summary>Budget Year</summary>
        public readonly NumberColumn BudgetYear = new NumberColumn("Budget_Year", "N8A", "Budget Year");
        /// <summary>LY Week</summary>
        public readonly NumberColumn LYWeek = new NumberColumn("LY_Week", "N8A", "LY Week");
        /// <summary>LY Period</summary>
        public readonly NumberColumn LYPeriod = new NumberColumn("LY_Period", "N8A", "LY Period");
        /// <summary>LY Year</summary>
        public readonly NumberColumn LYYear = new NumberColumn("LY_Year", "N8A", "LY Year");
        /// <summary>Number Of Bays</summary>
        public readonly NumberColumn NumberOfBays = new NumberColumn("Number_Of_Bays", "4A", "Number Of Bays");
        /// <summary>Comp Store</summary>
        public readonly TextColumn CompStore = new TextColumn("Comp_Store", "UA", "Comp Store");
        #endregion
        
        #region Indexes
        /// <summary>ss252_Branch_Work_X1</summary>
        public readonly Index SortByss252_Branch_Work_X1 = new Index
        {
        	Caption = "ss252_Branch_Work_X1",
        	Name = "ss252_Branch_Work_X1",
        	AutoCreate = true,
        	Unique = true
        };
        #endregion
        
        public ss252BranchWork():base("ss252_Branch_Work", "ss252 Branch Work", DataSources.SS)
        {
            UseRowIdAsPrimaryKey();
            InitializeIndexes();
        }
        void InitializeIndexes()
        {
            SortByss252_Branch_Work_X1.Add(AreaCode, BranchNumber, ReportGroup, FloorCode, ReportDepartment, ReportSubDepartment);
            
        }
        
        
    }
}
