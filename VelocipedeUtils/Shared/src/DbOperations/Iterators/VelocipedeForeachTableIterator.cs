using VelocipedeUtils.Shared.DbOperations.DbConnections;
using VelocipedeUtils.Shared.DbOperations.Models;

namespace VelocipedeUtils.Shared.DbOperations.Iterators
{
    /// <summary>
    /// Iterator for the foreach operation.
    /// </summary>
    public class VelocipedeForeachTableIterator : IVelocipedeForeachTableIterator
    {
        public IVelocipedeIterator EndForeach()
        {
            throw new NotImplementedException();
        }

        public IVelocipedeForeachTableIterator GetAllDataFromTable()
        {
            throw new NotImplementedException();
        }

        public IVelocipedeForeachTableIterator GetColumns()
        {
            throw new NotImplementedException();
        }

        public IVelocipedeDbConnection GetForeachResult(out VelocipedeForeachResult foreachResult)
        {
            throw new NotImplementedException();
        }

        public IVelocipedeForeachTableIterator GetForeignKeys()
        {
            throw new NotImplementedException();
        }

        public IVelocipedeForeachTableIterator GetSqlDefinition()
        {
            throw new NotImplementedException();
        }

        public IVelocipedeForeachTableIterator GetTriggers()
        {
            throw new NotImplementedException();
        }
    }
}
