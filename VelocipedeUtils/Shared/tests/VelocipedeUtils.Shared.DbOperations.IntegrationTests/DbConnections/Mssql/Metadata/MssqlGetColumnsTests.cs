using VelocipedeUtils.Shared.DbOperations.DbConnections;
using VelocipedeUtils.Shared.DbOperations.IntegrationTests.Constants;
using VelocipedeUtils.Shared.DbOperations.IntegrationTests.DatabaseFixtures;
using VelocipedeUtils.Shared.DbOperations.IntegrationTests.DbConnections.Base.Metadata;
using VelocipedeUtils.Shared.DbOperations.Models;

namespace VelocipedeUtils.Shared.DbOperations.IntegrationTests.DbConnections.Mssql.Metadata;

/// <summary>
/// Class for testing <see cref="MssqlDbConnection.GetColumns(string, out List{VelocipedeColumnInfo})"/>.
/// </summary>
[Collection("MssqlDatabaseFixtureCollection")]
public sealed class MssqlGetColumnsTests : BaseGetColumnsTests
{
    /// <summary>
    /// Default constructor for creating <see cref="MssqlGetColumnsTests"/>.
    /// </summary>
    /// <param name="fixture">Database fixture.</param>
    public MssqlGetColumnsTests(MssqlDatabaseFixture fixture)
        : base(fixture, MssqlTestConstants.CREATE_DATABASE_SQL)
    {
    }

    public override void GetColumns_BlobAffinity()
    {
        throw new NotImplementedException();
    }

    public override void GetColumns_IntegerAffinity()
    {
        throw new NotImplementedException();
    }

    public override void GetColumns_NumericAffinity()
    {
        throw new NotImplementedException();
    }

    public override void GetColumns_RealAffinity()
    {
        throw new NotImplementedException();
    }

    public override void GetColumns_TextAffinity()
    {
        throw new NotImplementedException();
    }
}
