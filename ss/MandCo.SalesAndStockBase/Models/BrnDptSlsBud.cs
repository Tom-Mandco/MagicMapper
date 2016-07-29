using Firefly.Box;
using ENV.Data;
namespace MandCo.SalesAndStock.Models
{
    /// <summary>BrnDptSlsBud(E#144)</summary>
    public class BrnDptSlsBud : OracleEntity 
    {
        
        #region Columns
        /// <summary>Branch Number</summary>
        public readonly Types.BranchNumber BranchNumber = new Types.BranchNumber
        {
        	Name = "Branch_Number"
        };
        /// <summary>Department</summary>
        public readonly TextColumn Department1 = new TextColumn("Department", "UUA");
        /// <summary>CenturyWeek</summary>
        public readonly Types.CenturyWeek CenturyWeek = new Types.CenturyWeek
        {
        	Name = "CenturyWeek"
        };
        /// <summary>Sales Budget</summary>
        public readonly NumberColumn SalesBudget = new NumberColumn("Sales_Budget", "9.2", "Sales Budget");
        #endregion
        
        #region Indexes
        /// <summary>SS_BR_DPT_SLS_BUD_X1</summary>
        public readonly Index SortBySS_BR_DPT_SLS_BUD_X1 = new Index
        {
        	Caption = "SS_BR_DPT_SLS_BUD_X1",
        	Name = "SS_BR_DPT_SLS_BUD_X1",
        	AutoCreate = true,
        	Unique = true
        };
        /// <summary>SS_BR_DPT_SLS_BUD_X2</summary>
        public readonly Index SortBySS_BR_DPT_SLS_BUD_X2 = new Index
        {
        	Caption = "SS_BR_DPT_SLS_BUD_X2",
        	Name = "SS_BR_DPT_SLS_BUD_X2",
        	AutoCreate = true,
        	Unique = true
        };
        /// <summary>SS_BR_DPT_SLS_BUD_X3</summary>
        public readonly Index SortBySS_BR_DPT_SLS_BUD_X3 = new Index
        {
        	Caption = "SS_BR_DPT_SLS_BUD_X3",
        	Name = "SS_BR_DPT_SLS_BUD_X3",
        	AutoCreate = true,
        	Unique = true
        };
        #endregion
        
        public BrnDptSlsBud():base("SS_BR_DPT_SLS_BUD", "BrnDptSlsBud", DataSources.SS)
        {
            UseRowIdAsPrimaryKey();
            InitializeIndexes();
        }
        void InitializeIndexes()
        {
            SortBySS_BR_DPT_SLS_BUD_X1.Add(BranchNumber, Department1, CenturyWeek);
            
            SortBySS_BR_DPT_SLS_BUD_X2.Add(BranchNumber, CenturyWeek, Department1);
            
            SortBySS_BR_DPT_SLS_BUD_X3.Add(CenturyWeek, Department1, BranchNumber);
            
        }
        
        
    }
}
