using Firefly.Box;
using ENV.Data;
namespace MandCo.SalesAndStock.Models
{
    /// <summary>SLA_Tenders(E#66)</summary>
    public class SLA_Tenders : OracleEntity 
    {
        
        #region Columns
        /// <summary>BRANCH_NUMBER</summary>
        public readonly NumberColumn BRANCH_NUMBER = new NumberColumn("BRANCH_NUMBER", "N4");
        /// <summary>RTC_TRANSACTION_TYPE</summary>
        public readonly NumberColumn RTC_TRANSACTION_TYPE = new NumberColumn("RTC_TRANSACTION_TYPE", "N2");
        /// <summary>RTC_TRANSACTION_SUBTYPE</summary>
        public readonly TextColumn RTC_TRANSACTION_SUBTYPE = new TextColumn("RTC_TRANSACTION_SUBTYPE", "1");
        /// <summary>RTC_TENDER_TYPE_CODE</summary>
        public readonly NumberColumn RTC_TENDER_TYPE_CODE = new NumberColumn("RTC_TENDER_TYPE_CODE", "N2");
        /// <summary>TILL_DATE</summary>
        public readonly DateColumn TILL_DATE = new DateColumn("TILL_DATE", "DD/MM/YYYY")
        {
        	Storage = new ENV.Data.Storage.DateTimeDateStorage()
        };
        /// <summary>VALUE</summary>
        public readonly NumberColumn VALUE = new NumberColumn("VALUE", "N9.2");
        /// <summary>MANUAL_ADJUSTMENT</summary>
        public readonly TextColumn MANUAL_ADJUSTMENT = new TextColumn("MANUAL_ADJUSTMENT", "1");
        /// <summary>UPDATE_USER_ID</summary>
        public readonly TextColumn UPDATE_USER_ID = new TextColumn("UPDATE_USER_ID", "10");
        /// <summary>UPDATE_DATE</summary>
        public readonly DateColumn UPDATE_DATE = new DateColumn("UPDATE_DATE", "DD/MM/YYYY")
        {
        	Storage = new ENV.Data.Storage.DateTimeDateStorage()
        };
        /// <summary>UPDATE_TIME</summary>
        public readonly TimeColumn UPDATE_TIME = new TimeColumn("UPDATE_TIME")
        {
        	Storage = new ENV.Data.Storage.StringTimeStorage()
        };
        /// <summary>CONCESSION_CODE</summary>
        public readonly NumberColumn CONCESSION_CODE = new NumberColumn("CONCESSION_CODE", "N2");
        #endregion
        
        #region Indexes
        /// <summary>SLA_TENDERS_X1</summary>
        public readonly Index SortBySLA_TENDERS_X1 = new Index
        {
        	Caption = "SLA_TENDERS_X1",
        	Name = "SLA_TENDERS_X1",
        	AutoCreate = true,
        	Unique = true
        };
        /// <summary>SLA_TENDERS_X2</summary>
        public readonly Index SortBySLA_TENDERS_X2 = new Index
        {
        	Caption = "SLA_TENDERS_X2",
        	Name = "SLA_TENDERS_X2",
        	AutoCreate = true,
        	Unique = true
        };
        /// <summary>SLA_TENDERS_X3</summary>
        public readonly Index SortBySLA_TENDERS_X3 = new Index
        {
        	Caption = "SLA_TENDERS_X3",
        	Name = "SLA_TENDERS_X3",
        	AutoCreate = true,
        	Unique = true
        };
        /// <summary>SLA_TENDERS_X4</summary>
        public readonly Index SortBySLA_TENDERS_X4 = new Index
        {
        	Caption = "SLA_TENDERS_X4",
        	Name = "SLA_TENDERS_X4",
        	AutoCreate = true,
        	Unique = true
        };
        #endregion
        
        public SLA_Tenders():base("mackays.SLA_Tenders", "SLA_Tenders", DataSources.SLA)
        {
            AutoCreateTable = true;
            UseRowIdAsPrimaryKey();
            InitializeIndexes();
        }
        void InitializeIndexes()
        {
            SortBySLA_TENDERS_X1.Add(RTC_TRANSACTION_TYPE, RTC_TRANSACTION_SUBTYPE, BRANCH_NUMBER, TILL_DATE, RTC_TENDER_TYPE_CODE, CONCESSION_CODE);
            
            SortBySLA_TENDERS_X2.Add(RTC_TENDER_TYPE_CODE, BRANCH_NUMBER, TILL_DATE, RTC_TRANSACTION_TYPE, RTC_TRANSACTION_SUBTYPE, CONCESSION_CODE);
            
            SortBySLA_TENDERS_X3.Add(BRANCH_NUMBER, TILL_DATE, RTC_TRANSACTION_TYPE, RTC_TRANSACTION_SUBTYPE, RTC_TENDER_TYPE_CODE, CONCESSION_CODE);
            
            SortBySLA_TENDERS_X4.Add(TILL_DATE, BRANCH_NUMBER, RTC_TRANSACTION_TYPE, RTC_TRANSACTION_SUBTYPE, RTC_TENDER_TYPE_CODE, CONCESSION_CODE);
            
        }
        
        
    }
}
