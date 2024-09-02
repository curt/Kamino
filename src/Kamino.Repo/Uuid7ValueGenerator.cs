using Medo;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace Kamino.Repo;

public class Uuid7ValueGenerator : ValueGenerator<Guid>
{
    public override Guid Next(EntityEntry entry)
    {
        var uuid = Uuid7.NewUuid7();
        return uuid.ToGuid();
    }

    public override bool GeneratesTemporaryValues => false;
}
