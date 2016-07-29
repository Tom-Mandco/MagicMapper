using Firefly.Box;
using ENV.Data;
namespace MandCo.SalesAndStock.Models
{
    /// <summary>ss252 Print Work(E#217)</summary>
    public class ss252PrintWork : OracleEntity 
    {
        
        #region Columns
        /// <summary>Branch Number</summary>
        public readonly NumberColumn BranchNumber = new NumberColumn("Branch_Number", "4A", "Branch Number");
        /// <summary>Record Type</summary>
        public readonly TextColumn RecordType = new TextColumn("Record_Type", "UA", "Record Type");
        /// <summary>Report Group</summary>
        public readonly TextColumn ReportGroup = new TextColumn("Report_Group", "U2A", "Report Group");
        /// <summary>Floor Code</summary>
        public readonly NumberColumn FloorCode = new NumberColumn("Floor_Code", "N1A", "Floor Code");
        /// <summary>Report Department</summary>
        public readonly TextColumn ReportDepartment = new TextColumn("Report_Department", "U2A", "Report Department");
        /// <summary>Report SubDepartment</summary>
        public readonly TextColumn ReportSubDepartment = new TextColumn("Report_SubDepartment", "U2A", "Report SubDepartment");
        /// <summary>Description</summary>
        public readonly TextColumn Description = new TextColumn("Description", "18A");
        /// <summary>This Year</summary>
        public readonly NumberColumn ThisYear = new NumberColumn("This_Year", "N8.2A", "This Year");
        /// <summary>Budget</summary>
        public readonly NumberColumn Budget = new NumberColumn("Budget", "N8.2A");
        /// <summary>Last Year</summary>
        public readonly NumberColumn LastYear = new NumberColumn("Last_Year", "N8.2A", "Last Year");
        /// <summary>PCent To Budget</summary>
        public readonly NumberColumn PCentToBudget = new NumberColumn("PCent_To_Budget", "N3.2A", "PCent To Budget");
        /// <summary>PCent To LY</summary>
        public readonly NumberColumn PCentToLY = new NumberColumn("PCent_To_Last_Year", "N3.2A", "PCent To LY");
        /// <summary>Area PCent To LY</summary>
        public readonly NumberColumn AreaPCentToLY = new NumberColumn("Area_PCent_To_LY", "N3.2A", "Area PCent To LY");
        /// <summary>PCent Var To Area</summary>
        public readonly NumberColumn PCentVarToArea = new NumberColumn("PCent_Var_To_Area", "N3.2A", "PCent Var To Area");
        /// <summary>PCent Sales By Floor</summary>
        public readonly NumberColumn PCentSalesByFloor = new NumberColumn("PCent_Sales_By_Floor", "N3.1A", "PCent Sales By Floor");
        /// <summary>PCent Bay By Floor</summary>
        public readonly NumberColumn PCentBayByFloor = new NumberColumn("PCent_Bay_By_Floor", "N3.1A", "PCent Bay By Floor");
        /// <summary>Sales Per Bay</summary>
        public readonly NumberColumn SalesPerBay = new NumberColumn("Sales_Per_Bay", "N8.2A", "Sales Per Bay");
        /// <summary>Area Sales Per Bay</summary>
        public readonly NumberColumn AreaSalesPerBay = new NumberColumn("Area_Sales_Per_Bay", "N8.2A", "Area Sales Per Bay");
        /// <summary>Bay Sales Var To Area</summary>
        public readonly NumberColumn BaySalesVarToArea = new NumberColumn("Bay_Sales_Var_To_Area", "N8.2A", "Bay Sales Var To Area");
        /// <summary>Bay PCent Var To Area</summary>
        public readonly NumberColumn BayPCentVarToArea = new NumberColumn("Bay_PCent_Var_To_Area", "N3.2A", "Bay PCent Var To Area");
        /// <summary>Bay Count</summary>
        public readonly NumberColumn BayCount = new NumberColumn("Bay_Count", "N8A", "Bay Count");
        #endregion
        
        #region Indexes
        /// <summary>ss252_Print_Work_x1</summary>
        public readonly Index SortByss252_Print_Work_x1 = new Index
        {
        	Caption = "ss252_Print_Work_x1",
        	Name = "ss252_Print_Work_x1",
        	AutoCreate = true,
        	Unique = true
        };
        #endregion
        
        public ss252PrintWork():base("ss252_Print_Work", "ss252 Print Work", DataSources.SS)
        {
            UseRowIdAsPrimaryKey();
            InitializeIndexes();
        }
        void InitializeIndexes()
        {
            SortByss252_Print_Work_x1.Add(BranchNumber, RecordType, ReportGroup, FloorCode, ReportDepartment, ReportSubDepartment);
            
        }
        
        
    }
}
