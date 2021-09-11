public class Register
{
    internal string username { get; set; }
    internal string password { get; set; }

    public Register(string username, string password)
    {
        this.username = username;
        this.password = password;
    }
}