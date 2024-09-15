namespace Kamino.Models;

public class ProfileWebfingerModelFactory(Uri endpoint) : ModelFactoryBase<Profile, ProfileWebfingerModel>(endpoint)
{
    public override ProfileWebfingerModel Create(Profile entity)
    {
        var profileUri = UriInternalizer.Externalize(entity.Uri)!;

        return new ProfileWebfingerModel()
        {
            Aliases = [profileUri],
            Links = [new LinkWebfingerModel() { Href = profileUri, Rel = "self", Type = "application/activity+json" }],
            Subject = $"acct:{entity.Name}@{UriInternalizer.ExternalHost}",
        };
    }

    public override Profile Parse(ProfileWebfingerModel model)
    {
        throw new NotImplementedException();
    }
}
