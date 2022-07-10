namespace PR76_2018_Nikola_Jovicevic_WEB_PROJEKAT.Interfaces
{
    public interface IMail
    {
        void SendVerificationMail(string recieverEmailAddress, string purpouse);
    }
}
