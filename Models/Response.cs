namespace Paintolog.Models;

public class ResetPasswordResponse
{
    public bool isSuccess { get; set; }
    public List<string> message { get; set; }
    public string errorCode { get; set; }
}
