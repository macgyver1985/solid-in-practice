namespace Solid.SingleResponsibilityPrinciple.Correct
{
    public interface INewsletterRepository
    {
        bool Register(string email);
    }
}