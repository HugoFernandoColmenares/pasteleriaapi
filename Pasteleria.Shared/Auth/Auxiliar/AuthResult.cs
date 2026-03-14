namespace Pasteleria.Shared.Auth.Auxiliar
{
    public class AuthResult
    {
        public string Token { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public bool Result { get; set; }
        public List<string> Errors { get; set; } = new List<string>();
    }
}
