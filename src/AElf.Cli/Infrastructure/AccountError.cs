namespace AElf.Cli.Infrastructure
{
    public enum AccountError
    {
        None = 0,
        AccountAlreadyUnlocked = 1,
        WrongPassword = 2,
        AccountFileNotFound = 3
    }
}