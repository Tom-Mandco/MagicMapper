using Firefly.Box;
using ENV.Data;
namespace MandCo.SalesAndStock.Models
{
    /// <summary>Season(E#23)</summary>
    public class Season : OracleEntity 
    {
        
        #region Columns
        /// <summary>Season Code</summary>
        public readonly Types.SeasonCode SeasonCode = new Types.SeasonCode
        {
        	Name = "Season_Code"
        };
        /// <summary>Year</summary>
        public readonly NumberColumn Year = new NumberColumn("Year", "##")
        {
        	StatusTip = Views.ToolTips.WeekNumberP,
        	CustomHelp = null
        };
        /// <summary>Weeks per Period</summary>
        public readonly Types.WeeksPerPeriod WeeksPerPeriod = new Types.WeeksPerPeriod
        {
        	Name = "Weeks_per_Period"
        };
        /// <summary>Season Start Week</summary>
        public readonly Types.WeekNumber SeasonStartWeek = new Types.WeekNumber
        {
        	Caption = "Season Start Week",
        	Name = "Season_Start_Week"
        };
        /// <summary>Season End Week</summary>
        public readonly Types.WeekNumber SeasonEndWeek = new Types.WeekNumber
        {
        	Caption = "Season End Week",
        	Name = "Season_End_Week"
        };
        /// <summary>Branch Target Stock</summary>
        public readonly Types.BranchTargetStock BranchTargetStock = new Types.BranchTargetStock
        {
        	Name = "Branch_Target_Stock"
        };
        /// <summary>WHouse Target Stock</summary>
        public readonly Types.WhouseTargetStock WHouseTargetStock = new Types.WhouseTargetStock
        {
        	Caption = "WHouse Target Stock",
        	Name = "WHouse_Target_Stock"
        };
        /// <summary>Current Season</summary>
        public readonly Types.CurrentSeason CurrentSeason = new Types.CurrentSeason
        {
        	Name = "Current_Season"
        };
        /// <summary>Century Year</summary>
        public readonly Types.Num4 CenturyYear = new Types.Num4
        {
        	Caption = "Century Year",
        	Name = "Century_Year"
        };
        /// <summary>Season Start Century Week</summary>
        public readonly Types.Num6 SeasonStartCenturyWeek = new Types.Num6
        {
        	Caption = "Season Start Century Week",
        	Name = "Season_Start_Century_Week"
        };
        /// <summary>Season End Century Week</summary>
        public readonly Types.Num6 SeasonEndCenturyWeek = new Types.Num6
        {
        	Caption = "Season End Century Week",
        	Name = "Season_End_Century_Week"
        };
        #endregion
        
        #region Indexes
        /// <summary>REF_Season_X1</summary>
        public readonly Index SortByREF_Season_X1 = new Index
        {
        	Caption = "REF_Season_X1",
        	Name = "REF_Season_X1",
        	AutoCreate = true,
        	Unique = true
        };
        /// <summary>REF_Season_X2</summary>
        public readonly Index SortByREF_Season_X2 = new Index
        {
        	Caption = "REF_Season_X2",
        	Name = "REF_Season_X2",
        	AutoCreate = true,
        	Unique = true
        };
        /// <summary>REF_Season_X3</summary>
        public readonly Index SortByREF_Season_X3 = new Index
        {
        	Caption = "REF_Season_X3",
        	Name = "REF_Season_X3",
        	AutoCreate = true,
        	Unique = true
        };
        /// <summary>REF_Season_X4</summary>
        public readonly Index SortByREF_Season_X4 = new Index
        {
        	Caption = "REF_Season_X4",
        	Name = "REF_Season_X4",
        	AutoCreate = true,
        	Unique = true
        };
        /// <summary>REF_Season_X5</summary>
        public readonly Index SortByREF_Season_X5 = new Index
        {
        	Caption = "REF_Season_X5",
        	Name = "REF_Season_X5",
        	AutoCreate = true,
        	Unique = true
        };
        /// <summary>REF_Season_X6</summary>
        public readonly Index SortByREF_Season_X6 = new Index
        {
        	Caption = "REF_Season_X6",
        	Name = "REF_Season_X6",
        	AutoCreate = true,
        	Unique = true
        };
        #endregion
        
        public Season():base("REF_Season", "Season", DataSources.Ref1)
        {
            Cached = false;
            UseRowIdAsPrimaryKey();
            InitializeIndexes();
        }
        void InitializeIndexes()
        {
            SortByREF_Season_X1.Add(SeasonCode, Year);
            
            SortByREF_Season_X2.Add(SeasonStartWeek);
            
            SortByREF_Season_X3.Add(SeasonEndWeek);
            
            SortByREF_Season_X4.Add(SeasonCode, CenturyYear);
            
            SortByREF_Season_X5.Add(SeasonStartCenturyWeek);
            
            SortByREF_Season_X6.Add(SeasonEndCenturyWeek);
            
        }
        
        
    }
}
