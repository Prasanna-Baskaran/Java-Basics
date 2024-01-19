using System;

namespace ReflectionIT.Common.Data.SqlClient
{
	/// <summary>
	/// Summary description for DalException.
	/// </summary>
	public class DalException : Exception
	{
		/// <summary>Initializes a new instance of the DalException class.</summary>
		public DalException() : base() {}

		/// <summary>
		/// Initializes a new instance of the DalException class with a specified error message.
		/// </summary>
		/// <param name="message">The error message string.</param>
		public DalException(string message) : base(message) {}

		/// <summary>
		/// Initializes a new instance of the DalException class with a specified error message and a reference to the inner exception that is the cause of this exception.
		/// </summary>
		/// <param name="message">The error message string.</param>
		/// <param name="innerException">The inner exception reference.</param>
		public DalException(string message, System.Exception innerException) : base(message,innerException) {}
	}
	
	/// <summary>
	/// Summary description for DalUniqueConstraintException.
	/// </summary>
	public class DalUniqueConstraintException : DalException
	{
		/// <summary>Initializes a new instance of the DalUniqueConstraintException class.</summary>
		public DalUniqueConstraintException() : base() {}

		/// <summary>
		/// Initializes a new instance of the DalUniqueConstraintException class with a specified error message.
		/// </summary>
		/// <param name="message">The error message string.</param>
		public DalUniqueConstraintException(string message) : base(message) {}

		/// <summary>
		/// Initializes a new instance of the DalUniqueConstraintException class with a specified error message and a reference to the inner exception that is the cause of this exception.
		/// </summary>
		/// <param name="message">The error message string.</param>
		/// <param name="innerException">The inner exception reference.</param>
		public DalUniqueConstraintException(string message, System.Exception innerException) : base(message,innerException) {}
	}
	
	/// <summary>
	/// Summary description for DalLoginException.
	/// </summary>
	public class DalLoginException : DalException
	{
		/// <summary>Initializes a new instance of the DalLoginException class.</summary>
		public DalLoginException() : base() {}

		/// <summary>
		/// Initializes a new instance of the DalLoginException class with a specified error message.
		/// </summary>
		/// <param name="message">The error message string.</param>
		public DalLoginException(string message) : base(message) {}

		/// <summary>
		/// Initializes a new instance of the DalLoginException class with a specified error message and a reference to the inner exception that is the cause of this exception.
		/// </summary>
		/// <param name="message">The error message string.</param>
		/// <param name="innerException">The inner exception reference.</param>
		public DalLoginException(string message, System.Exception innerException) : base(message,innerException) {}
	}

	/// <summary>
	/// Summary description for DalForeignKeyException.
	/// </summary>
	public class DalForeignKeyException : DalException
	{
		/// <summary>Initializes a new instance of the DalForeignKeyException class.</summary>
		public DalForeignKeyException() : base() {}

		/// <summary>
		/// Initializes a new instance of the DalForeignKeyException class with a specified error message.
		/// </summary>
		/// <param name="message">The error message string.</param>
		public DalForeignKeyException(string message) : base(message) {}

		/// <summary>
		/// Initializes a new instance of the DalForeignKeyException class with a specified error message and a reference to the inner exception that is the cause of this exception.
		/// </summary>
		/// <param name="message">The error message string.</param>
		/// <param name="innerException">The inner exception reference.</param>
		public DalForeignKeyException(string message, System.Exception innerException) : base(message,innerException) {}
	}

	/// <summary>
	/// Summary description for DalForeignKeyException.
	/// </summary>
	public class DalDeadLockException : DalException
	{
		/// <summary>Initializes a new instance of the DalDeadLockException class.</summary>
		public DalDeadLockException() : base() {}

		/// <summary>
		/// Initializes a new instance of the DalDeadLockException class with a specified error message.
		/// </summary>
		/// <param name="message">The error message string.</param>
		public DalDeadLockException(string message) : base(message) {}

		/// <summary>
		/// Initializes a new instance of the DalDeadLockException class with a specified error message and a reference to the inner exception that is the cause of this exception.
		/// </summary>
		/// <param name="message">The error message string.</param>
		/// <param name="innerException">The inner exception reference.</param>
		public DalDeadLockException(string message, System.Exception innerException) : base(message,innerException) {}
	}
}
