namespace Database
{
    public interface IDbContextFactory
    {
        TestApiDb Create();
    }

    public class DbContextFactory : IDbContextFactory
    {
        public TestApiDb Create()
        {
            return new TestApiDb();
        }
    }
}
