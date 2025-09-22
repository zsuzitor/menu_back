


namespace BO.Models.DAL
{
    public interface IDomainRecord
    {
    }

    public interface IDomainRecord<T1> : IDomainRecord
    {
        T1 Id { get; set; }
    }
}
