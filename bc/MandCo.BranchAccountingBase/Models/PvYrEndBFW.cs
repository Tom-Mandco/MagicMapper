﻿#region Copyright Firefly Ltd 2014
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
    
    /// <summary>PvYrEndBFW(E#35)</summary>
    public class PvYrEndBFW : OracleEntity 
    {
        
        #region Columns
        
        /// <summary>Branch Number</summary>
        public readonly Types.BranchNumber BranchNumber = new Types.BranchNumber
        {
        	Name = "Branch_Number"
        };
        
        /// <summary>Goods Value</summary>
        public readonly NumberColumn GoodsValue = new NumberColumn("Goods_Value", "N9.2", "Goods Value");
        
        /// <summary>Voucher Value</summary>
        public readonly NumberColumn VoucherValue = new NumberColumn("Voucher_Value", "N9.2", "Voucher Value");
        #endregion
        
        #region Indexes
        
        /// <summary>BAC_PvYrEnd_BFW_X1</summary>
        public readonly Index SortByBAC_PvYrEnd_BFW_X1 = new Index
        {
        	Caption = "BAC_PvYrEnd_BFW_X1",
        	Name = "BAC_PvYrEnd_BFW_X1",
        	AutoCreate = true,
        	Unique = true
        };
        #endregion
        
        
        /// <summary>PvYrEndBFW(E#35)</summary>
        public PvYrEndBFW():base("BAC_PvYrEnd_BFW", "PvYrEndBFW", DataSources.BAC)
        {
            UseRowIdAsPrimaryKey();
            InitializeIndexes();
        }
        void InitializeIndexes()
        {
            SortByBAC_PvYrEnd_BFW_X1.Add(BranchNumber);
            
        }
        
        
    }
}
