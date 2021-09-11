using Npgsql;
using System.Data;
using System.Text;
using System.IO;
using System.Security.Cryptography;
using System;
using System.Threading;
using System.Threading.Tasks;


public class Registration
{

    public string encodePassword(string password)
    {
        string pass = null;
        using (var provider = System.Security.Cryptography.MD5.Create())
        {
            StringBuilder builder = new StringBuilder();

            foreach (byte b in provider.ComputeHash(Encoding.UTF8.GetBytes(password)))
                builder.Append(b.ToString("x2").ToLower());
            using (var sha256 = new SHA256Managed())
            {
                pass = BitConverter.ToString(sha256.ComputeHash(Encoding.UTF8.GetBytes(builder.ToString()))).Replace("-", "");
            }
        }

        return pass;
    }

    public async Task<string> register(Register register)
    {
        string status;
        var connString = "Host=localhost;Username=postgres;Password=1781490cvb;Database=postgres";
        await using var conn = new NpgsqlConnection(connString);
        await conn.OpenAsync();
        using (var cmd = new NpgsqlCommand("function.registration", conn))
        {
            try
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("_username", register.username);
                cmd.Parameters.AddWithValue("_password", register.password);
                using (var reader = cmd.ExecuteReader())
                {
                    status = "OK";
                }
            }
            catch (Exception e)
            {
                status = "NO " + e.Message;
            }
        }

        return status;
    }
}