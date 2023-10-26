using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace BotDeTelegram
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            TelegramBotClient jonabot = new TelegramBotClient("6489047847:AAEzmsr0H6KWy91zlQFuRGOwmakMDWxE-WY");

            jonabot.StartReceiving(EnviarMensaje, ErrorMensaje);
            Console.WriteLine("Bot iniciado exitosamente y escuchando");
            Console.ReadKey();
        }

        public static async Task EnviarMensaje(ITelegramBotClient bot, Update update, CancellationToken token)
        {
            string responseMessage = (update.Message.Text == "buenos dias") ? "Buenos dias huamno" : "No te entendí";
            await bot.SendTextMessageAsync(update.Message.Chat.Id, responseMessage);
        }

        private static async Task ErrorMensaje(ITelegramBotClient bot, Exception exception, CancellationToken token)
        {
            Console.WriteLine("Hubo un error");
        }
    }
}