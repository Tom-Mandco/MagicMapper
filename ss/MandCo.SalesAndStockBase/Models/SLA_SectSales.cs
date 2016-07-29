using Firefly.Box;
using ENV.Data;
namespace MandCo.SalesAndStock.Models
{
    /// <summary>SLA_SectSales(E#69)</summary>
    public class SLA_SectSales : OracleEntity 
    {
        
        #region Columns
        /// <summary>PDM_DEPARTMENT_CODE</summary>
        public readonly NumberColumn PDM_DEPARTMENT_CODE = new NumberColumn("PDM_DEPARTMENT_CODE", "N2");
        /// <summary>PDM_SECTION_CODE</summary>
        public readonly NumberColumn PDM_SECTION_CODE = new NumberColumn("PDM_SECTION_CODE", "N2");
        /// <summary>BRANCH_NUMBER</summary>
        public readonly NumberColumn BRANCH_NUMBER = new NumberColumn("BRANCH_NUMBER", "N4");
        /// <summary>TRANSACTION_DATE</summary>
        public readonly DateColumn TRANSACTION_DATE = new DateColumn("TRANSACTION_DATE", "DD/MM/YYYY")
        {
        	Storage = new ENV.Data.Storage.DateTimeDateStorage()
        };
        /// <summary>RTC_TRANSACTION_TYPE</summary>
        public readonly NumberColumn RTC_TRANSACTION_TYPE = new NumberColumn("RTC_TRANSACTION_TYPE", "N2");
        /// <summary>RTC_TRANSACTION_SUBTYPE</summary>
        public readonly TextColumn RTC_TRANSACTION_SUBTYPE = new TextColumn("RTC_TRANSACTION_SUBTYPE", "1");
        /// <summary>RTC_REASON_CODE</summary>
        public readonly NumberColumn RTC_REASON_CODE = new NumberColumn("RTC_REASON_CODE", "N8");
        /// <summary>SECTION_VALUE</summary>
        public readonly NumberColumn SECTION_VALUE = new NumberColumn("SECTION_VALUE", "N9.2");
        #endregion
        
        #region Indexes
        /// <summary>SLA_SECTSALES_X1</summary>
        public readonly Index SortBySLA_SECTSALES_X1 = new Index
        {
        	Caption = "SLA_SECTSALES_X1",
        	Name = "SLA_SECTSALES_X1",
        	AutoCreate = true,
        	Unique = true
        };
        /// <summary>SLA_SECTSALES_X2</summary>
        public readonly Index SortBySLA_SECTSALES_X2 = new Index
        {
        	Caption = "SLA_SECTSALES_X2",
        	Name = "SLA_SECTSALES_X2",
        	AutoCreate = true,
        	Unique = true
        };
        /// <summary>SLA_SECTSALES_X3</summary>
        public readonly Index SortBySLA_SECTSALES_X3 = new Index
        {
        	Caption = "SLA_SECTSALES_X3",
        	Name = "SLA_SECTSALES_X3",
        	AutoCreate = true,
        	Unique = true
        };
        #endregion
        
        public SLA_SectSales():base("mackays.SLA_SectSales", "SLA_SectSales", DataSources.SLA)
        {
            AutoCreateTable = true;
            UseRowIdAsPrimaryKey();
            InitializeIndexes();
        }
        void InitializeIndexes()
        {
            SortBySLA_SECTSALES_X1.Add(PDM_DEPARTMENT_CODE, PDM_SECTION_CODE, BRANCH_NUMBER, TRANSACTION_DATE, RTC_TRANSACTION_TYPE, RTC_TRANSACTION_SUBTYPE);
            
            SortBySLA_SECTSALES_X2.Add(BRANCH_NUMBER, PDM_DEPARTMENT_CODE, PDM_SECTION_CODE, TRANSACTION_DATE, RTC_TRANSACTION_TYPE, RTC_TRANSACTION_SUBTYPE);
            
            SortBySLA_SECTSALES_X3.Add(TRANSACTION_DATE, BRANCH_NUMBER, PDM_DEPARTMENT_CODE, PDM_SECTION_CODE, RTC_TRANSACTION_TYPE, RTC_TRANSACTION_SUBTYPE);
            
        }
        
        
    }
}
