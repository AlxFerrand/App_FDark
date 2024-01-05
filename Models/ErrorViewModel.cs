namespace App_FDark.Models
{
    public class ErrorViewModel
    {
        public string? RequestId { get; set; }
        public string? MessageError { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

    }
}