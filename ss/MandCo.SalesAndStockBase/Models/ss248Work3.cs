using Firefly.Box;
using ENV.Data;
namespace MandCo.SalesAndStock.Models
{
    /// <summary>ss248 Work3(E#212)</summary>
    public class ss248Work3 : Entity 
    {
        
        #region Columns
        /// <summary>Report Group</summary>
        [PrimaryKey]
        public readonly TextColumn ReportGroup = new TextColumn("Report Group", "U")
        {
        	AllowNull = false
        };
        /// <summary>Report Department</summary>
        [PrimaryKey]
        public readonly TextColumn ReportDepartment = new TextColumn("Report Department", "U2")
        {
        	AllowNull = false
        };
        /// <summary>Report SubDept</summary>
        [PrimaryKey]
        public readonly TextColumn ReportSubDept = new TextColumn("Report SubDept", "U2")
        {
        	AllowNull = false
        };
        /// <summary>Percent Indicator</summary>
        [PrimaryKey]
        public readonly TextColumn PercentIndicator = new TextColumn("Percent Indicator", "U")
        {
        	AllowNull = false
        };
        /// <summary>Department Desc</summary>
        public readonly TextColumn DepartmentDesc = new TextColumn("Department Desc", "12")
        {
        	AllowNull = false
        };
        /// <summary>Sat Act</summary>
        public readonly NumberColumn SatAct = new NumberColumn("Sat Act", "N6.2")
        {
        	AllowNull = false
        };
        /// <summary>Sat LY</summary>
        public readonly NumberColumn SatLY = new NumberColumn("Sat LY", "N6.2")
        {
        	AllowNull = false
        };
        /// <summary>Sun Act</summary>
        public readonly NumberColumn SunAct = new NumberColumn("Sun Act", "N6.2")
        {
        	AllowNull = false
        };
        /// <summary>Sun LY</summary>
        public readonly NumberColumn SunLY = new NumberColumn("Sun LY", "N6.2")
        {
        	AllowNull = false
        };
        /// <summary>Mon Act</summary>
        public readonly NumberColumn MonAct = new NumberColumn("Mon Act", "N6.2")
        {
        	AllowNull = false
        };
        /// <summary>Mon LY</summary>
        public readonly NumberColumn MonLY = new NumberColumn("Mon LY", "N6.2")
        {
        	AllowNull = false
        };
        /// <summary>Tue Act</summary>
        public readonly NumberColumn TueAct = new NumberColumn("Tue Act", "N6.2")
        {
        	AllowNull = false
        };
        /// <summary>Tue LY</summary>
        public readonly NumberColumn TueLY = new NumberColumn("Tue LY", "N6.2")
        {
        	AllowNull = false
        };
        /// <summary>Wed Act</summary>
        public readonly NumberColumn WedAct = new NumberColumn("Wed Act", "N6.2")
        {
        	AllowNull = false
        };
        /// <summary>Wed LY</summary>
        public readonly NumberColumn WedLY = new NumberColumn("Wed LY", "N6.2")
        {
        	AllowNull = false
        };
        /// <summary>Thu Act</summary>
        public readonly NumberColumn ThuAct = new NumberColumn("Thu Act", "N6.2")
        {
        	AllowNull = false
        };
        /// <summary>Thu LY</summary>
        public readonly NumberColumn ThuLY = new NumberColumn("Thu LY", "N6.2")
        {
        	AllowNull = false
        };
        /// <summary>Fri Act</summary>
        public readonly NumberColumn FriAct = new NumberColumn("Fri Act", "N6.2")
        {
        	AllowNull = false
        };
        /// <summary>Fri LY</summary>
        public readonly NumberColumn FriLY = new NumberColumn("Fri LY", "N6.2")
        {
        	AllowNull = false
        };
        /// <summary>Week Act</summary>
        public readonly NumberColumn WeekAct = new NumberColumn("Week Act", "N6.2")
        {
        	AllowNull = false
        };
        /// <summary>Week LY</summary>
        public readonly NumberColumn WeekLY = new NumberColumn("Week LY", "N6.2")
        {
        	AllowNull = false
        };
        #endregion
        
        #region Indexes
        /// <summary>ss_ss248_Work3_X1</summary>
        public readonly Index SortByss_ss248_Work3_X1 = new Index
        {
        	Caption = "ss_ss248_Work3_X1",
        	Name = "ss_ss248_Work3_X1",
        	AutoCreate = true,
        	Unique = true
        };
        #endregion
        
        public ss248Work3():base("ss_ss248_Work3", "ss248 Work3", DataSources.Memory)
        {
            InitializeIndexes();
        }
        void InitializeIndexes()
        {
            SortByss_ss248_Work3_X1.Add(ReportGroup, ReportDepartment, ReportSubDept, PercentIndicator);
            
        }
        
        
    }
}
