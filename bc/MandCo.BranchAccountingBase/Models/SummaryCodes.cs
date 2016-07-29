#region Copyright Firefly Ltd 2014
/* *********************** DISCLAIMER **************************************
 * This software and documentation constitute an unpublished work and contain 
 * valuable trade secrets and proprietary information belonging to Firefly Ltd. 
 * None of the foregoing material may be copied, duplicated or disclosed without 
 * the express written permission of Firefly Ltd. 
 * FIREFLY LTD EXPRESSLY DISCLAIMS ANY AND ALL WARRANTIES CONCERNING THIS SOFTWARE 
 * AND DOCUMENTATION, INCLUDING ANY WARRANTIES OF MERCHANTABILITY AND/OR FITNESS 
 * FOR ANY PARTICULAR PURPOSE, AND WARRANTIES OF PERFORMANCE, AND ANY WARRANTY 
 * THAT MIGHT OTHERWISE ARISE FROM COURSE OF DEALING OR USAGE OF TRADE. NO WARRANTY 
 * IS EITHER EXPRESS OR IMPLIED WITH RESPECT TO THE USE OF THE SOFTWARE OR 
 * DOCUMENTATION. 
 * Under no circumstances shall Firefly Ltd be liable for incidental, special, 
 * indirect, direct or consequential damages or loss of profits, interruption of 
 * business, or related expenses which may arise from use of software or documentation, 
 * including but not limited to those resulting from defects in software and/or 
 * documentation, or loss or inaccuracy of data of any kind. 
 */
#endregion
using Firefly.Box;
using ENV.Data;
namespace MandCo.BranchAccounting.Models
{
    
    /// <summary>Summary Codes(E#11)</summary>
    public class SummaryCodes : OracleEntity 
    {
        
        #region Columns
        
        /// <summary>Summary Code</summary>
        public readonly Types.SummaryCode SummaryCode = new Types.SummaryCode
        {
        	Name = "Summary_Code"
        };
        
        /// <summary>Transaction Type</summary>
        public readonly Types.TransactionType TransactionType = new Types.TransactionType
        {
        	Name = "Transaction_Type"
        };
        
        /// <summary>Indicator</summary>
        public readonly Types.Indicator Indicator = new Types.Indicator
        {
        	Name = "Indicator"
        };
        
        /// <summary>Summary Code Decription</summary>
        public readonly Types.Alpha30 SummaryCodeDecription = new Types.Alpha30
        {
        	Caption = "Summary Code Decription",
        	Name = "Summary_Code_Decription"
        };
        
        /// <summary>Summary Balance Code</summary>
        public readonly Types.SummaryCode SummaryBalanceCode = new Types.SummaryCode
        {
        	Caption = "Summary Balance Code",
        	Name = "Summary_Balance_Code"
        };
        
        /// <summary>Sequence Code A</summary>
        public readonly Types.SequenceCode SequenceCodeA = new Types.SequenceCode
        {
        	Caption = "Sequence Code A",
        	Name = "Sequence_Code_A"
        };
        
        /// <summary>Sequence Code B</summary>
        public readonly Types.SequenceCode SequenceCodeB = new Types.SequenceCode
        {
        	Caption = "Sequence Code B",
        	Name = "Sequence_Code_B"
        };
        
        /// <summary>Sequence Code C</summary>
        public readonly Types.SequenceCode SequenceCodeC = new Types.SequenceCode
        {
        	Caption = "Sequence Code C",
        	Name = "Sequence_Code_C"
        };
        
        /// <summary>Sequence Code D</summary>
        public readonly Types.SequenceCode SequenceCodeD = new Types.SequenceCode
        {
        	Caption = "Sequence Code D",
        	Name = "Sequence_Code_D"
        };
        
        /// <summary>Last Update User</summary>
        public readonly Types.Alpha10 LastUpdateUser = new Types.Alpha10
        {
        	Caption = "Last Update User",
        	Name = "Last_Update_User"
        };
        
        /// <summary>Last Update Date</summary>
        public readonly Types.Date1 LastUpdateDate = new Types.Date1
        {
        	Caption = "Last Update Date",
        	Name = "Last_Update_Date",
        	AllowNull = true,
        	DefaultValue = null,
        	Storage = new ENV.Data.Storage.DateTimeDateStorage()
        };
        
        /// <summary>Last Update Time</summary>
        public readonly Types.Time1 LastUpdateTime = new Types.Time1
        {
        	Caption = "Last Update Time",
        	Name = "Last_Update_Time",
        	Storage = new ENV.Data.Storage.StringTimeStorage()
        };
        #endregion
        
        #region Indexes
        
        /// <summary>BAC_Summ_Codes_X1</summary>
        public readonly Index SortByBAC_Summ_Codes_X1 = new Index
        {
        	Caption = "BAC_Summ_Codes_X1",
        	Name = "BAC_Summ_Codes_X1",
        	AutoCreate = true,
        	Unique = true
        };
        #endregion
        
        
        /// <summary>Summary Codes(E#11)</summary>
        public SummaryCodes():base("BAC_Summ_Codes", "Summary Codes", DataSources.BAC)
        {
            Cached = false;
            UseRowIdAsPrimaryKey();
            InitializeIndexes();
        }
        void InitializeIndexes()
        {
            SortByBAC_Summ_Codes_X1.Add(SummaryCode);
            
        }
        
        
    }
}
