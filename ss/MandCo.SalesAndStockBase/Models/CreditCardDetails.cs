using Firefly.Box;
using ENV.Data;
namespace MandCo.SalesAndStock.Models
{
    /// <summary>Credit Card Details(E#201)</summary>
    public class CreditCardDetails : OracleEntity 
    {
        
        #region Columns
        /// <summary>Credit Card Number</summary>
        public readonly TextColumn CreditCardNumber = new TextColumn("Credit_Card_Number", "19A", "Credit Card Number");
        /// <summary>Branch Number</summary>
        public readonly NumberColumn BranchNumber = new NumberColumn("Branch_Number", "4A", "Branch Number")
        {
        	StatusTip = Views.ToolTips.BranchNumberP,
        	CustomHelp = null
        };
        /// <summary>Receipt Number</summary>
        public readonly NumberColumn ReceiptNumber = new NumberColumn("Receipt_Number", "5A", "Receipt Number");
        /// <summary>Transaction Date</summary>
        public readonly Types.Date1 TransactionDate = new Types.Date1
        {
        	Caption = "Transaction Date",
        	Name = "Transaction_Date",
        	Storage = new ENV.Data.Storage.DateTimeDateStorage()
        };
        /// <summary>Transaction Time</summary>
        public readonly TimeColumn TransactionTime = new TimeColumn("Transaction_Time", "HH:MM", "Transaction Time")
        {
        	Storage = new ENV.Data.Storage.StringTimeStorage()
        };
        /// <summary>Payment Type</summary>
        public readonly NumberColumn PaymentType = new NumberColumn("Payment_Type", "2A", "Payment Type");
        /// <summary>Transaction Status</summary>
        public readonly NumberColumn TransactionStatus = new NumberColumn("Transaction_Status", "2A", "Transaction Status");
        /// <summary>Transaction Code</summary>
        public readonly NumberColumn TransactionCode = new NumberColumn("Transaction_Code", "2A", "Transaction Code");
        /// <summary>Transaction Value</summary>
        public readonly NumberColumn TransactionValue = new NumberColumn("Transaction_Value", "N9.2A", "Transaction Value");
        /// <summary>Authorisation Code</summary>
        public readonly TextColumn AuthorisationCode = new TextColumn("Authorisation_Code", "8A", "Authorisation Code");
        /// <summary>Concession Code</summary>
        public readonly NumberColumn ConcessionCode1 = new NumberColumn("Concession_Code", "2A", "Concession Code")
        {
        	StatusTip = Views.ToolTips.ConcessionCodeP,
        	CustomHelp = null
        };
        /// <summary>Card Start Date</summary>
        public readonly NumberColumn CardStartDate = new NumberColumn("Card_Start_Date", "4P0A", "Card Start Date");
        /// <summary>Card Expiry</summary>
        public readonly NumberColumn CardExpiry = new NumberColumn("Card_Expiry", "4P0A", "Card Expiry");
        /// <summary>Source Code</summary>
        public readonly NumberColumn SourceCode = new NumberColumn("Source_Code", "1A", "Source Code");
        /// <summary>Manager No</summary>
        public readonly NumberColumn ManagerNo = new NumberColumn("Manager_No", "4A", "Manager No");
        /// <summary>Assistant No</summary>
        public readonly NumberColumn AssistantNo = new NumberColumn("Assistant_No", "4A", "Assistant No");
        /// <summary>Data Correct Usr Id</summary>
        public readonly TextColumn DataCorrectUsrId = new TextColumn("Data_Correct_Usr_Id", "4A", "Data Correct Usr Id");
        /// <summary>Credit Card Company</summary>
        public readonly NumberColumn CreditCardCompany1 = new NumberColumn("Credit_Card_Company", "2A", "Credit Card Company")
        {
        	StatusTip = Views.ToolTips.CreditCardCompanyP,
        	CustomHelp = null
        };
        /// <summary>StallUnstall Date</summary>
        public readonly Types.Date1 StallUnstallDate = new Types.Date1
        {
        	Caption = "StallUnstall Date",
        	Name = "StallUnstall_Date",
        	AllowNull = true,
        	DefaultValue = null,
        	Storage = new ENV.Data.Storage.DateTimeDateStorage()
        };
        /// <summary>Transaction Week No</summary>
        public readonly NumberColumn TransactionWeekNo = new NumberColumn("Transaction_Week_No", "4P0A", "Transaction Week No")
        {
        	StatusTip = Views.ToolTips.WeekNumberP,
        	CustomHelp = null
        };
        /// <summary>Batch Number</summary>
        public readonly NumberColumn BatchNumber = new NumberColumn("Batch_Number", "6A", "Batch Number");
        /// <summary>Card Issue Number</summary>
        public readonly NumberColumn CardIssueNumber = new NumberColumn("Card_Issue_Number", "2A", "Card Issue Number");
        /// <summary>CrDb Indicator</summary>
        public readonly TextColumn CrDbIndicator = new TextColumn("CrDb_Indicator", "UA", "CrDb Indicator")
        {
        	InputRange = "C,D"
        };
        /// <summary>Application Id</summary>
        public readonly TextColumn ApplicationId = new TextColumn("Application_Id", "UUUX9A", "Application Id");
        /// <summary>Instant Credit</summary>
        public readonly TextColumn InstantCredit = new TextColumn("Instant_Credit", "XA", "Instant Credit");
        /// <summary>Seq No</summary>
        public readonly NumberColumn SeqNo = new NumberColumn("Seq_No", "6A", "Seq No");
        /// <summary>Reference</summary>
        public readonly TextColumn Reference = new TextColumn("Reference", "X20A")
        {
        	AllowNull = true
        };
        #endregion
        
