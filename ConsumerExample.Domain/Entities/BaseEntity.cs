using System.ComponentModel.DataAnnotations.Schema;

namespace ConsumerExample.Domain.Entities
{
    public abstract class BaseEntity
    {
        [NotMapped]
        private List<string> Errors { get; set; } = new List<string>();
        public void AddError(string error)
        {
            Errors.Add(error);
        }  

        public string GetErrorsAsString()
        {
            return string.Join("; ", Errors);
        }

        public bool HasErrors => Errors.Any();
    }
}
