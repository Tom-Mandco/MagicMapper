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
    
    /// <summary>SumCodesExt(E#34)</summary>
    public class SumCodesExt : OracleEntity 
    {
        
        #region Columns
        
        /// <summary>Branch Number</summary>
        public readonly Types.BranchNumber BranchNumber = new Types.BranchNumber
        {
        	Name = "Branch_Number"
        };
        
        /// <summary>Summary Code</summary>
        public readonly Types.SummaryCode SummaryCode = new Types.SummaryCode
        {
        	Name = "Summary_Code"
        };
        
        /// <summary>BFW Value</summary>
        public readonly NumberColumn BFWValue = new NumberColumn("BFW_Value", "N14.2", "BFW Value");
        
        /// <summary>BFW Count</summary>
        public readonly NumberColumn BFWCount = new NumberColumn("BFW_Count", "N9", "BFW Count");
        
        /// <summary>TWK Value</summary>
        public readonly NumberColumn TWKValue = new NumberColumn("TWK_Value", "N14.2", "TWK Value");
        
        /// <summary>TWK Count</summary>
        public readonly NumberColumn TWKCount = new NumberColumn("TWK_Count", "N9", "TWK Count");
        #endregion
        
        #region Indexes
        
        /// <summary>BAC_Summ_Codes_ExtA_X1</summary>
        public readonly Index SortByBAC_Summ_Codes_ExtA_X1 = new Index
        {
        	Caption = "BAC_Summ_Codes_ExtA_X1",
        	Name = "BAC_Summ_Codes_ExtA_X1",
        	AutoCreate = true,
        	Unique = true
        };
        #endregion
        
        
        /// <summary>SumCodesExt(E#34)</summary>
        public SumCodesExt():base("BAC_Summ_Codes_ExtA", "SumCodesExt", DataSources.BAC)
        {
            UseRowIdAsPrimaryKey();
            InitializeIndexes();
        }
        void InitializeIndexes()
        {
            SortByBAC_Summ_Codes_ExtA_X1.Add(BranchNumber, SummaryCode);
            
        }
        
        
    }
}
