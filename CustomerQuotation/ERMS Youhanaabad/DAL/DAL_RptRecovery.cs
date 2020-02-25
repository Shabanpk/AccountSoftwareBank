using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using BusinessEntities;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Configuration;
using Infragistics.Win.UltraWinGrid;

namespace DAL
{
    public class DAL_RptRecovery
    {
        string ConnString = DataAccess.ConnString;
        //string ConnString = "Data Source=HADI-B94B6CB176;Initial Catalog=RashanGhar;User id=sa;password=iloveyoumuhammad786";
        //string ConnString = @"Data Source=192.168.3.90;Initial Catalog=RashanGharLatest;user id=sa;password=123;";
        //string ConnString = "Data Source=FAHAD;Initial Catalog=RashanGharLatest;User id=sa;password=123456";

        private DataTable GetdataTable(SqlCommand sqlcomm, SqlConnection sqlconn)
        {
            sqlcomm.Connection = sqlconn;
            SqlDataAdapter da = new SqlDataAdapter(sqlcomm);
            DataTable dt = new DataTable();
            da.Fill(dt);
            return dt;
        }

        private DataTable GetDataTable(string qry, SqlConnection sqlconn)
        {
            sqlconn.ConnectionString = ConnString;
            SqlDataAdapter da = new SqlDataAdapter(qry, sqlconn);
            DataTable dt = new DataTable();
            da.Fill(dt);
            return dt;
        }

