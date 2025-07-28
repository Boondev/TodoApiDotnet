namespace TodoApi.Models;

public class Config
{
    public JwtConfig JwtConfig { get; set; }
}

public class JwtConfig
{
    public string ValidIssuer { get; set; }
    public string ValidAudience { get; set; }
    public string Secret { get; set; }
}
