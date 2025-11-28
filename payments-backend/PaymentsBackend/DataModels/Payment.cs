using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace PaymentsBackend.DataModels
{
    public class Payment
    {
        [Key]
        public int Id { get; set; } 

        public string UserId { get; set; } = string.Empty;
        public string? Reference { get; set; } = default!;
        
        public string ClientRequestId { get; set; } = Guid.NewGuid().ToString(); 
        public decimal Amount { get; set; }
        public string Currency { get; set; } = default!; // USD, EUR, INR, GBP

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }
    }
    public class ApiResponse
    {
        [JsonProperty("response")]
        public dynamic Response { get; set; }

        public ApiResponse(dynamic response)
        {
            Response = response;
        }
    }

    public class PaymentLog
    {
        public int Id { get; set; }
        public string UserId { get; set; } = string.Empty;
        public string Action { get; set; } = string.Empty;        
        public string RequestJson { get; set; } = string.Empty;   // incoming JSON
        public string ResponseJson { get; set; } = string.Empty;  // outgoing JSON
        public bool IsSuccess { get; set; }                       // true/false

        public string ErrorMessage { get; set; } = string.Empty;  // if failed
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }

    public class PaymentRequestDto
    {

        public string UserId { get; set; } = string.Empty;       

        public decimal Amount { get; set; }
        public string Currency { get; set; } = default!; // USD, EUR, INR, GBP
        public int id { get; set; } // send default 0 for Insert
       
    }
    public class UserDto
    {
        public List<string> Name { get; set; }
    }

}
