namespace Solid.SingleResponsibilityPrinciple.Correct
{
    public interface ISMTPService
    {
        void SendMail(string from, string to, string subject, string body);
    }
}