namespace bankingapp.API.Entities
{
    // Her HTTP istegi icin tek bir satir tutulur.
    // Hata olusmazsa Exception* alanlari null kalir.
    public class RequestLog
    {
        public int Id { get; set; }

        public string Method { get; set; } = string.Empty;
        public string Path { get; set; } = string.Empty;
        public string? RequestBody { get; set; }

        public int StatusCode { get; set; }
        public string? ResponseBody { get; set; }
        public long SureMs { get; set; }

        public string? ExceptionType { get; set; }
        public string? ExceptionMessage { get; set; }
        public string? StackTrace { get; set; }

        public DateTime OlusturulmaTarihi { get; set; } = DateTime.UtcNow;
    }
}
