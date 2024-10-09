using MassTransit.EntityFrameworkCoreIntegration;
using System.Text;

namespace MTDemo.JobConsumer
{
	public class OracleLockStatementProvider :
		SqlLockStatementProvider
	{
		public OracleLockStatementProvider(bool enableSchemaCaching = true)
			: base(new OracleLockStatementFormatter(), enableSchemaCaching)
		{
		}
	}

	public class OracleLockStatementFormatter :
		ILockStatementFormatter
	{
		public void Create(StringBuilder sb, string schema, string table)
		{
			sb.AppendFormat($"SELECT * FROM mt_job.\"{table}\"");
		}

		public void AppendColumn(StringBuilder sb, int index, string columnName)
		{
			//if (index == 0)
			//	sb.AppendFormat("\"{0}\" = @p0", columnName);
			//else
			//	sb.AppendFormat(" AND \"{0}\" = @p{1}", columnName, index);
		}

		public void Complete(StringBuilder sb)
		{
			
		}

		public void CreateOutboxStatement(StringBuilder sb, string schema, string table, string columnName)
		{
			sb.AppendFormat(@"SELECT * FROM `{0}` ORDER BY `{1}` LIMIT 1 FOR UPDATE SKIP LOCKED", table, columnName);
		}
	}
}
