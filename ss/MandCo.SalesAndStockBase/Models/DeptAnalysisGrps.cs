using Firefly.Box;
using ENV.Data;
namespace MandCo.SalesAndStock.Models
{
    /// <summary>Dept Analysis Grps(E#82)</summary>
    public class DeptAnalysisGrps : OracleEntity 
    {
        
        #region Columns
        /// <summary>DEPARTMENT_CODE</summary>
        public readonly TextColumn DEPARTMENT_CODE = new TextColumn("DEPARTMENT_CODE", "2");
        /// <summary>SUBDEPARTMENT_CODE</summary>
        public readonly TextColumn SUBDEPARTMENT_CODE = new TextColumn("SUBDEPARTMENT_CODE", "2");
        /// <summary>GROUP_1_DEPT</summary>
        public readonly TextColumn GROUP_1_DEPT = new TextColumn("GROUP_1_DEPT", "2");
        /// <summary>GROUP_1_SUBDEPARTMENT</summary>
        public readonly TextColumn GROUP_1_SUBDEPARTMENT = new TextColumn("GROUP_1_SUBDEPARTMENT", "2");
        #endregion
        
        #region Indexes
        /// <summary>REF_DPT_ANALYSIS_GRPS_X1</summary>
        public readonly Index SortByREF_DPT_ANALYSIS_GRPS_X1 = new Index
        {
        	Caption = "REF_DPT_ANALYSIS_GRPS_X1",
        	Name = "REF_DPT_ANALYSIS_GRPS_X1",
        	AutoCreate = true,
        	Unique = true
        };
        /// <summary>REF_DPT_ANALYSIS_GRPS_X2</summary>
        public readonly Index SortByREF_DPT_ANALYSIS_GRPS_X2 = new Index
        {
        	Caption = "REF_DPT_ANALYSIS_GRPS_X2",
        	Name = "REF_DPT_ANALYSIS_GRPS_X2",
        	AutoCreate = true,
        	Unique = true
        };
        #endregion
        
        public DeptAnalysisGrps():base("mackays.REF_Dpt_Analysis_Grps", "Dept Analysis Grps", DataSources.Ref1)
        {
            UseRowIdAsPrimaryKey();
            InitializeIndexes();
        }
        void InitializeIndexes()
        {
            SortByREF_DPT_ANALYSIS_GRPS_X1.Add(DEPARTMENT_CODE, SUBDEPARTMENT_CODE);
            
            SortByREF_DPT_ANALYSIS_GRPS_X2.Add(GROUP_1_DEPT, GROUP_1_SUBDEPARTMENT, DEPARTMENT_CODE, SUBDEPARTMENT_CODE);
            
        }
        
        
    }
}
