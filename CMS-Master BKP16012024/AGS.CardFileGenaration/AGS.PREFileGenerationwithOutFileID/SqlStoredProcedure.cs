using System;
using System.Data;
using System.Data.SqlClient;
using System.Xml;
using System.Diagnostics;

namespace ReflectionIT.Common.Data.SqlClient {
    /// <summary>
    /// Represents a stored procedure to execute against a SQL Server database. 
    /// </summary>
    /// <remarks>
    /// Created by: Fons Sonnemans, Reflection IT
    /// 
    /// The generic SqlStoredProcedure class is a SqlCommand wrapper for SQL Server 
    /// which offers next to all normal 'execute' mehods some extra features: 
    /// - The type is set to 'StoredProcedure'. 
    /// - The Timeout is default set to 0. 
    /// - A 'non-open' Connection is Opened and Closed automatically. 
    /// - ReturnValue support. 
    /// - ExecuteDataSet and ExecuteDataTable methods 
    /// - Advanced Error handling is provided with Typed DalExceptions
    /// - Tracing support
    /// - Advanced Parameter support: 
    ///     - Can convert empty strings to NULL (simplifies NOT NULL constraints)
    ///     - NULL = dbNull.Value
    /// 
    /// For questions and comments: Fons.Sonnemans@reflectionit.nl
    /// </remarks>
    /// 
    public class SqlStoredProcedure : IDisposable {
        public static readonly TraceSource TraceSource = new TraceSource("SqlStoredProcedure");
        private static int _eventId = 0;

