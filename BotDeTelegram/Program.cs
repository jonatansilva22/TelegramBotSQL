using System;
using System.Data.SqlClient;
using Telegram.Bot;
using Telegram.Bot.Types;
using System.Threading;

namespace BotDeTelegram
{
    internal class Program
    {
        private static string connectionString = "Data Source=LAPTOP-AEVKD7EC;Initial Catalog=Danymar;User ID=sa;Password=messi22;";
        private static TelegramBotClient jonabot;

        private static void Main(string[] args)
        {
            jonabot = new TelegramBotClient("6489047847:AAEzmsr0H6KWy91zlQFuRGOwmakMDWxE-WY"); // Reemplaza "TU_TOKEN" con tu token real.

            Console.WriteLine("Bot iniciado exitosamente. Presiona Enter para salir.");

            int lastUpdateId = 0;

            while (true)
            {
                var updates = jonabot.GetUpdatesAsync(offset: lastUpdateId).Result;

                foreach (var update in updates)
                {
                    lastUpdateId = update.Id + 1;

                    if (update.Message == null || string.IsNullOrWhiteSpace(update.Message.Text))
                    {
                        continue;
                    }

                    string command = update.Message.Text;

                    if (command.StartsWith("/consultar jonatansilva"))
                    {
                        string username = command.Replace("/consultar", "").Trim();
                        string response = ConsultarUsuario(username);
                        SendMessage(update.Message.Chat.Id, response);
                    }
                    else if (command.StartsWith("/eliminar jonatansilva"))
                    {
                        string username = command.Replace("/eliminar", "").Trim();
                        string response = EliminarUsuario(username);
                        SendMessage(update.Message.Chat.Id, response);
                    }
                }

                Thread.Sleep(1000); // Espera 1 segundo antes de revisar actualizaciones nuevamente.
            }
        }

        private static string ConsultarUsuario(string username)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT idCliente, nombreCliente, apellidoCliente, correoCliente, telefonoCliente FROM Cliente WHERE idCliente = 1";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Username", username);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string idCliente = reader["idCliente"].ToString();
                            string nombreCliente = reader["nombreCliente"].ToString();
                            string apellidoCliente = reader["apellidoCliente"].ToString();
                            string correoCliente = reader["correoCliente"].ToString();
                            string telefonoCliente = reader["telefonoCliente"].ToString();
                            return $"ID: {idCliente}\nNombre: {nombreCliente}\nApellido: {apellidoCliente}\nCorreo: {correoCliente}\nTeléfono: {telefonoCliente}";
                        }
                        else
                        {
                            return "Usuario no encontrado en la base de datos.";
                        }
                    }
                }
            }
        }

        private static string EliminarUsuario(string username)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "DELETE FROM Cliente WHERE idCliente = 1";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Username", username);
                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        return "Usuario eliminado correctamente.";
                    }
                    else
                    {
                        return "Usuario no encontrado en la base de datos.";
                    }
                }
            }
        }

        private static async void SendMessage(long chatId, string message)
        {
            await jonabot.SendTextMessageAsync(chatId, message);
        }
    }
}