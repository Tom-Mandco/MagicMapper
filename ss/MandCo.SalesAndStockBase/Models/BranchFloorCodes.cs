using Firefly.Box;
using ENV.Data;
namespace MandCo.SalesAndStock.Models
{
    /// <summary>Branch Floor Codes(E#81)</summary>
    public class BranchFloorCodes : OracleEntity 
    {
        
        #region Columns
        /// <summary>FLOOR_CODE</summary>
        public readonly NumberColumn FLOOR_CODE = new NumberColumn("FLOOR_CODE", "N1");
        /// <summary>FLOOR_NAME</summary>
        public readonly TextColumn FLOOR_NAME = new TextColumn("FLOOR_NAME", "20");
        /// <summary>LAST_UPDATE_USER</summary>
        public readonly TextColumn LAST_UPDATE_USER = new TextColumn("LAST_UPDATE_USER", "10");
        /// <summary>LAST_UPD_DATETIME</summary>
        public readonly TextColumn LAST_UPD_DATETIME = new TextColumn("LAST_UPD_DATETIME", "19");
        #endregion
        
        #region Indexes
        /// <summary>REF_BR_FLOOR_CODES_X1</summary>
        public readonly Index SortByREF_BR_FLOOR_CODES_X1 = new Index
        {
        	Caption = "REF_BR_FLOOR_CODES_X1",
        	Name = "REF_BR_FLOOR_CODES_X1",
        	AutoCreate = true,
        	Unique = true
        };
        #endregion
        
        public BranchFloorCodes():base("mackays.REF_Br_Floor_Codes", "Branch Floor Codes", DataSources.Ref1)
        {
            UseRowIdAsPrimaryKey();
            InitializeIndexes();
        }
        void InitializeIndexes()
        {
            SortByREF_BR_FLOOR_CODES_X1.Add(FLOOR_CODE);
            
        }
        
        
    }
}
