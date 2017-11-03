using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Oracle.DataAccess.Types;
using Oracle.DataAccess.Client;
using CanadianNatural.ApplicationServices.Security.AppSecure;
using CanadianNatural.ApplicationServices.Security.Interface;

namespace OracleData
{
    public class DBAccess
    {
        private static OracleTransaction moDbTransaction = null;
        private static OracleConnection DataRepositoryCon;
        private static System.Data.IDbConnection DBCon;
        private ICNRLUser mCurrentUser = null;

        //private string AS_URL;
        //private string AS_Environment ;
        //private string AS_AppID;
        private string AppSecureURL;
        private string AppSecureEnvironment;
        private string AppSecureAppID;
        private string AS_ConnectionName;

        public DBAccess()
        {
            // DataRepositoryCon = new OracleConnection(GetConnectionString());
            //
            DBCon = new OracleConnection();

            AppSecureURL = Properties.Settings.Default.appSecureURL;
            AppSecureAppID = Properties.Settings.Default.appSecureAppId;
            AppSecureEnvironment = Properties.Settings.Default.appSecureEnvironment;
            AS_ConnectionName = Properties.Settings.Default.AppSecureConnString;
        }

        public static void BeginTransaction()
        {
            // moDbTransaction = DataRepositoryCon.BeginTransaction();
            moDbTransaction = ((OracleConnection)DBCon).BeginTransaction();
        }

        public static void EndTransaction()
        {
            moDbTransaction = null;
        }

        public static void DBCommit()
        {
            moDbTransaction.Commit();
        }

        public static void DBRollback()
        {
            moDbTransaction.Rollback();
        }

        public string AppSecureConnectionName
        {
            get
            {
                return AS_ConnectionName;
            }
            set
            {
                AS_ConnectionName = value;
            }
        }

        public int MyProperty { get; set; }

        public bool ConnectToDataRepository1()
        {
            try
            {
                //mCurrentUser.Applicaton.OpenDBConnection(AppSecureConnectionName, ref (System.Data.IDbConnection)DataRepositoryCon);
                OracleConnection conn = new OracleConnection(GetConnectionString());
            }
            catch (Exception ex)
            {
                throw new Exception("clsDBAccess.ConnectToDataRepository1. Error:" + ex.Message);
            }

            return true;
        }

        public bool ConnectToDataRepository()
        {
            Int32 c = 52;
            Int32 d = 99;
            Console.WriteLine("values is " + (c + d));
            ICNRLAuthenticationProvider authenticationProvider = new SecurityProvider();
            string sErrorMessage = string.Empty;

            try
            {
                mCurrentUser = authenticationProvider.Login(ICNRLAuthenticationProvider.UserLoginType.Automatic,
                    AppSecureURL, AppSecureEnvironment, AppSecureAppID, "", "", ref sErrorMessage);
                mCurrentUser.Applicaton.OpenDBConnection(AppSecureConnectionName, ref DBCon);
            }
            catch (Exception ex)
            {
                throw new Exception("clsDBAccess.ConnectToDataRepository. Error:" + ex.Message);
            }

            return true;
        }

        public void CreateMyOracleDataReader(string queryString)
        {
            OracleConnection conn = (OracleConnection)DBCon;
            using (OracleCommand command = new OracleCommand(queryString, conn))
            {
                // conn.Open();
                OracleDataReader reader = command.ExecuteReader();
                try
                {
                    while (reader.Read())
                    {
                        Console.WriteLine(reader.GetValue(0));
                    }
                }
                finally
                {
                    reader.Close();
                }
            }
        }

        private static void OpenSqlConnection()
        {
            string connString = GetConnectionString();
            //using (SqlConnection conn = new SqlConnection(connString))
            //{
            //    conn.open();
            //    Console.WriteLine("ServerVersion:{0}", conn.serverversion);
            //}
        }

        static private string GetConnectionString()
        {
            // To avoid storing the connection string in your code,  
            // you can retrieve it from a configuration file, using the  
            // System.Configuration.ConfigurationSettings.AppSettings property  
            return "Data Source=ARSDEV1;User Id=REMEDYFM;Password=MssyCrflGes$78;";
        }

        #region "update data"

        public System.Data.DataTable GetDataViaPackage(String vsProcedureName)
        {
            OracleCommand loDBCommand = CreateDbCommand(vsProcedureName);
            return GetDataViaDataTable(loDBCommand);
        }

