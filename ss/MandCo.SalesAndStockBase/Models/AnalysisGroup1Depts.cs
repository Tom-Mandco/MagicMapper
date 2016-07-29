using Firefly.Box;
using ENV.Data;
namespace MandCo.SalesAndStock.Models
{
    /// <summary>Analysis Group 1 Depts(E#83)</summary>
    public class AnalysisGroup1Depts : OracleEntity 
    {
        
        #region Columns
        /// <summary>GROUP1_DEPT</summary>
        public readonly TextColumn GROUP1_DEPT = new TextColumn("GROUP1_DEPT", "2");
        /// <summary>GROUP1_SUBDEPT</summary>
        public readonly TextColumn GROUP1_SUBDEPT = new TextColumn("GROUP1_SUBDEPT", "2");
        /// <summary>GROUP1_NAME</summary>
        public readonly TextColumn GROUP1_NAME = new TextColumn("GROUP1_NAME", "20");
        #endregion
        
        #region Indexes
        /// <summary>REF_ANAL_GRP1_DPTS_X1</summary>
        public readonly Index SortByREF_ANAL_GRP1_DPTS_X1 = new Index
        {
        	Caption = "REF_ANAL_GRP1_DPTS_X1",
        	Name = "REF_ANAL_GRP1_DPTS_X1",
        	AutoCreate = true,
        	Unique = true
        };
        #endregion
        
        public AnalysisGroup1Depts():base("mackays.REF_Anal_Grp1_Dpts", "Analysis Group 1 Depts", DataSources.Ref1)
        {
            UseRowIdAsPrimaryKey();
            InitializeIndexes();
        }
        void InitializeIndexes()
        {
            SortByREF_ANAL_GRP1_DPTS_X1.Add(GROUP1_DEPT, GROUP1_SUBDEPT);
            
        }
        
        
    }
}
