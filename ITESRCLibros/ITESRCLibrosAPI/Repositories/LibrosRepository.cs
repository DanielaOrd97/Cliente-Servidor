using ITESRCLibrosAPI.Models.Entities;

namespace ITESRCLibrosAPI.Repositories
{
    
    public class LibrosRepository
    {
        public LibrosRepository(ItesrcneLibrosContext context)
        {
            Context = context;
        }

        public ItesrcneLibrosContext Context { get; }

        public IEnumerable<Libros> GetAll()
        {
            return Context.Libros.OrderBy(x => x.Titulo);
        }

        public Libros? Get(int id)
        {
            return Context.Libros.Find(id);
        }

        public void Insert(Libros libros)
        {
            Context.Libros.Add(libros);
            Context.SaveChanges();
        }

        public void Update(Libros libros)
        {
            Context.Libros.Update(libros);
            Context.SaveChanges();
        }

        public void Delete(Libros libros)
        {
            Context.Libros.Remove(libros);
            Context.SaveChanges();
        }
    }
}
