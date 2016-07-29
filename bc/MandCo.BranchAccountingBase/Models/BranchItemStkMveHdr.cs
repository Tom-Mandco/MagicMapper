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
    
    /// <summary>Branch Item Stk Mve Hdr(E#41)</summary>
    public class BranchItemStkMveHdr : OracleEntity 
    {
        
        #region Columns
        
        /// <summary>SEND_BR_NUM</summary>
        public readonly NumberColumn SEND_BR_NUM = new NumberColumn("SEND_BR_NUM", "N4");
        
        /// <summary>STK_MVE_REF</summary>
        public readonly TextColumn STK_MVE_REF = new TextColumn("STK_MVE_REF", "8");
        
        /// <summary>STK_MVE_TYPE</summary>
        public readonly TextColumn STK_MVE_TYPE = new TextColumn("STK_MVE_TYPE", "4");
        
        /// <summary>SENT_DATETIME</summary>
        public readonly TextColumn SENT_DATETIME = new TextColumn("SENT_DATETIME", "19")
        {
        	AllowNull = true,
        	DefaultValue = null,
        	Storage = new ENV.Data.Storage.DateTimeTextStorage()
        };
        
        /// <summary>SENT_MGR_NUM</summary>
        public readonly NumberColumn SENT_MGR_NUM = new NumberColumn("SENT_MGR_NUM", "N6");
        
        /// <summary>SENT_EMP_NUM</summary>
        public readonly NumberColumn SENT_EMP_NUM = new NumberColumn("SENT_EMP_NUM", "N6");
        
        /// <summary>SENT_TO_BR_NUM</summary>
        public readonly NumberColumn SENT_TO_BR_NUM = new NumberColumn("SENT_TO_BR_NUM", "N4");
        
        /// <summary>TRANSPORT_CODE</summary>
        public readonly NumberColumn TRANSPORT_CODE = new NumberColumn("TRANSPORT_CODE", "N2");
        
        /// <summary>STATUS_CODE</summary>
        public readonly NumberColumn STATUS_CODE = new NumberColumn("STATUS_CODE", "N2");
        
        /// <summary>RCV_BR_NUM</summary>
        public readonly NumberColumn RCV_BR_NUM = new NumberColumn("RCV_BR_NUM", "N4");
        
        /// <summary>RCV_RCPT_NUM</summary>
        public readonly NumberColumn RCV_RCPT_NUM = new NumberColumn("RCV_RCPT_NUM", "N6");
        
        /// <summary>RCV_DATETIME</summary>
        public readonly TextColumn RCV_DATETIME = new TextColumn("RCV_DATETIME", "19")
        {
        	AllowNull = true,
        	DefaultValue = null,
        	Storage = new ENV.Data.Storage.DateTimeTextStorage()
        };
        
        /// <summary>RCV_MGR_NUM</summary>
        public readonly NumberColumn RCV_MGR_NUM = new NumberColumn("RCV_MGR_NUM", "N6");
        
        /// <summary>SENT_PROCESSED</summary>
        public readonly TextColumn SENT_PROCESSED = new TextColumn("SENT_PROCESSED", "1");
        
        /// <summary>RCV_PROCESSED</summary>
        public readonly TextColumn RCV_PROCESSED = new TextColumn("RCV_PROCESSED", "1");
        
        /// <summary>BAC_SENT_POST</summary>
        public readonly TextColumn BAC_SENT_POST = new TextColumn("BAC_SENT_POST", "1");
        
        /// <summary>BAC_RCV_POST</summary>
        public readonly TextColumn BAC_RCV_POST = new TextColumn("BAC_RCV_POST", "1");
        
        /// <summary>SOURCE_CHANNEL</summary>
        public readonly TextColumn SOURCE_CHANNEL = new TextColumn("SOURCE_CHANNEL", "2")
        {
        	AllowNull = false
        };
        
        /// <summary>ORDER_NUMBER</summary>
        public readonly TextColumn ORDER_NUMBER = new TextColumn("ORDER_NUMBER", "20")
        {
        	AllowNull = false
        };
        #endregion
        
        #region Indexes
        
        /// <summary>SS_BR_ITEM_STK_MVE_HDR_X1</summary>
        public readonly Index SortBySS_BR_ITEM_STK_MVE_HDR_X1 = new Index
        {
        	Caption = "SS_BR_ITEM_STK_MVE_HDR_X1",
        	Name = "SS_BR_ITEM_STK_MVE_HDR_X1",
        	AutoCreate = true,
        	Unique = true
        };
        
        /// <summary>SS_BR_ITEM_STK_MVE_HDR_X2</summary>
        public readonly Index SortBySS_BR_ITEM_STK_MVE_HDR_X2 = new Index
        {
        	Caption = "SS_BR_ITEM_STK_MVE_HDR_X2",
        	Name = "SS_BR_ITEM_STK_MVE_HDR_X2",
        	AutoCreate = true,
        	Unique = true
        };
        #endregion
        
        
        /// <summary>Branch Item Stk Mve Hdr(E#41)</summary>
        public BranchItemStkMveHdr():base("mackays.SS_Br_Item_Stk_Mve_Hdr", "Branch Item Stk Mve Hdr", DataSources.SS)
        {
            UseRowIdAsPrimaryKey();
            InitializeIndexes();
        }
        void InitializeIndexes()
        {
            SortBySS_BR_ITEM_STK_MVE_HDR_X1.Add(SEND_BR_NUM, STK_MVE_REF, STK_MVE_TYPE);
            
            SortBySS_BR_ITEM_STK_MVE_HDR_X2.Add(SEND_BR_NUM, STK_MVE_TYPE, STK_MVE_REF);
            
        }
        
        
    }
}
