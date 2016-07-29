using Firefly.Box;
using ENV.Data;
namespace MandCo.SalesAndStock.Models
{
    /// <summary>Branch Bays(E#80)</summary>
    public class BranchBays : OracleEntity 
    {
        
        #region Columns
        /// <summary>BRANCH</summary>
        public readonly NumberColumn BRANCH1 = new NumberColumn("BRANCH", "N4");
        /// <summary>DEPARTMENT</summary>
        public readonly TextColumn DEPARTMENT1 = new TextColumn("DEPARTMENT", "2");
        /// <summary>SUBDEPARTMENT</summary>
        public readonly TextColumn SUBDEPARTMENT1 = new TextColumn("SUBDEPARTMENT", "2");
        /// <summary>EFF_DATE</summary>
        public readonly DateColumn EFF_DATE = new DateColumn("EFF_DATE", "DD/MM/YYYY")
        {
        	Storage = new ENV.Data.Storage.DateTimeDateStorage()
        };
        /// <summary>NO_OF_BAYS</summary>
        public readonly NumberColumn NO_OF_BAYS = new NumberColumn("NO_OF_BAYS", "N3.2");
        /// <summary>Floor Code</summary>
        public readonly NumberColumn FloorCode = new NumberColumn("Floor_Code", "N1", "Floor Code");
        /// <summary>LAST_UPD_USER</summary>
        public readonly TextColumn LAST_UPD_USER = new TextColumn("LAST_UPD_USER", "10");
        /// <summary>LAST_UPD_DATE</summary>
        public readonly DateColumn LAST_UPD_DATE = new DateColumn("LAST_UPD_DATE", "DD/MM/YYYY")
        {
        	Storage = new ENV.Data.Storage.DateTimeDateStorage()
        };
        /// <summary>LAST_UPD_TIME</summary>
        public readonly TextColumn LAST_UPD_TIME = new TextColumn("LAST_UPD_TIME", "6");
        #endregion
        
        #region Indexes
        /// <summary>REF_BRANCH_BAYS_X1</summary>
        public readonly Index SortByREF_BRANCH_BAYS_X1 = new Index
        {
        	Caption = "REF_BRANCH_BAYS_X1",
        	Name = "REF_BRANCH_BAYS_X1",
        	AutoCreate = true,
        	Unique = true
        };
        #endregion
        
        public BranchBays():base("mackays.REF_Branch_Bays", "Branch Bays", DataSources.Ref1)
        {
            UseRowIdAsPrimaryKey();
            InitializeIndexes();
        }
        void InitializeIndexes()
        {
            SortByREF_BRANCH_BAYS_X1.Add(BRANCH1, DEPARTMENT1, SUBDEPARTMENT1, EFF_DATE);
            
        }
        
        
    }
}
