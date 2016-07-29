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
    
    /// <summary>TransDetailsExtractA(E#32)</summary>
    public class TransDetailsExtractA : OracleEntity 
    {
        
        #region Columns
        
        /// <summary>Branch Number</summary>
        public readonly Types.BranchNumber BranchNumber = new Types.BranchNumber
        {
        	Name = "Branch_Number"
        };
        
        /// <summary>Other Branch Number</summary>
        public readonly Types.BranchNumber OtherBranchNumber = new Types.BranchNumber
        {
        	Caption = "Other Branch Number",
        	Name = "Other_Branch_Number"
        };
        
        /// <summary>Department Code</summary>
        public readonly Types.DepartmentCode DepartmentCode = new Types.DepartmentCode
        {
        	Name = "Department_Code"
        };
        
        /// <summary>Section Code</summary>
        public readonly Types.SectionCode SectionCode = new Types.SectionCode
        {
        	Name = "Section_Code"
        };
        
        /// <summary>Transaction Code</summary>
        public readonly Types.TransactionCode TransactionCode = new Types.TransactionCode
        {
        	Name = "Transaction_Code"
        };
        
        /// <summary>Document</summary>
        public readonly Types.Document Document = new Types.Document
        {
        	Name = "Document"
        };
        
        /// <summary>Value</summary>
        public readonly NumberColumn Value = new NumberColumn("Value", "N9.2A");
        
        /// <summary>Transaction Date</summary>
        public readonly Types.Date1 TransactionDate = new Types.Date1
        {
        	Caption = "Transaction Date",
        	Name = "Transaction_Date",
        	AllowNull = true,
        	DefaultValue = null,
        	Storage = new ENV.Data.Storage.DateTimeDateStorage()
        };
        
        /// <summary>Run Number</summary>
        public readonly Types.RunNumber RunNumber = new Types.RunNumber
        {
        	Name = "Run_Number"
        };
        
        /// <summary>Source</summary>
        public readonly TextColumn Source = new TextColumn("Source", "UUUA");
        
        /// <summary>Creation Date</summary>
        public readonly Types.Date1 CreationDate = new Types.Date1
        {
        	Caption = "Creation Date",
        	Name = "Creation_Date",
        	AllowNull = true,
        	DefaultValue = null,
        	Storage = new ENV.Data.Storage.DateTimeDateStorage()
        };
        
        /// <summary>Creation Time</summary>
        public readonly Types.Time1 CreationTime = new Types.Time1
        {
        	Caption = "Creation Time",
        	Name = "Creation_Time",
        	Storage = new ENV.Data.Storage.StringTimeStorage()
        };
        
        /// <summary>UniqueId</summary>
        public readonly TextColumn UniqueId = new TextColumn("UniqueId", "18");
        #endregion
        
        #region Indexes
        
        /// <summary>BAC_Trans_Details_ExtA_X1</summary>
        public readonly Index SortByBAC_Trans_Details_ExtA_X1 = new Index
        {
        	Caption = "BAC_Trans_Details_ExtA_X1",
        	Name = "BAC_Trans_Details_ExtA_X1",
        	AutoCreate = true,
        	Unique = true
        };
        #endregion
        
        
        /// <summary>TransDetailsExtractA(E#32)</summary>
        public TransDetailsExtractA():base("BAC_Trans_Details_ExtA", "TransDetailsExtractA", DataSources.BAC)
        {
            UseRowIdAsPrimaryKey();
            InitializeIndexes();
        }
        void InitializeIndexes()
        {
            SortByBAC_Trans_Details_ExtA_X1.Add(BranchNumber, TransactionCode, Document, DepartmentCode, SectionCode, CreationDate, CreationTime, UniqueId);
            
        }
        
        
    }
}
