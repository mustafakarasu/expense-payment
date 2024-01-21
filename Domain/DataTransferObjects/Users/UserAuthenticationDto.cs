namespace Domain.DataTransferObjects.Users
{
    public record UserAuthenticationDto
    {
        public string Email { get; init; }
        public string Password { get; init; }
    }
}
