using Firefly.Box;
using ENV.Data;
namespace MandCo.SalesAndStock.Models
{
    /// <summary>ss252 Area Work Mk2(E#243)</summary>
    public class ss252AreaWorkMk2 : OracleEntity 
    {
        
        #region Columns
        /// <summary>Area Code</summary>
        public readonly TextColumn AreaCode = new TextColumn("Area_Code", "U2A", "Area Code")
        {
        	DefaultValue = "0"
        };
        /// <summary>Report Group</summary>
        public readonly TextColumn ReportGroup = new TextColumn("Report_Group", "UA", "Report Group");
        /// <summary>Report Department</summary>
        public readonly TextColumn ReportDepartment = new TextColumn("Report_Department", "U2A", "Report Department");
        /// <summary>Report SubDepartment</summary>
        public readonly TextColumn ReportSubDepartment = new TextColumn("Report_SubDepartment", "U2A", "Report SubDepartment");
        /// <summary>TY Week</summary>
        public readonly NumberColumn TYWeek = new NumberColumn("TY_Week", "N9.2A", "TY Week");
        /// <summary>TY Period</summary>
        public readonly NumberColumn TYPeriod = new NumberColumn("TY_Period", "N9.2A", "TY Period");
        /// <summary>TY Season</summary>
        public readonly NumberColumn TYSeason = new NumberColumn("TY_Season", "N9.2A", "TY Season");
        /// <summary>TY Year</summary>
        public readonly NumberColumn TYYear = new NumberColumn("TY_Year", "N9.2A", "TY Year");
        /// <summary>LY Week</summary>
        public readonly NumberColumn LYWeek = new NumberColumn("LY_Week", "N9.2A", "LY Week");
        /// <summary>LY Period</summary>
        public readonly NumberColumn LYPeriod = new NumberColumn("LY_Period", "N9.2A", "LY Period");
        /// <summary>LY Season</summary>
        public readonly NumberColumn LYSeason = new NumberColumn("LY_Season", "N9.2A", "LY Season");
        /// <summary>LY Year</summary>
        public readonly NumberColumn LYYear = new NumberColumn("LY_Year", "N9.2A", "LY Year");
        /// <summary>Number Of Bays</summary>
        public readonly NumberColumn NumberOfBays = new NumberColumn("Number_Of_Bays", "4A", "Number Of Bays");
        #endregion
        
        #region Indexes
        /// <summary>ss252_Area_Mk2_X1</summary>
        public readonly Index SortByss252_Area_Mk2_X1 = new Index
        {
        	Caption = "ss252_Area_Mk2_X1",
        	Name = "ss252_Area_Mk2_X1",
        	AutoCreate = true,
        	Unique = true
        };
        #endregion
        
        public ss252AreaWorkMk2():base("ss252_Area_Mk2", "ss252 Area Work Mk2", DataSources.SS)
        {
            UseRowIdAsPrimaryKey();
            InitializeIndexes();
        }
        void InitializeIndexes()
        {
            SortByss252_Area_Mk2_X1.Add(AreaCode, ReportGroup, ReportDepartment, ReportSubDepartment);
            
        }
        
        
    }
}
