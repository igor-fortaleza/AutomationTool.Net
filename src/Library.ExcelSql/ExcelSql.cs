using System;
using System.Data;
using System.Data.OleDb;

namespace Library.ExcelSQL
{
    public class ExcelSql : IDisposable
    {
        private OleDbConnection _olecon;
        private OleDbCommand _oleCmd;
        private OleDbDataReader _reader;

        private String _connectionStr;
        private bool firstExecutation;

        public ExcelSql()
        {
            firstExecutation = true;
        }
        public void Open(string file)
        {
            try
            {                
                _connectionStr = $"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={file};Extended Properties='Excel 12.0 Xml;HDR=YES;ReadOnly=False';";
                _olecon = new OleDbConnection(_connectionStr);
                //_olecon.Open();

                _oleCmd = new OleDbCommand {Connection = _olecon, CommandType = CommandType.Text};
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public OleDbDataReader ExecuteReader(String sqlCommand)
        {
            try
            {
                if (!firstExecutation)
                {
                    _olecon.Close();
                    _olecon.Open();
                    _oleCmd = new OleDbCommand {Connection = _olecon, CommandType = CommandType.Text};
                }
                else
                {
                    _olecon.Open();
                }

                this._oleCmd.CommandText = sqlCommand;
                _reader = this._oleCmd.ExecuteReader();

                firstExecutation = false;

                return _reader;
            }
            catch (Exception x)
            {
                throw x;
            }
        }
        public int ExecuteNonQuery(String sqlCommand)
        {
            try
            {
                if (!firstExecutation)
                {
                    _olecon.Close();
                    _olecon.Open();
                    _oleCmd = new OleDbCommand { Connection = _olecon, CommandType = CommandType.Text };
                }
                else
                {
                    _olecon.Open();
                }

                this._oleCmd.CommandText = sqlCommand;
                firstExecutation = false;

                var result = this._oleCmd.ExecuteNonQuery();

                return result;
            }
            catch (Exception x)
            {
                throw x;
            }
            finally
            {
                _olecon.Close();
            }
        }
        
        public void Dispose()
        {
            _olecon?.Dispose();
            _oleCmd?.Dispose();
        }
    }
}
