using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using BusinessEntities;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Configuration;
namespace DAL
{
    public class DAL_FrmRptDueDate
    {
        string ConnString = DataAccess.ConnString;
        //string ConnString = "Data Source=HADI-B94B6CB176;Initial Catalog=RashanGhar;User id=sa;password=iloveyoumuhammad786";
        //string ConnString = @"Data Source=JAWAD-PC\FAHAD;Initial Catalog=RashanGhar;user id=sa;password=12345;";
        //string ConnString = "Data Source=FAHAD;Initial Catalog=RashanGhar;User id=sa;password=123456";

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

        public DataTable GetReportData(string Options,string FromDate,string ToDate)
        {
            string qry = "";
            DataTable dt = new DataTable();
            try
            {
                string Newqry = Options.Trim(' ');
                SqlConnection sqlconn = new SqlConnection(ConnString);
                SqlCommand sqlcomm = new SqlCommand();
                if (Newqry.Equals(""))
                {
                    qry = @"Select ClientID,MemberCode,dbo.InItCap(MemberName) AS [MemberName],dbo.InItCap(Address) AS [Address],
                            dbo.InItCap(EmpName) AS [EmpName],Area,CellNo,dbo.InItcap(NearBy) AS [NearBy],BirthDate,DueDate,IssueDate,AmountDisb From (
                            Select ClientID,MemberCode,MemberName + ' ' + Relation + ' ' + HusbandName As [MemberName],Address,EmpName,Area,CellNo,NearBy,BirthDate,DueDate,IssueDate,
                            AmountDisb From (
                            Select ClientID,MemberCode,MemberName,HusbandName,Address,EmpName,Area,CellNo,NearBy,
                            (Case Relation when '1' then 'W/O' when '3' then 'D/O' when '4' then 'S/O' when '9' then 'DIA/O' 
                            when '10' then 'MIA/O' when '13' then 'C/O' else '' end) AS [Relation],BirthDate,DueDate,IssueDate,AmountDisb 
                             From (
                            Select Acc.ClientID,ClientCode As [MemberCode],HusbandName,ClientName AS [MemberName],Cl.Address,EmpName,
                            AreaDescription AS [Area],CellNo,NearBy,IsNull(RelationToClient,0) AS [Relation],Convert(varchar(12),DOB,106) AS [BirthDate],
                            Convert(varchar(12),DueDate,106) As [DueDate],Convert(varchar(12),IssueDate,106) As [IssueDate],
                            SUM(Convert(int,NetAmount))-sum(Convert(int,Installment)) As [AmountDisb]
                            From Trans_AccountHistory Acc
                            Inner Join Trans_Clients Cl on Acc.ClientID=cl.ClientID
                            Inner Join Areas_Def Area on Cl.AreaID=Area.AreaID
                            Inner JOin Emp_Def Emp on Cl.EmpCode=emp.EmpCode
                            Left Join Relation_Def Rel on Cl.RelationToClient=Rel.RelationID
                            Where Convert(varchar(11),DueDate,121) between '"+FromDate+ @"' And '"+ ToDate+ @"'
                            Group by Acc.ClientID,DueDate,HusbandName,IssueDate,ClientCode,DOB ,AreaDescription,CellNo,RelationToClient,NearBy,ClientName,Cl.Address,EmpName) AA
                            Where AmountDisb>0) AAA) ABC
                            Order by issueDate asc";
                    sqlcomm.CommandText = qry;
                    dt = GetDataTable(qry, sqlconn);
                }
                else
                {
                    qry =    " Select ClientID,MemberCode,dbo.InItCap(MemberName + ' ' + Relation + ' ' + HusbandName) As [MemberName],dbo.InItCap(Address) As [Address],dbo.InItCap(EmpName) As [EmpName],Area,CellNo,dbo.InItCap(NearBy) As [NearBy],BirthDate,DueDate,IssueDate, " +
                             " AmountDisb From (  " +
                             " Select ClientID,MemberCode,MemberName,HusbandName,Address,EmpName,Area,CellNo,NearBy, " +
                             " (Case Relation when '1' then 'W/O' when '3' then 'D/O' when '4' then 'S/O' when '9' then 'DIA/O'  " +
                             " when '10' then 'MIA/O' when '13' then 'C/O' else '' end) AS [Relation],BirthDate,DueDate,IssueDate,AmountDisb  " +
                             " From ( " +
                             " Select Acc.ClientID,ClientCode As [MemberCode],HusbandName,ClientName AS [MemberName],Cl.Address,EmpName, " +
                             " AreaDescription AS [Area],CellNo,NearBy,IsNull(RelationToClient,0) AS [Relation],Convert(varchar(12),DOB,106) AS [BirthDate], " +
                             " Convert(varchar(12),DueDate,106) As [DueDate],Convert(varchar(12),IssueDate,106) As [IssueDate], " +
                             " SUM(Convert(int,NetAmount))-sum(Convert(int,Installment)) As [AmountDisb] " +
                             " From Trans_AccountHistory Acc " +
                             " Inner Join Trans_Clients Cl on Acc.ClientID=cl.ClientID " +
                             " Inner Join Areas_Def Area on Cl.AreaID=Area.AreaID " +
                             " Inner JOin Emp_Def Emp on Cl.EmpCode=emp.EmpCode " +
                             " Left Join Relation_Def Rel on Cl.RelationToClient=Rel.RelationID " +
                             " where 1=1 "+ Newqry+" And Convert(varchar(11),dueDate,121) between '"+FromDate+"' And '"+ToDate+"'" +
                             " Group by Acc.ClientID,DueDate,HusbandName,IssueDate,ClientCode,DOB ,AreaDescription,CellNo,RelationToClient,NearBy,ClientName,Cl.Address,EmpName) AA " +
                             " Where AmountDisb>0) AAA " +
                             " Order by issueDate Asc";
                    sqlcomm.CommandText = qry;
                    dt = GetDataTable(qry, sqlconn);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, DataAccess.ProjectName, MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            return dt;
        }

        public DataTable ShowReportData(string param)
        {
            DataTable dt = new DataTable();
            try
            {
                //string SqlQry = "";
                param.Trim(' ');
                if (param.Equals(""))
                {
                    SqlConnection sqlconn = new SqlConnection(ConnString);
                    SqlCommand sqlcomm = new SqlCommand("RptAllClientDueDate", sqlconn);
                    sqlcomm.CommandType = CommandType.StoredProcedure;
                    dt = GetdataTable(sqlcomm, sqlconn);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, DataAccess.ProjectName, MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            return dt;
        }

    }
}
