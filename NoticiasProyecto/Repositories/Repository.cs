using NoticiasProyecto.Models.Entities;

namespace NoticiasProyecto.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        public ItesrcneOctavoContext Context { get; set; }

        public Repository(ItesrcneOctavoContext context)
        {
            Context = context;
        }

        public virtual T? Get(object id)
        {
            return Context.Find<T>(id);
        }

        public virtual IEnumerable<T> GetAll()
        {
            return Context.Set<T>();
        }

        public virtual void Insert(T item)
        {
            Context.Add(item);
            Context.SaveChanges();
        }

        public virtual void Update(T item)
        {
            Context.Update(item);
            Context.SaveChanges();
        }

        public virtual void Delete(T item)
        {
            Context.Remove(item);
            Context.SaveChanges();
        }

    }
}
