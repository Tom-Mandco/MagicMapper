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
    
    /// <summary>Transaction Codes(E#10)</summary>
    public class TransactionCodes : OracleEntity 
    {
        
        #region Columns
        
        /// <summary>Transaction Code</summary>
        public readonly Types.TransactionCode TransactionCode = new Types.TransactionCode
        {
        	Name = "Transaction_Code"
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
        
        /// <summary>Transaction Description</summary>
        public readonly Types.Alpha30 TransactionDescription = new Types.Alpha30
        {
        	Caption = "Transaction Description",
        	Name = "Transaction_Description"
        };
        
        /// <summary>Balance Code</summary>
        public readonly Types.TransactionCode BalanceCode = new Types.TransactionCode
        {
        	Caption = "Balance Code",
        	Name = "Balance_Code"
        };
        
        /// <summary>Sign</summary>
        public readonly Types.Sign Sign = new Types.Sign
        {
        	Name = "Sign"
        };
        
        /// <summary>Manual Input Allowed</summary>
        public readonly Types.Alpha1 ManualInputAllowed = new Types.Alpha1
        {
        	Caption = "Manual Input Allowed",
        	Name = "Manual_Input_Allowed",
        	InputRange = "Y,N"
        };
        
        /// <summary>Summary Code A</summary>
        public readonly Types.SummaryCode SummaryCodeA = new Types.SummaryCode
        {
        	Caption = "Summary Code A",
        	Name = "Summary_Code_A"
        };
        
        /// <summary>Summary Code B</summary>
        public readonly Types.SummaryCode SummaryCodeB = new Types.SummaryCode
        {
        	Caption = "Summary Code B",
        	Name = "Summary_Code_B"
        };
        
        /// <summary>Summary Code C</summary>
        public readonly Types.SummaryCode SummaryCodeC = new Types.SummaryCode
        {
        	Caption = "Summary Code C",
        	Name = "Summary_Code_C"
        };
        
        /// <summary>Summary Code D</summary>
        public readonly Types.SummaryCode SummaryCodeD = new Types.SummaryCode
        {
        	Caption = "Summary Code D",
        	Name = "Summary_Code_D"
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
        
        /// <summary>ManInp Branch</summary>
        public readonly Types.ManualInputAllowed ManInpBranch = new Types.ManualInputAllowed
        {
        	Caption = "ManInp Branch",
        	Name = "ManInp_Branch"
        };
        
        /// <summary>ManInp Other Branch</summary>
        public readonly Types.ManualInputAllowed ManInpOtherBranch = new Types.ManualInputAllowed
        {
        	Caption = "ManInp Other Branch",
        	Name = "ManInp_Other_Branch"
        };
        
        /// <summary>ManInp Section Code</summary>
        public readonly Types.ManualInputAllowed ManInpSectionCode = new Types.ManualInputAllowed
        {
        	Caption = "ManInp Section Code",
        	Name = "ManInp_Section_Code"
        };
        
        /// <summary>ManInp Document Number</summary>
        public readonly Types.ManualInputAllowed ManInpDocumentNumber = new Types.ManualInputAllowed
        {
        	Caption = "ManInp Document Number",
        	Name = "ManInp_Document_Number"
        };
        
        /// <summary>ManInp Transaction Date</summary>
        public readonly Types.ManualInputAllowed ManInpTransactionDate = new Types.ManualInputAllowed
        {
        	Caption = "ManInp Transaction Date",
        	Name = "ManInp_Transaction_Date"
        };
        
        /// <summary>ManInp Value</summary>
        public readonly Types.ManualInputAllowed ManInpValue = new Types.ManualInputAllowed
        {
        	Caption = "ManInp Value",
        	Name = "ManInp_Value"
        };
        
        /// <summary>Accumulator</summary>
        public readonly NumberColumn Accumulator = new NumberColumn("Accumulator", "N11.2A");
        
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
        
        /// <summary>BAC_Trans_Codes_X1</summary>
        public readonly Index SortByBAC_Trans_Codes_X1 = new Index
        {
        	Caption = "BAC_Trans_Codes_X1",
        	Name = "BAC_Trans_Codes_X1",
        	AutoCreate = true,
        	Unique = true
        };
        
        /// <summary>BAC_Trans_Codes_X2</summary>
        public readonly Index SortByBAC_Trans_Codes_X2 = new Index
        {
        	Caption = "BAC_Trans_Codes_X2",
        	Name = "BAC_Trans_Codes_X2",
        	AutoCreate = true,
        	Unique = true
        };
        #endregion
        
        
        /// <summary>Transaction Codes(E#10)</summary>
        public TransactionCodes():base("BAC_Trans_Codes", "Transaction Codes", DataSources.BAC)
        {
            Cached = false;
            UseRowIdAsPrimaryKey();
            InitializeIndexes();
        }
        void InitializeIndexes()
        {
            SortByBAC_Trans_Codes_X1.Add(TransactionCode);
            
            SortByBAC_Trans_Codes_X2.Add(TransactionDescription, TransactionCode);
            
        }
        
        
    }
}
