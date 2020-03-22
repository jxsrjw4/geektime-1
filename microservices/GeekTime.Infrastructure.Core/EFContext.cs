using DotNetCore.CAP;
using GeekTime.Domain;
using GeekTime.Infrastructure.Core.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;

namespace GeekTime.Infrastructure.Core
{
    public class EFContext : DbContext, IUnitOfWork, ITransaction
    {
        protected IMediator _mediator;
        ICapPublisher _capBus;

        public EFContext(DbContextOptions options, IMediator mediator, ICapPublisher capBus) : base(options)
        {
            _mediator = mediator;
            _capBus = capBus;
        }

        #region IUnitOfWork
        public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default)
        {
            var result = await base.SaveChangesAsync(cancellationToken);
            await _mediator.DispatchDomainEventsAsync(this);
            return true;
        }
        #endregion

        #region ITransaction

        private IDbContextTransaction _currentTransaction;
        public IDbContextTransaction GetCurrentTransaction() => _currentTransaction;
        public bool HasActiveTransaction => _currentTransaction != null;
        public Task<IDbContextTransaction> BeginTransactionAsync()
        {
            if (_currentTransaction != null) return null;
            _currentTransaction = Database.BeginTransaction(_capBus, autoCommit: false);
            return Task.FromResult(_currentTransaction);
        }

        public async Task CommitTransactionAsync(IDbContextTransaction transaction)
        {
            if (transaction == null) throw new ArgumentNullException(nameof(transaction));
            if (transaction != _currentTransaction) throw new InvalidOperationException($"Transaction {transaction.TransactionId} is not current");

            try
            {
                await SaveChangesAsync();
                transaction.Commit();
            }
            catch
            {
                RollbackTransaction();
                throw;
            }
            finally
            {
                if (_currentTransaction != null)
                {
                    _currentTransaction.Dispose();
                    _currentTransaction = null;
                }
            }
        }

        public void RollbackTransaction()
        {
            try
            {
                _currentTransaction?.Rollback();
            }
            finally
            {
                if (_currentTransaction != null)
                {
                    _currentTransaction.Dispose();
                    _currentTransaction = null;
                }
            }
        }


		#endregion

		#region 执行存储过程
		public void ExceSP(string SpName, DataSet IOParameterDataSet, out int Return_value, out DataSet IOParameterDataSetReturn)
		{
			Return_value = -1;
			IOParameterDataSetReturn = null;


			var sqlCommand = new SqlCommand();
			DataSet result2 = new DataSet();
			if (IOParameterDataSet != null && IOParameterDataSet.Tables.Count > 0 && IOParameterDataSet.Tables[0].Columns.Count > 0)
			{
				sqlCommand.CommandText = SpName;
				for (int i = 0; i < IOParameterDataSet.Tables[0].Rows.Count; i++)
				{
					bool flag = false;
					string parameterName = IOParameterDataSet.Tables[0].Rows[i]["FieldName"].ToString();
					if (IOParameterDataSet.Tables[0].Rows[i]["IsOutput"].ToString().ToUpper().Trim() == "TRUE")
					{
						flag = true;
					}
					int size = 0;
					string text = IOParameterDataSet.Tables[0].Rows[i]["FieldType"].ToString();
					if (text.ToUpper() == "Bit".ToUpper())
					{
						sqlCommand.Parameters.Add(new SqlParameter(parameterName, SqlDbType.Bit));
					}
					if (text.ToUpper() == "DateTime".ToUpper())
					{
						sqlCommand.Parameters.Add(new SqlParameter(parameterName, SqlDbType.DateTime));
					}
					if (text.ToUpper() == "Decimal".ToUpper())
					{
						sqlCommand.Parameters.Add(new SqlParameter(parameterName, SqlDbType.Decimal));
					}
					if (text.ToUpper() == "Int".ToUpper())
					{
						sqlCommand.Parameters.Add(new SqlParameter(parameterName, SqlDbType.BigInt));
					}
					if (text.ToUpper() == "NVarChar".ToUpper())
					{
						sqlCommand.Parameters.Add(new SqlParameter(parameterName, SqlDbType.NVarChar, 4000));
					}
					if (text.ToUpper() == "Xml".ToUpper())
					{
						if (IOParameterDataSet.Tables[0].Rows[i]["FieldValue"] != DBNull.Value)
						{
							size = IOParameterDataSet.Tables[0].Rows[i]["FieldValue"].ToString().Trim().Length;
						}
						sqlCommand.Parameters.Add(new SqlParameter(parameterName, SqlDbType.Xml, size));
					}
					if (!(text.ToUpper() == "Bit".ToUpper()) || !(IOParameterDataSet.Tables[0].Rows[i]["FieldValue"].ToString() == ""))
					{
						sqlCommand.Parameters[parameterName].Value = IOParameterDataSet.Tables[0].Rows[i]["FieldValue"];
					}
					if (flag)
					{
						sqlCommand.Parameters[parameterName].Direction = ParameterDirection.InputOutput;
					}
				}
				sqlCommand.Parameters.Add("RETURN_VALUE", SqlDbType.Int);
				sqlCommand.Parameters["RETURN_VALUE"].Direction = ParameterDirection.ReturnValue;


				using (SqlConnection sqlConnection = new SqlConnection(Database.GetDbConnection().ConnectionString))
				{
					sqlConnection.Open();
					sqlCommand.CommandTimeout = sqlConnection.ConnectionTimeout;
					sqlCommand.Connection = sqlConnection;
					sqlCommand.CommandType = CommandType.StoredProcedure;
					DataSet dataSet = new DataSet("WCFSQLDataSet");
					new SqlDataAdapter
					{
						SelectCommand = sqlCommand
					}.Fill(dataSet, "WCFSQLDataSet");
					result2 = dataSet;
					sqlConnection.Close();
					for (int i = 0; i < sqlCommand.Parameters.Count; i++)
					{
						if (sqlCommand.Parameters[i].Direction.ToString() == "InputOutput")
						{
							for (int j = 0; j < IOParameterDataSet.Tables[0].Rows.Count; j++)
							{
								if (IOParameterDataSet.Tables[0].Rows[j]["IsOutput"].ToString().ToUpper().Trim() == "TRUE" && IOParameterDataSet.Tables[0].Rows[j]["FieldName"].ToString().ToUpper() == sqlCommand.Parameters[i].ToString().ToUpper())
								{
									IOParameterDataSet.Tables[0].Rows[j]["FieldValue"] = sqlCommand.Parameters[i].Value;
								}
							}
						}
						if (sqlCommand.Parameters[i].Direction.ToString() == "ReturnValue")
						{
							Return_value = (int)sqlCommand.Parameters["RETURN_VALUE"].Value;
						}
					}
					IOParameterDataSet.AcceptChanges();
					IOParameterDataSetReturn = IOParameterDataSet;
				}
			}
		}
        #endregion
    }
}
