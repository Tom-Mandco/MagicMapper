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
    
    /// <summary>Company(E#6)</summary>
    public class Company : OracleEntity 
    {
        
        #region Columns
        
        /// <summary>Company Number</summary>
        public readonly Types.CompanyNumber CompanyNumber = new Types.CompanyNumber
        {
        	Name = "Company_Number"
        };
        
        /// <summary>Company Name</summary>
        public readonly Types.Alpha30 CompanyName = new Types.Alpha30
        {
        	Caption = "Company Name",
        	Name = "Company_Name"
        };
        
        /// <summary>Company Name Abbv</summary>
        public readonly Types.Alpha10 CompanyNameAbbv = new Types.Alpha10
        {
        	Caption = "Company Name Abbv",
        	Name = "Company_Name_Abbv"
        };
        
        /// <summary>Company Status</summary>
        public readonly Types.CompanyStatus CompanyStatus = new Types.CompanyStatus
        {
        	Name = "Company_Status"
        };
        
        /// <summary>Company Effect Date</summary>
        public readonly Types.WeekNumber CompanyEffectDate = new Types.WeekNumber
        {
        	Caption = "Company Effect Date",
        	Name = "Company_Effect_Date"
        };
        
        /// <summary>Nom Ledg Post Comp</summary>
        public readonly Types.CompanyNumber NomLedgPostComp = new Types.CompanyNumber
        {
        	Caption = "Nom Ledg Post Comp",
        	Name = "Nom_Ledg_Post_Comp"
        };
        
        /// <summary>Stk Belong Post Comp</summary>
        public readonly Types.CompanyNumber StkBelongPostComp = new Types.CompanyNumber
        {
        	Caption = "Stk Belong Post Comp",
        	Name = "Stk_Belong_Post_Comp"
        };
        #endregion
        
        #region Indexes
        
        /// <summary>REF_Company_X1</summary>
        public readonly Index SortByREF_Company_X1 = new Index
        {
        	Caption = "REF_Company_X1",
        	Name = "REF_Company_X1",
        	AutoCreate = true,
        	Unique = true
        };
        #endregion
        
        
        /// <summary>Company(E#6)</summary>
        public Company():base("REF_Company", "Company", DataSources.Ref1)
        {
            Cached = false;
            UseRowIdAsPrimaryKey();
            InitializeIndexes();
        }
        void InitializeIndexes()
        {
            SortByREF_Company_X1.Add(CompanyNumber);
            
        }
        
        
    }
}
