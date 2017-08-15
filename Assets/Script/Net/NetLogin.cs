
public class C2S_Login : KBEventData
{
    public string username;
    public string password;
    public byte[] datas;

    public C2S_Login(string _username, string _password, byte[] _datas)
    {
        username = _username;
        password = _password;
        datas = _datas;
    }
}