        #region Indexes
        /// <summary>CCC_Detail_X1</summary>
        public readonly Index SortByCCC_Detail_X1 = new Index
        {
        	Caption = "CCC_Detail_X1",
        	Name = "CCC_Detail_X1",
        	AutoCreate = true,
        	Unique = true
        };
        /// <summary>CCC_Detail_X2</summary>
        public readonly Index SortByCCC_Detail_X2 = new Index
        {
        	Caption = "CCC_Detail_X2",
        	Name = "CCC_Detail_X2",
        	AutoCreate = true,
        	Unique = true
        };
        /// <summary>CCC_Detail_X3</summary>
        public readonly Index SortByCCC_Detail_X3 = new Index
        {
        	Caption = "CCC_Detail_X3",
        	Name = "CCC_Detail_X3",
        	AutoCreate = true,
        	Unique = true
        };
        /// <summary>CCC_Detail_X4</summary>
        public readonly Index SortByCCC_Detail_X4 = new Index
        {
        	Caption = "CCC_Detail_X4",
        	Name = "CCC_Detail_X4",
        	AutoCreate = true,
        	Unique = true
        };
        #endregion
        
        public CreditCardDetails():base("CCC_Detail", "Credit Card Details", DataSources.CCC)
        {
            Cached = false;
            BranchNumber.ClearExpandEvent();
            ConcessionCode1.Expand += () => Batches.RollUPIntkStkByMDTypSs036.Create().Run((Text)null /* parameters mismatch in calling this task */);
            CreditCardCompany1.Expand += () => Batches.CorrectEOW_MD_TYPESsFIX1.Create().Run(CreditCardCompany1.GetNumberParameterAcordingToActivity());
            TransactionWeekNo.Expand += () => Batches.EOWDispCheckPtTableSs028.Create().Run();
            ApplicationId.Expand += () => Browses.BrowseRemoveFromTill.Create().Run();
            UseRowIdAsPrimaryKey();
            InitializeIndexes();
        }
        void InitializeIndexes()
        {
            SortByCCC_Detail_X1.Add(CreditCardNumber, BranchNumber, ReceiptNumber, TransactionDate, TransactionTime, PaymentType, TransactionStatus, SeqNo);
            
            SortByCCC_Detail_X2.Add(ApplicationId, CreditCardNumber, BranchNumber, ReceiptNumber, SeqNo);
            
            SortByCCC_Detail_X3.Add(CreditCardNumber, ReceiptNumber, TransactionDate, TransactionTime, PaymentType, TransactionStatus, BranchNumber, SeqNo);
            
            SortByCCC_Detail_X4.Add(BranchNumber, ReceiptNumber, CreditCardNumber, TransactionDate, TransactionTime, PaymentType, TransactionStatus, SeqNo);
            
        }
        
        
    }
}