        private const string _returnValue = "ReturnValue";
        private SqlCommand _command;
        private bool _connectionOpened = false;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="name">Stored Procedure name</param>
        /// <param name="connection">A Data.SqlClients.SqlConnection that represents the connection to an instance of SQL Server.</param>
        public SqlStoredProcedure(string name, SqlConnection connection, CommandType comtype)
            : this(name, connection, null, comtype) {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="name">Stored Procedure name</param>
        /// <param name="connection">A Data.SqlClients.SqlConnection that represents the connection to an instance of SQL Server.</param>
        /// <param name="transaction">The SqlTransaction in which the SqlCommand executes.</param>
        public SqlStoredProcedure(string name, SqlConnection connection, SqlTransaction transaction, CommandType comtype) {
            if (name.IndexOf('.') == -1) {
                throw new ArithmeticException("In the name the owner of a procedure must be specified, this improves performance");
            }
            _command = new SqlCommand(name, connection, transaction);
            _command.CommandTimeout = 0;
            //_command.CommandType = CommandType.StoredProcedure;
            _command.CommandType = comtype;
            AddReturnValue();
        }

        /// <summary>
        /// Close the Command
        /// </summary>
        public void Dispose() {
            if (_command != null) {
                _command.Dispose();
                _command = null;
            }
        }

        /// <summary>
        /// Gets or sets the name of the StoredProcedure
        /// </summary>
        virtual public string Name {
            get { return _command.CommandText; }
            set { _command.CommandText = value; }
        }

        /// <summary>
        /// Gets or sets the wait time before terminating the attempt to execute a command and generating an error.
        /// </summary>
        virtual public int Timeout {
            get { return _command.CommandTimeout; }
            set { _command.CommandTimeout = value; }
        }

        /// <summary>
        /// Gets the SqlCommand encapsulated in this object
        /// </summary>
        virtual public SqlCommand Command {
            get { return _command; }
        }

        /// <summary>
        /// Gets or sets the SqlConnection used by this instance of the Stored Procedure.
        /// </summary>
        virtual public SqlConnection Connection {
            get { return _command.Connection; }
            set { _command.Connection = value; }
        }

        /// <summary>
        /// Gets or sets the SqlTransaction used by this instance of the Stored Procedure.
        /// </summary>
        virtual public SqlTransaction Transaction {
            get { return _command.Transaction; }
            set { _command.Transaction = value; }
        }

        /// <summary>
        /// Gets the SqlParameterCollection.
        /// </summary>
        virtual public SqlParameterCollection Parameters {
            get { return _command.Parameters; }
        }

        /// <summary>
        /// Gets the ReturnValue.
        /// </summary>
        /// <returns>The ReturnValue value.</returns>
        virtual public int ReturnValue {
            get { return (int)_command.Parameters[_returnValue].Value; }
        }

        /// <summary>
        /// Add a parameter to the collection.
        /// </summary>
        /// <param name="parameterName">The name of the parameter to map.</param>
        /// <param name="dbType">One of the SqlDbType values.</param>
        /// <param name="size">The width of the parameter, <=0 will autodetect it using the dbType</param>
        /// <param name="direction">One of the ParameterDirection values. </param>
        /// <returns>The SqlParameter object added to the Parameters collection.</returns>
        virtual public SqlParameter AddParameter(string parameterName,
            SqlDbType dbType,
            int size,
            ParameterDirection direction) {
            SqlParameter p;

            if (size > 0) {
                p = new SqlParameter(parameterName, dbType, size);
            } else {
                // size is automacally detected using dbType
                p = new SqlParameter(parameterName, dbType);
            }

            p.Direction = direction;

            Parameters.Add(p);
            return p;
        }

        /// <summary>
        /// Add a parameter to the collection.
        /// </summary>
        /// <param name="parameterName">The name of the parameter to map.</param>
        /// <param name="dbType">One of the SqlDbType values.</param>
        /// <param name="size">The width of the parameter, <=0 will autodetect it using the dbType</param>
        /// <param name="direction">One of the ParameterDirection values. </param>
        /// <param name="value">An Object that is the value of the SqlParameter. </param>
        /// <returns>The SqlParameter object added to the Parameters collection.</returns>
        virtual public SqlParameter AddParameterWithValue(string parameterName,
            SqlDbType dbType,
            int size,
            ParameterDirection direction,
            object value) {

            SqlParameter p = this.AddParameter(parameterName, dbType, size, direction);

            if (value == null) {
                value = DBNull.Value;
            }

            p.Value = value;

            return p;
        }

        /// <summary>
        /// Add a string parameter to the collection specifying whether a empty string is threated as a NULL value
        /// </summary>
        /// <param name="parameterName">The name of the parameter to map.</param>
        /// <param name="dbType">One of the SqlDbType values.</param>
        /// <param name="size">The width of the parameter, <=0 will autodetect it using the dbType</param>
        /// <param name="direction">One of the ParameterDirection values. </param>
        /// <param name="value">An string that is the value of the SqlParameter. </param>
        /// <param name="emptyIsDBNull">True when an Empty string is stored as an DBNull value</param>
        /// <returns>The SqlParameter object added to the Parameters collection.</returns>
        virtual public SqlParameter AddParameterWithStringValue(string parameterName,
            SqlDbType dbType,
            int size,
            ParameterDirection direction,
            string value,
            bool emptyIsDBNull) {

            SqlParameter p = this.AddParameter(parameterName, dbType, size, direction);

            if (value == null) {
                p.Value = DBNull.Value;
            } else {
                value = value.TrimEnd(' ');
                if (emptyIsDBNull && value.Length == 0) {
                    p.Value = DBNull.Value;
                } else {
                    p.Value = value;
                }
            }

            return p;
        }

        /// <summary>
        /// Add the ReturnValue parameter
        /// </summary>
        /// <returns>The SqlParameter object added to the Parameters collection.</returns>
        virtual protected SqlParameter AddReturnValue() {
            SqlParameter p = Parameters.Add(
                new SqlParameter(_returnValue,
                    SqlDbType.Int,
                /* int size */ 4,
                    ParameterDirection.ReturnValue,
                /* bool isNullable */ false,
                /* byte precision */ 0,
                /* byte scale */ 0,
                /* string srcColumn */ string.Empty,
                    DataRowVersion.Default,
                /* value */ null));

            return p;
        }

        /// <summary>
        /// Executes a Transact-SQL statement against the Connection and 
        /// returns the number of rows affected.
        /// </summary>
        /// <returns>The number of rows affected.</returns>
        virtual public int ExecuteNonQuery() {
            int rowsAffected = -1;

            try {
                Prepare("ExecuteNonQuery");

                rowsAffected = _command.ExecuteNonQuery();

                TraceResult("RowsAffected = " + rowsAffected.ToString());
            } catch (SqlException e) {
                throw TranslateException(e);
            } finally {
                CloseOpenedConnection();
            }
            return rowsAffected;
        }

        /// <summary>
        /// Executes the StoredProcedure on the Connection and builds a SqlDataReader object.
        /// </summary>
        /// <returns>The SqlDataReader</returns>
        virtual public SqlDataReader ExecuteReader() {
            SqlDataReader reader;
            try {
                Prepare("ExecuteReader");

                reader = _command.ExecuteReader();

                TraceResult(null);
            } catch (SqlException e) {
                throw TranslateException(e);
            } finally {
                //CloseOpenedConnection();
            }
            return reader;
        }

        /// <summary>
        /// Executes the StoredProcedure on the Connection and builds a SqlDataReader using one of the CommandBehavior values.
        /// </summary>
        /// <param name="behavior">One of the CommandBehavior values.</param>
        /// <returns>A SqlDataReader object.</returns>
        virtual public SqlDataReader ExecuteReader(CommandBehavior behavior) {
            SqlDataReader reader;
            try {
                Prepare("ExecuteReader");

                reader = _command.ExecuteReader(behavior);

                TraceResult(null);
            } catch (SqlException e) {
                throw TranslateException(e);
            } finally {
                CloseOpenedConnection();
            }
            return reader;
        }

        /// <summary>
        /// Executes the Stored Procedure, and returns the first column of the first row in the resultset returned by the query. Extra columns or rows are ignored.
        /// </summary>
        /// <returns>The first column of the first row in the resultset.</returns>
        virtual public object ExecuteScalar() {
            object val = null;

            try {
                Prepare("ExecuteScalar");

                val = _command.ExecuteScalar();

                TraceResult("Scalar Value = " + Convert.ToString(val));
            } catch (SqlException e) {
                throw TranslateException(e);
            } finally {
                CloseOpenedConnection();
            }
            return val;
        }

        /// <summary>
        /// Executes the StoredProcedure on the Connection and builds a XmlReader object.
        /// </summary>
        /// <returns>A XmlReader object.</returns>
        virtual public XmlReader ExecuteXmlReader() {
            XmlReader reader;

            try {
                Prepare("ExecuteXmlReader");

                reader = _command.ExecuteXmlReader();

                TraceResult(null);
            } catch (SqlException e) {
                throw TranslateException(e);
            } finally {
                CloseOpenedConnection();
            }
            return reader;
        }

        /// <summary>
        /// Executes the StoredProcedure on the Connection and builds a DataSet object.
        /// </summary>
        /// <returns>A DataSet object.</returns>
        virtual public DataSet ExecuteDataSet() {
            DataSet dataset = new DataSet();

            this.ExecuteDataSet(dataset);

            return dataset;
        }

        /// <summary>
        /// Executes the StoredProcedure on the Connection and fill the given DataSet object.
        /// </summary>
        /// <param name="dataSet">The DataSet to be filled</param>
        /// <returns>A DataSet object.</returns>
        virtual public DataSet ExecuteDataSet(DataSet dataSet) {
            try {
                Prepare("ExecuteDataSet");

                SqlDataAdapter a = new SqlDataAdapter(this.Command);
                a.Fill(dataSet);

                TraceResult("# Tables in DataSet = " + dataSet.Tables.Count);
            } catch (SqlException e) {
                throw TranslateException(e);
            } finally {
                CloseOpenedConnection();
            }
            return dataSet;
        }

        /// <summary>
        /// Executes the StoredProcedure on the Connection and builds a DataTable object.
        /// </summary>
        /// <returns>A DataTable object.</returns>
        virtual public DataTable ExecuteDataTable() {
            DataTable dt = null;
            try {
                Prepare("ExecuteDataTable");

                SqlDataAdapter a = new SqlDataAdapter(this.Command);
                dt = new DataTable();
                a.Fill(dt);

                TraceResult("# Rows in DataTable = " + dt.Rows.Count);
            } catch (SqlException e) {
                throw TranslateException(e);
            } finally {
                CloseOpenedConnection();
            }

            return dt;
        }

        /// <summary>
        /// Translate a SqlException to the correct DALException
        /// </summary>
        /// <param name="ex">SqlException to be translated</param>
        /// <returns>An DALException</returns>
        protected Exception TranslateException(SqlException ex) {
            Exception dalException = null;

            SqlStoredProcedure.TraceSource.TraceEvent(TraceEventType.Error, _eventId, "{0} throwed exception: {1}", this.Name, ex.ToString());

            // Return the first Custom exception thrown by a RAISERROR
            foreach (SqlError error in ex.Errors) {
                if (error.Number >= 50000) {
                    dalException = new DalException(error.Message, ex);
                }
            }

            if (dalException == null) {
                // uses SQLServer 2000 ErrorCodes
                switch (ex.Number) {
                    case 17:
                    // 	SQL Server does not exist or access denied.
                    case 4060:
                    // Invalid Database
                    case 18456:
                        // Login Failed
                        dalException = new DalLoginException(ex.Message, ex);
                        break;
                    case 547:
                        // ForeignKey Violation
                        dalException = new DalForeignKeyException(ex.Message, ex);
                        break;
                    case 1205:
                        // DeadLock Victim
                        dalException = new DalDeadLockException(ex.Message, ex);
                        break;
                    case 2627:
                    case 2601:
                        // Unique Index/Constriant Violation
                        dalException = new DalUniqueConstraintException(ex.Message, ex);
                        break;
                    default:
                        // throw a general DAL Exception
                        dalException = new DalException(ex.Message, ex);
                        break;
                }
            }

            // return the error
            return dalException;
        }

        /// <summary>
        /// Trace the Execute and open the Connection when the state is not already open.
        /// </summary>
        protected void Prepare(string executeType) {
            _eventId++;
            if (_eventId > ushort.MaxValue) {
                _eventId = 0;
            }

            SqlStoredProcedure.TraceSource.TraceEvent(TraceEventType.Information, _eventId, "{0}: {1}", executeType, this.Name);

            TraceParameters(true);

            if (_command.Connection.State != ConnectionState.Open) {
                _command.Connection.Open();
                _connectionOpened = true;
            }
        }

        private void TraceParameters(bool input) {
            if (SqlStoredProcedure.TraceSource.Switch.ShouldTrace(TraceEventType.Verbose) && this.Parameters.Count > 0) {
                foreach (SqlParameter p in this.Parameters) {
                    bool isInput = p.Direction != ParameterDirection.ReturnValue && p.Direction != ParameterDirection.Output;
                    bool isOutput = p.Direction != ParameterDirection.Input;
                    if ((input && isInput) || (!input && isOutput)) {
                        SqlStoredProcedure.TraceSource.TraceEvent(TraceEventType.Verbose, _eventId, "SqlParamter: Name = {0}, Value = '{1}', Type = {2}, Size = {3}", p.ParameterName, p.Value, p.DbType, p.Size);
                    }
                }
            }
        }

        /// <summary>
        /// Close the Connection when the state is open.
        /// </summary>
        protected void CloseOpenedConnection() {
            if ((_command.Connection.State == ConnectionState.Open) & _connectionOpened)
                _command.Connection.Close();
        }

        /// <summary>
        /// Close the Connection when the state is open.
        /// </summary>
        protected void TraceResult(string result) {
            if (result != null) {
                SqlStoredProcedure.TraceSource.TraceEvent(TraceEventType.Verbose, _eventId, "Result: {0}", result);
            }

            TraceParameters(false);
        }

    }
}
