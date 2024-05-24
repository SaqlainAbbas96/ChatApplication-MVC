namespace ChatApplication.Services
{
    public interface IJwtAuthenticationService
    {
        string GenerateToken(int userId, string username, string userrole);
    }
}
