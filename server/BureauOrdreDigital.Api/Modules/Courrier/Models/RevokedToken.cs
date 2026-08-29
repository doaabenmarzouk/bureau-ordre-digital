public class RevokedToken
{
    public int Id { get; set; }
    public required string TokenId { get; set; } // le "jti" (JWT ID) du token
    public DateTime ExpirationDate { get; set; } // pour nettoyer plus tard
}