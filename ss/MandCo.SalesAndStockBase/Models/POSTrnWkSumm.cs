using Firefly.Box;
using ENV.Data;
namespace MandCo.SalesAndStock.Models
{
    /// <summary>POS Trn Wk Summ(E#220)</summary>
    public class POSTrnWkSumm : OracleEntity 
    {
        
        #region Columns
        /// <summary>Week No</summary>
        public readonly NumberColumn WeekNo = new NumberColumn("WEEK_NO", "6P0A", "Week No")
        {
        	AllowNull = false
        };
        /// <summary>Branch Number</summary>
        public readonly NumberColumn BranchNumber = new NumberColumn("BRANCH_NUMBER", "4A", "Branch Number")
        {
        	AllowNull = false
        };
        /// <summary>Trans Type</summary>
        public readonly NumberColumn TransType = new NumberColumn("TRANS_TYPE", "2A", "Trans Type")
        {
        	AllowNull = false
        };
        /// <summary>Trans Sub Type</summary>
        public readonly TextColumn TransSubType = new TextColumn("TRANS_SUB_TYPE", "UA", "Trans Sub Type")
        {
        	AllowNull = false
        };
        /// <summary>Total Units</summary>
        public readonly NumberColumn TotalUnits = new NumberColumn("TOTAL_UNITS", "N9.2A", "Total Units")
        {
        	AllowNull = false
        };
        /// <summary>Total Net Value</summary>
        public readonly NumberColumn TotalNetValue = new NumberColumn("TOTAL_NET_VALUE", "N10.2", "Total Net Value")
        {
        	AllowNull = false
        };
        /// <summary>Summary DateTime</summary>
        public readonly TextColumn SummaryDateTime = new TextColumn("SUMMARY_DATETIME", "19A", "Summary DateTime")
        {
        	Storage = new ENV.Data.Storage.DateTimeTextStorage()
        };
        /// <summary>Transaction Count</summary>
        public readonly NumberColumn TransactionCount = new NumberColumn("TRANSACTION_COUNT", "N9", "Transaction Count")
        {
        	AllowNull = false
        };
        #endregion
        
        #region Indexes
        /// <summary>ss_POS_Trn_Wk_Summ_X1</summary>
        public readonly Index SortByss_POS_Trn_Wk_Summ_X1 = new Index
        {
        	Caption = "ss_POS_Trn_Wk_Summ_X1",
        	Name = "SS_POS_TRN_WK_SUMM_X1",
        	AutoCreate = true,
        	Unique = true
        };
        /// <summary>ss_POS_Trn_Wk_Summ_X2</summary>
        public readonly Index SortByss_POS_Trn_Wk_Summ_X2 = new Index
        {
        	Caption = "ss_POS_Trn_Wk_Summ_X2",
        	Name = "SS_POS_TRN_WK_SUMM_X2",
        	AutoCreate = true,
        	Unique = true
        };
        #endregion
        
        public POSTrnWkSumm():base("mackays.ss_POS_Trn_Wk_Summ", "POS Trn Wk Summ", DataSources.SS)
        {
            UseRowIdAsPrimaryKey();
            InitializeIndexes();
        }
        void InitializeIndexes()
        {
            SortByss_POS_Trn_Wk_Summ_X1.Add(WeekNo, BranchNumber, TransType, TransSubType);
            
            SortByss_POS_Trn_Wk_Summ_X2.Add(BranchNumber, WeekNo, TransType, TransSubType);
            
        }
        
        
    }
}
