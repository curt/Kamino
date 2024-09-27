namespace Kamino.Models;

public class ProfileActivityModelFactory(Uri endpoint)
    : ModelFactoryBase<Profile, ProfileActivityModel>(endpoint)
{
    public override ProfileActivityModel Create(Profile entity)
    {
        return new ProfileActivityModel()
        {
            Id = UriInternalizer.Externalize(entity.Uri),
            Type = "Person",
            Inbox = UriInternalizer.Externalize(entity.Uri) + "inbox",
            Outbox = UriInternalizer.Externalize(entity.Uri) + "outbox",
            Followers = UriInternalizer.Externalize(entity.Uri) + "followers",
            Following = UriInternalizer.Externalize(entity.Uri) + "following",
            Name = entity.DisplayName,
            PreferredUsername = entity.Name,
            Summary = entity.Summary,
            Url = UriInternalizer.Externalize(entity.Url),
            PublicKey = new
            {
                Id = UriInternalizer.Externalize(entity.PublicKeyId),
                Owner = UriInternalizer.Externalize(entity.Uri),
                PublicKeyPem = entity.PublicKey,
            },
        };
    }

    public override Profile Parse(ProfileActivityModel model)
    {
        throw new NotImplementedException();
    }
}
