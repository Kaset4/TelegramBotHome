using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types;
using Telegram.Bot;
using TelegramBotHome.Models;

namespace TelegramBotHome.Controllers
{
    public class InlineKeyboardController
    {
        private readonly IStorage _memoryStorage;
        private readonly ITelegramBotClient _telegramClient;

        public InlineKeyboardController(ITelegramBotClient telegramBotClient, IStorage memoryStorage)
        {
            _telegramClient = telegramBotClient;
            _memoryStorage = memoryStorage;
        }

        public async Task Handle(CallbackQuery? callbackQuery, CancellationToken ct)
        {
            if (callbackQuery?.Data == null)
                return;

            var session = _memoryStorage.GetSession(callbackQuery.From.Id);

            switch (callbackQuery.Data)
            {
                case "1. Подсчет символов":
                    session.IsCountingCharacters = true;
                    session.IsCalculatingSum = false;
                    await _telegramClient.SendTextMessageAsync(callbackQuery.From.Id,
                        "Вы выбрали подсчет символов. Пожалуйста, отправьте текст для подсчета символов.", cancellationToken: ct);
                    break;
                case "2. Сумма чисел":
                    session.IsCountingCharacters = false;
                    session.IsCalculatingSum = true; // Устанавливаем в true для выполнения суммирования чисел
                    await _telegramClient.SendTextMessageAsync(callbackQuery.From.Id,
                        "Вы выбрали сумму чисел. Пожалуйста, отправьте числа для вычисления суммы.", cancellationToken: ct);
                    break;
                default:
                    return;
            }

            // Добавляем вызов CalcMessageController после выбора функции "Сумма чисел"
            if (session.IsCalculatingSum)
            {
                var calcController = new CalcMessageController(_telegramClient, _memoryStorage);
                await calcController.Handle(new Message { Text = "" }, ct);
            }
        }
    }
}
