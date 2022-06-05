namespace aspnetapp.TokenProvider
{
    public interface ITokenProvider
    {
        AuthToken GenerateToken(SignInUser user);
    }
}
