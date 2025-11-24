using VelocipedeUtils.Shared.DbOperations.DbConnections;
using VelocipedeUtils.Shared.DbOperations.IntegrationTests.Constants;
using VelocipedeUtils.Shared.DbOperations.IntegrationTests.DatabaseFixtures;
using VelocipedeUtils.Shared.DbOperations.IntegrationTests.DbConnections.Base.Metadata;
using VelocipedeUtils.Shared.DbOperations.Models;

namespace VelocipedeUtils.Shared.DbOperations.IntegrationTests.DbConnections.Postgres.Metadata;

/// <summary>
/// Class for testing <see cref="PgDbConnection.GetColumns(string, out List{VelocipedeColumnInfo})"/>.
/// </summary>
public sealed class PgGetColumnsTests : BaseGetColumnsTests, IClassFixture<PgDatabaseFixture>
{
    /// <summary>
    /// Default constructor for creating <see cref="PgGetColumnsTests"/>.
    /// </summary>
    /// <param name="fixture">Database fixture.</param>
    public PgGetColumnsTests(PgDatabaseFixture fixture)
        : base(fixture, PgTestConstants.CREATE_DATABASE_SQL)
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