        public Int32 GetDataViaProcedure(String vsProcedureName)
        {
            OracleCommand loDBCommand = CreateDbCommand(vsProcedureName);
            return TestProcedure(loDBCommand);
        }

        protected OracleCommand CreateCommandBase(string storedProcName)
        {
            Oracle.DataAccess.Client.OracleCommand command = new Oracle.DataAccess.Client.OracleCommand(storedProcName);
            command.CommandType = System.Data.CommandType.StoredProcedure;
            return command;
        }

        protected OracleCommand CreateDbCommand(string storedProcName)
        {
            OracleCommand loDbComm = CreateCommandBase(storedProcName);
            OracleConnection conn = (OracleConnection)DBCon;

            loDbComm.Connection = conn;

            if (moDbTransaction != null)
            {
                loDbComm.Transaction = moDbTransaction;
            }

            return loDbComm;
        }

        protected System.Data.DataTable GetDataViaDataTable(OracleCommand voDBCommand)
        {
            System.Data.DataTable dtData = new System.Data.DataTable();
            AddedOutputRefCursor(voDBCommand, "CUR_OUT");
            //voDBCommand.CommandType = System.Data.CommandType.StoredProcedure;
            //voDBCommand.Connection = (OracleConnection)DBCon;

            using (OracleDataAdapter oda = new OracleDataAdapter(voDBCommand))
            {
                // voDBCommand.ExecuteNonQuery();
                voDBCommand.ExecuteNonQuery();
                if (voDBCommand.Parameters["CUR_OUT"].Value.ToString() != string.Empty)
                {
                    oda.Fill(dtData);
                    return dtData;
                }
            }

            return new System.Data.DataTable();
        }

        public Int32 TestProcedure(OracleCommand voDBCommand)
        {
            System.Data.DataTable dtData = null;
            AddedOutputVarChar(voDBCommand, "CUR_OUT", 256);
            voDBCommand.CommandType = System.Data.CommandType.StoredProcedure;
            voDBCommand.Connection = (OracleConnection)DBCon;

            using (OracleDataAdapter oda = new OracleDataAdapter(voDBCommand))
            {
                // voDBCommand.ExecuteNonQuery();
                voDBCommand.ExecuteReader();
                if (voDBCommand.Parameters["cur_out"].Value.ToString() != string.Empty)
                {
                    //oda.Fill(dtData);
                    return Convert.ToInt32(voDBCommand.Parameters["cur_out"].Value);
                }
            }

            return 1;
        }

        protected void AddedOutputRefCursor(OracleCommand voDBCommand, string vsParameterName)
        {
            AddedOutputParamater(voDBCommand, vsParameterName, OracleDbType.RefCursor, 1024);
        }

        protected void AddedOutputVarChar(OracleCommand voDBCommand, string sName, Int32 dLength)
        {
            AddedOutputParamater(voDBCommand, sName, OracleDbType.Varchar2, dLength);
        }

        private void AddedOutputParamater(OracleCommand voDBComm, string vsParameterName,
             Oracle.DataAccess.Client.OracleDbType voOracleType, int vsLength)
        {
            voDBComm.Parameters.AddRange(new Oracle.DataAccess.Client.OracleParameter[] {
            new Oracle.DataAccess.Client.OracleParameter(vsParameterName, voOracleType, vsLength,
            System.Data.ParameterDirection.Output, true, 0,
            0, "", System.Data.DataRowVersion.Current, null)});
        }

        private void AddedInputParamater(OracleCommand voDBCommand,
                                      String sParameterName,
                                      Object voValue,
                                     Oracle.DataAccess.Client.OracleDbType oOracleType)
        {
            voDBCommand.Parameters.AddRange(new Oracle.DataAccess.Client.OracleParameter[] { 
           new Oracle.DataAccess.Client.OracleParameter(sParameterName, oOracleType)});
            voDBCommand.Parameters[sParameterName].Value = voValue;
        }

        protected void AddedInputVarChar(OracleCommand voDBCommand,
                                     String vsParameterName, String vsValue)
        {
            AddedInputParamater(voDBCommand, vsParameterName, vsValue, OracleDbType.Varchar2);
        }
        #endregion
    }


}
