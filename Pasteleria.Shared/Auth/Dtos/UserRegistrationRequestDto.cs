namespace Pasteleria.Shared.Auth.Dtos
{
    public class UserRegistrationRequestDto
    {
        private string _firstName = string.Empty;
        private string _lastName = string.Empty;

        public string FirstName
        {
            get => _firstName;
            set
            {
                _firstName = value;
                UpdateFullName();
                UpdateUserName();
            }
        }

        public string LastName
        {
            get => _lastName;
            set
            {
                _lastName = value;
                UpdateFullName();
                UpdateUserName();
            }
        }

        public string Charge { get; set; } = string.Empty;
        public string Company { get; set; } = "CIC";
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;

        // Propiedades calculadas
        public string FullName { get; private set; } = string.Empty;
        public string UserName { get; private set; } = string.Empty;

        // Métodos para actualizar FullName y UserName
        private void UpdateFullName()
        {
            FullName = $"{_firstName} {_lastName}".Trim();
        }

        private void UpdateUserName()
        {
            if (!string.IsNullOrEmpty(_firstName))
            {
                UserName = $"{_firstName[0]}{_lastName}".ToLower();
            }
        }
    }
}
