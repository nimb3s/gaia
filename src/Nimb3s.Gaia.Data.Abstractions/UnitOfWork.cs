using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Nimb3s.Data.Abstractions
{
    //https://stackoverflow.com/questions/31298235/how-to-implement-unit-of-work-pattern-with-dapper
    public interface IUnitOfWorkFactory
    {
        UnitOfWork Create();
    }

    public interface IDbContext
    {
        void Commit();
        void Rollback();
    }

    public interface IUnitOfWork
    {
        IDbTransaction Transaction { get; }

        void Commit();
        void Rollback();
    }

    public class DbContext : IDbContext
    {
        private IUnitOfWorkFactory unitOfWorkFactory;

        private UnitOfWork unitOfWork;

        public DbContext(IUnitOfWorkFactory unitOfWorkFactory)
        {
            this.unitOfWorkFactory = unitOfWorkFactory;
        }

        protected UnitOfWork UnitOfWork =>
            unitOfWork ?? (unitOfWork = unitOfWorkFactory.Create());

        public void Commit()
        {
            try
            {
                UnitOfWork.Commit();
            }
            finally
            {
                Reset();
            }
        }

        public void Rollback()
        {
            try
            {
                UnitOfWork.Rollback();
            }
            finally
            {
                Reset();
            }
        }

        private void Reset()
        {
            unitOfWork = null;
        }
    }

    public class UnitOfWork : IUnitOfWork
    {
        private IDbTransaction transaction;

        public UnitOfWork(IDbConnection connection)
        {
            transaction = connection.BeginTransaction();
        }

        public IDbTransaction Transaction =>
            transaction;


        public void Commit()
        {
            try
            {
                transaction.Commit();
                transaction.Connection?.Close();
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
            finally
            {
                transaction?.Dispose();
                transaction.Connection?.Dispose();
                transaction = null;
            }
        }

        public void Rollback()
        {
            try
            {
                transaction.Rollback();
                transaction.Connection?.Close();
            }
            catch
            {
                throw;
            }
            finally
            {
                transaction?.Dispose();
                transaction.Connection?.Dispose();
                transaction = null;
            }
        }
    }

    public class UnitOfWorkFactory<TConnection> : IUnitOfWorkFactory where TConnection : IDbConnection, new()
    {
        private string connectionString;

        public UnitOfWorkFactory(string connectionString)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new ArgumentNullException("connectionString cannot be null");
            }

            this.connectionString = connectionString;
        }

        public UnitOfWork Create()
        {
            return new UnitOfWork(CreateOpenConnection());
        }

        private IDbConnection CreateOpenConnection()
        {
            var conn = new TConnection();
            conn.ConnectionString = connectionString;

            try
            {
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
            }
            catch (Exception exception)
            {
                throw new Exception("An error occured while connecting to the database. See innerException for details.", exception);
            }

            return conn;
        }
    }
}
