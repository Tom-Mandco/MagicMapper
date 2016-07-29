using Firefly.Box;
using ENV.Data;
namespace MandCo.SalesAndStock.Models
{
    /// <summary>Section Conversion(E#11)</summary>
    public class SectionConversion : OracleEntity 
    {
        
        #region Columns
        /// <summary>Bull Section</summary>
        public readonly Types.BullSection BullSection = new Types.BullSection
        {
        	Name = "Bull_Section"
        };
        /// <summary>Department Code</summary>
        public readonly Types.DepartmentCode DepartmentCode = new Types.DepartmentCode
        {
        	Name = "Department_Code"
        };
        /// <summary>Section Code</summary>
        public readonly Types.SectionCode SectionCode = new Types.SectionCode
        {
        	Name = "Section_Code"
        };
        /// <summary>DSS Department</summary>
        public readonly Types.DSSDepartmentCode DSSDepartment = new Types.DSSDepartmentCode
        {
        	Caption = "DSS Department",
        	Name = "DSS_Department"
        };
        /// <summary>DSS SubDepartment</summary>
        public readonly Types.DSSSubDepartmentCode DSSSubDepartment = new Types.DSSSubDepartmentCode
        {
        	Caption = "DSS SubDepartment",
        	Name = "DSS_SubDepartment"
        };
        /// <summary>DSS Section</summary>
        public readonly Types.DSSSectionCode DSSSection = new Types.DSSSectionCode
        {
        	Caption = "DSS Section",
        	Name = "DSS_Section"
        };
        #endregion
        
        #region Indexes
        /// <summary>REF_SectConv_X1</summary>
        public readonly Index SortByREF_SectConv_X1 = new Index
        {
        	Caption = "REF_SectConv_X1",
        	Name = "REF_SectConv_X1",
        	AutoCreate = true,
        	Unique = true
        };
        /// <summary>REF_SectConv_X2</summary>
        public readonly Index SortByREF_SectConv_X2 = new Index
        {
        	Caption = "REF_SectConv_X2",
        	Name = "REF_SectConv_X2",
        	AutoCreate = true,
        	Unique = true
        };
        /// <summary>REF_SectConv_X3</summary>
        public readonly Index SortByREF_SectConv_X3 = new Index
        {
        	Caption = "REF_SectConv_X3",
        	Name = "REF_SectConv_X3",
        	AutoCreate = true,
        	Unique = true
        };
        #endregion
        
        public SectionConversion():base("REF_SectConv", "Section Conversion", DataSources.Ref1)
        {
            Cached = false;
            UseRowIdAsPrimaryKey();
            InitializeIndexes();
        }
        void InitializeIndexes()
        {
            SortByREF_SectConv_X1.Add(BullSection);
            
            SortByREF_SectConv_X2.Add(DepartmentCode, SectionCode);
            
            SortByREF_SectConv_X3.Add(DSSDepartment, DSSSubDepartment, DSSSection);
            
        }
        
        
    }
}
