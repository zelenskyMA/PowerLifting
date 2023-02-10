using System.Collections;

namespace SportAssistant.Domain.CustomExceptions
{
    public class CustomError
    {
        public string? Message { get; set; }

        public string? Detail { get; set; }

        public IDictionary? ExtData { get; set; }
    }
}
