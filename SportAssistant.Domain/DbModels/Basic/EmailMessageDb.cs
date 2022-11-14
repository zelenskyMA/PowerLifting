using SportAssistant.Domain.DbModels.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace SportAssistant.Domain.DbModels.Basic
{
    [Table("EmailMessages", Schema = "dbo")]
    public class EmailMessageDb : EntityDb
    {
        public string Subject { get; set; }

        public string Body { get; set; }
    }
}
