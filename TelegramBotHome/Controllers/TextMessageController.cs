using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Types;
using Telegram.Bot;
using TelegramBotHome.Models;
namespace TelegramBotHome.Controllers
{
    public class TextMessageController
    {
        private readonly ITelegramBotClient _telegramClient;
        private readonly IStorage _memoryStorage;

        public TextMessageController(ITelegramBotClient telegramBotClient, IStorage memoryStorage)
        {
            _telegramClient = telegramBotClient;
            _memoryStorage = memoryStorage;
        }

        public async Task Handle(Message message, CancellationToken ct)
        {
            var session = _memoryStorage.GetSession(message.Chat.Id);

            switch (message.Text)
            {
                case "/start":
                    await SendMainMenu(message.Chat.Id, ct);
                    break;
                default:
                    if (session.IsCountingCharacters)
                    {
                        int characterCount = message.Text.Length;
                        await _telegramClient.SendTextMessageAsync(message.Chat.Id, $"Количество символов в тексте: {characterCount}", cancellationToken: ct);
                        session.IsCountingCharacters = false; // Сбрасываем флаг после выполнения операции
                    }
                    else
                    {
                        await _telegramClient.SendTextMessageAsync(message.Chat.Id, "Отправьте текст или выберите другую функцию.", cancellationToken: ct);
                    }
                    break;
            }
        }

        public async Task SendMainMenu(long chatId, CancellationToken ct)
        {
            var buttons = new List<InlineKeyboardButton[]>
        {
            new[]
            {
                InlineKeyboardButton.WithCallbackData("Подсчет символов", "1. Подсчет символов"),
                InlineKeyboardButton.WithCallbackData("Сумма чисел", "2. Сумма чисел"),
            }
        };

            await _telegramClient.SendTextMessageAsync(chatId, "<b>Выберите функцию:</b>", cancellationToken: ct, parseMode: ParseMode.Html, replyMarkup: new InlineKeyboardMarkup(buttons));
        }
    }
}
