using System.ComponentModel.DataAnnotations;

namespace Commons.Models;

public abstract class IndexedEntity
{
    [Key] public int Id { get; set; }
}