        public DataTable GetDailyActivity(string CurrentDate)
        {
            DataTable dt = new DataTable();
            try
            {
                SqlConnection sqlconn = new SqlConnection(ConnString);
                SqlCommand sqlcomm = new SqlCommand();
                //string Qry = "Select count(*) AS [ExpiryDateofClients], " +
                //             " IsNull((Select count(*) AS [DueDateofClients]From Trans_IssueM Where Convert(varchar(11),DueDate,121)='"+ CurrentDate +"'),0)AS [DueDateofClients], " +
                //             " IsNull((Select count(*) AS [DueDateofClients]From Trans_IssueM Where Convert(varchar(11),IssueDate,121)='" + CurrentDate + "'),0)As [RashanIssuedClients], " +
                //             " IsNull((Select count(*) As [RegisteredClients]From Trans_Clients Where Convert(varchar(11),RegistrationDate,121)='"+ CurrentDate +"'),0) AS [RegisteredClients], " +
                //             " IsNull((Select Count(*) AS [Recovery]From Trans_recovery where Convert(varchar(11),RecoveryDate,121)='" + CurrentDate + "'),0) As [Recovery], " +
                //             " IsNUll((Select Count(*) From Trans_Clients Where ClientStatus=1 And Convert(varchar(11),RegistrationDate,121)='" + CurrentDate + "'),0) As [NewClients], " +
                //             " IsNUll((Select Count(*) From Trans_Clients Where ClientStatus=4 And Convert(varchar(11),RegistrationDate,121)='" + CurrentDate + "'),0) As [DisbursedClients], " +
                //             " IsNUll((Select Count(*) From Trans_Clients Where ClientStatus=3 And Convert(varchar(11),RegistrationDate,121)='" + CurrentDate + "'),0) As [PartiallyRecoveredClients], " +
                //             " IsNUll((Select Count(*) From Trans_Clients Where ClientStatus=2 And Convert(varchar(11),RegistrationDate,121)='" + CurrentDate + "'),0) As [NillRecovered], " +
                //             " IsNUll((Select IsnUll(sum(totalAmount),0) AS [TotalSale] From Trans_IssueM IM Inner JOIn Trans_IssueD ID on IM.IssueID=ID.IssueID Where Convert(varchar(11),IssueDate,121) ='" + CurrentDate + "'),0) As [TotalSale], " +
                //             " Isnull((Select Isnull(sum(TotalAmount),0) As [RecoveryAmount] From Trans_Recovery where Convert(varchar(11),RecoveryDate,121)='2013-01-02'),0) AS [TotalRecoveryAmount]," +
                //             " IsNull((Select IsNUll(sum(Convert(int,Registration_fee)),0)As[RegistrationFee] From Trans_Clients where Convert(varchar(11),RegistrationDate,121)='" + CurrentDate + "'),0) AS [TotalRegistrationFee], " +
                //             " IsNull((Select IsNUll(sum(RegistrationFee),0) From Trans_Recovery Where Convert(varchar(11),RecoveryDate,121) ='" + CurrentDate + "'),0) As [Re.Registered]  " +
                //             " From Trans_IssueM Where Convert(varchar(11),ExpiryDate,121)='" + CurrentDate + "'";
//                string Qry = @"Select count(*) AS [ExpiryDateofClients],  
//                               IsNull((Select count(*) AS [DueDateofClients]From Trans_IssueM Where Convert(varchar(11),DueDate,121)='"+CurrentDate +@"'),0)AS [DueDateofClients],
//                               IsNull((Select IsNull(Sum(totalAmount),0) AS [TotalDueAmount] From Trans_IssueM IM
//                               Inner Join trans_IssueD ID on IM.IssueID=ID.IssueID
//                               Where Convert(varchar(11),DueDate,121)='" + CurrentDate + @"'
//                               ),0) AS [TotalDueAmountToday],
//                               IsNull((Select count(*) AS [DueDateofClients]From Trans_IssueM Where Convert(varchar(11),IssueDate,121)='" + CurrentDate + @"'),0)As [RashanIssuedClients],
//                               IsNull((Select count(*) As [RegisteredClients]From Trans_Clients Where Convert(varchar(11),RegistrationDate,121)='" + CurrentDate + @"'),0) AS [RegisteredClients],
//                               IsNull((Select Count(*) AS [Recovery]From Trans_recovery where Convert(varchar(11),RecoveryDate,121)='" + CurrentDate + @"'),0) As [Recovery],
//                               IsNull((Select Count(*) From Trans_Clients ),0) AS [TotalClients],
//                               IsNUll((Select Count(*) From Trans_Clients Where ClientStatus=1 And Convert(varchar(11),RegistrationDate,121)='" + CurrentDate + @"'),0) As [FreshClients],  
//                               IsNUll((Select count(*) From Trans_recovery REC Inner Join Trans_Clients Cl on REC.ClientID=Cl.ClientID Where ClientStatus=4 and Convert(varchar(11),RecoveryDate,121)='"+CurrentDate +@"'),0) As [FullyPaid],
//                               IsNUll((Select Count(*) From Trans_Clients Where ClientStatus=4),0) As [FullyPaidAll],  
//                               IsNUll((Select IsnUll(sum(totalAmount),0) AS [TotalSale] From Trans_IssueM IM Inner JOIn Trans_IssueD ID on IM.IssueID=ID.IssueID Where Convert(varchar(11),IssueDate,121) ='" + CurrentDate + @"'),0) As [TotalSale],  
//                               Isnull((Select Isnull(sum(RecoveryAmount),0) As [RecoveryAmount] From Trans_Recovery where Convert(varchar(11),RecoveryDate,121)='" + CurrentDate + @"'),0) AS [TotalRecoveryAmount],
//                               IsNull((Select IsNUll(sum(Convert(int,Registration_fee)),0)As[RegistrationFee] From Trans_Clients where Convert(varchar(11),RegistrationDate,121)='" + CurrentDate + @"'),0) AS [TotalRegistrationFee],  
//                               IsNULL((Select IsnULL(sum(regamount),0) AS [Re.Registration] From REgistration Where Convert(varchar(11),RegDate,121) ='"+@CurrentDate+ @"'),0)As [Re.Registered],
//                               IsnULL((Select count(*)From Trans_PurchaseM where Convert(varchar(11),PrDate,121)='" + CurrentDate+ @"'),0) AS [TotalPurchases],
//                               ISNULL((Select isnull(sum(PurchasePrice),0)As [PurchasePrice] From Trans_PurchaseD PD Inner Join Trans_PurchaseM PM on PD.PRID=PM.PRID Where Convert(varchar(11),PRDate,121)='"+CurrentDate +@"'),0) as [TotalPurchasePrice],
//                               IsNULL((Select IsnULL(sum(qty),0) as [Qty] From OpeningBalanceM OM
//                               Inner Join OpeningBalanceD OD on OM.OpeningBalanceID=OD.OpeningBalanceID
//                               Where Convert(varchar(11),OpeningDate,121)='"+CurrentDate+ @"'),0) as [StockInserted],
//                               IsNULL((Select count(*) As [TotalDirectClients] From Trans_DRSM DRM
//                               Inner Join Trans_DRSD DRD on DRM.DRS_ID=DRD.DRS_ID
//                               Where DRS_Date='"+CurrentDate+ @"'),0) As [TotalDirectClients],
//                               IsNULL((Select IsNULL(sum(salesPrice*Qty),0) as [DirectSales] From Trans_DRSM DRM
//                               Inner Join Trans_DRSD DRD on DRM.DRS_ID=DRD.DRS_ID
//                               Where DRS_Date='" + CurrentDate + @"'),0) As [TotalDirectSales]
//                               From Trans_IssueM 
//                               Where Convert(varchar(11),ExpiryDate,121)='" + CurrentDate + @"'";
                string Qry = @"Select count(*) AS [ExpiryDateofClients],  
                               IsNull((Select count(*) AS [DueDateofClients]From Trans_IssueM Where Convert(varchar(11),DueDate,121)='" + CurrentDate + @"'),0)AS [DueDateofClients],
                               IsNull((Select IsNull(Sum(totalAmount),0) AS [TotalDueAmount] From Trans_IssueM IM
                               Inner Join trans_IssueD ID on IM.IssueID=ID.IssueID
                               Where Convert(varchar(11),DueDate,121)='" + CurrentDate + @"'
                               ),0) AS [TotalDueAmountToday],
                               IsNull((Select count(*) AS [DueDateofClients]From Trans_IssueM Where Convert(varchar(11),IssueDate,121)='" + CurrentDate + @"'),0)As [RashanIssuedClients],
                               IsNull((Select count(*) As [RegisteredClients]From Trans_Clients Where Convert(varchar(11),RegistrationDate,121)='" + CurrentDate + @"'),0) AS [RegisteredClients],
                               IsNull((Select Count(*) AS [Recovery]From Trans_recovery where Convert(varchar(11),RecoveryDate,121)='" + CurrentDate + @"'),0) As [Recovery],
                               IsNull((Select Count(*) From Trans_Clients ),0) AS [TotalClients],
                               IsNUll((Select Count(*) From Trans_Clients Where ClientStatus=1 And Convert(varchar(11),RegistrationDate,121)='" + CurrentDate + @"'),0) As [FreshClients],  
                               IsNUll((Select count(*) From Trans_recovery REC Inner Join Trans_Clients Cl on REC.ClientID=Cl.ClientID Where ClientStatus=4 and Convert(varchar(11),RecoveryDate,121)='" + CurrentDate + @"'),0) As [FullyPaid],
                               IsNUll((Select Count(*) From Trans_Clients Where ClientStatus=4),0) As [FullyPaidAll],  
                               IsNUll((Select IsnUll(sum(totalAmount),0) AS [TotalSale] From Trans_IssueM IM Inner JOIn Trans_IssueD ID on IM.IssueID=ID.IssueID Where Convert(varchar(11),IssueDate,121) ='" + CurrentDate + @"'),0) As [TotalSale],  
                               Isnull((Select Isnull(sum(RecoveryAmount),0) As [RecoveryAmount] From Trans_Recovery where Convert(varchar(11),RecoveryDate,121)='" + CurrentDate + @"'),0) AS [TotalRecoveryAmount],
                               IsNull((Select IsNUll(sum(Convert(int,Registration_fee)),0)As[RegistrationFee] From Trans_Clients where Convert(varchar(11),RegistrationDate,121)='" + CurrentDate + @"'),0) AS [TotalRegistrationFee],  
                               IsNULL((Select IsnULL(sum(regamount),0) AS [Re.Registration] From Registration Where Convert(varchar(11),RegDate,121) ='" + @CurrentDate + @"'),0)As [Re.Registered],
                               IsnULL((Select count(*)From Trans_PurchaseM where Convert(varchar(11),PrDate,121)='" + CurrentDate + @"'),0) AS [TotalPurchases],
                               ISNULL((Select ISNULL(sum(cr),0) As [KnockOffPurchase] From CashBalance Where CONVERT(varchar(11),cashindate,121)='" + CurrentDate + @"' and TableName='KnockOffPurchase'),0) AS [KnockoffPurchase],
                               IsNULL((Select ISNULL(sum(PurchasePrice),0) As [PurchasePrice] From (
                               Select (PrQty*PurchasePrice)As [PurchasePrice] From Trans_PurchaseD PD 
                               Inner Join Trans_PurchaseM PM on PD.PRID=PM.PRID 
                               Where Convert(varchar(11),PRDate,121)='" + CurrentDate+ @"')AA),0) AS [TotalPurchasePrice] ,
                               IsNULL((Select IsnULL(sum(qty),0) as [Qty] From OpeningBalanceM OM
                               Inner Join OpeningBalanceD OD on OM.OpeningBalanceID=OD.OpeningBalanceID
                               Where Convert(varchar(11),OpeningDate,121)='" + CurrentDate + @"'),0) as [StockInserted],
                               IsNULL((Select count(*) As [TotalDirectClients] From Trans_DRSM DRM
                               Inner Join Trans_DRSD DRD on DRM.DRS_ID=DRD.DRS_ID
                               Where DRS_Date='" + CurrentDate + @"'),0) As [TotalDirectClients],
                               IsNULL((Select IsNULL(sum(salesPrice*Qty),0) as [DirectSales] From Trans_DRSM DRM
                               Inner Join Trans_DRSD DRD on DRM.DRS_ID=DRD.DRS_ID
                               Where DRS_Date='" + CurrentDate + @"'),0) As [TotalDirectSales]
                               From Trans_IssueM 
                               Where Convert(varchar(11),ExpiryDate,121)='" + CurrentDate + @"'";
                //sqlcomm.CommandText = "RptDailyActivity";
                sqlcomm.CommandText = Qry;
                sqlcomm.Connection = sqlconn;
                //sqlcomm.CommandType = CommandType.StoredProcedure;
                dt = GetdataTable(sqlcomm, sqlconn);
                sqlconn.Close();
                sqlcomm.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, DataAccess.ProjectName, MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            return dt;
        }

        public DataTable GetClientIssuenceReport(string param)
        {
            DataTable dt = new DataTable();
            try
            {
                string Concatination = "";
                string sqlQry = "";
                param.Trim(' ');
                Concatination = param;
                SqlConnection sqlconn = new SqlConnection(ConnString);
                SqlCommand sqlcomm = new SqlCommand();
                sqlQry = " Select MemberCode,dbo.InItCap(ClientName) As [ClientName],dbo.InItCap(HusbandName) AS [HusbandName],Area, " +
                          "  CellNo,dbo.InItCap(NearBy) As [NearBy],DisbAmount aS [Disb Amount.],Relation, " +
                            " DateOfDisb As [Date Of Disb.],DueDate, " +
                            " dbo.InItCap(EmpName) AS [EmpName] From ( " +
                            " Select MemberCode,ClientName,HusbandName,Area,CellNo,Nearby,TotalAmount As [DisbAmount], " +
                            " (Case Relation when '1' then 'W/O' when '3' then 'D/O' when '4' then 'S/O' when '9' then 'DIA/O'  " +
                             " when '10' then 'MIA/O' when '13' then 'C/O' else '' end) AS [Relation], " +
                             " DisDate As [DateofDisb],DueDate, " +
                             " EmpName From ( " +
                             " Select ClientCode As [MemberCode],ClientName,HusbandName,AreaDescription AS [Area],CellNo,NearBy, " +
                             " Sum(TotalAmount) AS [TotalAmount],Convert(varchar,RelationToClient) As [Relation], " +
                             " Convert(varchar,IssueDate,106) As [DisDate] ,Convert(varchar,DueDate,106) As [DueDate],EmpName " +
                             " From trans_Clients Cl " +
                             " Inner JOIn Trans_IssueM IM on Cl.ClientID=Im.ClientID " +
                             " Inner Join Trans_IssueD ID on Im.IssueID=ID.IssueID " +
                             " Inner Join Emp_Def Emp on Cl.EmpCode=Emp.EmpCode " +
                             " Inner JOin Areas_Def Area On Cl.AreaID=Area.AreaID " +
                             " Inner JOIn Relation_def Ref on Cl.RelationToClient=Ref.RelationID " +
                             " Where 1=1  " + Concatination + "" +
                             " Group  by ClientCode ,ClientName,HusbandName,RelationToClient,AreaDescription,CellNo,NearBy,IssueDate, " +
                             " DueDate,EmpName)AA) ABC";
                sqlcomm.CommandText = sqlQry;
                dt = GetdataTable(sqlcomm, sqlconn);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, DataAccess.ProjectName, MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            return dt;
        }

        public DataTable GetStockBalance()
        {
            DataTable dt = new DataTable();

            try
            {
                SqlConnection sqlconn = new SqlConnection(ConnString);

                string qry = @"Select St.ItemID,ItemName,ItemNameUrdu,MUnitName,IsNull(sum(dr),0) AS [Dr],IsNull(sum(cr),0) As [Cr],ItemPrice,itm.SalePrice,IsNull(sum(dr)-sum(cr),0) As [Stock]
                               From Stock st
                               Inner Join Items itm on Itm.ItemID=st.Itemid
                               Inner join Munit Mu on Itm.MunitID=Mu.MunitID
                               Group By st.Itemid,ItemName,ItemNameUrdu,MUnitName,ItemPrice,itm.ItemID,SalePrice
                               Order by ItemID,ItemName";

                SqlCommand sqlcomm = new SqlCommand(qry, sqlconn);
                sqlcomm.CommandType = CommandType.Text;
                dt = GetdataTable(sqlcomm, sqlconn);
                sqlconn.Close();
                sqlcomm.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, DataAccess.ProjectName, MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            return dt;
        }

        public DataTable ShowPurchaseItems(string FromDate,string ToDate)
        {
            DataTable dt = new DataTable();
            try
            {
                SqlConnection sqlconn = new SqlConnection(ConnString);
                string qry = @"Select Convert(varchar(12),ModifiedDate,106) as PurchaseDate, VendorName,
                               PD.ItemID,PrDate,ItemName,ItemNameUrdu,PD.MunitID,MunitName,PrQty,ItemPrice,SalePrice From Trans_PurchaseM PM
                               Inner JOin Trans_PurchaseD PD on Pm.PrID=Pd.PrID
                               Inner JOin Items ITMS On Pd.ItemID=Itms.ItemID
                               Inner Join Munit Mu On PD.MunitID=Mu.MunitID
                               Where Convert(varchar(11),ModifiedDate,121) between @FromDate and @ToDate
                               order by PrDate Asc";
                SqlCommand sqlcomm = new SqlCommand(qry, sqlconn);
                sqlcomm.CommandType = CommandType.Text;
                //sqlcomm.Parameters.AddWithValue("@PrID", PrID);
                sqlcomm.Parameters.AddWithValue("@FromDate", FromDate);
                sqlcomm.Parameters.AddWithValue("@ToDate", ToDate);

                dt = GetdataTable(sqlcomm, sqlconn);
                sqlconn.Close();
                sqlcomm.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, DataAccess.ProjectName, MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            return dt;
        }

        public DataTable ShowPurchasePerItems(string FromDate, string ToDate,int ItemID)
        {
            DataTable dt = new DataTable();
            try
            {
                SqlConnection sqlconn = new SqlConnection(ConnString);
                string qry = @"Select Convert(varchar(12),ModifiedDate,106) as PurchaseDate, VendorName,
                               PD.ItemID,PrDate,ItemName,ItemNameUrdu,PD.MunitID,MunitName,PrQty,ItemPrice,SalePrice From Trans_PurchaseM PM
                               Inner JOin Trans_PurchaseD PD on Pm.PrID=Pd.PrID
                               Inner JOin Items ITMS On Pd.ItemID=Itms.ItemID
                               Inner Join Munit Mu On PD.MunitID=Mu.MunitID
                               Where Convert(varchar(11),ModifiedDate,121) between @FromDate and @ToDate and ITMS.ItemID= '" + ItemID + "' order by PrDate Asc";
                SqlCommand sqlcomm = new SqlCommand(qry, sqlconn);
                sqlcomm.CommandType = CommandType.Text;
                //sqlcomm.Parameters.AddWithValue("@PrID", PrID);
                sqlcomm.Parameters.AddWithValue("@FromDate", FromDate);
                sqlcomm.Parameters.AddWithValue("@ToDate", ToDate);

                dt = GetdataTable(sqlcomm, sqlconn);
                sqlconn.Close();
                sqlcomm.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, DataAccess.ProjectName, MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            return dt;
        }

        public DataTable GetVendorName(int PrID)
        {
            DataTable dt = new DataTable();
            try
            {
                SqlConnection sqlconn = new SqlConnection(ConnString);
                SqlCommand sqlcomm = new SqlCommand("GetVendorName", sqlconn);
                sqlcomm.CommandType = CommandType.StoredProcedure;
                sqlcomm.Parameters.AddWithValue("@PrID", PrID);
                dt = GetdataTable(sqlcomm, sqlconn);
                sqlconn.Close();
                sqlcomm.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, DataAccess.ProjectName, MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            return dt;
        }

        public DataTable GetAreaName(int AreaID)
        {
            DataTable dt = new DataTable();
            try
            {
                SqlConnection sqlconn = new SqlConnection(ConnString);
                SqlCommand sqlcomm = new SqlCommand("Select AreaDescription From Areas_Def Where AreaID='"+ AreaID +"'", sqlconn);
                dt = GetdataTable(sqlcomm, sqlconn);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, DataAccess.ProjectName, MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            return dt;
        }

        public DataTable GetEmpName(int EmpID)
        {
            DataTable dt = new DataTable();
            try
            {
                SqlConnection sqlconn = new SqlConnection(ConnString);
                SqlCommand sqlcomm = new SqlCommand("Select EmpName From Emp_Def Where EmpCode='"+EmpID+"'",sqlconn);
                dt = GetdataTable(sqlcomm, sqlconn);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, DataAccess.ProjectName, MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            return dt;
        }

        public DataTable DisbursmentReport(string From,string To,bool IsOnlyDate,string OnlyDate)
        {
            DataTable dt = new DataTable();
            SqlConnection sqlconn = new SqlConnection(ConnString);
            try
            {
                if (IsOnlyDate == true)
                {
                    string SqlQry = @"Select ClientID,MemberCode,dbo.InItCap(MemberName)AS [MemberName],dbo.InItCap(Address)AS [Address],dbo.InItCap(EmpName) AS  [EmpName],
                                    Area,CellNo,dbo.InItCap(NearBy) AS[NearBy],BirthDate,DueDate,Convert(varchar(11),IssueDate,106) AS [IssueDate],AmountDisb From (
                                    SELECT     ClientID, MemberCode, MemberName + ' ' + Relation + ' ' + HusbandName AS MemberName, Address, EmpName, Area, CellNo, NearBy, BirthDate, 
                                    DueDate, IssueDate, AmountDisb
                                    FROM (SELECT     ClientID, MemberCode, MemberName, HusbandName, Address, EmpName, Area, CellNo, NearBy, 
                                    (CASE Relation WHEN '1' THEN 'W/O' WHEN '3' THEN 'D/O' WHEN '4' THEN 'S/O' WHEN '9' THEN 'DIA/O' WHEN '10' THEN 'MIA/O' WHEN '13'
                                    THEN 'C/O' ELSE '' END) AS Relation, BirthDate, DueDate, IssueDate, AmountDisb
                                    FROM (SELECT     Acc.ClientID, Cl.ClientCode AS MemberCode, Cl.HusbandName, Cl.ClientName AS MemberName, Cl.Address, Emp.EmpName, 
                                    Area.AreaDescription AS Area, Cl.CellNO, Cl.NearBy, ISNULL(Cl.RelationToClient, 0) AS Relation, CONVERT(varchar(12), Cl.DOB, 
                                    106) AS BirthDate, CONVERT(varchar(12), Acc.DueDate, 106) AS DueDate, IssueDate 
                                    AS IssueDate, Acc.NetAmount AS AmountDisb
                                    FROM Trans_AccountHistory AS Acc INNER JOIN
                                    Trans_Clients AS Cl ON Acc.ClientID = Cl.ClientID INNER JOIN
                                    Areas_Def AS Area ON Cl.AreaID = Area.AreaID INNER JOIN
                                    Emp_Def AS Emp ON Cl.EmpCode = Emp.EmpCode LEFT OUTER JOIN
                                    Relation_Def AS Rel ON Cl.RelationToClient = Rel.RelationID
                                    WHERE (1 = 1) And Convert(varchar(11),IssueDate,121) = '"+ OnlyDate + @"'
                                    GROUP BY Acc.ClientID, Acc.DueDate, Cl.HusbandName, Acc.NetAmount ,Acc.IssueDate, Cl.ClientCode, Cl.DOB, Area.AreaDescription, Cl.CellNO, 
                                    Cl.RelationToClient, Cl.NearBy, cl.ClientName, Cl.Address, Emp.EmpName) AS AA
                                    WHERE (AmountDisb > 0)) AS AAA)ABC
                                    Order by IssueDate Asc";
                    SqlCommand sqlcomm = new SqlCommand(SqlQry, sqlconn);
                    dt = GetdataTable(sqlcomm, sqlconn);
                }
                else
                {
                    string SqlQry = @"Select ClientID,MemberCode,dbo.InItCap(MemberName)AS [MemberName],dbo.InItCap(Address)AS [Address],dbo.InItCap(EmpName) AS  [EmpName],
                                    Area,CellNo,dbo.InItCap(NearBy) AS[NearBy],BirthDate,DueDate,Convert(varchar(11),IssueDate,106) AS [IssueDate],AmountDisb From (
                                    SELECT     ClientID, MemberCode, MemberName + ' ' + Relation + ' ' + HusbandName AS MemberName, Address, EmpName, Area, CellNo, NearBy, BirthDate, 
                                    DueDate, IssueDate, AmountDisb
                                    FROM (SELECT     ClientID, MemberCode, MemberName, HusbandName, Address, EmpName, Area, CellNo, NearBy, 
                                    (CASE Relation WHEN '1' THEN 'W/O' WHEN '3' THEN 'D/O' WHEN '4' THEN 'S/O' WHEN '9' THEN 'DIA/O' WHEN '10' THEN 'MIA/O' WHEN '13'
                                    THEN 'C/O' ELSE '' END) AS Relation, BirthDate, DueDate, IssueDate, AmountDisb
                                    FROM (SELECT     Acc.ClientID, Cl.ClientCode AS MemberCode, Cl.HusbandName, Cl.ClientName AS MemberName, Cl.Address, Emp.EmpName, 
                                    Area.AreaDescription AS Area, Cl.CellNO, Cl.NearBy, ISNULL(Cl.RelationToClient, 0) AS Relation, CONVERT(varchar(12), Cl.DOB, 
                                    106) AS BirthDate, CONVERT(varchar(12), Acc.DueDate, 106) AS DueDate, IssueDate 
                                    AS IssueDate, SUM(Acc.NetAmount) - SUM(Acc.Installment) AS AmountDisb
                                    FROM Trans_AccountHistory AS Acc INNER JOIN
                                    Trans_Clients AS Cl ON Acc.ClientID = Cl.ClientID INNER JOIN
                                    Areas_Def AS Area ON Cl.AreaID = Area.AreaID INNER JOIN
                                    Emp_Def AS Emp ON Cl.EmpCode = Emp.EmpCode LEFT OUTER JOIN
                                    Relation_Def AS Rel ON Cl.RelationToClient = Rel.RelationID
                                    WHERE (1 = 1) And Convert(varchar(11),IssueDate,120) between '"+ From + @"' And '"+ To + @"' 
                                    GROUP BY Acc.ClientID, Acc.DueDate, Cl.HusbandName, Acc.IssueDate, Cl.ClientCode, Cl.DOB, Area.AreaDescription, Cl.CellNO, 
                                    Cl.RelationToClient, Cl.NearBy, cl.ClientName, Cl.Address, Emp.EmpName) AS AA
                                    WHERE (AmountDisb > 0)) AS AAA)ABC
                                    Order By IssueDate Asc";
                    SqlCommand sqlcomm = new SqlCommand(SqlQry, sqlconn);
                    dt = GetdataTable(sqlcomm, sqlconn);
 
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, DataAccess.ProjectName, MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            return dt;
        }

        public DataTable AllClientDisbursmentReport()
        {
            DataTable dt = new DataTable();
            try
            {
                SqlConnection sqlconn = new SqlConnection(ConnString);
                string Qry = @"Select ClientID,MemberCode,dbo.InItCap(MemberName)AS [MemberName],dbo.InItCap(Address)AS [Address],dbo.InItCap(EmpName) AS  [EmpName],
                                    Area,CellNo,dbo.InItCap(NearBy) AS[NearBy],BirthDate,DueDate,Convert(varchar(11),IssueDate,106) AS [IssueDate],AmountDisb From (
                                    SELECT ClientID, MemberCode, MemberName + ' ' + Relation + ' ' + HusbandName AS MemberName, Address, EmpName, Area, CellNo, NearBy, BirthDate, 
                                    DueDate, IssueDate, AmountDisb
                                    FROM (SELECT ClientID, MemberCode, MemberName, HusbandName, Address, EmpName, Area, CellNo, NearBy, 
                                    (CASE Relation WHEN '1' THEN 'W/O' WHEN '3' THEN 'D/O' WHEN '4' THEN 'S/O' WHEN '9' THEN 'DIA/O' WHEN '10' THEN 'MIA/O' WHEN '13'
                                    THEN 'C/O' ELSE '' END) AS Relation, BirthDate, DueDate, IssueDate, AmountDisb
                                    FROM (SELECT Acc.ClientID, Cl.ClientCode AS MemberCode, Cl.HusbandName, Cl.ClientName AS MemberName, Cl.Address, Emp.EmpName, 
                                    Area.AreaDescription AS Area, Cl.CellNO, Cl.NearBy, ISNULL(Cl.RelationToClient, 0) AS Relation, CONVERT(varchar(12), Cl.DOB, 
                                    106) AS BirthDate, CONVERT(varchar(12), Acc.DueDate, 106) AS DueDate, IssueDate 
                                    AS IssueDate, SUM(Acc.NetAmount) - SUM(Acc.Installment) AS AmountDisb
                                    FROM Trans_AccountHistory AS Acc INNER JOIN
                                    Trans_Clients AS Cl ON Acc.ClientID = Cl.ClientID INNER JOIN
                                    Areas_Def AS Area ON Cl.AreaID = Area.AreaID INNER JOIN
                                    Emp_Def AS Emp ON Cl.EmpCode = Emp.EmpCode LEFT OUTER JOIN
                                    Relation_Def AS Rel ON Cl.RelationToClient = Rel.RelationID
                                    GROUP BY Acc.ClientID, Acc.DueDate, Cl.HusbandName, Acc.IssueDate, Cl.ClientCode, Cl.DOB, Area.AreaDescription, Cl.CellNO, 
                                    Cl.RelationToClient, Cl.NearBy, cl.ClientName, Cl.Address, Emp.EmpName) AS AA
                                    WHERE (AmountDisb > 0)) AS AAA)ABC
                                    Order By IssueDate Asc"; 
                SqlCommand sqlcomm = new SqlCommand(Qry,sqlconn);
                dt = GetdataTable(sqlcomm, sqlconn);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, DataAccess.ProjectName, MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            return dt;
        }

        public DataTable RptAreaWiseAll(int AreaID,string FromDate,string ToDate)
        {
            DataTable dt = new DataTable();
            try
            {
                if (AreaID == 0)
                {
                    SqlConnection sqlconn = new SqlConnection(ConnString);
                    string Qry = @"Select ClientID,MemberCode,dbo.InItCap(MemberName) AS [MemberName],dbo.InItCap(Address) As [Address],dbo.InitCap(EmpName) AS [EmpName],Area,CellNo,NearBy,Birthdate,DueDate,IssueDate,AmountDisb From (
                                   SELECT     ClientID, MemberCode, MemberName + ' ' + Relation + ' ' + HusbandName AS MemberName, Address, EmpName, Area, CellNo, NearBy, BirthDate, 
                                   DueDate, IssueDate, AmountDisb
                                   FROM         (SELECT     ClientID, MemberCode, MemberName, HusbandName, Address, EmpName, Area, CellNo, NearBy, 
                                   (CASE Relation WHEN '1' THEN 'W/O' WHEN '3' THEN 'D/O' WHEN '4' THEN 'S/O' WHEN '9' THEN 'DIA/O' WHEN '10' THEN 'MIA/O' WHEN '13'
                                   THEN 'C/O' ELSE '' END) AS Relation, BirthDate, DueDate, IssueDate, AmountDisb
                                   FROM          (SELECT     Acc.ClientID, Cl.ClientCode AS MemberCode, Cl.HusbandName, Cl.ClientName AS MemberName, Cl.Address, Emp.EmpName, 
                                   Area.AreaDescription AS Area, Cl.CellNO, Cl.NearBy, ISNULL(Cl.RelationToClient, 0) AS Relation, CONVERT(varchar(12), Cl.DOB, 
                                   106) AS BirthDate, CONVERT(varchar(12), Acc.DueDate, 106) AS DueDate, CONVERT(varchar(12), Acc.IssueDate, 106) 
                                   AS IssueDate, IsnULl(Convert(int,SUM(Acc.NetAmount)),0) - IsNUll(Convert(int,SUM(Acc.Installment)),0) AS AmountDisb
                                   FROM Trans_AccountHistory AS Acc INNER JOIN
                                   Trans_Clients AS Cl ON Acc.ClientID = Cl.ClientID INNER JOIN
                                   Areas_Def AS Area ON Cl.AreaID = Area.AreaID INNER JOIN
                                   Emp_Def AS Emp ON Cl.EmpCode = Emp.EmpCode LEFT OUTER JOIN
                                   Relation_Def AS Rel ON Cl.RelationToClient = Rel.RelationID
                                   WHERE      (1 = 1) And Convert(varchar(11),DueDate,121) between '" + FromDate+@"' And '"+ToDate+@"' 
                                   GROUP BY Acc.ClientID, Acc.DueDate, Cl.HusbandName, Acc.IssueDate, Cl.ClientCode, Cl.DOB, Area.AreaDescription, Cl.CellNO, 
                                   Cl.RelationToClient, Cl.NearBy, cl.ClientName, Cl.Address, Emp.EmpName) AS AA
                                   WHERE      (AmountDisb > 0)) AS AAA) ABC
                                   Order By IssueDate Asc";
                    SqlCommand sqlcomm = new SqlCommand(Qry,sqlconn);
                    dt = GetdataTable(sqlcomm, sqlconn);
                }
                else
                {
                    SqlConnection sqlconn = new SqlConnection(ConnString);
                    string SqlQry = @"Select ClientID,MemberCode,dbo.InItCap(MemberName) AS [MemberName],dbo.InItCap(Address) As [Address],dbo.InitCap(EmpName) AS [EmpName],Area,CellNo,NearBy,Birthdate,DueDate,IssueDate,AmountDisb From (
                                   SELECT     ClientID, MemberCode, MemberName + ' ' + Relation + ' ' + HusbandName AS MemberName, Address, EmpName, Area, CellNo, NearBy, BirthDate, 
                                   DueDate, IssueDate, AmountDisb
                                   FROM         (SELECT     ClientID, MemberCode, MemberName, HusbandName, Address, EmpName, Area, CellNo, NearBy, 
                                   (CASE Relation WHEN '1' THEN 'W/O' WHEN '3' THEN 'D/O' WHEN '4' THEN 'S/O' WHEN '9' THEN 'DIA/O' WHEN '10' THEN 'MIA/O' WHEN '13'
                                   THEN 'C/O' ELSE '' END) AS Relation, BirthDate, DueDate, IssueDate, AmountDisb
                                   FROM          (SELECT     Acc.ClientID, Cl.ClientCode AS MemberCode, Cl.HusbandName, Cl.ClientName AS MemberName, Cl.Address, Emp.EmpName, 
                                   Area.AreaDescription AS Area, Cl.CellNO, Cl.NearBy, ISNULL(Cl.RelationToClient, 0) AS Relation, CONVERT(varchar(12), Cl.DOB, 
                                   106) AS BirthDate, CONVERT(varchar(12), Acc.DueDate, 106) AS DueDate, CONVERT(varchar(12), Acc.IssueDate, 106) 
                                   AS IssueDate, IsnULl(Convert(int,SUM(Acc.NetAmount)),0) - IsNUll(Convert(int,SUM(Acc.Installment)),0)AS AmountDisb
                                   FROM          Trans_AccountHistory AS Acc INNER JOIN
                                   Trans_Clients AS Cl ON Acc.ClientID = Cl.ClientID INNER JOIN
                                   Areas_Def AS Area ON Cl.AreaID = Area.AreaID INNER JOIN
                                   Emp_Def AS Emp ON Cl.EmpCode = Emp.EmpCode LEFT OUTER JOIN
                                   Relation_Def AS Rel ON Cl.RelationToClient = Rel.RelationID
                                   WHERE      (1 = 1) And Area.AreaID='" + AreaID + @"' And Convert(varchar(11),duedate,121) between '" + FromDate + @"' And '" + ToDate + @"'  
                                   GROUP BY Acc.ClientID, Acc.DueDate, Cl.HusbandName, Acc.IssueDate, Cl.ClientCode, Cl.DOB, Area.AreaDescription, Cl.CellNO, 
                                   Cl.RelationToClient, Cl.NearBy, cl.ClientName, Cl.Address, Emp.EmpName) AS AA
                                   WHERE      (AmountDisb > 0)) AS AAA) ABC
                                   Order By IssueDate Asc";
                    SqlCommand sqlcomm = new SqlCommand(SqlQry,sqlconn);
                    dt = GetdataTable(sqlcomm, sqlconn);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, DataAccess.ProjectName, MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            return dt;
        }

        public DataTable RptEmpWiseReport(int EmpID,string FromDate,string ToDate,int AreaID)
        {
            DataTable dt = new DataTable();
            try
            {
                if (EmpID == 0 && AreaID==0)
                {
                    SqlConnection sqlconn = new SqlConnection(ConnString);
                    string Qry = @"Select ClientID,MemberCode,dbo.InItCap(MemberName) AS [MemberName],dbo.InItCap(Address) As [Address],dbo.InitCap(EmpName) AS [EmpName],Area,CellNo,NearBy,Birthdate,DueDate,IssueDate,AmountDisb From (
                                   SELECT     ClientID, MemberCode, MemberName + ' ' + Relation + ' ' + HusbandName AS MemberName, Address, EmpName, Area, CellNo, NearBy, BirthDate, 
                                   DueDate, IssueDate, AmountDisb
                                   FROM         (SELECT     ClientID, MemberCode, MemberName, HusbandName, Address, EmpName, Area, CellNo, NearBy, 
                                   (CASE Relation WHEN '1' THEN 'W/O' WHEN '3' THEN 'D/O' WHEN '4' THEN 'S/O' WHEN '9' THEN 'DIA/O' WHEN '10' THEN 'MIA/O' WHEN '13'
                                   THEN 'C/O' ELSE '' END) AS Relation, BirthDate, DueDate, IssueDate, AmountDisb
                                   FROM          (SELECT     Acc.ClientID, Cl.ClientCode AS MemberCode, Cl.HusbandName, Cl.ClientName AS MemberName, Cl.Address, Emp.EmpName, 
                                   Area.AreaDescription AS Area, Cl.CellNO, Cl.NearBy, ISNULL(Cl.RelationToClient, 0) AS Relation, CONVERT(varchar(12), Cl.DOB, 
                                   106) AS BirthDate, CONVERT(varchar(12), Acc.DueDate, 106) AS DueDate, CONVERT(varchar(12), Acc.IssueDate, 106) 
                                   AS IssueDate, IsnULl(Convert(int,SUM(Acc.NetAmount)),0) - IsNUll(Convert(int,SUM(Acc.Installment)),0) AS AmountDisb
                                   FROM Trans_AccountHistory AS Acc INNER JOIN
                                   Trans_Clients AS Cl ON Acc.ClientID = Cl.ClientID INNER JOIN
                                   Areas_Def AS Area ON Cl.AreaID = Area.AreaID INNER JOIN
                                   Emp_Def AS Emp ON Cl.EmpCode = Emp.EmpCode LEFT OUTER JOIN
                                   Relation_Def AS Rel ON Cl.RelationToClient = Rel.RelationID
                                   WHERE      (1 = 1) And Convert(varchar(11),DueDate,121) between '" + FromDate +@"' And '"+ToDate+ @"'
                                   GROUP BY Acc.ClientID, Acc.DueDate, Cl.HusbandName, Acc.IssueDate, Cl.ClientCode, Cl.DOB, Area.AreaDescription, Cl.CellNO, 
                                   Cl.RelationToClient, Cl.NearBy, cl.ClientName, Cl.Address, Emp.EmpName) AS AA
                                   WHERE (AmountDisb > 0)) AS AAA) ABC
                                   Order By clientID Asc";
                    SqlCommand sqlcomm = new SqlCommand(Qry, sqlconn);
                    dt = GetdataTable(sqlcomm, sqlconn);
                }
                else if (EmpID>0 && AreaID==0)
                {
                    SqlConnection sqlconn = new SqlConnection(ConnString);
                    string Qry = @"Select ClientID,MemberCode,dbo.InItCap(MemberName) AS [MemberName],dbo.InItCap(Address) As [Address],dbo.InitCap(EmpName) AS [EmpName],Area,CellNo,NearBy,Birthdate,DueDate,IssueDate,AmountDisb From (
                                   SELECT ClientID, MemberCode, MemberName + ' ' + Relation + ' ' + HusbandName AS MemberName, Address, EmpName, Area, CellNo, NearBy, BirthDate, 
                                   DueDate, IssueDate, AmountDisb
                                   FROM         (SELECT     ClientID, MemberCode, MemberName, HusbandName, Address, EmpName, Area, CellNo, NearBy, 
                                   (CASE Relation WHEN '1' THEN 'W/O' WHEN '3' THEN 'D/O' WHEN '4' THEN 'S/O' WHEN '9' THEN 'DIA/O' WHEN '10' THEN 'MIA/O' WHEN '13'
                                   THEN 'C/O' ELSE '' END) AS Relation, BirthDate, DueDate, IssueDate, AmountDisb
                                   FROM (SELECT Acc.ClientID, Cl.ClientCode AS MemberCode, Cl.HusbandName, Cl.ClientName AS MemberName, Cl.Address, Emp.EmpName, 
                                   Area.AreaDescription AS Area, Cl.CellNO, Cl.NearBy, ISNULL(Cl.RelationToClient, 0) AS Relation, CONVERT(varchar(12), Cl.DOB, 
                                   106) AS BirthDate, CONVERT(varchar(12), Acc.DueDate, 106) AS DueDate, CONVERT(varchar(12), Acc.IssueDate, 106) 
                                   AS IssueDate, IsnULl(Convert(int,SUM(Acc.NetAmount)),0) - IsNUll(Convert(int,SUM(Acc.Installment)),0) AS AmountDisb
                                   FROM Trans_AccountHistory AS Acc INNER JOIN
                                   Trans_Clients AS Cl ON Acc.ClientID = Cl.ClientID INNER JOIN
                                   Areas_Def AS Area ON Cl.AreaID = Area.AreaID INNER JOIN
                                   Emp_Def AS Emp ON Cl.EmpCode = Emp.EmpCode LEFT OUTER JOIN
                                   Relation_Def AS Rel ON Cl.RelationToClient = Rel.RelationID
                                   WHERE      (1 = 1) And Cl.EmpCode='" + EmpID + @"' And Convert(varchar(11),DueDate,121) between '"+FromDate+ @"' And '"+ToDate+ @"'
                                   GROUP BY Acc.ClientID, Acc.DueDate, Cl.HusbandName, Acc.IssueDate, Cl.ClientCode, Cl.DOB, Area.AreaDescription, Cl.CellNO, 
                                   Cl.RelationToClient, Cl.NearBy, cl.ClientName, Cl.Address, Emp.EmpName) AS AA
                                   WHERE (AmountDisb > 0)) AS AAA) ABC
                                   Order By clientID Asc";
                    SqlCommand sqlcomm = new SqlCommand(Qry, sqlconn);
                    dt = GetdataTable(sqlcomm, sqlconn);
                }
                else if (EmpID > 0 && AreaID > 0)
                {
                    SqlConnection sqlconn = new SqlConnection(ConnString);
                    string Qry = @"Select ClientID,MemberCode,dbo.InItCap(MemberName) AS [MemberName],dbo.InItCap(Address) As [Address],dbo.InitCap(EmpName) AS [EmpName],Area,CellNo,NearBy,Birthdate,DueDate,IssueDate,AmountDisb From (
                                   SELECT ClientID, MemberCode, MemberName + ' ' + Relation + ' ' + HusbandName AS MemberName, Address, EmpName, Area, CellNo, NearBy, BirthDate, 
                                   DueDate, IssueDate, AmountDisb
                                   FROM (SELECT ClientID, MemberCode, MemberName, HusbandName, Address, EmpName, Area, CellNo, NearBy, 
                                   (CASE Relation WHEN '1' THEN 'W/O' WHEN '3' THEN 'D/O' WHEN '4' THEN 'S/O' WHEN '9' THEN 'DIA/O' WHEN '10' THEN 'MIA/O' WHEN '13'
                                   THEN 'C/O' ELSE '' END) AS Relation, BirthDate, DueDate, IssueDate, AmountDisb
                                   FROM (SELECT Acc.ClientID, Cl.ClientCode AS MemberCode, Cl.HusbandName, Cl.ClientName AS MemberName, Cl.Address, Emp.EmpName, 
                                   Area.AreaDescription AS Area, Cl.CellNO, Cl.NearBy, ISNULL(Cl.RelationToClient, 0) AS Relation, CONVERT(varchar(12), Cl.DOB, 
                                   106) AS BirthDate, CONVERT(varchar(12), Acc.DueDate, 106) AS DueDate, CONVERT(varchar(12), Acc.IssueDate, 106) 
                                   AS IssueDate, IsnULl(Convert(int,SUM(Acc.NetAmount)),0) - IsNUll(Convert(int,SUM(Acc.Installment)),0) AS AmountDisb
                                   FROM Trans_AccountHistory AS Acc INNER JOIN
                                   Trans_Clients AS Cl ON Acc.ClientID = Cl.ClientID INNER JOIN
                                   Areas_Def AS Area ON Cl.AreaID = Area.AreaID INNER JOIN
                                   Emp_Def AS Emp ON Cl.EmpCode = Emp.EmpCode LEFT OUTER JOIN
                                   Relation_Def AS Rel ON Cl.RelationToClient = Rel.RelationID
                                   WHERE      (1 = 1) And Cl.EmpCode='" + EmpID + @"' And Convert(varchar(11),DueDate,121) between '"+FromDate+ @"' And '"+ToDate+@"' and Area.AreaID='"+AreaID+ @"'
                                   GROUP BY Acc.ClientID, Acc.DueDate, Cl.HusbandName, Acc.IssueDate, Cl.ClientCode, Cl.DOB, Area.AreaDescription, Cl.CellNO, 
                                   Cl.RelationToClient, Cl.NearBy, cl.ClientName, Cl.Address, Emp.EmpName) AS AA
                                   WHERE (AmountDisb > 0)) AS AAA) ABC
                                   Order By ClientID Asc";
                    SqlCommand sqlcomm = new SqlCommand(Qry, sqlconn);
                    dt = GetdataTable(sqlcomm, sqlconn);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, DataAccess.ProjectName, MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            return dt;
        }

        public DataTable GetClientName(long ClientID)
        {
            DataTable dt = new DataTable();
            try
            {
                SqlConnection sqlconn = new SqlConnection(ConnString);
                SqlCommand sqlcomm = new SqlCommand("Select ClientName From Trans_Clients Where ClientID='"+ClientID+"'", sqlconn);
                dt = GetdataTable(sqlcomm, sqlconn);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, DataAccess.ProjectName, MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            return dt;
        }

        public DataTable GetMemberDisbursmentReport(long ClientID,string DisDate)
        {
            DataTable dt = new DataTable();
            try
            {
                SqlConnection sqlconn = new SqlConnection(ConnString);
                string Qry = @"Select *,isnull((Select isnull(sum(bal),0) bal From (
                               Select IM.IssueID,sum(NetAMount)-sum(Installment) As Bal From trans_issuem IM
                               Inner JOIn Trans_AccountHistory A on Im.IssueID=A.TableID
                               WHere Im.ClientID =BB.ClientID and IssueID !=BB.IssueID
                               Group by IM.IssueID) AA
                               Where Bal>0),0) As [PreviousBalance],EmpName From (
                               Select IssueID,Convert(varchar(12),IssueDate,106)AS [IssueDate],DueDate,NoOfCycles,ExpiryDate,ClientId,MemberName,
                               MemberCode,ItemID,ItemName,ItemPrice,ItemSalePrice,IssueQty,TotalAmount,EmpName From (
                               Select ID.IssueID, IssueDate,Convert(varchar(12),DueDate,106) As [DueDate],A.EmpName,
                               ISNULL((Select COUNT(clientid) AS [ClientID] From Trans_IssueM Where ClientID=iM.ClientID),0) AS [NoOfCycles],
                               Convert(varchar(12),ExpiryDate,106) As [ExpiryDate],Im.ClientID,dbo.InitCap(ClientName) AS [MemberName],ClientCode As [MemberCode],
                               ID.ItemID,ItemNameUrdu AS [ItemName],it.ItemPrice,ItemSalePrice,IssueQty,TotalAmount,IsNull((Select IsNull(sum(RecoveryAmount),0) AS [RecoveryAmount] From Trans_Recovery Rec 
                               Where Rec.IssueID=ID.IssueID And Rec.ClientID=IM.ClientID),0) As [RecoveryAmmount]
                               From Trans_IssueM IM
                               Inner Join Trans_Clients Cl on Im.ClientId=Cl.ClientID
                               Inner Join Trans_IssueD ID on Im.IssueID=ID.IssueID
                               Inner Join Items It On Id.ItemID=It.ItemID
                               Inner Join Emp_Def A on Cl.EmpCode=A.EmpCode
                               Where Im.ClientID='" + ClientID + @"' and Convert(varchar(11),issueDate,120)='" + DisDate + @"'
                               Group By ID.IssueID,IssueDate,DueDate,ExpiryDate,Im.ClientID,ClientName,ClientCode,ID.ItemID,ItemNameUrdu,it.ItemPrice,ItemSalePrice,A.EmpCode,
                               IssueQty,TotalAmount,EmpName) AA)BB
                               Order By IssueDate Asc";
                SqlCommand sqlcomm = new SqlCommand(Qry, sqlconn);
                dt = GetdataTable(sqlcomm, sqlconn);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, DataAccess.ProjectName, MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            return dt;
        }

        public DataTable GetMemberDisbursmentReportEnglish(long IssueID)
        {
            DataTable dt = new DataTable();
            try
            {
                SqlConnection sqlconn = new SqlConnection(ConnString);
                string Qry = @"Select *, isnull((Select isnull(sum(bal),0) bal From (
                               Select IM.IssueID,sum(NetAMount)-sum(Installment) As Bal From trans_issuem IM
                               Inner JOIn Trans_AccountHistory A on Im.IssueID=A.TableID
                               WHere Im.ClientID =BB.ClientID and IssueID!=BB.IssueID
                               Group by IM.IssueID) AA
                               Where Bal>0),0) As [PreviousBalance],Discount,TotalDiscount

 From (
Select IssueID,Convert(varchar(12),IssueDate,106)AS [IssueDate],DueDate,NoOfCycles,ExpiryDate,ClientId,MemberName,
                               MemberCode,ItemID,ItemName,ItemPrice,ItemSalePrice,IssueQty,EmpName,TotalAmount,Discount,TotalDiscount From (
                               Select ID.IssueID, IssueDate,Convert(varchar(12),DueDate,106) As [DueDate],
                               ISNULL((Select COUNT(clientid) AS [ClientID] From Trans_IssueM Where ClientID=iM.ClientID),0) AS [NoOfCycles],
                               Convert(varchar(12),ExpiryDate,106) As [ExpiryDate],Im.ClientID,dbo.InitCap(ClientName) AS [MemberName],ClientCode As [MemberCode],
                               ID.ItemID,ItemName AS [ItemName],it.ItemPrice,ItemSalePrice,EmpName,IssueQty,TotalAmount,IsNull((Select IsNull(sum(RecoveryAmount),0) AS [RecoveryAmount] From Trans_Recovery Rec 
                               Where Rec.IssueID=ID.IssueID And Rec.ClientID=IM.ClientID),0) As [RecoveryAmmount],Isnull(ID.Discount,0) AS [Discount],Isnull(ID.TotalDiscount,0) AS [TotalDiscount]
                               From Trans_IssueM IM
                               Inner Join Trans_Clients Cl on Im.ClientId=Cl.ClientID
                               Inner Join Trans_IssueD ID on Im.IssueID=ID.IssueID
                               Inner Join Emp_Def A on Cl.EmpCode=A.EmpCode
                               Inner Join Items It On Id.ItemID=It.ItemID
                               Where Im.IssueID='" + IssueID+ @"'
                               Group By ID.IssueID,IssueDate,DueDate,ExpiryDate,Im.ClientID,ClientName,EmpName,ClientCode,ID.ItemID,ItemName,it.ItemPrice,ItemSalePrice,ID.Discount,ID.TotalDiscount,
                               IssueQty,TotalAmount) AA)BB
                               Order By IssueDate Asc";
                SqlCommand sqlcomm = new SqlCommand(Qry, sqlconn);
                dt = GetdataTable(sqlcomm, sqlconn);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, DataAccess.ProjectName, MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            return dt;
        }

        public DataTable GetMemberDisbursmentReportUrdu(long IssueID)
        {
            DataTable dt = new DataTable();
            try
            {
                SqlConnection sqlconn = new SqlConnection(ConnString);
                string Qry = @"Select *, isnull((Select isnull(sum(bal),0) bal From (
                               Select IM.IssueID,sum(NetAMount)-sum(Installment) As Bal From trans_issuem IM
                               Inner JOIn Trans_AccountHistory A on Im.IssueID=A.TableID
                               WHere Im.ClientID =BB.ClientID and IssueID!=BB.IssueID
                               Group by IM.IssueID) AA
                               Where Bal>0),0) As [PreviousBalance],0.05 AS [Surcharge]
                               From (
                               Select IssueID,Convert(varchar(12),IssueDate,106)AS [IssueDate],DueDate,NoOfCycles,ExpiryDate,ClientId,MemberName,
                               MemberCode,ItemID,ItemName,ItemPrice,ItemSalePrice,EmpName,IssueQty,TotalAmount,Discount,TotalDiscount From (
                               Select ID.IssueID, IssueDate,Convert(varchar(12),DueDate,106) As [DueDate],
                               ISNULL((Select COUNT(clientid) AS [ClientID] From Trans_IssueM Where ClientID=iM.ClientID),0) AS [NoOfCycles],
                               Convert(varchar(12),ExpiryDate,106) As [ExpiryDate],Im.ClientID,dbo.InitCap(ClientName) AS [MemberName],ClientCode As [MemberCode],
                               ID.ItemID,ItemNameUrdu AS [ItemName],it.ItemPrice,EmpName,ItemSalePrice,IssueQty,TotalAmount,IsNull((Select IsNull(sum(RecoveryAmount),0) AS [RecoveryAmount] From Trans_Recovery Rec 
                               Where Rec.IssueID=ID.IssueID And Rec.ClientID=IM.ClientID),0) As [RecoveryAmmount],Isnull(ID.Discount,0) AS [Discount],isnull(ID.TotalDiscount,0) As [TotalDiscount]
                               From Trans_IssueM IM
                               Inner Join Trans_Clients Cl on Im.ClientId=Cl.ClientID
                               Inner Join Trans_IssueD ID on Im.IssueID=ID.IssueID
                               Inner JOin Emp_Def A on Cl.EmpCode=A.EmpCode
                               Inner Join Items It On Id.ItemID=It.ItemID
                               Where Im.IssueID='" + IssueID + @"'
                               Group By ID.IssueID,IssueDate,DueDate,ExpiryDate,Im.ClientID,ClientName,EmpName,ClientCode,ID.ItemID,ItemNameUrdu,it.ItemPrice,ItemSalePrice,ID.Discount,ID.TotalDiscount,
                               IssueQty,TotalAmount) AA)BB
                               Order By IssueDate Asc";
                SqlCommand sqlcomm = new SqlCommand(Qry, sqlconn);
                dt = GetdataTable(sqlcomm, sqlconn);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, DataAccess.ProjectName, MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            return dt;
        }

        public DataTable RptRecoveryDateWise(string FromDate,string ToDate)
        {
            DataTable dt = new DataTable();
            try
            {
                SqlConnection sqlconn = new SqlConnection(ConnString);
                string Qry = @"SELECT ClientID, MemberCode, dbo.InItCap(MemberName + ' ' + Relation + ' ' + HusbandName) AS MemberName, Address, EmpName, Area, CellNo, 
                               DueDate, IssueDate,RecoveryDate,RecoveryAmount,Convert(nvarchar(25),Remarks) As [Remarks]
                               From (
                               select Rec.ClientID,ClientCode AS [MemberCode], ClientName AS [MemberName], HusbandName,
                               (CASE Relation WHEN '1' THEN 'W/O' WHEN '3' THEN 'D/O' WHEN '4' THEN 'S/O' WHEN '9' THEN 'DIA/O' WHEN '10' THEN 'MIA/O' WHEN '13'
                               THEN 'C/O' ELSE '' END) AS Relation,CellNo,dbo.InItCap(Cl.Address) AS [Address],
                               dbo.InitCap(EmpName) AS [EmpName],dbo.initcap(AreaDescription) AS [Area],Rec.Remarks,
                               Convert(varchar(11),RecoveryDate,106) AS [RecoveryDate],
                               Convert(varchar(11),IssueDate,106) AS [IssueDate],
                               Convert(varchar(11),DueDate,106) AS [DueDate],Sum(RecoveryAmount) AS [RecoveryAmount] From Trans_Recovery Rec
                               Inner Join Trans_Clients Cl on Rec.ClientID=Cl.ClientID
                               Inner JOin Areas_Def Area on Cl.AreaID=Area.AreaID
                               Inner Join Emp_Def Emp on Cl.EmpCode=Emp.EmpCode
                               Where Convert(Varchar(11),RecoveryDate,121) between '" + FromDate +@"' and '"+ ToDate + @"'
                               Group by Rec.ClientID,IssueDate,DueDate,relation,CellNo,Cl.Address,EmpName,Rec.Remarks,RecoveryDate,AreaDescription,ClientCode, ClientName, HusbandName
                               )A Order By Remarks Asc";
                SqlCommand sqlcomm = new SqlCommand(Qry, sqlconn);
                dt = GetdataTable(sqlcomm, sqlconn);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, DataAccess.ProjectName, MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            return dt;
        }

        public DataTable RptRecoveryDateWiseAndClient(string FromDate, string ToDate,long EmpID)
        {
            DataTable dt = new DataTable();
            try
            {
                SqlConnection sqlconn = new SqlConnection(ConnString);
                string Qry = @"SELECT ClientID, MemberCode, dbo.InItCap(MemberName + ' ' + Relation + ' ' + HusbandName) AS MemberName, Address, EmpName, Area, CellNo, 
                               DueDate, IssueDate,RecoveryDate,RecoveryAmount,Convert(nvarchar(25),Remarks) As [Remarks]
                               From (
                               select Rec.ClientID,ClientCode AS [MemberCode], ClientName AS [MemberName], HusbandName,
                               (CASE Relation WHEN '1' THEN 'W/O' WHEN '3' THEN 'D/O' WHEN '4' THEN 'S/O' WHEN '9' THEN 'DIA/O' WHEN '10' THEN 'MIA/O' WHEN '13'
                               THEN 'C/O' ELSE '' END) AS Relation,CellNo,dbo.InItCap(Cl.Address) AS [Address],
                               dbo.InitCap(EmpName) AS [EmpName],dbo.initcap(AreaDescription) AS [Area],
                               Convert(varchar(11),RecoveryDate,106) AS [RecoveryDate],Rec.Remarks,
                               Convert(varchar(11),IssueDate,106) AS [IssueDate],
                               Convert(varchar(11),DueDate,106) AS [DueDate],Sum(RecoveryAmount) AS [RecoveryAmount] From Trans_Recovery Rec
                               Inner Join Trans_Clients Cl on Rec.ClientID=Cl.ClientID
                               Inner JOin Areas_Def Area on Cl.AreaID=Area.AreaID
                               Inner Join Emp_Def Emp on Cl.EmpCode=Emp.EmpCode
                               Where Convert(Varchar(11),RecoveryDate,121) between '" + FromDate + @"' and '" + ToDate + @"' And Emp.EmpCode='"+EmpID+ @"'
                               Group by Rec.ClientID,IssueDate,DueDate,relation,CellNo,Cl.Address,Rec.Remarks,EmpName,RecoveryDate,AreaDescription,ClientCode, ClientName, HusbandName
                               )A Order By Remarks Asc";
                SqlCommand sqlcomm = new SqlCommand(Qry, sqlconn);
                dt = GetdataTable(sqlcomm, sqlconn);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, DataAccess.ProjectName, MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            return dt;
        }

        public DataTable RptRecoveryDateWiseAndClientWiseArea(string FromDate, string ToDate, long EmpID,int AreaID)
        {
            DataTable dt = new DataTable();
            try
            {
                SqlConnection sqlconn = new SqlConnection(ConnString);
                string Qry = @"SELECT ClientID, MemberCode, dbo.InItCap(MemberName + ' ' + Relation + ' ' + HusbandName) AS MemberName, Address, EmpName, Area, CellNo, 
                               DueDate, IssueDate,RecoveryDate,RecoveryAmount,Convert(varchar(25),Remarks) As Remarks
                               From (
                               select Rec.ClientID,ClientCode AS [MemberCode], ClientName AS [MemberName], HusbandName,
                               (CASE Relation WHEN '1' THEN 'W/O' WHEN '3' THEN 'D/O' WHEN '4' THEN 'S/O' WHEN '9' THEN 'DIA/O' WHEN '10' THEN 'MIA/O' WHEN '13'
                               THEN 'C/O' ELSE '' END) AS Relation,CellNo,dbo.InItCap(Cl.Address) AS [Address],
                               dbo.InitCap(EmpName) AS [EmpName],dbo.initcap(AreaDescription) AS [Area],
                               Convert(varchar(11),RecoveryDate,106) AS [RecoveryDate],Rec.Remarks,
                               Convert(varchar(11),IssueDate,106) AS [IssueDate],
                               Convert(varchar(11),DueDate,106) AS [DueDate],Sum(RecoveryAmount) AS [RecoveryAmount] From Trans_Recovery Rec
                               Inner Join Trans_Clients Cl on Rec.ClientID=Cl.ClientID
                               Inner JOin Areas_Def Area on Cl.AreaID=Area.AreaID
                               Inner Join Emp_Def Emp on Cl.EmpCode=Emp.EmpCode
                               Where Convert(Varchar(11),RecoveryDate,121) between '" + FromDate + @"' and '" + ToDate + @"' And Emp.EmpCode='" + EmpID + @"' And Area.AreaID ='" + AreaID + @"'
                               Group by Rec.ClientID,IssueDate,DueDate,relation,CellNo,Cl.Address,Rec.Remarks,EmpName,RecoveryDate,AreaDescription,ClientCode, ClientName, HusbandName
                               )A Order By Remarks Asc";
                SqlCommand sqlcomm = new SqlCommand(Qry, sqlconn);
                dt = GetdataTable(sqlcomm, sqlconn);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, DataAccess.ProjectName, MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            return dt;
        }

        public DataTable RptRecoveryDateWiseOnlnyArea(string FromDate, string ToDate, int AreaID)
        {
            DataTable dt = new DataTable();
            try
            {
                SqlConnection sqlconn = new SqlConnection(ConnString);
                string Qry = @"SELECT ClientID, MemberCode, dbo.InItCap(MemberName + ' ' + Relation + ' ' + HusbandName) AS MemberName, Address, EmpName, Area, CellNo, 
                               DueDate, IssueDate,RecoveryDate,RecoveryAmount,Convert(nvarchar(25),Remarks) AS [Remarks]
                               From (
                               select Rec.ClientID,ClientCode AS [MemberCode], ClientName AS [MemberName], HusbandName,
                               (CASE Relation WHEN '1' THEN 'W/O' WHEN '3' THEN 'D/O' WHEN '4' THEN 'S/O' WHEN '9' THEN 'DIA/O' WHEN '10' THEN 'MIA/O' WHEN '13'
                               THEN 'C/O' ELSE '' END) AS Relation,CellNo,dbo.InItCap(Cl.Address) AS [Address],
                               dbo.InitCap(EmpName) AS [EmpName],dbo.initcap(AreaDescription) AS [Area],
                               Convert(varchar(11),RecoveryDate,106) AS [RecoveryDate],Rec.Remarks,
                               Convert(varchar(11),IssueDate,106) AS [IssueDate],
                               Convert(varchar(11),DueDate,106) AS [DueDate],Sum(RecoveryAmount) AS [RecoveryAmount] From Trans_Recovery Rec
                               Inner Join Trans_Clients Cl on Rec.ClientID=Cl.ClientID
                               Inner JOin Areas_Def Area on Cl.AreaID=Area.AreaID
                               Inner Join Emp_Def Emp on Cl.EmpCode=Emp.EmpCode
                               Where Convert(Varchar(11),RecoveryDate,121) between '" + FromDate + @"' and '" + ToDate + @"'And Area.AreaID ='" + AreaID + @"'
                               Group by Rec.ClientID,IssueDate,DueDate,relation,CellNo,Rec.Remarks,Cl.Address,EmpName,RecoveryDate,AreaDescription,ClientCode, ClientName, HusbandName
                               )A Order By IssueDate Asc";
                SqlCommand sqlcomm = new SqlCommand(Qry, sqlconn);
                dt = GetdataTable(sqlcomm, sqlconn);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, DataAccess.ProjectName, MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            return dt;
        }

        public DataTable GetCustomerName(long DRS_ID)
        {
            DataTable dt = new DataTable();
            try
            {
                SqlConnection sqlconn = new SqlConnection(ConnString);
                SqlCommand sqlcomm = new SqlCommand("Select DRS_NAME From Trans_DRSM Where DRS_ID='"+DRS_ID+"' And IsActive=1", sqlconn);
                dt = GetdataTable(sqlcomm, sqlconn);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, DataAccess.ProjectName, MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            return dt;

        }

        public DataTable RptDirectSalesCustomer(long DRS_ID)
        {
            DataTable dt = new DataTable();
            try
            {
                SqlConnection sqlconn = new SqlConnection(ConnString);
                string Qry = @"SELECT DSM.DRS_ID,Convert(varchar(11),DRS_DATE,106) As IssueDate,DRS_Name,DSD.ItemID,DSD.ItemPrice,ItemName,DSD.SalesPrice,Qty FROM tRANS_drSM DSM
                               Inner Join tRANS_drSd DSD on DSM.DRS_iD=DSD.DRS_ID
                               Inner Join items itms on DSD.ItemID=itms.itemID
                               Where DSM.DRS_ID='"+DRS_ID+ @"'
                               Group BY DSM.DRS_ID,DRS_Date,DRS_Name,DSD.ItemID,DSD.ItemPrice,ItemName,DSD.SalesPrice,Qty";
                SqlCommand sqlcomm = new SqlCommand(Qry, sqlconn);
                dt = GetdataTable(sqlcomm, sqlconn);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, DataAccess.ProjectName, MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            return dt; 
        }

        public DataTable RptDirectSalesAllCustomer(string FromDate,string ToDate)
        {
            DataTable dt = new DataTable();
            try
            {
                SqlConnection sqlconn = new SqlConnection(ConnString);
//                string Qry = @"SELECT     DSM.DRS_ID, CONVERT(varchar(11), DSM.DRS_Date, 106) AS IssueDate, DSM.DRS_Name, SUM(DSD.ItemPrice) AS ItemPurchasePrice, 
//                               SUM(DSD.SalesPrice) AS SalePrice
//                               FROM Trans_DRSM AS DSM INNER JOIN
//                               Trans_DRSD AS DSD ON DSM.DRS_ID = DSD.DRS_ID INNER JOIN
//                               Items AS itms ON DSD.ItemID = itms.ItemID
//                               Where Convert(varchar(11),DSM.DRS_Date,121) between '"+ FromDate +@"' and '"+ToDate+@"'
//                               GROUP BY DSM.DRS_ID, DSM.DRS_Date, DSM.DRS_Name";
                string Qry = @"Select A.DRS_ID,Convert(varchar(11),A.DRS_Date,106) As IssueDate,A.DRS_Name,B.ItemID,C.ItemName,B.ItemPrice,B.SalesPrice,Qty,B.SalesPrice* Qty as [Total Price],A.Discount,Type from Trans_DRSM A
inner join Trans_DRSD B ON A.DRS_ID = B.DRS_ID
inner join Items C On B.ItemID = C.ItemID
Where Convert(varchar(11),A.DRS_Date,121) between '" + FromDate + @"' and '" + ToDate + @"'
order by A.DRS_Date";
                SqlCommand sqlcomm = new SqlCommand(Qry, sqlconn);
                dt = GetdataTable(sqlcomm, sqlconn);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, DataAccess.ProjectName, MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            return dt; 
        }

        public DataTable RptDirectSalesBySpecificType(string FromDate, string ToDate,string SaleType)
        {
            DataTable dt = new DataTable();
            try
            {
                SqlConnection sqlconn = new SqlConnection(ConnString);
                //                string Qry = @"SELECT     DSM.DRS_ID, CONVERT(varchar(11), DSM.DRS_Date, 106) AS IssueDate, DSM.DRS_Name, SUM(DSD.ItemPrice) AS ItemPurchasePrice, 
                //                               SUM(DSD.SalesPrice) AS SalePrice
                //                               FROM Trans_DRSM AS DSM INNER JOIN
                //                               Trans_DRSD AS DSD ON DSM.DRS_ID = DSD.DRS_ID INNER JOIN
                //                               Items AS itms ON DSD.ItemID = itms.ItemID
                //                               Where Convert(varchar(11),DSM.DRS_Date,121) between '"+ FromDate +@"' and '"+ToDate+@"'
                //                               GROUP BY DSM.DRS_ID, DSM.DRS_Date, DSM.DRS_Name";
                string Qry = @"Select A.DRS_ID,Convert(varchar(11),A.DRS_Date,106) As IssueDate,A.DRS_Name,B.ItemID,C.ItemName,B.ItemPrice,B.SalesPrice,Qty,B.SalesPrice* Qty as [Total Price],A.Discount,Type from Trans_DRSM A
inner join Trans_DRSD B ON A.DRS_ID = B.DRS_ID
inner join Items C On B.ItemID = C.ItemID
Where Convert(varchar(11),A.DRS_Date,121) between '" + FromDate + @"' and '" + ToDate + @"' and type='"+SaleType+"' order by A.DRS_Date";
                SqlCommand sqlcomm = new SqlCommand(Qry, sqlconn);
                dt = GetdataTable(sqlcomm, sqlconn);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, DataAccess.ProjectName, MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            return dt;
        }

        public DataTable RptInActiveClients(bool IsAllEmp,bool IsAllArea, long EmpID,long AreaID,string FromDate,string ToDate)
        {
            DataTable dt = new DataTable();
            try
            {
                if (IsAllEmp == true && IsAllArea ==true)
                {
                    SqlConnection sqlconn = new SqlConnection(ConnString);

                    string Qry = @"Select ClientID,ClientCOde,CLientName,CellNo,AreaDescription,EmpName,B.EmpCode,B.AreaID,Convert(varchar(11),IssueDate,106) As [LastIssueDate],B.Address From (
                                   Select C.ClientID,ClientCode,ClientName,CellNo,AreaID,EmpCode,Address
                                   ,isnull((Select top 1 IssueDate From Trans_IssueM M where C.ClientID=M.ClientID order by IssueDate desc),'') AS [IssueDate]
                                   From trans_clients C
                                   where clientID  not in (
                                   Select distinct clientID From trans_issueM where issuedate between dateadd(Month,-2,'" + FromDate+@"') and '"+ToDate+ @"'
                                   ) ) B
                                   Inner Join Areas_Def A on B.AreaID=A.AreaID
                                   Inner Join Emp_Def E on B.EmpCode=E.EmpCode
                                   Where IssueDate not in ('1900-01-01')
                                   --and C.ClientID=2
                                   order by 1 ";

                    //Commented on 2015-02-23 on Request of Mr Saghar Farman to amend the report
//                    string Qry = @"Select ClientCode,ClientName + ' '+ Relation + ' ' + HusbandName AS [MemberName],CellNo,EmpName,Address,
//                                    areaDescription From ( 
//                                    select cl.clientid,ClientCode,ClientName, husbandname,CellNo,EmpName,AreaDescription, Cl.Address,
//                                    (CASE RelationToClient WHEN '1' THEN 'W/O' WHEN '3' THEN 'D/O' WHEN '4' THEN 'S/O' WHEN '9' THEN 'DIA/O' 
//                                    WHEN '10' THEN 'MIA/O' WHEN '13'THEN 'C/O' ELSE '' END) AS Relation,
//                                    count(*)As [NoofTurns] from trans_recovery rec
//                                    inner join trans_Clients cl on rec.clientid=cl.clientid
//                                    inner join areas_def area on cl.areaid=area.areaid
//                                    inner join emp_def emp on cl.empcode=emp.empcode
//                                    where cl.clientid in (select clientid from trans_Clients where clientstatus=4)
//                                    group by cl.clientid,ClientName ,husbandname,RelationToClient,CellNo,EmpName,ClientCode,AreaDescription, Cl.Address
//                                    having count(cl.clientid)=1) AA";
//                    string Qry = @"Select ClientStatus,ClientCode,(ClientName + ' ' + Relation + ' ' + HUsbandName) As [ClientName],
//                                   Address,CellNO,Area,AreaID,EmpCode,
//                                   EmpName From (
//                                   Select (case ClientStatus when 1 then 'New Clients' when 4 then 'Fully Recovered' end) As ClientStatus,
//                                   dbo.InItCaP(Cl.Address) AS [Address],CellNO,
//                                   (CASE RelationToClient WHEN '1' THEN 'W/O' WHEN '3' THEN 'D/O' WHEN '4' THEN 'S/O' WHEN '9' THEN 'DIA/O' WHEN '10' THEN 'MIA/O' WHEN '13'
//                                   THEN 'C/O' ELSE '' END) AS Relation,HUsbandName,Cl.AreaID,Cl.EmpCode,
//                                   ClientID,ClientCode,ClientName,EmpName,AreaDescription As [Area] From Trans_Clients Cl
//                                   Inner join Emp_Def Emp on Cl.EMpcode=Emp.EmpCode
//                                   Inner Join Areas_def Area on Cl.AreaID=Area.AreaID
//                                   where clientstatus in (1,4)
//                                   ) A";
                    SqlCommand sqlcomm = new SqlCommand(Qry, sqlconn);
                    dt = GetdataTable(sqlcomm, sqlconn);
                }
                else if (IsAllEmp == false && IsAllArea == false)
                {
                    SqlConnection sqlconn = new SqlConnection(ConnString);
                    string Qry = @"Select ClientID,ClientCOde,CLientName,CellNo,AreaDescription,EmpName,B.EmpCode,B.AreaID,Convert(varchar(11),IssueDate,106) As [LastIssueDate],B.Address From (
                                   Select C.ClientID,ClientCode,ClientName,CellNo,AreaID,EmpCode,Address
                                   ,isnull((Select top 1 IssueDate From Trans_IssueM M where C.ClientID=M.ClientID order by IssueDate desc),'') AS [IssueDate]
                                   From trans_clients C
                                   where clientID  not in (
                                   Select distinct clientID From trans_issueM where issuedate between dateadd(Month,-2,'" + FromDate + @"') and '" + ToDate + @"'
                                   ) ) B
                                   Inner Join Areas_Def A on B.AreaID=A.AreaID
                                   Inner Join Emp_Def E on B.EmpCode=E.EmpCode
                                   Where IssueDate not in ('1900-01-01') and AreaID='"+AreaID+ @"' and EmpCode ='"+EmpID+ @"'
                                   --and C.ClientID=2
                                   order by 1 ";
                    SqlCommand sqlcomm = new SqlCommand(Qry, sqlconn);
                    dt = GetdataTable(sqlcomm, sqlconn);
                }
                else if (IsAllEmp == false && IsAllArea == true)
                {
                    SqlConnection sqlconn = new SqlConnection(ConnString);
                    string Qry = @"Select ClientID,ClientCOde,CLientName,CellNo,AreaDescription,EmpName,B.EmpCode,B.AreaID,Convert(varchar(11),IssueDate,106) As [LastIssueDate],B.Address From (
                                   Select C.ClientID,ClientCode,ClientName,CellNo,AreaID,EmpCode,Address
                                   ,isnull((Select top 1 IssueDate From Trans_IssueM M where C.ClientID=M.ClientID order by IssueDate desc),'') AS [IssueDate]
                                   From trans_clients C
                                   where clientID  not in (
                                   Select distinct clientID From trans_issueM where issuedate between dateadd(Month,-2,'" + FromDate + @"') and '" + ToDate + @"'
                                   ) ) B
                                   Inner Join Areas_Def A on B.AreaID=A.AreaID
                                   Inner Join Emp_Def E on B.EmpCode=E.EmpCode
                                   Where IssueDate not in ('1900-01-01') and EmpCode ='" + EmpID + @"'
                                   --and C.ClientID=2
                                   order by 1 ";
                    SqlCommand sqlcomm = new SqlCommand(Qry, sqlconn);
                    dt = GetdataTable(sqlcomm, sqlconn);
                }
                else if (IsAllEmp == true && IsAllArea == false)
                {
                    SqlConnection sqlconn = new SqlConnection(ConnString);
                    string Qry = @"Select ClientID,ClientCOde,CLientName,CellNo,AreaDescription,EmpName,B.EmpCode,B.AreaID,Convert(varchar(11),IssueDate,106) As [LastIssueDate],B.Address From (
                                   Select C.ClientID,ClientCode,ClientName,CellNo,AreaID,EmpCode,Address
                                   ,isnull((Select top 1 IssueDate From Trans_IssueM M where C.ClientID=M.ClientID order by IssueDate desc),'') AS [IssueDate]
                                   From trans_clients C
                                   where clientID  not in (
                                   Select distinct clientID From trans_issueM where issuedate between dateadd(Month,-2,'" + FromDate + @"') and '" + ToDate + @"'
                                   ) ) B
                                   Inner Join Areas_Def A on B.AreaID=A.AreaID
                                   Inner Join Emp_Def E on B.EmpCode=E.EmpCode
                                   Where IssueDate not in ('1900-01-01') and AreaID='"+AreaID+ @"'
                                   --and C.ClientID=2
                                   order by 1 ";
                    SqlCommand sqlcomm = new SqlCommand(Qry, sqlconn);
                    dt = GetdataTable(sqlcomm, sqlconn);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, DataAccess.ProjectName, MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            return dt;
        }

        public DataTable Rpt_ClearanceDetails(string FromDate,string ToDate)
        {
            DataTable dt = new DataTable();
            try
            {
                SqlConnection sqlconn = new SqlConnection(ConnString);
                string qry = @"Select ClearanceDate,
                               HeadDetailName,HeadName,
                               sum(amount) As [Amount]
                               From trans_ClearanceM CM
                               Inner JOIn Trans_ClearanceD cd on Cm.ClearanceID=cd.ClearanceID
                               Inner Join accountheadDetails AHD on CD.HeadDetailID=AHD.HeadDetailID
                               Inner Join AccountsHead AH on AHD.HeadID=AH.HeadID
                               Where convert(varchar(11),ClearanceDate,121) between '"+FromDate+@"' and '"+ToDate+@"'
                               Group By ClearanceDate,HeadDetailName,HeadName";
                SqlCommand sqlcomm = new SqlCommand(qry,sqlconn);
                dt = GetdataTable(sqlcomm, sqlconn);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, DataAccess.ProjectName, MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            return dt;
        }

        public DataTable RptDisbursmentProfit(string FromDate,string ToDate)
        {
            DataTable dt = new DataTable();
            try
            {
                SqlConnection sqlconn = new SqlConnection(ConnString);
                //Commented on 2016-10-27 by Fahad...
//                string qry = @"Select MemberCode,MemberName,IssueDate,EmpName,AreaDescription,ItemPrice,IssueQty,ItemSalePrice,Profit,ServiceCharge,(TotalSalePrice +ServiceCharge) as [TotalSalePrice],totalpr From (
//Select MemberCode,MemberName,IssueDate,EmpName,AreaDescription ,ItemPrice,IssueQty,ItemSalePrice,Profit,TotalSalePrice*0.05 as [ServiceCharge] ,TotalSalePrice ,TotalPr From (
//Select memberCode,MemberName,IssueDate,EmpName,AreaDescription ,sum(ItemPrice) as [ItemPrice],sum(IssueQty) as [IssueQty],sum(ItemSalePrice) as [ItemSalePrice],sum(Profit) as [Profit],sum(totalsaleprice) as [TotalSalePrice],sum(TotalPr) as [TotalPr] From (
//Select MemberCode,membername,IssueDate,EmpName,AreaDescription ,itemPrice,IssueQty,ItemSalePrice, ((ItemSalePrice-ItemPrice)*Issueqty) as [Profit], TotalSalePrice,TotalPr From (
//Select MemberCode,MemberName,IssueDate,EMpname,AreaDescription ,sum(ItemPrice) As [ItemPrice],IssueQty,
//                               sum(ItemSalePrice) As [ItemSalePrice],sum(TotalPr)as [TotalPr],
//                               sum(TotalPrice)As [TotalSalePrice] From (
//                               Select ClientCode As [MemberCode],ClientName as [MemberName],cl.EmpCode,EmpName,
//                               areaDescription,
//                               Convert(varchar(11),issueDate,106) As [IssueDate],
//                               ItemName,IssueQty,ID.ItemPrice,itemSalePrice,(issueqty*id.ItemPrice)as [TotalPR],
//                               (IssueQty*ItemSalePrice) AS [TotalPrice]
//                               From Trans_Issuem IM
//                               Inner Join Trans_IssueD ID on IM.IssueID=ID.IssueID
//                               Inner JOIn trans_Clients cl on Im.ClientID=Cl.ClientID
//                               Inner Join items on ID.ItemID=Items.ItemID
//                               Inner JOin Emp_Def Emp on Cl.EmpCode=Emp.EmpCode
//                               Inner JOin Areas_Def Area on Cl.AreaID=Area.AreaID
//                               Where Convert(varchar(11),IssueDate,121) between '"+FromDate+@"' and '"+ToDate+@"' 
//                               )AA
//                               Group by MemberCode,MemberName,IssueDate,EmpName,AreaDescription,IssueQty)Main )Main2
//Group by memberCode,MemberName,IssueDate,EmpName,AreaDescription)Main3) MainQry";
                string qry = @"Select MemberCode,MemberName,IssueDate,EmpName,AreaDescription,ItemPrice,IssueQty,ItemSalePrice,Profit,ServiceCharge,(TotalSalePrice) as [TotalSalePrice],totalpr From (
Select MemberCode,MemberName,IssueDate,EmpName,AreaDescription ,ItemPrice,IssueQty,ItemSalePrice,Profit,TotalSalePrice as [ServiceCharge] ,TotalSalePrice ,TotalPr From (
Select memberCode,MemberName,IssueDate,EmpName,AreaDescription ,sum(ItemPrice) as [ItemPrice],sum(IssueQty) as [IssueQty],sum(ItemSalePrice) as [ItemSalePrice],sum(Profit) as [Profit],sum(totalsaleprice) as [TotalSalePrice],sum(TotalPr) as [TotalPr] From (
Select MemberCode,membername,IssueDate,EmpName,AreaDescription ,itemPrice,IssueQty,ItemSalePrice, ((ItemSalePrice-ItemPrice)*Issueqty) as [Profit], TotalSalePrice,TotalPr From (
Select MemberCode,MemberName,IssueDate,EMpname,AreaDescription ,sum(ItemPrice) As [ItemPrice],IssueQty,
                               sum(ItemSalePrice) As [ItemSalePrice],sum(TotalPr)as [TotalPr],
                               sum(TotalPrice)As [TotalSalePrice] From (
                               Select ClientCode As [MemberCode],ClientName as [MemberName],cl.EmpCode,EmpName,
                               areaDescription,
                               Convert(varchar(11),issueDate,106) As [IssueDate],
                               ItemName,IssueQty,ID.ItemPrice,itemSalePrice,(issueqty*id.ItemPrice)as [TotalPR],
                               (IssueQty*ItemSalePrice) AS [TotalPrice]
                               From Trans_Issuem IM
                               Inner Join Trans_IssueD ID on IM.IssueID=ID.IssueID
                               Inner JOIn trans_Clients cl on Im.ClientID=Cl.ClientID
                               Inner Join items on ID.ItemID=Items.ItemID
                               Inner JOin Emp_Def Emp on Cl.EmpCode=Emp.EmpCode
                               Inner JOin Areas_Def Area on Cl.AreaID=Area.AreaID
                               Where Convert(varchar(11),IssueDate,121) between '" + FromDate + @"' and '" + ToDate + @"' 
                               )AA
                               Group by MemberCode,MemberName,IssueDate,EmpName,AreaDescription,IssueQty)Main )Main2
Group by memberCode,MemberName,IssueDate,EmpName,AreaDescription)Main3) MainQry";
                SqlCommand sqlcomm = new SqlCommand(qry, sqlconn);
                //sqlcomm.Parameters.AddWithValue("@FromDate",FromDate);
                //sqlcomm.Parameters.AddWithValue("@ToDate", ToDate);
                dt = GetdataTable(sqlcomm, sqlconn);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, DataAccess.ProjectName, MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            return dt;
        }

        public DataTable RptInvestment(string FromDate, string ToDate)
        {
            DataTable dataTable = new DataTable();
            try
            {
                string Qry = @"Select Convert(varchar(11),A.Created_At,106) As[LoanStartDate],PersonName,PersonPhone,PersonCNIC,CityName,PersonEmail,
ISNULL(Sum(Amount),0) As [LoanAmount],
ISNULL((Select ISNULL(Sum(T.Amount),0) as [ReturnAmount] from Trans_LoanReturn T where T.PersonID = A.PersonID and ReturnDate between @FromDate and @ToDate),0) As [ReturnAmount]
from Trans_LoanAssign A
inner join Trans_LoanPosting B ON A.PersonID=B.PersonID
where A.IsActive=1 and B.Created_At between @FromDate and @ToDate
Group by A.Created_At,PersonName,PersonPhone,PersonCNIC,CityName,PersonEmail,A.PersonID
";
                SqlConnection sqlConnection = new SqlConnection(this.ConnString);
                //SqlCommand sqlcomm = new SqlCommand("Select Convert(varchar(11),INV.InvestmentDate,106) AS [InvestmentStartDate],\r\n                               PersonName,PersonPhone,PersonNIC,PersonEmail,\r\n                               IsNULL(sum(amount),0) As [InvestmentAmount], \r\n                               IsNULL((Select IsNULL(sum(IVR.amount),0) AS [ReturnAmount] From Trans_InvestmentReturn IVR Where IVR.InvestorID=INV.InvestorsID And ReturnDate between @FromDate and @ToDate),0) AS [ReturnAmount]\r\n                               From Trans_Investors INV\r\n                               Inner Join Trans_InvestmentPosting INVP on INV.InvestorsID=INVP.InvestorsID\r\n                               Where INV.IsActive=1 And INVP.InvestmentDate between @FromDate and @ToDate\r\n                               Group by INV.InvestmentDate,PersonName,PersonPhone,PersonNIc,PersonEmail,INV.InvestorsID", sqlConnection);
                SqlCommand sqlcomm = new SqlCommand(Qry,sqlConnection);
                sqlcomm.Parameters.AddWithValue("@FromDate", (object)FromDate);
                sqlcomm.Parameters.AddWithValue("@ToDate", (object)ToDate);
                dataTable = this.GetdataTable(sqlcomm, sqlConnection);
            }
            catch (Exception ex)
            {
                int num = (int)MessageBox.Show(ex.Message, DataAccess.ProjectName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
            return dataTable;
        }

        public DataTable Rpt_FundStatement(string FromDate, string ToDate)
        {
            DataTable dataTable = new DataTable();
            try
            {
                SqlConnection sqlConnection = new SqlConnection(this.ConnString);
                dataTable = this.GetdataTable(new SqlCommand("Select * From (Select \r\n                            IsNULL((Select count(*) From Trans_IssueM \r\n                            where Convert(varchar(11),issuedate,121) between '" + FromDate + "' and '" + ToDate + "'),0) AS [NoOfDisbursmentClients], \r\n                            IsNULL((Select IsNULL(sum(totalamount),0) AS [DisbursmentAmount] From Trans_IssueM IM\r\n                            Inner Join Trans_Issued ID on IM.IssueID=ID.IssueID\r\n                            where Convert(varchar(11),issuedate,121) between '" + FromDate + "' and '" + ToDate + "' \r\n                            ),0) AS [DisbursmentAmount],\r\n                            IsNull((select count(*)As [RenewelRegistration] from registration where convert(varchar(11),regdate,121) between '" + FromDate + "' and '" + ToDate + "'),0) As[RenewelRegistration],\r\n                            isNULL((Select isnull(sum(RegAmount),0) As [Regamount] From registration where regdate between '" + FromDate + "' and '" + ToDate + "'),0) As [RenewelRegistrationAmount],\r\n                            Isnull((Select count(*) as [NewRegistration] From Trans_clients where Registrationdate between '" + FromDate + "' and '" + ToDate + "'),0) as [NewRegistration],\r\n                            IsNULL((Select isnull(sum(Convert(int,Registration_Fee)),0) As [RegistrationFee] From trans_Clients where registrationdate between '" + FromDate + "' and '" + ToDate + "'),0) As [NewRegisteredClients],\r\n                            IsNull((Select isnull(sum(amount),0) As [Salary] From trans_clearanceD CD\r\n                            Inner JOIn accountheaddetails AHD on CD.HeaddetailID=AHD.HeadDetailID\r\n                            Inner Join Trans_ClearanceM CM on CD.ClearanceID=CM.ClearanceID\r\n                            Where headdetailname like '%salary%' and clearanceDate between '" + FromDate + "' and '" + ToDate + "'),0) AS [Salary],\r\n                            IsNull((Select isnull(sum(amount),0) As [Salary] From trans_clearanceD CD\r\n                            Inner JOIn accountheaddetails AHD on CD.HeaddetailID=AHD.HeadDetailID\r\n                            Inner Join Trans_ClearanceM CM on CD.ClearanceID=CM.ClearanceID\r\n                            Where headdetailname like '%Entertainment%' and clearanceDate between '" + FromDate + "' and '" + ToDate + "'),0) AS [Entertainment],\r\n                            IsNull((Select isnull(sum(amount),0) As [Salary] From trans_clearanceD CD\r\n                            Inner JOIn accountheaddetails AHD on CD.HeaddetailID=AHD.HeadDetailID\r\n                            Inner Join Trans_ClearanceM CM on CD.ClearanceID=CM.ClearanceID\r\n                            Where headdetailname like '%Stationery%' and clearanceDate between '" + FromDate + "' and '" + ToDate + "'),0) AS [Stationery],\r\n                            IsNull((Select isnull(sum(amount),0) As [Salary] From trans_clearanceD CD\r\n                            Inner JOIn accountheaddetails AHD on CD.HeaddetailID=AHD.HeadDetailID\r\n                            Inner Join Trans_ClearanceM CM on CD.ClearanceID=CM.ClearanceID\r\n                            Where headdetailname like '%Utility%' and clearanceDate between '" + FromDate + "' and '" + ToDate + "'),0) AS [Utility],\r\n                            IsNULL((Select Isnull(sum(totalamount),0)-IsNULL(sum(recoveryamount),0) From Trans_Recovery where RecoveryDate between '" + FromDate + "' and '" + ToDate + "'),0) AS [TotalRecovery],\r\n                            IsNULL((select Convert(int,IsNULL(sum(NetAmount),0)-IsNULL(sum(installment),0)) As [Due]From trans_accounthistory where Convert(varchar(11),duedate,121) between '" + FromDate + "' and '" + ToDate + "'),0) AS [Due],\r\n                            IsNull((Select isnull(sum(amount),0) AS [InvestmentAmount] From trans_investmentposting where investmentdate between '" + FromDate + "' and '" + ToDate + "'),0) As [InvestmentReceived],\r\n                            IsNULL((Select IsNULL(sum(amount),0) AS [InvestmentReturn] From Trans_InvestmentReturn where returndate between '" + FromDate + "' and '" + ToDate + "'),0) AS [InvestmentReturn]\r\n                            From Trans_IssueM)AA\r\n                            Group BY NoOfDisbursmentClients,DisbursmentAmount,RenewelRegistration,newregistration,TotalRecovery,due,RenewelRegistrationAmount,\r\n                            NewRegisteredClients,Salary,Entertainment,Stationery,Utility,\r\n                            InvestmentReceived,InvestmentReturn", sqlConnection), sqlConnection);
            }
            catch (Exception ex)
            {
                int num = (int)MessageBox.Show(ex.Message, DataAccess.ProjectName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
            return dataTable;
        }

        public DataTable Rpt_Cash(string FromDate)
        {
            DataTable dataTable = new DataTable();
            try
            {
                SqlConnection sqlConnection = new SqlConnection(ConnString);
                if (FromDate == "2014-02-01")
                {
                    string qry1stFeb = @"Select * FRom (
                                        Select 'Recovery' AS [TransactionType],ISNULL(SUM(Dr),0) As [Debit],0 As [Credit] From CashBalance Where TableName ='Trans_Recovery' and Convert(varchar(11),CashInDate,121) ='2014-02-01'
                                        union All
                                        Select 'BankWithDrawl',ISNULL(SUM(Dr),0) As [Debit],0 As [Credit] From CashBalance Where TableName ='Trans_BankWithDrawl' and Convert(varchar(11),CashInDate,121)='2014-02-01' 
                                        union All
                                        Select 'Investment',ISNULL(SUM(Dr),0) As [Debit],0 As [Credit] From CashBalance Where TableName ='Trans_InvestmentPosting' and Convert(varchar(11),CashInDate,121) ='2014-02-01' 
                                        union all
                                        Select 'OtherSales',ISNULL(SUM(Dr),0) As [Debit],0 As [Credit] From CashBalance Where TableName ='Trans_otherSale' and Convert(varchar(11),CashInDate,121)='2014-02-01' 
                                        Union All
                                        Select 'DirectSales',ISNULL(SUM(Dr),0) As [Debit],0 As [Credit] From CashBalance Where TableName ='Trans_DRSM' and Convert(varchar(11),CashInDate,121) ='2014-02-01' 
                                        Union All
                                        Select 'BankDeposit',0 As [Debit],ISNULL(SUM(Cr),0) As [Credit] From CashBalance Where TableName ='Trans_BankDeposit' and Convert(varchar(11),CashInDate,121)='2014-02-01' 
                                        union All
                                        Select 'Expense',0 As [Debit],ISNULL(SUM(Cr),0) As [Credit] From CashBalance Where TableName ='Trans_ClearanceD' and Convert(varchar(11),CashInDate,121)='2014-02-01' 
                                        Union All
                                        Select 'InvestmentReturn',0 As [Debit],ISNULL(SUM(Cr),0) As [Credit] From CashBalance Where TableName ='Trans_InvestmentReturn' and Convert(varchar(11),CashInDate,121)='2014-02-01' 
                                        Union All
                                        Select 'ProfitSharing',0 As [Debit],Isnull(Sum(ProfitSharing),0) As [Credit] From Trans_InvestmentReturn where Convert(varchar(11),ReturnDate,121) between '2016-08-13' and '2016-08-13'
									    union all
                                        Select abc,Debit,SUM(PurchasePrice) AS [PurchasePrice] From (
                                        Select 'Purchase' AS [abc] ,0 as debit,(PrQty*PurchasePrice)As [PurchasePrice] From Trans_PurchaseD PD 
                                        Inner Join Trans_PurchaseM PM on PD.PRID=PM.PRID 
                                        Where Convert(varchar(11),PRDate,121)='2014-02-01') AA
                                        group by abc,debit
                                        Union All
                                        Select 'KnockOffPurchase' as [abc], 0 AS debit,isnull(Sum(cr),0) AS [Credit] From CashBalance Where TableName ='KnockOffPurchase' 
                                        and Convert(varchar(11),CashIndate,121)='2014-02-01'
                                        union all
                                        Select 'Registration',isnULL(sum(RegistrationClients+RegistrationAmount+RegistrationRecovery),0) As [Registration],
                                        0 As [Credit] From (
                                        Select ISNULL(SUM(Convert(int,Registration_Fee)),0) As [RegistrationClients],
                                        IsnULL((select isnull(sum(RegAmount),0) As [RegistrationAmount] from Registration 
                                        where Convert(varchar(11),RegDate,121)=TC.RegistrationDate
                                        ),0) As [RegistrationAmount],
                                        ISNULL((Select Isnull(SUM(registrationFee),0) + Isnull((Select isnull(SUM(regAmount),0) AS [Registration] From Registration where RegDate ='2014-02-01'
                                        ),0) As [RegistrationFee] From Trans_Recovery 
                                        Where Convert(varchar(11),RecoveryDate,121) =TC.RegistrationDate
                                        ),0) AS [RegistrationRecovery]
                                        From Trans_Clients TC
                                        Where Convert(varchar(11),TC.RegistrationDate,121)='2014-02-01'
                                        Group By TC.RegistrationDate) AA ) AAA";
                    dataTable = this.GetdataTable(new SqlCommand(qry1stFeb, sqlConnection), sqlConnection);
                }
                else
                {
                    string qry = @"Select * FRom (
                                    Select 'Recovery' AS [TransactionType],ISNULL(SUM(Dr),0) As [Debit],0 As [Credit] From CashBalance Where TableName ='Trans_Recovery' and Convert(varchar(11),CashInDate,121) = '" + FromDate + @"'

union all
									Select 'Discount',0 As Debit,isnull(sum(DisAmount),0) As [DiscountAmount] From Trans_Recovery Where Convert(varchar(11),RecoveryDate,121) ='" + FromDate + @"'

                                    union All
                                    Select 'BankWithDrawl',ISNULL(SUM(Dr),0) As [Debit],0 As [Credit] From CashBalance Where TableName ='Trans_BankWithDrawl' and Convert(varchar(11),CashInDate,121) ='" + FromDate + @"' 
                                    union All
                                    Select 'Investment',ISNULL(SUM(Dr),0) As [Debit],0 As [Credit] From CashBalance Where TableName ='Trans_InvestmentPosting' and Convert(varchar(11),CashInDate,121) = '" + FromDate + @"' 
                                    union all
                                    Select 'OtherSales',ISNULL(SUM(Dr),0) As [Debit],0 As [Credit] From CashBalance Where TableName ='Trans_otherSale' and Convert(varchar(11),CashInDate,121)= '" + FromDate + @"' 
                                    Union All
                                    Select 'DirectSales',ISNULL(SUM(Dr),0) As [Debit],0 As [Credit] From CashBalance Where TableName ='Trans_DRSM' and Convert(varchar(11),CashInDate,121)= '" + FromDate + @"' 
                                    Union All
                                    Select 'BankDeposit',0 As [Debit],ISNULL(SUM(Cr),0) As [Credit] From CashBalance Where TableName ='Trans_BankDeposit' and Convert(varchar(11),CashInDate,121)= '" + FromDate + @"' 
                                    union All
                                    Select 'Expense',0 As [Debit],ISNULL(SUM(Cr),0) As [Credit] From CashBalance Where TableName ='Trans_ClearanceD' and Convert(varchar(11),CashInDate,121)= '" + FromDate + @"' 
                                    Union All
                                    Select 'InvestmentReturn',0 As [Debit],ISNULL(SUM(Cr),0) As [Credit] From CashBalance Where TableName ='Trans_InvestmentReturn' and Convert(varchar(11),CashInDate,121)= '" + FromDate + @"' 
                                    Union All
                                    Select 'ProfitSharing',0 As [Debit],Isnull(Sum(ProfitSharing),0) As [Credit] From Trans_InvestmentReturn where Convert(varchar(11),ReturnDate,121) between '2016-08-13' and '2016-08-13'
									Union all
                                    Select abc,Debit,SUM(PurchasePrice) AS [PurchasePrice] From (
                                    Select 'Purchase' AS [abc] ,0 as debit,(PrQty*PurchasePrice)As [PurchasePrice] From Trans_PurchaseD PD 
                                    Inner Join Trans_PurchaseM PM on PD.PRID=PM.PRID 
                                    Where Convert(varchar(11),PRDate,121) = '" + FromDate + @"') AA
                                    group by abc,debit
                                    Union All
                                    Select 'KnockOffPurchase' as [abc], 0 AS debit,isnull(Sum(cr),0) AS [Credit] From CashBalance Where TableName ='KnockOffPurchase' 
                                    and Convert(varchar(11),CashIndate,121)='" + FromDate + @"'
                                    union all

                                    Select 'Registration',isnULL(sum(RegistrationClients+RegistrationAmount+RegistrationRecovery),0) As [Registration],
                                    0 As [Credit] From (
                                    Select ISNULL(SUM(Convert(int,Registration_Fee)),0) As [RegistrationClients],
                                    IsnULL((select isnull(sum(RegAmount),0) As [RegistrationAmount] from Registration 
                                    where Convert(varchar(11),RegDate,121)=TC.RegistrationDate
                                    ),0) As [RegistrationAmount],

                                    ISNULL((Select Isnull(SUM(registrationFee),0) + Isnull((Select isnull(SUM(regAmount),0) AS [Registration] From Registration where RegDate = '" + FromDate + @"'
                                    ),0) As [RegistrationFee] From Trans_Recovery 
                                    Where Convert(varchar(11),RecoveryDate,121) =TC.RegistrationDate
                                    ),0) AS [RegistrationRecovery]
                                    From Trans_Clients TC
                                    Where Convert(varchar(11),TC.RegistrationDate,121)='" + FromDate + @"'
                                    Group By TC.RegistrationDate) AA ) AAA";
                    dataTable = this.GetdataTable(new SqlCommand(qry, sqlConnection), sqlConnection);
                }

                //                string qry = @"Select TransactionType,Debit,Credit,(OpeningBalance-Opening2) AS [OpeningBalance] From (
                //                               Select TransactionType,Debit,Credit,
                //                               ISNULL((Select IsNULL(SUM(Dr),0) As [Dr] From CashBalance Where TableName in ('Trans_Recovery','Trans_BankWithDrawl','Trans_InvestmentPosting','Trans_OtherSale','Trans_DRSM') and Convert(varchar(11),CashInDate,121) <'"+FromDate+ @"' ),0) AS [OpeningBalance],
                //                               ISNULL((Select IsNULL(SUM(Cr),0) As [Cr] From CashBalance Where TableName in ('Trans_BankDeposit','Trans_ClearanceD','Trans_InvestmentReturn') and Convert(varchar(11),CashInDate,121) <'" + FromDate + @"'),0) AS [Opening2] 
                //                               From (
                //                               Select 'Recovery' AS [TransactionType],ISNULL(SUM(Dr),0) As [Debit],0 As [Credit] From CashBalance Where TableName ='Trans_Recovery' and Convert(varchar(11),CashInDate,121) = '" + FromDate + @"' --and '2013-04-10' 
                //                               union All
                //                               Select 'BankWithDrawl',ISNULL(SUM(Dr),0) As [Debit],0 As [Credit] From CashBalance Where TableName ='Trans_BankWithDrawl' and Convert(varchar(11),CashInDate,121)= '" + FromDate + @"' --and '2013-04-10' 
                //                               union All
                //                               Select 'Investment',ISNULL(SUM(Dr),0) As [Debit],0 As [Credit] From CashBalance Where TableName ='Trans_InvestmentPosting' and Convert(varchar(11),CashInDate,121) ='" + FromDate + @"' ---and '2013-04-10' 
                //                               union all
                //                               Select 'OtherSales',ISNULL(SUM(Dr),0) As [Debit],0 As [Credit] From CashBalance Where TableName ='Trans_otherSale' and Convert(varchar(11),CashInDate,121)= '" + FromDate + @"' --and '2013-04-10' 
                //                               Union All
                //                               Select 'DirectSales',ISNULL(SUM(Dr),0) As [Debit],0 As [Credit] From CashBalance Where TableName ='Trans_DRSM' and Convert(varchar(11),CashInDate,121) ='" + FromDate + @"' --and '2013-04-10' 
                //                               Union All
                //                               Select 'BankDeposit',0 As [Debit],ISNULL(SUM(Cr),0) As [Credit] From CashBalance Where TableName ='Trans_BankDeposit' and Convert(varchar(11),CashInDate,121)= '" + FromDate + @"' --and '2013-04-10' 
                //                               union All
                //                               Select 'Expense',0 As [Debit],ISNULL(SUM(Cr),0) As [Credit] From CashBalance Where TableName ='Trans_ClearanceD' and Convert(varchar(11),CashInDate,121)= '" + FromDate + @"' --and '2013-04-10' 
                //                               Union All
                //                               Select 'InvestmentReturn',0 As [Debit],ISNULL(SUM(Cr),0) As [Credit] From CashBalance Where TableName ='Trans_InvestmentReturn' and Convert(varchar(11),CashInDate,121)='" + FromDate + @"' --and '2013-04-10'
                //                               Union All
                //                                Select abc,Debit,SUM(PurchasePrice) AS [PurchasePrice] From (
                //                               Select 'Purchase' AS [abc] ,0 as debit,(PrQty*PurchasePrice)As [PurchasePrice] From Trans_PurchaseD PD 
                //                               Inner Join Trans_PurchaseM PM on PD.PRID=PM.PRID 
                //                               Where Convert(varchar(11),PRDate,121)='" + FromDate + @"') AA
                //                               group by abc,debit
                //                               Union All
                //                               Select 'KnockOffPurchase' as [abc], 0 AS debit,isnull(Sum(cr),0) AS [Credit] From CashBalance Where TableName ='KnockOffPurchase' 
                //                               and Convert(varchar(11),CashIndate,121)='" + FromDate + @"'
                //                               union all
                //                               Select 'Registration',sum(RegistrationClients+RegistrationAmount+RegistrationRecovery) As [Registration],
                //                               0 As [Credit] From (
                //                               Select ISNULL(SUM(Convert(int,Registration_Fee)),0) As [RegistrationClients],
                //                               IsnULL((select isnull(sum(RegAmount),0) As [RegistrationAmount] from Registration 
                //                               where Convert(varchar(11),RegDate,121)=TC.RegistrationDate
                //                               ),0) As [RegistrationAmount],
                //                               ISNULL((Select Isnull(SUM(registrationFee),0) + Isnull((Select isnull(SUM(regAmount),0) AS [Registration] From Registration where RegDate='"+FromDate+ @"'
                //                               ),0) As [RegistrationFee] From Trans_Recovery 
                //                               Where Convert(varchar(11),RecoveryDate,121) =TC.RegistrationDate
                //                               ),0) AS [RegistrationRecovery]
                //                               From Trans_Clients TC
                //                               Where Convert(varchar(11),TC.RegistrationDate,121)='" + FromDate+@"'
                //                               Group By TC.RegistrationDate) AA)AA)ABC";

            }
            catch (Exception ex)
            {
                int num = (int)MessageBox.Show(ex.Message, DataAccess.ProjectName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
            return dataTable;
        }

        public DataTable Rpt_StockTransfer(string FromDate, string ToDate)
        {
            DataTable dataTable = new DataTable();
            try
            {
                SqlConnection sqlConnection = new SqlConnection(this.ConnString);
                string qry = @"Select GodownName,GodownAddress,ItemName,Stock,Convert(varchar(11),Transferdate,106) AS [TransferDate] From Trans_StockTransferM TM
                               Inner join Trans_StockTransferD TD on TM.TransferID=TD.TransferID
                               Inner Join Items on TD.ItemID=Items.ItemID
                               Inner Join Godown_Def Godown on TM.GodownID=Godown.GodownID
                               Where Transferdate between '"+FromDate+@"' and '"+ToDate+@"'";
                dataTable = this.GetdataTable(new SqlCommand(qry, sqlConnection), sqlConnection);
            }
            catch (Exception ex)
            {
                int num = (int)MessageBox.Show(ex.Message, DataAccess.ProjectName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
            return dataTable;
        }

        public DataTable RptSalary(string MonthID,string SalaryYear, bool IsAllEmployees,int EmpID)
        {
            DataTable dt = new DataTable();
            try
            {
                string qry = "";
                SqlConnection sqlconn = new SqlConnection(ConnString);
                if (IsAllEmployees)
                {

//                    qry = @"Select (case SalaryMonth when 1 then 'January' when 2 then 'February' when 3 then 'march' when 4 then 'April' when 5 then 'may'
//                                when 6 then 'June' when 7 then 'July' when 8 then 'August' when 9 then 'September' when 10 then 'Octobar' when 11 then 'November' when 12 then 'December' end) 
//                                AS [MonthName],EmpName,SAL.Salary,EOBI,ProvidentFund,Deductions,SalaryDays,Deductions,Leaves,
//                                Designation,Address,IsNULL(Incentive,0) AS [Incentive] From Trans_Salaries SAL
//                                Inner Join Emp_Def Emp on SAL.EmpID=Emp.EmpCode
//                                Where SalaryMonth='" + MonthID + "' and SalaryYear='" + SalaryYear + "'";
                    qry = @"Select (case SalaryMonth when 1 then 'January' when 2 then 'February' when 3 then 'March' when 4 then 'April' when 5 then 'May'
                            when 6 then 'June' when 7 then 'July' when 8 then 'August' when 9 then 'September' when 10 then 'Octobar' when 11 then 'November' when 12 then 'December' end) 
                            AS [MonthName],EmpName,SAL.Salary,EOBI,ProvidentFund,Deductions,SalaryDays,Deductions,Leaves
                            ,ISNULL((Select DepartmentName From Department Deptt Where Deptt.DepartmentID=Emp.DepartmentID),0) As [DepartmentName]
                            ,Designation,Address,IsNULL(Incentive,0) AS [Incentive],IsnULL(LoanAmount,0) AS [LoanAmount], ISNULL(BasicSalary,0) AS [BasicSalary],ISNULL(HouseRent,0) AS [HouseRent],
                            ISNULL(Utilities,0) AS [Utilities],Isnull(TravelExpense,0) AS [TranvelExpense],Isnull(Allowance,0) AS [Allowance],ISNULL(InComeTax,0) AS [InComeTax],
                            Isnull(LoanApproved,0) AS [LoanApproved],ISNULL(LoanApprovedDate,'1900-01-01') AS [LoanApprovedDate],Isnull(OutStandingAmount,0) AS [OutStandingAmount],
                            Address,CNIC,Grade,EmployeeType,NTNNo
                            From Trans_Salaries SAL
                            Inner Join Emp_Def Emp on SAL.EmpID=Emp.EmpCode
                            Where SalaryMonth='" + MonthID + "' and SalaryYear='" + SalaryYear + "'";
                }
                else
                {
//                    qry = @"Select (case SalaryMonth when 1 then 'January' when 2 then 'February' when 3 then 'march' when 4 then 'April' when 5 then 'may'
//                                when 6 then 'June' when 7 then 'July' when 8 then 'August' when 9 then 'September' when 10 then 'Octobar' when 11 then 'November' when 12 then 'December' end) 
//                                AS [MonthName],EmpName,SAL.Salary,EOBI,ProvidentFund,Deductions,SalaryDays,Deductions,Leaves,
//                                Designation,Address,IsNULL(Incentive,0) AS [Incentive] From Trans_Salaries SAL
//                                Inner Join Emp_Def Emp on SAL.EmpID=Emp.EmpCode
//                                Where EmpCode='" + EmpID + "' and SalaryMonth='"+MonthID+"' and SalaryYear='"+SalaryYear+"'";
                    qry = @"Select (case SalaryMonth when 1 then 'January' when 2 then 'February' when 3 then 'March' when 4 then 'April' when 5 then 'May'
                            when 6 then 'June' when 7 then 'July' when 8 then 'August' when 9 then 'September' when 10 then 'Octobar' when 11 then 'November' when 12 then 'December' end) 
                            AS [MonthName],EmpName,SAL.Salary,EOBI,ProvidentFund,Deductions,SalaryDays,Deductions,Leaves
                            ,ISNULL((Select DepartmentName From Department Deptt Where Deptt.DepartmentID=Emp.DepartmentID),0) As [DepartmentName]
                            ,Designation,Address,IsNULL(Incentive,0) AS [Incentive],IsnULL(LoanAmount,0) AS [LoanAmount], ISNULL(BasicSalary,0) AS [BasicSalary],ISNULL(HouseRent,0) AS [HouseRent],
                            ISNULL(Utilities,0) AS [Utilities],Isnull(TravelExpense,0) AS [TranvelExpense],Isnull(Allowance,0) AS [Allowance],ISNULL(InComeTax,0) AS [InComeTax],
                            Isnull(LoanApproved,0) AS [LoanApproved],ISNULL(LoanApprovedDate,'1900-01-01') AS [LoanApprovedDate],Isnull(OutStandingAmount,0) AS [OutStandingAmount],
                            Address,CNIC,Grade,EmployeeType,NTNNo
                            From Trans_Salaries SAL
                            Inner Join Emp_Def Emp on SAL.EmpID=Emp.EmpCode
                            Where SalaryMonth='" + MonthID + "' and SalaryYear='" + SalaryYear + "' And EmpCode='"+EmpID+"'";
                }
                dt = GetDataTable(qry,sqlconn);
                return dt;
            }
            catch (Exception ex)
            {

                MessageBox.Show("Error in DAL_RptRecovery.RptSalary" + ex.Message, DataAccess.ProjectName, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return dt = null;
            }
        }

        public DataTable Rpt_NewActivity(string FromDate)
        {
            DataTable dt = new DataTable();
            try
            {
                SqlConnection sqlConnection = new SqlConnection(this.ConnString);
//                string qry = @"Select 'Issuence' AS [TransType],ClientName,Convert(varchar(11),IM.IssueDate,106) AS [IssueDate],'1900-01-01' AS [RecoveryDate],'1900-01-01' AS [ClearanceDate],NetAmount As [DisbursmentAmount], 0 AS [RecoveryAmount],0 AS [ClearanceAmount]   From Trans_AccountHistory ACH
//                               Inner Join Trans_IssueM IM on ACH.TableID=im.IssueID 
//                               Inner Join Trans_Clients CL on IM.ClientID=CL.ClientID
//                               where Convert(varchar(11),Im.IssueDate,121)='" + FromDate + @"'
//                               union all
//                               Select 'Recovery' As [TransType],ClientName,'1900-01-01' AS [IssueDate],Convert(varchar(11),rec.RecoveryDate,106) As [RecoveryDate],'1900-01-01' AS [ClearanceDate],0 As DisbursmentAmount,RecoveryAmount,0 AS [ClearanceAmount] From Trans_Recovery Rec
//                               Inner Join Trans_IssueM IM on REc.IssueID=Im.IssueID
//                               Inner Join Trans_Clients CL on IM.ClientID=CL.ClientID
//                               Where Convert(varchar(11),rec.RecoveryDate,121)='" + FromDate + @"'
//                               Union All
//                               select 'Clearance' AS [TransType],HeadDetailName,'1900-01-01' AS [IssueDate],'1900-01-01' AS [RecoveryDate],Convert(varchar(11),ClearanceDate,106) AS [ClearanceDate],0 AS [DisbursmentAmount],0 AS [RecoveryAmount],SUM(Amount) AS [ClearanceAmount] From Trans_ClearanceM CM
//                               Inner Join Trans_ClearanceD CD on CM.ClearanceID=CD.ClearanceID
//                               Inner join AccountHeadDetails AHD on CD.HeadDetailID=AHD.HeadDetailID
//                               Where CONVERT(varchar(11),ClearanceDate,121)='" + FromDate + @"'
//                               Group by HeadDetailName,ClearanceDate";
                //Commented on 2016-12-15 on request of Saghar Saab
//                string qry = @"Select 'Issuence' AS [TransType],CL.ClientCode,ClientName,EmpName,Isnull(Manual_Bill_No,'') AS [SlipNo] ,Convert(varchar(11),IM.IssueDate,106) AS [IssueDate],'1900-01-01' AS [RecoveryDate],'1900-01-01' AS [ClearanceDate],NetAmount As [DisbursmentAmount], 0 AS [RecoveryAmount],0 AS [ClearanceAmount]   From Trans_AccountHistory ACH
//                               Inner Join Trans_IssueM IM on ACH.TableID=im.IssueID 
//                               Inner Join Trans_Clients CL on IM.ClientID=CL.ClientID
//                               Inner jOIn Emp_Def EM on Cl.EmpCode=EM.EmpCode
//                               where Convert(varchar(11),Im.IssueDate,121)='" + FromDate + @"' and Installment=0
//                               union all
//                               Select 'Recovery' As [TransType],CL.ClientCode,ClientName,EMpName,Isnull(Convert(varchar,Rec.Remarks),0) AS [RecoveryID],'1900-01-01' AS [IssueDate],Convert(varchar(11),rec.RecoveryDate,106) As [RecoveryDate],'1900-01-01' AS [ClearanceDate],0 As DisbursmentAmount,(isnull(RecoveryAmount,0)-isnull(disamount,0)) As [RecoveryAmount],0 AS [ClearanceAmount] From Trans_Recovery Rec
//                               Inner Join Trans_IssueM IM on REc.IssueID=Im.IssueID
//                               Inner Join Trans_Clients CL on IM.ClientID=CL.ClientID
//                               inner Join Trans_AccountHistory ACT on Rec.RecoveryID=ACT.RecoveryID
//                               Inner jOIn Emp_Def EM on Cl.EmpCode=EM.EmpCode
//                               Where Convert(varchar(11),rec.RecoveryDate,121)='" + FromDate + @"'
//                               Union All
//                               select 'Clearance' AS [TransType],'0000' AS [ClientCode],HeadDetailName,'----' As EmpName,'0' AS [RecoveryID],'1900-01-01' AS [IssueDate],'1900-01-01' AS [RecoveryDate],Convert(varchar(11),ClearanceDate,106) AS [ClearanceDate],0 AS [DisbursmentAmount],0 AS [RecoveryAmount],SUM(Amount) AS [ClearanceAmount] From Trans_ClearanceM CM
//                               Inner Join Trans_ClearanceD CD on CM.ClearanceID=CD.ClearanceID
//                               Inner join AccountHeadDetails AHD on CD.HeadDetailID=AHD.HeadDetailID
//                                Where CONVERT(varchar(11),ClearanceDate,121)='" + FromDate + @"'
//                               Group by HeadDetailName,ClearanceDate
//                               Union All 
//                               Select TransType,ClientCode,HeadDetailName,'----' As EmpName,RecoveryID,IssueDate,RecoveryDate,ClearanceDate,DisbursmentAmount,RecoveryAmount,
//                               SUM(ClearanceAmount) As [ClearanceAmount] From (
//                               Select 'Clearance' AS [TransType],'0000' As [ClientCode], 'PURCHASE OF TODAY' AS [HeadDetailName],0 AS [RecoveryID],'1900-01-01' AS [IssueDate],
//                               '1900-01-01' As [RecoveryDate],CONVERT(varchar(11),Prdate,106) As [ClearanceDate],0 As [DisbursmentAmount], 0 As [RecoveryAmount],
//                               isnull(sum(PD.PrQty*PD.PurchasePrice),0)  As [ClearanceAmount]
//                               From Trans_PurchaseM PM
//                               Inner Join Trans_PurchaseD PD on PM.PrID=PD.PrID
//                               Where Convert(varchar(11),PrDate,121) ='" + FromDate + @"'
//                               Group by PrDate )AA
//                               group by TransType,ClientCode,HeadDetailName,RecoveryID,IssueDate,RecoveryDate,ClearanceDate,DisbursmentAmount,RecoveryAmount";
                //New Code 2016-12-15
//                string qry = @"Select 'Issuence' AS [TransType],CL.ClientCode,ClientName,EmpName,Isnull(Manual_Bill_No,'') AS [SlipNo] ,Convert(varchar(11),IM.IssueDate,106) AS [IssueDate],'1900-01-01' AS [RecoveryDate],'1900-01-01' AS [ClearanceDate],NetAmount As [DisbursmentAmount], 0 AS [RecoveryAmount],0 AS [ClearanceAmount],0 As [NewRegistration]   From Trans_AccountHistory ACH
//                               Inner Join Trans_IssueM IM on ACH.TableID=im.IssueID 
//                               Inner Join Trans_Clients CL on IM.ClientID=CL.ClientID
//                               Inner jOIn Emp_Def EM on Cl.EmpCode=EM.EmpCode
//                               where Convert(varchar(11),Im.IssueDate,121)='" + FromDate + @"' and Installment=0
//                               union all
//                               Select 'Recovery' As [TransType],CL.ClientCode,ClientName,EMpName,Isnull(Convert(varchar,Rec.Remarks),0) AS [RecoveryID],'1900-01-01' AS [IssueDate],
//Convert(varchar(11),rec.RecoveryDate,106) As [RecoveryDate],'1900-01-01' AS [ClearanceDate],0 As DisbursmentAmount,
//(isnull(RecoveryAmount,0)-isnull(disamount,0)) As [RecoveryAmount],0 AS [ClearanceAmount],
//isnull((Select RegAmount From Registration Where ClientID =Rec.ClientID and RegDate='"+FromDate+ @"'),0) As [NewRegistration]
//From Trans_Recovery Rec
//                               Inner Join Trans_IssueM IM on REc.IssueID=Im.IssueID
//                               Inner Join Trans_Clients CL on IM.ClientID=CL.ClientID
//                               inner Join Trans_AccountHistory ACT on Rec.RecoveryID=ACT.RecoveryID
//                               Inner jOIn Emp_Def EM on Cl.EmpCode=EM.EmpCode
//                               Where Convert(varchar(11),rec.RecoveryDate,121)='" + FromDate + @"'
//                               Union All
//                               select 'Clearance' AS [TransType],'0000' AS [ClientCode],HeadDetailName,'----' As EmpName,'0' AS [RecoveryID],'1900-01-01' AS [IssueDate],'1900-01-01' AS [RecoveryDate],Convert(varchar(11),ClearanceDate,106) AS [ClearanceDate],0 AS [DisbursmentAmount],0 AS [RecoveryAmount],SUM(Amount) AS [ClearanceAmount],0 As [NewRegistration] From Trans_ClearanceM CM
//                               Inner Join Trans_ClearanceD CD on CM.ClearanceID=CD.ClearanceID
//                               Inner join AccountHeadDetails AHD on CD.HeadDetailID=AHD.HeadDetailID
//                                Where CONVERT(varchar(11),ClearanceDate,121)='" + FromDate + @"'
//                               Group by HeadDetailName,ClearanceDate
//                               Union All 
//                               Select TransType,ClientCode,HeadDetailName,'----' As EmpName,RecoveryID,IssueDate,RecoveryDate,ClearanceDate,DisbursmentAmount,RecoveryAmount,
//                               SUM(ClearanceAmount) As [ClearanceAmount],0 As [NewRegistration] From (
//                               Select 'Clearance' AS [TransType],'0000' As [ClientCode], 'PURCHASE OF TODAY' AS [HeadDetailName],0 AS [RecoveryID],'1900-01-01' AS [IssueDate],
//                               '1900-01-01' As [RecoveryDate],CONVERT(varchar(11),Prdate,106) As [ClearanceDate],0 As [DisbursmentAmount], 0 As [RecoveryAmount],
//                               isnull(sum(PD.PrQty*PD.PurchasePrice),0)  As [ClearanceAmount]
//                               From Trans_PurchaseM PM
//                               Inner Join Trans_PurchaseD PD on PM.PrID=PD.PrID
//                               Where Convert(varchar(11),PrDate,121) ='" + FromDate + @"'
//                               Group by PrDate )AA
//                               group by TransType,ClientCode,HeadDetailName,RecoveryID,IssueDate,RecoveryDate,ClearanceDate,DisbursmentAmount,RecoveryAmount";                
                //new Code on 2016-12-19
                //Commented on 2017-02-05
//                string qry = @"Select 'Issuence' AS [TransType],CL.ClientCode,ClientName,EmpName,Isnull(Manual_Bill_No,'') AS [SlipNo] ,Convert(varchar(11),IM.IssueDate,106) AS [IssueDate],'1900-01-01' AS [RecoveryDate],'1900-01-01' AS [ClearanceDate],NetAmount As [DisbursmentAmount], 0 AS [RecoveryAmount],0 AS [ClearanceAmount],0 As [NewRegistration]   From Trans_AccountHistory ACH
//                               Inner Join Trans_IssueM IM on ACH.TableID=im.IssueID 
//                               Inner Join Trans_Clients CL on IM.ClientID=CL.ClientID
//                               Inner jOIn Emp_Def EM on Cl.EmpCode=EM.EmpCode
//                               where Convert(varchar(11),Im.IssueDate,121)='" + FromDate + @"' and Installment=0
//                               union all
//                               Select 'Recovery' As [TransType],CL.ClientCode,ClientName,EMpName,Isnull(Convert(varchar,Rec.Remarks),0) AS [RecoveryID],'1900-01-01' AS [IssueDate],
//Convert(varchar(11),rec.RecoveryDate,106) As [RecoveryDate],'1900-01-01' AS [ClearanceDate],0 As DisbursmentAmount,
//(isnull(RecoveryAmount,0)-isnull(disamount,0)) As [RecoveryAmount],0 AS [ClearanceAmount],
//isnull((Select RegAmount From Registration Where ClientID =Rec.ClientID and RegDate='" + FromDate + @"'),0) As [NewRegistration]
//From Trans_Recovery Rec
//                               Inner Join Trans_IssueM IM on REc.IssueID=Im.IssueID
//                               Inner Join Trans_Clients CL on IM.ClientID=CL.ClientID
//                               inner Join Trans_AccountHistory ACT on Rec.RecoveryID=ACT.RecoveryID
//                               Inner jOIn Emp_Def EM on Cl.EmpCode=EM.EmpCode
//                               Where Convert(varchar(11),rec.RecoveryDate,121)='" + FromDate + @"'
//                               Union All
//                               select 'Clearance' AS [TransType],'0000' AS [ClientCode],HeadDetailName,'----' As EmpName,'0' AS [RecoveryID],'1900-01-01' AS [IssueDate],'1900-01-01' AS [RecoveryDate],Convert(varchar(11),ClearanceDate,106) AS [ClearanceDate],0 AS [DisbursmentAmount],0 AS [RecoveryAmount],SUM(Amount) AS [ClearanceAmount],0 As [NewRegistration] From Trans_ClearanceM CM
//                               Inner Join Trans_ClearanceD CD on CM.ClearanceID=CD.ClearanceID
//                               Inner join AccountHeadDetails AHD on CD.HeadDetailID=AHD.HeadDetailID
//                                Where CONVERT(varchar(11),ClearanceDate,121)='" + FromDate + @"'
//                               Group by HeadDetailName,ClearanceDate
//                               Union All 
//                               Select TransType,ClientCode,HeadDetailName,'----' As EmpName,RecoveryID,IssueDate,RecoveryDate,ClearanceDate,DisbursmentAmount,RecoveryAmount,
//                               SUM(ClearanceAmount) As [ClearanceAmount],0 As [NewRegistration] From (
//                               Select 'Clearance' AS [TransType],'0000' As [ClientCode], 'PURCHASE OF TODAY' AS [HeadDetailName],0 AS [RecoveryID],'1900-01-01' AS [IssueDate],
//                               '1900-01-01' As [RecoveryDate],CONVERT(varchar(11),Prdate,106) As [ClearanceDate],0 As [DisbursmentAmount], 0 As [RecoveryAmount],
//                               isnull(sum(PD.PrQty*PD.PurchasePrice),0)  As [ClearanceAmount]
//                               From Trans_PurchaseM PM
//                               Inner Join Trans_PurchaseD PD on PM.PrID=PD.PrID
//                               Where Convert(varchar(11),PrDate,121) ='" + FromDate + @"'
//                               Group by PrDate )AA
//                               group by TransType,ClientCode,HeadDetailName,RecoveryID,IssueDate,RecoveryDate,ClearanceDate,DisbursmentAmount,RecoveryAmount
//                               Union all
//							   Select 'New Registration' As [TransType],'----' As ClientCode,ClientName,EmpName,A.SlipNo,Convert(varchar(11),RegDate,106) As [IssueDate],'1900-01-01' As [RecoveryDate],'1900-01-01' As [ClearanceDate],
//							   0 As [DisbursementAmount], 0 As [RecoveryAmount],0 AS [ClearanceAmount],isnull(sum(a.RegAmount),0) As [NewRegistration] From Registration A
//							   Inner JOin Trans_Clients B on A.CLientID=B.ClientID
//							   Inner Join Emp_Def C on B.EmpCode=C.EmpCode
//							   Where RegDate ='"+FromDate+@"'
//							   group by ClientCode,ClientName,EmpName,A.SlipNo,RegDate
//							   Union All
//							   Select 'Direct Sales' As [TransType], '----' As [ClientCode],DRS_Name As [ClientName],'' As EmpName, SlipNo, Convert(varchar(11),DRS_Date,106) As [IssueDate],'1900-01-01' As [RecoveryDate],
//							   '1900-01-01' As [ClearanceDate],isnull(sum((SalesPrice*Qty)),0)  As [DisbursementDate], 0 As [RecoveryAmount], 0 As [ClearanceAmount],0 As [NewRegistration]  From Trans_DRSM A
//							   Inner JOin Trans_DRSD B on A.DRS_ID=B.DRS_ID
//                               Where DRS_Date='"+FromDate+@"'
//							   Group By DRS_Name,DRS_Date,SlipNo
//							   --union all
//							   --Select 'Purchase' As [TransType],'0000' As [ClientCode],VendorName As [ClientName],'' As [EmpName],'' As SlipNo,Convert(varchar(11),PRDate,106) As [IssueDate],'1900-01-01' As [RecoveryDate],
//							   --'1900-01-01' As [ClearanceDate], isnull(sum(PrQty),0) * isnull(sum(PurchasePrice),0) As [DisbursementAmount], 0 As [RecoveryAmount],0 As [ClearanceAmount], 0 As [NewRegistration]  From Trans_PurchaseM A
//							   --Inner Join Trans_PurchaseD B on A.PrID=B.PrID
//                               --Where PRDate ='2016-09-01'
//							   --Group by VendorName,PrDate";       
                //ENd of Commented on 2017-02-05

                //New LInes 2017-02-05
                string qry = @"Select 'Issuence' AS [TransType],CL.ClientCode,ClientName,EmpName,Isnull(Manual_Bill_No,'') AS [SlipNo] ,Convert(varchar(11),IM.IssueDate,106) AS [IssueDate],'1900-01-01' AS [RecoveryDate],'1900-01-01' AS [ClearanceDate],NetAmount As [DisbursmentAmount], 0 AS [RecoveryAmount],0 AS [ClearanceAmount],0 As [NewRegistration]   From Trans_AccountHistory ACH
                               Inner Join Trans_IssueM IM on ACH.TableID=im.IssueID 
                               Inner Join Trans_Clients CL on IM.ClientID=CL.ClientID
                               Inner jOIn Emp_Def EM on Cl.EmpCode=EM.EmpCode
                               where Convert(varchar(11),Im.IssueDate,121)='" + FromDate + @"' and Installment=0
                               union all
                               Select 'Recovery' As [TransType],CL.ClientCode,ClientName,EMpName,Isnull(Convert(varchar,Rec.Remarks),0) AS [RecoveryID],'1900-01-01' AS [IssueDate],
Convert(varchar(11),rec.RecoveryDate,106) As [RecoveryDate],'1900-01-01' AS [ClearanceDate],0 As DisbursmentAmount,
(isnull(RecoveryAmount,0)-isnull(disamount,0)) As [RecoveryAmount],0 AS [ClearanceAmount],
isnull((Select RegAmount From Registration Where ClientID =Rec.ClientID and RegDate='" + FromDate + @"'),0) As [NewRegistration]
From Trans_Recovery Rec
                               Inner Join Trans_IssueM IM on REc.IssueID=Im.IssueID
                               Inner Join Trans_Clients CL on IM.ClientID=CL.ClientID
                               inner Join Trans_AccountHistory ACT on Rec.RecoveryID=ACT.RecoveryID
                               Inner jOIn Emp_Def EM on Cl.EmpCode=EM.EmpCode
                               Where Convert(varchar(11),rec.RecoveryDate,121)='" + FromDate + @"'
                               Union All
                               select 'Clearance' AS [TransType],'0000' AS [ClientCode],HeadDetailName,'----' As EmpName,'0' AS [RecoveryID],'1900-01-01' AS [IssueDate],'1900-01-01' AS [RecoveryDate],Convert(varchar(11),ClearanceDate,106) AS [ClearanceDate],0 AS [DisbursmentAmount],0 AS [RecoveryAmount],SUM(Amount) AS [ClearanceAmount],0 As [NewRegistration] From Trans_ClearanceM CM
                               Inner Join Trans_ClearanceD CD on CM.ClearanceID=CD.ClearanceID
                               Inner join AccountHeadDetails AHD on CD.HeadDetailID=AHD.HeadDetailID
                                Where CONVERT(varchar(11),ClearanceDate,121)='" + FromDate + @"'
                               Group by HeadDetailName,ClearanceDate
                               Union All 
                               Select TransType,ClientCode,HeadDetailName,'----' As EmpName,RecoveryID,IssueDate,RecoveryDate,ClearanceDate,DisbursmentAmount,RecoveryAmount,
                               SUM(ClearanceAmount) As [ClearanceAmount],0 As [NewRegistration] From (
                               Select 'Clearance' AS [TransType],'0000' As [ClientCode], 'PURCHASE OF TODAY' AS [HeadDetailName],0 AS [RecoveryID],'1900-01-01' AS [IssueDate],
                               '1900-01-01' As [RecoveryDate],CONVERT(varchar(11),Prdate,106) As [ClearanceDate],0 As [DisbursmentAmount], 0 As [RecoveryAmount],
                               isnull(sum(PD.PrQty*PD.PurchasePrice),0)  As [ClearanceAmount]
                               From Trans_PurchaseM PM
                               Inner Join Trans_PurchaseD PD on PM.PrID=PD.PrID
                               Where Convert(varchar(11),PrDate,121) ='" + FromDate + @"'
                               Group by PrDate )AA
                               group by TransType,ClientCode,HeadDetailName,RecoveryID,IssueDate,RecoveryDate,ClearanceDate,DisbursmentAmount,RecoveryAmount
                               Union all
							   Select * From (
Select 'New Registration' As [TransType],ClientCode,ClientName,EmpName,A.SlipNo,Convert(varchar(11),RegDate,106) As [IssueDate],'1900-01-01' As [RecoveryDate],'1900-01-01' As [ClearanceDate],
							   0 As [DisbursementAmount], 0 As [RecoveryAmount],0 AS [ClearanceAmount],isnull(sum(a.RegAmount),0) As [NewRegistration] From Registration A
							   Inner JOin Trans_Clients B on A.CLientID=B.ClientID
							   Inner Join Emp_Def C on B.EmpCode=C.EmpCode
							   Where RegDate ='" + FromDate + @"'
							   group by ClientCode,ClientName,EmpName,A.SlipNo,RegDate

Union All
Select 'New Registration' As [TransType],ClientCode,ClientName,EmpName,A.SlipNo,Convert(varchar(11),RegistrationDate,106) As [IssueDate],'1900-01-01' As [RecoveryDate],'1900-01-01' As [ClearanceDate],
							   0 As [DisbursementAmount], 0 As [RecoveryAmount],0 AS [ClearanceAmount],isnull(sum(Convert(int,A.Registration_Fee)),0) As [NewRegistration] From Trans_Clients A
							   Inner Join Emp_Def B on B.EmpCode=A.EmpCode
							   Where Convert(varchar(11),RegistrationDate,121) ='" +FromDate+@"'
							   group by ClientCode,ClientName,EmpName,A.SlipNo,RegistrationDate)AA
							   Union All
							   Select 'Direct Sales' As [TransType], '----' As [ClientCode],DRS_Name As [ClientName],'' As EmpName, SlipNo, Convert(varchar(11),DRS_Date,106) As [IssueDate],'1900-01-01' As [RecoveryDate],
							   '1900-01-01' As [ClearanceDate],isnull(sum((SalesPrice*Qty)),0)  As [DisbursementDate], 0 As [RecoveryAmount], 0 As [ClearanceAmount],0 As [NewRegistration]  From Trans_DRSM A
							   Inner JOin Trans_DRSD B on A.DRS_ID=B.DRS_ID
                               Where DRS_Date='" + FromDate + @"'
							   Group By DRS_Name,DRS_Date,SlipNo
							   --union all
							   --Select 'Purchase' As [TransType],'0000' As [ClientCode],VendorName As [ClientName],'' As [EmpName],'' As SlipNo,Convert(varchar(11),PRDate,106) As [IssueDate],'1900-01-01' As [RecoveryDate],
							   --'1900-01-01' As [ClearanceDate], isnull(sum(PrQty),0) * isnull(sum(PurchasePrice),0) As [DisbursementAmount], 0 As [RecoveryAmount],0 As [ClearanceAmount], 0 As [NewRegistration]  From Trans_PurchaseM A
							   --Inner Join Trans_PurchaseD B on A.PrID=B.PrID
                               --Where PRDate ='2016-09-01'
							   --Group by VendorName,PrDate";
                //end of New Lines 2017-02-05

//                string qry = @"Select 'Issuence' AS [TransType],CL.ClientCode,ClientName,Isnull(Manual_Bill_No,'') AS [SlipNo] ,Convert(varchar(11),IM.IssueDate,106) AS [IssueDate],'1900-01-01' AS [RecoveryDate],'1900-01-01' AS [ClearanceDate],NetAmount As [DisbursmentAmount], 0 AS [RecoveryAmount],0 AS [ClearanceAmount]   From Trans_AccountHistory ACH
//                               Inner Join Trans_IssueM IM on ACH.TableID=im.IssueID 
//                               Inner Join Trans_Clients CL on IM.ClientID=CL.ClientID
//                               where Convert(varchar(11),Im.IssueDate,121)='" + FromDate + @"' and Installment=0
//                               union all
//                               Select 'Recovery' As [TransType],CL.ClientCode,ClientName,Isnull(Convert(varchar,Rec.Remarks),0) AS [RecoveryID],'1900-01-01' AS [IssueDate],Convert(varchar(11),rec.RecoveryDate,106) As [RecoveryDate],'1900-01-01' AS [ClearanceDate],0 As DisbursmentAmount,RecoveryAmount,0 AS [ClearanceAmount] From Trans_Recovery Rec
//                               Inner Join Trans_IssueM IM on REc.IssueID=Im.IssueID
//                               Inner Join Trans_Clients CL on IM.ClientID=CL.ClientID
//                               inner Join Trans_AccountHistory ACT on Rec.RecoveryID=ACT.RecoveryID
//                               Where Convert(varchar(11),rec.RecoveryDate,121)='" + FromDate + @"'
//                               Union All
//                               select 'Clearance' AS [TransType],'0000' AS [ClientCode],HeadDetailName,'0' AS [RecoveryID],'1900-01-01' AS [IssueDate],'1900-01-01' AS [RecoveryDate],Convert(varchar(11),ClearanceDate,106) AS [ClearanceDate],0 AS [DisbursmentAmount],0 AS [RecoveryAmount],SUM(Amount) AS [ClearanceAmount] From Trans_ClearanceM CM
//                               Inner Join Trans_ClearanceD CD on CM.ClearanceID=CD.ClearanceID
//                               Inner join AccountHeadDetails AHD on CD.HeadDetailID=AHD.HeadDetailID
//                               Where CONVERT(varchar(11),ClearanceDate,121)='" + FromDate + @"'
//                               Group by HeadDetailName,ClearanceDate
//                               Union All 
//                               Select TransType,ClientCode,HeadDetailName,RecoveryID,IssueDate,RecoveryDate,ClearanceDate,DisbursmentAmount,(Isnull(RecoveryAmount,0)-isnull(DisAmount,0)) AS [RecoveryAmount],
//                               SUM(ClearanceAmount) As [ClearanceAmount] From (
//                               Select 'Clearance' AS [TransType],'0000' As [ClientCode], 'PURCHASE OF TODAY' AS [HeadDetailName],0 AS [RecoveryID],'1900-01-01' AS [IssueDate],
//                               '1900-01-01' As [RecoveryDate],CONVERT(varchar(11),Prdate,106) As [ClearanceDate],0 As [DisbursmentAmount], 0 As [RecoveryAmount],
//                               isnull(sum(PD.PrQty*PD.PurchasePrice),0)  As [ClearanceAmount]
//                               From Trans_PurchaseM PM
//                               Inner Join Trans_PurchaseD PD on PM.PrID=PD.PrID
//                               Where Convert(varchar(11),PrDate,121) ='" + FromDate + @"'
//                               Group by PrDate )AA
//                               group by TransType,ClientCode,HeadDetailName,RecoveryID,IssueDate,RecoveryDate,ClearanceDate,DisbursmentAmount,RecoveryAmount";
                


                dt = this.GetdataTable(new SqlCommand(qry, sqlConnection), sqlConnection);
            }
            catch (Exception ex)
            {
                int num = (int)MessageBox.Show(ex.Message, DataAccess.ProjectName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
            return dt;
        }

        public DataTable RptDiscountClientWise(string FromDate,string ToDate)
        {
            DataTable dt = new DataTable();
            try
            {
                SqlConnection sqlconn = new SqlConnection(ConnString);
//                string qry = @"Select ClientCode,ClientName,R.IssueID,Convert(varchar,R.IssueDate,106) As [IssueDate],Convert(varchar,R.dueDate,106)                                AS [DueDate],Convert(varchar,RecoveryDate,106) As [RecoveryDate],RecoveryAmount,DisAmount,(RecoveryAmount-DisAmount) 
//                               AS [ReceivedAmount] From Trans_issueM I
//                               Inner Join Trans_Recovery R on I.IssueID=I.IssueID and R.IssueDate=I.IssueDate
//                               Inner Join Trans_clients C on I.ClientID=C.ClientID
//                               where DisAmount is not null and DisAmount !=0 
//                               order by 2";
                string qry = @"Select ClientCode,ClientName,R.IssueID,Convert(varchar,R.IssueDate,106) As [IssueDate],Convert(varchar,R.dueDate,106) AS [DueDate],
                               Convert(varchar,RecoveryDate,106) As [RecoveryDate],RecoveryAmount,Disamount As DisAmount,(RecoveryAmount-DisAmount) 
                               AS [ReceivedAmount],RecoveryDate as SortDate From Trans_issueM I
                               Inner Join Trans_Recovery R on I.IssueID=I.IssueID and R.IssueDate=I.IssueDate
                               Inner Join Trans_clients C on I.ClientID=C.ClientID
                               where DisAmount is not null and DisAmount !=0 and Convert(varchar(11),Recoverydate,121) between '"+FromDate+"' and '"+ToDate+@"'  
                               order by RecoveryDate";
                SqlCommand sqlcomm = new SqlCommand(qry, sqlconn);
                dt = GetdataTable(sqlcomm, sqlconn);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, DataAccess.ProjectName, MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            return dt;
        }

        public DataTable SalaryTransferLetterToBank(string SalaryMonth, string SalaryYear)
        {
            DataTable dt = new DataTable();
            try
            {
                SqlConnection sqlconn = new SqlConnection(ConnString);
                string qry = @"Select EmpCode,EmpName,Isnull(AccountNo,0) As [AccountNo],A.Salary From Trans_Salaries A
                               Inner Join Emp_Def B on A.EmpID=B.EmpCode
                               Where SalaryMonth='"+SalaryMonth+"' and SalaryYear='"+SalaryYear+"'";
                SqlCommand sqlcomm = new SqlCommand(qry, sqlconn);
                dt = GetdataTable(sqlcomm, sqlconn);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, DataAccess.ProjectName, MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            return dt;
        }

        public DataTable CashReceiptReport(int RecoveryID)
        {
            DataTable dt = new DataTable();
            try
            {
                SqlConnection sqlconn = new SqlConnection(ConnString);
                string qry = @"Select BB.IssueDate,BB.DueDate,BB.RecoveryDate,BB.ClientCOde,BB.Clientname,BB.TotalIssueAmount,BB.RecoveryAmount,BB.DisAmount,BB.Remarks From (
                               Select A.RecoveryID,A.IssueID,Convert(varchar(11),A.IssueDate,106) AS [IssueDate],Convert(varchar(11),A.DueDate,106) AS [DueDate],
                               Convert(varchar(11),A.RecoveryDate,106) AS [RecoveryDate],ClientCode,ClientName,
                               isnull((Select isnull(sum(totalamount),0) As [Total] From Trans_issueM AA
                               inner Join Trans_IssueD B on A.IssueID=B.IssueID
                               Where A.ClientID=A.ClientID and AA.IssueID=A.IssueID and AA.IssueDate=A.IssueDate and AA.DueDate=A.DueDate
                               ),0) As [TotalIssueAmount],
                               RecoveryAmount,A.DisAmount,A.Remarks
                               From Trans_Recovery A
                               Inner Join Trans_IssueM B on A.IssueID=B.IssueID
                               Inner join Trans_Clients C on A.ClientID=C.ClientID
                               Where A.RecoveryID='"+RecoveryID+"')BB";
                SqlCommand sqlcomm = new SqlCommand(qry, sqlconn);
                dt = GetdataTable(sqlcomm, sqlconn);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, DataAccess.ProjectName, MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            return dt;
        }

        public DataTable RptEmpWiseClientDisbursement(bool IsAll,long EmpId,string FromDate,string ToDate)
        {
            DataTable dt = new DataTable();
            try
            {
                SqlConnection sqlconn = new SqlConnection(ConnString);
                string qry =string.Empty;
                if (IsAll)
                {
                    qry = @"Select ClientCode,ClientName,Convert(varchar(11),IssueDate,106) AS [DisbursementDate],EmpName,Isnull(sum(totalamount),0) As [Amount],IssueDate As SortDate From Trans_IssueM A
                               Inner Join Trans_IssueD B on A.IssueID=B.IssueID
                               Inner JOIn Trans_Clients C on A.ClientID=C.ClientID
                               Inner Join Emp_Def D on C.EmpCode=D.EmpCode
                               Where Convert(varchar(11),IssueDate,121) between '"+FromDate+@"' and '"+ToDate+@"'
                               Group bY ClientCode,ClientName,IssueDate,EmpName
                               Order by SortDate Desc";
                }
                else
                {
                    qry = @"Select ClientCode,ClientName,Convert(varchar(11),IssueDate,106) AS [DisbursementDate],EmpName,Isnull(sum(totalamount),0) As [Amount],IssueDate as SortDate From Trans_IssueM A
                               Inner Join Trans_IssueD B on A.IssueID=B.IssueID
                               Inner JOIn Trans_Clients C on A.ClientID=C.ClientID
                               Inner Join Emp_Def D on C.EmpCode=D.EmpCode
                               Where c.EmpCode='" + EmpId + @"' and Convert(varchar(11),IssueDate,121) between '" + FromDate + @"' and '" + ToDate + @"'
                               Group bY ClientCode,ClientName,IssueDate,EmpName
                               Order by IssueDate Desc";
                }
                
                SqlCommand sqlcomm = new SqlCommand(qry, sqlconn);
                dt = GetdataTable(sqlcomm, sqlconn);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, DataAccess.ProjectName, MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            return dt;
        }

        public DataTable RptEmpWiseDisbursementSummary(string FromDate,string ToDate)
        {
            DataTable dt = new DataTable();
            SqlConnection sqlconn = new SqlConnection(ConnString);
            string qry = string.Empty;
            qry = @"Select b.EmpCode,EMpName,Aa.ClientID,count(Aa.ClientID) As [NoOfCycles],sum(Actual) As [Actual] from (
                    Select IssueID,C.IssueDate,C.ClientID,Disbursement,Recovery,Balance,Actual
                    From (
                    Select IssueID,Convert(varchar(11),IssueDate,121) As [IssueDate],ClientID,Disbursement,Recovery,Balance,Actual From (
                    Select IssueID,IssueDate,ClientID,Convert(int,(Disbursement+SC)) As [Disbursement],Disbursement As [Actual],
                    Recovery,Convert(int,(Disbursement+SC))-Recovery As [Balance]  From (
                    Select A.IssueID,A.IssueDate,clientID,sum(TotalAMount) As [Disbursement],(sum(totalAmount)*0.05) SC,
                    isnull((select sum(installment) From Trans_AccountHistory Where TableID=A.IssueID and ClientID=A.ClientID),0) As [Recovery]
                    From trans_IssueM A
                    INner JOin Trans_IssueD B on A.IssueID=B.IssueID
                    Group by A.IssueID,A.IssueDate,clientID )A)B
                    Where Balance=0)C
                    Inner Join Trans_AccountHistory A on C.ClientID=A.ClientID and c.IssueID=a.TableID and RecoveryID is not null 
                    Where a.ModifiedDate between '"+FromDate+"' and '"+ToDate+@"'
                    group by IssueID,C.IssueDate,C.ClientID,Disbursement,Recovery,Balance,Actual)AA
                    Inner Join Trans_Clients AB on AA.ClientID=AB.ClientID
                    Inner JOin Emp_Def B on AB.EmpCode=B.EmpCode
                    Group By AA.ClientID,EmpName,b.EmpCode
                    having count(AA.clientID)=1
                    order by B.EmpCode";
            SqlCommand sqlcomm = new SqlCommand(qry, sqlconn);
            dt = GetdataTable(sqlcomm, sqlconn);
            double sum = 0.0;
            //filter No of Employees
            DataTable dtTempEmployee = dt;
            DataView dvEmpName = new DataView(dtTempEmployee);
            dtTempEmployee = dvEmpName.ToTable(true, "EmpCode", "EmpName");
            DataTable dtorg = new DataTable();
            dtorg.Columns.Add("EmpCode");
            dtorg.Columns.Add("EmpName");
            dtorg.Columns.Add("EmpCount");
            dtorg.Columns.Add("Amount");
            DataRow dr = dtorg.NewRow();

            foreach (DataRow drFinal in dtTempEmployee.Rows)
            {
                DataView dv = new DataView(dt);
                dv.RowFilter = "EmpCode='"+drFinal["EmpCode"]+"'";
                int CountofEmployees = dv.Count;
                DataTable dtsum = dv.ToTable();
                sum = Convert.ToDouble(dtsum.Compute("sum(Actual)", ""));
                dr = dtorg.NewRow();
                dr["EmpCode"] = drFinal["EmpCode"].ToString();
                dr["EmpName"] = drFinal["EmpName"].ToString();
                dr["EmpCount"] = CountofEmployees;
                dr["Amount"] = sum;
                dtorg.Rows.Add(dr);
            }
            return dtorg;
        }

        public DataTable GetCashbalanceForCashReport(string FromDate)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("BalanceDate");
            dt.Columns.Add("OpeningBalance");
            dt.Columns.Add("Transactions");
            dt.Columns.Add("Closing");

            DateTime dtStart = Convert.ToDateTime("2017-02-01");
            DateTime dtEnd = Convert.ToDateTime(FromDate);
            int DiffDays = (dtEnd - dtStart).Days;
            DataRow dr = dt.NewRow();
            dr["BalanceDate"] = "2017-02-01";
            dr["OpeningBalance"] = 3170;
            dr["Transactions"] = 79017;
            dr["Closing"] = 82817;
            dt.Rows.Add(dr);
            DataTable dtDebit = new DataTable();
            DataTable dtCredit = new DataTable();

            List<DateTime> lstDates = GetDates(dtStart, dtEnd);

            int CurrOpening = 0;
            for (int u = 0; u < lstDates.Count; u++)
            {
                dr = dt.NewRow();

                dtDebit = GetDebitValuesForCashBalance(GetSpecificDateFormat(lstDates[u].Date));
                dtCredit = GetCreditValuesForCashBalance(GetSpecificDateFormat(lstDates[u].Date));
                int DebitValues = Convert.ToInt32(dtDebit.Rows[0]["Debit"]);
                int CreditValues = Convert.ToInt32(dtCredit.Rows[0]["Credit"]);
                int Diff = DebitValues - CreditValues;
                int q = dt.Rows.Count;
                
                if (q == 1)
                {
                    CurrOpening = 82817;
                }
                else
                {
                    CurrOpening = Convert.ToInt32(dt.Rows[q - 1]["Closing"]);

                }
                dr = dt.NewRow();
                int CurrentClosing = CurrOpening + Diff;
                dr["BalanceDate"] = GetSpecificDateFormat(lstDates[u].Date);
                dr["OpeningBalance"] = CurrOpening;
                dr["Transactions"] = Diff;
                dr["Closing"] = CurrentClosing;
                dt.Rows.Add(dr);
            }

            int TotalRowsIndt = dt.Rows.Count;
            CurrOpening = Convert.ToInt32(dt.Rows[TotalRowsIndt - 1]["Closing"]);
            dtDebit = GetDebitValuesForCashBalance(GetSpecificDateFormat(dtEnd));
            dtCredit = GetCreditValuesForCashBalance(GetSpecificDateFormat(dtEnd));
            int DebitValues1 = Convert.ToInt32(dtDebit.Rows[0]["Debit"]);
            int CreditValues1 = Convert.ToInt32(dtCredit.Rows[0]["Credit"]);
            int Diff1 = DebitValues1 - CreditValues1;

            int CurrentClosing1 = CurrOpening + Diff1;
            dr = dt.NewRow();
            dr["BalanceDate"] = GetSpecificDateFormat(dtEnd);
            dr["OpeningBalance"] = CurrOpening;
            dr["Transactions"] = Diff1;
            dr["Closing"] = CurrentClosing1;
            dt.Rows.Add(dr);
            return dt;
        }

        List<DateTime> GetDates(DateTime startDate, DateTime endDate)
        {
            List<DateTime> dates = new List<DateTime>();

            while ((startDate = startDate.AddDays(1)) < endDate)
            {
                dates.Add(startDate);
            }

            return dates;
        }

        DataTable GetDebitValuesForCashBalance(string FromDate)
        {
            SqlConnection sqlConnection = new SqlConnection(this.ConnString);
            string Debitqry = @"Select sum(Debit) AS [Debit] FRom (
                                    Select 'Recovery' AS [TransactionType],ISNULL(SUM(Dr),0) As [Debit],0 As [Credit] From CashBalance Where TableName ='Trans_Recovery' and Convert(varchar(11),CashInDate,121) = '" + FromDate + @"'
                                    union All
                                    Select 'BankWithDrawl',ISNULL(SUM(Dr),0) As [Debit],0 As [Credit] From CashBalance Where TableName ='Trans_BankWithDrawl' and Convert(varchar(11),CashInDate,121)='" + FromDate + @"' 
                                    union All
                                    Select 'Investment',ISNULL(SUM(Dr),0) As [Debit],0 As [Credit] From CashBalance Where TableName ='Trans_InvestmentPosting' and Convert(varchar(11),CashInDate,121) = '" + FromDate + @"' 
                                    union all
                                    Select 'OtherSales',ISNULL(SUM(Dr),0) As [Debit],0 As [Credit] From CashBalance Where TableName ='Trans_otherSale' and Convert(varchar(11),CashInDate,121)= '" + FromDate + @"' 
                                    Union All
                                    Select 'DirectSales',ISNULL(SUM(Dr),0) As [Debit],0 As [Credit] From CashBalance Where TableName ='Trans_DRSM' and Convert(varchar(11),CashInDate,121)='" + FromDate + @"' 
                                    Union All
                                    Select 'BankDeposit',0 As [Debit],ISNULL(SUM(Cr),0) As [Credit] From CashBalance Where TableName ='Trans_BankDeposit' and Convert(varchar(11),CashInDate,121)= '" + FromDate + @"' 
                                    union All
                                    Select 'Expense',0 As [Debit],ISNULL(SUM(Cr),0) As [Credit] From CashBalance Where TableName ='Trans_ClearanceD' and Convert(varchar(11),CashInDate,121)= '" + FromDate + @"' 
                                    Union All
                                    Select 'InvestmentReturn',0 As [Debit],ISNULL(SUM(Cr),0) As [Credit] From CashBalance Where TableName ='Trans_InvestmentReturn' and Convert(varchar(11),CashInDate,121)='" + FromDate + @"' 
                                    Union All
                                    Select 'ProfitSharing',0 As [Debit],Isnull(Sum(ProfitSharing),0) As [Credit] From Trans_InvestmentReturn where Convert(varchar(11),ReturnDate,121) ='" + FromDate + @"' 
									union all
                                    Select abc,Debit,SUM(PurchasePrice) AS [PurchasePrice] From (
                                    Select 'Purchase' AS [abc] ,0 as debit,(PrQty*PurchasePrice)As [PurchasePrice] From Trans_PurchaseD PD 
                                    Inner Join Trans_PurchaseM PM on PD.PRID=PM.PRID 
                                    Where Convert(varchar(11),PRDate,121)='" + FromDate + @"') AA
                                    group by abc,debit
                                    Union All
                                    Select 'KnockOffPurchase' as [abc], 0 AS debit,isnull(Sum(cr),0) AS [Credit] From CashBalance Where TableName ='KnockOffPurchase' 
                                    and Convert(varchar(11),CashIndate,121)='" + FromDate + @"'
                                    union all
Select 'Discount',0 As Debit,isnull(sum(DisAmount),0) As [DiscountAmount] From Trans_Recovery Where Convert(varchar(11),RecoveryDate,121) ='" + FromDate + @"'

                                    union All

                                    Select 'Registration',isnULL(sum(RegistrationClients+RegistrationAmount+RegistrationRecovery),0) As [Registration],
                                    0 As [Credit] From (
                                    Select ISNULL(SUM(Convert(int,Registration_Fee)),0) As [RegistrationClients],
                                    IsnULL((select isnull(sum(RegAmount),0) As [RegistrationAmount] from Registration 
                                    where Convert(varchar(11),RegDate,121)=TC.RegistrationDate
                                    ),0) As [RegistrationAmount],

                                    ISNULL((Select Isnull(SUM(registrationFee),0) + Isnull((Select isnull(SUM(regAmount),0) AS [Registration] From Registration where RegDate = '" + FromDate + @"'
                                    ),0) As [RegistrationFee] From Trans_Recovery 
                                    Where Convert(varchar(11),RecoveryDate,121) =TC.RegistrationDate
                                    ),0) AS [RegistrationRecovery]
                                    From Trans_Clients TC
                                    Where Convert(varchar(11),TC.RegistrationDate,121)='" + FromDate + @"'
                                    Group By TC.RegistrationDate) AA ) AAA";

            DataTable dtdebit = this.GetdataTable(new SqlCommand(Debitqry, sqlConnection), sqlConnection);
            return dtdebit;
        }

        DataTable GetCreditValuesForCashBalance(string FromDate)
        {
            SqlConnection sqlConnection = new SqlConnection(this.ConnString);
            string Creditqry = @"Select sum(Credit) AS [Credit] FRom (
                                    Select 'Recovery' AS [TransactionType],ISNULL(SUM(Dr),0) As [Debit],0 As [Credit] From CashBalance Where TableName ='Trans_Recovery' and Convert(varchar(11),CashInDate,121) ='" + FromDate + @"'
                                    union All
                                    Select 'BankWithDrawl',ISNULL(SUM(Dr),0) As [Debit],0 As [Credit] From CashBalance Where TableName ='Trans_BankWithDrawl' and Convert(varchar(11),CashInDate,121)='" + FromDate + @"' 
                                    union All
                                    Select 'Investment',ISNULL(SUM(Dr),0) As [Debit],0 As [Credit] From CashBalance Where TableName ='Trans_InvestmentPosting' and Convert(varchar(11),CashInDate,121) ='" + FromDate + @"' 
                                    union all
                                    Select 'OtherSales',ISNULL(SUM(Dr),0) As [Debit],0 As [Credit] From CashBalance Where TableName ='Trans_otherSale' and Convert(varchar(11),CashInDate,121)= '" + FromDate + @"' 
                                    Union All
                                    Select 'DirectSales',ISNULL(SUM(Dr),0) As [Debit],0 As [Credit] From CashBalance Where TableName ='Trans_DRSM' and Convert(varchar(11),CashInDate,121) ='" + FromDate + @"' 
                                    Union All
                                    Select 'BankDeposit',0 As [Debit],ISNULL(SUM(Cr),0) As [Credit] From CashBalance Where TableName ='Trans_BankDeposit' and Convert(varchar(11),CashInDate,121)='" + FromDate + @"' 
                                    union All
                                    Select 'Expense',0 As [Debit],ISNULL(SUM(Cr),0) As [Credit] From CashBalance Where TableName ='Trans_ClearanceD' and Convert(varchar(11),CashInDate,121)='" + FromDate + @"' 
                                    Union All
                                    Select 'InvestmentReturn',0 As [Debit],ISNULL(SUM(Cr),0) As [Credit] From CashBalance Where TableName ='Trans_InvestmentReturn' and Convert(varchar(11),CashInDate,121)='" + FromDate + @"' 
                                    Union All
Select 'Discount',0 As Debit,isnull(sum(DisAmount),0) As [DiscountAmount] From Trans_Recovery Where Convert(varchar(11),RecoveryDate,121) ='" + FromDate + @"'

                                    union All
                                    Select 'ProfitSharing',0 As [Debit],Isnull(Sum(ProfitSharing),0) As [Credit] From Trans_InvestmentReturn where Convert(varchar(11),ReturnDate,121) ='" + FromDate + @"' 
									union all
                                    Select abc,Debit,SUM(PurchasePrice) AS [PurchasePrice] From (
                                    Select 'Purchase' AS [abc] ,0 as debit,(PrQty*PurchasePrice)As [PurchasePrice] From Trans_PurchaseD PD 
                                    Inner Join Trans_PurchaseM PM on PD.PRID=PM.PRID 
                                    Where Convert(varchar(11),PRDate,121)='" + FromDate + @"') AA
                                    group by abc,debit
                                    Union All
                                    Select 'KnockOffPurchase' as [abc], 0 AS debit,isnull(Sum(cr),0) AS [Credit] From CashBalance Where TableName ='KnockOffPurchase' 
                                    and Convert(varchar(11),CashIndate,121)='" + FromDate + @"'
                                    union all

                                    Select 'Registration',isnULL(sum(RegistrationClients+RegistrationAmount+RegistrationRecovery),0) As [Registration],
                                    0 As [Credit] From (
                                    Select ISNULL(SUM(Convert(int,Registration_Fee)),0) As [RegistrationClients],
                                    IsnULL((select isnull(sum(RegAmount),0) As [RegistrationAmount] from Registration 
                                    where Convert(varchar(11),RegDate,121)=TC.RegistrationDate
                                    ),0) As [RegistrationAmount],

                                    ISNULL((Select Isnull(SUM(registrationFee),0) + Isnull((Select isnull(SUM(regAmount),0) AS [Registration] From Registration where RegDate = '" + FromDate + @"'
                                    ),0) As [RegistrationFee] From Trans_Recovery 
                                    Where Convert(varchar(11),RecoveryDate,121) =TC.RegistrationDate
                                    ),0) AS [RegistrationRecovery]
                                    From Trans_Clients TC
                                    Where Convert(varchar(11),TC.RegistrationDate,121)= '" + FromDate + @"'
                                    Group By TC.RegistrationDate) AA ) AAA";

            DataTable dtCredit = this.GetdataTable(new SqlCommand(Creditqry, sqlConnection), sqlConnection);
            return dtCredit;
        }

        string GetSpecificDateFormat(DateTime dtDate)
        {
            string ToDateQry ="";
            string ToDate = dtDate.Day.ToString();
            string ToMonth = dtDate.Month.ToString();
            string ToYear = dtDate.Year.ToString();

            if (int.Parse(ToDate.ToString()) <= 9)
            {
                ToDate = "0" + ToDate;
            }
            if (int.Parse(ToMonth.ToString()) <= 9)
            {
                ToMonth = "0" + ToMonth;
            }
            ToDateQry= ToYear + "-" + ToMonth + "-" + ToDate;
            return ToDateQry;
        }
        
        public double GetCurrentClosingBalance(string FromDate,double PreOpeningBalance)
        {
            double ClosingBalanceofCurrentDay = 0.0;

            try
            {
                SqlConnection sqlConnection = new SqlConnection(this.ConnString);
                DataTable dt = new DataTable();
                //Commented on 2016-11-14
//                string qry = @"Select Round(ISNULL(SUM(Debit)-SUM(Credit),0),2) AS [Transactions] FRom (
//                                    Select 'Recovery' AS [TransactionType],ISNULL(SUM(Dr),0) As [Debit],0 As [Credit] From CashBalance Where TableName ='Trans_Recovery' and Convert(varchar(11),CashInDate,121) between '2016-10-31' and '" + FromDate + @"'
//                                    union All
//                                    Select 'BankWithDrawl',ISNULL(SUM(Dr),0) As [Debit],0 As [Credit] From CashBalance Where TableName ='Trans_BankWithDrawl' and Convert(varchar(11),CashInDate,121)between '2016-10-31' and '" + FromDate + @"' 
//                                    union All
//                                    Select 'Investment',ISNULL(SUM(Dr),0) As [Debit],0 As [Credit] From CashBalance Where TableName ='Trans_InvestmentPosting' and Convert(varchar(11),CashInDate,121) between '2016-10-31' and '" + FromDate + @"' 
//                                    union all
//                                    Select 'OtherSales',ISNULL(SUM(Dr),0) As [Debit],0 As [Credit] From CashBalance Where TableName ='Trans_otherSale' and Convert(varchar(11),CashInDate,121)between '2016-10-31' and '" + FromDate + @"' 
//                                    Union All
//                                    Select 'DirectSales',ISNULL(SUM(Dr),0) As [Debit],0 As [Credit] From CashBalance Where TableName ='Trans_DRSM' and Convert(varchar(11),CashInDate,121) between '2016-10-31' and '" + FromDate + @"' 
//                                    Union All
//                                    Select 'BankDeposit',0 As [Debit],ISNULL(SUM(Cr),0) As [Credit] From CashBalance Where TableName ='Trans_BankDeposit' and Convert(varchar(11),CashInDate,121)between '2016-10-31' and '" + FromDate + @"' 
//                                    union All
//                                    Select 'Expense',0 As [Debit],ISNULL(SUM(Cr),0) As [Credit] From CashBalance Where TableName ='Trans_ClearanceD' and Convert(varchar(11),CashInDate,121)between '2016-10-31' and '" + FromDate + @"' 
//                                    Union All
//                                    Select 'InvestmentReturn',0 As [Debit],ISNULL(SUM(Cr),0) As [Credit] From CashBalance Where TableName ='Trans_InvestmentReturn' and Convert(varchar(11),CashInDate,121)between '2016-10-31' and '" + FromDate + @"' 
//                                    Union All
//                                    Select 'ProfitSharing',0 As [Debit],Isnull(Sum(ProfitSharing),0) As [Credit] From Trans_InvestmentReturn where Convert(varchar(11),ReturnDate,121) between '2016-10-31' and '" + FromDate + @"' 
//									union all
//                                    Select abc,Debit,SUM(PurchasePrice) AS [PurchasePrice] From (
//                                    Select 'Purchase' AS [abc] ,0 as debit,(PrQty*PurchasePrice)As [PurchasePrice] From Trans_PurchaseD PD 
//                                    Inner Join Trans_PurchaseM PM on PD.PRID=PM.PRID 
//                                    Where Convert(varchar(11),PRDate,121)between '2016-10-31' and '" + FromDate + @"') AA
//                                    group by abc,debit
//                                    Union All
//                                    Select 'KnockOffPurchase' as [abc], 0 AS debit,isnull(Sum(cr),0) AS [Credit] From CashBalance Where TableName ='KnockOffPurchase' 
//                                    and Convert(varchar(11),CashIndate,121)between '2016-10-31' and '" + FromDate + @"'
//                                    union all
//
//                                    Select 'Registration',isnULL(sum(RegistrationClients+RegistrationAmount+RegistrationRecovery),0) As [Registration],
//                                    0 As [Credit] From (
//                                    Select ISNULL(SUM(Convert(int,Registration_Fee)),0) As [RegistrationClients],
//                                    IsnULL((select isnull(sum(RegAmount),0) As [RegistrationAmount] from Registration 
//                                    where Convert(varchar(11),RegDate,121)=TC.RegistrationDate
//                                    ),0) As [RegistrationAmount],
//
//                                    ISNULL((Select Isnull(SUM(registrationFee),0) + Isnull((Select isnull(SUM(regAmount),0) AS [Registration] From Registration where RegDate between '2016-10-31' and '" + FromDate + @"'
//                                    ),0) As [RegistrationFee] From Trans_Recovery 
//                                    Where Convert(varchar(11),RecoveryDate,121) =TC.RegistrationDate
//                                    ),0) AS [RegistrationRecovery]
//                                    From Trans_Clients TC
//                                    Where Convert(varchar(11),TC.RegistrationDate,121)between '2016-10-31' and '" + FromDate + @"'
//                                    Group By TC.RegistrationDate) AA ) AAA";
                string Debitqry = @"Select sum(Debit) AS [Debit] FRom (
                                    Select 'Recovery' AS [TransactionType],ISNULL(SUM(Dr),0) As [Debit],0 As [Credit] From CashBalance Where TableName ='Trans_Recovery' and Convert(varchar(11),CashInDate,121) = '" + FromDate + @"'
                                    union All
                                    Select 'BankWithDrawl',ISNULL(SUM(Dr),0) As [Debit],0 As [Credit] From CashBalance Where TableName ='Trans_BankWithDrawl' and Convert(varchar(11),CashInDate,121)='" + FromDate + @"' 
                                    union All
                                    Select 'Investment',ISNULL(SUM(Dr),0) As [Debit],0 As [Credit] From CashBalance Where TableName ='Trans_InvestmentPosting' and Convert(varchar(11),CashInDate,121) = '" + FromDate + @"' 
                                    union all
                                    Select 'OtherSales',ISNULL(SUM(Dr),0) As [Debit],0 As [Credit] From CashBalance Where TableName ='Trans_otherSale' and Convert(varchar(11),CashInDate,121)= '" + FromDate + @"' 
                                    Union All
                                    Select 'DirectSales',ISNULL(SUM(Dr),0) As [Debit],0 As [Credit] From CashBalance Where TableName ='Trans_DRSM' and Convert(varchar(11),CashInDate,121)='" + FromDate + @"' 
                                    Union All
                                    Select 'BankDeposit',0 As [Debit],ISNULL(SUM(Cr),0) As [Credit] From CashBalance Where TableName ='Trans_BankDeposit' and Convert(varchar(11),CashInDate,121)= '" + FromDate + @"' 
                                    union All
                                    Select 'Expense',0 As [Debit],ISNULL(SUM(Cr),0) As [Credit] From CashBalance Where TableName ='Trans_ClearanceD' and Convert(varchar(11),CashInDate,121)= '" + FromDate + @"' 
                                    Union All
                                    Select 'InvestmentReturn',0 As [Debit],ISNULL(SUM(Cr),0) As [Credit] From CashBalance Where TableName ='Trans_InvestmentReturn' and Convert(varchar(11),CashInDate,121)='" + FromDate + @"' 
                                    Union All
                                    Select 'ProfitSharing',0 As [Debit],Isnull(Sum(ProfitSharing),0) As [Credit] From Trans_InvestmentReturn where Convert(varchar(11),ReturnDate,121) ='" + FromDate + @"' 
									union all
                                    Select abc,Debit,SUM(PurchasePrice) AS [PurchasePrice] From (
                                    Select 'Purchase' AS [abc] ,0 as debit,(PrQty*PurchasePrice)As [PurchasePrice] From Trans_PurchaseD PD 
                                    Inner Join Trans_PurchaseM PM on PD.PRID=PM.PRID 
                                    Where Convert(varchar(11),PRDate,121)='" + FromDate + @"') AA
                                    group by abc,debit
                                    Union All
                                    Select 'KnockOffPurchase' as [abc], 0 AS debit,isnull(Sum(cr),0) AS [Credit] From CashBalance Where TableName ='KnockOffPurchase' 
                                    and Convert(varchar(11),CashIndate,121)='" + FromDate + @"'
                                    union all
Select 'Discount',0 As Debit,isnull(sum(DisAmount),0) As [DiscountAmount] From Trans_Recovery Where Convert(varchar(11),RecoveryDate,121) ='" + FromDate + @"'

                                    union All

                                    Select 'Registration',isnULL(sum(RegistrationClients+RegistrationAmount+RegistrationRecovery),0) As [Registration],
                                    0 As [Credit] From (
                                    Select ISNULL(SUM(Convert(int,Registration_Fee)),0) As [RegistrationClients],
                                    IsnULL((select isnull(sum(RegAmount),0) As [RegistrationAmount] from Registration 
                                    where Convert(varchar(11),RegDate,121)=TC.RegistrationDate
                                    ),0) As [RegistrationAmount],

                                    ISNULL((Select Isnull(SUM(registrationFee),0) + Isnull((Select isnull(SUM(regAmount),0) AS [Registration] From Registration where RegDate = '" + FromDate + @"'
                                    ),0) As [RegistrationFee] From Trans_Recovery 
                                    Where Convert(varchar(11),RecoveryDate,121) =TC.RegistrationDate
                                    ),0) AS [RegistrationRecovery]
                                    From Trans_Clients TC
                                    Where Convert(varchar(11),TC.RegistrationDate,121)='" + FromDate + @"'
                                    Group By TC.RegistrationDate) AA ) AAA";

                DataTable dtdebit = this.GetdataTable(new SqlCommand(Debitqry, sqlConnection), sqlConnection);


                string Creditqry = @"Select sum(Credit) AS [Credit] FRom (
                                    Select 'Recovery' AS [TransactionType],ISNULL(SUM(Dr),0) As [Debit],0 As [Credit] From CashBalance Where TableName ='Trans_Recovery' and Convert(varchar(11),CashInDate,121) ='" + FromDate + @"'
                                    union All
                                    Select 'BankWithDrawl',ISNULL(SUM(Dr),0) As [Debit],0 As [Credit] From CashBalance Where TableName ='Trans_BankWithDrawl' and Convert(varchar(11),CashInDate,121)='" + FromDate + @"' 
                                    union All
                                    Select 'Investment',ISNULL(SUM(Dr),0) As [Debit],0 As [Credit] From CashBalance Where TableName ='Trans_InvestmentPosting' and Convert(varchar(11),CashInDate,121) ='" + FromDate + @"' 
                                    union all
                                    Select 'OtherSales',ISNULL(SUM(Dr),0) As [Debit],0 As [Credit] From CashBalance Where TableName ='Trans_otherSale' and Convert(varchar(11),CashInDate,121)= '" + FromDate + @"' 
                                    Union All
                                    Select 'DirectSales',ISNULL(SUM(Dr),0) As [Debit],0 As [Credit] From CashBalance Where TableName ='Trans_DRSM' and Convert(varchar(11),CashInDate,121) ='" + FromDate + @"' 
                                    Union All
                                    Select 'BankDeposit',0 As [Debit],ISNULL(SUM(Cr),0) As [Credit] From CashBalance Where TableName ='Trans_BankDeposit' and Convert(varchar(11),CashInDate,121)='" + FromDate + @"' 
                                    union All
                                    Select 'Expense',0 As [Debit],ISNULL(SUM(Cr),0) As [Credit] From CashBalance Where TableName ='Trans_ClearanceD' and Convert(varchar(11),CashInDate,121)='" + FromDate + @"' 
                                    Union All
                                    Select 'InvestmentReturn',0 As [Debit],ISNULL(SUM(Cr),0) As [Credit] From CashBalance Where TableName ='Trans_InvestmentReturn' and Convert(varchar(11),CashInDate,121)='" + FromDate + @"' 
                                    Union All
Select 'Discount',0 As Debit,isnull(sum(DisAmount),0) As [DiscountAmount] From Trans_Recovery Where Convert(varchar(11),RecoveryDate,121) ='" + FromDate + @"'

                                    union All
                                    Select 'ProfitSharing',0 As [Debit],Isnull(Sum(ProfitSharing),0) As [Credit] From Trans_InvestmentReturn where Convert(varchar(11),ReturnDate,121) ='" + FromDate + @"' 
									union all
                                    Select abc,Debit,SUM(PurchasePrice) AS [PurchasePrice] From (
                                    Select 'Purchase' AS [abc] ,0 as debit,(PrQty*PurchasePrice)As [PurchasePrice] From Trans_PurchaseD PD 
                                    Inner Join Trans_PurchaseM PM on PD.PRID=PM.PRID 
                                    Where Convert(varchar(11),PRDate,121)='" + FromDate + @"') AA
                                    group by abc,debit
                                    Union All
                                    Select 'KnockOffPurchase' as [abc], 0 AS debit,isnull(Sum(cr),0) AS [Credit] From CashBalance Where TableName ='KnockOffPurchase' 
                                    and Convert(varchar(11),CashIndate,121)='" + FromDate + @"'
                                    union all

                                    Select 'Registration',isnULL(sum(RegistrationClients+RegistrationAmount+RegistrationRecovery),0) As [Registration],
                                    0 As [Credit] From (
                                    Select ISNULL(SUM(Convert(int,Registration_Fee)),0) As [RegistrationClients],
                                    IsnULL((select isnull(sum(RegAmount),0) As [RegistrationAmount] from Registration 
                                    where Convert(varchar(11),RegDate,121)=TC.RegistrationDate
                                    ),0) As [RegistrationAmount],

                                    ISNULL((Select Isnull(SUM(registrationFee),0) + Isnull((Select isnull(SUM(regAmount),0) AS [Registration] From Registration where RegDate = '" + FromDate + @"'
                                    ),0) As [RegistrationFee] From Trans_Recovery 
                                    Where Convert(varchar(11),RecoveryDate,121) =TC.RegistrationDate
                                    ),0) AS [RegistrationRecovery]
                                    From Trans_Clients TC
                                    Where Convert(varchar(11),TC.RegistrationDate,121)= '" + FromDate + @"'
                                    Group By TC.RegistrationDate) AA ) AAA";

                DataTable dtCredit= this.GetdataTable(new SqlCommand(Creditqry, sqlConnection), sqlConnection);
                //'2014-02-02'
                //dt = this.GetdataTable(new SqlCommand(qry, sqlConnection), sqlConnection);
                //double Opening = 32994;
                //double Opening = 116505;

                double Sum= PreOpeningBalance + Convert.ToDouble(dtdebit.Rows[0]["Debit"]);
                Math.Round(ClosingBalanceofCurrentDay = Sum - Convert.ToDouble(dtCredit.Rows[0]["Credit"]),0);
                
                //double Opening = 3055085;
                //double Opening = 20014082;
                //double ClosingBalance = Math.Round(double.Parse(dt.Rows[0]["Transactions"].ToString()),0);
                //ClosingBalance = Opening + ClosingBalance;
                //ClosingBalanceofCurrentDay = ClosingBalance;
            }
            catch (Exception ex)
            {

                throw;
            }
            return ClosingBalanceofCurrentDay;
        }

        public double GetPreviousClosingBalance(string FromDate)
        {
            double ClosingBalanceofCurrentDay = 0.0;

            try
            {
                SqlConnection sqlConnection = new SqlConnection(this.ConnString);
                DataTable dt = new DataTable();
                DateTime dt1 = Convert.ToDateTime(FromDate);
                dt1 = dt1.AddDays(-1);

                int FrmDay = dt1.Day;
                int FrmMonth = dt1.Month;

                string FromDay = "";
                string FromMonth = "";


                if (FrmDay <= 9)
                {
                    FromDay = "0" + FrmDay.ToString();
                }
                else
                {
                    FromDay = FrmDay.ToString();
                }
                if (FrmMonth <= 9)
                {
                    FromMonth = "0" + FrmMonth;
                }
                else
                {
                    FromMonth = FrmMonth.ToString();
                }
                string FrmYear = dt1.Year.ToString();
                string NewFromDate = FrmYear + "-" + FromMonth + "-" + FromDay;

//                string qry = @"Select Round(ISNULL(SUM(Debit)-SUM(Credit),0),2) AS [Transactions] FRom (
//                                    Select 'Recovery' AS [TransactionType],ISNULL(SUM(Dr),0) As [Debit],0 As [Credit] From CashBalance Where TableName ='Trans_Recovery' and Convert(varchar(11),CashInDate,121) between '2016-10-31' and '" + NewFromDate + @"'
//                                    union All
//                                    Select 'BankWithDrawl',ISNULL(SUM(Dr),0) As [Debit],0 As [Credit] From CashBalance Where TableName ='Trans_BankWithDrawl' and Convert(varchar(11),CashInDate,121)between '2016-10-31' and '" + NewFromDate + @"' 
//                                    union All
//                                    Select 'Investment',ISNULL(SUM(Dr),0) As [Debit],0 As [Credit] From CashBalance Where TableName ='Trans_InvestmentPosting' and Convert(varchar(11),CashInDate,121) between '2016-10-31' and '" + NewFromDate + @"' 
//                                    union all
//                                    Select 'OtherSales',ISNULL(SUM(Dr),0) As [Debit],0 As [Credit] From CashBalance Where TableName ='Trans_otherSale' and Convert(varchar(11),CashInDate,121)between '2016-10-31' and '" + NewFromDate + @"' 
//                                    Union All
//                                    Select 'DirectSales',ISNULL(SUM(Dr),0) As [Debit],0 As [Credit] From CashBalance Where TableName ='Trans_DRSM' and Convert(varchar(11),CashInDate,121) between '2016-10-31' and '" + NewFromDate + @"' 
//                                    Union All
//                                    Select 'BankDeposit',0 As [Debit],ISNULL(SUM(Cr),0) As [Credit] From CashBalance Where TableName ='Trans_BankDeposit' and Convert(varchar(11),CashInDate,121)between '2016-10-31' and '" + NewFromDate + @"' 
//                                    union All
//                                    Select 'Expense',0 As [Debit],ISNULL(SUM(Cr),0) As [Credit] From CashBalance Where TableName ='Trans_ClearanceD' and Convert(varchar(11),CashInDate,121)between '2016-10-31' and '" + NewFromDate + @"' 
//                                    Union All
//                                    Select 'InvestmentReturn',0 As [Debit],ISNULL(SUM(Cr),0) As [Credit] From CashBalance Where TableName ='Trans_InvestmentReturn' and Convert(varchar(11),CashInDate,121)between '2016-10-31' and '" + NewFromDate + @"' 
//                                    Union All
//                                    Select 'ProfitSharing',0 As [Debit],Isnull(Sum(ProfitSharing),0) As [Credit] From Trans_InvestmentReturn where Convert(varchar(11),ReturnDate,121) between '2016-10-31' and '" + NewFromDate + @"' 
//									union all
//                                    Select abc,Debit,SUM(PurchasePrice) AS [PurchasePrice] From (
//                                    Select 'Purchase' AS [abc] ,0 as debit,(PrQty*PurchasePrice)As [PurchasePrice] From Trans_PurchaseD PD 
//                                    Inner Join Trans_PurchaseM PM on PD.PRID=PM.PRID 
//                                    Where Convert(varchar(11),PRDate,121)between '2016-10-31' and '" + NewFromDate + @"') AA
//                                    group by abc,debit
//                                    Union All
//                                    Select 'KnockOffPurchase' as [abc], 0 AS debit,isnull(Sum(cr),0) AS [Credit] From CashBalance Where TableName ='KnockOffPurchase' 
//                                    and Convert(varchar(11),CashIndate,121)between '2016-10-31' and '" + NewFromDate + @"'
//                                    union all
//
//                                    Select 'Registration',isnULL(sum(RegistrationClients+RegistrationAmount+RegistrationRecovery),0) As [Registration],
//                                    0 As [Credit] From (
//                                    Select ISNULL(SUM(Convert(int,Registration_Fee)),0) As [RegistrationClients],
//                                    IsnULL((select isnull(sum(RegAmount),0) As [RegistrationAmount] from Registration 
//                                    where Convert(varchar(11),RegDate,121)=TC.RegistrationDate
//                                    ),0) As [RegistrationAmount],
//
//                                    ISNULL((Select Isnull(SUM(registrationFee),0) + Isnull((Select isnull(SUM(regAmount),0) AS [Registration] From Registration where RegDate between '2016-10-31' and '" + NewFromDate + @"'
//                                    ),0) As [RegistrationFee] From Trans_Recovery 
//                                    Where Convert(varchar(11),RecoveryDate,121) =TC.RegistrationDate
//                                    ),0) AS [RegistrationRecovery]
//                                    From Trans_Clients TC
//                                    Where Convert(varchar(11),TC.RegistrationDate,121)between '2016-10-31' and '" + NewFromDate + @"'
//                                    Group By TC.RegistrationDate) AA ) AAA";

//                string qry = @"Select Round(ISNULL(SUM(Debit)-SUM(Credit),0),2) AS [Transactions] FRom (
//                                    Select 'Recovery' AS [TransactionType],ISNULL(SUM(Dr),0) As [Debit],0 As [Credit] From CashBalance Where TableName ='Trans_Recovery' and Convert(varchar(11),CashInDate,121) between '2016-10-31' and '" + NewFromDate + @"'
//                                    union All
//                                    Select 'BankWithDrawl',ISNULL(SUM(Dr),0) As [Debit],0 As [Credit] From CashBalance Where TableName ='Trans_BankWithDrawl' and Convert(varchar(11),CashInDate,121)between '2016-10-31' and '" + NewFromDate + @"' 
//                                    union All
//                                    Select 'Investment',ISNULL(SUM(Dr),0) As [Debit],0 As [Credit] From CashBalance Where TableName ='Trans_InvestmentPosting' and Convert(varchar(11),CashInDate,121) between '2016-10-31' and '" + NewFromDate + @"' 
//                                    union all
//                                    Select 'OtherSales',ISNULL(SUM(Dr),0) As [Debit],0 As [Credit] From CashBalance Where TableName ='Trans_otherSale' and Convert(varchar(11),CashInDate,121)between '2016-10-31' and '" + NewFromDate + @"' 
//                                    Union All
//                                    Select 'DirectSales',ISNULL(SUM(Dr),0) As [Debit],0 As [Credit] From CashBalance Where TableName ='Trans_DRSM' and Convert(varchar(11),CashInDate,121) between '2016-10-31' and '" + NewFromDate + @"' 
//                                    Union All
//                                    Select 'BankDeposit',0 As [Debit],ISNULL(SUM(Cr),0) As [Credit] From CashBalance Where TableName ='Trans_BankDeposit' and Convert(varchar(11),CashInDate,121)between '2016-10-31' and '" + NewFromDate + @"' 
//                                    union All
//                                    Select 'Expense',0 As [Debit],ISNULL(SUM(Cr),0) As [Credit] From CashBalance Where TableName ='Trans_ClearanceD' and Convert(varchar(11),CashInDate,121)between '2016-10-31' and '" + NewFromDate + @"' 
//                                    Union All
//                                    Select 'InvestmentReturn',0 As [Debit],ISNULL(SUM(Cr),0) As [Credit] From CashBalance Where TableName ='Trans_InvestmentReturn' and Convert(varchar(11),CashInDate,121)between '2016-10-31' and '" + NewFromDate + @"' 
//                                    Union All
//                                    Select 'ProfitSharing',0 As [Debit],Isnull(Sum(ProfitSharing),0) As [Credit] From Trans_InvestmentReturn where Convert(varchar(11),ReturnDate,121) between '2016-10-31' and '" + NewFromDate + @"' 
//									union all
//                                    Select abc,Debit,SUM(PurchasePrice) AS [PurchasePrice] From (
//                                    Select 'Purchase' AS [abc] ,0 as debit,(PrQty*PurchasePrice)As [PurchasePrice] From Trans_PurchaseD PD 
//                                    Inner Join Trans_PurchaseM PM on PD.PRID=PM.PRID 
//                                    Where Convert(varchar(11),PRDate,121)between '2016-10-31' and '" + NewFromDate + @"') AA
//                                    group by abc,debit
//                                    Union All
//                                    Select 'KnockOffPurchase' as [abc], 0 AS debit,isnull(Sum(cr),0) AS [Credit] From CashBalance Where TableName ='KnockOffPurchase' 
//                                    and Convert(varchar(11),CashIndate,121)between '2016-10-31' and '" + NewFromDate + @"'
//                                    union all
//
//                                    Select 'Registration',isnULL(sum(RegistrationClients+RegistrationAmount+RegistrationRecovery),0) As [Registration],
//                                    0 As [Credit] From (
//                                    Select ISNULL(SUM(Convert(int,Registration_Fee)),0) As [RegistrationClients],
//                                    IsnULL((select isnull(sum(RegAmount),0) As [RegistrationAmount] from Registration 
//                                    where Convert(varchar(11),RegDate,121)=TC.RegistrationDate
//                                    ),0) As [RegistrationAmount],
//
//                                    ISNULL((Select Isnull(SUM(registrationFee),0) + Isnull((Select isnull(SUM(regAmount),0) AS [Registration] From Registration where RegDate between '2016-10-31' and '" + NewFromDate + @"'
//                                    ),0) As [RegistrationFee] From Trans_Recovery 
//                                    Where Convert(varchar(11),RecoveryDate,121) =TC.RegistrationDate
//                                    ),0) AS [RegistrationRecovery]
//                                    From Trans_Clients TC
//                                    Where Convert(varchar(11),TC.RegistrationDate,121)between '2016-10-31' and '" + NewFromDate + @"'
//                                    Group By TC.RegistrationDate) AA ) AAA";

                string Debitqry = @"Select Round(ISNULL(SUM(Debit),0),2) AS [Debit] FRom (
                                    Select 'Recovery' AS [TransactionType],ISNULL(SUM(Dr),0) As [Debit],0 As [Credit] From CashBalance Where TableName ='Trans_Recovery' and Convert(varchar(11),CashInDate,121) between '2016-10-31' and '" + NewFromDate + @"'
                                    union All
                                    Select 'BankWithDrawl',ISNULL(SUM(Dr),0) As [Debit],0 As [Credit] From CashBalance Where TableName ='Trans_BankWithDrawl' and Convert(varchar(11),CashInDate,121)between '2016-10-31' and '" + NewFromDate + @"' 
                                    union All
Select 'Discount',0 As Debit,isnull(sum(DisAmount),0) As [DiscountAmount] From Trans_Recovery Where Convert(varchar(11),RecoveryDate,121) between '2016-10-31' and '" + FromDate + @"'

                                    union All
                                    Select 'Investment',ISNULL(SUM(Dr),0) As [Debit],0 As [Credit] From CashBalance Where TableName ='Trans_InvestmentPosting' and Convert(varchar(11),CashInDate,121) between '2016-10-31' and '" + NewFromDate + @"' 
                                    union all
                                    Select 'OtherSales',ISNULL(SUM(Dr),0) As [Debit],0 As [Credit] From CashBalance Where TableName ='Trans_otherSale' and Convert(varchar(11),CashInDate,121)between '2016-10-31' and '" + NewFromDate + @"' 
                                    Union All
                                    Select 'DirectSales',ISNULL(SUM(Dr),0) As [Debit],0 As [Credit] From CashBalance Where TableName ='Trans_DRSM' and Convert(varchar(11),CashInDate,121) between '2016-10-31' and '" + NewFromDate + @"' 
                                    Union All
                                    Select 'BankDeposit',0 As [Debit],ISNULL(SUM(Cr),0) As [Credit] From CashBalance Where TableName ='Trans_BankDeposit' and Convert(varchar(11),CashInDate,121)between '2016-10-31' and '" + NewFromDate + @"' 
                                    union All
                                    Select 'Expense',0 As [Debit],ISNULL(SUM(Cr),0) As [Credit] From CashBalance Where TableName ='Trans_ClearanceD' and Convert(varchar(11),CashInDate,121)between '2016-10-31' and '" + NewFromDate + @"' 
                                    Union All
                                    Select 'InvestmentReturn',0 As [Debit],ISNULL(SUM(Cr),0) As [Credit] From CashBalance Where TableName ='Trans_InvestmentReturn' and Convert(varchar(11),CashInDate,121)between '2016-10-31' and '" + NewFromDate + @"' 
                                    Union All
                                    Select 'ProfitSharing',0 As [Debit],Isnull(Sum(ProfitSharing),0) As [Credit] From Trans_InvestmentReturn where Convert(varchar(11),ReturnDate,121) between '2016-10-31' and '" + NewFromDate + @"' 
									union all
                                    Select abc,Debit,SUM(PurchasePrice) AS [PurchasePrice] From (
                                    Select 'Purchase' AS [abc] ,0 as debit,(PrQty*PurchasePrice)As [PurchasePrice] From Trans_PurchaseD PD 
                                    Inner Join Trans_PurchaseM PM on PD.PRID=PM.PRID 
                                    Where Convert(varchar(11),PRDate,121)between '2016-10-31' and '" + NewFromDate + @"') AA
                                    group by abc,debit
                                    Union All
                                    Select 'KnockOffPurchase' as [abc], 0 AS debit,isnull(Sum(cr),0) AS [Credit] From CashBalance Where TableName ='KnockOffPurchase' 
                                    and Convert(varchar(11),CashIndate,121)between '2016-10-31' and '" + NewFromDate + @"'
                                    union all

                                    Select 'Registration',isnULL(sum(RegistrationClients+RegistrationAmount+RegistrationRecovery),0) As [Registration],
                                    0 As [Credit] From (
                                    Select ISNULL(SUM(Convert(int,Registration_Fee)),0) As [RegistrationClients],
                                    IsnULL((select isnull(sum(RegAmount),0) As [RegistrationAmount] from Registration 
                                    where Convert(varchar(11),RegDate,121)=TC.RegistrationDate
                                    ),0) As [RegistrationAmount],

                                    ISNULL((Select Isnull(SUM(registrationFee),0) + Isnull((Select isnull(SUM(regAmount),0) AS [Registration] From Registration where RegDate between '2016-10-31' and '" + NewFromDate + @"'
                                    ),0) As [RegistrationFee] From Trans_Recovery 
                                    Where Convert(varchar(11),RecoveryDate,121) =TC.RegistrationDate
                                    ),0) AS [RegistrationRecovery]
                                    From Trans_Clients TC
                                    Where Convert(varchar(11),TC.RegistrationDate,121)between '2016-10-31' and '" + NewFromDate + @"'
                                    Group By TC.RegistrationDate) AA ) AAA";
                DataTable dtDebit = this.GetdataTable(new SqlCommand(Debitqry, sqlConnection), sqlConnection);

                string Creditqry = @"Select Round(ISNULL(SUM(Credit),0),2) AS [Credit] FRom (
                                    Select 'Recovery' AS [TransactionType],ISNULL(SUM(Dr),0) As [Debit],0 As [Credit] From CashBalance Where TableName ='Trans_Recovery' and Convert(varchar(11),CashInDate,121) between '2016-10-31' and '" + NewFromDate + @"'
                                    union All
                                    Select 'BankWithDrawl',ISNULL(SUM(Dr),0) As [Debit],0 As [Credit] From CashBalance Where TableName ='Trans_BankWithDrawl' and Convert(varchar(11),CashInDate,121)between '2016-10-31' and '" + NewFromDate + @"' 
                                    union All
                                    Select 'Investment',ISNULL(SUM(Dr),0) As [Debit],0 As [Credit] From CashBalance Where TableName ='Trans_InvestmentPosting' and Convert(varchar(11),CashInDate,121) between '2016-10-31' and '" + NewFromDate + @"' 
                                    union all
Select 'Discount',0 As Debit,isnull(sum(DisAmount),0) As [DiscountAmount] From Trans_Recovery Where Convert(varchar(11),RecoveryDate,121) between '2016-10-31' and '" + FromDate + @"'

                                    union All
                                    Select 'OtherSales',ISNULL(SUM(Dr),0) As [Debit],0 As [Credit] From CashBalance Where TableName ='Trans_otherSale' and Convert(varchar(11),CashInDate,121)between '2016-10-31' and '" + NewFromDate + @"' 
                                    Union All
                                    Select 'DirectSales',ISNULL(SUM(Dr),0) As [Debit],0 As [Credit] From CashBalance Where TableName ='Trans_DRSM' and Convert(varchar(11),CashInDate,121) between '2016-10-31' and '" + NewFromDate + @"' 
                                    Union All
                                    Select 'BankDeposit',0 As [Debit],ISNULL(SUM(Cr),0) As [Credit] From CashBalance Where TableName ='Trans_BankDeposit' and Convert(varchar(11),CashInDate,121)between '2016-10-31' and '" + NewFromDate + @"' 
                                    union All
                                    Select 'Expense',0 As [Debit],ISNULL(SUM(Cr),0) As [Credit] From CashBalance Where TableName ='Trans_ClearanceD' and Convert(varchar(11),CashInDate,121)between '2016-10-31' and '" + NewFromDate + @"' 
                                    Union All
                                    Select 'InvestmentReturn',0 As [Debit],ISNULL(SUM(Cr),0) As [Credit] From CashBalance Where TableName ='Trans_InvestmentReturn' and Convert(varchar(11),CashInDate,121)between '2016-10-31' and '" + NewFromDate + @"' 
                                    Union All
                                    Select 'ProfitSharing',0 As [Debit],Isnull(Sum(ProfitSharing),0) As [Credit] From Trans_InvestmentReturn where Convert(varchar(11),ReturnDate,121) between '2016-10-31' and '" + NewFromDate + @"' 
									union all
                                    Select abc,Debit,SUM(PurchasePrice) AS [PurchasePrice] From (
                                    Select 'Purchase' AS [abc] ,0 as debit,(PrQty*PurchasePrice)As [PurchasePrice] From Trans_PurchaseD PD 
                                    Inner Join Trans_PurchaseM PM on PD.PRID=PM.PRID 
                                    Where Convert(varchar(11),PRDate,121)between '2016-10-31' and '" + NewFromDate + @"') AA
                                    group by abc,debit
                                    Union All
                                    Select 'KnockOffPurchase' as [abc], 0 AS debit,isnull(Sum(cr),0) AS [Credit] From CashBalance Where TableName ='KnockOffPurchase' 
                                    and Convert(varchar(11),CashIndate,121)between '2016-10-31' and '" + NewFromDate + @"'
                                    union all

                                    Select 'Registration',isnULL(sum(RegistrationClients+RegistrationAmount+RegistrationRecovery),0) As [Registration],
                                    0 As [Credit] From (
                                    Select ISNULL(SUM(Convert(int,Registration_Fee)),0) As [RegistrationClients],
                                    IsnULL((select isnull(sum(RegAmount),0) As [RegistrationAmount] from Registration 
                                    where Convert(varchar(11),RegDate,121)=TC.RegistrationDate
                                    ),0) As [RegistrationAmount],

                                    ISNULL((Select Isnull(SUM(registrationFee),0) + Isnull((Select isnull(SUM(regAmount),0) AS [Registration] From Registration where RegDate between '2016-10-31' and '" + NewFromDate + @"'
                                    ),0) As [RegistrationFee] From Trans_Recovery 
                                    Where Convert(varchar(11),RecoveryDate,121) =TC.RegistrationDate
                                    ),0) AS [RegistrationRecovery]
                                    From Trans_Clients TC
                                    Where Convert(varchar(11),TC.RegistrationDate,121)between '2016-10-31' and '" + NewFromDate + @"'
                                    Group By TC.RegistrationDate) AA ) AAA";

                DataTable dtCredit = this.GetdataTable(new SqlCommand(Creditqry, sqlConnection), sqlConnection);

                

                //'2014-02-02'
                //dt = this.GetdataTable(new SqlCommand(qry, sqlConnection), sqlConnection);
                //double Opening = 32994;
                double Opening = 3055085;
                //double Opening = 20014082;
                //double ClosingBalance = Math.Round(double.Parse(dt.Rows[0]["Transactions"].ToString()), 0);
                if (Convert.ToDouble(dtCredit.Rows[0]["Credit"]) != 0)
                {
                    Opening = Opening + Convert.ToDouble(dtDebit.Rows[0]["Debit"]) - Convert.ToDouble(dtCredit.Rows[0]["Credit"]);
                    
                }
                if (FromDate.Equals("2016-11-01"))
                {
                    Opening = 3056383;
                }
                Math.Round(ClosingBalanceofCurrentDay = Opening,0);
                //double ClosingBalance = 0;
                //ClosingBalance = Opening + ClosingBalance;
                //ClosingBalanceofCurrentDay = ClosingBalance;
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return ClosingBalanceofCurrentDay;
        }

        public DataTable GetInActiveClients(string FromDate,string ToDate)
        {
            DataTable dtDisbursement = new DataTable();
            DataTable dtRecovery = new DataTable();
            DataTable dtFinal = new DataTable();
            dtFinal.Columns.Add("ClientCode");
            dtFinal.Columns.Add("ClientName");
            dtFinal.Columns.Add("Address");
            dtFinal.Columns.Add("CellNo");
            dtFinal.Columns.Add("EmpName");

            SqlConnection sqlConnection = new SqlConnection(this.ConnString);
            string qry = @"Select * from Trans_IssueM A
                            Inner Join Trans_Clients B on A.ClientID=B.ClientID
                            where A.ClientID in (
                            Select Distinct A.ClientID From Trans_Recovery A
                            Where Convert(varchar(11),RecoveryDate,121) between '"+FromDate+@"' and '"+ToDate+@"')
                            and Convert(varchar(11),IssueDate,121) between '"+FromDate+"' and '"+ToDate+@"'
                            Order by A.ClientID";
            dtDisbursement = this.GetdataTable(new SqlCommand(qry, sqlConnection), sqlConnection);
            qry = "";
            qry = @"Select Distinct A.ClientID,ClientCode,ClientName,CellNo,B.Address,EmpName From Trans_Recovery A
                    Inner Join Trans_Clients B on A.ClientID=B.ClientID
                    Inner JOin Emp_Def C on B.EmpCode=C.EmpCode
                    Where Convert(varchar(11),RecoveryDate,121) between '" + FromDate+"' and '"+ToDate+@"'
                    Order by A.ClientID";
            dtRecovery = this.GetdataTable(new SqlCommand(qry, sqlConnection), sqlConnection);
            int Ivar = 0;
            foreach (DataRow dr in dtRecovery.Rows)
            {
                DataView dv = new DataView(dtDisbursement);
                dv.RowFilter = "ClientId='"+dr["ClientId"]+"'";
                DataTable dtTemp = dv.ToTable();
                if (dtTemp.Rows.Count == 0)
                {
                    DataRow drRow = dtFinal.NewRow();
                    drRow["ClientCode"] = dr["ClientCode"];
                    drRow["ClientName"] = dr["ClientName"];
                    drRow["CellNo"] = dr["CellNo"];
                    drRow["Address"] = dr["Address"];
                    drRow["EmpName"] = dr["EmpName"];
                    dtFinal.Rows.Add(drRow);
                }
            }
            return dtFinal;
        }

        string GetColummName(DataTable dtTemp, string ColumnName)
        {
            string ColName = "";
            foreach (DataColumn col in dtTemp.Columns)
            {
                if (ColumnName == col.ColumnName)
                {
                    ColName = col.ColumnName.ToString();
                    return ColName;
                }
            }
            return ColName;
        }

        string ChangeDateFormat(DateTime dtDate)
        {
            string cDate = "";
            string dtEndDate = dtDate.ToShortDateString();
            string ToDate = dtDate.Day.ToString();
            string ToMonth = dtDate.Month.ToString();
            string ToYear = dtDate.Year.ToString();

            if (int.Parse(ToDate.ToString()) <= 9)
            {
                ToDate = "0" + ToDate;
            }
            if (int.Parse(ToMonth.ToString()) <= 9)
            {
                ToMonth = "0" + ToMonth;
            }
            return cDate = ToYear + "-" + ToMonth + "-" + ToDate;
        }

        int GetMonthIDbyMonthName(string SelMonthName)
        {
            int ID = 0;
            switch (SelMonthName)
            {
                case "June":
                    {
                        ID = 6;
                        break;
                    }
                case "July":
                    {
                        ID = 7;
                        break;
                    }
                case "August":
                    {
                        ID = 8;
                        break;
                    }
                case "September":
                    {
                        ID = 9;
                        break;
                    }
                case "October":
                    {
                        ID = 10;
                        break;
                    }
                case "November":
                    {
                        ID = 11;
                        break;
                    }
                case "December":
                    {
                        ID = 12;
                        break;
                    }
            }
            return ID;
        }

        string GetMonthNameByMonthID(int SelMonthID)
        {
            string strMonthName = "";
            //SelMonthID++;
            switch (SelMonthID)
            {
                case 1:
                    {
                        strMonthName = "January";
                        break;
                    }
                case 2:
                    {
                        strMonthName = "February";
                        break;
                    }
                case 3:
                    {
                        strMonthName = "March";
                        break;
                    }
                case 4:
                    {
                        strMonthName = "April";
                        break;
                    }






                case 5:
                    {
                        strMonthName = "May";
                        break;
                    }
                case 6:
                    {
                        strMonthName = "June";
                        break;
                    }
                case 7:
                    {
                        strMonthName = "July";
                        break;
                    }
                case 8:
                    {
                        strMonthName = "August";
                        break;
                    }
                case 9:
                    {
                        strMonthName = "September";
                        break;
                    }
                case 10:
                    {
                        strMonthName = "October";
                        break;
                    }
                case 11:
                    {
                        strMonthName = "November";
                        break;
                    }
                case 12:
                    {
                        strMonthName = "December";
                        break;
                    }
            }
            return strMonthName;
        }

        public DataTable Rpt_DirectSalesBalance(string FromDate, string ToDate)
        {
            DataTable dt = new DataTable();
            try
            {
                SqlConnection sqlconn = new SqlConnection(ConnString);
                string Qry = @"Select *,TotalSale-Discount AS [NetBill] From (
                                Select A.DRS_ID,Convert(varchar(11),A.DRS_Date,106) As IssueDate,A.DRS_Name,sum(B.ItemPrice*B.Qty) As[Purchase Price],sum(B.SalesPrice*B.Qty) As [TotalSale],Type,
                                A.Discount from Trans_DRSM A
                                inner join Trans_DRSD B ON A.DRS_ID = B.DRS_ID
                                inner join Items C On B.ItemID = C.ItemID
                                Where Convert(varchar(11),A.DRS_Date,121) between '" + FromDate + @"' and '" + ToDate + @"'
                                group bY A.DRS_ID,DRS_Date,DRS_Name,a.Discount,Type)A
                                order by IssueDate";
                SqlCommand sqlcomm = new SqlCommand(Qry, sqlconn);
                dt = GetdataTable(sqlcomm, sqlconn);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, DataAccess.ProjectName, MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            return dt;
        }

        public DataTable Rpt_DirectSalesByType(string FromDate, string ToDate,string SaleType)
        {
            DataTable dt = new DataTable();
            try
            {
                SqlConnection sqlconn = new SqlConnection(ConnString);
               
                string Qry = @"Select *,TotalSale-Discount AS [NetBill] From (
Select A.DRS_ID,Convert(varchar(11),A.DRS_Date,106) As IssueDate,A.DRS_Name,sum(B.ItemPrice*B.Qty) As[Purchase Price],sum(B.SalesPrice*B.Qty) As [TotalSale],Type,
A.Discount from Trans_DRSM A
inner join Trans_DRSD B ON A.DRS_ID = B.DRS_ID
inner join Items C On B.ItemID = C.ItemID
Where Convert(varchar(11),A.DRS_Date,121) between '" + FromDate + @"' and '" + ToDate + @"' and type = '"+SaleType+"' group bY A.DRS_ID,DRS_Date,DRS_Name,a.Discount,Type)A order by IssueDate";
                SqlCommand sqlcomm = new SqlCommand(Qry, sqlconn);
                dt = GetdataTable(sqlcomm, sqlconn);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, DataAccess.ProjectName, MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            return dt;
        }

        public DataTable Rpt_StockLoan(string FromDate, string ToDate, string SaleType)
        {
            DataTable dt = new DataTable();
            try
            {
                SqlConnection sqlconn = new SqlConnection(ConnString);

                string Qry = @"Select *,TotalSale-Discount AS [NetBill] From (
                                Select A.DRS_ID,Convert(varchar(11),A.DRS_Date,106) As IssueDate,A.DRS_Name,A.MobileNo,A.Remarks,Convert(Varchar(11),A.LoanReceivedDate,106) as [LoanRecivedDate],(case IsLoan when 1 then 'Pending' else 'Approved' end) As [IsLoan],sum(B.ItemPrice*B.Qty) As[Purchase Price],sum(B.SalesPrice*B.Qty) As [TotalSale],Type,
                                A.Discount from Trans_DRSM A
                                inner join Trans_DRSD B ON A.DRS_ID = B.DRS_ID
                                inner join Items C On B.ItemID = C.ItemID
                                Where Convert(varchar(11),A.DRS_Date,121) between '" + FromDate + @"' and '" + ToDate + @"' and type = '"+SaleType+"' group bY A.DRS_ID,DRS_Date,DRS_Name,a.MobileNo,A.LoanReceivedDate,a.IsLoan,a.Remarks,a.Discount,Type)A order by IssueDate";
                SqlCommand sqlcomm = new SqlCommand(Qry, sqlconn);
                dt = GetdataTable(sqlcomm, sqlconn);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, DataAccess.ProjectName, MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            return dt;
        }

        public DataTable GetPersonRecord(int PersonID)
        {
            DataTable dt = new DataTable();
            try
            {
                SqlConnection sqlconn = new SqlConnection(ConnString);
                string Qry = @"Select LoanReturnID,Convert(Varchar(11),ReturnDate,106) As [Return Date],A.Amount as [Return Amount],Convert(varchar(11),B.Created_At,106)as [Assign Date],B.Amount As [Loan Amount],PersonName,PersonPhone,PersonCNIC,PersonAddress,CityName
from Trans_LoanReturn A
inner join Trans_LoanPosting B On A.PersonID=B.PersonID
inner join Trans_LoanAssign C On A.PersonID = C.PersonID
where A.PersonID='"+PersonID+"' and B.IsActive=1 order  by A.ReturnDate desc";
                SqlCommand sqlcomm = new SqlCommand(Qry, sqlconn);
                dt = GetdataTable(sqlcomm, sqlconn);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, DataAccess.ProjectName, MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            return dt;
        }

        public DataTable RptWalfare(string FromDate, string ToDate)
        {
            DataTable dataTable = new DataTable();
            try
            {
                string Qry = @"Select WelfareID,CONVERT(varchar(11),A.AssignDate,106) As [AssignDate],PersonName,Amount,Description
                                from Trans_Welfare A
                                where IsActive=1 and Convert(Varchar(11),A.AssignDate,121) between '" + FromDate + "' and '" + ToDate + "' ";
                SqlConnection sqlConnection = new SqlConnection(this.ConnString);
                //SqlCommand sqlcomm = new SqlCommand("Select Convert(varchar(11),INV.InvestmentDate,106) AS [InvestmentStartDate],\r\n                               PersonName,PersonPhone,PersonNIC,PersonEmail,\r\n                               IsNULL(sum(amount),0) As [InvestmentAmount], \r\n                               IsNULL((Select IsNULL(sum(IVR.amount),0) AS [ReturnAmount] From Trans_InvestmentReturn IVR Where IVR.InvestorID=INV.InvestorsID And ReturnDate between @FromDate and @ToDate),0) AS [ReturnAmount]\r\n                               From Trans_Investors INV\r\n                               Inner Join Trans_InvestmentPosting INVP on INV.InvestorsID=INVP.InvestorsID\r\n                               Where INV.IsActive=1 And INVP.InvestmentDate between @FromDate and @ToDate\r\n                               Group by INV.InvestmentDate,PersonName,PersonPhone,PersonNIc,PersonEmail,INV.InvestorsID", sqlConnection);
                SqlCommand sqlcomm = new SqlCommand(Qry, sqlConnection);
                sqlcomm.Parameters.AddWithValue("@FromDate", (object)FromDate);
                sqlcomm.Parameters.AddWithValue("@ToDate", (object)ToDate);
                dataTable = this.GetdataTable(sqlcomm, sqlConnection);
            }
            catch (Exception ex)
            {
                int num = (int)MessageBox.Show(ex.Message, DataAccess.ProjectName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
            return dataTable;
        }

        public DataTable RptExpenses(string FromDate, string ToDate)
        {
            DataTable dataTable = new DataTable();
            try
            {
                string Qry = @"Select ExpenseID,CONVERT(Varchar(11),A.AssignDate,106) as [AssignDate],Expenses,Amount,Description 
                                from Trans_Expenses A
                                where IsActive=1 and Convert(Varchar(11),A.AssignDate,121) between '" + FromDate + "' and '" + ToDate + "' order by 1 asc";
                SqlConnection sqlConnection = new SqlConnection(this.ConnString);
                //SqlCommand sqlcomm = new SqlCommand("Select Convert(varchar(11),INV.InvestmentDate,106) AS [InvestmentStartDate],\r\n                               PersonName,PersonPhone,PersonNIC,PersonEmail,\r\n                               IsNULL(sum(amount),0) As [InvestmentAmount], \r\n                               IsNULL((Select IsNULL(sum(IVR.amount),0) AS [ReturnAmount] From Trans_InvestmentReturn IVR Where IVR.InvestorID=INV.InvestorsID And ReturnDate between @FromDate and @ToDate),0) AS [ReturnAmount]\r\n                               From Trans_Investors INV\r\n                               Inner Join Trans_InvestmentPosting INVP on INV.InvestorsID=INVP.InvestorsID\r\n                               Where INV.IsActive=1 And INVP.InvestmentDate between @FromDate and @ToDate\r\n                               Group by INV.InvestmentDate,PersonName,PersonPhone,PersonNIc,PersonEmail,INV.InvestorsID", sqlConnection);
                SqlCommand sqlcomm = new SqlCommand(Qry, sqlConnection);
                sqlcomm.Parameters.AddWithValue("@FromDate", (object)FromDate);
                sqlcomm.Parameters.AddWithValue("@ToDate", (object)ToDate);
                dataTable = this.GetdataTable(sqlcomm, sqlConnection);
            }
            catch (Exception ex)
            {
                int num = (int)MessageBox.Show(ex.Message, DataAccess.ProjectName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
            return dataTable;
        }

        public DataTable RptPreviousRecord(string FromDate, string ToDate)
        {
            DataTable dataTable = new DataTable();
            try
            {
                string Qry = @"Select RecordID,CONVERT(varchar(11),A.Date,106) As [AssignDate],Name,Amount,Description
                                from Trans_PreviousData A
                                where IsActive=1 and Convert(varchar(11),A.Date,121) between '" + FromDate + "' and '" + ToDate + "' order by 1 asc";
                SqlConnection sqlConnection = new SqlConnection(this.ConnString);
                //SqlCommand sqlcomm = new SqlCommand("Select Convert(varchar(11),INV.InvestmentDate,106) AS [InvestmentStartDate],\r\n                               PersonName,PersonPhone,PersonNIC,PersonEmail,\r\n                               IsNULL(sum(amount),0) As [InvestmentAmount], \r\n                               IsNULL((Select IsNULL(sum(IVR.amount),0) AS [ReturnAmount] From Trans_InvestmentReturn IVR Where IVR.InvestorID=INV.InvestorsID And ReturnDate between @FromDate and @ToDate),0) AS [ReturnAmount]\r\n                               From Trans_Investors INV\r\n                               Inner Join Trans_InvestmentPosting INVP on INV.InvestorsID=INVP.InvestorsID\r\n                               Where INV.IsActive=1 And INVP.InvestmentDate between @FromDate and @ToDate\r\n                               Group by INV.InvestmentDate,PersonName,PersonPhone,PersonNIc,PersonEmail,INV.InvestorsID", sqlConnection);
                SqlCommand sqlcomm = new SqlCommand(Qry, sqlConnection);
                sqlcomm.Parameters.AddWithValue("@FromDate", (object)FromDate);
                sqlcomm.Parameters.AddWithValue("@ToDate", (object)ToDate);
                dataTable = this.GetdataTable(sqlcomm, sqlConnection);
            }
            catch (Exception ex)
            {
                int num = (int)MessageBox.Show(ex.Message, DataAccess.ProjectName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
            return dataTable;
        }

        public DataTable RptPreviousGiftRecord(string FromDate, string ToDate)
        {
            DataTable dataTable = new DataTable();
            try
            {
                string Qry = @"Select RecordID,CONVERT(varchar(11),A.Date,106) As [AssignDate],Name,Amount,Description
                                from Trans_PreviousGiftRecord A
                                where IsActive=1 and Convert(varchar(11),A.Date,121) between '" + FromDate + "' and '" + ToDate + "' order by 1 asc";
                SqlConnection sqlConnection = new SqlConnection(this.ConnString);
                //SqlCommand sqlcomm = new SqlCommand("Select Convert(varchar(11),INV.InvestmentDate,106) AS [InvestmentStartDate],\r\n                               PersonName,PersonPhone,PersonNIC,PersonEmail,\r\n                               IsNULL(sum(amount),0) As [InvestmentAmount], \r\n                               IsNULL((Select IsNULL(sum(IVR.amount),0) AS [ReturnAmount] From Trans_InvestmentReturn IVR Where IVR.InvestorID=INV.InvestorsID And ReturnDate between @FromDate and @ToDate),0) AS [ReturnAmount]\r\n                               From Trans_Investors INV\r\n                               Inner Join Trans_InvestmentPosting INVP on INV.InvestorsID=INVP.InvestorsID\r\n                               Where INV.IsActive=1 And INVP.InvestmentDate between @FromDate and @ToDate\r\n                               Group by INV.InvestmentDate,PersonName,PersonPhone,PersonNIc,PersonEmail,INV.InvestorsID", sqlConnection);
                SqlCommand sqlcomm = new SqlCommand(Qry, sqlConnection);
                sqlcomm.Parameters.AddWithValue("@FromDate", (object)FromDate);
                sqlcomm.Parameters.AddWithValue("@ToDate", (object)ToDate);
                dataTable = this.GetdataTable(sqlcomm, sqlConnection);
            }
            catch (Exception ex)
            {
                int num = (int)MessageBox.Show(ex.Message, DataAccess.ProjectName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
            return dataTable;
        }

        public void LoadItemName(UltraCombo cbo)
        {
            SqlConnection connection = new SqlConnection(this.ConnString);
            SqlCommand selectCommand = new SqlCommand("Select ItemID,ItemName from Items where IsActive=1", connection);
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(selectCommand);
            DataTable dataTable = new DataTable();
            sqlDataAdapter.Fill(dataTable);
            cbo.DataSource = dataTable;
            cbo.ValueMember = "ItemID";
            cbo.DisplayMember = "ItemName";
            cbo.DisplayLayout.Bands[0].Columns["ItemID"].Hidden = true;
            cbo.DisplayLayout.AutoFitStyle = AutoFitStyle.ResizeAllColumns;
            cbo.Value = 0;
            connection.Close();
            sqlDataAdapter.Dispose();
            selectCommand.Dispose();
        }

        public DataTable GetSlipReportInUrdu(long IssueID)
        {
            DataTable dt = new DataTable();
            try
            {
                SqlConnection sqlconn = new SqlConnection(ConnString);
                string Qry = @"Select A.DRS_ID as [Bill No],Convert(varchar(11),A.DRS_Date,106) As [BillDate],DRS_Name,MobileNo,Type,Remarks,ItemName as[ItemNameEnglish],ItemNameUrdu,MUnitName,B.SalesPrice,Qty,A.Discount,
                                B.SalesPrice*Qty As [Totalprice]
                                from Trans_DRSM A
                                Inner Join Trans_DRSD B on A.DRS_ID=B.DRS_ID
                                Inner Join Items C on B.ItemID=C.ItemID
                                Inner Join MUnit D on C.MUnitID=D.MUnitID
                                Where A.DRS_ID='"+IssueID+"'";
                SqlCommand sqlcomm = new SqlCommand(Qry, sqlconn);
                dt = GetdataTable(sqlcomm, sqlconn);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, DataAccess.ProjectName, MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            return dt;
        }

    }
}
