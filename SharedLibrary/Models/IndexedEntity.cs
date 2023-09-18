#if NET7_0
using System.ComponentModel.DataAnnotations;
#endif

namespace Commons.Models
{
    public abstract class IndexedEntity
    {
#if NET7_0
        [Key] public int Id { get; set; }
#else
    public int Id { get; set; }
#endif
    }
}