namespace Studently.UserCredential.Service;

public interface IPasswordHashingStrategy
{
    string Hash(string password);

    bool Verify(string password, string hash);

    string AlgorithmName { get; }
}
