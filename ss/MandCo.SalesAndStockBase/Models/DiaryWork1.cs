using Firefly.Box;
using ENV.Data;
namespace MandCo.SalesAndStock.Models
{
    /// <summary>Diary Work1(E#216)</summary>
    public class DiaryWork1 : OracleEntity 
    {
        
        #region Columns
        /// <summary>Prog No</summary>
        public readonly TextColumn ProgNo = new TextColumn("Prog_No", "10A", "Prog No");
        /// <summary>Branch Number</summary>
        public readonly NumberColumn BranchNumber = new NumberColumn("Branch_Number", "4A", "Branch Number");
        /// <summary>Area Code</summary>
        public readonly TextColumn AreaCode = new TextColumn("Area_Code", "U2A", "Area Code");
        /// <summary>Comp Store</summary>
        public readonly TextColumn CompStore = new TextColumn("Comp_Store", "UA", "Comp Store");
        #endregion
        
        #region Indexes
        /// <summary>SS_Diary_Work1_X1</summary>
        public readonly Index SortBySS_Diary_Work1_X1 = new Index
        {
        	Caption = "SS_Diary_Work1_X1",
        	Name = "SS_Diary_Work1_X1",
        	AutoCreate = true,
        	Unique = true
        };
        #endregion
        
        public DiaryWork1():base("SS_Diary_Work1", "Diary Work1", DataSources.SS)
        {
            UseRowIdAsPrimaryKey();
            InitializeIndexes();
        }
        void InitializeIndexes()
        {
            SortBySS_Diary_Work1_X1.Add(ProgNo, BranchNumber);
            
        }
        
        
    }
}
