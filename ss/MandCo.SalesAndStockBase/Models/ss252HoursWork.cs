using Firefly.Box;
using ENV.Data;
namespace MandCo.SalesAndStock.Models
{
    /// <summary>ss252 Hours Work(E#215)</summary>
    public class ss252HoursWork : OracleEntity 
    {
        
        #region Columns
        /// <summary>Week Number</summary>
        public readonly NumberColumn WeekNumber = new NumberColumn("Week_Number", "6A", "Week Number");
        /// <summary>Area Code</summary>
        public readonly TextColumn AreaCode = new TextColumn("Area_Code", "U2A", "Area Code");
        /// <summary>Branch Number</summary>
        public readonly NumberColumn BranchNumber = new NumberColumn("Branch_Number", "4A", "Branch Number");
        /// <summary>Employee Number</summary>
        public readonly TextColumn EmployeeNumber = new TextColumn("Employee_Number", "10A", "Employee Number");
        /// <summary>TY 39 Hour Rule</summary>
        public readonly TextColumn TY39HourRule = new TextColumn("TY_39_Hour_Rule", "UA", "TY 39 Hour Rule");
        /// <summary>TY Hours</summary>
        public readonly NumberColumn TYHours = new NumberColumn("TY_Hours", "3.2A", "TY Hours");
        /// <summary>LY 39 Hour Rule</summary>
        public readonly TextColumn LY39HourRule = new TextColumn("LY_39_Hour_Rule", "UA", "LY 39 Hour Rule");
        /// <summary>LY Hours</summary>
        public readonly NumberColumn LYHours = new NumberColumn("LY_Hours", "3.2A", "LY Hours");
        #endregion
        
        #region Indexes
        /// <summary>ss252_Hours_Work_X1</summary>
        public readonly Index SortByss252_Hours_Work_X1 = new Index
        {
        	Caption = "ss252_Hours_Work_X1",
        	Name = "ss252_Hours_Work_X1",
        	AutoCreate = true,
        	Unique = true
        };
        #endregion
        
        public ss252HoursWork():base("ss252_Hours_Work", "ss252 Hours Work", DataSources.SS)
        {
            UseRowIdAsPrimaryKey();
            InitializeIndexes();
        }
        void InitializeIndexes()
        {
            SortByss252_Hours_Work_X1.Add(WeekNumber, AreaCode, BranchNumber, EmployeeNumber);
            
        }
        
        
    }
}
