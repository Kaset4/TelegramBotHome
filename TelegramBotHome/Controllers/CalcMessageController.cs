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
    public class CalcMessageController
    {
        private readonly ITelegramBotClient _telegramClient;
        private readonly IStorage _memoryStorage;

        public CalcMessageController(ITelegramBotClient telegramBotClient, IStorage memoryStorage)
        {
            _telegramClient = telegramBotClient;
            _memoryStorage = memoryStorage;
        }

        public async Task Handle(Message message, CancellationToken ct)
        {
            var session = _memoryStorage.GetSession(message.Chat.Id);

            if (session.IsCalculatingSum)
            {
                var numbers = message.Text.Split(' ', StringSplitOptions.RemoveEmptyEntries);

                int sum = 0;
                foreach (var num in numbers)
                {
                    if (int.TryParse(num, out int parsedNum))
                    {
                        sum += parsedNum;
                    }
                    else
                    {
                        await _telegramClient.SendTextMessageAsync(message.Chat.Id, "Введите только числа.", cancellationToken: ct);
                        return;
                    }
                }

                await _telegramClient.SendTextMessageAsync(message.Chat.Id, $"Сумма чисел: {sum}", cancellationToken: ct);
                session.IsCalculatingSum = false;
            }
            else
            {
                await _telegramClient.SendTextMessageAsync(message.Chat.Id, "Отправьте числа или выберите другую функцию.", cancellationToken: ct);
            }
        }
    }
}
