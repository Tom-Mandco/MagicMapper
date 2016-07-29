using Firefly.Box;
using ENV.Data;
namespace MandCo.SalesAndStock.Models
{
    /// <summary>Concession Code(E#15)</summary>
    public class ConcessionCode : OracleEntity 
    {
        
        #region Columns
        /// <summary>Concession Code</summary>
        public readonly Types.ConcessionCode ConcessionCode1 = new Types.ConcessionCode
        {
        	Name = "Concession_Code"
        };
        /// <summary>Concession Name</summary>
        public readonly Types.Alpha20 ConcessionName = new Types.Alpha20
        {
        	Caption = "Concession Name",
        	Name = "Concession_Name"
        };
        /// <summary>Mackays pcent Take</summary>
        public readonly Types.MackaysPcentTake MackaysPcentTake = new Types.MackaysPcentTake
        {
        	Name = "Mackays_pcent_Take"
        };
        /// <summary>Mackays pcent Take2</summary>
        public readonly Types.MackaysPcentTake MackaysPcentTake2 = new Types.MackaysPcentTake
        {
        	Caption = "Mackays pcent Take2",
        	Name = "Mackays_pcent_Take2"
        };
        /// <summary>Commision Details</summary>
        public readonly TextColumn CommisionDetails = new TextColumn("Commision_Details", "U", "Commision Details");
        #endregion
        
        #region Indexes
        /// <summary>REF_Concess_X1</summary>
        public readonly Index SortByREF_Concess_X1 = new Index
        {
        	Caption = "REF_Concess_X1",
        	Name = "REF_Concess_X1",
        	AutoCreate = true,
        	Unique = true
        };
        #endregion
        
        public ConcessionCode():base("REF_Concess", "Concession Code", DataSources.Ref1)
        {
            Cached = false;
            UseRowIdAsPrimaryKey();
            InitializeIndexes();
        }
        void InitializeIndexes()
        {
            SortByREF_Concess_X1.Add(ConcessionCode1);
            
        }
        
        
    }
}
