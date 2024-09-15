namespace Kamino.Models;

public abstract class ModelFactoryBase<TEntity, TModel>(Uri endpoint)
{
    private readonly UriInternalizer _internalizer = new(endpoint);

    public UriInternalizer UriInternalizer { get { return _internalizer; } }

    public abstract TModel Create(TEntity entity);

    public abstract TEntity Parse(TModel model);
}
