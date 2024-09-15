namespace Kamino.Models;

public class ProfileActivityModelFactory(Uri endpoint) : ModelFactoryBase<Profile, ProfileActivityModel>(endpoint)
{
    public override ProfileActivityModel Create(Profile entity)
    {
        return new ProfileActivityModel()
        {
            Id = UriInternalizer.Externalize(entity.Uri),
            Type = "Person",

            Inbox = UriInternalizer.Externalize(entity.Uri) + "inbox",
            Name = entity.DisplayName,
            PreferredUsername = entity.Name,
            Summary = entity.Summary,
            Url = UriInternalizer.Externalize(entity.Url),
        };
    }

    public override Profile Parse(ProfileActivityModel model)
    {
        throw new NotImplementedException();
    }
}
